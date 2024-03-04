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
    public partial class ResultadoPruebaRotterRpt : DevExpress.XtraReports.UI.XtraReport
    {
        private Alumno _alumno;
        private Usuario _usuario;
        private int _resRotter;
        private string _fechaFin;

        public ResultadoPruebaRotterRpt()
        {
            InitializeComponent();
        }

        public ResultadoPruebaRotterRpt(Alumno alumno, Usuario usuario, int resRotter, string fechaFin)
        {
            InitializeComponent();
            this.SetData(alumno, usuario, resRotter, fechaFin);

        }

        public void SetData(Alumno alumno, Usuario usuario, int resRotter, string fechaFin)
        {
            this._alumno = alumno;
            this._usuario = usuario;
            this._resRotter = resRotter;
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

            this.xrLine1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff9e19");
            this.xrLine2.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff9e19");
            #endregion

            this.cNombre.Text = _alumno.Nombre + " " + " " + _alumno.PrimerApellido + " " + _alumno.SegundoApellido;
            this.cCorreo.Text = _usuario.Email;
            this.cFinalPrueba.Text = _fechaFin;

            #region Habitos
            ResaltarResultadoHabitos(_resRotter);
            #endregion
            this.lblAnio.Text = "©" + DateTime.Now.ToString("yyyy") + ", Todos los derechos reservados YOY Vocacional";
        }

        private void ResaltarResultadoHabitos(int puntaje)
        {
            if (puntaje >= 0 && puntaje <= 11)
            {
                locusCapital.Text = "I";
                locusTexto.Text = "Internalidad del Locus de Control";
                locusDescripcion.Text = "Los individuos con un locus de control interno creen que los eventos son el resultado principalmente de su propia conducta y de sus "+
                                          "propias acciones, por lo que hace que se perciban como capaces de influir en su propio destino, de transformar una situación "+
                                          "desfavorable, o de incrementar su probabilidad de éxito. La percepción de control sobre la situación aumenta la motivación para "+
                                          "enfrentarla, de esta manera se espera que las personas con un locus de control interno se sientan más comprometidas, y actúen de "+
                                          "forma más activa ante la situación.\n"+
                                          "Estar convencido de la posibilidad de afectar el funcionamiento del mundo a nuestro alrededor (locus de control interno) ha "+
                                          "demostrado ser un elemento positivo en el rendimiento educativo, el campo laboral  y la prevención de la salud.\n"+
                                          "En el campo de las relaciones interpersonales, estas personas tienen un buen control de su comportamiento y tienden a expresarse "+
                                          "mejor socialmente.";
            }
            else if (puntaje >= 12)
            {
                locusCapital.Text = "E";
                locusTexto.Text = "Externalidad del Locus de control";
                locusDescripcion.Text = "Aquellos con un locus de control externo creen que son las otras personas (o situaciones) quienes determinan el rumbo de los "+
                                          "eventos. Este tipo de personas apreciarían que los resultados de sus conductas obedecen a factores ajenos a su control, como la "+
                                          "suerte, el destino o la participación de otras personas, no reconociendo en ellas mismas la capacidad de afectar el curso de los "+
                                          "eventos y de influir mediante sus acciones en el control de las contingencias de refuerzo que seguirán a su comportamiento.\n"+
                                          "Las personas con un locus de control externo pueden funcionar muy bien en el campo de lo laboral, incluso con las metas adecuadas "+
                                          "y los estímulos adecuados en el medio, pueden llegar a niveles muy elevados.\n"+
                                          "En el plano de las relaciones interpersonales, aquellos con un locus de control externo es más probable que intenten influir en "+
                                          "las personas y mantener cercanía (incluso dependencia), ya que son las personas las que ofrecen los refuerzos que la persona "+
                                          "requiere.";
            }

            if (puntaje >= 0 && puntaje <= 5) 
            {
                locusIndice.Text = "A";
            }
            else if (puntaje >= 6 && puntaje <= 9) 
            {
                locusIndice.Text = "M";
            }
            else if (puntaje >= 10 && puntaje <= 11) 
            {
                locusIndice.Text = "B";
            }
            else if (puntaje >= 12 && puntaje <= 14) 
            {
                locusIndice.Text = "B";
            }
            else if (puntaje >= 15 && puntaje <= 18) 
            {
                locusIndice.Text = "M";
            }
            else if (puntaje >= 19 && puntaje <= 23) 
            {
                locusIndice.Text = "A";
            }
        }
    }
}
