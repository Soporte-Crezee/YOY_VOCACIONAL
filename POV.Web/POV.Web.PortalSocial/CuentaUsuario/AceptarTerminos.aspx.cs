using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.Core.RedSocial.Interfaces;
using POV.Core.RedSocial.Implement;
using Framework.Base.DataAccess;
using POV.Web.PortalSocial.AppCode;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.Seguridad.Utils;
using POV.Logger.Service;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;

namespace POV.Web.PortalSocial.CuentaUsuario
{
    public partial class AceptarTerminos : System.Web.UI.Page
    {
        private IUserSession userSession;
        private IRedirector redirector;
        private readonly IDataContext dctx = ConnectionHelper.Default.Connection;
        private UsuarioCtrl usuarioCtrl;
        private AccountService accountService;
        private AlumnoCtrl alumnoCtrl;
        private DocenteCtrl docenteCtrl;

        public AceptarTerminos()
        {
            userSession = new UserSession();
            redirector = new Redirector();
            accountService = new AccountService();
            usuarioCtrl = new UsuarioCtrl();
            alumnoCtrl = new AlumnoCtrl();
            docenteCtrl = new DocenteCtrl();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (userSession.CurrentUser != null && userSession.LoggedIn)
                {
                    if (userSession.IsAlumno())
                    {
                        Alumno alumno = alumnoCtrl.LastDataRowToAlumno(alumnoCtrl.Retrieve(dctx, new Alumno { AlumnoID = userSession.CurrentAlumno.AlumnoID }));
                        if (!alumno.EstatusIdentificacion.Value)
                            redirector.GoToConfirmarAlumno(true);
                    }
                    else
                    {
                        if (userSession.IsDocente()) 
                        {
                             Docente docente = docenteCtrl.LastDataRowToDocente(docenteCtrl.Retrieve(dctx, new Docente { DocenteID = userSession.CurrentDocente.DocenteID }));
                             if (!docente.EstatusIdentificacion.Value)
                                 redirector.GoToConfirmarMaestro(true);
                             
                        }
                    }

                    Usuario usuario = usuarioCtrl.RetrieveComplete(dctx, new Usuario { UsuarioID = userSession.CurrentUser.UsuarioID });
                    if ((bool)usuario.AceptoTerminos)
                    {
                        if (userSession.IsAlumno() && userSession.CurrentAlumno.NivelEscolar == ENivelEscolar.Superior)
                        {
                            Response.Write("<script>window.location='ActivarUsuario.aspx';</script>");
                        }
                        else
                        {
                            redirector.GoToCambiarEscuela(true);
                        }
                    }
                    else
                    {
                        hdnRefTerminos.Value = usuario.Termino.Cuerpo;
                    }

                }
                else
                {
                    redirector.GoToLoginPage(true);
                }
            }
        }

        protected void BtnAceptarTerminos_OnClick(object sender, EventArgs e)
        {
            bool isOk = false;
            try
            {
                Usuario usuario = usuarioCtrl.RetrieveComplete(dctx, new Usuario { UsuarioID = userSession.CurrentUser.UsuarioID });
                Usuario copyUsuario = (Usuario)usuario.Clone();
                copyUsuario.AceptoTerminos = true;

                usuarioCtrl.Update(dctx, copyUsuario, usuario);


                userSession.CurrentUser = usuarioCtrl.RetrieveComplete(dctx, new Usuario { UsuarioID = userSession.CurrentUser.UsuarioID });

                isOk = true;
            }
            catch (Exception ex)
            {
                LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
            if (isOk)
            {
                if (userSession.IsAlumno())
                {
                    if (userSession.CurrentAlumno.NivelEscolar == ENivelEscolar.Superior)
                    {
                        Response.Write("<script>window.location='ActivarUsuario.aspx';</script>");
                    }
                }
                else
                {
                    redirector.GoToCambiarEscuela(true);
                }
            }


        }

        protected void BtnRechazarTerminos_OnClick(object sender, EventArgs e)
        {
            accountService.Logout();
        }
    }
}