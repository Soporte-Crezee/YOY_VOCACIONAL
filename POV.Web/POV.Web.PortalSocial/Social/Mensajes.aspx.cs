using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.DataAccess;
using GP.SocialEngine.BO;
using GP.SocialEngine.Service;
using POV.Core.RedSocial.Implement;
using POV.Core.RedSocial.Interfaces;
using POV.Logger.Service;
using POV.CentroEducativo.Services;
using POV.CentroEducativo.BO;

namespace POV.Web.PortalSocial.Social
{
    public partial class Mensajes : System.Web.UI.Page
    {
        
        private IUserSession userSession;
        private IAccountService accountService;
        private IRedirector redirector;

        readonly IDataContext dctx = new DataContext((new DataProviderFactory()).GetDataProvider("POV"));

        private NotificacionCtrl notificacionCtrl;
        private MensajeCtrl mensajeCtrl;

        public Mensajes()
        {
            mensajeCtrl = new MensajeCtrl();
            userSession = new UserSession();
            redirector = new Redirector();
            accountService = new AccountService();
            notificacionCtrl = new NotificacionCtrl();
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (userSession.IsLogin())
                {

                    if (userSession.CurrentUsuarioSocial != null && userSession.CurrentGrupoSocial != null)
                    {
                        if (!IsPostBack)
                        {
                            EFAlumnoCtrl alumnoCtrl = new EFAlumnoCtrl(null);
                            Alumno alumno = alumnoCtrl.Retrieve(new Alumno { AlumnoID = userSession.CurrentAlumno.AlumnoID }, false).FirstOrDefault();
                        
                            if ((bool)alumno.CorreoConfirmado)
                            {
                                //Datos de Control
                                DataSet dataSet = mensajeCtrl.RetriveMensajesUsuario(dctx, true,
                                                                   userSession.CurrentUsuarioSocial.UsuarioSocialID, null);
                                if (dataSet != null && dataSet.Tables["Mensaje"].Rows.Count > 0)
                                    hdnRecordcount.Value = dataSet.Tables["Mensaje"].Rows.Count.ToString();

                                Guid notificacionId;
                                string strnotificacionId = Request.QueryString["n"];
                                strnotificacionId = string.IsNullOrEmpty(strnotificacionId) ? "" : strnotificacionId;
                                bool esNotificacionId = Guid.TryParse(strnotificacionId, out notificacionId);
                                if (esNotificacionId)
                                {
                                    hdnNotificacionID.Value = notificacionId.ToString();
                                    hdnTipoNotificacionID.Value = ((short)ETipoNotificacion.MENSAJE).ToString();

                                    Notificacion notificacion = notificacionCtrl.RetrieveComplete(dctx,
                                                                                                  new Notificacion { NotificacionID = notificacionId });
                                    Mensaje mensaje = GetMensajeNotificacion(notificacion);
                                    if (mensaje != null)
                                    {
                                        hdnMensajeID.Value = mensaje.MensajeID.ToString();
                                        Notificacion notificacionAnterior = (Notificacion)notificacion.Clone();
                                        notificacion.EstatusNotificacion = EEstatusNotificacion.CONFIRMADO;
                                        notificacionCtrl.Update(dctx, notificacion, notificacionAnterior);
                                    }
                                    else
                                    {
                                        //Mensaje no encontrado
                                        hdnMensajeID.Value = "0";
                                        Notificacion notificacionAnterior = (Notificacion)notificacion.Clone();
                                        notificacion.EstatusNotificacion = EEstatusNotificacion.ELIMINADO;
                                        notificacionCtrl.Update(dctx, notificacion, notificacionAnterior);
                                    }
                                }
                            }
                            else redirector.GoToHomeAlumno(true);
                        }
                    }
                    else
                    {
                        redirector.GoToHomePage(true);
                    }

                }
                else
                {
                    redirector.GoToLoginPage(true);
                }
                
                
            }
            catch (Exception ex)
            {
                LoggerHlp.Default.Error(this, ex);
                throw;
            }
            
        }

        /// <summary>
        /// Obtiene un mensaje mediante la notificación,verificado que el usuario esta asociado al mensaje
        /// </summary>
        /// <param name="notificacion"></param>
        /// <returns></returns>
        private Mensaje GetMensajeNotificacion(Notificacion notificacion)
        {
            if(notificacion == null)
                throw new SystemException("GetMensajeNotificacion:la notificación no puede ser nula");

            if(notificacion.NotificacionID == null)
                throw new Exception("GetMensajeNotificacion:el elemento notificado no puede ser nulo");

            Mensaje mensaje = mensajeCtrl.RetrieveComplete(dctx,(Mensaje)notificacion.Notificable);
            bool esAsociado = false;

            foreach(UsuarioSocial usrsocial in mensaje.Destinatarios)
                if (usrsocial.UsuarioSocialID == userSession.CurrentUsuarioSocial.UsuarioSocialID)
                {
                    esAsociado = true;
                    break;
                }
            if (esAsociado)
                return mensaje;

            return null;
        }
    }
}