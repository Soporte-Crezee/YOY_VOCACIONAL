using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Core.RedSocial.Implement;
using POV.Core.RedSocial.Interfaces;
using POV.Logger.Service;
using POV.Web.Administracion.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.Modelo.BO;
using System.Collections;
using POV.Licencias.BO;
using POV.Licencias.Service;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Expediente.Service;
using POV.Expediente.BO;
using Framework.Base.DataAccess;
using POV.Web.PortalSocial.AppCode;
using System.Globalization;
using POV.Blog.Services;
using POV.Blog.BO;
using System.Text.RegularExpressions;
using Framework.Base.Exceptions;

namespace POV.Web.PortalSocial.PortalAlumno
{
    public partial class ViewBlog : System.Web.UI.Page
    {
        private IUserSession userSession;
        private IUserSession alumnoPrevious;
        private IRedirector redirector;
        public bool estatusIdentificacion;
        public bool correoConfirmado;
        private AlumnoCtrl alumnoCtrl;
        private Alumno alumno;
        private PostBlogEngineCtrl postBlogEngineCtrl;
        private PostBlogEngine postBlogEngine;
        private readonly IDataContext dctx = ConnectionHelper.Default.Connection;
        private PostFavoritoCtrl postFavoritoCtrl;
        private PostFavorito postFavorito;
        private string areaConocimiento = string.Empty;

        #region QueryString
        private string QS_AreaConocimientoBlog
        {
            get { return this.Request.QueryString["AreaConocimientoBlog"]; }
        }
        private string QS_Redirect
        {
            get { return this.Request.QueryString["Redirect"]; }
        }
        private string QS_Search
        {
            get { return this.Request.QueryString["Search"]; }
        }
        #endregion

        #region Session_variables
        private string session_FechaInicio
        {
            get
            {
                return Session["session_FechaInicio"] as string;
            }
            set
            {
                Session["session_FechaInicio"] = value;
            }
        }

        private string session_FechaFin
        {
            get
            {
                return Session["session_FechaFin"] as string;
            }
            set
            {
                Session["session_FechaFin"] = value;
            }
        }
        #endregion

        public ViewBlog()
        {
            userSession = new UserSession();
            redirector = new Redirector();
            alumnoPrevious = new UserSession();
            alumnoCtrl = new AlumnoCtrl();
            alumno = new Alumno();
            postBlogEngineCtrl = new PostBlogEngineCtrl(null);
            postBlogEngine = new PostBlogEngine();
            postFavoritoCtrl = new PostFavoritoCtrl(null);
            postFavorito = new PostFavorito();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (userSession.IsLogin() && userSession.IsAlumno())
                    {
                        alumno = getDataAlumnoToObject();
                        if ((bool)alumno.CorreoConfirmado)
                        {
                            ((PortalAlumno)this.Master).LoadControlsAlumnoMaster((long)userSession.CurrentUsuarioSocial.UsuarioSocialID, false);
                            ((PortalAlumno)this.Master).divCssClass = "col-xs-12 col-md-9";

                            this.hdnSessionSocialHubID.Value = userSession.SocialHub.SocialHubID.ToString();
                            this.hdnSessionUsuarioSocialID.Value = userSession.CurrentUsuarioSocial.UsuarioSocialID.ToString();
                            hdnSocialHubID.Value = userSession.SocialHub.SocialHubID.ToString();
                            hdnUsuarioSocialID.Value = userSession.CurrentUsuarioSocial.UsuarioSocialID.ToString();

                            

                            if (!string.IsNullOrEmpty(QS_AreaConocimientoBlog))
                            {
                                blogIframe.Visible = false;
                                postIframe.Visible = false;
                                string hrefFavoritos = System.Configuration.ConfigurationManager.AppSettings["POVUrlHrefFavoritos"];
                                if (QS_AreaConocimientoBlog == hrefFavoritos && string.IsNullOrEmpty(QS_Redirect))
                                {
                                    showPostsFavoritos();
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(QS_Redirect))
                                    {
                                        postsList.Visible = false;
                                        showPost();
                                    }
                                    else showPosts();
                                }
                            }
                            else
                            {
                                postsList.Visible = false;
                                postIframe.Visible = false;
                            }
                        }
                        else
                        {
                            redirector.GoToHomeAlumno(true);
                        }
                    }
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

        private void showPost()
        {
            listPosts.Visible = false;
            postIframe.Visible = true;
            emptyPosts.Visible = false;
            string src = postView.Src.ToString();
            string href = backPosts.HRef.ToString();

            postView.Src = src + QS_Redirect;

            if (!string.IsNullOrEmpty(session_FechaInicio) || !string.IsNullOrEmpty(session_FechaFin))
                backPosts.HRef = href + QS_AreaConocimientoBlog + "&Search=true";
            else
                backPosts.HRef = href + QS_AreaConocimientoBlog;
        }

        private void showPosts()
        {
            var areasConocimiento = ((Site)this.Master.Master).GetAreasConocimientoAlumno();

            if (!string.IsNullOrEmpty(QS_Search) && QS_Search == "true")
            {
                if (!string.IsNullOrEmpty(session_FechaInicio))
                    txtFechaInicio.Text = session_FechaInicio;

                if (!string.IsNullOrEmpty(session_FechaFin))
                    txtFechaFin.Text = session_FechaFin;
            }
            else
            {
                if (string.IsNullOrEmpty(txtFechaInicio.Text))
                    session_FechaInicio = string.Empty;
                if (string.IsNullOrEmpty(txtFechaFin.Text))
                    session_FechaFin = string.Empty;
            }

            foreach (var item in areasConocimiento)
            {
                string QSareaConocimiento = item.Nombre.Replace(" ", "");
                QSareaConocimiento = ((Site)this.Master.Master).QuitarAcentos(QSareaConocimiento);

                if (QS_AreaConocimientoBlog == QSareaConocimiento)
                {
                    areaConocimiento = item.Nombre;
                    break;
                }
            }
            string urlPost = System.Configuration.ConfigurationManager.AppSettings["POVUrlBlogPost"];

            postBlogEngine.Categoria = areaConocimiento;
            postFavorito.AlumnoId = userSession.CurrentAlumno.AlumnoID;

            var posts = postBlogEngineCtrl.Retrieve(postBlogEngine, false);
            var postFavoritos = postFavoritoCtrl.Retrieve(postFavorito, false);
            string msgErrorFecha = string.Empty;
            if (!string.IsNullOrEmpty(txtFechaInicio.Text) || !string.IsNullOrEmpty(txtFechaFin.Text))
            {
                msgErrorFecha = validaFechas();
                string fechaInicio = string.Empty;
                string fechaFin = string.Empty;

                if ((!string.IsNullOrEmpty(session_FechaInicio) && !string.IsNullOrEmpty(msgErrorFecha)) || !string.IsNullOrEmpty(msgErrorFecha))
                    fechaInicio = session_FechaInicio;
                else
                    fechaInicio = txtFechaInicio.Text;

                if ((!string.IsNullOrEmpty(session_FechaFin) && !string.IsNullOrEmpty(msgErrorFecha)) || !string.IsNullOrEmpty(msgErrorFecha))
                    fechaFin = session_FechaFin;
                else
                    fechaFin = txtFechaFin.Text;


                DateTime dtFechaInicio = Convert.ToDateTime(null);
                DateTime dtFechaFin = Convert.ToDateTime(null);

                if (string.IsNullOrEmpty(msgErrorFecha))
                {

                    if (!string.IsNullOrEmpty(fechaInicio))
                    {
                        dtFechaInicio = Convert.ToDateTime(fechaInicio);
                        session_FechaInicio = fechaInicio;
                    }
                    else { session_FechaInicio = string.Empty; }

                    if (!string.IsNullOrEmpty(fechaFin))
                    {
                        dtFechaFin = Convert.ToDateTime(fechaFin);
                        session_FechaFin = fechaFin;
                    }
                    else { session_FechaFin = string.Empty; }
                }
                else
                {
                    if (!string.IsNullOrEmpty(session_FechaInicio))
                        dtFechaInicio = Convert.ToDateTime(session_FechaInicio);
                    if (!string.IsNullOrEmpty(session_FechaFin))
                        dtFechaFin = Convert.ToDateTime(session_FechaFin);
                }

                //Filtrar de la fecha de inicio en adelante (>FechaInicio)

                if (!string.IsNullOrEmpty(session_FechaInicio) || !string.IsNullOrEmpty(session_FechaFin))
                {
                    if (dtFechaInicio != null && string.IsNullOrEmpty(fechaFin))
                        posts = (from p in posts
                                 where Convert.ToDateTime(String.Format("{0:dd MM yyyy}", p.DateCreated)) >= dtFechaInicio
                                 select p).ToList();

                    //Filrar hasta la fecha fin (<FechaFin)
                    if (dtFechaFin != null && string.IsNullOrEmpty(fechaInicio))
                        posts = (from p in posts
                                 where Convert.ToDateTime(String.Format("{0:dd MM yyyy}", p.DateCreated)) <= dtFechaFin
                                 select p).ToList();

                    //Filtar por el rango de fechas establecido (between fechainicio and fechafin)
                    if (!string.IsNullOrEmpty(fechaInicio) && !string.IsNullOrEmpty(fechaFin))
                        posts = (from p in posts
                                 where Convert.ToDateTime(String.Format("{0:dd MM yyyy}", p.DateCreated)) >= dtFechaInicio
                                 where Convert.ToDateTime(String.Format("{0:dd MM yyyy}", p.DateCreated)) <= dtFechaFin
                                 select p).ToList();
                }

            }

            posts = posts.OrderByDescending(x => x.DateCreated).ToList();
            if (posts.Count > 0)
            {
                emptyPosts.Visible = false;
                listPosts.Visible = true;

                PrintPosts(posts, postFavoritos, false);
            }

            else
            {
                listPosts.Visible = false;
                emptyPosts.Visible = true;
            }
            if (msgErrorFecha != String.Empty)
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "showError('" + msgErrorFecha + "');", true);
        }

        private void showPostsFavoritos()
        {
            string urlPost = System.Configuration.ConfigurationManager.AppSettings["POVUrlBlogPost"];

            postFavorito.AlumnoId = userSession.CurrentAlumno.AlumnoID;

            var posts = postBlogEngineCtrl.Retrieve(new PostBlogEngine(), false);
            var postFavoritos = postFavoritoCtrl.Retrieve(postFavorito, false).ToList();

            if (posts.Count > 0)
            {
                filterPosts.Visible = false;
                emptyPosts.Visible = false;
                listPosts.Visible = true;


                //Crea una nueva lista con los items contenidos en la lista de postsFavoritos
                posts = (from p in posts
                         where
                         ((from pf in postFavoritos
                           select pf.PostId)
                                     .Contains(p.PostId))
                         select p).Distinct().OrderByDescending(x => x.DateCreated).ToList();

                //Crea una lista nueva a partir de la existente y agrega los items que no contengan el mismo PostID a la lista 
                var postFiltrados = posts.Aggregate(new List<PostBlogEngine>(),
                                                    (result, current) =>
                                                    {
                                                        if (!result.Any(x => x.PostId == current.PostId))
                                                        {
                                                            result.Add(current);
                                                        }
                                                        return result;
                                                    });


                if (postFavoritos.Count > 0)
                    PrintPosts(postFiltrados, postFavoritos, true);
            }

            else
            {
                listPosts.Visible = false;
                emptyPosts.Visible = true;
            }
        }

        private void PrintPosts(List<PostBlogEngine> posts, List<PostFavorito> postFavoritos, bool favorites)
        {
            foreach (var post in posts)
            {
                string favorite = "0";
                DateTime dateCreated = Convert.ToDateTime(post.DateCreated);
                string day = dateCreated.Day.ToString();
                string month = MonthName(dateCreated.Month);
                string year = dateCreated.Year.ToString();
                string fecha = day + " " + month + " " + year;
                string redirect = "?AreaConocimientoBlog=" + QS_AreaConocimientoBlog + "&Redirect=" + post.RutaPost;//urlPost + post.RutaPost;
                string redirectAuthor = "?AreaConocimientoBlog=" + QS_AreaConocimientoBlog + "&Redirect=author/" + post.Author;
                string redirectCategory = "?AreaConocimientoBlog=" + QS_AreaConocimientoBlog + "&Redirect=category/" + post.Categoria.Replace(' ', '-').ToString();
                List<string> categoriasFavorito = new List<string>();

                var listFavoritos = (from p in postFavoritos
                                     where p.PostId == post.PostId
                                     select p).ToList();

                if (listFavoritos.Count >= 1)
                {
                    if (listFavoritos[0].Categorias.ToLower() == areaConocimiento.ToLower())
                    {
                        favorite = "1";
                    }
                    if (favorites)
                    {
                        favorite = "1";
                        categoriasFavorito = listFavoritos[0].Categorias.ToString().Split(',').ToList();
                    }
                }

                if (favorites && favorite == "0") continue;

                string literalControl = @"
                            <div class='col-xs-12 col-md-12 espacio_opcion post-card'>
				                <div class='card card-block box_shadow'>
					                <div id='post" + post.PostId + @"' class='post post-home'>
						                <header class='post-header'>
							                <div class='pull-right'>
								                <input id='post_" + post.BlogId + "_" + post.PostId + "_" + post.Categoria + @"' value='" + favorite + @"' type='number' class='rating' min=0 max=1 step=1 data-size='md' data-stars='1' style='display:none' >										
							                </div> 
							                <h2 class='post-title'>
								                <a href='" + redirect + @"'>" + post.Title + @"</a>
							                </h2> 							
							                <div class='post-info clearfix'>
								                <span class='post-date'><i class='icon-calendar'></i>" + fecha + @"</span>
								                <span class='post-author'><i class=' icon-user'></i><a href='" + redirectAuthor + @"'>" + post.Author + @"</a></span>
                                                <span class='post-category'><i class=' icon-folder'></i>";
                if (favorites && categoriasFavorito.Count >= 1)
                {
                    int cont = 1;
                    foreach (var item in categoriasFavorito)
                    {
                        redirectCategory = "?AreaConocimientoBlog=" + QS_AreaConocimientoBlog + "&Redirect=category/" + item.Trim().Replace(' ', '-').ToString();
                        literalControl += @"<a href='" + redirectCategory + @"'>" + item + @"</a>";
                        if (cont < categoriasFavorito.Count) literalControl += ", ";
                        cont++;
                    }
                }
                else literalControl += @"<a href='" + redirectCategory + @"'>" + post.Categoria + @"</a>";
                literalControl += @"</span></div>
						                </header>
						                <section class='post-body text'>
							                <p>" + post.Description + @"</p>
						                </section>
					                </div>
				                </div>
			                </div>";

                postsList.Controls.Add(new LiteralControl(literalControl));
            }
        }

        public string MonthName(int month)
        {
            DateTimeFormatInfo dtinfo = new CultureInfo("es-ES", false).DateTimeFormat;
            return dtinfo.GetMonthName(month);
        }

        private Alumno getDataAlumnoToObject()
        {

            alumno.AlumnoID = userSession.CurrentAlumno.AlumnoID;
            DataSet ds = alumnoCtrl.Retrieve(ConnectionHlp.Default.Connection, alumno);

            if (ds.Tables[0].Rows.Count == 1)
                alumno = alumnoCtrl.LastDataRowToAlumno(ds);

            return alumno;
        }

        private string validaFechas()
        {
            string res = String.Empty;
            Regex re = new Regex(@"^(0?[1-9]|[12][0-9]|3[01])[\/](0?[1-9]|1[012])[\/](19|20)\d{2}$");
            if (txtFechaInicio.Text != String.Empty && !re.IsMatch(txtFechaInicio.Text))
                res = "La fecha de inicio no tiene el formato correcto";
            if (txtFechaFin.Text != String.Empty && !re.IsMatch(txtFechaFin.Text))
                res = "La fecha Fin No tiene el formato correcto";
            if (txtFechaInicio.Text != string.Empty && txtFechaFin.Text != string.Empty)
            {
                if (DateTime.Parse(txtFechaFin.Text) < DateTime.Parse(txtFechaInicio.Text))
                    res = "La fecha de inicio no puede ser mayor a la fecha fin";
            }
            return res;
        }

        protected void btnfiltrar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFechaInicio.Text))
                session_FechaInicio = string.Empty;
            if (string.IsNullOrEmpty(txtFechaFin.Text))
                session_FechaFin = string.Empty;

            showPosts();
        }
    }
}