using System;
using System.Collections.Generic;
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
using POV.Comun.Service;
using System.Configuration;

namespace POV.Web.PortalSocial.Social
{
    public partial class Perfil : System.Web.UI.Page
    {
        private IUserSession userSession;
        private IAccountService accountService;
        private IRedirector redirector;
        private readonly IDataContext dctx = ConnectionHelper.Default.Connection;

        public Perfil()
        {
            accountService = new AccountService();
            userSession = new UserSession();
            redirector = new Redirector();

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (userSession.IsLogin())
                {
                    Int64 usuarioSocialID = 0;

                    #region *** validaciones del usuario que llega del request ***
                    try
                    {
                        string user = Request.QueryString["u"];
                        user = string.IsNullOrEmpty(user) ? "0" : user;

                        usuarioSocialID = System.Convert.ToInt64(user);
                    }
                    catch (FormatException fex)
                    {
                        usuarioSocialID = 0;
                    }
                    catch (OverflowException ofex)
                    {
                        usuarioSocialID = 0;
                    }

                    #endregion
                    UsuarioSocial usuarioSocial = new UsuarioSocialCtrl().RetrieveComplete(dctx, new UsuarioSocial { UsuarioSocialID = usuarioSocialID });
                    //validamos que el usuario exista
                    if (usuarioSocial != null)
                    {
                        SocialHubCtrl socialHubCtrl = new SocialHubCtrl();
                        GrupoSocialCtrl grupoSocialCtrl = new GrupoSocialCtrl();
                        DataSet dsSocialHub;
                        SocialHub socialHub;

                        dsSocialHub = socialHubCtrl.RetrieveSocialHubUsuario(dctx, new SocialHub { SocialProfile = usuarioSocial, SocialProfileType = ESocialProfileType.USUARIOSOCIAL });
                        socialHub = socialHubCtrl.LastDataRowToSocialHub(dsSocialHub);
                        UsuarioGrupo usuarioGrupo = grupoSocialCtrl.RetrieveFriend(dctx,
                                                                                       userSession.
                                                                                           CurrentUsuarioSocial,
                                                                                       usuarioSocial, userSession.CurrentAlumno.AreasConocimiento, (long)userSession.CurrentAlumno.Universidades[0].UniversidadID);
                        if (usuarioGrupo != null)
                            LlenarCamposPerfil(usuarioGrupo, socialHub, usuarioSocial.UsuarioSocialID == userSession.CurrentUsuarioSocial.UsuarioSocialID);
                        else
                            redirector.GoToHomePage(true);
                    }

                    if (!this.userSession.IsAlumno())
                    {
                        this.btnReporteFelder.Visible = false;
                    }
                }
                else
                    redirector.GoToLoginPage(true);
            }
        }

        private void LlenarCamposPerfil(UsuarioGrupo usuarioGrupo, SocialHub socialHub, bool esPropietario)
        {
            InformacionSocialCtrl informacionSocialCtrl = new InformacionSocialCtrl();
            //firma del usuario
            InformacionSocial informacionSocial = new InformacionSocial { InformacionSocialID = socialHub.InformacionSocial.InformacionSocialID };
            informacionSocial = informacionSocialCtrl.LastDataRowToInformacionSocial(informacionSocialCtrl.Retrieve(dctx, informacionSocial));
            this.LblFirma.Text = string.IsNullOrEmpty(informacionSocial.Firma) ? "" : informacionSocial.Firma;

            //Presentar los datos del Usuario 
            this.LblEdad.Text = ((int)(DateTime.Now - usuarioGrupo.UsuarioSocial.FechaNacimiento.Value).TotalDays / 365).ToString();
            this.LblNombre.Text = usuarioGrupo.UsuarioSocial.ScreenName;
            this.LblFechaNacimiento.Text = String.Format("{0:dd \\de MMMM \\de yyyy}", usuarioGrupo.UsuarioSocial.FechaNacimiento);
            this.LblEscuela.Text = userSession.CurrentEscuela.NombreEscuela;
            if (!usuarioGrupo.EsModerador.Value)
            {
                this.LblGrado.Text = userSession.CurrentGrupoSocial.Nombre;
            }
            else
            {
                //Consultar los grupos del docente
                GrupoSocialCtrl grupoSocialCtrl = new GrupoSocialCtrl();
                List<GrupoSocial> lsGrupoSocial = grupoSocialCtrl.RetrieveGruposSocialSocialHub(dctx, socialHub);
                string strGrupos = string.Empty;
                string sMaterias = string.Empty;
                LicenciaEscuelaCtrl licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
                Docente docente = licenciaEscuelaCtrl.RetrieveDocente(dctx, usuarioGrupo.UsuarioSocial);

                GrupoCicloEscolarCtrl grupoCtrl = new GrupoCicloEscolarCtrl();
                if (lsGrupoSocial != null)
                {
                    foreach (GrupoSocial gsocial in lsGrupoSocial)
                    {
                        strGrupos += string.Format(", {0} {1}", gsocial.Nombre, System.Environment.NewLine);
                        List<Materia> materias = grupoCtrl.RetrieveMateriasDocente(dctx, docente, new GrupoCicloEscolar { GrupoSocialID = gsocial.GrupoSocialID });

                        foreach (Materia materia in materias)
                        {
                            if (sMaterias.Length > 0)
                                sMaterias += ", ";
                            sMaterias += materia.Titulo;
                        }
                    }
                    LblGrado.Text = strGrupos.Substring(2);
                }

                this.LblAsignatura.Visible = true;
                this.LblAsignaturaName.Visible = true;
                this.LblAsignatura.Text = string.Format("{0}.", sMaterias);
            }

            this.ImgUser.ImageUrl = UrlHelper.GetImagenPerfilURL("normal", (long)usuarioGrupo.UsuarioSocial.UsuarioSocialID);

            if (esPropietario)
            {
                this.HplEditarPerfil.Visible = true;
                HplEditarPerfil.NavigateUrl = UrlHelper.GetEditarPerfilURL();
            }
        }
        
        protected void btnReporteFelder_Click(object sender, EventArgs e)
        {
            try
            {
                string tituloCicloEscolar = null;
                CicloEscolarCtrl ctrl = new CicloEscolarCtrl();
                DataSet ds = ctrl.Retrieve(this.dctx, this.userSession.CurrentCicloEscolar);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    CicloEscolar ciclo = ctrl.DataRowToCicloEscolar(ds.Tables[0].Rows[0]);
                    tituloCicloEscolar = ciclo.Titulo;
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnReporteAptitudes_Click(object sender, EventArgs e)
        {
            try
            { 
                string ubicacionRelativaReportes = @ConfigurationManager.AppSettings["POVURLReportes"];
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
            }
        }
    }
}