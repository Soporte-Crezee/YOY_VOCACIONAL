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

namespace POV.Web.PortalSocial.PortalDocente
{
    public partial class Alumnos : System.Web.UI.Page
    {
        private IUserSession userSession;
        private IRedirector redirector;
        private readonly IDataContext dctx = ConnectionHelper.Default.Connection;

        public Alumnos()
        {
            userSession = new UserSession();
            redirector = new Redirector();

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (userSession.IsLogin())
                        if (!userSession.IsAlumno() && userSession.CurrentEscuela != null)
                        {


                            Int64 grupoSocialID = 0;
                            int index = 0;
                            #region *** validaciones del grupo que llega del request ***

                            string gsocial = Request.QueryString["gs"];
                            gsocial = string.IsNullOrEmpty(gsocial) ? "0" : gsocial;

                            if (!Int64.TryParse(gsocial, out grupoSocialID))
                            {
                                redirector.GoToHomePage(false);
                                return;
                            }
                            #endregion

                            GrupoSocialCtrl grupoSocialCtrl = new GrupoSocialCtrl();

                            List<GrupoSocial> gruposSociales = grupoSocialCtrl.RetrieveGruposSocialSocialHub(dctx, userSession.SocialHub);

                            if (grupoSocialID <= 0) //si el id del grupo social no llega se carga el grupo por default del docente
                            {
                                userSession.CurrentGrupoSocial = gruposSociales.First();
                            }
                            else //si llega, se busca si el grupo es parte de mis grupos como docente
                            {
                                foreach (GrupoSocial gs in gruposSociales)
                                {
                                    if (gs.GrupoSocialID == grupoSocialID)
                                    {
                                        userSession.CurrentGrupoSocial = gs;
                                        break;
                                    }
                                    index++;
                                }
                            }

                            ((PortalDocente)this.Master).SelectGrupoSocial(index);

                            if (userSession.CurrentGrupoSocial == null) //no se encontro ningun grupo, lo redirigimos al inicio.
                                redirector.GoToHomePage(false);
                            else
                                LblNombreGrupo.Text = userSession.CurrentGrupoSocial.Nombre;
                        }
                        else
                            redirector.GoToHomePage(false);
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