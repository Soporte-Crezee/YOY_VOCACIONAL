using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace POV.Administracion.Reports
{
    public partial class ResultadosPruebaDinamicaEscuelaRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public ResultadosPruebaDinamicaEscuelaRpt()
        {
            InitializeComponent();
        }
        public ResultadosPruebaDinamicaEscuelaRpt(POV.Prueba.Reportes.BO.PruebaDinamicaDetail prueba)
        {
            InitializeComponent();
            if (prueba != null)
            {
                this.bindingSource1.DataSource = prueba;
            }
        }
    }
}
