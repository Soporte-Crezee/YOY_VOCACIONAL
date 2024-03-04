using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using POV.Reactivos.BO;
using POV.Reactivos.Service;
using POV.ReactivosUsuario.BO;
using POV.ReactivosUsuario.Service;
using POV.Reactivos.DA;
using GP.SocialEngine.Service;
using GP.SocialEngine.BO;
using POV.Core.RedSocial.Implement;
using POV.Core.RedSocial.Interfaces;
using Framework.Base.DataAccess;
using POV.Modelo.Estandarizado.BO;
using POV.Modelo.Estandarizado.Service;

namespace POV.Web.DTO.Services
{
    public class ReactivoDTOCtrl
    {
        readonly IDataContext dctx = new DataContext((new DataProviderFactory()).GetDataProvider("POV"));

        private ReactivoCtrl reactivoCtrl;
        private AreaAplicacionCtrl areaAplicacionCtrl;
        private IUserSession userSession;

        public ReactivoDTOCtrl()
        {
            reactivoCtrl = new ReactivoCtrl();
            userSession = new UserSession();
            areaAplicacionCtrl = new AreaAplicacionCtrl();
        }

        public reactivodto SaveReactivo(reactivodto dto)
        {
            string sError = ValidateReactivo(dto);
            if (sError.Length < 1)
            {
                Reactivo reactivo = DtoToReactivo(dto);
                PreguntaDTOCtrl preguntaDTOCtrl = new PreguntaDTOCtrl();

                object myFirm = new object();
                dctx.OpenConnection(myFirm);
                
                try
                {
                    dctx.BeginTransaction(myFirm);
                    if (reactivo.ReactivoID == null)
                    {
                        reactivo.ReactivoID = Guid.NewGuid();
                        reactivo.Activo = true;
                        reactivo.Preguntas.ForEach(item => item.Activo = true);
                        reactivoCtrl.InsertComplete(dctx, reactivo);

                        reactivo = reactivoCtrl.RetrieveComplete(dctx, reactivo);
                        SocialHubCtrl socialhubCtrl = new SocialHubCtrl();
                        socialhubCtrl.InsertSocialHubReactivo(dctx, new SocialHub { SocialProfile = reactivo, SocialProfileType = ESocialProfileType.REACTIVO, FechaRegistro = DateTime.Now, Alias = reactivo.NombreReactivo });
                    
                    }
                    else
                    {
                        reactivoCtrl.UpdateComplete(dctx, reactivo);

                        if (dto.preguntasdeleted != null)
                            foreach (preguntadto preguntadto in dto.preguntasdeleted)
                            {
                                preguntaDTOCtrl.DeletePregunta(preguntadto);
                            }
                        if (dto.opcionesdeleted != null)
                            foreach (opcionrespuestadto opcion in dto.opcionesdeleted)
                            {
                                preguntaDTOCtrl.DeleteOpcionPregunta(opcion);
                            }
                    }
                    dto = ReactivoToDto(reactivo, true);
                    dto.success = true;
                    dctx.CommitTransaction(myFirm);
                }
                catch (Exception ex)
                {
                    POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                    dctx.RollbackTransaction(myFirm);
                    dctx.CloseConnection(myFirm);
                    dto = new reactivodto();
                    dto.success = false;
                    dto.errors = new List<string>();
                    dto.errors.Add(ex.Message);
                }
                finally
                {
                    if (dctx.ConnectionState == ConnectionState.Open)
                        dctx.CloseConnection(myFirm);
                }

            }
            else
            {
                dto.success = false;
                dto.errors = new List<string>();
                dto.errors.Add(sError);
            }

            return dto;
        }

        public reactivodto GetReactivo(reactivodto dto)
        {
            try
            {
                if (!string.IsNullOrEmpty(dto.reactivoid))
                {
                    
                    Reactivo reactivo = new Reactivo { ReactivoID = Guid.Parse(dto.reactivoid), TipoReactivo = ETipoReactivo.Estandarizado };
                    if (dto.tipodocente != null && dto.tipodocente.Value)
                        reactivo = new ReactivoDocente { ReactivoID = Guid.Parse(dto.reactivoid), TipoReactivo = ETipoReactivo.Estandarizado, Docente = new CentroEducativo.BO.Docente { DocenteID = dto.docenteid } };
                    
                    reactivo = reactivoCtrl.RetrieveComplete(dctx, reactivo);

                    dto = ReactivoToDto(reactivo, true);
                }

                return dto;
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
        }

        private string ValidateReactivo(reactivodto dto){
            string sError = string.Empty;

            List<preguntadto> preguntasDto = dto.preguntas;
            foreach (preguntadto preguntadto in preguntasDto)
            {
                if ((ETipoRespuestaPlantilla)preguntadto.tipoplantilla == ETipoRespuestaPlantilla.OPCION_MULTIPLE)
                {
                    int unChecks = preguntadto.opciones.Count(item => !(bool)item.check);

                    if (unChecks == preguntadto.opciones.Count)
                    {
                        sError = "La pregunta '" + preguntadto.textopregunta + "' debe tener una opción correcta seleccionada.";
                    }
                }
            }
            
            return sError;
        }

        /// <summary>
        /// Realiza una busqueda en la base de datos segun los parametros establecidos de entrada
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public reactivooutputdto SearchReactivos(reactivoinputdto dto)
        {
            try
            {
                reactivooutputdto outputDTO = new reactivooutputdto();
                List<reactivodto> reactivos = new List<reactivodto>();
                dto.sort = "NombreReactivo";
                dto.order = "ASC";

                ReactivoDARetHlp da = new ReactivoDARetHlp();

                DataSet ds = da.Action(dctx, dto.pagesize, dto.currentpage, dto.sort, dto.order, dtoInputToParemeters(dto));

                foreach (var reactivo in DataSetToListReactivo(ds))
                {
                    var reactivoComplete = reactivoCtrl.RetrieveComplete(dctx, reactivo);


                    reactivodto reDTO = ReactivoToDto(reactivoComplete, false);

                    if (!userSession.IsAlumno()) // si es docente no sera necesario que se suscriba
                        reDTO.issuscrito = true;
                    else
                    {
                        AppSuscripcionCtrl appSuscripcionCtrl = new AppSuscripcionCtrl();

                        // validamos que  exista la suscripcion.

                        DataSet appsDs = appSuscripcionCtrl.Retrieve(dctx, userSession.SocialHub, new AppSuscripcion { AppType = EAppType.REACTIVO, AppSocial = reactivo });

                        if (appsDs.Tables[0].Rows.Count < 1)
                            reDTO.issuscrito = false;
                        else
                            reDTO.issuscrito = true;
                    }

                    reactivos.Add(reDTO);
                }
                outputDTO.reactivos = reactivos;
                return outputDTO;
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Realiza la suscripcion de un reactivo
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public reactivodto SuscribirReactivo(reactivodto dto)
        {
            reactivodto output = new reactivodto();
            output.errors = new List<string>();
            object myFirm = new object();
            try
            {
                dctx.OpenConnection(myFirm);
                dctx.BeginTransaction(myFirm);
                Reactivo reactivo = new Reactivo { ReactivoID = Guid.Parse(dto.reactivoid) ,TipoReactivo = ETipoReactivo.Estandarizado};

                DataSet ds = reactivoCtrl.Retrieve(dctx, reactivo);
                if (ds.Tables["Reactivo"].Rows.Count > 0)
                {
                    SocialHub socialHub = userSession.SocialHub;
                    reactivo = reactivoCtrl.LastDataRowToReactivo(ds, ETipoReactivo.Estandarizado);
                    reactivo.TipoReactivo = ETipoReactivo.Estandarizado;

                    AppSuscripcionCtrl appSuscripcionCtrl = new AppSuscripcionCtrl();

                    // validamos que no exista la suscribcion.

                    DataSet appsDs = appSuscripcionCtrl.Retrieve(dctx, socialHub, new AppSuscripcion { AppType = EAppType.REACTIVO, AppSocial = reactivo });

                    if (appsDs.Tables["AppSuscripcion"].Rows.Count < 1)
                    {
                        //No existe la suscripcion por lo tanto se crea

                        AppSuscripcion appSuscripcion = new AppSuscripcion();
                        appSuscripcion.AppSocial = reactivo;
                        appSuscripcion.AppType = EAppType.REACTIVO;
                        appSuscripcion.Estatus = true;
                        appSuscripcion.FechaRegistro = DateTime.Now;

                        appSuscripcionCtrl.Insert(dctx, socialHub, appSuscripcion);

                        SavePublicacionSuscripcion(reactivo);
                        output = ReactivoToDto(reactivoCtrl.RetrieveComplete(dctx, reactivo),false);
                        output.success = true;

                    }
                    else
                    {
                        //Ya existe pero esta desactivada - se activa
                        AppSuscripcion appSuscripcion = appSuscripcionCtrl.LastDataRowToAppSuscripcion(appsDs);

                        if (appSuscripcion.Estatus == false)
                        {
                            appSuscripcion.Estatus = true;
                            appSuscripcionCtrl.Update(dctx, appSuscripcion, appSuscripcion);
                            output.success = true;
                            output.reactivoid = reactivo.ReactivoID.ToString();

                        }
                        else
                        {
                            output.success = false;
                            output.errors.Add("Ya estás suscrito al ejercicio de práctica.");
                        }

                    }
                }
                else
                {
                    output.success = false;
                    output.errors.Add("No existe el ejercicio de práctica.");
                }
                dctx.CommitTransaction(myFirm);
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                dctx.RollbackTransaction(myFirm);
                dctx.CloseConnection(myFirm);
                output.success = false;
                output.errors.Add("Error :" + ex.Message);
            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(myFirm);
            }
            return output;
        }

        public void SavePublicacionSuscripcion(Reactivo reactivo)
        {
            try
            {
                if (userSession.IsAlumno())
                {
                    Publicacion publicacion = new Publicacion();

                    publicacion.PublicacionID = Guid.NewGuid();
                    publicacion.Contenido = "Se suscribió al ejercicio de práctica";
                    publicacion.Ranking = new Ranking { RankingID = Guid.NewGuid() };
                    publicacion.FechaPublicacion = System.DateTime.Now;
                    publicacion.Estatus = true;
                    publicacion.SocialHub = userSession.SocialHub;
                    publicacion.UsuarioSocial = userSession.CurrentUsuarioSocial;
                    publicacion.TipoPublicacion = ETipoPublicacion.REACTIVO;
                    publicacion.AppSocial = reactivo;

                    List<IPrivacidad> privacidades = new List<IPrivacidad>();
                    privacidades.Add(userSession.CurrentGrupoSocial);
                    publicacion.Privacidades = privacidades;

                    RankingCtrl rankingCtrl = new RankingCtrl();
                    rankingCtrl.Insert(dctx, publicacion.Ranking);
                    PublicacionCtrl publicacionCtrl = new PublicacionCtrl();
                    publicacionCtrl.Insert(dctx, publicacion);
                }
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
        }

        public reactivodto QuitarSuscripcion(reactivodto dto)
        {
            reactivodto output = new reactivodto();
            output.errors = new List<string>();

            try
            {
                Reactivo reactivo = new Reactivo { ReactivoID = Guid.Parse(dto.reactivoid), TipoReactivo = ETipoReactivo.Estandarizado };

                DataSet ds = reactivoCtrl.Retrieve(dctx, reactivo);
                if (ds.Tables["Reactivo"].Rows.Count > 0)
                {
                     SocialHub socialHub = userSession.SocialHub;
                    reactivo = reactivoCtrl.LastDataRowToReactivo(ds, ETipoReactivo.Estandarizado);

                    AppSuscripcionCtrl appSuscripcionCtrl = new AppSuscripcionCtrl();

                    // validamos que exista la suscribcion.

                    DataSet appsDs = appSuscripcionCtrl.Retrieve(dctx, socialHub, new AppSuscripcion { AppType = EAppType.REACTIVO, AppSocial = reactivo });

                    if (appsDs.Tables["AppSuscripcion"].Rows.Count > 0)
                    {
                        AppSuscripcion appSuscripcion = appSuscripcionCtrl.LastDataRowToAppSuscripcion(appsDs);
                        appSuscripcion.Estatus = false;
                        appSuscripcionCtrl.Update(dctx, appSuscripcion, appSuscripcion);
                        output.success = true;
                        output.issuscrito = false;
                        output.reactivoid = reactivo.ReactivoID.ToString();
                    }
                    else
                    {
                        output.success = false;
                        output.errors.Add("No estás suscrito");
                    }
                }
                else
                {
                    output.success = false;
                    output.errors.Add("No existe el ejercicio de práctica.");
                }
            }
            catch (FormatException fex)
            {
                output.success = false;
                output.errors.Add("Error inesperado.");
            }
            return output;
        }

        /// <summary>
        /// Obtiene los reactivos suscritos por el usuario.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public List<reactivodto> GetReactivosUsuario(suscripcionreactivodto dto)
        {
            try
            {
                List<reactivodto> output = new List<reactivodto>();

                SocialHub socialHub = new SocialHub { SocialHubID = dto.socialhubid };
                SocialHub mySocialHub = userSession.SocialHub;

                //es mi social hub que visito?
                bool isSameHub = socialHub.SocialHubID == mySocialHub.SocialHubID;

                AppSuscripcionCtrl appSuscripcionCtrl = new AppSuscripcionCtrl();

                //Se consulta los reactivos del social hub que se visita.
                DataSet reactivosDs = appSuscripcionCtrl.Retrieve(dctx, socialHub, new AppSuscripcion { AppType = EAppType.REACTIVO, AppSocial = new Reactivo(), Estatus = true });

                //si no es un visitante consultar reactivos del visitante
                DataSet reactivosVisitanteDs = null;
                if (!isSameHub)
                    reactivosVisitanteDs = appSuscripcionCtrl.Retrieve(dctx, mySocialHub, new AppSuscripcion { AppType = EAppType.REACTIVO, AppSocial = new Reactivo(), Estatus = true });


                foreach (DataRow reactivoDr in reactivosDs.Tables["AppSuscripcion"].Rows)
                {
                    AppSuscripcion app = appSuscripcionCtrl.DataRowToAppSuscripcion(reactivoDr);
                    Reactivo reactivo = reactivoCtrl.RetrieveComplete(dctx, new Reactivo { ReactivoID = Guid.Parse(app.AppSocial.GetAppKey()) });
                    reactivodto reactivodto = ReactivoToDto(reactivo, false);

                    if (!isSameHub)
                    {
                        foreach (DataRow reactivoVisitanteDr in reactivosVisitanteDs.Tables["AppSuscripcion"].Rows)
                        {
                            AppSuscripcion appVisitante = appSuscripcionCtrl.DataRowToAppSuscripcion(reactivoVisitanteDr);

                            if (reactivo.ReactivoID == Guid.Parse(appVisitante.AppSocial.GetAppKey()))
                            {
                                reactivodto.issuscrito = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        reactivodto.issuscrito = true;
                    }
                    output.Add(reactivodto);

                }


                return output;
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
        }

        private Dictionary<string, string> dtoInputToParemeters(reactivoinputdto dto)
        {
            Dictionary<string, string> parametros = new Dictionary<string, string>();
            if (dto.nombrereactivo.Trim().Length > 0)
                parametros.Add("NombreReactivo", "%"+dto.nombrereactivo+"%");
            if (dto.areaaplicacionid > 0)
                parametros.Add("AreaAplicacionID", dto.areaaplicacionid.ToString());

            parametros.Add("SocialHubID", userSession.SocialHub.SocialHubID.ToString());
            parametros.Add("AppType", ((short)EAppType.REACTIVO).ToString());
            return parametros;
        }

        public reactivodto ReactivoToDto(Reactivo reactivo, bool complete)
        {
            reactivodto dto = new reactivodto();
            dto.issuscrito = false;
            dto.reactivoid = reactivo.ReactivoID.ToString();
            dto.nombrereactivo = reactivo.NombreReactivo;
            dto.descripcion = reactivo.Descripcion;

            if (!string.IsNullOrEmpty(reactivo.PlantillaReactivo))
                dto.plantilla = reactivo.PlantillaReactivo;

            if (complete)
            {
                dto.preguntas = new List<preguntadto>();

                PreguntaDTOCtrl preguntaDTOCtrl = new PreguntaDTOCtrl();
                if (reactivo.Preguntas != null && reactivo.Preguntas.Count > 0)
                {
                    foreach (Pregunta pregunta in reactivo.Preguntas)
                    {
                        dto.preguntas.Add(preguntaDTOCtrl.PreguntaToDTO(pregunta));
                    }
                }
            }
            return dto;
        }

        public Reactivo DtoToReactivo(reactivodto dto)
        {
            Reactivo reactivo = new Reactivo();
            if (dto.tipodocente != null && dto.tipodocente.Value)
            {
                reactivo = new ReactivoDocente();
                (reactivo as ReactivoDocente).Docente = new CentroEducativo.BO.Docente { DocenteID = dto.docenteid };
            }
            reactivo.TipoReactivo = ETipoReactivo.Estandarizado;
            reactivo.Caracteristicas = null;
            reactivo.PresentacionPlantilla = (EPresentacionPlantilla) dto.presentacionplantilla;
            PreguntaDTOCtrl preguntaDTOCtrl = new PreguntaDTOCtrl();
            if (!string.IsNullOrEmpty(dto.reactivoid))
                reactivo.ReactivoID = new Guid(dto.reactivoid);
            if (!string.IsNullOrEmpty(dto.nombrereactivo))
                reactivo.NombreReactivo = dto.nombrereactivo;
            if (!string.IsNullOrEmpty(dto.plantilla))
                reactivo.PlantillaReactivo = dto.plantilla;
            if (!string.IsNullOrEmpty(dto.descripcion))
                reactivo.Descripcion = dto.descripcion;

            if (dto.preguntas != null && dto.preguntas.Count > 0)
            {
                List<Pregunta> preguntas = new List<Pregunta>();
                foreach (preguntadto preguntadto in dto.preguntas)
                {
                    Pregunta pregunta = preguntaDTOCtrl.DTOToPregunta(preguntadto);
                    preguntas.Add(pregunta);
                }

                reactivo.Preguntas = preguntas;
            }

            return reactivo;
        }

        private List<Reactivo> DataSetToListReactivo(DataSet ds)
        {
            List<Reactivo> reactivos = new List<Reactivo>();
            ReactivoCtrl reactivoCtrl = new ReactivoCtrl();
            foreach (DataRow row in ds.Tables["Reactivo"].Rows)
            {
                Reactivo reactivo = reactivoCtrl.DataRowToReactivo(row, ETipoReactivo.Estandarizado);

                reactivo.TipoReactivo = ETipoReactivo.Estandarizado;
                reactivos.Add(reactivo);
            }
            return reactivos;
        }
    }
}
