using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using POV.CentroEducativo.BO;
using POV.Seguridad.BO;
using POV.Prueba.Diagnostico.Dinamica.BO;
using DevExpress.Office;

namespace POV.Expediente.Reports.Reports
{
    public partial class ResultadoPruebaSACKSRpt : DevExpress.XtraReports.UI.XtraReport
    {
        private Alumno _alumno;
        private Usuario _usuario;
        private string _fechaFin;

        public ResultadoPruebaSACKSRpt()
        {
            InitializeComponent();
        }

        public ResultadoPruebaSACKSRpt(SumarioGeneralSacks sumarioGeneralSacks, Alumno alumno, Usuario usuario, string fechaFin)
        {
            InitializeComponent();
            this.SetData(sumarioGeneralSacks, alumno, usuario, fechaFin);

        }

        public void SetData(SumarioGeneralSacks sumarioGeneralSacks, Alumno alumno, Usuario usuario, string fechaFin)
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

            this.TableInfoPrueba.BackColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");
            this.TableInfoPrueba.BorderColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");

            this.TableInfoEstudiante.BackColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");
            this.TableInfoEstudiante.BorderColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");

            this.TableResultPrueba.BackColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");
            this.TableResultPrueba.BorderColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");

            this.xrLine1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff9e19");
            this.xrLine2.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff9e19");
            #endregion

            cMadurez.Text = sumarioGeneralSacks.SumarioMadurez;
            cRealidad.Text = sumarioGeneralSacks.SumarioNivelRealida;
            cConflictos.Text = sumarioGeneralSacks.SumarioConflictoExpresados;
            this.lblAnio.Text = "©" + DateTime.Now.ToString("yyyy") + ", Todos los derechos reservados YOY Vocacional";
        }
    }
}
