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
    public partial class ExpedienteAlumnoRpt : DevExpress.XtraReports.UI.XtraReport
    {
        private Alumno _alumno;
        private Usuario _usuario;
        private Decimal _resHabitos;
        private Decimal _resDominos;
        private DataSet _resTerman;
        private Dictionary<string, string> _resKuder;
        private Dictionary<string, string> _resAllport;
        private Series series1 = new Series("Puntaje", ViewType.Line);

        public ExpedienteAlumnoRpt()
        {
            InitializeComponent();
        }

        public ExpedienteAlumnoRpt(Alumno alumno, Usuario usuario, Decimal resHabitos, Decimal resDominos, DataSet resTerman, Dictionary<string, string> resKuder, Dictionary<string, string> resAllport)
        {
            InitializeComponent();
            this.SetData(alumno, usuario, resHabitos, resDominos, resTerman, resKuder, resAllport);

        }

        public void SetData(Alumno alumno, Usuario usuario, Decimal resHabitos, Decimal resDominos, DataSet resTerman, Dictionary<string, string> resKuder, Dictionary<string, string> resAllport)
        {
            this._alumno = alumno;
            this._usuario = usuario;
            this._resHabitos = resHabitos;
            this._resDominos = resDominos;
            this._resTerman = resTerman;
            this._resKuder = resKuder;
            this._resAllport = resAllport;


            this.cNombre.Text = _alumno.Nombre + " " + " " + _alumno.PrimerApellido + " " + _alumno.SegundoApellido;
            this.cCorreo.Text = _usuario.Email;
            this.cTelefonoCasa.Text = _usuario.TelefonoCasa;
            this.cTelefonoReferencia.Text = _usuario.TelefonoCasa;

            #region encabezados
            var sbuilder = new StringBuilder();
            sbuilder.Append(AppDomain.CurrentDomain.BaseDirectory);
            sbuilder.Append(@"Files\protocolo\configHabitos.txt");
            string[] lines = System.IO.File.ReadAllLines(Path.GetFullPath(sbuilder.ToString()).ToString());

            string HeaderText = lines[1];
            string TituloText = lines[3];
            this.Header.Text = HeaderText;
            this.Titulo.Text = TituloText;

            string sinPruebas = lines[13];
            this.cNoInfo.Text = sinPruebas;
            #endregion

            #region Habitos
            this.cNombrePrueba.Text = "Cuestionario de Hábitos de estudio";
            string informacionHabitos = lines[5];
            string comentariosHabitos = lines[7];
            this.infoHabitos.Text = informacionHabitos;
            this.comHabitos.Text = comentariosHabitos;

            if (_resHabitos == 0)
            {
                CCargaHabitos.Visible = false;
            }
            else if (_resHabitos >= 1)
            {
                ResaltarResultadoHabitos(_resHabitos);
                CCargaNoInfo.Visible = false;
            }
            #endregion

            #region Dominos
            this.cDominos.Text = "Dominós";
            string informacionDominos = lines[9];
            string comentariosDominos = lines[11];
            this.infoDominos.Text = informacionDominos;
            this.comDominos.Text = comentariosDominos;

            if (_resDominos == 0)
            {
                CCargaDominos.Visible = false;
            }
            else if (_resDominos >= 1)
            {
                ResaltarResultadoDominos(_resDominos);
                CCargaNoInfo.Visible = false;
            }
            #endregion

            #region Terman
            this.cTerman.Text = "Terman Merrill";
            string informacionTerman = lines[15];
            string comentariosTerman = lines[17];
            this.infoTerman.Text = informacionTerman;
            this.comTerman.Text = comentariosTerman;

            if (_resTerman != null)
            {
                ResaltarResultadoTerman(_resTerman);
                CCargaNoInfo.Visible = false;

            }
            else
            {
                CCargaTerman.Visible = false;
            }
            #endregion

            #region Kuder
            this.cKuder.Text = "Kuder";
            string informacionKuder = lines[19];
            string comentariosKuder = lines[21];
            this.infoKuder.Text = informacionKuder;
            this.comKuder.Text = comentariosKuder;
            if (_resKuder != null)
            {
                ResaltarResultadoKuder(_resKuder);
                CCargaNoInfo.Visible = false;
            }
            else
            {
                CCargaKuder.Visible = false;
            }
            #endregion

            #region Allport
            this.cAllport.Text = "Allport";
            string informacionAllport = lines[23];
            string comentariosAllport = lines[25];
            this.infoAllport.Text = informacionAllport;
            this.comAllport.Text = comentariosAllport;
            if (_resAllport != null)
            {
                ResaltarResultadoAllport(resAllport);
                CCargaNoInfo.Visible = false;
            }
            else
            {
                CCargaAllport.Visible = false;
            }
            #endregion
            this.lblAnio.Text = "©" + DateTime.Now.ToString("yyyy") + ", Todos los derechos reservados, Compañia";
        }

        private void ResaltarResultadoHabitos(decimal puntaje)
        {
            if (puntaje >= 1 && puntaje <= 50)
            {
                punt1.Font = new Font("Arial", 12f, FontStyle.Bold);
                punt1.BackColor = Color.CadetBlue;
                punt1.ForeColor = Color.White;

                niv1.Font = new Font("Arial", 12f, FontStyle.Bold);
                niv1.BackColor = Color.CadetBlue;
                niv1.ForeColor = Color.White;
                reco1.Font = new Font("Arial", 12f, FontStyle.Bold);
                reco1.BackColor = Color.CadetBlue;
                reco1.ForeColor = Color.White;
            }
            else if (puntaje >= 51 && puntaje <= 75)
            {
                punt2.Font = new Font("Arial", 12f, FontStyle.Bold);
                punt2.BackColor = Color.CadetBlue;
                punt2.ForeColor = Color.White;
                niv2.Font = new Font("Arial", 12f, FontStyle.Bold);
                niv2.BackColor = Color.CadetBlue;
                niv2.ForeColor = Color.White;
                reco1b.Font = new Font("Arial", 12f, FontStyle.Bold);
                reco1b.BackColor = Color.CadetBlue;
                reco1b.ForeColor = Color.White;
            }
            else if (puntaje >= 76 && puntaje <= 95)
            {
                punt3.Font = new Font("Arial", 12f, FontStyle.Bold);
                punt3.BackColor = Color.CadetBlue;
                punt3.ForeColor = Color.White;
                niv3.Font = new Font("Arial", 12f, FontStyle.Bold);
                niv3.BackColor = Color.CadetBlue;
                niv3.ForeColor = Color.White;
                reco2.Font = new Font("Arial", 12f, FontStyle.Bold);
                reco2.BackColor = Color.CadetBlue;
                reco2.ForeColor = Color.White;
            }
            else if (puntaje >= 96 && puntaje <= 115)
            {
                punt4.Font = new Font("Arial", 12f, FontStyle.Bold);
                punt4.BackColor = Color.CadetBlue;
                punt4.ForeColor = Color.White;
                niv4.Font = new Font("Arial", 12f, FontStyle.Bold);
                niv4.BackColor = Color.CadetBlue;
                niv4.ForeColor = Color.White;
                reco3.Font = new Font("Arial", 12f, FontStyle.Bold);
                reco3.BackColor = Color.CadetBlue;
                reco3.ForeColor = Color.White;
            }
            else if (puntaje >= 114 && puntaje <= 150)
            {
                punt5.Font = new Font("Arial", 12f, FontStyle.Bold);
                punt5.BackColor = Color.CadetBlue;
                punt5.ForeColor = Color.White;
                niv5.Font = new Font("Arial", 12f, FontStyle.Bold);
                niv5.BackColor = Color.CadetBlue;
                niv5.ForeColor = Color.White;
                reco3b.Font = new Font("Arial", 12f, FontStyle.Bold);
                reco3b.BackColor = Color.CadetBlue;
                reco3b.ForeColor = Color.White;
            }
        }

        private void ResaltarResultadoDominos(decimal puntaje)
        {
            if (puntaje > 90 && puntaje <= 100)
            {
                cPercentil1.Font = new Font("Arial", 12f, FontStyle.Bold);
                cPercentil1.BackColor = Color.CadetBlue;
                cPercentil1.ForeColor = Color.White;

                cRango1.Font = new Font("Arial", 12f, FontStyle.Bold);
                cRango1.BackColor = Color.CadetBlue;
                cRango1.ForeColor = Color.White;
            }
            else if (puntaje >= 75 && puntaje <= 90)
            {
                cPercentil2.Font = new Font("Arial", 12f, FontStyle.Bold);
                cPercentil2.BackColor = Color.CadetBlue;
                cPercentil2.ForeColor = Color.White;

                cRango2.Font = new Font("Arial", 12f, FontStyle.Bold);
                cRango2.BackColor = Color.CadetBlue;
                cRango2.ForeColor = Color.White;
            }
            else if (puntaje > 25 && puntaje < 75)
            {
                cPercentil3.Font = new Font("Arial", 12f, FontStyle.Bold);
                cPercentil3.BackColor = Color.CadetBlue;
                cPercentil3.ForeColor = Color.White;

                cRango3.Font = new Font("Arial", 12f, FontStyle.Bold);
                cRango3.BackColor = Color.CadetBlue;
                cRango3.ForeColor = Color.White;
            }
            else if (puntaje >= 10 && puntaje <= 25)
            {
                cPercentil4.Font = new Font("Arial", 12f, FontStyle.Bold);
                cPercentil4.BackColor = Color.CadetBlue;
                cPercentil4.ForeColor = Color.White;

                cRango4.Font = new Font("Arial", 12f, FontStyle.Bold);
                cRango4.BackColor = Color.CadetBlue;
                cRango4.ForeColor = Color.White;
            }
            else if (puntaje > 0 && puntaje < 10)
            {
                cPercentil5.Font = new Font("Arial", 12f, FontStyle.Bold);
                cPercentil5.BackColor = Color.CadetBlue;
                cPercentil5.ForeColor = Color.White;

                cRango5.Font = new Font("Arial", 12f, FontStyle.Bold);
                cRango5.BackColor = Color.CadetBlue;
                cRango5.ForeColor = Color.White;
            }
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
            if (serieUno == 16)
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
            if (serieDos == 22)
                cTRango2.Text = "Sobresaliente";
            if (serieDos == 20)
                cTRango2.Text = "Superior";
            if (serieDos == 18)
                cTRango2.Text = "Térm. M. A";
            if (serieDos >= 12 && serieDos <= 16)
                cTRango2.Text = "Térm. Medio";
            if (serieDos == 10)
                cTRango2.Text = "Térm. M. B.";
            if (serieDos == 8)
                cTRango2.Text = "Inferior";
            if (serieDos <= 6)
                cTRango2.Text = "Deficiente";
            #endregion

            #region Serie III
            if (serieTres == 30 || serieTres == 29)
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
            if (serieCuatro == 18)
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
            if (serieCinco == 24)
                cTRango5.Text = "Sobresaliente";
            if (serieCinco >= 20 && serieCinco <= 22)
                cTRango5.Text = "Superior";
            if (serieCinco >= 16 && serieCinco <= 18)
                cTRango5.Text = "Térm. M. A";
            if (serieCinco >= 12 && serieCinco <= 14)
                cTRango5.Text = "Térm. Medio";
            if (serieCinco >= 8 && serieCinco <= 10)
                cTRango5.Text = "Térm. M. B.";
            if (serieCinco == 6)
                cTRango5.Text = "Inferior";
            if (serieCinco <= 4)
                cTRango5.Text = "Deficiente";
            #endregion

            #region Serie VI
            if (serieSeis == 20)
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
            if (serieSiete == 19 || serieSiete == 20)
                cTRango7.Text = "Sobresaliente";
            if (serieSiete == 18)
                cTRango7.Text = "Superior";
            if (serieSiete == 16 || serieSiete == 17)
                cTRango7.Text = "Térm. M. A";
            if (serieSiete >= 9 && serieSeis <= 15)
                cTRango7.Text = "Térm. Medio";
            if (serieSiete >= 6 && serieSiete <= 8)
                cTRango7.Text = "Térm. M. B.";
            if (serieSiete == 5)
                cTRango7.Text = "Inferior";
            if (serieSiete <= 4)
                cTRango7.Text = "Deficiente";
            #endregion

            #region Serie VIII
            if (serieOcho == 17)
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
            if (serieNueve == 18)
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
            if (serieDiez >= 20 && serieDiez <= 22)
                cTRango10.Text = "Sobresaliente";
            if (serieDiez == 18)
                cTRango10.Text = "Superior";
            if (serieDiez == 16)
                cTRango10.Text = "Térm. M. A";
            if (serieDiez >= 10 && serieDiez <= 14)
                cTRango10.Text = "Térm. Medio";
            if (serieDiez == 8)
                cTRango10.Text = "Térm. M. B.";
            if (serieDiez == 6)
                cTRango10.Text = "Inferior";
            if (serieDiez <= 4)
                cTRango10.Text = "Deficiente";
            #endregion
            #endregion

            #region Grafica
            List<CalificacionesTerman> listCalificacion = new List<CalificacionesTerman>();

            listCalificacion.AddRange(new CalificacionesTerman[] 
            {
                new CalificacionesTerman("Informacion",serieUno),
                new CalificacionesTerman("Juicio",serieDos),
                new CalificacionesTerman("Vocabulario",serieTres),
                new CalificacionesTerman("Síntesis",serieCuatro),
                new CalificacionesTerman("Concentración",serieCinco),
                new CalificacionesTerman("Análisis",serieSeis),
                new CalificacionesTerman("Abstracción",serieSiete),
                new CalificacionesTerman("Planeación",serieOcho),
                new CalificacionesTerman("Organizacón",serieNueve),
                new CalificacionesTerman("Atención",serieDiez)
            });

            series1.ArgumentScaleType = ScaleType.Qualitative;
            series1.ValueScaleType = ScaleType.Numerical;

            series1.ArgumentDataMember = "Nombre";
            series1.ValueDataMembers[0] = "Puntaje";

            cGrafTerman.Series.Add(series1);
            cGrafTerman.DataSource = listCalificacion;
            #endregion

            double Total = serieUno + serieDos + serieTres + serieCuatro + serieCinco + serieSeis + serieSiete + serieOcho + serieNueve + serieDiez;
            cPuntTotTerman.Text = Total + "";

            #region Tabla de CI y Rango
            int CI = 0;
            #region CI
            if (Total <= 67)
                cCI.Text = Total + "";
                cCI.Text = "80";

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

            if (Total >= 88 && Total <= 90)
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

            if (Total == 114 || Total == 115)
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
            if (CI >= 89 && CI <= 89)
                cRangoTerman.Text = "Térm. M. B";
            if (CI >= 70 && CI <= 79)
                cRangoTerman.Text = "Inferior";
            if (CI <= 69)
                cRangoTerman.Text = "Deficiente";
            #endregion
            #endregion
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
                            plantilla = @"“0” Interés para el trabajo al aire libre
                                                Ingenieros agrónomos
                                                Ingenieros forestales
                                                Ingenieros de minas
                                                Geólogos
                                                Oficiales de ejército, marina, aviación, policía.
                                                Profesores de educación física.";
                            break;
                        case "1 Mecanico":
                            descripcion = @"Un alto puntaje aquí indica interés para trabajar con máquinas y herramientas, construir o arreglar objetos mecánicos, 
                                            artefactos eléctricos, muebles, etc.";
                            plantilla = @"“1” Interés mecánico
                                                Ingenieros civiles
                                                Ingenieros electricistas
                                                Ingenieros industriales
                                                Ingenieros mecánicos
                                                Ingenieros metalúrgicos
                                                Ingenieros químicos
                                                Aviadores
                                                Técnicos en radio y televisión";
                            break;
                        case "2 Calculo":
                            descripcion = @"Lo poseen aquellas personas a quienes les gusta trabajar con números. Muchos ingenieros revelan también un marcado interés 
                                            por las actividades relacionadas con el cálculo.";
                            plantilla = @"“2” Interés para el cálculo
                                                Economistas
                                                Contadores públicos y auditores
                                                Estadígrafos
                                                Profesores de matemáticas";
                            break;
                        case "3 Cientifico":
                            descripcion = @"Manifiestan este interés las personas que encuentran placer en investigar la razón de los hechos o de las cosas, en 
                                            descubrir sus causas y en resolver problemas de distinta índole, por mera curiosidad científica y sin pensar en los beneficios 
                                            económicos que puedan resultar de sus descubrimientos. El interés científico es de gran importancia en el ejercicio de muchas 
                                            carreras profesionales, aun de aquéllas donde el móvil de la actividad puede ser de índole distinta al progreso de la ciencia.";
                            plantilla = @"“3” Interés científico
                                                Antropólogos Arqueólogos Astrónomos Biólogos
                                                Ingenieros electricistas Ingenieros químicos Médicos y cirujanos Médicos veterinarios Químicos
                                                Químicos farmaceutas Odontólogos Técnicos de laboratorio.";
                            break;
                        case "4 Persuasivo":
                            descripcion = @"Lo poseen aquellas personas a quienes les gusta tratar con la gente, imponer sus puntos de vista, convencer a los demás 
                                            respecto a algún proyecto, venderles un artículo, etc.";
                            plantilla = @"“4” Interés persuasivo
                                                Escritores
                                                Juristas (Abocados, Jueces, Consejeros jurídicos)
                                                Agentes de publicidad
                                                Jefes de ventas
                                                Locutores de radio y TV
                                                Periodistas";
                            break;
                        case "5 Artistico":
                            descripcion = @"Lo poseen las personas a quienes les agrada hacer trabajos de creación de tipo manual, usando combinaciones de colores, 
                                            materiales, formas y diseños.";
                            plantilla = @"“5” Interes Artístico-plástico
                                                Arquitectos
                                                Decoradores de interiores
                                                Dibujantes
                                                Escultores
                                                Pintores
                                                Fotógrafos";
                            break;
                        case "6 Literario":
                            descripcion = @"Es propio de todos aquellos a quienes les gusta la lectura o encuentran placer en expresar sus ideas en forma oral o escrita.";
                            plantilla = @"“6” Interés literario
                                                Escritores
                                                Juristas
                                                Periodistas
                                                Profesores de letras
                                                Bibliotecarios";
                            break;
                        case "7 Musical":
                            descripcion = @"Se sitúan aquí las personas que muestran un marcado gusto para tocar instrumentos musicales, cantar, bailar, leer sobre 
                                            música, estudiar la vida de compositores famosos, asistir a conciertos, etc.";
                            plantilla = @"“7” Interés musical
                                                Compositores
                                                Músicos
                                                Profesores de música
                                                Artistas de Ballet";
                            break;
                        case "8 Social":
                            descripcion = @"Un alto puntaje en esta área indica un gran interés por servir a los demás: a los necesitados, enfermos, niños y ancianos.";
                            plantilla = @"“8” Interés para el servicio social
                                                Sacerdotes
                                                Pedagogos (en general}
                                                Médicos y cirujanos
                                                Trabajadores sociales
                                                Enfermeros(as)
                                                Consejeros Vocacionales";
                            break;
                        case "9 Oficina":
                            descripcion = @"Es propio de las personas a quienes les gusta un tipo de trabajo de escritorio, que requiere exactitud y precisión.";
                            plantilla = @"“9” Interés en el trabajo de oficina
                                                Archivistas
                                                Contadores
                                                Mecanógrafas
                                                Secretários (as)
                                                Coleccionistas de libros
                                                Taquígrafos (as)";
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

        private void ResaltarResultadoAllport(Dictionary<string, string> plantillas)
        {
            var sbuilder = new StringBuilder();
            sbuilder.Append(AppDomain.CurrentDomain.BaseDirectory);
            sbuilder.Append(@"Files\protocolo\ValoresAllport.txt");
            string[] lines = System.IO.File.ReadAllLines(Path.GetFullPath(sbuilder.ToString()).ToString());

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
                            valor = "Teórico: " + lines[1];
                            meta = lines[3];
                            break;
                        case "2 Economico":
                            valor = "Económico: " + lines[5];
                            meta = lines[7];
                            break;
                        case "3 Estetico":
                            valor = "Estético: " + lines[9];
                            meta = lines[11];
                            break;
                        case "4 Social":
                            valor = "Social: " + lines[13];
                            meta = lines[15];
                            break;
                        case "5 Politico":
                            valor = "Político: " + lines[17];
                            meta = lines[19];
                            break;
                        case "6 Religioso":
                            valor = "Religioso: " + lines[21];
                            meta = lines[23];
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

    public class CalificacionesTerman
    {
        public string Nombre { get; set; }
        public double Puntaje { get; set; }

        public CalificacionesTerman(string nombre, double puntaje)
        {
            this.Nombre = nombre;
            this.Puntaje = puntaje;
        }
    }
}
