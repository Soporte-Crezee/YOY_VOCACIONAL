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
    public partial class ResultadoPruebaInventariodeInteresesRpt : DevExpress.XtraReports.UI.XtraReport
    {
        private CentroEducativo.BO.Alumno alumno;
        private Seguridad.BO.Usuario usuario;
        private System.Collections.Generic.Dictionary<string, string> SS_RespuestaIntMul;
        private string p;

        public ResultadoPruebaInventariodeInteresesRpt()
        {
            InitializeComponent();
        }

        public ResultadoPruebaInventariodeInteresesRpt(CentroEducativo.BO.Alumno alumno, Seguridad.BO.Usuario usuario, System.Collections.Generic.Dictionary<string, string> SS_RespuestaInv, string p)
        {
            // TODO: Complete member initialization
            InitializeComponent();

            SetData(alumno, usuario, SS_RespuestaInv, p);
        }
        public void SetData(Alumno alumno, Usuario usuario, IDictionary<string, string> respuestasInventario, string fechaFin)
        {

            this.cNombre.Text = alumno.Nombre + " " + alumno.PrimerApellido + " " + alumno.SegundoApellido;
            this.cCorreo.Text = usuario.Email;
            this.cFinalPrueba.Text = fechaFin;
            Series calculo = new Series("Cálculo", ViewType.Bar);
            Series cien = new Series("Científico-físico", ViewType.Bar);
            Series bio = new Series("Científico Biológico", ViewType.Bar);
            Series mec = new Series("Mecánico", ViewType.Bar);
            Series sersoc = new Series("Servicio Social", ViewType.Bar);
            Series lit = new Series("Literario", ViewType.Bar);
            Series persu = new Series("Persuasivo", ViewType.Bar);
            Series art = new Series("Artístico", ViewType.Bar);
            Series mus = new Series("Musical", ViewType.Bar);
            #region Resultados
            #region Cálculo de los 3 rangos mas altos
            auxiliar[] top3 = new auxiliar[9];
            top3[0] = new auxiliar("Cálculo", Int32.Parse(respuestasInventario["Cálculo"]));
            top3[1] = new auxiliar("Científico-físico", Int32.Parse(respuestasInventario["Científico-físico"]));
            top3[2] = new auxiliar("Científico Biológico", Int32.Parse(respuestasInventario["Científico Biológico"]));
            top3[3] = new auxiliar("Mecánico", Int32.Parse(respuestasInventario["Mecánico"]));
            top3[4] = new auxiliar("Servicio Social", Int32.Parse(respuestasInventario["Servicio Social"]));
            top3[5] = new auxiliar("Literario", Int32.Parse(respuestasInventario["Literario"]));
            top3[6] = new auxiliar("Persuasivo", Int32.Parse(respuestasInventario["Persuasivo"]));
            top3[7] = new auxiliar("Artístico", Int32.Parse(respuestasInventario["Artístico"]));
            top3[8] = new auxiliar("Musical", Int32.Parse(respuestasInventario["Musical"]));

            auxiliar t;
            for (int a = 1; a < top3.Length; a++)
                for (int b = top3.Length - 1; b >= a; b--)
                {
                    if (top3[b - 1].getValor() > top3[b].getValor())
                    {
                        t = top3[b - 1];
                        top3[b - 1] = top3[b];
                        top3[b] = t;
                    }
                }

            int porcentaje = 0;
            for (int i = 0; i < top3.Length; i++)
            {
                switch (top3[i].getClasificador())
                {
                    case "Cálculo":
                        porcentaje = top3[i].getValor() * 2;
                        calculo.Points.Add(new SeriesPoint(top3[i].getClasificador(), porcentaje.ToString()));
                        break;
                    case "Científico-físico":
                        porcentaje = top3[i].getValor() * 2;
                        cien.Points.Add(new SeriesPoint(top3[i].getClasificador(), porcentaje.ToString()));
                        break;
                    case "Científico Biológico":
                        porcentaje = top3[i].getValor() * 2;
                        bio.Points.Add(new SeriesPoint(top3[i].getClasificador(), porcentaje.ToString()));
                        break;
                    case "Mecánico":
                        porcentaje = top3[i].getValor() * 2;
                        mec.Points.Add(new SeriesPoint(top3[i].getClasificador(), porcentaje.ToString()));
                        break;
                    case "Servicio Social":
                        porcentaje = top3[i].getValor() * 2;
                        sersoc.Points.Add(new SeriesPoint(top3[i].getClasificador(), porcentaje.ToString()));
                        break;
                    case "Literario":
                        porcentaje = top3[i].getValor() * 2;
                        lit.Points.Add(new SeriesPoint(top3[i].getClasificador(), porcentaje.ToString()));
                        break;
                    case "Persuasivo":
                        porcentaje = top3[i].getValor() * 2;
                        persu.Points.Add(new SeriesPoint(top3[i].getClasificador(), porcentaje.ToString()));
                        break;
                    case "Artístico":
                        porcentaje = top3[i].getValor() * 2;
                        art.Points.Add(new SeriesPoint(top3[i].getClasificador(), porcentaje.ToString()));
                        break;
                    case "Musical":
                        porcentaje = top3[i].getValor() * 2;
                        mus.Points.Add(new SeriesPoint(top3[i].getClasificador(), porcentaje.ToString()));
                        break;
                }
            }

            #endregion

            xrChartInventario.Series.Add(calculo);
            xrChartInventario.Series.Add(cien);
            xrChartInventario.Series.Add(bio);
            xrChartInventario.Series.Add(mec);
            xrChartInventario.Series.Add(sersoc);
            xrChartInventario.Series.Add(lit);
            xrChartInventario.Series.Add(persu);
            xrChartInventario.Series.Add(art);
            xrChartInventario.Series.Add(mus);
            #endregion
            IntAltaDesc.Text = top3[8].getClasificador();
            Int2AltaDesc.Text = top3[7].getClasificador();
            Int3AltaDesc.Text = top3[6].getClasificador();

            this.lblAnio.Text = "©" + DateTime.Now.ToString("yyyy") + ", Todos los derechos reservados YOY Vocacional";
        }


    }

}