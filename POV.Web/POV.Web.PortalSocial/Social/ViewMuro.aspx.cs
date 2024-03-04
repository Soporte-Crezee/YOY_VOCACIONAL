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
using POV.CentroEducativo.BO;
using POV.Licencias.Service;
using POV.Expediente.BO;
using POV.Expediente.Services;
using POV.Seguridad.BO;

namespace POV.Web.PortalSocial.Social
{
    public partial class ViewMuro : System.Web.UI.Page
    {
        private IUserSession userSession;
        private IAccountService accountService;
        private IRedirector redirector;
        private readonly IDataContext dctx = ConnectionHelper.Default.Connection;

        public ViewMuro()
        {
            accountService = new AccountService();
            userSession = new UserSession();
            redirector = new Redirector();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (userSession.IsLogin())
            {
                Int64 usuarioSocialID = 0;
                UsuarioSocial usuarioSocial;
                #region *** validaciones del usuario que llega del request ***
                try
                {
                    string user = Request.QueryString["u"];
                    user = string.IsNullOrEmpty(user) ? "0" : user;
                    usuarioSocialID = System.Convert.ToInt64(user);
                }
                catch (FormatException fex) { usuarioSocialID = 0; }
                catch (OverflowException ofex) { usuarioSocialID = 0; }
                #endregion

                if (usuarioSocialID <= 0)
                    usuarioSocial = userSession.CurrentUsuarioSocial;
                else
                    usuarioSocial = new UsuarioSocial { UsuarioSocialID = usuarioSocialID };


                usuarioSocial = new UsuarioSocialCtrl().RetrieveComplete(dctx, new UsuarioSocial { UsuarioSocialID = usuarioSocialID });
                //validamos que el usuario exista y que tenga seleccionado un grupo social
                if (usuarioSocial != null)
                {
                    GrupoSocialCtrl grupoSocialCtrl = new GrupoSocialCtrl();
                    //verificamos que en el grupo del visitante se encuentre el propietario del muro

                    UsuarioGrupo usuarioGrupo = null;
                    List<GrupoSocial> misGrupos = grupoSocialCtrl.RetrieveGruposSocialSocialHub(dctx, userSession.SocialHub);

                    UsuarioGrupoCtrl usuarioGrupoCtrl = new UsuarioGrupoCtrl();
                    usuarioGrupo = usuarioGrupoCtrl.LastDataRowToUsuarioGrupo(usuarioGrupoCtrl.Retrieve(dctx, new UsuarioGrupo { UsuarioSocial = usuarioSocial }, new GrupoSocial()));

                    LicenciaEscuelaCtrl licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
                    Usuario orientadorUsuario = null;
                    UsuarioExpediente user = null;
                    bool verOrientadorAsignado = false;

                    // Si el usuario es orientador se valida que sea orientador del alumno
                    if (usuarioGrupo != null && usuarioGrupo.EsModerador == true)
                    {
                       
                        Docente docente = licenciaEscuelaCtrl.RetrieveDocente(dctx, usuarioSocial);
                        
                        if (docente != null)
                        {
                            orientadorUsuario = licenciaEscuelaCtrl.RetrieveUsuario(dctx, docente);
                            UsuarioExpedienteCtrl usuarioExpedienteCtrl = new UsuarioExpedienteCtrl();
                            user = usuarioExpedienteCtrl.LastDataRowToUsuarioExpediente(usuarioExpedienteCtrl.Retrieve(dctx, new UsuarioExpediente { AlumnoID = userSession.CurrentAlumno.AlumnoID, UsuarioID = orientadorUsuario.UsuarioID }));
                            if (user != null)
                                verOrientadorAsignado = true;
                        }
                    }

                    // Si el usuario no es orientador se consulta en los contactos
                    if (usuarioGrupo != null && usuarioGrupo.EsModerador == false)
                    {
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
                    }

                    if (usuarioGrupo != null)
                        if (userSession.IsAlumno()) //si el usuario en sesion es alumno
                            if ((bool)usuarioGrupo.EsModerador) // es docente
                                if (verOrientadorAsignado)
                                    HttpContext.Current.Response.Redirect(UrlHelper.GetGrupoNoticiasMiDocenteURL((long)usuarioSocial.UsuarioSocialID), true);
                                else
                                    redirector.GoToHomePage(true);
                            else //es alumno
                                HttpContext.Current.Response.Redirect(UrlHelper.GetAlumnoMuroURL((long)usuarioSocial.UsuarioSocialID), true);
                        else
                            if ((bool)usuarioGrupo.EsModerador) //un docente trata de ver el muro de otro docente del mismo grupo
                                redirector.GoToHomePage(true);
                            else //un docente trata de ver el muro de un alumno de su grupo
                                HttpContext.Current.Response.Redirect(UrlHelper.GetAlumnoMuroURL((long)usuarioSocial.UsuarioSocialID), true);
                    else
                        redirector.GoToHomePage(true);
                }
                else
                    redirector.GoToHomePage(true);
            }
            else
            {
                redirector.GoToLoginPage(true);
            }
        }
    }
}