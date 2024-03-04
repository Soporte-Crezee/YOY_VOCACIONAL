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
    public partial class ResultadoPruebaTermanMerrillRpt : DevExpress.XtraReports.UI.XtraReport
    {
        private Series series1 = new Series("Puntaje", ViewType.Line);

        public ResultadoPruebaTermanMerrillRpt()
        {
            InitializeComponent();
        }

        public ResultadoPruebaTermanMerrillRpt(Alumno alumno, Usuario usuario, DataSet resTerman, string fechaFin)
        {
            InitializeComponent();
            try
            {
                this.SetData(alumno, usuario, resTerman, fechaFin);
            }
            catch (Exception ex)
            {
                throw;
            }


        }

        public void SetData(Alumno alumno, Usuario usuario, DataSet resTerman, string fechaFin)
        {
            this.cNombre.Text = alumno.Nombre + " " + " " + alumno.PrimerApellido + " " + alumno.SegundoApellido;
            this.cCorreo.Text = usuario.Email;
            this.cFinalPrueba.Text = fechaFin;

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

            this.cSerie.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccc");
            this.cSerie.BorderColor = System.Drawing.ColorTranslator.FromHtml("#ccc");

            this.cCategoria.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccc");
            this.cCategoria.BorderColor = System.Drawing.ColorTranslator.FromHtml("#ccc");

            this.cPuntuacion.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccc");
            this.cPuntuacion.BorderColor = System.Drawing.ColorTranslator.FromHtml("#ccc");

            this.cRango.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccc");
            this.cRango.BorderColor = System.Drawing.ColorTranslator.FromHtml("#ccc");

            this.cPuntosTotales.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccc");
            this.cPuntosTotales.BorderColor = System.Drawing.ColorTranslator.FromHtml("#ccc");

            this.cRangoFinal.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccc");
            this.cRangoFinal.BorderColor = System.Drawing.ColorTranslator.FromHtml("#ccc");
            this.xrTableCell14.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccc");
            this.xrTableCell14.BorderColor = System.Drawing.ColorTranslator.FromHtml("#ccc");

            this.cCIFinal.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccc");
            this.cCIFinal.BorderColor = System.Drawing.ColorTranslator.FromHtml("#ccc");
            this.xrTableCell20.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccc");
            this.xrTableCell20.BorderColor = System.Drawing.ColorTranslator.FromHtml("#ccc");

            this.xrLine1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff9e19");
            this.xrLine2.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff9e19");
            #endregion

            #region Terman
            ResaltarResultadoTerman(resTerman);
            #endregion

            this.lblAnio.Text = "©" + DateTime.Now.ToString("yyyy") + ", Todos los derechos reservados YOY Vocacional";
        }

        private void ResaltarResultadoTerman(DataSet series)
        {
            series.Tables[0].DefaultView.Sort = "ClasificadorID ASC";
            DataTable dtb = series.Tables[0].DefaultView.ToTable();
            series.Tables[0].Rows.Clear();
            foreach (DataRow row in dtb.Rows)
                series.Tables[0].Rows.Add(row.ItemArray);

            int serieUno = Convert.ToInt32(series.Tables[0].Rows[0].ItemArray[1].ToString());
            cPuntTerm1.Text = serieUno + "";

            int serieDos = Convert.ToInt32(series.Tables[0].Rows[1].ItemArray[1].ToString());
            cPuntTerm2.Text = serieDos + "";

            int serieTres = Convert.ToInt32(series.Tables[0].Rows[2].ItemArray[1].ToString());
            cPuntTerm3.Text = serieTres + "";

            double serieCuatro = Convert.ToDouble(series.Tables[0].Rows[3].ItemArray[1].ToString());
            cPuntTerm4.Text = serieCuatro + "";

            int serieCinco = Convert.ToInt32(series.Tables[0].Rows[4].ItemArray[1].ToString());
            cPuntTerm5.Text = serieCinco + "";

            int serieSeis = Convert.ToInt32(series.Tables[0].Rows[5].ItemArray[1].ToString());
            cPuntTerm6.Text = serieSeis + "";

            int serieSiete = Convert.ToInt32(series.Tables[0].Rows[6].ItemArray[1].ToString());
            cPuntTerm7.Text = serieSiete + "";

            int serieOcho = Convert.ToInt32(series.Tables[0].Rows[7].ItemArray[1].ToString());
            cPuntTerm8.Text = serieOcho + "";

            int serieNueve = Convert.ToInt32(series.Tables[0].Rows[8].ItemArray[1].ToString());
            cPuntTerm9.Text = serieNueve + "";

            int serieDiez = Convert.ToInt32(series.Tables[0].Rows[9].ItemArray[1].ToString());
            cPuntTerm10.Text = serieDiez + "";

            #region Puntuaciones
            #region Serie I
            if (serieUno >= 16)
                cTRango1.Text = "Sobresaliente";
            if (serieUno == 15)
                cTRango1.Text = "Superior";
            if (serieUno == 14)
                cTRango1.Text = "Térm. M. A";
            if (serieUno == 12 || serieUno == 13)
                cTRango1.Text = "Térm. Medio";
            if (serieUno == 10 || serieUno == 11)
                cTRango1.Text = "Térm. M. B.";
            if (serieUno == 8 || serieUno == 9)
                cTRango1.Text = "Inferior";
            if (serieUno <= 7)
                cTRango1.Text = "Deficiente";
            #endregion

            #region Serie II
            if (serieDos >= 21)
                cTRango2.Text = "Sobresaliente";
            if (serieDos == 20)
                cTRango2.Text = "Superior";
            if (serieDos >= 17 && serieDos <= 19)
                cTRango2.Text = "Térm. M. A";
            if (serieDos >= 12 && serieDos <= 16)
                cTRango2.Text = "Térm. Medio";
            if (serieDos == 10 || serieDos == 11)
                cTRango2.Text = "Térm. M. B.";
            if (serieDos >= 7 && serieDos <= 9)
                cTRango2.Text = "Inferior";
            if (serieDos <= 6)
                cTRango2.Text = "Deficiente";
            #endregion

            #region Serie III
            if (serieTres >= 29)
                cTRango3.Text = "Sobresaliente";
            if (serieTres == 27 || serieTres == 28)
                cTRango3.Text = "Superior";
            if (serieTres >= 23 && serieTres <= 26)
                cTRango3.Text = "Térm. M. A";
            if (serieTres >= 14 && serieTres <= 22)
                cTRango3.Text = "Térm. Medio";
            if (serieTres == 12 || serieTres == 13)
                cTRango3.Text = "Térm. M. B.";
            if (serieTres >= 8 && serieTres <= 11)
                cTRango3.Text = "Inferior";
            if (serieTres <= 7)
                cTRango3.Text = "Deficiente";
            #endregion

            #region Serie IV
            if (serieCuatro >= 18)
                cTRango4.Text = "Sobresaliente";
            if (serieCuatro == 16 || serieCuatro == 17)
                cTRango4.Text = "Superior";
            if (serieCuatro == 14 || serieCuatro == 15)
                cTRango4.Text = "Térm. M. A";
            if (serieCuatro >= 10 && serieCuatro <= 13)
                cTRango4.Text = "Térm. Medio";
            if (serieCuatro >= 7 && serieCuatro <= 9)
                cTRango4.Text = "Térm. M. B.";
            if (serieCuatro == 6)
                cTRango4.Text = "Inferior";
            if (serieCuatro <= 5)
                cTRango4.Text = "Deficiente";
            #endregion

            #region Serie V
            if (serieCinco >= 23)
                cTRango5.Text = "Sobresaliente";
            if (serieCinco >= 19 && serieCinco <= 22)
                cTRango5.Text = "Superior";
            if (serieCinco >= 16 && serieCinco <= 18)
                cTRango5.Text = "Térm. M. A";
            if (serieCinco >= 12 && serieCinco <= 15)
                cTRango5.Text = "Térm. Medio";
            if (serieCinco >= 7 && serieCinco <= 11)
                cTRango5.Text = "Térm. M. B.";
            if (serieCinco == 6)
                cTRango5.Text = "Inferior";
            if (serieCinco <= 5)
                cTRango5.Text = "Deficiente";
            #endregion

            #region Serie VI
            if (serieSeis >= 20)
                cTRango6.Text = "Sobresaliente";
            if (serieSeis == 18 || serieSeis == 19)
                cTRango6.Text = "Superior";
            if (serieSeis >= 15 && serieSeis <= 17)
                cTRango6.Text = "Térm. M. A";
            if (serieSeis >= 9 && serieSeis <= 14)
                cTRango6.Text = "Térm. Medio";
            if (serieSeis == 7 || serieSeis == 8)
                cTRango6.Text = "Térm. M. B.";
            if (serieSeis == 5 || serieSeis == 6)
                cTRango6.Text = "Inferior";
            if (serieSeis <= 4)
                cTRango6.Text = "Deficiente";
            #endregion

            #region Serie VII
            if (serieSiete >= 19)
                cTRango7.Text = "Sobresaliente";
            if (serieSiete == 18)
                cTRango7.Text = "Superior";
            if (serieSiete == 16 || serieSiete == 17)
                cTRango7.Text = "Térm. M. A";
            if (serieSiete >= 9 && serieSiete <= 15)
                cTRango7.Text = "Térm. Medio";
            if (serieSiete >= 6 && serieSiete <= 8)
                cTRango7.Text = "Térm. M. B.";
            if (serieSiete == 5)
                cTRango7.Text = "Inferior";
            if (serieSiete <= 4)
                cTRango7.Text = "Deficiente";
            #endregion

            #region Serie VIII
            if (serieOcho >= 17)
                cTRango8.Text = "Sobresaliente";
            if (serieOcho == 15 || serieOcho == 16)
                cTRango8.Text = "Superior";
            if (serieOcho == 13 || serieOcho == 14)
                cTRango8.Text = "Térm. M. A";
            if (serieOcho >= 8 && serieOcho <= 12)
                cTRango8.Text = "Térm. Medio";
            if (serieOcho == 7)
                cTRango8.Text = "Térm. M. B.";
            if (serieOcho == 6)
                cTRango8.Text = "Inferior";
            if (serieOcho <= 5)
                cTRango8.Text = "Deficiente";
            #endregion

            #region Serie IX
            if (serieNueve >= 18)
                cTRango9.Text = "Sobresaliente";
            if (serieNueve == 17)
                cTRango9.Text = "Superior";
            if (serieNueve == 16)
                cTRango9.Text = "Térm. M. A";
            if (serieNueve >= 10 && serieNueve <= 15)
                cTRango9.Text = "Térm. Medio";
            if (serieNueve == 9)
                cTRango9.Text = "Térm. M. B.";
            if (serieNueve == 7 || serieNueve == 8)
                cTRango9.Text = "Inferior";
            if (serieNueve <= 6)
                cTRango9.Text = "Deficiente";
            #endregion

            #region Serie X
            if (serieDiez >= 19)
                cTRango10.Text = "Sobresaliente";
            if (serieDiez == 18)
                cTRango10.Text = "Superior";
            if (serieDiez == 16 || serieDiez == 17)
                cTRango10.Text = "Térm. M. A";
            if (serieDiez >= 9 && serieDiez <= 15)
                cTRango10.Text = "Térm. Medio";
            if (serieDiez == 8)
                cTRango10.Text = "Térm. M. B.";
            if (serieDiez == 6 || serieDiez == 7)
                cTRango10.Text = "Inferior";
            if (serieDiez <= 5)
                cTRango10.Text = "Deficiente";
            #endregion
            #endregion

            #region Grafica
            List<CalificacionTerman> listCalificacion = new List<CalificacionTerman>();

            listCalificacion.AddRange(new CalificacionTerman[] 
            {
                new CalificacionTerman("Información",serieUno),
                new CalificacionTerman("Juicio",serieDos),
                new CalificacionTerman("Vocabulario",serieTres),
                new CalificacionTerman("Síntesis",serieCuatro),
                new CalificacionTerman("Concentración",serieCinco),
                new CalificacionTerman("Análisis",serieSeis),
                new CalificacionTerman("Abstracción",serieSiete),
                new CalificacionTerman("Planeación",serieOcho),
                new CalificacionTerman("Organizacón",serieNueve),
                new CalificacionTerman("Atención",serieDiez)
            });

            series1.ArgumentScaleType = ScaleType.Qualitative;
            series1.ValueScaleType = ScaleType.Numerical;

            series1.ArgumentDataMember = "NombreSerie";
            series1.ValueDataMembers[0] = "PuntajeSerie";

            cGrafTerman.Series.Add(series1);
            cGrafTerman.DataSource = listCalificacion;
            #endregion

            double totalPrev = serieUno + serieDos + serieTres + serieCuatro + serieCinco + serieSeis + serieSiete + serieOcho + serieNueve + serieDiez;
            int Total = Convert.ToInt32(totalPrev);
            cPuntTotTerman.Text = Total.ToString();

            #region Tabla de CI y Rango
            int CI = 0;
            double tmpTotal = double.Parse(cPuntTotTerman.Text);
            Total = Convert.ToInt32(tmpTotal);
            #region CI
            if (Total < 67)
                cCI.Text = Total + "";

            if (Total >= 67 && Total <= 69)
                cCI.Text = "80";

            if (Total == 70 || Total == 71)
                cCI.Text = "81";

            if (Total >= 72 && Total <= 74)
                cCI.Text = "82";

            if (Total == 75 || Total == 76)
                cCI.Text = "83";

            if (Total >= 77 && Total <= 80)
                cCI.Text = "84";

            if (Total == 81 || Total == 82)
                cCI.Text = "85";

            if (Total >= 83 && Total <= 85)
                cCI.Text = "86";

            if (Total == 86)
                cCI.Text = "87";

            if (Total >= 87 && Total <= 90)
                cCI.Text = "88";

            if (Total >= 91 && Total <= 93)
                cCI.Text = "89";

            if (Total >= 94 && Total <= 96)
                cCI.Text = "90";

            if (Total >= 97 && Total <= 99)
                cCI.Text = "91";

            if (Total >= 100 && Total <= 102)
                cCI.Text = "92";

            if (Total == 103 || Total == 104)
                cCI.Text = "93";

            if (Total == 105 || Total == 106)
                cCI.Text = "94";

            if (Total >= 107 && Total <= 110)
                cCI.Text = "95";

            if (Total >= 111 && Total <= 113)
                cCI.Text = "96";

            if (Total >= 114 && Total <= 117)
                cCI.Text = "97";

            if (Total == 118 || Total == 119)
                cCI.Text = "98";

            if (Total >= 120 && Total <= 123)
                cCI.Text = "99";

            if (Total == 124 || Total == 125)
                cCI.Text = "100";

            if (Total >= 126 && Total <= 129)
                cCI.Text = "101";

            if (Total >= 130 && Total <= 133)
                cCI.Text = "102";

            if (Total >= 134 && Total <= 137)
                cCI.Text = "103";

            if (Total >= 138 && Total <= 141)
                cCI.Text = "104";

            if (Total >= 142 && Total <= 145)
                cCI.Text = "105";

            if (Total >= 146 && Total <= 149)
                cCI.Text = "106";

            if (Total >= 150 && Total <= 153)
                cCI.Text = "107";

            if (Total >= 154 && Total <= 157)
                cCI.Text = "108";

            if (Total == 158 || Total == 159)
                cCI.Text = "109";

            if (Total >= 160 && Total <= 162)
                cCI.Text = "110";

            if (Total >= 163 && Total <= 166)
                cCI.Text = "111";

            if (Total == 167)
                cCI.Text = "112";

            if (Total >= 168 && Total <= 170)
                cCI.Text = "113";

            if (Total >= 171 && Total <= 173)
                cCI.Text = "114";

            if (Total == 174 || Total == 175)
                cCI.Text = "115";

            if (Total == 176 || Total == 177)
                cCI.Text = "116";

            if (Total >= 178 && Total <= 180)
                cCI.Text = "117";

            if (Total >= 181 && Total <= 183)
                cCI.Text = "118";

            if (Total == 184 || Total == 185)
                cCI.Text = "119";

            if (Total == 186)
                cCI.Text = "120";

            if (Total == 187)
                cCI.Text = "121";

            if (Total == 188)
                cCI.Text = "122";

            if (Total == 189)
                cCI.Text = "123";

            if (Total == 190)
                cCI.Text = "124";

            if (Total == 191)
                cCI.Text = "125";

            if (Total == 192)
                cCI.Text = "126";

            if (Total == 193)
                cCI.Text = "127";

            if (Total == 194)
                cCI.Text = "128";

            if (Total == 195)
                cCI.Text = "129";

            if (Total == 196)
                cCI.Text = "130";

            if (Total == 197)
                cCI.Text = "131";

            if (Total == 198)
                cCI.Text = "132";

            if (Total == 199)
                cCI.Text = "133";

            if (Total == 200)
                cCI.Text = "134";

            if (Total == 201)
                cCI.Text = "135";

            if (Total == 202)
                cCI.Text = "136";

            if (Total == 203)
                cCI.Text = "137";

            if (Total == 204)
                cCI.Text = "138";

            if (Total == 205)
                cCI.Text = "139";

            if (Total == 206)
                cCI.Text = "140";

            if (Total == 207)
                cCI.Text = "141";
            #endregion

            #region Rango
            double tmp = double.Parse(cCI.Text);
            CI = Convert.ToInt32(tmp);
            if (CI >= 140)
                cRangoTerman.Text = "Sobresaliente";
            if (CI >= 120 && CI <= 139)
                cRangoTerman.Text = "Superior";
            if (CI >= 110 && CI <= 119)
                cRangoTerman.Text = "Térm. M. A";
            if (CI >= 90 && CI <= 109)
                cRangoTerman.Text = "Normal";
            if (CI >= 80 && CI <= 89)
                cRangoTerman.Text = "Térm. M. B";
            if (CI >= 70 && CI <= 79)
                cRangoTerman.Text = "Inferior";
            if (CI <= 69)
                cRangoTerman.Text = "Deficiente";
            #endregion
            #endregion
        }
    }

    public class CalificacionTerman
    {
        public string NombreSerie { get; set; }
        public double PuntajeSerie { get; set; }

        public CalificacionTerman(string nombre, double puntaje)
        {
            this.NombreSerie = nombre;
            this.PuntajeSerie = puntaje;
        }
    }
}
