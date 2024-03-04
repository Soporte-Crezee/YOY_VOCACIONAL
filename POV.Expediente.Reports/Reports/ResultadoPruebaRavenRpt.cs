using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using POV.CentroEducativo.BO;
using POV.Seguridad.BO;
using POV.Prueba.Diagnostico.Dinamica.BO;
using DevExpress.Office;
using POV.Reactivos.BO;

namespace POV.Expediente.Reports.Reports
{
    public partial class ResultadoPruebaRavenRpt : DevExpress.XtraReports.UI.XtraReport
    {
        private Alumno _alumno;
        private Usuario _usuario;
        private string _fechaInicio;
        private string _fechaFin;

        public ResultadoPruebaRavenRpt()
        {
            InitializeComponent();
        }

        public ResultadoPruebaRavenRpt(DiagnosticoRaven diagnostico, Alumno alumno, Usuario usuario, string fechaInicio, string fechaFin)
        {
            InitializeComponent();
            this.SetData(diagnostico, alumno, usuario, fechaInicio, fechaFin);

        }

        public void SetData(DiagnosticoRaven diagnostico, Alumno alumno, Usuario usuario, string fechaInicio, string fechaFin)
        {
            this._alumno = alumno;
            this._usuario = usuario;
            this._fechaInicio = fechaInicio;
            this._fechaFin = fechaFin;

            this.cNombre.Text = _alumno.Nombre + " " + " " + _alumno.PrimerApellido + " " + _alumno.SegundoApellido;
            this.cCorreo.Text = _usuario.Email;
            this.cFinalPrueba.Text = _fechaFin;

            #region Colores
            this.Header.ForeColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");

            this.Titulo.ForeColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");

            this.NamePrueba.ForeColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");

            this.TableInfoPrueba.BackColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");
            this.TableInfoPrueba.BorderColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");

            this.TableInfoEstudiante.BackColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");
            this.TableInfoEstudiante.BorderColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");

            this.TableResultPrueba.BackColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");
            this.TableResultPrueba.BorderColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");

            this.xrLine1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff9e19");
            this.xrLine2.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff9e19");
            #endregion

            cEdad.Text = diagnostico.Edad + " años";
            //cFechaAplicacion.Text = _fechaInicio;
            cEdad.Text = diagnostico.Puntaje.ToString();
            cPercentil.Text = diagnostico.Percentil.ToString();
            cRango.Text = diagnostico.Rango;
            cDiagnostico.Text = "Capacidad Intelectual " + diagnostico.Diagnostico;
            if (diagnostico.Validez)
            {
                cValidez.Text = "Prueba válida";
                this.cValidez.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                this.cValidez.BorderColor = System.Drawing.ColorTranslator.FromHtml("#17DA0A");
                this.cValidez.BackColor = System.Drawing.ColorTranslator.FromHtml("#17DA0A");
                this.cDiscrepancia.Text = "SI";
            }
            else
            {
                cValidez.Text = "Riesgo en la validez";
                this.cValidez.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                this.cValidez.BorderColor = System.Drawing.ColorTranslator.FromHtml("#ff9e19");
                this.cValidez.BackColor = System.Drawing.ColorTranslator.FromHtml("#ff9e19");
                this.cDiscrepancia.Text = "NO";
            }
            #region Obtencion Discrepancias
            #region Puntuacion Directa
            if (diagnostico.PuntuacionDirecta != null || diagnostico.PuntuacionDirecta.Count > 0) 
            {
                foreach (var item in diagnostico.PuntuacionDirecta)
                {
                    switch (item.Key)
                    {
                        case "A":
                            pdirectaA.Text = item.Value.ToString();
                            break;
                        case "B":
                            pdirectaB.Text = item.Value.ToString();
                            break;
                        case "C":
                            pdirectaC.Text = item.Value.ToString();
                            break;
                        case "D":
                            pdirectaD.Text = item.Value.ToString();
                            break;
                        case "E":
                            pdirectaE.Text = item.Value.ToString();
                            break;
                        case "Total":
                            cTotal.Text = item.Value.ToString();
                            cPuntaje.Text = item.Value.ToString();
                            break;
                    }
                }
            }
            #endregion
            #region Puntuacion Esperada
            if (diagnostico.PuntuacionEsperada != null || diagnostico.PuntuacionEsperada.Count > 0) 
            {
                foreach (var item in diagnostico.PuntuacionEsperada)
                {
                    switch (item.Key)
                    {
                        case "A":
                            pesperadaA.Text = item.Value.ToString();
                            break;
                        case "B":
                            pesperadaB.Text = item.Value.ToString();
                            break;
                        case "C":
                            pesperadaC.Text = item.Value.ToString();
                            break;
                        case "D":
                            pesperadaD.Text = item.Value.ToString();
                            break;
                        case "E":
                            pesperadaE.Text = item.Value.ToString();
                            break;
                    }
                }
            }
            #endregion
            #region Discrepacias
            if (diagnostico.Discrepancias != null || diagnostico.Discrepancias.Count > 0) 
            {
                foreach (var item in diagnostico.Discrepancias)
                {
                    switch (item.Key)
                    {
                        case "A":
                            discrepanciaA.Text = item.Value.ToString();
                            break;
                        case "B":
                            discrepanciaB.Text = item.Value.ToString();
                            break;
                        case "C":
                            discrepanciaC.Text = item.Value.ToString();
                            break;
                        case "D":
                            discrepanciaD.Text = item.Value.ToString();
                            break;
                        case "E":
                            discrepanciaE.Text = item.Value.ToString();
                            break;
                    }
                }
            }
            #endregion
            #endregion
            this.lblAnio.Text = "©" + DateTime.Now.ToString("yyyy") + ", Todos los derechos reservados YOY Vocacional";
        }
    }
}
