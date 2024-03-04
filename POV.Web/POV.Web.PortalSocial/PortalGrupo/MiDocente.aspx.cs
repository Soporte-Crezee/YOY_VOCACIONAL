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
using POV.CentroEducativo.Service;
using POV.Licencias.Service;
using POV.Seguridad.BO;
using POV.CentroEducativo.Services;
using POV.Seguridad.Service;
using POV.Comun.Service;
using System.Configuration;
using System.IO;
using POV.Logger.Service;
using System.Net.Mail;
using Framework.Base.Exceptions;

namespace POV.Web.PortalSocial.PortalGrupo
{
    public partial class MiDocente : System.Web.UI.Page
    {
        private IUserSession userSession;
        private IRedirector redirector;
        private readonly IDataContext dctx = ConnectionHelper.Default.Connection;

        public MiDocente()
        {
            userSession = new UserSession();
            redirector = new Redirector();

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (userSession.IsLogin() && userSession.IsAlumno())
                {
                    EFAlumnoCtrl alumnoCtrl = new EFAlumnoCtrl(null);
                    Alumno alumno = alumnoCtrl.Retrieve(new Alumno { AlumnoID = userSession.CurrentAlumno.AlumnoID }, false).FirstOrDefault();
                    if ((bool)alumno.CorreoConfirmado)
                    {

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
                            redirector.GoToHomePage(true);
                        }
                        else
                        {
                            UsuarioSocial usuarioSocial = new UsuarioSocialCtrl().RetrieveComplete(dctx, new UsuarioSocial { UsuarioSocialID = usuarioSocialID });
                            //validamos que el usuario exista
                            if (usuarioSocial != null)
                            {
                                // Es alumno o docente
                                bool esAlumno = true;
                                var tipolicencia = new LicenciaEscuelaCtrl().RetrieveDocente(dctx, usuarioSocial);
                                if (tipolicencia != null)
                                    esAlumno = false;
                                // para ver el muro del usuario, el visitante debe pertenecer al grupo 
                                GrupoSocialCtrl grupoSocialCtrl = new GrupoSocialCtrl();
                                //verificamos que en el grupo del visitante se encuentre el propietario del muro
                                //ademas no debe ser docente
                                UsuarioGrupo usuarioGrupo = new UsuarioGrupo();
                                if (userSession.IsDocente())
                                    usuarioGrupo = grupoSocialCtrl.RetrieveFriend(dctx, userSession.CurrentGrupoSocial, usuarioSocial, userSession.CurrentAlumno.AreasConocimiento, userSession.CurrentDocente.DocenteID, (long)userSession.CurrentUser.UniversidadId);
                                else
                                    usuarioGrupo = grupoSocialCtrl.RetrieveFriend(dctx, userSession.CurrentGrupoSocial, usuarioSocial, userSession.CurrentAlumno.AreasConocimiento, null, (long)userSession.CurrentAlumno.Universidades[0].UniversidadID, esAlumno);

                                if ((bool)usuarioGrupo.EsModerador) // si es su companiero y  es docente
                                {
                                    SocialHubCtrl socialHubCtrl = new SocialHubCtrl();
                                    DataSet dsSocialHub = socialHubCtrl.RetrieveSocialHubUsuario(dctx, new SocialHub { SocialProfile = usuarioSocial, SocialProfileType = ESocialProfileType.USUARIOSOCIAL });

                                    SocialHub socialHub = socialHubCtrl.LastDataRowToSocialHub(dsSocialHub);

                                    hdnSocialHubID.Value = socialHub.SocialHubID.ToString();
                                    hdnUsuarioSocialID.Value = ((UsuarioSocial)socialHub.SocialProfile).UsuarioSocialID.Value.ToString();

                                    //Orientador
                                    this.LblNombreEscuela.Text = string.Format(" Docente De:{0} ", userSession.CurrentEscuela.NombreEscuela);
                                    this.LblNombreDocente.Text = usuarioSocial.ScreenName;

                                    this.ImgUser.ImageUrl = UrlHelper.GetImagenPerfilURL("normal", (long)usuarioSocial.UsuarioSocialID);

                                    UniversidadCtrl uniCtrl = new UniversidadCtrl(null);
                                    UsuarioCtrl userCtrl = new UsuarioCtrl();

                                    LicenciaEscuelaCtrl licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
                                    Docente docente = licenciaEscuelaCtrl.RetrieveDocente(dctx, usuarioSocial);
                                    string siglasUniversidad = string.Empty;

                                    Usuario usuario = licenciaEscuelaCtrl.RetrieveUsuario(dctx, usuarioSocial);
                                    this.hdnSessionUsuarioOrientadorID.Value = usuario.UsuarioID.ToString();
                                    this.hdnCorreoOrientador.Value = usuario.Email;
                                    this.hdnSessionAlumnoID.Value = userSession.CurrentAlumno.AlumnoID.ToString();
                                    this.hdnAspiranteNombre.Value = userSession.CurrentAlumno.NombreCompletoAlumno;

                                    usuario = userCtrl.LastDataRowToUsuario(userCtrl.Retrieve(dctx, new Usuario() { UsuarioID = usuario.UsuarioID }));

                                    if (usuario.UniversidadId != null)
                                    {
                                        var universidadUsuario = uniCtrl.Retrieve(new Universidad() { UniversidadID = usuario.UniversidadId }, false).FirstOrDefault();
                                        siglasUniversidad = " (" + universidadUsuario.Siglas + ")";
                                        this.LblNombreUniversidad.Text = universidadUsuario.NombreUniversidad;
                                    }

                                    this.LblNombreUsuario.Text = string.Format("{0} {1}", usuarioSocial.ScreenName, siglasUniversidad);
                                    GrupoCicloEscolarCtrl grupoCtrl = new GrupoCicloEscolarCtrl();
                                    List<Materia> materias = grupoCtrl.RetrieveMateriasDocente(dctx, docente, new GrupoCicloEscolar { GrupoSocialID = userSession.CurrentGrupoSocial.GrupoSocialID });

                                    string sMaterias = string.Empty;

                                    foreach (Materia materia in materias)
                                    {
                                        if (sMaterias.Length > 0)
                                            sMaterias += ", ";
                                        sMaterias += materia.Titulo;
                                    }
                                    this.HplPerfil.NavigateUrl = UrlHelper.GetPerfilURL((long)usuarioSocial.UsuarioSocialID);
                                    this.LblAsignatura.Text = string.Format(" Asignatura(s): {0}.", sMaterias);
                                }
                                else
                                    redirector.GoToHomePage(true);
                            }
                            else
                                redirector.GoToHomePage(true);
                        }
                    }
                    else
                        redirector.GoToHomeAlumno(true);
                }
                else
                    redirector.GoToLoginPage(true);
            }
        }      
    }
}