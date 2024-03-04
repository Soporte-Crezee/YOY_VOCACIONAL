using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.Core.Administracion.Implements;
using POV.Core.Administracion.Interfaces;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Licencias.Service;
using POV.Licencias.BO;
using Framework.Base.DataAccess;
using POV.Web.Administracion.Helper;
using POV.Seguridad.BO;
using POV.Seguridad.Service;

namespace POV.Web.Administracion
{
    public partial class SeleccionarEscuela : System.Web.UI.Page
    {
        private IUserSession userSession;
        private IRedirector redirector;
        private EscuelaCtrl escuelaCtrl;
        private IDataContext dctx = ConnectionHlp.Default.Connection;
        private DirectorCtrl directorCtrl;
        private CicloEscolarCtrl cicloEscolarCtrl;
        private LicenciaEscuelaCtrl licenciaEscuelaCtrl;

        public SeleccionarEscuela()
        {
            userSession = new UserSession();
            redirector = new Redirector();
            escuelaCtrl = new EscuelaCtrl();
            directorCtrl = new DirectorCtrl();
            cicloEscolarCtrl = new CicloEscolarCtrl();
            licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (userSession.IsLogin() && userSession.LicenciasDirector != null && userSession.LicenciasDirector.Count > 0)
                {
                    if (userSession.LicenciasDirector.Count == 1)
                    {
                        SeleccionarEscuelaActual(userSession.LicenciasDirector.First());
                        return;
                    }

                    DataTable dtEscuelas = new DataTable();

                    dtEscuelas.Columns.Add("LicenciaEscuelaID", typeof(string));
                    dtEscuelas.Columns.Add("NombreEscuela", typeof(string));
                    dtEscuelas.Columns.Add("Turno", typeof(string));


                    foreach (LicenciaEscuela licenciaEscuela in userSession.LicenciasDirector)
                    {
                        Escuela escuela = escuelaCtrl.LastDataRowToEscuela(escuelaCtrl.Retrieve(dctx, licenciaEscuela.Escuela));
                        DataRow dr = dtEscuelas.NewRow();
                        dr[0] = licenciaEscuela.LicenciaEscuelaID.ToString();
                        dr[1] = escuela.NombreEscuela;
                        dr[2] = escuela.Turno.ToString();
                        dtEscuelas.Rows.Add(dr);
                    }

                    RptEscuelas.DataSource = dtEscuelas;
                    RptEscuelas.DataBind();
                }
                else
                    redirector.GoToLoginPage(true);
            }
        }

        protected void Escuelas_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "SELECT_LICENCIA")
            {
                long licenciaEscuelaID;

                if (long.TryParse(e.CommandArgument.ToString(), out licenciaEscuelaID))
                {
                    LicenciaEscuela licenciaEscuela = userSession.LicenciasDirector.First(item => item.LicenciaEscuelaID == licenciaEscuelaID);

                    SeleccionarEscuelaActual(licenciaEscuela);
                }
            }
        }

        /// <summary>
        /// Selecciona una escuela para iniciar en el portal
        /// </summary>
        /// <param name="licenciaEscuela"></param>
        private void SeleccionarEscuelaActual(LicenciaEscuela licenciaEscuela)
        {
            LicenciaDirector licenciaDirector = (LicenciaDirector)licenciaEscuela.ListaLicencia.First(item => item.Tipo == ETipoLicencia.DIRECTOR);
            userSession.CurrentDirector = directorCtrl.LastDataRowToDirector(directorCtrl.Retrieve(dctx, licenciaDirector.Director));
            userSession.CurrentEscuela = escuelaCtrl.LastDataRowToEscuela(escuelaCtrl.Retrieve(dctx, licenciaEscuela.Escuela));
            userSession.CurrentCicloEscolar = cicloEscolarCtrl.LastDataRowToCicloEscolar(cicloEscolarCtrl.Retrieve(dctx, licenciaEscuela.CicloEscolar));
            var licenciaCurrenEscuela = userSession.LicenciasDirector.Find(x => x.Escuela.EscuelaID == userSession.CurrentEscuela.EscuelaID);
            userSession.Contrato = licenciaCurrenEscuela.Contrato;
            

            UsuarioPrivilegiosCtrl usuarioPrivilegiosCtrl = new UsuarioPrivilegiosCtrl();
            UsuarioPrivilegios usuarioPrivilegios = new UsuarioEscolarPrivilegios();
            usuarioPrivilegios.Usuario = userSession.CurrentUser;
            (usuarioPrivilegios as UsuarioEscolarPrivilegios).Escuela = licenciaEscuela.Escuela;
            (usuarioPrivilegios as UsuarioEscolarPrivilegios).CicloEscolar = licenciaEscuela.CicloEscolar;
            usuarioPrivilegios = usuarioPrivilegiosCtrl.RetrieveComplete(dctx, usuarioPrivilegios);
            userSession.CurrentPrivilegiosDirector = usuarioPrivilegios;
            userSession.ModulosFuncionales = licenciaEscuelaCtrl.RetrieveModulosFuncionalesLicenciaEscuela(dctx,
                licenciaCurrenEscuela);
            redirector.GoToHomePage(true);

        }

        protected void Escuelas_DataBound(object sender, RepeaterItemEventArgs e)
        {
            
        }
    }
}