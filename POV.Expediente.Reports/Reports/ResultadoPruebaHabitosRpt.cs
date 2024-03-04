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
    public partial class ResultadoPruebaHabitosRpt : DevExpress.XtraReports.UI.XtraReport
    {
        private Alumno _alumno;
        private Usuario _usuario;
        private Decimal _resHabitos;
        private string _fechaFin;

        public ResultadoPruebaHabitosRpt()
        {
            InitializeComponent();
        }

        public ResultadoPruebaHabitosRpt(Alumno alumno, Usuario usuario, Decimal resHabitos, string fechaFin)
        {
            InitializeComponent();
            this.SetData(alumno, usuario, resHabitos, fechaFin);

        }

        public void SetData(Alumno alumno, Usuario usuario, Decimal resHabitos, string fechaFin)
        {
            this._alumno = alumno;
            this._usuario = usuario;
            this._resHabitos = resHabitos;
            this._fechaFin = fechaFin;

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

            this.cPuntaje.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccc");
            this.cPuntaje.BorderColor = System.Drawing.ColorTranslator.FromHtml("#ccc");

            this.cNiveles.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccc");
            this.cNiveles.BorderColor = System.Drawing.ColorTranslator.FromHtml("#ccc");

            this.cRecomendaciones.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccc");
            this.cRecomendaciones.BorderColor = System.Drawing.ColorTranslator.FromHtml("#ccc");

            this.xrLine1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff9e19");
            this.xrLine2.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff9e19");
            #endregion

            this.cNombre.Text = _alumno.Nombre + " " + " " + _alumno.PrimerApellido + " " + _alumno.SegundoApellido;
            this.cCorreo.Text = _usuario.Email;
            this.cFinalPrueba.Text = _fechaFin;

            #region Habitos
            ResaltarResultadoHabitos(_resHabitos);
            #endregion
            this.lblAnio.Text = "©" + DateTime.Now.ToString("yyyy") + ", Todos los derechos reservados YOY Vocacional";
        }

        private void ResaltarResultadoHabitos(decimal puntaje)
        {
            if (puntaje >= 0 && puntaje <= 50)
            {
                punt1.Font = new Font("Arial", 12f, FontStyle.Bold);
                punt1.BackColor = System.Drawing.ColorTranslator.FromHtml("#33acfd");
                punt1.ForeColor = Color.White;

                niv1.Font = new Font("Arial", 12f, FontStyle.Bold);
                niv1.BackColor = System.Drawing.ColorTranslator.FromHtml("#33acfd");
                niv1.ForeColor = Color.White;
                reco1.Font = new Font("Arial", 12f, FontStyle.Bold);
                reco1.BackColor = System.Drawing.ColorTranslator.FromHtml("#33acfd");
                reco1.ForeColor = Color.White;
            }
            else if (puntaje >= 51 && puntaje <= 75)
            {
                punt2.Font = new Font("Arial", 12f, FontStyle.Bold);
                punt2.BackColor = System.Drawing.ColorTranslator.FromHtml("#33acfd");
                punt2.ForeColor = Color.White;
                niv2.Font = new Font("Arial", 12f, FontStyle.Bold);
                niv2.BackColor = System.Drawing.ColorTranslator.FromHtml("#33acfd");
                niv2.ForeColor = Color.White;
                reco1b.Font = new Font("Arial", 12f, FontStyle.Bold);
                reco1b.BackColor = System.Drawing.ColorTranslator.FromHtml("#33acfd");
                reco1b.ForeColor = Color.White;
            }
            else if (puntaje >= 76 && puntaje <= 95)
            {
                punt3.Font = new Font("Arial", 12f, FontStyle.Bold);
                punt3.BackColor = System.Drawing.ColorTranslator.FromHtml("#33acfd");
                punt3.ForeColor = Color.White;
                niv3.Font = new Font("Arial", 12f, FontStyle.Bold);
                niv3.BackColor = System.Drawing.ColorTranslator.FromHtml("#33acfd");
                niv3.ForeColor = Color.White;
                reco2.Font = new Font("Arial", 12f, FontStyle.Bold);
                reco2.BackColor = System.Drawing.ColorTranslator.FromHtml("#33acfd");
                reco2.ForeColor = Color.White;
            }
            else if (puntaje >= 96 && puntaje <= 115)
            {
                punt4.Font = new Font("Arial", 12f, FontStyle.Bold);
                punt4.BackColor = System.Drawing.ColorTranslator.FromHtml("#33acfd");
                punt4.ForeColor = Color.White;
                niv4.Font = new Font("Arial", 12f, FontStyle.Bold);
                niv4.BackColor = System.Drawing.ColorTranslator.FromHtml("#33acfd");
                niv4.ForeColor = Color.White;
                reco3.Font = new Font("Arial", 12f, FontStyle.Bold);
                reco3.BackColor = System.Drawing.ColorTranslator.FromHtml("#33acfd");
                reco3.ForeColor = Color.White;
            }
            else if (puntaje >= 114)
            {
                punt5.Font = new Font("Arial", 12f, FontStyle.Bold);
                punt5.BackColor = System.Drawing.ColorTranslator.FromHtml("#33acfd");
                punt5.ForeColor = Color.White;
                niv5.Font = new Font("Arial", 12f, FontStyle.Bold);
                niv5.BackColor = System.Drawing.ColorTranslator.FromHtml("#33acfd");
                niv5.ForeColor = Color.White;
                reco3b.Font = new Font("Arial", 12f, FontStyle.Bold);
                reco3b.BackColor = System.Drawing.ColorTranslator.FromHtml("#33acfd");
                reco3b.ForeColor = Color.White;
            }
        }
    }
}
