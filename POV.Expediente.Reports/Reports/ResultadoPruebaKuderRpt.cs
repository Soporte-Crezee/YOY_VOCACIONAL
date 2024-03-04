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
    public partial class ResultadoPruebaKuderRpt : DevExpress.XtraReports.UI.XtraReport
    {
        private Alumno _alumno;
        private Usuario _usuario;
        private Dictionary<string, string> _resKuder;
        private string _fechaFin;

        public ResultadoPruebaKuderRpt()
        {
            InitializeComponent();
        }

        public ResultadoPruebaKuderRpt(Alumno alumno, Usuario usuario, Dictionary<string, string> resKuder, string fechaFin)
        {
            InitializeComponent();
            this.SetData(alumno, usuario, resKuder, fechaFin);

        }

        public void SetData(Alumno alumno, Usuario usuario, Dictionary<string, string> resKuder, string fechaFin)
        {
            this._alumno = alumno;
            this._usuario = usuario;
            this._resKuder = resKuder;
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

            this.cCuadro.BackColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");
            this.cCuadro.BorderColor = System.Drawing.ColorTranslator.FromHtml("#ff9e19");

            this.TableSignificadoAreas.BackColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");
            this.TableSignificadoAreas.BorderColor = System.Drawing.ColorTranslator.FromHtml("#ff9e19");

            this.xrTableClasificacion.BackColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");
            this.xrTableClasificacion.BorderColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");

            this.xrTableCombinaciones.BackColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");
            this.xrTableCombinaciones.BorderColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");

            this.xrLine1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff9e19");
            this.xrLine2.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff9e19");
            #endregion

            #region Kuder
            ResaltarResultadoKuder(_resKuder);
            #endregion

            this.lblAnio.Text = "©" + DateTime.Now.ToString("yyyy") + ", Todos los derechos reservados YOY Vocacional";
        }

        private void ResaltarResultadoKuder(Dictionary<string, string> plantillas)
        {
            if (plantillas.Count > 0)
            {
                foreach (var item in plantillas)
                {
                    string plantillaSeleccionado = item.Key;
                    string descripcion = string.Empty;
                    string plantilla = string.Empty;

                    switch (plantillaSeleccionado)
                    {
                        case "0 Area Libre":
                            descripcion = @"Altos puntajes en esta área significan que al examinado le gusta pasar la mayor parte del tiempo en, el campo, 
                                            en los bosques o en el mar, le agrada cultivar plantas, cuidar animales, etc. En cambio, no se sentiría muy a gusto en 
                                            una fábrica, en un laboratorio o en una oficina.";
                            plantilla = @"“0” Actividad al aire libre     
                                          • Licenciatura en oceanología.
                                          • Licenciatura en turismo.
                                          • Profesores de educación física";
                            break;
                        case "1 Mecanico":
                            descripcion = @"Un alto puntaje aquí indica interés para trabajar con máquinas y herramientas, construir o arreglar objetos mecánicos, 
                                            artefactos eléctricos, muebles, etc.";
                            plantilla = @"“1” Interés mecánico
                                          • Ingeniería en mecatrónica.                                                
                                          • Licenciatura en radio y televisión, Piloto aviadores.
                                          • Química Aplicada.";
                            break;
                        case "2 Calculo":
                            descripcion = @"Lo poseen aquellas personas a quienes les gusta trabajar con números. Muchos ingenieros revelan también un marcado interés 
                                            por las actividades relacionadas con el cálculo.";
                            plantilla = @"“2” Interés para el cálculo
                                          • Enseñanza de las matemáticas.
                                          • Ingeniería en computación.
                                          • Licenciatura en economía.";
                            break;
                        case "3 Cientifico":
                            descripcion = @"Manifiestan este interés las personas que encuentran placer en investigar la razón de los hechos o de las cosas, en 
                                            descubrir sus causas y en resolver problemas de distinta índole, por mera curiosidad científica y sin pensar en los beneficios 
                                            económicos que puedan resultar de sus descubrimientos. El interés científico es de gran importancia en el ejercicio de muchas 
                                            carreras profesionales, aun de aquéllas donde el móvil de la actividad puede ser de índole distinta al progreso de la ciencia.";
                            plantilla = @"“3” Interés científico
                                          • Antropólogos, Arqueólogos.
                                          • Licenciatura en nutrición.
                                          • Médico cirujano.";
                            break;
                        case "4 Persuasivo":
                            descripcion = @"Lo poseen aquellas personas a quienes les gusta tratar con la gente, imponer sus puntos de vista, convencer a los demás 
                                            respecto a algún proyecto, venderles un artículo, etc.";
                            plantilla = @"“4” Interés persuasivo
                                          • Licenciatura en periodismo. 
                                          • Locución de radio y TV. 
                                          • Licenciatura en ciencias de la comunicación.";
                            break;
                        case "5 Artistico":
                            descripcion = @"Lo poseen las personas a quienes les agrada hacer trabajos de creación de tipo manual, usando combinaciones de colores, 
                                            materiales, formas y diseños.";
                            plantilla = @"“5” Interes Artístico-plástico
                                          • Animación digital. 
                                          • Licenciatura en diseño de productos.
                                          • Licenciatura en diseño gráfico.";
                            break;
                        case "6 Literario":
                            descripcion = @"Es propio de todos aquellos a quienes les gusta la lectura o encuentran placer en expresar sus ideas en forma oral o escrita.";
                            plantilla = @"“6” Interés literario
                                          • Estudio de idiomas.
                                          • Licenciatura en derecho.
                                          • Licenciatura en traducción.";
                            break;
                        case "7 Musical":
                            descripcion = @"Se sitúan aquí las personas que muestran un marcado gusto para tocar instrumentos musicales, cantar, bailar, leer sobre 
                                            música, estudiar la vida de compositores famosos, asistir a conciertos, etc.";
                            plantilla = @"“7” Interés musical
                                          • Artistas de Ballet.
                                          • Licenciatura en ciencias del arte.
                                          • Licenciatura en teatro, Profesores de música.";
                            break;
                        case "8 Social":
                            descripcion = @"Un alto puntaje en esta área indica un gran interés por servir a los demás: a los necesitados, enfermos, niños y ancianos.";
                            plantilla = @"“8” Interés para el servicio social
                                          • Cirujano dentista.   
                                          • Fisioterapia y rehabilitación.
                                          • Licenciatura en desarrollo comunitario.";
                            break;
                        case "9 Oficina":
                            descripcion = @"Es propio de las personas a quienes les gusta un tipo de trabajo de escritorio, que requiere exactitud y precisión.";
                            plantilla = @"“9” Interés en el trabajo de oficina
                                          • Estudios en telemática.
                                          • Licenciatura en bibliotecología.
                                          • Psicología organizacional.";
                            break;
                    }
                    if (string.IsNullOrEmpty(Plantilla1.Text) && string.IsNullOrEmpty(Descripcion1.Text))
                    {
                        Plantilla1.Text = plantilla;
                        Descripcion1.Text = descripcion;
                        continue;
                    }
                    else if (string.IsNullOrEmpty(Plantilla2.Text) && string.IsNullOrEmpty(Descripcion2.Text))
                    {
                        Plantilla2.Text = plantilla;
                        Descripcion2.Text = descripcion;
                        continue;
                    }
                    else if (string.IsNullOrEmpty(Plantilla3.Text) && string.IsNullOrEmpty(Descripcion3.Text))
                    {
                        Plantilla3.Text = plantilla;
                        Descripcion3.Text = descripcion;
                        continue;
                    }
                }
            }
        }
    }
}
