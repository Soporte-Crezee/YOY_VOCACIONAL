using Framework.Base.DataAccess;
using POV.CentroEducativo.BO;
using POV.Prueba.BO;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Prueba.Diagnostico.Dinamica.Service;
using POV.Seguridad.BO;
using System;
using System.Data;
using DevExpress.Office;

namespace POV.Expediente.Reports.Reports
{
    public partial class ResultadoPruebaCHASIDERpt : DevExpress.XtraReports.UI.XtraReport
    {
        public ResultadoPruebaCHASIDERpt(IDataContext dctx, Alumno alumno, int prueba, Usuario usuario, string fechaFin)
        {
            InitializeComponent();
            ResultadoPruebaDinamicaCtrl ctrl = new ResultadoPruebaDinamicaCtrl();
            ResultadoPruebaChaside result;
            DataSet ds;
            int cant = 0;

            #region Colores
            this.Header.ForeColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");

            this.Titulo.ForeColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");

            this.NamePrueba.ForeColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");

            this.TableResultPrueba.BackColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");
            this.TableResultPrueba.BorderColor = System.Drawing.ColorTranslator.FromHtml("#ff9e19");

            this.TableInfoEstudiante.BackColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");
            this.TableInfoEstudiante.BorderColor = System.Drawing.ColorTranslator.FromHtml("#ff9e19");

            this.TableInfoPrueba.BackColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");
            this.TableInfoPrueba.BorderColor = System.Drawing.ColorTranslator.FromHtml("#ff9e19");

            this.xrLine1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff9e19");
            this.xrLine2.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff9e19");

            #endregion

            this.cNombre.Text = alumno.Nombre + " " + " " + alumno.PrimerApellido + " " + alumno.SegundoApellido;
            this.cCorreo.Text = usuario.Email;
            this.cFinalPrueba.Text = fechaFin;

            #region Resultado para Intereses
            ds = ctrl.RetrieveResultadoPruebaCHASIDE(dctx, alumno, new PruebaDinamica { PruebaID = prueba }, 1);
            ResultadoCHASIDE resIntereses = new ResultadoCHASIDE();
            cant = 0;
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                cant++;
                result = ctrl.DataRowToResultadoPruebaChaside(row);
                switch (result.Nombre)
                {
                    case "C":
                        resIntereses = Percentil_C(1);
                        break;
                    case "H":
                        resIntereses = Percentil_H(1);
                        break;
                    case "A":
                        resIntereses = Percentil_A(1);
                        break;
                    case "S":
                        resIntereses = Percentil_S(1);
                        break;
                    case "I":
                        resIntereses = Percentil_I(1);
                        break;
                    case "D":
                        resIntereses = Percentil_D(1);
                        break;
                    case "E":
                        resIntereses = Percentil_E(1);
                        break;
                }
                if (cant < 2)
                {
                    xrTableCell1.Text = resIntereses.Percentil;
                    xrTableCell2.Text = resIntereses.Descripcion;
                    xrTableCell3.Text = resIntereses.AptitudInteres;
                }
                else
                {
                    xrTableCell4.Text = resIntereses.Percentil;
                    xrTableCell5.Text = resIntereses.Descripcion;
                    xrTableCell6.Text = resIntereses.AptitudInteres;
                }
            }
            #endregion

            #region Resultado para Aptitudes
            ds = ctrl.RetrieveResultadoPruebaCHASIDE(dctx, alumno, new PruebaDinamica { PruebaID = prueba }, 2);
            
            ResultadoCHASIDE resAptitudes = new ResultadoCHASIDE();
            cant = 0;
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                cant++;
                result = ctrl.DataRowToResultadoPruebaChaside(row);
                switch (result.Nombre)
                {
                    case "C":
                        resAptitudes = Percentil_C(2);
                        break;
                    case "H":
                        resAptitudes = Percentil_H(2);
                        break;
                    case "A":
                        resAptitudes = Percentil_A(2);
                        break;
                    case "S":
                        resAptitudes = Percentil_S(2);
                        break;
                    case "I":
                        resAptitudes = Percentil_I(2);
                        break;
                    case "D":
                        resAptitudes = Percentil_D(2);
                        break;
                    case "E":
                        resAptitudes = Percentil_E(2);
                        break;
                }
                if (cant < 2)
                {
                    xrTableCell7.Text = resAptitudes.Percentil;
                    xrTableCell8.Text = resAptitudes.Descripcion;
                    xrTableCell9.Text = resAptitudes.AptitudInteres;
                }
                else
                {
                    xrTableCell10.Text = resAptitudes.Percentil;
                    xrTableCell11.Text = resAptitudes.Descripcion;
                    xrTableCell12.Text = resAptitudes.AptitudInteres;
                }
            }
            #endregion

            this.lblAnio.Text = "©" + DateTime.Now.ToString("yyyy") + ", Todos los derechos reservados YOY Vocacional";
        }

        public ResultadoCHASIDE Percentil_C(int tipo)
        {
            ResultadoCHASIDE obj = new ResultadoCHASIDE();
            obj.Percentil = "C";
                    obj.Descripcion = "Carreras de las áreas administrivas y contables";
            switch (tipo)
            {
                case 1:
                    obj.AptitudInteres = "Las carreras profesionales en esta área de tu interés se caracterizan por: \n" +
                        "  - Organizativo\n" +
                        "  - Supervisión\n" +
                        "  - Orden\n" +
                        "  - Análisis y sintesis\n" +
                        "  - Colaboración\n" +
                        "  - Cálculo\n";
                    break;
                case 2:
                    obj.AptitudInteres = "Para las carreras de esta área, tu tienes las siguientes aptitudes: \n" +
                        "  - Persuasivo\n" +
                        "  - Objetivo\n" +
                        "  - Práctico\n" +
                        "  - Tolerante\n" +
                        "  - Responsable\n" +
                        "  - Ambicioso\n";
                    break;
            }
            return obj;
        }
        public ResultadoCHASIDE Percentil_H(int tipo)
        {
            ResultadoCHASIDE obj = new ResultadoCHASIDE();
            obj.Percentil = "H";
            obj.Descripcion = "Carreras de las áreas humanisticas y sociales";
            switch (tipo)
            {
                case 1:
                    obj.AptitudInteres = "Las carreras profesionales en esta área de tu interés se caracterizan por: \n" +
                        "  - Precisión verbal\n" +
                        "  - Organización\n" +
                        "  - Relación de hechos\n" +
                        "  - Lingüistica\n" +
                        "  - Orden\n" +
                        "  - Justicia\n";
                    break;
                case 2:
                    obj.AptitudInteres = "Para las carreras de esta área, tu tienes las siguientes aptitudes: \n" +
                        "  - Responsable\n" +
                        "  - Justo\n" +
                        "  - Conciliador\n" +
                        "  - Persuasivo\n" +
                        "  - Sagaz\n" +
                        "  - Imaginativo\n";
                    break;
            }
            return obj;
        }
        public ResultadoCHASIDE Percentil_A(int tipo)
        {
            ResultadoCHASIDE obj = new ResultadoCHASIDE();
            obj.Percentil = "A";
            obj.Descripcion = "Carreras de las áreas Artisticas";
            switch (tipo)
            {
                case 1:
                    obj.AptitudInteres = "Las carreras profesionales en esta área de tu interés se caracterizan por: \n" +
                        "  - Estético\n" +
                        "  - Armónico\n" +
                        "  - Manual\n" +
                        "  - Visual\n" +
                        "  - Auditivo\n";
                    break;
                case 2:
                    obj.AptitudInteres = "Para las carreras de esta área, tu tienes las siguientes aptitudes: \n" +
                        "  - Sensible\n" +
                        "  - Imaginativo\n" +
                        "  - Creativo\n" +
                        "  - Detallista\n" +
                        "  - Innovador\n" +
                        "  - Intuitivo\n";
                    break;
            }
            return obj;
        }
        public ResultadoCHASIDE Percentil_S(int tipo)
        {
            ResultadoCHASIDE obj = new ResultadoCHASIDE();
            obj.Percentil = "S";
            obj.Descripcion = "Carreras de las áreas de Medicina y Ciencias de la salud";
            switch (tipo)
            {
                case 1:
                    obj.AptitudInteres = "Las carreras profesionales en esta área de tu interés se caracterizan por: \n" +
                        "  - Asistir\n" +
                        "  - Investigativo\n" +
                        "  - Precisión\n" +
                        "  - Senso-Perceptivo\n" +
                        "  - Analítio\n" +
                        "  - Ayudar\n";
                    break;
                case 2:
                    obj.AptitudInteres = "Para las carreras de esta área, tu tienes las siguientes aptitudes: \n" +
                        "  - Altruista\n" +
                        "  - Solidario\n" +
                        "  - Paciente\n" +
                        "  - Comprensivo\n" +
                        "  - Respetuoso\n" +
                        "  - Persuasivo\n";
                    break;
            }
            return obj;
        }
        public ResultadoCHASIDE Percentil_I(int tipo)
        {
            ResultadoCHASIDE obj = new ResultadoCHASIDE();
            obj.Percentil = "I";
            obj.Descripcion = "Carreras de las áreas de Ingeniería y computación";
            switch (tipo)
            {
                case 1:
                    obj.AptitudInteres = "Las carreras profesionales en esta área de tu interés se caracterizan por: \n" +
                        "  - Cálculo\n" +
                        "  - Científico\n" +
                        "  - Manual\n" +
                        "  - Exacto\n" +
                        "  - Planificar\n";
                    break;
                case 2:
                    obj.AptitudInteres = "Para las carreras de esta área, tu tienes las siguientes aptitudes: \n" +
                        "  - Preciso\n" +
                        "  - Práctivo\n" +
                        "  - Crítico\n" +
                        "  - Analítico\n" +
                        "  - Rígido\n";
                    break;
            }
            return obj;
        }
        public ResultadoCHASIDE Percentil_D(int tipo)
        {
            ResultadoCHASIDE obj = new ResultadoCHASIDE();
            obj.Percentil = "D";
            obj.Descripcion = "Carreras de las áreas de Defensa y Seguridad";
            switch (tipo)
            {
                case 1:
                    obj.AptitudInteres = "Las carreras profesionales en esta área de tu interés se caracterizan por: \n" +
                        "  - Justicia\n" +
                        "  - Equidad\n" +
                        "  - Colaboración\n" +
                        "  - Espíritu de equipo\n" +
                        "  - Liderazgo\n";
                    break;
                case 2:
                    obj.AptitudInteres = "Para las carreras de esta área, tu tienes las siguientes aptitudes: \n" +
                        "  - Arriesgado\n" +
                        "  - Solidario\n" +
                        "  - Valiente\n" +
                        "  - Agresivo\n" +
                        "  - Persuasivo\n";
                    break;
            }
            return obj;
        }
        public ResultadoCHASIDE Percentil_E(int tipo)
        {
            ResultadoCHASIDE obj = new ResultadoCHASIDE();
            obj.Percentil = "E";
            obj.Descripcion = "Carreras de las áreas de Ciencias exactas y Agrarias";
            switch (tipo)
            {
                case 1:
                    obj.AptitudInteres = "Las carreras profesionales en esta área de tu interés se caracterizan por: \n" +
                        "  - Investigación\n" +
                        "  - Orden\n" +
                        "  - Organización\n" +
                        "  - Analísis y sintesis\n" +
                        "  - Numérico\n" +
                        "  - Clasificar\n";
                    break;
                case 2:
                    obj.AptitudInteres = "Para las carreras de esta área, tu tienes las siguientes aptitudes: \n" +
                        "  - Metódico\n" +
                        "  - Analítico\n" +
                        "  - Observador\n" +
                        "  - Introvertido\n" +
                        "  - PAciente\n" +
                        "  - Seguro\n";
                    break;
            }
            return obj;
        }

        public class ResultadoCHASIDE
        {
            public string Percentil { get; set; }
            public String Descripcion { get; set; }
            public String AptitudInteres { get; set; }
        }

    }
}
