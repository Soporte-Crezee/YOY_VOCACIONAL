using Framework.Base.DataAccess;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.CentroEducativo.Services;
using POV.Core.RedSocial.Implement;
using POV.Core.RedSocial.Interfaces;
using POV.Expediente.BO;
using POV.Licencias.BO;
using POV.Licencias.Service;
using POV.Logger.Service;
using POV.Prueba.BO;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Prueba.Diagnostico.Dinamica.Service;
using POV.Prueba.Diagnostico.Service;
using POV.Web.PortalSocial.AppCode;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POV.Web.PortalSocial.PortalAlumno
{
    public partial class Pruebas : System.Web.UI.Page
    {
        #region Propiedades de la clase
        private IUserSession userSession;
        private IRedirector redirector;

        private PruebaDiagnosticoCtrl pruebaDiagnosticoCtrl;
        private PruebaDinamicaCtrl pruebaDinamicaCtrl;
        private RespuestaAlumnoCtrl respuestaAlumnoCtrl;
        private readonly IDataContext dctx = ConnectionHelper.Default.Connection;

        private Alumno alumno;
        #endregion

        public Pruebas()
        {
            userSession = new UserSession();
            redirector = new Redirector();

            pruebaDiagnosticoCtrl = new PruebaDiagnosticoCtrl();
            pruebaDinamicaCtrl = new PruebaDinamicaCtrl();
            respuestaAlumnoCtrl = new RespuestaAlumnoCtrl();

            alumno = new Alumno();
        }

        #region Eventos de la pagina
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (userSession.IsLogin() && userSession.IsAlumno())
                    {
                        bool datosCompletos = false;
                        EFAlumnoCtrl alumnoCtrl = new EFAlumnoCtrl(null);
                        alumno = alumnoCtrl.Retrieve(new Alumno { AlumnoID = userSession.CurrentAlumno.AlumnoID }, false).FirstOrDefault();
                        if (alumno.DatosCompletos != null)
                            datosCompletos = (bool)alumno.DatosCompletos;

                        if (!datosCompletos || !(bool)alumno.CorreoConfirmado)
                            redirector.GoToHomeAlumno(false);
                        else
                            ShowPruebasAlumno();
                    }
                    else
                    {
                        redirector.GoToLoginPage(false);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
        }

        #region Habitos
        protected void imgBtnTomarPruebaHabitos_Click(object sender, EventArgs e)
        {
            if (userSession.IsLogin())
            {

                var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, userSession.Contrato, userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, false).ToList();

                prueba[0] = prueba.FirstOrDefault(itm => itm.PruebaID == 9);

                if (prueba[0] != null)
                {
                    string UrlRedireccion = GenerarTokenYUrl(userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, prueba[0]);
                    Session["UrlRedireccion"] = UrlRedireccion;
                    Session["PruebaPendiente"] = prueba[0];
                    redirector.GoToDiagnostica(true);
                }
            }
            else
            {
                redirector.GoToLoginPage(true);
            }
        }

        protected void imgBtnContinuarHabitos_Click(object sender, EventArgs e)
        {
            if (userSession.IsLogin())
            {

                var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, userSession.Contrato, userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, false).ToList();

                prueba[0] = prueba.FirstOrDefault(itm => itm.PruebaID == 9);

                if (prueba[0] != null)
                {
                    string UrlRedireccion = GenerarTokenYUrl(userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, prueba[0]);
                    Session["UrlRedireccion"] = UrlRedireccion;
                    Session["PruebaPendiente"] = prueba[0];
                    redirector.GoToDiagnostica(true);
                }
            }
            else
            {
                redirector.GoToLoginPage(true);
            }
        }

        protected void imgBtnReporteHabitos_Click(object sender, EventArgs e)
        {
            string href = UrlHelper.GetReporteHabitosURL();
            Response.Redirect(href);
        }
        #endregion

        #region Dominos
        protected void imgBtnTomarPruebaDominos_Click(object sender, EventArgs e)
        {
            if (userSession.IsLogin())
            {

                var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, userSession.Contrato, userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, false).ToList();

                prueba[0] = prueba.FirstOrDefault(itm => itm.PruebaID == 10);

                if (prueba[0] != null)
                {
                    string UrlRedireccion = GenerarTokenYUrl(userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, prueba[0]);
                    Session["UrlRedireccion"] = UrlRedireccion;
                    Session["PruebaPendiente"] = prueba[0];
                    redirector.GoToDiagnostica(true);
                }
            }
            else
            {
                redirector.GoToLoginPage(true);
            }
        }

        protected void imgBtnContinuarDominos_Click(object sender, EventArgs e)
        {
            if (userSession.IsLogin())
            {

                var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, userSession.Contrato, userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, false).ToList();

                prueba[0] = prueba.FirstOrDefault(itm => itm.PruebaID == 10);

                if (prueba[0] != null)
                {
                    string UrlRedireccion = GenerarTokenYUrl(userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, prueba[0]);
                    Session["UrlRedireccion"] = UrlRedireccion;
                    Session["PruebaPendiente"] = prueba[0];
                    redirector.GoToDiagnostica(true);
                }
            }
            else
            {
                redirector.GoToLoginPage(true);
            }
        }

        protected void imgBtnReporteDominos_Click(object sender, EventArgs e)
        {
            string href = UrlHelper.GetReporteDominosURL();
            Response.Redirect(href);
        }
        #endregion

        #region Terman
        protected void imgBtnTomarPruebaTerman_Click(object sender, EventArgs e)
        {
            if (userSession.IsLogin())
            {

                var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, userSession.Contrato, userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, false).ToList();

                prueba[0] = prueba.FirstOrDefault(itm => itm.PruebaID == 11);

                if (prueba[0] != null)
                {
                    string UrlRedireccion = GenerarTokenYUrl(userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, prueba[0]);
                    Session["UrlRedireccion"] = UrlRedireccion;
                    Session["PruebaPendiente"] = prueba[0];
                    redirector.GoToDiagnostica(true);
                }
            }
            else
            {
                redirector.GoToLoginPage(true);
            }
        }

        protected void imgBtnContinuarTerman_Click(object sender, EventArgs e)
        {
            if (userSession.IsLogin())
            {

                var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, userSession.Contrato, userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, false).ToList();

                prueba[0] = prueba.FirstOrDefault(itm => itm.PruebaID == 11);

                if (prueba[0] != null)
                {
                    string UrlRedireccion = GenerarTokenYUrl(userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, prueba[0]);
                    Session["UrlRedireccion"] = UrlRedireccion;
                    Session["PruebaPendiente"] = prueba[0];
                    redirector.GoToDiagnostica(true);
                }
            }
            else
            {
                redirector.GoToLoginPage(true);
            }
        }

        protected void imgBtnReporteTerman_Click(object sender, EventArgs e)
        {
            string href = UrlHelper.GetReporteTermanURL();
            Response.Redirect(href);
        }
        #endregion

        #region Sacks
        protected void imgBtnTomarPruebaSacks_Click(object sender, EventArgs e)
        {
            if (userSession.IsLogin())
            {

                var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, userSession.Contrato, userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, false).ToList();

                prueba[0] = prueba.FirstOrDefault(itm => itm.PruebaID == 12);

                if (prueba[0] != null)
                {
                    string UrlRedireccion = GenerarTokenYUrl(userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, prueba[0]);
                    Session["UrlRedireccion"] = UrlRedireccion;
                    Session["PruebaPendiente"] = prueba[0];
                    redirector.GoToDiagnostica(true);
                }
            }
            else
            {
                redirector.GoToLoginPage(true);
            }
        }

        protected void imgBtnContinuarSacks_Click(object sender, EventArgs e)
        {
            if (userSession.IsLogin())
            {

                var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, userSession.Contrato, userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, false).ToList();

                prueba[0] = prueba.FirstOrDefault(itm => itm.PruebaID == 12);

                if (prueba[0] != null)
                {
                    string UrlRedireccion = GenerarTokenYUrl(userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, prueba[0]);
                    Session["UrlRedireccion"] = UrlRedireccion;
                    Session["PruebaPendiente"] = prueba[0];
                    redirector.GoToDiagnostica(true);
                }
            }
            else
            {
                redirector.GoToLoginPage(true);
            }
        }

        protected void imgBtnReporteSacks_Click(object sender, EventArgs e)
        {
            string href = UrlHelper.GetREporteSACKSURL();
            Response.Redirect(href);
        }
        #endregion

        #region Cleaver
        protected void imgBtnTomarPruebaCleaver_Click(object sender, EventArgs e)
        {
            if (userSession.IsLogin())
            {

                var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, userSession.Contrato, userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, false).ToList();

                prueba[0] = prueba.FirstOrDefault(itm => itm.PruebaID == 14);

                if (prueba[0] != null)
                {
                    string UrlRedireccion = GenerarTokenYUrl(userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, prueba[0]);
                    Session["UrlRedireccion"] = UrlRedireccion;
                    Session["PruebaPendiente"] = prueba[0];
                    redirector.GoToDiagnostica(true);
                }
            }
            else
            {
                redirector.GoToLoginPage(true);
            }
        }

        protected void imgBtnContinuarCleaver_Click(object sender, EventArgs e)
        {
            if (userSession.IsLogin())
            {

                var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, userSession.Contrato, userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, false).ToList();

                prueba[0] = prueba.FirstOrDefault(itm => itm.PruebaID == 14);

                if (prueba[0] != null)
                {
                    string UrlRedireccion = GenerarTokenYUrl(userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, prueba[0]);
                    Session["UrlRedireccion"] = UrlRedireccion;
                    Session["PruebaPendiente"] = prueba[0];
                    redirector.GoToDiagnostica(true);
                }
            }
            else
            {
                redirector.GoToLoginPage(true);
            }
        }

        protected void imgBtnReporteCleaver_Click(object sender, EventArgs e)
        {
            string href = UrlHelper.GetReporteCleaverURL();
            Response.Redirect(href);
        }
        #endregion

        #region Chaside
        protected void imgBtnTomarPruebaChaside_Click(object sender, EventArgs e)
        {
            if (userSession.IsLogin())
            {

                var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, userSession.Contrato, userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, false).ToList();

                prueba[0] = prueba.FirstOrDefault(itm => itm.PruebaID == 17);

                if (prueba[0] != null)
                {
                    string UrlRedireccion = GenerarTokenYUrl(userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, prueba[0]);
                    Session["UrlRedireccion"] = UrlRedireccion;
                    Session["PruebaPendiente"] = prueba[0];
                    redirector.GoToDiagnostica(true);
                }
            }
            else
            {
                redirector.GoToLoginPage(true);
            }
        }

        protected void imgBtnContinuarChaside_Click(object sender, EventArgs e)
        {
            if (userSession.IsLogin())
            {

                var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, userSession.Contrato, userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, false).ToList();

                prueba[0] = prueba.FirstOrDefault(itm => itm.PruebaID == 17);

                if (prueba[0] != null)
                {
                    string UrlRedireccion = GenerarTokenYUrl(userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, prueba[0]);
                    Session["UrlRedireccion"] = UrlRedireccion;
                    Session["PruebaPendiente"] = prueba[0];
                    redirector.GoToDiagnostica(true);
                }
            }
            else
            {
                redirector.GoToLoginPage(true);
            }
        }

        protected void imgBtnReporteChaside_Click(object sender, EventArgs e)
        {
            string href = UrlHelper.GetReporteChasideURL();
            Response.Redirect(href);
        }
        #endregion

        #region Allport
        protected void imgBtnTomarPruebaAllport_Click(object sender, EventArgs e)
        {
            if (userSession.IsLogin())
            {

                var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, userSession.Contrato, userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, false).ToList();

                prueba[0] = prueba.FirstOrDefault(itm => itm.PruebaID == 15);

                if (prueba[0] != null)
                {
                    string UrlRedireccion = GenerarTokenYUrl(userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, prueba[0]);
                    Session["UrlRedireccion"] = UrlRedireccion;
                    Session["PruebaPendiente"] = prueba[0];
                    redirector.GoToDiagnostica(true);
                }
            }
            else
            {
                redirector.GoToLoginPage(true);
            }
        }

        protected void imgBtnContinuarAllport_Click(object sender, EventArgs e)
        {
            if (userSession.IsLogin())
            {

                var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, userSession.Contrato, userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, false).ToList();

                prueba[0] = prueba.FirstOrDefault(itm => itm.PruebaID == 15);

                if (prueba[0] != null)
                {
                    string UrlRedireccion = GenerarTokenYUrl(userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, prueba[0]);
                    Session["UrlRedireccion"] = UrlRedireccion;
                    Session["PruebaPendiente"] = prueba[0];
                    redirector.GoToDiagnostica(true);
                }
            }
            else
            {
                redirector.GoToLoginPage(true);
            }
        }

        protected void imgBtnReporteAllport_Click(object sender, EventArgs e)
        {
            string href = UrlHelper.GetReporteAllportURL();
            Response.Redirect(href);
        }
        #endregion

        #region Kuder
        protected void imgBtnTomarPruebaKuder_Click(object sender, EventArgs e)
        {
            if (userSession.IsLogin())
            {

                var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, userSession.Contrato, userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, false).ToList();

                prueba[0] = prueba.FirstOrDefault(itm => itm.PruebaID == 13);

                if (prueba[0] != null)
                {
                    string UrlRedireccion = GenerarTokenYUrl(userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, prueba[0]);
                    Session["UrlRedireccion"] = UrlRedireccion;
                    Session["PruebaPendiente"] = prueba[0];
                    redirector.GoToDiagnostica(true);
                }
            }
            else
            {
                redirector.GoToLoginPage(true);
            }
        }

        protected void imgBtnContinuarKuder_Click(object sender, ImageClickEventArgs e)
        {
            if (userSession.IsLogin())
            {

                var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, userSession.Contrato, userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, false).ToList();

                prueba[0] = prueba.FirstOrDefault(itm => itm.PruebaID == 13);

                if (prueba[0] != null)
                {
                    string UrlRedireccion = GenerarTokenYUrl(userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, prueba[0]);
                    Session["UrlRedireccion"] = UrlRedireccion;
                    Session["PruebaPendiente"] = prueba[0];
                    redirector.GoToDiagnostica(true);
                }
            }
            else
            {
                redirector.GoToLoginPage(true);
            }
        }

        protected void imgBtnReporteKuder_Click(object sender, ImageClickEventArgs e)
        {
            string href = UrlHelper.GetReporteKuderURL();
            Response.Redirect(href);
        }
        #endregion

        #region Rotter
        protected void imgBtnTomarPruebaRotter_Click(object sender, ImageClickEventArgs e)
        {
            if (userSession.IsLogin())
            {
                var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, userSession.Contrato, userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, false).ToList();
                prueba[0] = prueba.FirstOrDefault(itm => itm.PruebaID == 18);

                if (prueba[0] != null)
                {
                    string UrlRedireccion = GenerarTokenYUrl(userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, prueba[0]);
                    Session["UrlRedireccion"] = UrlRedireccion;
                    Session["PruebaPendiente"] = prueba[0];
                    redirector.GoToDiagnostica(true);
                }
            }
            else
            {
                redirector.GoToLoginPage(true);
            }
        }

        protected void imgBtnContinuarPruebaRotter_Click(object sender, ImageClickEventArgs e)
        {
            if (userSession.IsLogin())
            {
                var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, userSession.Contrato, userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, false).ToList();
                prueba[0] = prueba.FirstOrDefault(itm => itm.PruebaID == 18);

                if (prueba[0] != null)
                {
                    string UrlRedireccion = GenerarTokenYUrl(userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, prueba[0]);
                    Session["UrlRedireccion"] = UrlRedireccion;
                    Session["PruebaPendiente"] = prueba[0];
                    redirector.GoToDiagnostica(true);
                }
            }
            else
            {
                redirector.GoToLoginPage(true);
            }
        }

        protected void imgBtnReporteRotter_Click(object sender, ImageClickEventArgs e)
        {
            string href = UrlHelper.GetReporteRotterURL();
            Response.Redirect(href);
        }
        #endregion

        #region Raven
        protected void imgBtnTomarPruebaRaven_Click(object sender, ImageClickEventArgs e)
        {
            if (userSession.IsLogin())
            {
                var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, userSession.Contrato, userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, false);
                prueba[0] = prueba.FirstOrDefault(itm => itm.PruebaID == 19);
                if (prueba[0] != null)
                {
                    string UrlRedireccion = GenerarTokenYUrl(userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, prueba[0]);
                    Session["UrlRedireccion"] = UrlRedireccion;
                    Session["PruebaPendiente"] = prueba[0];
                    redirector.GoToDiagnostica(true);
                }
            }
            else
                redirector.GoToLoginPage(true);
        }

        protected void imgBtnContinuarRaven_Click(object sender, ImageClickEventArgs e)
        {
            if (userSession.IsLogin())
            {
                var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, userSession.Contrato, userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, false);
                prueba[0] = prueba.FirstOrDefault(itm => itm.PruebaID == 19);
                if (prueba[0] != null)
                {
                    string UrlRedireccion = GenerarTokenYUrl(userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, prueba[0]);
                    Session["UrlRedireccion"] = UrlRedireccion;
                    Session["PruebaPendiente"] = prueba[0];
                    redirector.GoToDiagnostica(true);
                }
            }
            else
                redirector.GoToLoginPage(true);
        }

        protected void imgBtnReporteRaven_Click(object sender, ImageClickEventArgs e)
        {
            if (userSession.IsLogin())
            {
                string href = UrlHelper.GetReporteRavenURL();
                Response.Redirect(href);
            }
            else
                redirector.GoToLoginPage(true);
        }
        #endregion

        #region Frases Vocacionales Incompletas
        protected void imgBtnTomarPruebaFrasesVocacionales_Click(object sender, ImageClickEventArgs e)
        {
            if (userSession.IsLogin())
            {
                var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, userSession.Contrato, userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, false);
                prueba[0] = prueba.FirstOrDefault(itm => itm.PruebaID == 20);
                if (prueba[0] != null)
                {
                    string UrlRedireccion = GenerarTokenYUrl(userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, prueba[0]);
                    Session["UrlRedireccion"] = UrlRedireccion;
                    Session["PruebaPendiente"] = prueba[0];
                    redirector.GoToDiagnostica(true);
                }
            }
            else
                redirector.GoToLoginPage(true);
        }

        protected void imgBtnContinuarPruebaFrasesVocacionales_Click(object sender, ImageClickEventArgs e)
        {
            if (userSession.IsLogin())
            {
                var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, userSession.Contrato, userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, false);
                prueba[0] = prueba.FirstOrDefault(itm => itm.PruebaID == 20);
                if (prueba[0] != null)
                {
                    string UrlRedireccion = GenerarTokenYUrl(userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, prueba[0]);
                    Session["UrlRedireccion"] = UrlRedireccion;
                    Session["PruebaPendiente"] = prueba[0];
                    redirector.GoToDiagnostica(true);
                }
            }
            else
                redirector.GoToLoginPage(true);
        }

        protected void imgBtnReportePruebaFrasesVocacionales_Click(object sender, ImageClickEventArgs e)
        {
            if (userSession.IsLogin())
            {
                string href = UrlHelper.GetReporteFrasesVocacionalesURL();
                Response.Redirect(href);
            }
            else
                redirector.GoToLoginPage(true);
        }
        #endregion

        #region Zavic
        protected void imgBtnTomarPruebaZavic_Click(object sender, ImageClickEventArgs e)
        {
            if (userSession.IsLogin())
            {
                var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, userSession.Contrato, userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, false);
                prueba[0] = prueba.FirstOrDefault(itm => itm.PruebaID == 21);
                if (prueba[0] != null)
                {
                    string UrlRedireccion = GenerarTokenYUrl(userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, prueba[0]);
                    Session["UrlRedireccion"] = UrlRedireccion;
                    Session["PruebaPendiente"] = prueba[0];
                    redirector.GoToDiagnostica(true);
                }
            }
            else
                redirector.GoToLoginPage(true);
        }

        protected void imgBtnContinuarZavic_Click(object sender, ImageClickEventArgs e)
        {
            if (userSession.IsLogin())
            {
                var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, userSession.Contrato, userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, false);
                prueba[0] = prueba.FirstOrDefault(itm => itm.PruebaID == 21);
                if (prueba[0] != null)
                {
                    string UrlRedireccion = GenerarTokenYUrl(userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, prueba[0]);
                    Session["UrlRedireccion"] = UrlRedireccion;
                    Session["PruebaPendiente"] = prueba[0];
                    redirector.GoToDiagnostica(true);
                }
            }
            else
                redirector.GoToLoginPage(true);
        }

        protected void imgBtnReporteZavic_Click(object sender, ImageClickEventArgs e)
        {
            if (userSession.IsLogin())
            {
                string href = UrlHelper.GetReporteZavicURL();
                Response.Redirect(href);
            }
            else
                redirector.GoToLoginPage(true);
        }
        #endregion

        #region Estilos de Aprendizaje
        protected void ImageButtonTomarPruebaEstilos_Click(object sender, ImageClickEventArgs e)
        {
            if (userSession.IsLogin())
            {
                var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, userSession.Contrato, userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, false);
                prueba[0] = prueba.FirstOrDefault(itm => itm.PruebaID == 35);
                if (prueba[0] != null)
                {
                    string UrlRedireccion = GenerarTokenYUrl(userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, prueba[0]);
                    Session["UrlRedireccion"] = UrlRedireccion;
                    Session["PruebaPendiente"] = prueba[0];
                    redirector.GoToDiagnostica(true);
                }
            }
            else
                redirector.GoToLoginPage(true);
        }

        protected void ImageButtoncontinuarPruebaEstilos_Click(object sender, ImageClickEventArgs e)
        {
            if (userSession.IsLogin())
            {
                var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, userSession.Contrato, userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, false);
                prueba[0] = prueba.FirstOrDefault(itm => itm.PruebaID == 35);
                if (prueba[0] != null)
                {
                    string UrlRedireccion = GenerarTokenYUrl(userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, prueba[0]);
                    Session["UrlRedireccion"] = UrlRedireccion;
                    Session["PruebaPendiente"] = prueba[0];
                    redirector.GoToDiagnostica(true);
                }
            }
            else
                redirector.GoToLoginPage(true);
        }

        protected void ImageButtonVerReporteEstilos_Click(object sender, ImageClickEventArgs e)
        {
            if (userSession.IsLogin())
            {
                string href = UrlHelper.GetReporteEstilosDeAprendizajeURL();
                Response.Redirect(href);
            }
            else
                redirector.GoToLoginPage(true);
        }
       
          
        
        #endregion 

        #region Inteligencias Multiples
        protected void ImageButtonTomarPruebaIntMul_Click(object sender, ImageClickEventArgs e)
        {
            if (userSession.IsLogin())
            {
                var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, userSession.Contrato, userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, false);
                prueba[0] = prueba.FirstOrDefault(itm => itm.PruebaID == 37);
                if (prueba[0] != null)
                {
                    string UrlRedireccion = GenerarTokenYUrl(userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, prueba[0]);
                    Session["UrlRedireccion"] = UrlRedireccion;
                    Session["PruebaPendiente"] = prueba[0];
                    redirector.GoToDiagnostica(true);
                }
            }
            else
                redirector.GoToLoginPage(true);
        }

        protected void ImageButtonContinuarPruebaIntMul_Click(object sender, ImageClickEventArgs e)
        {
            if (userSession.IsLogin())
            {
                var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, userSession.Contrato, userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, false);
                prueba[0] = prueba.FirstOrDefault(itm => itm.PruebaID == 37);
                if (prueba[0] != null)
                {
                    string UrlRedireccion = GenerarTokenYUrl(userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, prueba[0]);
                    Session["UrlRedireccion"] = UrlRedireccion;
                    Session["PruebaPendiente"] = prueba[0];
                    redirector.GoToDiagnostica(true);
                }
            }
        }

        protected void ImageButtonVerReporteIntMul_Click(object sender, ImageClickEventArgs e)
        {
             if (userSession.IsLogin())
            {
                string href = UrlHelper.GetReporteInteligenciasMultiplesURL();
                Response.Redirect(href);
            }
            else
                redirector.GoToLoginPage(true);
        }


        #region BateriaBullying
        #region Bloque 1 : Victimario
        #region Autoconcepto
        protected void imgBtnTomarPruebaAutoconcepto_Click(object sender, ImageClickEventArgs e)
        {
            if (userSession.IsLogin())
            {
                var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, userSession.Contrato, userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, false).ToList();
                prueba[0] = prueba.FirstOrDefault(itm => itm.PruebaID == 22);

                if (prueba[0] != null)
                {
                    string UrlRedireccion = GenerarTokenYUrl(userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, prueba[0]);
                    Session["UrlRedireccion"] = UrlRedireccion;
                    Session["PruebaPendiente"] = prueba[0];
                    redirector.GoToDiagnostica(true);
                }
            }
            else
            {
                redirector.GoToLoginPage(true);
            }
        }
        #endregion
        #region Actitudes
        protected void imgBtnTomarPruebaActitudes_Click(object sender, ImageClickEventArgs e)
        {
            if (userSession.IsLogin())
            {
                var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, userSession.Contrato, userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, false).ToList();
                prueba[0] = prueba.FirstOrDefault(itm => itm.PruebaID == 23);

                if (prueba[0] != null)
                {
                    string UrlRedireccion = GenerarTokenYUrl(userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, prueba[0]);
                    Session["UrlRedireccion"] = UrlRedireccion;
                    Session["PruebaPendiente"] = prueba[0];
                    redirector.GoToDiagnostica(true);
                }
            }
            else
            {
                redirector.GoToLoginPage(true);
            }
        }
        #endregion
        #region Empatia
        protected void imgBtnTomarPruebaEmpatia_Click(object sender, ImageClickEventArgs e)
        {
            if (userSession.IsLogin())
            {
                var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, userSession.Contrato, userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, false).ToList();
                prueba[0] = prueba.FirstOrDefault(itm => itm.PruebaID == 24);

                if (prueba[0] != null)
                {
                    string UrlRedireccion = GenerarTokenYUrl(userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, prueba[0]);
                    Session["UrlRedireccion"] = UrlRedireccion;
                    Session["PruebaPendiente"] = prueba[0];
                    redirector.GoToDiagnostica(true);
                }
            }
            else
            {
                redirector.GoToLoginPage(true);
            }
        }
        #endregion
        #region Humor
        protected void imgBtnTomarPruebaHumor_Click(object sender, ImageClickEventArgs e)
        {
            if (userSession.IsLogin())
            {
                var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, userSession.Contrato, userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, false).ToList();
                prueba[0] = prueba.FirstOrDefault(itm => itm.PruebaID == 25);

                if (prueba[0] != null)
                {
                    string UrlRedireccion = GenerarTokenYUrl(userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, prueba[0]);
                    Session["UrlRedireccion"] = UrlRedireccion;
                    Session["PruebaPendiente"] = prueba[0];
                    redirector.GoToDiagnostica(true);
                }
            }
            else
            {
                redirector.GoToLoginPage(true);
            }
        }
        #endregion
        #endregion
        #region Bloque 2 : Victima
        #region Victimizacion
        protected void imgBtnTomarPruebaVictimizacion_Click(object sender, ImageClickEventArgs e)
        {
            if (userSession.IsLogin())
            {
                var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, userSession.Contrato, userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, false).ToList();
                prueba[0] = prueba.FirstOrDefault(itm => itm.PruebaID == 26);

                if (prueba[0] != null)
                {
                    string UrlRedireccion = GenerarTokenYUrl(userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, prueba[0]);
                    Session["UrlRedireccion"] = UrlRedireccion;
                    Session["PruebaPendiente"] = prueba[0];
                    redirector.GoToDiagnostica(true);
                }
            }
            else
            {
                redirector.GoToLoginPage(true);
            }
        }
        #endregion
        #region Ciberbullying
        protected void imgBtnTomarPruebaCiberbullying_Click(object sender, ImageClickEventArgs e)
        {
            if (userSession.IsLogin())
            {
                var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, userSession.Contrato, userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, false).ToList();
                prueba[0] = prueba.FirstOrDefault(itm => itm.PruebaID == 27);

                if (prueba[0] != null)
                {
                    string UrlRedireccion = GenerarTokenYUrl(userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, prueba[0]);
                    Session["UrlRedireccion"] = UrlRedireccion;
                    Session["PruebaPendiente"] = prueba[0];
                    redirector.GoToDiagnostica(true);
                }
            }
            else
            {
                redirector.GoToLoginPage(true);
            }
        }
        #endregion
        #region Bullying
        protected void imgBtnTomarPruebaBullying_Click(object sender, ImageClickEventArgs e)
        {
            if (userSession.IsLogin())
            {
                var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, userSession.Contrato, userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, false).ToList();
                prueba[0] = prueba.FirstOrDefault(itm => itm.PruebaID == 28);

                if (prueba[0] != null)
                {
                    string UrlRedireccion = GenerarTokenYUrl(userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, prueba[0]);
                    Session["UrlRedireccion"] = UrlRedireccion;
                    Session["PruebaPendiente"] = prueba[0];
                    redirector.GoToDiagnostica(true);
                }
            }
            else
            {
                redirector.GoToLoginPage(true);
            }
        }
        #endregion
        #endregion
        #region Bloque 3 : Funcionamiento Familiar
        #region Violencia
        protected void imgBtnTomarPruebaViolencia_Click(object sender, ImageClickEventArgs e)
        {
            if (userSession.IsLogin())
            {
                var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, userSession.Contrato, userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, false).ToList();
                prueba[0] = prueba.FirstOrDefault(itm => itm.PruebaID == 29);

                if (prueba[0] != null)
                {
                    string UrlRedireccion = GenerarTokenYUrl(userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, prueba[0]);
                    Session["UrlRedireccion"] = UrlRedireccion;
                    Session["PruebaPendiente"] = prueba[0];
                    redirector.GoToDiagnostica(true);
                }
            }
            else
            {
                redirector.GoToLoginPage(true);
            }
        }
        #endregion
        #region Comunicacion
        protected void imgBtnTomarPruebaComunicacion_Click(object sender, ImageClickEventArgs e)
        {
            if (userSession.IsLogin())
            {
                var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, userSession.Contrato, userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, false).ToList();
                prueba[0] = prueba.FirstOrDefault(itm => itm.PruebaID == 30);

                if (prueba[0] != null)
                {
                    string UrlRedireccion = GenerarTokenYUrl(userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, prueba[0]);
                    Session["UrlRedireccion"] = UrlRedireccion;
                    Session["PruebaPendiente"] = prueba[0];
                    redirector.GoToDiagnostica(true);
                }
            }
            else
            {
                redirector.GoToLoginPage(true);
            }
        }
        #endregion
        #region Imagen Corporal
        protected void imgBtnTomarPruebaImagen_Click(object sender, ImageClickEventArgs e)
        {
            if (userSession.IsLogin())
            {
                var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, userSession.Contrato, userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, false).ToList();
                prueba[0] = prueba.FirstOrDefault(itm => itm.PruebaID == 31);

                if (prueba[0] != null)
                {
                    string UrlRedireccion = GenerarTokenYUrl(userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, prueba[0]);
                    Session["UrlRedireccion"] = UrlRedireccion;
                    Session["PruebaPendiente"] = prueba[0];
                    redirector.GoToDiagnostica(true);
                }
            }
            else
            {
                redirector.GoToLoginPage(true);
            }
        }
        #endregion
        #region Ansiedad
        protected void imgBtnTomarPruebaAnsiedad_Click(object sender, ImageClickEventArgs e)
        {
            if (userSession.IsLogin())
            {
                var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, userSession.Contrato, userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, false).ToList();
                prueba[0] = prueba.FirstOrDefault(itm => itm.PruebaID == 32);

                if (prueba[0] != null)
                {
                    string UrlRedireccion = GenerarTokenYUrl(userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, prueba[0]);
                    Session["UrlRedireccion"] = UrlRedireccion;
                    Session["PruebaPendiente"] = prueba[0];
                    redirector.GoToDiagnostica(true);
                }
            }
            else
            {
                redirector.GoToLoginPage(true);
            }
        }
        #endregion
        #region Depresion
        protected void imgBtnTomarPruebaDepresion_Click(object sender, ImageClickEventArgs e)
        {
            if (userSession.IsLogin())
            {
                var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, userSession.Contrato, userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, false).ToList();
                prueba[0] = prueba.FirstOrDefault(itm => itm.PruebaID == 33);

                if (prueba[0] != null)
                {
                    string UrlRedireccion = GenerarTokenYUrl(userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, prueba[0]);
                    Session["UrlRedireccion"] = UrlRedireccion;
                    Session["PruebaPendiente"] = prueba[0];
                    redirector.GoToDiagnostica(true);
                }
            }
            else
            {
                redirector.GoToLoginPage(true);
            }
        }
        #endregion
        #endregion
        #region Reporte
        protected void imgBtnReporteBullying_Click(object sender, ImageClickEventArgs e)
        {
            string href = UrlHelper.GetReporteBateriaBullyingURL();
            Response.Redirect(href);
        }
        #endregion
        #endregion

        #endregion

        #region Inventario de Intereses
        protected void imgBtnTomarPruebaInventariodeIntereses_Click(object sender, ImageClickEventArgs e)
        {
            if (userSession.IsLogin())
            {
                var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, userSession.Contrato, userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, false);
                prueba[0] = prueba.FirstOrDefault(itm => itm.PruebaID == 46);
                if (prueba[0] != null)
                {
                    string UrlRedireccion = GenerarTokenYUrl(userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, prueba[0]);
                    Session["UrlRedireccion"] = UrlRedireccion;
                    Session["PruebaPendiente"] = prueba[0];
                    redirector.GoToDiagnostica(true);
                }
            }
            else
                redirector.GoToLoginPage(true);
        }

        protected void imgBtnContinuarPruebaInventariodeIntereses_Click(object sender, ImageClickEventArgs e)
        {
            if (userSession.IsLogin())
            {
                var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, userSession.Contrato, userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, false);
                prueba[0] = prueba.FirstOrDefault(itm => itm.PruebaID == 46);
                if (prueba[0] != null)
                {
                    string UrlRedireccion = GenerarTokenYUrl(userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, prueba[0]);
                    Session["UrlRedireccion"] = UrlRedireccion;
                    Session["PruebaPendiente"] = prueba[0];
                    redirector.GoToDiagnostica(true);
                }
            }
        }

        protected void verRptPruebaInventariodeIntereses_Click(object sender, ImageClickEventArgs e)
        {
            if (userSession.IsLogin())
            {
                string href = UrlHelper.GetReporteInventarioDeInteresesURL();
                Response.Redirect(href);
            }
            else
                redirector.GoToLoginPage(true);
        }

        #endregion 

        #region InventarioHerrera
        protected void TomarPruebaInventarioHerrera_Click(object sender, ImageClickEventArgs e)
        {
            if (userSession.IsLogin())
            {
                var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, userSession.Contrato, userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, false);
                prueba[0] = prueba.FirstOrDefault(itm => itm.PruebaID == 1049);
                if (prueba[0] != null)
                {
                    string UrlRedireccion = GenerarTokenYUrl(userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, prueba[0]);
                    Session["UrlRedireccion"] = UrlRedireccion;
                    Session["PruebaPendiente"] = prueba[0];
                    redirector.GoToDiagnostica(true);
                }
            }
            else
                redirector.GoToLoginPage(true);
        }
        protected void ContinuarPruebaInventarioHerrera_Click(object sender, ImageClickEventArgs e)
        {
             if (userSession.IsLogin())
            {
                var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, userSession.Contrato, userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, false);
                prueba[0] = prueba.FirstOrDefault(itm => itm.PruebaID == 1049);
                if (prueba[0] != null)
                {
                    string UrlRedireccion = GenerarTokenYUrl(userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, prueba[0]);
                    Session["UrlRedireccion"] = UrlRedireccion;
                    Session["PruebaPendiente"] = prueba[0];
                    redirector.GoToDiagnostica(true);
                }
            }
        }
        

        protected void VerReportePruebaInventarioHerrera_Click(object sender, ImageClickEventArgs e)
        {
            if (userSession.IsLogin())
            {
                string href = UrlHelper.GetReporteInventarioHerreraURL();
                Response.Redirect(href);
            }
            else
                redirector.GoToLoginPage(true);
        }
        #endregion

        #region Sucesos de vida
        protected void TomarPruebaSucesos_Click(object sender, ImageClickEventArgs e)
        {
            if (userSession.IsLogin())
            {
                var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, userSession.Contrato, userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, false);
                prueba[0] = prueba.FirstOrDefault(itm => itm.PruebaID == 1051);
                if (prueba[0] != null)
                {
                    string UrlRedireccion = GenerarTokenYUrl(userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, prueba[0]);
                    Session["UrlRedireccion"] = UrlRedireccion;
                    Session["PruebaPendiente"] = prueba[0];
                    redirector.GoToDiagnostica(true);
                }
            }
            else
                redirector.GoToLoginPage(true);
        }

        protected void ContinuarPruebaSucesos_Click(object sender, ImageClickEventArgs e)
        {
            if (userSession.IsLogin())
            {
                var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, userSession.Contrato, userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, false);
                prueba[0] = prueba.FirstOrDefault(itm => itm.PruebaID == 1051);
                if (prueba[0] != null)
                {
                    string UrlRedireccion = GenerarTokenYUrl(userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, prueba[0]);
                    Session["UrlRedireccion"] = UrlRedireccion;
                    Session["PruebaPendiente"] = prueba[0];
                    redirector.GoToDiagnostica(true);
                }
            }
        }

        protected void VerReporteSucesos_Click(object sender, ImageClickEventArgs e)
        {
            if (userSession.IsLogin())
            {
                string href = UrlHelper.GetReporteSucesosdeVidaURL();
                Response.Redirect(href);
            }
            else
                redirector.GoToLoginPage(true);
        }
        #endregion

        #region Prueba Luscher
        protected void TomarPruebaLuscher_Click(object sender, ImageClickEventArgs e)
        {
            if (userSession.IsLogin())
            {
                var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, userSession.Contrato, userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, false);
                prueba[0] = prueba.FirstOrDefault(itm => itm.PruebaID == 1051);
                if (prueba[0] != null)
                {
                    string UrlRedireccion = GenerarTokenYUrl(userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, prueba[0]);
                    Session["UrlRedireccion"] = UrlRedireccion;
                    Session["PruebaPendiente"] = prueba[0];
                    redirector.GoToDiagnostica(true);
                }
            }
            else
                redirector.GoToLoginPage(true);
        }
        protected void ContinuarPruebaLuscher_Click(object sender, ImageClickEventArgs e)
        {
            if (userSession.IsLogin())
            {
                var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, userSession.Contrato, userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, false);
                prueba[0] = prueba.FirstOrDefault(itm => itm.PruebaID == 1051);
                if (prueba[0] != null)
                {
                    string UrlRedireccion = GenerarTokenYUrl(userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, prueba[0]);
                    Session["UrlRedireccion"] = UrlRedireccion;
                    Session["PruebaPendiente"] = prueba[0];
                    redirector.GoToDiagnostica(true);
                }
            }
            else
                redirector.GoToLoginPage(true);
        }

        protected void VerReporteLuscher_Click(object sender, ImageClickEventArgs e)
        {
            if (userSession.IsLogin())
            {
                string href = UrlHelper.GetReportePruebaLuscherURL();
                Response.Redirect(href);
            }
            else
                redirector.GoToLoginPage(true);
        }
        #endregion

        #region Métodos Auxiliares
        public void ShowPruebasAlumno()
        {
            var pruebaDinamica = new PruebaDinamica();
            List<PruebaAsignadaAlumno> pruebas = respuestaAlumnoCtrl.RetrievePruebasAsignadaAlumno(dctx, userSession.CurrentAlumno);
            List<bool> bateriaCompleta = new List<bool>();
            bool bateriabullyin = false;
            if (pruebas.Count > 0)
            {
                foreach (var prueba in pruebas)
                {
                    #region Habitos
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.HabitosEstudio)
                    {
                        if (prueba.EstadoPrueba == (byte)EEstadoPrueba.ENCURSO)
                        {
                            imgBtnTomarPruebaHabitos.Visible = false;
                            imgBtnContinuarHabitos.Visible = true;
                        }
                        else if (prueba.EstadoPrueba == (byte)EEstadoPrueba.CERRADA)
                        {
                            imgBtnTomarPruebaHabitos.Visible = false;
                            imgBtnReporteHabitos.Visible = true;
                        }
                    }
                    #endregion
                    #region Dominos
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.Dominos)
                    {
                        if (prueba.EstadoPrueba == (byte)EEstadoPrueba.ENCURSO)
                        {
                            imgBtnTomarPruebaDominos.Visible = false;
                            imgBtnContinuarDominos.Visible = true;
                        }
                        else if (prueba.EstadoPrueba == (byte)EEstadoPrueba.CERRADA)
                        {
                            imgBtnTomarPruebaDominos.Visible = false;
                            imgBtnReporteDominos.Visible = true;
                        }
                    }
                    #endregion
                    #region Terman
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.TermanMerrill)
                    {
                        if (prueba.EstadoPrueba == (byte)EEstadoPrueba.ENCURSO)
                        {
                            imgBtnTomarPruebaTerman.Visible = false;
                            imgBtnContinuarTerman.Visible = true;
                        }
                        else if (prueba.EstadoPrueba == (byte)EEstadoPrueba.CERRADA)
                        {
                            imgBtnTomarPruebaTerman.Visible = false;
                            imgBtnReporteTerman.Visible = true;
                        }
                    }
                    #endregion
                    #region Sacks
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.FrasesIncompletasSacks)
                    {
                        if (prueba.EstadoPrueba == (byte)EEstadoPrueba.ENCURSO)
                        {
                            imgBtnTomarPruebaSacks.Visible = false;
                            imgBtnContinuarSacks.Visible = true;
                        }
                        else if (prueba.EstadoPrueba == (byte)EEstadoPrueba.CERRADA)
                        {
                            ResultadoPruebaDinamicaCtrl resultCtrl = new ResultadoPruebaDinamicaCtrl();
                            RegistroPruebaDinamicaCtrl registroPruebaDinamicaCtrl = new RegistroPruebaDinamicaCtrl();
                            SumarioGeneralSacks sumario = new SumarioGeneralSacks();
                            sumario.Prueba = new PruebaDinamica() { PruebaID = 12 };
                            sumario.Alumno = new Alumno() { AlumnoID = userSession.CurrentAlumno.AlumnoID };
                            SumarioGeneralSacks sumarioResult = new SumarioGeneralSacks();
                            sumarioResult = registroPruebaDinamicaCtrl.LastDataRowToSumarioGeneralSacks(registroPruebaDinamicaCtrl.Retrieve(dctx, sumario)); ;

                            if (sumarioResult != null)
                            {
                                imgBtnTomarPruebaSacks.Visible = false;
                                imgBtnReporteSacks.Visible = true;
                                imgBtnReporteSacks.ImageUrl = "~/Images/YOY_btnImg_Reporte.png";
                                imgBtnReporteSacks.Enabled = true;
                            }
                            else
                            {
                                imgBtnTomarPruebaSacks.Visible = false;
                                imgBtnReporteSacks.ImageUrl = "~/Images/YOY_btnImg_procesoPrueba.png";
                                imgBtnReporteSacks.Visible = true;
                                imgBtnReporteSacks.Enabled = false;
                            }
                        }
                    }
                    #endregion
                    #region Cleaver
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.Cleaver)
                    {
                        if (prueba.EstadoPrueba == (byte)EEstadoPrueba.ENCURSO)
                        {
                            imgBtnTomarPruebaCleaver.Visible = false;
                            imgBtnContinuarCleaver.Visible = true;
                        }
                        else if (prueba.EstadoPrueba == (byte)EEstadoPrueba.CERRADA)
                        {
                            imgBtnTomarPruebaCleaver.Visible = false;
                            imgBtnReporteCleaver.Visible = true;
                        }
                    }
                    #endregion
                    #region Chaside
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.Chaside)
                    {
                        if (prueba.EstadoPrueba == (byte)EEstadoPrueba.ENCURSO)
                        {
                            imgBtnTomarPruebaChaside.Visible = false;
                            imgBtnContinuarChaside.Visible = true;
                        }
                        else if (prueba.EstadoPrueba == (byte)EEstadoPrueba.CERRADA)
                        {
                            imgBtnTomarPruebaChaside.Visible = false;
                            imgBtnReporteChaside.Visible = true;
                        }
                    }
                    #endregion
                    #region Allport
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.Allport)
                    {
                        if (prueba.EstadoPrueba == (byte)EEstadoPrueba.ENCURSO)
                        {
                            imgBtnTomarPruebaAllport.Visible = false;
                            imgBtnContinuarAllport.Visible = true;
                        }
                        else if (prueba.EstadoPrueba == (byte)EEstadoPrueba.CERRADA)
                        {
                            imgBtnTomarPruebaAllport.Visible = false;
                            imgBtnReporteAllport.Visible = true;
                        }
                    }
                    #endregion
                    #region Kuder
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.Kuder)
                    {
                        if (prueba.EstadoPrueba == (byte)EEstadoPrueba.ENCURSO)
                        {
                            imgBtnTomarPruebaKuder.Visible = false;
                            imgBtnContinuarKuder.Visible = true;
                        }
                        else if (prueba.EstadoPrueba == (byte)EEstadoPrueba.CERRADA)
                        {
                            imgBtnTomarPruebaKuder.Visible = false;
                            imgBtnReporteKuder.Visible = true;
                        }
                    }
                    #endregion
                    #region Rotter
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.Rotter)
                    {
                        if (prueba.EstadoPrueba == (byte)EEstadoPrueba.ENCURSO)
                        {
                            imgBtnTomarPruebaRotter.Visible = false;
                            imgBtnContinuarRotter.Visible = true;
                        }
                        else if (prueba.EstadoPrueba == (byte)EEstadoPrueba.CERRADA)
                        {
                            imgBtnTomarPruebaRotter.Visible = false;
                            imgBtnReporteRotter.Visible = true;
                        }
                    }
                    #endregion
                    #region Raven
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.Raven)
                    {
                        if (prueba.EstadoPrueba == (byte)EEstadoPrueba.ENCURSO)
                        {
                            imgBtnTomarPruebaRaven.Visible = false;
                            imgBtnContinuarRaven.Visible = true;
                        }
                        else if (prueba.EstadoPrueba == (byte)EEstadoPrueba.CERRADA)
                        {
                            imgBtnTomarPruebaRaven.Visible = false;
                            imgBtnReporteRaven.Visible = true;
                        }
                    }
                    #endregion
                    #region Frases Vocacionales
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.FrasesIncompletasVocacionales)
                    {
                        if (prueba.EstadoPrueba == (byte)EEstadoPrueba.ENCURSO)
                        {
                            imgBtnTomarPruebaFrasesVocacionales.Visible = false;
                            imgBtnContinuarPruebaFrasesVocacionales.Visible = true;
                        }
                        else if (prueba.EstadoPrueba == (byte)EEstadoPrueba.CERRADA)
                        {
                            ResultadoPruebaDinamicaCtrl resultCtrl = new ResultadoPruebaDinamicaCtrl();
                            RegistroPruebaDinamicaCtrl registroPruebaDinamicaCtrl = new RegistroPruebaDinamicaCtrl();
                            SumarioGeneralFrasesVocacionales sumario = new SumarioGeneralFrasesVocacionales();
                            sumario.Prueba = new PruebaDinamica() { PruebaID = 20 };
                            sumario.Alumno = new Alumno() { AlumnoID = userSession.CurrentAlumno.AlumnoID };
                            SumarioGeneralFrasesVocacionales sumarioResult = new SumarioGeneralFrasesVocacionales();
                            sumarioResult = registroPruebaDinamicaCtrl.LastDataRowToSumarioGeneralFrasesVocacionales(registroPruebaDinamicaCtrl.Retrieve(dctx, sumario));
                            if (sumarioResult != null)
                            {
                                imgBtnTomarPruebaFrasesVocacionales.Visible = false;
                                imgBtnReportePruebaFrasesVocacionales.Visible = true;
                                imgBtnReportePruebaFrasesVocacionales.ImageUrl = "~/Images/YOY_btnImg_Reporte.png";
                                imgBtnReportePruebaFrasesVocacionales.Enabled = true;
                            }
                            else
                            {
                                imgBtnTomarPruebaFrasesVocacionales.Visible = false;
                                imgBtnReportePruebaFrasesVocacionales.ImageUrl = "~/Images/YOY_btnImg_procesoPrueba.png";
                                imgBtnReportePruebaFrasesVocacionales.Visible = true;
                                imgBtnReportePruebaFrasesVocacionales.Enabled = false;
                            }
                        }
                    }
                    #endregion
                    #region Zavic
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.Zavic)
                    {
                        if (prueba.EstadoPrueba == (byte)EEstadoPrueba.ENCURSO)
                        {
                            imgBtnTomarPruebaZavic.Visible = false;
                            imgBtnContinuarZavic.Visible = true;
                        }
                        else if (prueba.EstadoPrueba == (byte)EEstadoPrueba.CERRADA)
                        {
                            imgBtnTomarPruebaZavic.Visible = false;
                            imgBtnReporteZavic.Visible = true;
                        }
                    }
                    #endregion
                    #region Estilos de Aprendizaje
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.EstilosdeAprendizaje)
                    {
                        if (prueba.EstadoPrueba == (byte)EEstadoPrueba.ENCURSO)
                        {
                           ImageButtonTomarPruebaEstilos.Visible = false;
                           ImageButtoncontinuarPruebaEstilos.Visible = true;
                        }
                        else if (prueba.EstadoPrueba == (byte)EEstadoPrueba.CERRADA)
                        {
                           ImageButtonTomarPruebaEstilos.Visible = false;
                            ImageButtonVerReporteEstilos.Visible = true;
                        }
                    }
                    #endregion
                    #region Inteligencias Multiples
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.InteligengiasMultiples)
                    {
                        if (prueba.EstadoPrueba == (byte)EEstadoPrueba.ENCURSO)
                        {
                            ImageButtonTomarPruebaIntMul.Visible = false;
                            ImageButtonContinuarPruebaIntMul.Visible = true;
                        }
                        else if (prueba.EstadoPrueba == (byte)EEstadoPrueba.CERRADA)
                        {
                            ImageButtonTomarPruebaIntMul.Visible = false;
                            ImageButtonVerReporteIntMul.Visible = true;
                        }
                    }
                    #endregion 
                    #region Inventario de Intereses
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.InventarioDeIntereses)
                    {
                        if (prueba.EstadoPrueba == (byte)EEstadoPrueba.ENCURSO)
                        {
                          imgBtnTomarPruebaInventariodeIntereses.Visible = false;
                            imgBtnContinuarPruebaInventariodeIntereses.Visible = true;
                        }
                        else if (prueba.EstadoPrueba == (byte)EEstadoPrueba.CERRADA)
                        {
                            imgBtnTomarPruebaInventariodeIntereses.Visible = false;
                            verRptPruebaInventariodeIntereses.Visible = true;
                        }
                    }
                    #endregion 
                    #region Inventario Herrera Montes
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.InventarioHerreraMontes)
                    {
                        if (prueba.EstadoPrueba == (byte)EEstadoPrueba.ENCURSO)
                        {
                            TomarPruebaInventrarioHerrera.Visible = false;
                            ContinuarPruebaInventarioHerrera.Visible = true;
                        }
                        else if (prueba.EstadoPrueba == (byte)EEstadoPrueba.CERRADA)
                        {
                           TomarPruebaInventrarioHerrera.Visible = false;
                            VerReporteInventarioHerrera.Visible = true;
                        }
                    }
                    #endregion
                    #region Sucesos de Vida
                      if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.SucesosDeVida)
                    {
                        if (prueba.EstadoPrueba == (byte)EEstadoPrueba.ENCURSO)
                        {
                            TomarPruebaSucesos.Visible = false;
                            ContinuarPruebaSucesos.Visible = true;
                        }
                        else if (prueba.EstadoPrueba == (byte)EEstadoPrueba.CERRADA)
                        {
                           TomarPruebaSucesos.Visible = false;
                            VerReporteSucesos.Visible = true;
                        }
                    }
                    
                    #endregion
                    #region PruebaLsucher
                      if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.Luscher)
                    {
                        if (prueba.EstadoPrueba == (byte)EEstadoPrueba.ENCURSO)
                        {
                            TomarPruebaLuscher.Visible = false;
                            ContinuarPruebaLuscher.Visible = true;
                        }
                        else if (prueba.EstadoPrueba == (byte)EEstadoPrueba.CERRADA)
                        {
                           TomarPruebaLuscher.Visible = false;
                            VerReporteLuscher.Visible = true;
                        }
                    }
                    
                    #endregion 


                    #region BateriaBullying
                    #region Bloque 1 : Victimario
                    #region Autoconcepto
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.Autoconcepto)
                    {
                        if (prueba.EstadoPrueba == (byte)EEstadoPrueba.ENCURSO)
                        {
                            bateriaCompleta.Add(false);
                            imgBtnTomarPruebaAutoconcepto.CssClass = "classenabled";
                        }
                        else if (prueba.EstadoPrueba == (byte)EEstadoPrueba.CERRADA)
                        {
                            imgBtnTomarPruebaAutoconcepto.Enabled = false;
                            imgBtnTomarPruebaAutoconcepto.ImageUrl = "~/Images/bullying/btnautoconcepto-off.png";
                            imgBtnTomarPruebaAutoconcepto.CssClass = "classdisabled";
                            bateriaCompleta.Add(true);
                        }
                    }
                    #endregion
                    #region Actitudes
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.Actitudes)
                    {
                        if (prueba.EstadoPrueba == (byte)EEstadoPrueba.ENCURSO)
                        {
                            bateriaCompleta.Add(false);
                            imgBtnTomarPruebaActitudes.CssClass = "classenabled";
                        }
                        else if (prueba.EstadoPrueba == (byte)EEstadoPrueba.CERRADA)
                        {
                            imgBtnTomarPruebaActitudes.Enabled = false;
                            imgBtnTomarPruebaActitudes.ImageUrl = "~/Images/bullying/btnactitudes-off.png";
                            imgBtnTomarPruebaActitudes.CssClass = "classdisabled";
                            bateriaCompleta.Add(true);
                        }
                    }
                    #endregion
                    #region Empatia
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.Empatia)
                    {
                        if (prueba.EstadoPrueba == (byte)EEstadoPrueba.ENCURSO)
                        {
                            bateriaCompleta.Add(false);
                            imgBtnTomarPruebaEmpatia.CssClass = "classenabled";
                        }
                        else if (prueba.EstadoPrueba == (byte)EEstadoPrueba.CERRADA)
                        {
                            imgBtnTomarPruebaEmpatia.Enabled = false;
                            imgBtnTomarPruebaEmpatia.ImageUrl = "~/Images/bullying/btnempatia-off.png";
                            imgBtnTomarPruebaEmpatia.CssClass = "classdisabled";
                            bateriaCompleta.Add(true);
                        }
                    }
                    #endregion
                    #region Humor
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.Humor)
                    {
                        if (prueba.EstadoPrueba == (byte)EEstadoPrueba.ENCURSO)
                        {
                            bateriaCompleta.Add(false);
                            imgBtnTomarPruebaHumor.CssClass = "classenabled";
                        }
                        else if (prueba.EstadoPrueba == (byte)EEstadoPrueba.CERRADA)
                        {
                            imgBtnTomarPruebaHumor.Enabled = false;
                            imgBtnTomarPruebaHumor.ImageUrl = "~/Images/bullying/btnhumor-off.png";
                            imgBtnTomarPruebaHumor.CssClass = "classdisabled";
                            bateriaCompleta.Add(true);
                        }
                    }
                    #endregion
                    #endregion
                    #region Bloque 2 : Victima
                    #region Victimizacion
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.Victimizacion)
                    {
                        if (prueba.EstadoPrueba == (byte)EEstadoPrueba.ENCURSO)
                        {
                            bateriaCompleta.Add(false);
                            imgBtnTomarPruebaVictimizacion.CssClass = "classenabled";
                        }
                        else if (prueba.EstadoPrueba == (byte)EEstadoPrueba.CERRADA)
                        {
                            imgBtnTomarPruebaVictimizacion.Enabled = false;
                            imgBtnTomarPruebaVictimizacion.ImageUrl = "~/Images/bullying/btnvictimizacion-off.png";
                            imgBtnTomarPruebaVictimizacion.CssClass = "classdisabled";
                            bateriaCompleta.Add(true);
                        }
                    }
                    #endregion
                    #region Ciberbullying
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.Ciberbullying)
                    {
                        if (prueba.EstadoPrueba == (byte)EEstadoPrueba.ENCURSO)
                        {
                            bateriaCompleta.Add(false);
                            imgBtnTomarPruebaCiberbullying.CssClass = "classenabled";
                        }
                        else if (prueba.EstadoPrueba == (byte)EEstadoPrueba.CERRADA)
                        {
                            imgBtnTomarPruebaCiberbullying.Enabled = false;
                            imgBtnTomarPruebaCiberbullying.ImageUrl = "~/Images/bullying/btnciberbullying-off.png";
                            imgBtnTomarPruebaCiberbullying.CssClass = "classdisabled";
                            bateriaCompleta.Add(true);
                        }
                    }
                    #endregion
                    #region Bullying
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.Bullying)
                    {
                        if (prueba.EstadoPrueba == (byte)EEstadoPrueba.ENCURSO)
                        {
                            bateriaCompleta.Add(false);
                            imgBtnTomarPruebaBullying.CssClass = "classenabled";
                        }
                        else if (prueba.EstadoPrueba == (byte)EEstadoPrueba.CERRADA)
                        {
                            imgBtnTomarPruebaBullying.Enabled = false;
                            imgBtnTomarPruebaBullying.ImageUrl = "~/Images/bullying/btnbullying-off.png";
                            imgBtnTomarPruebaBullying.CssClass = "classdisabled";
                            bateriaCompleta.Add(true);
                        }
                    }
                    #endregion
                    #endregion
                    #region Bloque 3 : Funcionamiento Familiar
                    #region Violencia
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.Violencia)
                    {
                        if (prueba.EstadoPrueba == (byte)EEstadoPrueba.ENCURSO)
                        {
                            bateriaCompleta.Add(false);
                            imgBtnTomarPruebaViolencia.CssClass = "classenabled";
                        }
                        else if (prueba.EstadoPrueba == (byte)EEstadoPrueba.CERRADA)
                        {
                            imgBtnTomarPruebaViolencia.Enabled = false;
                            imgBtnTomarPruebaViolencia.ImageUrl = "~/Images/bullying/btnviolencia-off.png";
                            imgBtnTomarPruebaViolencia.CssClass = "classdisabled";
                            bateriaCompleta.Add(true);
                        }
                    }
                    #endregion
                    #region Comunicacion
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.Comunicacion)
                    {
                        if (prueba.EstadoPrueba == (byte)EEstadoPrueba.ENCURSO)
                        {
                            bateriaCompleta.Add(false);
                            imgBtnTomarPruebaComunicacion.CssClass = "classenabled";
                        }
                        else if (prueba.EstadoPrueba == (byte)EEstadoPrueba.CERRADA)
                        {
                            imgBtnTomarPruebaComunicacion.Enabled = false;
                            imgBtnTomarPruebaComunicacion.ImageUrl = "~/Images/bullying/btncomunicacion-off.png";
                            imgBtnTomarPruebaComunicacion.CssClass = "classdisabled";
                            bateriaCompleta.Add(true);
                        }
                    }
                    #endregion
                    #region Imagen Corporal
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.ImagenCorporal)
                    {
                        if (prueba.EstadoPrueba == (byte)EEstadoPrueba.ENCURSO)
                        {
                            bateriaCompleta.Add(false);
                            imgBtnTomarPruebaImagen.CssClass = "classenabled";
                        }
                        else if (prueba.EstadoPrueba == (byte)EEstadoPrueba.CERRADA)
                        {
                            imgBtnTomarPruebaImagen.Enabled = false;
                            imgBtnTomarPruebaImagen.ImageUrl = "~/Images/bullying/btnimagen-off.png";
                            imgBtnTomarPruebaImagen.CssClass = "classdisabled";
                            bateriaCompleta.Add(true);
                        }
                    }
                    #endregion
                    #region Ansiedad
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.Ansiedad)
                    {
                        if (prueba.EstadoPrueba == (byte)EEstadoPrueba.ENCURSO)
                        {
                            bateriaCompleta.Add(false);
                            imgBtnTomarPruebaAnsiedad.CssClass = "classenabled";
                        }
                        else if (prueba.EstadoPrueba == (byte)EEstadoPrueba.CERRADA)
                        {
                            imgBtnTomarPruebaAnsiedad.Enabled = false;
                            imgBtnTomarPruebaAnsiedad.ImageUrl = "~/Images/bullying/btnansiedad-off.png";
                            imgBtnTomarPruebaAnsiedad.CssClass = "classdisabled";
                            bateriaCompleta.Add(true);
                        }
                    }
                    #endregion
                    #region Depresion
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.Depresion)
                    {
                        if (prueba.EstadoPrueba == (byte)EEstadoPrueba.ENCURSO)
                        {
                            bateriaCompleta.Add(false);
                            imgBtnTomarPruebaDepresion.CssClass = "classenabled";
                        }
                        else if (prueba.EstadoPrueba == (byte)EEstadoPrueba.CERRADA)
                        {
                            imgBtnTomarPruebaDepresion.Enabled = false;
                            imgBtnTomarPruebaDepresion.ImageUrl = "~/Images/bullying/btndepresion-off.png";
                            imgBtnTomarPruebaDepresion.CssClass = "classdisabled";
                            bateriaCompleta.Add(true);
                        }
                    }
                    #endregion
                    #endregion

                    #endregion
                }
            }

            #region Reporte
            if (bateriaCompleta.Count > 0)
            {
                if (bateriaCompleta.Count < 12)
                    bateriabullyin = false;
                else
                    bateriabullyin = (bateriaCompleta.Contains(false)) ? false : true;
            }

            if (!bateriabullyin)
            {
                imgBtnReporteBullying.Enabled = false;
                imgBtnReporteBullying.CssClass = "classdisabled";
            }
            else
            {
                imgBtnReporteBullying.Enabled = true;
                imgBtnReporteBullying.CssClass = "classenabled";
            }
            #endregion
        }

        private string GenerarTokenYUrl(Alumno alumno, Escuela escuela, GrupoCicloEscolar grupoCicloEscolar, APrueba prueba)
        {

            string UrlDiagnostica;
            string formatoFecha = "yyyyMMddHHmmss.fff";
            DateTime fecha = System.DateTime.Now;
            string nombre = alumno.Nombre;
            nombre = nombre.Trim();
            string apellido = alumno.PrimerApellido;
            apellido = apellido.Trim();
            string curp = alumno.Curp;
            DateTime fechaNacimiento = (DateTime)alumno.FechaNacimiento;
            curp = curp.Trim();
            string token = fechaNacimiento.ToString(formatoFecha) + nombre + apellido + curp + fecha.ToString(formatoFecha);
            byte[] clave = POV.Seguridad.Utils.EncryptHash.SHA1encrypt(token);
            token = POV.Seguridad.Utils.EncryptHash.byteArrayToStringBase64(clave);
            token = System.Web.HttpUtility.UrlEncode(token);
            UrlDiagnostica = "?alumno=" + alumno.Curp + "&escuela=" + escuela.EscuelaID + "&grupo=" + grupoCicloEscolar.GrupoCicloEscolarID + "&fechahora=" + fecha.ToString(formatoFecha) + "&prueba=" + prueba.PruebaID + "&token=" + token;
            return UrlDiagnostica;
        }
        #endregion

      

      
      

       
     
# endregion 

        

    

    
        
    }
}