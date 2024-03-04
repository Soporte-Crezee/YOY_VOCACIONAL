using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.Core.RedSocial.Interfaces;
using POV.Core.RedSocial.Implement;
using GP.SocialEngine.BO;
using GP.SocialEngine.Service;
using Framework.Base.DataAccess;
using POV.Web.PortalSocial.AppCode;
using System.Data;
using POV.Logger.Service;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.CentroEducativo.Services;

namespace POV.Web.PortalSocial.PortalAlumno
{
    public partial class Muro : System.Web.UI.Page
    {
       
        private IUserSession userSession;
        private IRedirector redirector;
        private readonly IDataContext dctx = ConnectionHelper.Default.Connection;
        private AlumnoCtrl alumnoCtrl;

        public Muro()
        {
            userSession = new UserSession();
            redirector = new Redirector();
            alumnoCtrl = new AlumnoCtrl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (!IsPostBack)
                {
                    if (userSession.IsLogin() && userSession.CurrentEscuela != null)
                    {
                        EFAlumnoCtrl alumnoCtrl = new EFAlumnoCtrl(null);
                        Alumno alumno = alumnoCtrl.Retrieve(new Alumno { AlumnoID = userSession.CurrentAlumno.AlumnoID }, false).FirstOrDefault();
                        if (userSession.IsAlumno()) 
                        {
                            if (!(bool)alumno.CorreoConfirmado)
                                redirector.GoToHomeAlumno(true);
                        }
                        this.hdnSessionSocialHubID.Value = userSession.SocialHub.SocialHubID.ToString();
                        this.hdnSessionUsuarioSocialID.Value = userSession.CurrentUsuarioSocial.UsuarioSocialID.ToString();

                        
                        #region *** validaciones del usuario que llega del request ***
                        Int64 usuarioSocialID;

                        string user = Request.QueryString["u"];
                        user = string.IsNullOrEmpty(user) ? "0" : user;

                        if (!Int64.TryParse(user, out usuarioSocialID))
                        {
                            redirector.GoToHomePage(true);
                            return;
                        }
                        #endregion

                        hdnTipoPublicacionSuscripcionReactivo.Value = ((short)ETipoPublicacion.REACTIVO).ToString();
                        hdnTipoPublicacionTexto.Value = ((short)ETipoPublicacion.TEXTO).ToString();

                        if (usuarioSocialID <= 0 || usuarioSocialID == userSession.CurrentUsuarioSocial.UsuarioSocialID) // el 
                        {
                            if (userSession.IsAlumno())
                            {
                                ((PortalAlumno)this.Master).LoadControlsAlumnoMaster((long)userSession.CurrentUsuarioSocial.UsuarioSocialID, false);
                                hdnSocialHubID.Value = userSession.SocialHub.SocialHubID.ToString();

                                hdnUsuarioSocialID.Value = userSession.CurrentUsuarioSocial.UsuarioSocialID.ToString();

                                this.LblNombreUsuario.Text = userSession.CurrentUsuarioSocial.ScreenName;
                            }
                            else
                                redirector.GoToHomePage(true);
                        }
                        else
                        {
                            UsuarioSocial usuarioSocial = new UsuarioSocialCtrl().RetrieveComplete(dctx, new UsuarioSocial { UsuarioSocialID = usuarioSocialID });
                            //validamos que el usuario exista
                            if (usuarioSocial != null)
                            {
                                if (!userSession.IsAlumno())
                                {
                                    hdnFuente.Value = "D";
                                    pnlSeleccionContenido.Visible = true;
                                }
                                // para ver el muro del usuario, el visitante debe pertenecer al grupo 
                                GrupoSocialCtrl grupoSocialCtrl = new GrupoSocialCtrl();
                                //verificamos que en el grupo del visitante se encuentre el propietario del muro
                                //ademas no debe ser docente
                              
                                UsuarioGrupo usuarioGrupo = null;
                                List<GrupoSocial> misGrupos = grupoSocialCtrl.RetrieveGruposSocialSocialHub(dctx, userSession.SocialHub);

                                foreach (GrupoSocial gs in misGrupos)
                                {
                                    long universidadID = 0;
                                    if (userSession.IsDocente())
                                        universidadID = (long)userSession.CurrentUser.UniversidadId;
                                    else
                                        universidadID = (long)userSession.CurrentAlumno.Universidades[0].UniversidadID;

                                    usuarioGrupo = grupoSocialCtrl.RetrieveFriend(dctx, gs, usuarioSocial, userSession.CurrentAlumno.AreasConocimiento, null, universidadID);
                                    if (usuarioGrupo != null)
                                        break;
                                }
                                if (usuarioGrupo != null && !(bool)usuarioGrupo.EsModerador) // si es su companiero y no es docente
                                {
                                    ((PortalAlumno)this.Master).LoadControlsAlumnoMaster((long)usuarioSocial.UsuarioSocialID, true);
                                    SocialHubCtrl socialHubCtrl = new SocialHubCtrl();
                                    DataSet dsSocialHub = socialHubCtrl.RetrieveSocialHubUsuario(dctx, new SocialHub { SocialProfile = usuarioSocial, SocialProfileType = ESocialProfileType.USUARIOSOCIAL });

                                    SocialHub socialHub = socialHubCtrl.LastDataRowToSocialHub(dsSocialHub);

                                    hdnSocialHubID.Value = socialHub.SocialHubID.ToString();
                                    hdnUsuarioSocialID.Value = ((UsuarioSocial)socialHub.SocialProfile).UsuarioSocialID.Value.ToString();
                                    this.LblNombreUsuario.Text = usuarioSocial.ScreenName;


                                }
                                else
                                    redirector.GoToHomePage(false);
                            }
                            else
                                redirector.GoToHomePage(false);
                        }

                    }
                    else
                        redirector.GoToLoginPage(false);
                }
            }
            catch (Exception ex)
            {
                LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
        }
    }
}