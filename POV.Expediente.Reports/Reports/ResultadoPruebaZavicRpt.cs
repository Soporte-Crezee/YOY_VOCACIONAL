using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using System.Linq;
using POV.CentroEducativo.BO;
using POV.Seguridad.BO;
using POV.Expediente.BO;
using System.Collections.Generic;
using System.Drawing.Printing;
using POV.Modelo.BO;
using System.IO;
using System.Text;
using DevExpress.XtraCharts;
using DevExpress.Office;

namespace POV.Expediente.Reports.Reports
{
    public partial class ResultadoPruebaZavicRpt : DevExpress.XtraReports.UI.XtraReport
    {
        private Alumno _alumno;
        private Usuario _usuario;
        private IDictionary<string, string> _puntuacionesZavic;
        private string _fechaFin;

        public ResultadoPruebaZavicRpt()
        {
            InitializeComponent();
        }

        public ResultadoPruebaZavicRpt(Alumno alumno, Usuario usuario, IDictionary<string,string> puntuacionesZavic, string fechaFin)
        {
            InitializeComponent();
            this.SetData(alumno, usuario, puntuacionesZavic, fechaFin);

        }

        public void SetData(Alumno alumno, Usuario usuario, IDictionary<string, string> puntuacionesZavic, string fechaFin)
        {
            this._alumno = alumno;
            this._usuario = usuario;
            this._puntuacionesZavic = puntuacionesZavic;
            this._fechaFin = fechaFin;

            this.cNombre.Text = _alumno.Nombre + " " + " " + _alumno.PrimerApellido + " " + _alumno.SegundoApellido;
            this.cCorreo.Text = _usuario.Email;
            this.cFinalPrueba.Text = _fechaFin;
            #region Colores
            this.Header.ForeColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");

            this.Titulo.ForeColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");

            this.NamePrueba.ForeColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");

            this.TableInfoEstudiante.BackColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");
            this.TableInfoEstudiante.BorderColor = System.Drawing.ColorTranslator.FromHtml("#ff9e19");

            this.TableInfoPrueba.BackColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");
            this.TableInfoPrueba.BorderColor = System.Drawing.ColorTranslator.FromHtml("#ff9e19");

            this.TableResultPrueba.BackColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");
            this.TableResultPrueba.BorderColor = System.Drawing.ColorTranslator.FromHtml("#ff9e19");

            //this.cPercentiles.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccc");
            //this.cPercentiles.BorderColor = System.Drawing.ColorTranslator.FromHtml("#ccc");

            //this.cRangos.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccc");
            //this.cRangos.BorderColor = System.Drawing.ColorTranslator.FromHtml("#ccc");

            this.xrLine1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff9e19");
            this.xrLine2.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff9e19");
            #endregion

            #region Resultados
            Series valores = new Series("Valores", ViewType.Bar);
            Series intereses = new Series("Intereses", ViewType.Bar);
            foreach (var item in puntuacionesZavic)
            {
                if (item.Key == "Moral" || item.Key == "Legalidad" || item.Key == "Indiferencia" || item.Key == "Corrupto")
                    valores.Points.Add(new SeriesPoint(item.Key, item.Value));
                else
                    intereses.Points.Add(new SeriesPoint(item.Key, item.Value));
            }
            xrChartZavic.Series.Add(valores);
            xrChartZavic.Series.Add(intereses);
            #endregion

            this.lblAnio.Text = "©" + DateTime.Now.ToString("yyyy") + ", Todos los derechos reservados YOY Vocacional";
        }
    }
}
