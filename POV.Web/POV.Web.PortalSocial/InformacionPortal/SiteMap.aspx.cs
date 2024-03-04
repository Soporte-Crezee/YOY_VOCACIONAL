//Mapa del sitio
using System;
using System.Xml;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using POV.Core.RedSocial.Implement;
using POV.Core.RedSocial.Interfaces;

namespace POV.Web.PortalSocial.InformacionPortal
{
    public partial class SiteMap : System.Web.UI.Page
    {
        private IUserSession userSession;
        protected void Page_Load(object sender, EventArgs e)
        {
                userSession = new UserSession();
                string nombreBoton = string.Empty;

                if (userSession.IsLogin())
                {
                    XmlDocument xDoc = new XmlDocument();

                    if (userSession.IsAlumno())
                    {
                        xDoc.Load(HttpContext.Current.Request.Url.Scheme + "://" +
                                  HttpContext.Current.Request.Url.Authority +
                                  HttpContext.Current.Request.ApplicationPath + "../InformacionPortal/SiteMapAlumno.xml");
                    }

                    else
                    {
                        xDoc.Load(HttpContext.Current.Request.Url.Scheme + "://" +
                                  HttpContext.Current.Request.Url.Authority +
                                  HttpContext.Current.Request.ApplicationPath +
                                  "../InformacionPortal/SiteMapDocente.xml");
                    }

                    XmlNodeList xml = xDoc.GetElementsByTagName("urlset");

                    XmlNodeList lista =
                        ((XmlElement) xml[0]).GetElementsByTagName("url");
                    int i = 0;

                    foreach (XmlElement nodoPadre in lista)
                    {

                        XmlNodeList nombre =
                            nodoPadre.GetElementsByTagName("name");
                        XmlNodeList link =
                            nodoPadre.GetElementsByTagName("loc");
                        XmlNodeList descripcion =
                            nodoPadre.GetElementsByTagName("descripcion");
                        XmlNodeList seccion =
                            nodoPadre.GetElementsByTagName("seccion");
                        XmlNodeList imagen = 
                            nodoPadre.GetElementsByTagName("imagen");

                        if (seccion[0].InnerText == "")
                        {
                            HtmlGenericControl br = new HtmlGenericControl();
                            HtmlGenericControl li = new HtmlGenericControl();
                            HtmlGenericControl ul = new HtmlGenericControl();
                            HtmlGenericControl a = new HtmlGenericControl();
                            HiddenField hdnLink = new HiddenField();
                            HiddenField hdnDescripcion = new HiddenField();
                            HiddenField hdnImagen = new HiddenField();
                            br.TagName = "br";
                            li.TagName = "li";
                            ul.TagName = "ul";
                            a.TagName = "a";
                            a.InnerText = nombre[0].InnerText;
                            a.Attributes.Add("href", "#");
                           
                            if (link[0].InnerText != "")
                            {
                                hdnLink.Value = VirtualPathUtility.ToAbsolute(link[0].InnerText);
                            }
                            if (imagen[0].InnerText != "")
                            {
                                hdnImagen.Value = VirtualPathUtility.ToAbsolute(imagen[0].InnerText);
                            }
                            hdnDescripcion.Value = descripcion[0].InnerText;

                            li.Controls.Add(a);
                            hdnDescripcion.ID = "Pdescripcion" + i;
                            hdnLink.ID = "Plink" + i;
                            hdnImagen.ID = "PImagen" + i;
                           
                            foreach (XmlElement nodoHijo in lista)
                            {
                                int x = 0;
                                XmlNodeList nombreHijo =
                                    nodoHijo.GetElementsByTagName("name");
                                XmlNodeList linkHijo =
                                    nodoHijo.GetElementsByTagName("loc");
                                XmlNodeList descripcionHijo =
                                    nodoHijo.GetElementsByTagName("descripcion");
                                XmlNodeList seccionHijo =
                                    nodoHijo.GetElementsByTagName("seccion");
                                XmlNodeList imagen2 = 
                                    nodoHijo.GetElementsByTagName("imagen");

                                if (seccionHijo[0].InnerText == nombre[0].InnerText)
                                {
                                    HiddenField hdnLinkHijo = new HiddenField();
                                    HiddenField hdnDescripcionHijo = new HiddenField();
                                    HiddenField hdnImagenHijo = new HiddenField();
                                    HtmlGenericControl liHijo = new HtmlGenericControl();
                                    HtmlGenericControl aHijo = new HtmlGenericControl();
                                    liHijo.TagName = "li";
                                    aHijo.TagName = "a";
                                    aHijo.InnerText = nombreHijo[0].InnerText;
                                    aHijo.Attributes.Add("href", "#");
                                    hdnDescripcionHijo.Value = descripcionHijo[0].InnerText;
                                    
                                    if (linkHijo[0].InnerText!="")
                                    {
                                        hdnLinkHijo.Value = VirtualPathUtility.ToAbsolute(linkHijo[0].InnerText);
                                    }
                                    if (imagen2[0].InnerText != "")
                                    {
                                        hdnImagenHijo.Value = VirtualPathUtility.ToAbsolute(imagen2[0].InnerText);
                                    }
                                    hdnLinkHijo.ID = "Clink" + i;
                                    hdnDescripcionHijo.ID = "Cdescripcion" + i;
                                    hdnImagenHijo.ID = "CImagen" + i;
                                    liHijo.Controls.Add(aHijo);
                                    liHijo.Controls.Add(hdnLinkHijo);
                                    liHijo.Controls.Add(hdnDescripcionHijo);
                                    liHijo.Controls.Add(hdnImagenHijo);
                                    liHijo.ID = "liAnidada" + x;
                                    ul.Controls.Add(liHijo); 
                                }
                                x++;
                            }

                            li.Controls.Add(ul);                            
                            li.Controls.Add(hdnLink);
                            li.Controls.Add(hdnDescripcion);
                            li.Controls.Add(hdnImagen);
                            li.ID = "liPadre" + i;
                            linkList.Controls.Add(li);
                            i++;
                        }
                    }
                }
            }
        }
    }