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
    public partial class ResultadoPruebaDominosRpt : DevExpress.XtraReports.UI.XtraReport
    {
        private Alumno _alumno;
        private Usuario _usuario;
        private Decimal _resDominos;
        private string _fechaFin;

        public ResultadoPruebaDominosRpt()
        {
            InitializeComponent();
        }

        public ResultadoPruebaDominosRpt(Alumno alumno, Usuario usuario, Decimal resDominos, string fechaFin)
        {
            InitializeComponent();
            this.SetData(alumno, usuario, resDominos, fechaFin);

        }

        public void SetData(Alumno alumno, Usuario usuario, Decimal resDominos, string fechaFin)
        {
            this._alumno = alumno;
            this._usuario = usuario;
            this._resDominos = resDominos;
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

            this.cPercentiles.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccc");
            this.cPercentiles.BorderColor = System.Drawing.ColorTranslator.FromHtml("#ccc");

            this.cRangos.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccc");
            this.cRangos.BorderColor = System.Drawing.ColorTranslator.FromHtml("#ccc");

            this.xrLine1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff9e19");
            this.xrLine2.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff9e19");
            #endregion

            #region Dominos
            ResaltarResultadoDominos(_resDominos);
            #endregion

            this.lblAnio.Text = "©" + DateTime.Now.ToString("yyyy") + ", Todos los derechos reservados YOY Vocacional";
        }

        private void ResaltarResultadoDominos(decimal puntaje)
        {
            if (puntaje > 90 && puntaje <= 100)
            {
                cPercentil1.Font = new Font("Arial", 12f, FontStyle.Bold);
                cPercentil1.BackColor = System.Drawing.ColorTranslator.FromHtml("#33acfd");
                cPercentil1.ForeColor = Color.White;

                cRango1.Font = new Font("Arial", 12f, FontStyle.Bold);
                cRango1.BackColor = System.Drawing.ColorTranslator.FromHtml("#33acfd");
                cRango1.ForeColor = Color.White;
            }
            else if (puntaje >= 75 && puntaje <= 90)
            {
                cPercentil2.Font = new Font("Arial", 12f, FontStyle.Bold);
                cPercentil2.BackColor = System.Drawing.ColorTranslator.FromHtml("#33acfd");
                cPercentil2.ForeColor = Color.White;

                cRango2.Font = new Font("Arial", 12f, FontStyle.Bold);
                cRango2.BackColor = System.Drawing.ColorTranslator.FromHtml("#33acfd");
                cRango2.ForeColor = Color.White;
            }
            else if (puntaje > 25 && puntaje < 75)
            {
                cPercentil3.Font = new Font("Arial", 12f, FontStyle.Bold);
                cPercentil3.BackColor = System.Drawing.ColorTranslator.FromHtml("#33acfd");
                cPercentil3.ForeColor = Color.White;

                cRango3.Font = new Font("Arial", 12f, FontStyle.Bold);
                cRango3.BackColor = System.Drawing.ColorTranslator.FromHtml("#33acfd");
                cRango3.ForeColor = Color.White;
            }
            else if (puntaje >= 10 && puntaje <= 25)
            {
                cPercentil4.Font = new Font("Arial", 12f, FontStyle.Bold);
                cPercentil4.BackColor = System.Drawing.ColorTranslator.FromHtml("#33acfd");
                cPercentil4.ForeColor = Color.White;

                cRango4.Font = new Font("Arial", 12f, FontStyle.Bold);
                cRango4.BackColor = System.Drawing.ColorTranslator.FromHtml("#33acfd");
                cRango4.ForeColor = Color.White;
            }
            else if (puntaje >= 0 && puntaje < 10)
            {
                cPercentil5.Font = new Font("Arial", 12f, FontStyle.Bold);
                cPercentil5.BackColor = System.Drawing.ColorTranslator.FromHtml("#33acfd");
                cPercentil5.ForeColor = Color.White;

                cRango5.Font = new Font("Arial", 12f, FontStyle.Bold);
                cRango5.BackColor = System.Drawing.ColorTranslator.FromHtml("#33acfd");
                cRango5.ForeColor = Color.White;
            }
        }
    }
}
