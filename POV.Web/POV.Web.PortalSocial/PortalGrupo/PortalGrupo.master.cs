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
using POV.Web.PortalSocial.AppCode.Controls;
using POV.Licencias.Service;
using POV.CentroEducativo.BO;
using POV.Expediente.Services;
using POV.Expediente.BO;
using POV.Licencias.BO;
using POV.Seguridad.BO;
using POV.CentroEducativo.Services;
using POV.Seguridad.Service;

namespace POV.Web.PortalSocial.PortalGrupo
{
    public partial class PortalGrupo : System.Web.UI.MasterPage
    {
        private IUserSession userSession;
        private IRedirector redirector;
        private readonly IDataContext dctx = ConnectionHelper.Default.Connection;
        private GrupoSocialCtrl grupoSocialCtrl;
        public PortalGrupo()
        {
            userSession = new UserSession();
            redirector = new Redirector();
            grupoSocialCtrl = new GrupoSocialCtrl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (userSession.IsLogin())
                {
                    this.LblNombreGrupo.Text = userSession.CurrentGrupoSocial.Nombre;
                        this.HplNoticiasDocentes.NavigateUrl = UrlHelper.GetGrupoNoticiasDocentesURL();
                        this.HplNoticiasDocentes.ToolTip = "Para tener acceso a docente tienes que tener una cuenta premium";
                    this.HplListaAlumnos.NavigateUrl = UrlHelper.GetGrupoListaURL();

                    //cargar menu de docentes
                    LicenciaEscuelaCtrl lec = new LicenciaEscuelaCtrl();
                    LicenciaEscuela le = new LicenciaEscuela();
                    le.Escuela = new Escuela() { EscuelaID = userSession.CurrentEscuela.EscuelaID};
                    
                    long universidadID = 0;
                    if (userSession.IsAlumno())
                    {
                        if (userSession.CurrentAlumno.Universidades.Count > 0)
                            universidadID = (long)userSession.CurrentAlumno.Universidades[0].UniversidadID;

                        GrupoSocial grupoSocial = grupoSocialCtrl.RetrieveComplete(dctx, new GrupoSocial { GrupoSocialID = userSession.CurrentGrupoSocial.GrupoSocialID }, userSession.CurrentAlumno.AreasConocimiento, null, universidadID);

                        var licenciaEscuela = lec.RetriveLicenciaEscuela(dctx,le);

                        var rootPlataforma = new POV.Web.PortalSocial.AppCode.Controls.MenuItem("Sin orientador", "Sin orientador", null);
                        var rootUniversidad = new POV.Web.PortalSocial.AppCode.Controls.MenuItem("Sin orientador", "Sin orientador", null);

                        // Buscar los orientadores del alumno
                        UsuarioExpedienteCtrl usuarioExpedienteCtrl = new UsuarioExpedienteCtrl();
                        var usuariosExpediente = usuarioExpedienteCtrl.Retrieve(dctx, new UsuarioExpediente() { AlumnoID = userSession.CurrentAlumno.AlumnoID });



                        if (usuariosExpediente.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in usuariosExpediente.Tables[0].Rows)
                            {
                                var usuarioExpediente = usuarioExpedienteCtrl.DataRowToUsuarioExpediente(dr);

                                var docenteUsuario = lec.RetrieveUsuarioOrientador(dctx, new Usuario() { UsuarioID = usuarioExpediente.UsuarioID });
 
                                UniversidadCtrl uniCtrl = new UniversidadCtrl(null);
                                UsuarioCtrl userCtrl = new UsuarioCtrl();
                                var usuario = userCtrl.LastDataRowToUsuario(userCtrl.Retrieve(dctx, new Usuario() { UsuarioID=usuarioExpediente.UsuarioID }));
                                string siglasUniversidad = string.Empty;
                                if (usuario.UniversidadId != null)
                                {
                                    var universidadUsuario = uniCtrl.Retrieve(new Universidad() { UniversidadID = usuario.UniversidadId }, false).FirstOrDefault();
                                    siglasUniversidad = " (" + universidadUsuario.Siglas + ")";
                                }

                                var usrSocialDocente = new LicenciaEscuelaCtrl().RetrieveUsuarioSocial(dctx, usuario);

                                usrSocialDocente.Estatus = true;                                

                                var docente = new POV.Web.PortalSocial.AppCode.Controls.MenuItem(usrSocialDocente.ScreenName, UrlHelper.GetImagenPerfilURL("thumb", (long)usrSocialDocente.UsuarioSocialID), UrlHelper.GetGrupoNoticiasMiDocenteURL((long)usrSocialDocente.UsuarioSocialID));
                                if (usuario.UniversidadId != null)
                                    rootUniversidad.Children.Add(docente);
                            }
                        }

                        if (rootUniversidad.Children.Count > 0)
                        {
                            divOrientadoresUniversidad.Visible = true;
                            MenuOrientadoresUniversidad.MenuItems = rootUniversidad;
                        }

                        int misOrientadores=(rootPlataforma.Children.Count + rootUniversidad.Children.Count);
                        lblMisOrientadores.InnerText = (misOrientadores > 1) ? "Mis orientadores" : (misOrientadores > 0) ? "Mi Orientador" : "Sin orientador";      
                    }
                    else
                        redirector.GoToHomePage(true);
                }
                else
                    redirector.GoToLoginPage(true);
            }
        }
    }
}