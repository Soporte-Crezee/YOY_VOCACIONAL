using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POV.Web.Portal.Pruebas
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect(GetLoginURL());
        }

        private String GetLoginURL()
        {
            string ruta = Server.MapPath("~/redireccion.txt");
            StreamReader objReader = new StreamReader(ruta);
            string sLine = "";
            ArrayList arrText = new ArrayList();
            while (sLine != null)
            {
                sLine = objReader.ReadLine();
                if (sLine != null)
                    arrText.Add(sLine);
            }
            string dominio = "";
            dominio = String.Format("{0}://{1}:{2}/", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port);
            var pathBase = dominio + (string)arrText[0];

            return pathBase;
        }
    }
}