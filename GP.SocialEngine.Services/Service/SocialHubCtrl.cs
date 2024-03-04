using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;
using GP.SocialEngine.Interfaces;
using GP.SocialEngine.DAO;
using POV.CentroEducativo.BO;

namespace GP.SocialEngine.Service
{
    /// <summary>
    /// Controlador del objeto SocialHub
    /// </summary>
    public class SocialHubCtrl
    {
        /// <summary>
        /// Consulta registros de SocialHubRetHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="socialHubRetHlp">SocialHubRetHlp que provee el criterio de selecciÃ³n para realizar la consulta</param>
        /// <returns>El DataSet que contiene la informaciÃ³n de SocialHubRetHlp generada por la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, SocialHub socialHub)
        {
            SocialHubRetHlp da = new SocialHubRetHlp();
            DataSet ds = da.Action(dctx, socialHub);
            return ds;
        }
        /// <summary>
        /// Consulta registros de SocialHubUsuarioRetHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="socialHubUsuarioRetHlp">SocialHubUsuarioRetHlp que provee el criterio de selecciÃ³n para realizar la consulta</param>
        /// <returns>El DataSet que contiene la informaciÃ³n de SocialHubUsuarioRetHlp generada por la consulta</returns>
        public DataSet RetrieveSocialHubUsuario(IDataContext dctx, SocialHub socialHub)
        {
            SocialHubUsuarioRetHlp da = new SocialHubUsuarioRetHlp();
            DataSet ds = da.Action(dctx, socialHub);
            return ds;
        }

        /// <summary>
        /// Devuelve todos los contactos de los grupos sociales del social hub.
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="socialhub"></param>
        /// <returns></returns>
        public List<UsuarioSocial> RetrieveContactos(IDataContext dctx, SocialHub socialhub)
        {
            List<UsuarioSocial> contactos = new List<UsuarioSocial>();
            socialhub = new GP.SocialEngine.Service.SocialHubCtrl().RetrieveSocialHubUsuarioComplete(dctx, socialhub);

            foreach (GrupoSocial item in socialhub.ListaGrupoSocial)
            {
                foreach (UsuarioGrupo item2 in item.ListaUsuarioGrupo)
                {
                    contactos.Add(item2.UsuarioSocial);
                }
            }
            return contactos;
        }
        /// <summary>
        /// Consulta registros de SocialHubReactivoRetHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="socialHubReactivoRetHlp">SocialHubReactivoRetHlp que provee el criterio de selecciÃ³n para realizar la consulta</param>
        /// <returns>El DataSet que contiene la informaciÃ³n de SocialHubReactivoRetHlp generada por la consulta</returns>
        public DataSet RetrieveSocialHubReactivo(IDataContext dctx, SocialHub socialHub)
        {
            SocialHubReactivoRetHlp da = new SocialHubReactivoRetHlp();
            DataSet ds = da.Action(dctx, socialHub);
            return ds;
        }
        /// <summary>
        /// Regresa un resgistro completp de SocialHub
        /// </summary>
        public SocialHub RetrieveSocialHubUsuarioComplete(IDataContext dctx, SocialHub socialHub)
        {
            InformacionSocialCtrl informacionSocialCtrl = new InformacionSocialCtrl();
            UsuarioSocialCtrl usuarioSocialCtrl = new UsuarioSocialCtrl();
            GrupoSocialCtrl grupoSocialCtrl = new GrupoSocialCtrl();
            AppSuscripcionCtrl appSuscripcion = new AppSuscripcionCtrl();

            //cargamos la informacion del social hub
            socialHub = LastDataRowToSocialHub(RetrieveSocialHubUsuario(dctx, new SocialHub { SocialHubID = socialHub.SocialHubID, SocialProfile = socialHub.SocialProfile, SocialProfileType = ESocialProfileType.USUARIOSOCIAL }));

            //cargamos la informacion social
            socialHub.InformacionSocial = informacionSocialCtrl.LastDataRowToInformacionSocial(informacionSocialCtrl.Retrieve(dctx, new InformacionSocial { InformacionSocialID = socialHub.InformacionSocial.InformacionSocialID }));

            //cargamos el usuario social
            socialHub.SocialProfile = usuarioSocialCtrl.LastDataRowToUsuarioSocial(usuarioSocialCtrl.Retrieve(dctx, new UsuarioSocial { UsuarioSocialID = Convert.ToInt64(socialHub.SocialProfileID) }));


            //Cargamos los grupos sociales
            socialHub.ListaGrupoSocial = grupoSocialCtrl.RetrieveGruposSocialSocialHub(dctx, socialHub);

            return socialHub;

        }
        /// <summary>
        /// Crea un registro de SocialHubUsuarioInsHlp en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="socialHubUsuarioInsHlp">SocialHubUsuarioInsHlp que desea crear</param>
        public void InsertSocialHubUsuario(IDataContext dctx, SocialHub socialHub)
        {
            SocialHubUsuarioInsHlp da = new SocialHubUsuarioInsHlp();
            da.Action(dctx, socialHub);
        }

        /// <summary>
        /// Crea e inserta un nuevo socialhub para un usuario social, si existe el social hub para el usuario, no se inserta
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="usuarioSocial"></param>
        public void InsertSocialHubUsuarioSocial(IDataContext dctx, UsuarioSocial usuarioSocial)
        {
            string sError = string.Empty;

            if (usuarioSocial.UsuarioSocialID == null)
                sError += " ,UsuarioSocialID";
            if (string.IsNullOrEmpty(usuarioSocial.ScreenName))
                sError += " ,Screename";
            if (sError.Length > 0)
                throw new Exception("SocialHubCtrl: Los siguientes campos no puede ser vacios: " + sError.Substring(2) + " ");

            //si existe el socialub para el usuario, no se si inserta
            DataSet ds = RetrieveSocialHubUsuario(dctx, new SocialHub { SocialProfile = usuarioSocial, SocialProfileType = ESocialProfileType.USUARIOSOCIAL });

            if (ds.Tables[0].Rows.Count > 0)
                return;

            object myFirm = new object();

            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);

            try
            {
                //insertamos informacion social
                InformacionSocialCtrl informacionSocialCtrl = new InformacionSocialCtrl();
                InformacionSocial informacionSocial = new InformacionSocial { FechaRegistro = DateTime.Now };

                informacionSocialCtrl.Insert(dctx, informacionSocial);

                informacionSocial = informacionSocialCtrl.LastDataRowToInformacionSocial(informacionSocialCtrl.Retrieve(dctx, informacionSocial));

                //creamos el social hub
                SocialHub socialHub = new SocialHub
                {
                    Alias = usuarioSocial.ScreenName,
                    FechaRegistro = DateTime.Now,
                    SocialProfile = usuarioSocial,
                    InformacionSocial = informacionSocial,
                    SocialProfileType = ESocialProfileType.USUARIOSOCIAL
                };

                //insertamos el socialhub
                InsertSocialHubUsuario(dctx, socialHub);
                dctx.CommitTransaction(myFirm);
            }
            catch (Exception ex)
            {

                dctx.RollbackTransaction(myFirm);
                dctx.CloseConnection(myFirm);
                throw ex;
            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(myFirm);

            }

        }

        /// <summary>
        /// Asocia un grupo social a un social hub y se crea el usuario grupo dentro del grupo social.
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="socialHub"></param>
        /// <param name="grupoSocial"></param>
        /// <param name="esModerador"></param>
        public void AsociarSocialHubGrupoSocialUsuarioGrupo(IDataContext dctx, SocialHub socialHub, GrupoSocial grupoSocial, bool esModerador, List<AreaConocimiento> areasConocimiento, long universidadID)
        {
            #region **** validaciones ****
            string sError = string.Empty;
            if (socialHub == null)
                sError += " ,SocialHub";
            if (sError.Length > 0)
                throw new Exception("SocialHubCtrl: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (grupoSocial == null)
                sError += " ,GrupoSocial";
            if (sError.Length > 0)
                throw new Exception("SocialHubCtrl: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));

            if (socialHub.SocialHubID == null)
                sError += " ,SocialHubID";
            if (socialHub.SocialProfileType == null)
                sError += " ,Social Profile Type";
            if (grupoSocial.GrupoSocialID == null)
                sError += " ,GrupoSocialID";
            if (sError.Length > 0)
                throw new Exception("SocialHubCtrl: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));

            DataSet ds = RetrieveSocialHubUsuario(dctx, socialHub);

            if (ds.Tables[0].Rows.Count < 1)
                throw new Exception("SocialHubCtrl: el social hub no existe " );

            socialHub = LastDataRowToSocialHub(ds);

            GrupoSocialCtrl grupoSocialCtrl = new GrupoSocialCtrl();

            DataSet dsGrupo = grupoSocialCtrl.Retrieve(dctx, grupoSocial);

            if (dsGrupo.Tables[0].Rows.Count < 1)
                throw new Exception("SocialHubCtrl: el grupo social no existe ");
#endregion

            List<GrupoSocial> grupos = grupoSocialCtrl.RetrieveGruposSocialSocialHub(dctx, socialHub);

            //el social hub ya tiene asignado el grupo social, por lo tanto no se inserta.
            if (grupos.Count > 0 && grupos.Exists(item => item.GrupoSocialID == grupoSocial.GrupoSocialID))
                return;

            object myFirm = new object();

            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);

            try
            {
                //asignamos el grupo al social hub

                InsertGrupoSocialSocialHub(dctx, grupoSocial, socialHub);


                UsuarioGrupo usuarioGrupo = new UsuarioGrupo { UsuarioSocial = (socialHub.SocialProfile as UsuarioSocial), Estatus = true };

                UsuarioGrupoCtrl usuarioGrupoCtrl = new UsuarioGrupoCtrl();

                DataSet dsUsuarioGrupo = usuarioGrupoCtrl.Retrieve(dctx, usuarioGrupo, grupoSocial, areasConocimiento, universidadID);

                //si el usuario ya fue asignado al grupo social y se encuentra activo, no se inserta.
                if (dsUsuarioGrupo.Tables[0].Rows.Count > 0)
                    return;

                if (!esModerador)
                    DesasociarSocialHubGruposSociales(dctx, socialHub, areasConocimiento, universidadID);

                usuarioGrupo.EsModerador = esModerador;
                usuarioGrupo.FechaAsignacion = DateTime.Now;
                usuarioGrupo.Estatus = true;

                //insertamos el usuario grupo en el grupo social, segun el perfil que se recibe.
                usuarioGrupoCtrl.Insert(dctx, usuarioGrupo, grupoSocial);

                dctx.CommitTransaction(myFirm);
            }
            catch (Exception ex)
            {

                dctx.RollbackTransaction(myFirm);
                dctx.CloseConnection(myFirm);
                throw ex;
            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(myFirm);

            }

        }

        public void DesasociarSocialHubGrupoSocialUsuarioGrupo(IDataContext dctx, SocialHub socialHub, GrupoSocial grupoSocial, List<AreaConocimiento> areasConocimiento, long universidadID)
        {
            #region **** validaciones ****
            string sError = string.Empty;
            if (socialHub == null)
                sError += " ,SocialHub";
            if (sError.Length > 0)
                throw new Exception("DesAsocialSocialHubGrupoSocialUsuarioGrupo: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (grupoSocial == null)
                sError += " ,GrupoSocial";
            if (sError.Length > 0)
                throw new Exception("DesAsocialSocialHubGrupoSocialUsuarioGrupo: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));

            if (socialHub.SocialHubID == null)
                sError += " ,SocialHubID";

            if (socialHub.SocialProfile == null)
                sError += " ,Social Profile";
            if (grupoSocial.GrupoSocialID == null)
                sError += " ,GrupoSocialID";
            if (sError.Length > 0)
                throw new Exception("DesAsocialSocialHubGrupoSocialUsuarioGrupo: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));

            DataSet ds = RetrieveSocialHubUsuario(dctx, socialHub);
            object firma = new object();

            if (ds.Tables[0].Rows.Count < 1)
                throw new Exception("DesAsocialSocialHubGrupoSocialUsuarioGrupo: el social hub no existe ");

            socialHub = LastDataRowToSocialHub(ds);
            UsuarioGrupo usuarioGrupo = new UsuarioGrupo { UsuarioSocial = (socialHub.SocialProfile as UsuarioSocial), Estatus = true };


            GrupoSocialCtrl grupoSocialCtrl = new GrupoSocialCtrl();

            DataSet dsGrupo = grupoSocialCtrl.Retrieve(dctx, grupoSocial);

            if (dsGrupo.Tables["GrupoSocial"].Rows.Count < 1)
                throw new Exception("DesasocialSocialHubGrupoSocialUsuarioGrupo: el grupo social no existe ");
            #endregion

            List<GrupoSocial> lsGrupoSocial = grupoSocialCtrl.RetrieveGruposSocialSocialHub(dctx, socialHub);
            if (lsGrupoSocial.Exists(gr => gr.GrupoSocialID == grupoSocial.GrupoSocialID))
            {
                grupoSocial = grupoSocialCtrl.RetrieveComplete(dctx, new GrupoSocial { GrupoSocialID = grupoSocial.GrupoSocialID }, areasConocimiento, null, universidadID);
                try
                {
                    //Desactivar el usuarioGrupo
                    dctx.BeginTransaction(firma);
                    if (grupoSocial.ListaUsuarioGrupo.Exists(usr => usr.UsuarioSocial.UsuarioSocialID == usuarioGrupo.UsuarioSocial.UsuarioSocialID))
                        grupoSocialCtrl.DeleteUsuarioGrupoGrupoSocial(dctx, grupoSocial, usuarioGrupo, areasConocimiento, universidadID);
                    else
                        return;
                    DeleteGrupoSocialSocialHub(dctx, grupoSocial, socialHub);

                    dctx.CommitTransaction(firma);
                }
                catch (Exception)
                {
                    dctx.RollbackTransaction(firma);
                    throw;
                }
                
            }
            else
                throw new Exception("DesasocialSocialHubGrupoSocialUsuarioGrupo: el grupo social no se encuentra asignado ");
        }

         
        public void DeleteGrupoSocialSocialHub(IDataContext dctx, GrupoSocial grupoSocial, SocialHub socialHub)
        {
            SocialHubGrupoSocialDelHlp da = new SocialHubGrupoSocialDelHlp();
            da.Action(dctx, grupoSocial, socialHub);
        }


        /// <summary>
        /// Crea un registro de GrupoSocialSocialHub en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="grupoSocial">GrupoSocial que desea agregar </param>
        /// <param name="socialHub">SocialHub donde desea agregar el GrupoSocial </param>
        public void InsertGrupoSocialSocialHub(IDataContext dctx, GrupoSocial grupoSocial, SocialHub socialHub)
        {
            SocialHubGrupoSocialInsHlp da = new SocialHubGrupoSocialInsHlp();
            da.Action(dctx, grupoSocial, socialHub);
        }

        /// <summary>
        /// Crea un registro de SocialHubReactivoInsHlp en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="socialHubReactivoInsHlp">SocialHubReactivoInsHlp que desea crear</param>
        public void InsertSocialHubReactivo(IDataContext dctx, SocialHub socialHub)
        {
            SocialHubReactivoInsHlp da = new SocialHubReactivoInsHlp();
            da.Action(dctx, socialHub);
        }
        /// <summary>
        /// Elimina un registro completo de socialhub
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="socialHub"></param>
        public void DeleteComplete(IDataContext dctx, SocialHub socialHub)
        {
            string sError = string.Empty;
            if (socialHub.SocialProfileType == null)
                sError += " ,Social Profile Type";
            if (sError.Length > 0)
                throw new Exception("SocialHubCtrl: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (socialHub.SocialProfile == null)
                sError += " ,Social Profile";
            if (sError.Length > 0)
                throw new Exception("SocialHubCtrl: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            object myFirm = new object();
            try
            {
                dctx.OpenConnection(myFirm);
                dctx.BeginTransaction(myFirm);
                AppSuscripcionCtrl appSuscripcionCtrl = new AppSuscripcionCtrl();
                if (socialHub.SocialProfileType == ESocialProfileType.USUARIOSOCIAL)
                {
                    DataSet ds = RetrieveSocialHubUsuario(dctx, socialHub);
                    if (ds.Tables["SocialHub"].Rows.Count < 1)
                        return;
                    socialHub = RetrieveSocialHubUsuarioComplete(dctx, socialHub);
                    //eliminar suscripciones


                    foreach (AppSuscripcion app in socialHub.ListaAppSuscripcion)
                    {
                        appSuscripcionCtrl.Delete(dctx, app, socialHub);
                    }
                }
                else if (socialHub.SocialProfileType == ESocialProfileType.REACTIVO)
                {
                    DataSet ds = RetrieveSocialHubReactivo(dctx, socialHub);
                    if (ds.Tables["SocialHub"].Rows.Count < 1)
                        return;
                    socialHub = LastDataRowToSocialHub(ds);

                }


                PublicacionCtrl publicacionCtrl = new PublicacionCtrl();
                DataSet dsPublicaciones = publicacionCtrl.Retrieve(dctx, new Publicacion { SocialHub = socialHub });

                foreach (DataRow drPublicacion in dsPublicaciones.Tables["Publicacion"].Rows)
                {
                    publicacionCtrl.DeleteComplete(dctx, publicacionCtrl.DataRowToPublicacion(drPublicacion));
                }

                SocialHubDelHlp da = new SocialHubDelHlp();
                da.Action(dctx, socialHub);

                if (socialHub.InformacionSocial != null && socialHub.InformacionSocial.InformacionSocialID != null)
                {
                    InformacionSocialCtrl informacionCtrl = new InformacionSocialCtrl();

                    informacionCtrl.Delete(dctx, socialHub.InformacionSocial);
                }
                dctx.CommitTransaction(myFirm);
            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(myFirm);
                dctx.CloseConnection(myFirm);
                throw ex;
            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(myFirm);
            }

        }

        public bool IsContacto(IDataContext dctx, UsuarioSocial usuarioSocial, UsuarioSocial visitante)
        {
            bool isContacto = false;
            List<UsuarioSocial> contactos = RetrieveContactos(dctx, new SocialHub { SocialProfile = usuarioSocial, SocialProfileType = ESocialProfileType.USUARIOSOCIAL });

            foreach (UsuarioSocial contacto in contactos)
            {
                if (contacto.UsuarioSocialID == visitante.UsuarioSocialID)
                {
                    isContacto = true;
                    break;
                }
            }
            return isContacto;

        }
        /// <summary>
        /// Crea un objeto de SocialHub a partir de los datos del Ãºltimo DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la informaciÃ³n de SocialHub</param>
        /// <returns>Un objeto de SocialHub creado a partir de los datos</returns>
        public SocialHub LastDataRowToSocialHub(DataSet ds)
        {
            if (!ds.Tables.Contains("SocialHub"))
                throw new Exception("LastDataRowToSocialHub: DataSet no tiene la tabla SocialHub");
            int index = ds.Tables["SocialHub"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToSocialHub: El DataSet no tiene filas");
            return this.DataRowToSocialHub(ds.Tables["SocialHub"].Rows[index - 1]);
        }
        /// <summary>
        /// Crea un objeto de SocialHub a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la informaciÃ³n de SocialHub</param>
        /// <returns>Un objeto de SocialHub creado a partir de los datos</returns>
        public SocialHub DataRowToSocialHub(DataRow row)
        {
            SocialHub socialHub = new SocialHub();
            socialHub.InformacionSocial = new InformacionSocial();
            if (row.IsNull("SocialHubID"))
                socialHub.SocialHubID = null;
            else
                socialHub.SocialHubID = (long)Convert.ChangeType(row["SocialHubID"], typeof(long));
            if (row.IsNull("FechaRegistro"))
                socialHub.FechaRegistro = null;
            else
                socialHub.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
            if (row.IsNull("Alias"))
                socialHub.Alias = null;
            else
                socialHub.Alias = (string)Convert.ChangeType(row["Alias"], typeof(string));
            if (row.IsNull("InformacionSocialID"))
                socialHub.InformacionSocial.InformacionSocialID = null;
            else
                socialHub.InformacionSocial.InformacionSocialID = (long)Convert.ChangeType(row["InformacionSocialID"], typeof(long));
            if (row.IsNull("SocialProfileType"))
                socialHub.SocialProfileType = null;
            else
            {
                socialHub.ToShortSocialProfileType = (short)Convert.ChangeType(row["SocialProfileType"], typeof(short));
                switch (socialHub.SocialProfileType)
                {
                    case ESocialProfileType.REACTIVO:
                        if (row.IsNull("ReactivoID"))
                            socialHub.SocialProfile = null;
                        break;
                    case ESocialProfileType.USUARIOSOCIAL:
                        if (row.IsNull("UsuarioSocialID"))
                            socialHub.SocialProfile = null;
                        else
                        {
                            UsuarioSocial usuarioSocial = new UsuarioSocial();
                            usuarioSocial.UsuarioSocialID = (long)Convert.ChangeType(row["UsuarioSocialID"], typeof(long));
                            socialHub.SocialProfile = usuarioSocial;
                        }
                        break;
                }
            }
            return socialHub;
        }

        private void DesasociarSocialHubGruposSociales(IDataContext dctx, SocialHub socialHub, List<AreaConocimiento> areasConocimiento, long universidadID)
        {
            //Consultar los grupos con el SocialHub
            GrupoSocialCtrl grupoSocialCtrl = new GrupoSocialCtrl();
            List<GrupoSocial> lsGrupoSocial = new List<GrupoSocial>();
            object firm = new object();

            try
            {
                dctx.OpenConnection(firm);
                dctx.BeginTransaction(firm);
                lsGrupoSocial = grupoSocialCtrl.RetrieveGruposSocialSocialHub(dctx, socialHub);

                if (lsGrupoSocial.Count > 0)
                {
                    foreach (GrupoSocial grupo in lsGrupoSocial)
                    {
                        DesasociarSocialHubGrupoSocialUsuarioGrupo(dctx, socialHub, grupo, areasConocimiento, universidadID);
                    }
                    dctx.CommitTransaction(firm);
                }
                
            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(firm);
                if(dctx.ConnectionState==ConnectionState.Open)
                    dctx.CloseConnection(firm);

                throw new Exception("DesasociarSocialHubGrupos:Ocurrió un error al desasociar los grupos sociales del SocialHub proporcionado");
            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(firm);

            }
        }
    }
}
