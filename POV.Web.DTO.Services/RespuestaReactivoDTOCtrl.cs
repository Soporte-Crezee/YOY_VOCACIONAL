using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using POV.Reactivos.BO;
using POV.Reactivos.Service;
using POV.ReactivosUsuario.BO;
using POV.ReactivosUsuario.Service;
using GP.SocialEngine.Service;
using GP.SocialEngine.BO;
using POV.Core.RedSocial.Implement;
using POV.Core.RedSocial.Interfaces;
using Framework.Base.DataAccess;
using POV.Modelo.Estandarizado.Service;
using POV.Modelo.Estandarizado.BO;
using POV.Licencias.BO;

namespace POV.Web.DTO.Services
{
    /// <summary>
    /// Controlador de reactivo dto
    /// </summary>
    public class RespuestaReactivoDTOCtrl
    {
        readonly IDataContext dctx = new DataContext((new DataProviderFactory()).GetDataProvider("POV"));

        private ReactivoCtrl reactivoCtrl;

        private RespuestaReactivoUsuarioCtrl respuestaReactivoCtrl;
        private RespuestaPreguntaUsuarioCtrl respuestaPreguntaCtrl;
        private RespuestaUsuarioCtrl respuestaUsuarioCtrl;

        private IUserSession userSession;

        public RespuestaReactivoDTOCtrl()
        {
            reactivoCtrl = new ReactivoCtrl();
            userSession = new UserSession();
            respuestaReactivoCtrl = new RespuestaReactivoUsuarioCtrl();
            respuestaPreguntaCtrl = new RespuestaPreguntaUsuarioCtrl();
            respuestaUsuarioCtrl = new RespuestaUsuarioCtrl();
        }

        /// <summary>
        /// Obtiene un reactivo dto para resolverlo.
        /// </summary>
        /// <param name="dto">DTO de reactivo</param>
        /// <returns>DTO de reactivo completo</returns>
        public reactivodto GetReactivo(reactivodto dto)
        {
            try
            {
                Guid reactivoID;

                if (Guid.TryParse(dto.reactivoid, out reactivoID))
                {
                    Reactivo filtro = null;
                    if (dto.tipo != null && dto.tipo == 1) //decision para saber si es un reactivo de un docente o un reactivo normal
                        filtro = new ReactivoDocente { ReactivoID = reactivoID, TipoReactivo = ETipoReactivo.Estandarizado };
                    else
                        filtro = new Reactivo { ReactivoID = reactivoID, TipoReactivo = ETipoReactivo.Estandarizado };

                    Reactivo reactivo = reactivoCtrl.RetrieveComplete(dctx, filtro);

                    if (reactivo != null)
                    {
                        dto = new reactivodto();
                        dto = ReactivoToDTO(reactivo);

                        dto.preguntas = new List<preguntadto>();

                        foreach (Pregunta pregunta in reactivo.Preguntas)
                        {
                            dto.preguntas.Add(PreguntaToDTO(reactivo, pregunta));
                        }
                    }
                    else
                    {
                        dto = new reactivodto();
                        dto.success = false;
                    }
                }
                else
                {
                    dto = new reactivodto();
                    dto.success = false;
                }

                return dto;
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
           
        }
        /// <summary>
        /// Registra la respuesta del reactivo de un usuario
        /// </summary>
        /// <param name="dto">DTO de respuesta</param>
        /// <returns>DTO de respuesta</returns>
        public respuestareactivodto RegistrarRespuestas(respuestareactivodto dto)
        {
            respuestareactivodto output = new respuestareactivodto();

            UsuarioSocial usuarioSocial = userSession.CurrentUsuarioSocial;

            Guid reactivoID;

            bool bValid = false;

            if (Guid.TryParse(dto.reactivoid, out reactivoID))
            {
                Reactivo reactivo = reactivoCtrl.RetrieveComplete(dctx, new Reactivo { ReactivoID = reactivoID, TipoReactivo = ETipoReactivo.Estandarizado , Activo = true});

                if (reactivo != null && reactivo.Preguntas != null && reactivo.Preguntas.Count > 0) // si el reactivo existe
                {
                    if (dto.preguntas != null && dto.preguntas.Count > 0) // si el listado de respuestas no esta vacio
                    {
                        List<RespuestaPreguntaUsuario> respuestasPregunta = new List<RespuestaPreguntaUsuario>();

                        #region *** creamos el listado de respuestas pregunta, no se asigna ids ***
                        foreach (Pregunta pregunta in reactivo.Preguntas) // por cada pregunta del reactivo crear la respuesta de pregunta
                        {
                            respuestapreguntadto respuestadto = dto.preguntas.First(item => item.preguntaid == pregunta.PreguntaID);

                            if (respuestadto != null)
                            {

                                ETipoRespuestaPlantilla tipo = (ETipoRespuestaPlantilla)pregunta.RespuestaPlantilla.TipoRespuestaPlantilla;

                                switch (tipo)
                                {
                                    case ETipoRespuestaPlantilla.OPCION_MULTIPLE:
                                        RespuestaPlantillaOpcionMultiple respuestaOpcion = (RespuestaPlantillaOpcionMultiple)pregunta.RespuestaPlantilla;
                                        OpcionRespuestaPlantilla opcionSeleccionada = respuestaOpcion.ListaOpcionRespuestaPlantilla.First(item => item.OpcionRespuestaPlantillaID == respuestadto.opcionseleccionadaid);

                                        if (opcionSeleccionada != null)
                                        {
                                            RespuestaPreguntaUsuario respuestaPregunta = new RespuestaPreguntaUsuario();
                                            respuestaPregunta.RespuestaPreguntaUsuarioID = Guid.NewGuid();
                                            respuestaPregunta.Pregunta = pregunta;
                                            respuestaPregunta.RespuestaUsuario = new RespuestaUsuarioOpcionMultiple
                                            {
                                                FechaRegistro = DateTime.Now,
                                                TipoRespuestaUsuario = ETipoRespuestaUsuario.OPCION_MULTIPLE,
                                                OpcionRespuestaPlantilla = opcionSeleccionada
                                            };

                                            respuestasPregunta.Add(respuestaPregunta);

                                        }

                                        break;
                                    default:
                                        output.success = false;
                                        output.error = "Reactivo Incorrecto";
                                        break;
                                }


                            }
                        }
                        #endregion

                        //validamos que se hayan respondido todas las preguntas
                        if (respuestasPregunta.Count != reactivo.Preguntas.Count)
                        {
                            output.success = false;
                            output.error = "Reactivo Incorrecto";
                            return output;
                        }

                        DataSet ds = respuestaReactivoCtrl.Retrieve(dctx, new RespuestaReactivoUsuario { Reactivo = reactivo, UsuarioSocial = usuarioSocial });
                        RespuestaReactivoUsuario respuestaReactivo = new RespuestaReactivoUsuario();

                        int intentos = 1;

                        object myFirm = new object();
                        dctx.OpenConnection(myFirm);
                        dctx.BeginTransaction(myFirm);
                        try
                        {
                            if (ds.Tables[0].Rows.Count > 0) //ya existe el registro de respuesta, se actualizan sus respuestas
                            {
                                respuestaReactivo = respuestaReactivoCtrl.RetrieveComplete(dctx, respuestaReactivoCtrl.LastDataRowToRespuestaReactivoUsuario(ds));

                                foreach (RespuestaPreguntaUsuario respuestaPregunta in respuestasPregunta)
                                {
                                    bool exist = respuestaReactivo.ListaRespuestaPreguntaUsuario.Exists(item => item.Pregunta.PreguntaID == respuestaPregunta.Pregunta.PreguntaID);
                                    if (exist) //existe la respuesta pregunta?
                                    {
                                        //obtenemos la respuesta registrada
                                        RespuestaPreguntaUsuario respuestaPreguntaUsuario = respuestaReactivo.ListaRespuestaPreguntaUsuario.First(item => item.Pregunta.PreguntaID == respuestaPregunta.Pregunta.PreguntaID);
                                        if (respuestaPreguntaUsuario != null)
                                        {
                                            if (respuestaPreguntaUsuario.RespuestaUsuario.TipoRespuestaUsuario == ETipoRespuestaUsuario.OPCION_MULTIPLE)
                                            {
                                                //actualizamos la opcion seleccionada
                                                RespuestaUsuarioOpcionMultiple opcion = (RespuestaUsuarioOpcionMultiple)respuestaPreguntaUsuario.RespuestaUsuario;
                                                opcion.OpcionRespuestaPlantilla = ((RespuestaUsuarioOpcionMultiple)respuestaPregunta.RespuestaUsuario).OpcionRespuestaPlantilla;

                                                respuestaUsuarioCtrl.UpdateOpcionMultiple(dctx, respuestaPreguntaUsuario, opcion, opcion);

                                            }
                                        }
                                    }
                                    else
                                    {
                                        respuestaPreguntaCtrl.Insert(dctx, respuestaReactivo, respuestaPregunta);
                                        respuestaUsuarioCtrl.Insert(dctx, respuestaPregunta, respuestaPregunta.RespuestaUsuario);
                                    }
                                }

                                decimal nuevaCalificacion = respuestaReactivoCtrl.CalificarReactivo(dctx, respuestaReactivo);

                                respuestaReactivo.NumeroIntentos = respuestaReactivo.NumeroIntentos + 1;
                                respuestaReactivo.UltimaActualizacion = DateTime.Now;
                                respuestaReactivo.UltimaCalificacion = nuevaCalificacion;

                                if (respuestaReactivo.EstadoReactivoUsuario != EEstadoReactivoUsuario.TERMINADO)
                                {
                                    //respuesta correcta, por lo que se termina el estado del reactivo
                                    if (nuevaCalificacion > 0)
                                    {
                                        intentos = respuestaReactivo.NumeroIntentos.Value;
                                        respuestaReactivo.EstadoReactivoUsuario = EEstadoReactivoUsuario.TERMINADO;
                                    }

                                    respuestaReactivoCtrl.Update(dctx, respuestaReactivo, respuestaReactivo);
                                }
                            }
                            else
                            {
                                //creacion de los datos de respuesta reactivo
                                respuestaReactivo.RespuestaReactivoUsuarioID = Guid.NewGuid();
                                respuestaReactivo.Reactivo = reactivo;
                                respuestaReactivo.UsuarioSocial = usuarioSocial;
                                respuestaReactivo.NumeroIntentos = 1;
                                respuestaReactivo.FechaRegistro = DateTime.Now;
                                respuestaReactivo.UltimaActualizacion = respuestaReactivo.FechaRegistro;
                                respuestaReactivo.ListaRespuestaPreguntaUsuario = respuestasPregunta;
                                respuestaReactivo.EstadoReactivoUsuario = EEstadoReactivoUsuario.INICIADO;

                                respuestaReactivoCtrl.InsertComplete(dctx, respuestaReactivo);

                                decimal calificacion = respuestaReactivoCtrl.CalificarReactivo(dctx, respuestaReactivo);

                                respuestaReactivo.PrimeraCalificacion = calificacion;
                                respuestaReactivo.UltimaCalificacion = calificacion;
                                if (calificacion > 0)
                                {
                                    respuestaReactivo.EstadoReactivoUsuario = EEstadoReactivoUsuario.TERMINADO;
                                    intentos = respuestaReactivo.NumeroIntentos.Value;
                                }
                                respuestaReactivoCtrl.Update(dctx, respuestaReactivo, respuestaReactivo);

                            }
                            
                            output = new respuestareactivodto();
                            output.success = true;
                            output.fallocompleto = respuestaReactivo.UltimaCalificacion > 0 ? false : true;
                            output.rendersugerir = !(bool)output.fallocompleto && respuestaReactivo.NumeroIntentos == 1;
                            output.retroalimentacion = string.Empty;
                            output.reactivoid = reactivo.ReactivoID.ToString();

                            dctx.CommitTransaction(myFirm);
                        }
                        catch (Exception ex)
                        {
                            POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                            dctx.RollbackTransaction(myFirm);
                            dctx.CloseConnection(myFirm);
                            output.success = false;
                            output.error = "La operación no se pudo realizar >> " + ex.Message ;
                        }
                        finally
                        {
                            if (dctx.ConnectionState == ConnectionState.Open)
                                dctx.CloseConnection(myFirm);
                        }
                    }
                    else
                    {
                        output.success = false;
                        output.error = "Reactivo Incorrecto";
                    }

                }
                else
                {
                    output.success = false;
                    output.error = "Reactivo Incorrecto";
                }
            }
            else
            {
                output.success = false;
                output.error = "Reactivo Incorrecto";
            }


            return output;
        }

        private reactivodto ReactivoToDTO(Reactivo reactivo)
        {
            reactivodto dto = new reactivodto();
            dto.issuscrito = false;
            dto.reactivoid = reactivo.ReactivoID.ToString();
            dto.nombrereactivo = reactivo.NombreReactivo;
            dto.plantilla = reactivo.PlantillaReactivo;
            dto.descripcion = reactivo.Descripcion;

            if (reactivo.PresentacionPlantilla != null)
                dto.presentacionplantilla = (byte)reactivo.PresentacionPlantilla;

            return dto;
        }

        private preguntadto PreguntaToDTO(Reactivo reactivo, Pregunta pregunta)
        {
            preguntadto dto = new preguntadto();
            dto.reactivoid = reactivo.ReactivoID.ToString();
            if (pregunta.Orden != null)
                dto.orden = pregunta.Orden;
            if (pregunta.PreguntaID != null)
                dto.preguntaid = pregunta.PreguntaID;

            if (pregunta.TextoPregunta != null)
                dto.textopregunta = pregunta.TextoPregunta;



            RespuestaPlantilla respuestaPlantilla = pregunta.RespuestaPlantilla;
            switch (respuestaPlantilla.TipoRespuestaPlantilla)
            {
                case ETipoRespuestaPlantilla.ABIERTA:
                    RespuestaPlantillaTexto plantillaAbierta = (RespuestaPlantillaTexto)respuestaPlantilla;

                    dto.tipoplantilla = plantillaAbierta.ToShortTipoRespuestaPlantilla;
                    dto.maximocarateres = plantillaAbierta.MaximoCaracteres;
                    break;
                case ETipoRespuestaPlantilla.ABIERTA_NUMERICO:
                    RespuestaPlantillaNumerico plantillaNumerico = (RespuestaPlantillaNumerico) respuestaPlantilla;

                    dto.tipoplantilla = plantillaNumerico.ToShortTipoRespuestaPlantilla;
                    break;
                case ETipoRespuestaPlantilla.OPCION_MULTIPLE:
                    RespuestaPlantillaOpcionMultiple plantillaOpcionMultiple = (RespuestaPlantillaOpcionMultiple)respuestaPlantilla;
                    dto.tipoplantilla = plantillaOpcionMultiple.ToShortTipoRespuestaPlantilla;
                    dto.opciones = new List<opcionrespuestadto>();
                    foreach (OpcionRespuestaPlantilla opcion in plantillaOpcionMultiple.ListaOpcionRespuestaPlantilla)
                    {
                        opcionrespuestadto opcionrespuestadto = OpcionRespuestaToDTO(opcion);
                        opcionrespuestadto.preguntaid = dto.preguntaid;
                        dto.opciones.Add(opcionrespuestadto);
                    }
                    break;
            }

            return dto;
        }

        private opcionrespuestadto OpcionRespuestaToDTO(OpcionRespuestaPlantilla opcionRespuesta)
        {
            opcionrespuestadto dto = new opcionrespuestadto();
            if (opcionRespuesta.OpcionRespuestaPlantillaID != null)
                dto.opcionid = opcionRespuesta.OpcionRespuestaPlantillaID;
            if (opcionRespuesta.Texto != null)
                dto.texto = opcionRespuesta.Texto;
            if (opcionRespuesta.ImagenUrl != null)
                dto.imagen = opcionRespuesta.ImagenUrl;
            if (opcionRespuesta.EsPredeterminado != null)
                dto.predeterminado = opcionRespuesta.EsPredeterminado;

            return dto;
        }
    }
}
