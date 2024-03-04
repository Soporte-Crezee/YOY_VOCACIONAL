using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.XtraCharts;
using POV.Prueba.Diagnostico.Dinamica.BO;
using System.Collections.Generic;
using POV.CentroEducativo.BO;
using POV.Seguridad.BO;
using DevExpress.Office;

namespace POV.Expediente.Reports.Reports
{
    public partial class ResultadoPruebaCleaverRpt : DevExpress.XtraReports.UI.XtraReport
    {
        private Alumno _alumno;
        private Usuario _usuario;
        private string _fechaFin;

        private Series series1 = new Series("Normal", ViewType.Line);
        private Series series2 = new Series("Motivación", ViewType.Line);
        private Series series3 = new Series("Presión", ViewType.Line);

        public ResultadoPruebaCleaverRpt()
        {
            InitializeComponent();
        }

        public ResultadoPruebaCleaverRpt(ResultadoPruebaCleaver serieMore, ResultadoPruebaCleaver serieLess, ResultadoPruebaCleaver serieTotal, Alumno alumno, Usuario usuario, string fechaFin) 
        {
            InitializeComponent();
            this.SetData(serieMore, serieLess, serieTotal, alumno, usuario, fechaFin);
        }

        public void SetData(ResultadoPruebaCleaver serieMore, ResultadoPruebaCleaver serieLess, ResultadoPruebaCleaver serieTotal, Alumno alumno, Usuario usuario, string fechaFin)
        {
            this._alumno = alumno;
            this._usuario = usuario;
            this._fechaFin = fechaFin;

            this.cNombre.Text = _alumno.Nombre + " " + " " + _alumno.PrimerApellido + " " + _alumno.SegundoApellido;
            this.cCorreo.Text = _usuario.Email;
            this.cFinalPrueba.Text = _fechaFin;

            #region Colores
            this.Header.ForeColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");

            this.Titulo.ForeColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");

            this.NamePrueba.ForeColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");

            this.TableResultPrueba.BackColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");
            this.TableResultPrueba.BorderColor = System.Drawing.ColorTranslator.FromHtml("#ff9e19");

            this.TableInfoPrueba.BackColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");
            this.TableInfoPrueba.BorderColor = System.Drawing.ColorTranslator.FromHtml("#ff9e19");

            this.TableInfoEstudiante.BackColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");
            this.TableInfoEstudiante.BorderColor = System.Drawing.ColorTranslator.FromHtml("#ff9e19");

            this.cCaracteristica.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccc");
            this.cCaracteristica.BorderColor = System.Drawing.ColorTranslator.FromHtml("#ccc");

            this.cTag.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccc");
            this.cTag.BorderColor = System.Drawing.ColorTranslator.FromHtml("#ccc");

            this.cTexto.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccc");
            this.cTexto.BorderColor = System.Drawing.ColorTranslator.FromHtml("#ccc");

            this.xrLine1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff9e19");
            this.xrLine2.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff9e19");
            #endregion

            #region Grafica Normal
            List<CalificacionCleaver> listCalificacionNormal = new List<CalificacionCleaver>();

            listCalificacionNormal.AddRange(new CalificacionCleaver[] 
            {
                new CalificacionCleaver("D", (int)serieTotal.Resultado_D),
                new CalificacionCleaver("I", (int)serieTotal.Resultado_I),
                new CalificacionCleaver("S", (int)serieTotal.Resultado_S),
                new CalificacionCleaver("C", (int)serieTotal.Resultado_C)
            });

            series1.ArgumentScaleType = ScaleType.Qualitative;
            series1.ValueScaleType = ScaleType.Numerical;

            series1.ArgumentDataMember = "Nombre";
            series1.ValueDataMembers[0] = "Puntaje";

            xrChartNormal.Series.Add(series1);
            xrChartNormal.DataSource = listCalificacionNormal;
            #endregion

            #region Grafica Motivación
            List<CalificacionCleaver> listCalificacionMotvacion = new List<CalificacionCleaver>();

            listCalificacionMotvacion.AddRange(new CalificacionCleaver[] 
            {
                new CalificacionCleaver("D", (int)serieMore.Resultado_D),
                new CalificacionCleaver("I", (int)serieMore.Resultado_I),
                new CalificacionCleaver("S", (int)serieMore.Resultado_S),
                new CalificacionCleaver("C", (int)serieMore.Resultado_C)
            });

            series2.ArgumentScaleType = ScaleType.Qualitative;
            series2.ValueScaleType = ScaleType.Numerical;

            series2.ArgumentDataMember = "Nombre";
            series2.ValueDataMembers[0] = "Puntaje";

            xrChartMotivacion.Series.Add(series2);
            xrChartMotivacion.DataSource = listCalificacionMotvacion;
            #endregion

            #region Grafica Presión
            List<CalificacionCleaver> listCalificacionPresion = new List<CalificacionCleaver>();

            listCalificacionPresion.AddRange(new CalificacionCleaver[] 
            {
                new CalificacionCleaver("D", serieLess.Resultado_D != null ? (int)serieLess.Resultado_D : 0),
                new CalificacionCleaver("I", serieLess.Resultado_I != null ? (int)serieLess.Resultado_I : 0),
                new CalificacionCleaver("S", serieLess.Resultado_S != null ? (int)serieLess.Resultado_S : 0),
                new CalificacionCleaver("C", serieLess.Resultado_C != null ? (int)serieLess.Resultado_C : 0)
            });

            series3.ArgumentScaleType = ScaleType.Qualitative;
            series3.ValueScaleType = ScaleType.Numerical;

            series3.ArgumentDataMember = "Nombre";
            series3.ValueDataMembers[0] = "Puntaje";

            xrChartPresion.Series.Add(series3);
            xrChartPresion.DataSource = listCalificacionPresion;
            #endregion

            #region Características
            #region More
            if (serieMore.Resultado_D > 55 || serieLess.Resultado_D > 55 || serieTotal.Resultado_D > 55)
                xrTableRowDMore.Visible = true;
            if (serieMore.Resultado_I > 55 || serieLess.Resultado_I > 55 || serieTotal.Resultado_I > 55)
                xrTableRowIMore.Visible = true;
            if (serieMore.Resultado_S > 55 || serieLess.Resultado_S > 55 || serieTotal.Resultado_S > 55)
                xrTableRowSMore.Visible = true;
            if (serieMore.Resultado_C > 55 || serieLess.Resultado_C > 55 || serieTotal.Resultado_C > 55)
                xrTableRowCMore.Visible = true;
            #endregion

            #region Less
            if (serieMore.Resultado_D < 45 || serieLess.Resultado_D < 45 || serieTotal.Resultado_D < 45)
                xrTableRowDLess.Visible = true;
            if (serieMore.Resultado_I < 45 || serieLess.Resultado_I < 45 || serieTotal.Resultado_I < 45)
                xrTableRowILess.Visible = true;
            if (serieMore.Resultado_S < 45 || serieLess.Resultado_S < 45 || serieTotal.Resultado_S < 45)
                xrTableRowSLess.Visible = true;
            if (serieMore.Resultado_C < 45 || serieLess.Resultado_C < 45 || serieTotal.Resultado_C < 45)
                xrTableRowCLess.Visible = true;
            #endregion
            #endregion

            this.lblAnio.Text = "©" + DateTime.Now.ToString("yyyy") + ", Todos los derechos reservados YOY Vocacional";
        }

        public class CalificacionCleaver
        {
            public string Nombre { get; set; }
            public int? Puntaje { get; set; }

            public CalificacionCleaver(string nombre, int puntaje)
            {
                this.Nombre = nombre;
                this.Puntaje = puntaje;
            }
        }
    }
}
