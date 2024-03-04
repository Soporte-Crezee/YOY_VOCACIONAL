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
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;

namespace POV.Web.PortalSocial.PortalDocente
{
    public partial class PortalDocente : System.Web.UI.MasterPage
    {
        private IUserSession userSession;
        private IAccountService accountService;
        private IRedirector redirector;
        private readonly IDataContext dctx = ConnectionHelper.Default.Connection;
        private GrupoSocialCtrl grupoSocialCtrl;
        private SocialHubCtrl socialHubCtrl;
        private GrupoCicloEscolarCtrl grupoCicloEscolarCtrl;

        public PortalDocente()
        {
            accountService = new AccountService();
            userSession = new UserSession();
            redirector = new Redirector();
            grupoSocialCtrl = new GrupoSocialCtrl();
            socialHubCtrl = new SocialHubCtrl();
            grupoCicloEscolarCtrl = new GrupoCicloEscolarCtrl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (userSession.IsLogin())
                {
                    if (!userSession.IsAlumno())
                    {
                        if (userSession.CurrentEscuela == null)
                        {
                            redirector.GoToCambiarEscuela(true);
                            return;
                        }
                        //consultamos sus grupos ciclo escolar
                        DataSet ds = grupoCicloEscolarCtrl.Retrieve(dctx, new GrupoCicloEscolar { Escuela = userSession.CurrentEscuela, CicloEscolar = userSession.CurrentCicloEscolar });
                        List<GrupoSocial> gruposSociales = new List<GrupoSocial>();
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            //por cada grupo ciclo escolar se cargan los grupos sociales
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                GrupoCicloEscolar grupoCicloEscolar = grupoCicloEscolarCtrl.DataRowToGrupoCicloEscolar(dr);

                                List<Materia>  materias = grupoCicloEscolarCtrl.RetrieveMateriasDocente(dctx, userSession.CurrentDocente, grupoCicloEscolar);

                                //si el docente tiene materias asignadas en el grupo se agrega
                                if (materias.Count > 0)
                                {
                                    DataSet dsGrupoSocial = grupoSocialCtrl.Retrieve(dctx, new GrupoSocial { GrupoSocialID = grupoCicloEscolar.GrupoSocialID });
                                    if (dsGrupoSocial.Tables[0].Rows.Count > 0)
                                        //se agregan los grupos sociales
                                        gruposSociales.Add(grupoSocialCtrl.LastDataRowToGrupoSocial(dsGrupoSocial));
                                }
                            }
                        }

                        if (gruposSociales.Count > 0)
                        {
                            gruposSociales = gruposSociales.OrderBy(item => item.GrupoSocialID).ToList();
                            var root = new POV.Web.PortalSocial.AppCode.Controls.MenuItem("root", "", null);

                            foreach (GrupoSocial gs in gruposSociales)
                            {
                                var grupo = new POV.Web.PortalSocial.AppCode.Controls.MenuItem((long)gs.GrupoSocialID,null, "icon_group", null);

                                var muro = new POV.Web.PortalSocial.AppCode.Controls.MenuItem("Apuntes", "icon_muro", UrlHelper.GetDocenteGrupoMuroURL((long)gs.GrupoSocialID));
                                var noticias = new POV.Web.PortalSocial.AppCode.Controls.MenuItem("Inicio", "icon_noticias", UrlHelper.GetDocenteGrupoNoticiasURL((long)gs.GrupoSocialID));
                                var alumnos = new POV.Web.PortalSocial.AppCode.Controls.MenuItem("Estudiantes", "icon_alumnos", UrlHelper.GetDocenteGrupoAlumnosURL((long)gs.GrupoSocialID));
                                var orientacion = new POV.Web.PortalSocial.AppCode.Controls.MenuItem("Orientacion vocacional", "icon_calendar", UrlHelper.GetCalendarOrientadorURL());

                                grupo.Children.Add(muro);
                                grupo.Children.Add(noticias);
                                grupo.Children.Add(alumnos);
                                grupo.Children.Add(orientacion);

                                root.Children.Add(grupo);
                            }

                            this.ImgUser.ImageUrl = UrlHelper.GetImagenPerfilURL("normal", (long)userSession.CurrentUsuarioSocial.UsuarioSocialID);
                            this.LblEscuela.Text = userSession.CurrentEscuela.NombreEscuela;
                            this.HplPerfil.NavigateUrl = UrlHelper.GetPerfilURL(userSession.CurrentUsuarioSocial.UsuarioSocialID.Value);
                            this.MenuLateraDocente.MenuItems = root;
                        }
                        else
                            redirector.GoToCambiarEscuela(true);
                    }
                    else
                        redirector.GoToHomePage(true);
                }
                else
                    redirector.GoToLoginPage(true);
            }
        }

        public void SelectGrupoSocial(long index)
        {
            this.MenuLateraDocente.SelectedItem = index.ToString();
            
            
        }
    }
}