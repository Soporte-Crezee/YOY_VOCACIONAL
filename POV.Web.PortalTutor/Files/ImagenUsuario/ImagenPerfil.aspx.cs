using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.DataAccess;
using System.Net;
using System.IO;
using POV.Comun.BO;
using POV.Comun.Service;
using GP.SocialEngine.BO;
using GP.SocialEngine.Service;
using System.Configuration;
using POV.Core.PadreTutor.Interfaces;
using POV.Core.PadreTutor.Implements;

namespace POV.Web.PortalTutor.Files.ImagenUsuario
{
    public partial class ImagenPerfil : System.Web.UI.Page
    {
        readonly IDataContext dctx = new DataContext((new DataProviderFactory()).GetDataProvider("POV"));
        private IUserSession userSession;
        private IAccountService accountService;
        private IRedirector redirector;
        private int profileId;
        private long usuarioID;
        private string defaultImage;

        
        public  ImagenPerfil()
        {
            userSession = new UserSession();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
            string imgType = Request.QueryString["img"];
            string userid = Request.QueryString["usr"];
            StringBuilder sUri = new StringBuilder();
            string uri = "";
            string uriUnknown = "~/Files/ImagenUsuario/UnknownProfile.png";
            string uriUnknownThumb = "~/Files/ImagenUsuario/UnknownProfile_thumb.png";

            string normalPath = ConfigurationManager.AppSettings["POVImagenesNormal"];
            string thumbPath = ConfigurationManager.AppSettings["POVImagenesThumb"];


            long userID;
            if (!string.IsNullOrEmpty(imgType) && !string.IsNullOrEmpty(userid) && long.TryParse(userid, out userID))
            {
                ImagenPerfilCtrl imagenPerfilCtrl = new ImagenPerfilCtrl();
                GP.SocialEngine.BO.ImagenPerfil imagenPerfil = new GP.SocialEngine.BO.ImagenPerfil();
                imagenPerfil.AdjuntoImagen = new AdjuntoImagen();
            }
            else
            {
                if (imgType.CompareTo("thumb") == 0)
                {
                    sUri.Append(uriUnknownThumb);

                }
                if (imgType.CompareTo("normal") == 0)
                {
                    sUri.Append(uriUnknown);
                }
            }

            System.Web.HttpContext.Current.Response.AddHeader("Pragma", "no-cache");
            System.Web.HttpContext.Current.Response.AddHeader("Pragma", "no-store");
            System.Web.HttpContext.Current.Response.Cache.SetNoStore();
            Random rand = new Random((int)DateTime.Now.Ticks);
            System.Web.HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            System.Web.HttpContext.Current.Response.Redirect(sUri.ToString() + "?rand=" + rand.Next(1, 100), true);
            
        }

        }
    }
