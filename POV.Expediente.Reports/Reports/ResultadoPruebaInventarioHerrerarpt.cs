using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using DevExpress.XtraCharts;
using System.Collections.Generic;
using POV.CentroEducativo.BO;
using POV.Seguridad.BO;

namespace POV.Expediente.Reports.Reports
{
    public partial class ResultadoPruebaInventarioHerrerarpt : DevExpress.XtraReports.UI.XtraReport
    {
        private CentroEducativo.BO.Alumno alumno;
        private Seguridad.BO.Usuario usuario;
        private System.Collections.Generic.Dictionary<string, string> SS_RespuestaInvHerr;
        private string p;

        public ResultadoPruebaInventarioHerrerarpt()
        {
            InitializeComponent();
        }

        public ResultadoPruebaInventarioHerrerarpt(CentroEducativo.BO.Alumno alumno, Seguridad.BO.Usuario usuario, System.Collections.Generic.Dictionary<string, string> SS_RespuestaInvHerr, string p)
        {
            // TODO: Complete member initialization
            InitializeComponent();

            SetData(alumno, usuario, SS_RespuestaInvHerr, p);
        }

        public void SetData(Alumno alumno, Usuario usuario, IDictionary<string, string> respuestasInventario, string fechaFin)
        {

            this.cNombre.Text = alumno.Nombre + " " + alumno.PrimerApellido + " " + alumno.SegundoApellido;
            this.cCorreo.Text = usuario.Email;
            this.cFinalPrueba.Text = fechaFin;


            Series ss = new Series("Servicio Social", ViewType.Bar);
            Series ep = new Series("Ejecutivo Persuasivo", ViewType.Bar);
            Series v = new Series("Verbal", ViewType.Bar);
            Series ap = new Series("Artistico Palstico", ViewType.Bar);
            Series ms = new Series("Musical", ViewType.Bar);
            Series og = new Series("Oraganización", ViewType.Bar);
            Series ct = new Series("Cientifica", ViewType.Bar);
            Series cl = new Series("Calculo", ViewType.Bar);
            Series mc = new Series("Mecanica Constructiva", ViewType.Bar);
            Series al = new Series("Trabajo al Aire Libre", ViewType.Bar);
            Series ssapt = new Series("Servicio Social", ViewType.Bar);
            Series epapt = new Series("Ejecutivo Persuasivo", ViewType.Bar);
            Series vapt = new Series("Verbal", ViewType.Bar);
            Series apapt = new Series("Artistico Palstico", ViewType.Bar);
            Series msapt = new Series("Musical", ViewType.Bar);
            Series ogapt = new Series("Oraganización", ViewType.Bar);
            Series ctapt = new Series("Cientifica", ViewType.Bar);
            Series clapt = new Series("Calculo", ViewType.Bar);
            Series mcapt = new Series("Mecanica Constructiva", ViewType.Bar);
            Series dt = new Series("Trabajo al Aire Libre", ViewType.Bar);

           
          

            auxiliar[] top3 = new auxiliar[20];
            top3[0] = new auxiliar("ss", Int32.Parse(respuestasInventario["ss"]));
            top3[1] = new auxiliar("ep", Int32.Parse(respuestasInventario["ep"]));
            top3[2] = new auxiliar("v", Int32.Parse(respuestasInventario["v"]));
            top3[3] = new auxiliar("ap", Int32.Parse(respuestasInventario["ap"]));
            top3[4] = new auxiliar("ms", Int32.Parse(respuestasInventario["ms"]));
            top3[5] = new auxiliar("og", Int32.Parse(respuestasInventario["og"]));
            top3[6] = new auxiliar("ct", Int32.Parse(respuestasInventario["ct"]));
            top3[7] = new auxiliar("cl", Int32.Parse(respuestasInventario["cl"]));
            top3[8] = new auxiliar("mc", Int32.Parse(respuestasInventario["mc"]));
            top3[9] = new auxiliar("al", Int32.Parse(respuestasInventario["al"]));
            top3[10] = new auxiliar("ssapt", Int32.Parse(respuestasInventario["ssapt"]));
            top3[11] = new auxiliar("epapt", Int32.Parse(respuestasInventario["epapt"]));
            top3[12] = new auxiliar("vapt", Int32.Parse(respuestasInventario["vapt"]));
            top3[13] = new auxiliar("apapt", Int32.Parse(respuestasInventario["apapt"]));
            top3[14] = new auxiliar("msapt", Int32.Parse(respuestasInventario["msapt"]));
            top3[15] = new auxiliar("ogapt", Int32.Parse(respuestasInventario["ogapt"]));
            top3[16] = new auxiliar("ctapt", Int32.Parse(respuestasInventario["ctapt"]));
            top3[17] = new auxiliar("clapt", Int32.Parse(respuestasInventario["clapt"]));
            top3[18] = new auxiliar("mcapt", Int32.Parse(respuestasInventario["mcapt"]));
            top3[19] = new auxiliar("dt", Int32.Parse(respuestasInventario["dt"]));
            double porcentaje = 0;

            for (int i = 0; i < top3.Length; i++)
            {
                switch (top3[i].getClasificador())
                {
                    case "ss":
                        porcentaje = top3[i].getValor() * 4.16;
                        ss.Points.Add(new SeriesPoint(top3[i].getClasificador(), porcentaje));
                        break;
                    case "ep":
                        porcentaje = top3[i].getValor() * 4.16 ;
                        ep.Points.Add(new SeriesPoint(top3[i].getClasificador(), porcentaje));
                        break;
                    case "v":
                        porcentaje = top3[i].getValor() * 4.16 ;
                        v.Points.Add(new SeriesPoint(top3[i].getClasificador(), porcentaje));
                        break;
                    case "ap":
                        porcentaje = top3[i].getValor() * 4.16;
                        ap.Points.Add(new SeriesPoint(top3[i].getClasificador(), porcentaje));
                        break;
                    case "ms":
                        porcentaje = top3[i].getValor() * 4.16;
                        ms.Points.Add(new SeriesPoint(top3[i].getClasificador(), porcentaje));
                        break;
                    case "og":
                        porcentaje = top3[i].getValor() * 4.16;
                        Console.Write(porcentaje);
                        og.Points.Add(new SeriesPoint(top3[i].getClasificador(), porcentaje));
                        break;
                    case "ct":
                        porcentaje = top3[i].getValor() * 4.16;
                        ct.Points.Add(new SeriesPoint(top3[i].getClasificador(), porcentaje));
                        break;
                    case "cl":
                        porcentaje = top3[i].getValor() * 4.16;
                        
                        cl.Points.Add(new SeriesPoint(top3[i].getClasificador(), porcentaje));
                        break;
                    case "mc":
                        porcentaje = top3[i].getValor() * 4.16;                
                        mc.Points.Add(new SeriesPoint(top3[i].getClasificador(), porcentaje));
                        break;
                    case "al":
                        porcentaje = top3[i].getValor() * 4.16;
                        al.Points.Add(new SeriesPoint(top3[i].getClasificador(), porcentaje));
                        break;
                        case "ssapt":
                        porcentaje = top3[i].getValor() * 4.16;
                        ssapt.Points.Add(new SeriesPoint(top3[i].getClasificador(), porcentaje));
                        break;
                        case "epapt":
                        porcentaje = top3[i].getValor() * 4.16 ;
                        epapt.Points.Add(new SeriesPoint(top3[i].getClasificador(), porcentaje));
                        break;
                        case "vapt":
                        porcentaje = top3[i].getValor() * 4.16 ;
                        vapt.Points.Add(new SeriesPoint(top3[i].getClasificador(), porcentaje));
                        break;
                        case "apapt":
                        porcentaje = top3[i].getValor() * 4.16;
                        apapt.Points.Add(new SeriesPoint(top3[i].getClasificador(), porcentaje));
                        break;
                        case "msapt":
                        porcentaje = top3[i].getValor() * 4.16;
                        msapt.Points.Add(new SeriesPoint(top3[i].getClasificador(), porcentaje));
                        break;
                        case "ogapt":
                        porcentaje = top3[i].getValor() * 4.16;

                        ogapt.Points.Add(new SeriesPoint(top3[i].getClasificador(), porcentaje));
                        break;
                        case "ctapt":
                        porcentaje = top3[i].getValor() * 4.16;
                        ctapt.Points.Add(new SeriesPoint(top3[i].getClasificador(), porcentaje));
                        break;
                        case "clapt":
                        porcentaje = top3[i].getValor() * 4.16;

                        clapt.Points.Add(new SeriesPoint(top3[i].getClasificador(), porcentaje));
                        break;
                        case "mcapt":
                        porcentaje = top3[i].getValor() * 4.16;
                        mcapt.Points.Add(new SeriesPoint(top3[i].getClasificador(), porcentaje));
                        break;
                        case "dt":
                        porcentaje = top3[i].getValor() * 4.16;
                        dt.Points.Add(new SeriesPoint(top3[i].getClasificador(), porcentaje));
                        break;
                }
            }


            Series series1 = new Series("Aptitudes", ViewType.Bar);
            series1.Points.Add(new SeriesPoint("SS", double.Parse(respuestasInventario["ssapt"]) * 4.16));
            series1.Points.Add(new SeriesPoint("EP", double.Parse(respuestasInventario["epapt"]) * 4.16));
            series1.Points.Add(new SeriesPoint("V", double.Parse(respuestasInventario["vapt"]) * 4.16));
            series1.Points.Add(new SeriesPoint("AP", double.Parse(respuestasInventario["apapt"]) * 4.16));
            series1.Points.Add(new SeriesPoint("MS", double.Parse(respuestasInventario["msapt"]) * 4.16));
            series1.Points.Add(new SeriesPoint("OG", double.Parse(respuestasInventario["ogapt"]) * 4.16));
            series1.Points.Add(new SeriesPoint("CT", double.Parse(respuestasInventario["ctapt"]) * 4.16));
            series1.Points.Add(new SeriesPoint("CL", double.Parse(respuestasInventario["clapt"]) * 4.16));
            series1.Points.Add(new SeriesPoint("MC", double.Parse(respuestasInventario["mcapt"]) * 4.16));
            series1.Points.Add(new SeriesPoint("DT", double.Parse(respuestasInventario["dt"]) * 4.16));

            Series series2 = new Series("Intereses", ViewType.Bar);
            series2.Points.Add(new SeriesPoint("SS", double.Parse(respuestasInventario["ss"]) * 4.16));
            series2.Points.Add(new SeriesPoint("EP", double.Parse(respuestasInventario["ep"]) * 4.16));
            series2.Points.Add(new SeriesPoint("V", double.Parse(respuestasInventario["v"])* 4.16));
            series2.Points.Add(new SeriesPoint("AP", double.Parse(respuestasInventario["ap"]) * 4.16));
            series2.Points.Add(new SeriesPoint("MS", double.Parse(respuestasInventario["ms"]) * 4.16));
            series2.Points.Add(new SeriesPoint("OG", double.Parse(respuestasInventario["og"]) * 4.16));
            series2.Points.Add(new SeriesPoint("CT", double.Parse(respuestasInventario["ct"]) * 4.16));
            series2.Points.Add(new SeriesPoint("CL", double.Parse(respuestasInventario["cl"]) * 4.16));
            series2.Points.Add(new SeriesPoint("MC", double.Parse(respuestasInventario["mc"]) * 4.16));
            series1.Points.Add(new SeriesPoint("AL", double.Parse(respuestasInventario["al"]) * 4.16));


                xrChartIntereses.Series.Add(ss);
                xrChartIntereses.Series.Add(ep);
                xrChartIntereses.Series.Add(v);
                xrChartIntereses.Series.Add(ap);
                xrChartIntereses.Series.Add(ms);
                xrChartIntereses.Series.Add(og);
                xrChartIntereses.Series.Add(ct);
                xrChartIntereses.Series.Add(cl);
                xrChartIntereses.Series.Add(mc);
                xrChartIntereses.Series.Add(al);

                xrChartAptitudes.Series.Add(ssapt);
                xrChartAptitudes.Series.Add(epapt);
                xrChartAptitudes.Series.Add(vapt);
                xrChartIntereses.Series.Add(apapt);
                xrChartAptitudes.Series.Add(msapt);
                xrChartAptitudes.Series.Add(ogapt);
                xrChartAptitudes.Series.Add(ctapt);
                xrChartAptitudes.Series.Add(clapt);
                xrChartAptitudes.Series.Add(mcapt);
                xrChartAptitudes.Series.Add(dt);

                xrCharttodo.Series.Add(series1);
                xrCharttodo.Series.Add(series2);

                this.lblAnio.Text = "©" + DateTime.Now.ToString("yyyy") + ", Todos los derechos reservados YOY Vocacional";
            }
        }
    }

