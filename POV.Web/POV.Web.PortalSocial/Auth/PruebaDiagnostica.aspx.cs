using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.Core.RedSocial.Implement;
using POV.Core.RedSocial.Interfaces;
using POV.Prueba.BO;

namespace POV.Web.PortalSocial.Auth
{
    public partial class PruebaDiagnostica : System.Web.UI.Page
    {
      private IRedirector redirector;
      private IUserSession userSesion;

      public PruebaDiagnostica()
      {
          this.userSesion = new UserSession();
          redirector = new Redirector();
      }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (userSesion.IsLogin())
                {
                    redireccionar();
                }
                else
                    redirector.GoToLoginPage(true);
            }  
            
        }
        /// <summary>
        /// Redirecciona a alguna pagina configurada a ciertos parámetros
        /// al grupo
        /// </summary>
        public void redireccionar()
        {
            if (Session["UrlRedireccion"] == null)
            {
                throw new Exception("No se puede acceder a esta página");
            }
            else
            {
                string redireccion = ObtenerUrl();
                url.Value = redireccion;
                Session.Clear();
            }
        }

        private string ObtenerUrl()
        {
            string dominio = "";
            dominio = String.Format("{0}://{1}:{2}/", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port);
            string ruta = Server.MapPath("~/redireccion.txt");
            StreamReader objReader = new StreamReader(ruta);
            string sLine = "";
            List<string> arrText = new List<string>();
            while (sLine != null)
            {
                sLine = objReader.ReadLine();
                if (sLine != null)
                    arrText.Add(sLine);
            }

            string portal = string.Empty;
            APrueba prueba = (APrueba)Session["PruebaPendiente"];

            if (prueba.TipoPrueba == ETipoPrueba.Dinamica || prueba.TipoPrueba ==ETipoPrueba.Estandarizada)
                portal = arrText[2];


            string redireccion = dominio + portal + (string)Session["UrlRedireccion"];
            return redireccion;
        }
        protected void BtnEntrar_Click(object sender, ImageClickEventArgs e)
        {

        }
    }
}