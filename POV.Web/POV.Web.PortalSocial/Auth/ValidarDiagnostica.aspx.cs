using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.CentroEducativo.BO;
using Framework.Base.DataAccess;
using POV.Web.PortalSocial.AppCode;
using POV.Core.RedSocial.Interfaces;
using POV.Core.RedSocial.Implement;
using POV.Prueba.Diagnostico.Service;
using POV.Licencias.BO;
using POV.Prueba.BO;
using POV.ServiciosActividades.Controllers;
using System.Threading;
using POV.Expediente.BO;
using POV.Licencias.Service;
using POV.CentroEducativo.Service;
using POV.DetalleExpediente.Services;
using POV.Expediente.Services;
using POV.Logger.Service;
namespace POV.Web.PortalSocial.Auth
{
    public partial class ValidarDiagnotica : System.Web.UI.Page
    {

        private PruebaDiagnosticoCtrl pruebaDiagnosticoCtrl;

        private readonly IDataContext dctx = ConnectionHelper.Default.Connection;
        private IRedirector redirector = new Redirector();
        private IUserSession userSession;

        private AlumnoCtrl alumnoCtrl;

        public ValidarDiagnotica()
        {
            userSession = new UserSession();
            pruebaDiagnosticoCtrl = new PruebaDiagnosticoCtrl();
            alumnoCtrl = new AlumnoCtrl();
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (userSession.IsLogin())
                {
                    Alumno alumno = userSession.CurrentAlumno;
                    Escuela escuela = userSession.CurrentEscuela;
                    CicloEscolar cicloEscolar = userSession.CurrentCicloEscolar;
                    GrupoCicloEscolar grupoCicloEscolar = userSession.CurrentGrupoCicloEscolar;
                    Contrato contrato = userSession.Contrato;

                    var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, contrato, alumno, escuela, grupoCicloEscolar, true).ToList();

                    if (prueba.Count > 0 && prueba[0] != null) //Si tiene una prueba Pivote pendiente se redirige al portal de prueba diagnostica
                    {
                        string UrlRedireccion = GenerarTokenYUrl(alumno, escuela, grupoCicloEscolar);
                        Session["UrlRedireccion"] = UrlRedireccion;
                        Session["PruebaPendiente"] = prueba[0];
                        redirector.GoToDiagnostica(true);
                    }
                    else //no tiene prueba Pivote pendiente
                    {
                        if ((userSession.CurrentAlumno.CarreraSeleccionada != false) && (userSession.CurrentAlumno.CarreraSeleccionada != null))
                        {
                            redirector.GoToHomePage(true);
                        }
                        else
                        {
                            try
                            {
                                var estudiante = new Alumno();
                                estudiante = (Alumno)alumno.Clone();
                                estudiante.CarreraSeleccionada = true;
                                estudiante.CorreoConfirmado = false;
                                alumnoCtrl.Update(dctx, estudiante, alumno);
                                redirector.GoToHomePage(true);
                            }
                            catch (Exception ex)
                            {
                                LoggerHlp.Default.Error(this, ex);
                            }
                        }
                    }

                }
                else
                    redirector.GoToLoginPage(true);
            }
        }

        private string GenerarTokenYUrl(Alumno alumno, Escuela escuela, GrupoCicloEscolar grupoCicloEscolar)
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
            UrlDiagnostica = "?alumno=" + alumno.Curp + "&escuela=" + escuela.EscuelaID + "&grupo=" + grupoCicloEscolar.GrupoCicloEscolarID + "&fechahora=" + fecha.ToString(formatoFecha) + "&token=" + token;
            return UrlDiagnostica;
        }
    }
}