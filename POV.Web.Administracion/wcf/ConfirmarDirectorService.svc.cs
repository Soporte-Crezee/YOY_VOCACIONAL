using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using POV.Core.Administracion.Implements;
using POV.Core.Administracion.Interfaces;
using POV.CentroEducativo.Service;
using POV.CentroEducativo.BO;
using POV.Web.DTO.Services;
using POV.Web.DTO;
using POV.Comun.Service;
using Framework.Base.DataAccess;
using POV.Web.Administracion.Helper;
using POV.Licencias.BO;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using System.Net.Mail;

namespace POV.Web.Administracion.wcf
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ConfirmarDirectorService
    {
        private IUserSession userSession;
        private EscuelaCtrl escuelaCtrl;
        private DirectorCtrl directorCtrl;
        private EscuelaDTOCtrl escuelaDTOCtrl;
        private IDataContext dctx = ConnectionHlp.Default.Connection;

        public ConfirmarDirectorService()
        {
            userSession = new UserSession();
            escuelaCtrl = new EscuelaCtrl();
            escuelaDTOCtrl = new EscuelaDTOCtrl();
            directorCtrl = new DirectorCtrl();
        }
        
        [OperationContract]
        public confirmaciondirectordto  GetInfoConfirmacion()
        {
            try
            {
                confirmaciondirectordto dto = new confirmaciondirectordto();
                if (userSession.LicenciasDirector != null && userSession.LicenciasDirector.Count > 0)
                {
                    dto.nombreusuario = userSession.CurrentUser.NombreUsuario;
                    dto.directorid = userSession.CurrentDirector.DirectorID;
                    dto.escuelas = new List<escueladto>();
                    foreach (LicenciaEscuela licenciaEscuela in userSession.LicenciasDirector)
                    {
                        Escuela escuela = DatosCompletos(licenciaEscuela.Escuela);
                        dto.escuelas.Add(escuelaDTOCtrl.ObjectToDto(escuela));
                    }
                }

                return dto;
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
        }

        [OperationContract]
        public confirmaciondirectordto ConfirmarInformacionDirector(confirmaciondirectordto dto)
        {
            Usuario usuarioNuevo = (Usuario)userSession.CurrentUser.Clone();
            Director directorNuevo = (Director)userSession.CurrentDirector.Clone();
            directorNuevo.EstatusIdentificacion = true;

            object myFirm = new object();
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            try
            {
                #region update usuario
                usuarioNuevo = UsuarioToObjeto(usuarioNuevo, dto);
                UsuarioCtrl usuarioCtrl = new UsuarioCtrl();
                usuarioCtrl.Update(dctx, usuarioNuevo, userSession.CurrentUser);
                #endregion

                #region update director
                directorCtrl.Update(dctx, directorNuevo, userSession.CurrentDirector);
                #endregion

                #region registro informacion centro de computo
                foreach (escueladto escueladto in dto.escuelas)
                {
                    Escuela escuela = escuelaDTOCtrl.DtoToObject(escueladto);
                    if (escuela.CentroComputo.CentroComputoID == null)
                    {
                        escuela.CentroComputo.FechaRegistro = DateTime.Now;
                        escuela.CentroComputo.Activo = true;
                        escuelaCtrl.InsertCentroComputo(dctx, escuela.CentroComputo, escuela);
                    }
                    else
                    {
                        escuela.CentroComputo.Activo = true;
                        escuelaCtrl.UpdateCentroComputo(dctx, escuela.CentroComputo, escuela.CentroComputo, escuela);
                    }
                }
                #endregion

                dto.infomsg = "";

                #region envio de correo
                if (usuarioNuevo.NombreUsuario != userSession.CurrentUser.NombreUsuario)
                {
                    if (usuarioNuevo.Email != null)
                    {
                        EnviarCorreo(usuarioNuevo);
                        dto.infomsg = "Tu usuario ha sido actualizado.Te hemos enviado un correo con tu nuevo usuario";
                    }
                }
                #endregion

                
                dctx.CommitTransaction(myFirm);

                userSession.CurrentDirector.EstatusIdentificacion = true;
                userSession.CurrentUser.NombreUsuario = usuarioNuevo.NombreUsuario;
                dto.success = true;
                dto.urlredirect = POV.Core.Administracion.UrlHelper.GetAceptarTerminosURL();
            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(myFirm);
                dctx.CloseConnection(myFirm);
                dto.success = false;
                if (ex.Source == "Usuario")
                    dto.errormsg = "El usuario no esta disponible";
                if (ex.Source == "usuarioTamano")
                    dto.errormsg = "El usuario tiene que ser mínimo de 6 caracteres y máximo de 20 caracteres";
                if (ex.Source == "whitespace")
                    dto.errormsg = "El usuario no puede tener espacios en blanco.";
                else
                    dto.errormsg = ex.Message;
            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(myFirm);
            }

            
            return dto;
        }

        private void EnviarCorreo(Usuario usuarioNuevo)
        {
            CorreoCtrl correoCtrl = new CorreoCtrl();
            
            string urlimg = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgNatware"];
            const string imgalt = "YOY - ADMINISTRADOR";
            const string titulo = "YOY - ADMINISTRADOR";
            string linkportal = System.Configuration.ConfigurationManager.AppSettings["POVUrlPortalAdministrativo"];
        
            
            string cuerpo = string.Format(@"<table width='600'><tr><td>
                                            <img src='{0}' alt='{1}' /></td></tr>
                                            <tr><td><h2 style='color:#A5439A'>{2}</h2>
                                            </p><p>Este correo fue enviado debido al cambio de usuario.</p><p>Nuevo Usuario: {3}</p>
                                            </tr>
                                            <tr><td>
                                            <a href='{4}'>YOY - Portal administraci&oacute;n</a>
                                            </td></tr>
                                          </table>"
                                          , urlimg, imgalt, titulo, usuarioNuevo.NombreUsuario, linkportal);

            
            List<string> tos = new List<string>();
            tos.Add(usuarioNuevo.Email);
            AlternateView texto = null;
            List<string> archivos = new List<string>();
            List<string> copias = new List<string>();
            try
            {
                correoCtrl.sendMessage(tos, "Cambio de Nombre de Usuario", cuerpo, texto, archivos, copias);
            }
            catch (Exception ex)
            {
                ex.Source = "EnviarCorreo";
            }
        }
        private Usuario UsuarioToObjeto(Usuario usuarioNuevo, confirmaciondirectordto dto)
        {
            if (dto.nombreusuario.Trim().Contains(" "))
            {
                var ex = new Exception();
                ex.Source = "whitespace";
                throw (ex);
            }
            if (dto.nombreusuario.Trim() == "" || (dto.nombreusuario.Trim().Count() < 6 || dto.nombreusuario.Trim().Count() > 50))
            {
                var ex = new Exception();
                ex.Source = "usuarioTamano";
                throw (ex);
            }
            else
                usuarioNuevo.NombreUsuario = dto.nombreusuario.Trim();
            return usuarioNuevo;
        }
        private Escuela DatosCompletos(Escuela escuela)
        {
            escuela = escuelaCtrl.RetrieveComplete(dctx, escuela); 
            NivelEducativoCtrl nivelEducativoCtrl = new NivelEducativoCtrl();
            TipoServicioCtrl tipoServicioCtrl = new TipoServicioCtrl();
            ZonaCtrl zonaCtrl = new ZonaCtrl();
            PaisCtrl paisCtrl = new PaisCtrl();
            EstadoCtrl estadoCtrl = new EstadoCtrl();
            CiudadCtrl ciudadCtrl = new CiudadCtrl();
            LocalidadCtrl localidadCtrl = new LocalidadCtrl();
            
            escuela.TipoServicio = tipoServicioCtrl.LastDataRowToTipoServicio(tipoServicioCtrl.Retrieve(dctx, escuela.TipoServicio));
            escuela.TipoServicio.NivelEducativoID = nivelEducativoCtrl.LastDataRowToNivelEducativo(nivelEducativoCtrl.Retrieve(dctx, escuela.TipoServicio.NivelEducativoID));
            escuela.ZonaID = zonaCtrl.LastDataRowToZona(zonaCtrl.Retrieve(dctx, escuela.ZonaID));
            escuela.Ubicacion.Pais = paisCtrl.LastDataRowToPais(paisCtrl.Retrieve(dctx, escuela.Ubicacion.Pais));
            escuela.Ubicacion.Estado = estadoCtrl.LastDataRowToEstado(estadoCtrl.Retrieve(dctx, escuela.Ubicacion.Estado));
            escuela.Ubicacion.Localidad = localidadCtrl.LastDataRowToLocalidad(localidadCtrl.Retrieve(dctx, escuela.Ubicacion.Localidad));
            escuela.Ubicacion.Ciudad = ciudadCtrl.LastDataRowToCiudad(ciudadCtrl.Retrieve(dctx, escuela.Ubicacion.Ciudad));
            escuela.CentroComputo = escuelaCtrl.RetrieveCentroComputo(dctx, escuela);
            return escuela;
        }
    }
}
