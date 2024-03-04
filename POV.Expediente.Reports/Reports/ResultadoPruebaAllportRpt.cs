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
    public partial class ResultadoPruebaAllportRpt : DevExpress.XtraReports.UI.XtraReport
    {
        private Alumno _alumno;
        private Usuario _usuario;
        private string _fechaFin;
        private Dictionary<string, string> _resAllport;
        private Series series1 = new Series("Puntaje", ViewType.Line);

        public ResultadoPruebaAllportRpt()
        {
            InitializeComponent();
        }

        public ResultadoPruebaAllportRpt(Alumno alumno, Usuario usuario, Dictionary<string, string> resAllport, string fechaFin)
        {
            InitializeComponent();
            this.SetData(alumno, usuario, resAllport, fechaFin);

        }

        public void SetData(Alumno alumno, Usuario usuario, Dictionary<string, string> resAllport, string fechaFin)
        {
            this._alumno = alumno;
            this._usuario = usuario;
            this._resAllport = resAllport;
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

            this.cValor.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccc");
            this.cValor.BorderColor = System.Drawing.ColorTranslator.FromHtml("#ccc");

            this.cInterpretacion.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccc");
            this.cInterpretacion.BorderColor = System.Drawing.ColorTranslator.FromHtml("#ccc");

            this.xrLine1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff9e19");
            this.xrLine2.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff9e19");
            #endregion

            #region Allport
            ResaltarResultadoAllport(resAllport);
            #endregion
            this.lblAnio.Text = "©" + DateTime.Now.ToString("yyyy") + ", Todos los derechos reservados YOY Vocacional";
        }

        private void ResaltarResultadoAllport(Dictionary<string, string> plantillas)
        {
            if (plantillas.Count > 0)
            {
                foreach (var item in plantillas)
                {
                    string plantilla = item.Key;
                    string valor = string.Empty;
                    string meta = string.Empty;

                    switch (plantilla)
                    {
                        case "1 Teorica":
                            valor = "Alto Valor Teórico - Búsqueda de la Verdad";
                            meta = "El interés dominante de esta persona tiende a ser la búsqueda del conocimiento. Innatamente curioso, está interesado en el aprendizaje y el " 
                                 + "proceso del razonamiento. Tendiente a intelectualizar o teorizar, esta persona puede tratar de arreglar las ideas o la información en base a " 
                                 + "sus análisis o en base a sistemas lógicos. Más que aceptar las cosas tal cual son, tiende a ser inquisitivo y crítico. Trata de organizar y " 
                                 + "sistematizar el conocimiento con investigación y validación. Estudioso tanto como creativo, es una buena fuente de información y de ideas.";
                            break;
                        case "2 Economico":
                            valor = "Alto Valor Económico - Sentido de Negocio y Búsqueda de Bienestar";
                            meta = "Tiende a ser impulsado por un abundante interés en el dinero, las utilidades y la posición económica. Ve todas las cosas e ideas en su " 
                                 + "ambiente como parte de una estructura materialista. Esta persona tiende a ser práctico y darle un valor monetario a todo. Siempre está " 
                                 + "buscando utilidades o rendimientos sobre la inversión. Tiene un deseo de ganancias materiales personales y, en el sentido empresarial, "
                                 + "aprecia los resultados positivos de un Centro de Utilidades. Asimismo, respeta los logros económicos de la gente que está muy consciente de "
                                 + "las utilidades o de los costos. Es adquisitivo y con frecuencia competitivamente, quiere que los demás sepan que sus posesiones tienen un " 
                                 + "valor en el mercado.";
                            break;
                        case "3 Estetico":
                            valor = "Alto Valor Artístico - Apreciación de la Belleza";
                            meta = "Esta persona tiende a buscar la realización artística en las áreas de expresión cultural. Se siente enormemente atraído por las formas, "
                                 + "la armonía, la gracia y la simetría. Sus intereses pueden variar desde la música hasta las bellas artes y la naturaleza. Las percepciones de "
                                 + "esta persona pueden variar desde individualistas hasta “vanguardistas”. Es probable que sea un perfeccionista con respecto al diseño, los "
                                 + "colores y los detalles. Asimismo, exige libertad para crear lo “suyo”. Su clara sensibilidad hacia lo bello puede, por otro lado, ir "
                                 + "acompañada de una intolerancia hacia lo feo. Puede en ocasiones argumentar la falta de belleza o falta de sensibilidad artística en algunas "
                                 + "tareas que le sean presentadas por otras personas.";
                            break;
                        case "4 Social":
                            valor = "Alto Valor Social - Preocupación por la Gente";
                            meta = "Esta persona tiende a tener buenos sentimientos hacia toda la gente por igual, sean propios o extraños. Desinteresadamente intenta mejorar " 
                                 + "el bienestar de los demás. Su fuerte interés humanitario y su sentido de justicia social son lo que mueve a esta persona a actuar."
                                 +"\n"+
                                 "En asuntos donde se trate asuntos sobre el bienestar de gente, esta persona puede emitir juicios subjetivos, emocionales o idealistas. Su "
                                 + "interés humano lo puede llevar a tener diferencias con gente económicamente motivada o en ocasiones pudiera pasar por alto controles de "
                                 + "costos o presupuestos, con tal de ayudar a la gente.";
                            break;
                        case "5 Politico":
                            valor = "Alto Valor Político - Búsqueda de la Autoridad y del Status";
                            meta = "Tiende a buscar status y poder en la sociedad o en la Organización. Es una persona que tiene ambiciones en la vida y que busca oportunidades "
                                 + "que le brinden proyección en la Organización y oportunidades de avanzar en la jerarquía organizacional. Esta persona le gusta tener gente "
                                 + "bajo su responsabilidad, ya que le gusta tener influencia sobre otras personas. Le emocionan, más que apenarlo los reconocimientos personales. "
                                 + "Es una persona que está dispuesto a pagar el precio personal con tal de obtener ascensos, reconocimiento y puestos desde donde pueda "
                                 + "controlar.";
                            break;
                        case "6 Religioso":
                            valor = "Alto Valor Regulatorio - Respeto a la Autoridad";
                            meta = "Tiende a identificarse con una fuerza del bien y a gobernar su bien organizada vida según un código de conducta muy personal. Es una persona "
                                 + "que desea orden y unidad en todo lo que hace. Lo “bueno” y lo “malo” con respecto a sus estándares de conducta son muy observados por esta "
                                 + "persona, ya que siempre busca que se actúe “correctamente”. Generalmente, es disciplinado, controlado y observa los estándares establecidos. "
                                 + "Es una persona que conduce su vida, ganándose el respeto de quienes lo rodean.";
                            break;
                    }
                    if (string.IsNullOrEmpty(PlantillaAll1.Text) && string.IsNullOrEmpty(ObjetivoAll1.Text))
                    {
                        PlantillaAll1.Text = valor;
                        ObjetivoAll1.Text = meta;
                        continue;
                    }
                    else if (string.IsNullOrEmpty(PlantillaAll2.Text) && string.IsNullOrEmpty(ObjetivoAll2.Text))
                    {
                        PlantillaAll2.Text = valor;
                        ObjetivoAll2.Text = meta;
                        continue;
                    }
                    else if (string.IsNullOrEmpty(PlantillaAll3.Text) && string.IsNullOrEmpty(ObjetivoAll3.Text))
                    {
                        PlantillaAll3.Text = valor;
                        ObjetivoAll3.Text = meta;
                        continue;
                    }
                }
            }
        }
    }
}
