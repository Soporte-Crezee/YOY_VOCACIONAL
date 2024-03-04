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
using POV.Prueba.Diagnostico.Dinamica.BO;
namespace POV.Web.PortalSocial.Auth
{
    public partial class ValidarBateriaBullying : System.Web.UI.Page
    {

        private PruebaDiagnosticoCtrl pruebaDiagnosticoCtrl;

        private readonly IDataContext dctx = ConnectionHelper.Default.Connection;
        private IRedirector redirector = new Redirector();
        private IUserSession userSession;

        private AlumnoCtrl alumnoCtrl;

        public ValidarBateriaBullying()
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

                    var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, contrato, alumno, escuela, grupoCicloEscolar, false).ToList();
                    List<APrueba> pruebasPendientesBullying = new List<APrueba>();
                    APrueba estadoPrueba = null;
                    foreach (var item in prueba)
                    {
                        //var estadoPrueba as APrueba;
                        //estadoPrueba = item;
                        estadoPrueba = pruebaDiagnosticoCtrl.TienePruebaPendiente(item, dctx, alumno, escuela, grupoCicloEscolar, ETipoResultadoPrueba.PRUEBA_DIAGNOSTICA);
                        if (estadoPrueba != null)
                            pruebasPendientesBullying.Add(item);
                    }

                    var pruebaBullying = RetrievePruebaPendiente(pruebasPendientesBullying);



                    if (pruebaBullying != null) //Si tiene una prueba de Bullying pendiente se redirige al portal de prueba diagnostica
                    {
                        string UrlRedireccion = GenerarTokenYUrl(alumno, escuela, grupoCicloEscolar, pruebaBullying);
                        Session["UrlRedireccion"] = UrlRedireccion;
                        Session["PruebaPendiente"] = pruebaBullying;
                        redirector.GoToDiagnostica(true);
                    }
                    else //no tiene prueba Bullyign pendiente
                    {
                        redirector.GoToPruebas(true);
                    }

                }
                else
                    redirector.GoToLoginPage(true);
            }
        }

        private APrueba RetrievePruebaPendiente(List<APrueba> pruebas)
        {
            APrueba prueba = null;
            APrueba pruebafilter = null;
            if (pruebas.Count > 0 || pruebas != null)
            {
                pruebafilter = pruebas.FirstOrDefault(itm => itm.PruebaID == 22);
                if (pruebafilter != null)
                    return prueba = pruebafilter;

                pruebafilter = pruebas.FirstOrDefault(itm => itm.PruebaID == 23);
                if (pruebafilter != null)
                    return prueba = pruebafilter;

                pruebafilter = pruebas.FirstOrDefault(itm => itm.PruebaID == 24);
                if (pruebafilter != null)
                    return prueba = pruebafilter;

                pruebafilter = pruebas.FirstOrDefault(itm => itm.PruebaID == 25);
                if (pruebafilter != null)
                    return prueba = pruebafilter;

                pruebafilter = pruebas.FirstOrDefault(itm => itm.PruebaID == 26);
                if (pruebafilter != null)
                    return prueba = pruebafilter;

                pruebafilter = pruebas.FirstOrDefault(itm => itm.PruebaID == 27);
                if (pruebafilter != null)
                    return prueba = pruebafilter;

                pruebafilter = pruebas.FirstOrDefault(itm => itm.PruebaID == 28);
                if (pruebafilter != null)
                    return prueba = pruebafilter;

                pruebafilter = pruebas.FirstOrDefault(itm => itm.PruebaID == 29);
                if (pruebafilter != null)
                    return prueba = pruebafilter;

                pruebafilter = pruebas.FirstOrDefault(itm => itm.PruebaID == 30);
                if (pruebafilter != null)
                    return prueba = pruebafilter;

                pruebafilter = pruebas.FirstOrDefault(itm => itm.PruebaID == 31);
                if (pruebafilter != null)
                    return prueba = pruebafilter;

                pruebafilter = pruebas.FirstOrDefault(itm => itm.PruebaID == 32);
                if (pruebafilter != null)
                    return prueba = pruebafilter;

                pruebafilter = pruebas.FirstOrDefault(itm => itm.PruebaID == 33);
                if (pruebafilter != null)
                    return prueba = pruebafilter;
            }

            return prueba;
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
    }
}