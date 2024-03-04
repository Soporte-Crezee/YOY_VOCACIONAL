using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
namespace POV.Web.PortalSocial.Files
{
    public partial class ResourceProxy : System.Web.UI.Page
    {

        private const string URL_CARPETA_REACTIVOS = "~/Files/Reactivos/";
        protected void Page_Load(object sender, EventArgs e)
        {
            string resource_url = Request.QueryString["url"];
            string html = "<html><head></head><body><div id='reactivo-contenido' class='reactivo_column_left1 box_style_reactivo_element' style='float:none'>El recurso no se encuentra disponible en estos momentos. Por favor, intente de nuevo en unos minutos.</div></body></html>";
                    
            if (!string.IsNullOrEmpty(resource_url))
            {
                string url = string.Empty;
                if (resource_url.Contains("http://") || resource_url.Contains("https://"))
                {
                    url = HttpUtility.UrlDecode(resource_url);
                }
                else
                {
                    string root = Request.Url.OriginalString;
                    root = root.Replace(Request.Url.PathAndQuery, string.Empty);

                    url = root + VirtualPathUtility.ToAbsolute(URL_CARPETA_REACTIVOS + resource_url);

                    
                }
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    if (response.StatusCode.ToString().ToLower() == "ok")
                    {
                        string contentType = response.ContentType;
                        Stream content = response.GetResponseStream();
                        StreamReader contentReader = new StreamReader(content);
                        Response.ContentType = contentType;
                        Response.Write(contentReader.ReadToEnd());
                        Response.End();
                        return;
                    }
                    else
                    {
                        Response.Write(html);
                        Response.End();
                        return;
                    }
                }
                catch (WebException ex)
                {
                    Response.Write(html);
                    Response.End();
                    return;
                }

            }
            else
            {
                Response.Write(html);
                Response.End();
                return;
            }
        }
    }
}