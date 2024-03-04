using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace POV.Administracion.Reports
{
    public partial class ResultadosPruebaEstandarizadaEscuelaRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public ResultadosPruebaEstandarizadaEscuelaRpt()
        {
            InitializeComponent();
        }

        public ResultadosPruebaEstandarizadaEscuelaRpt(POV.Prueba.Reportes.BO.PruebaEstandarizadaDetail prueba)
        {
            InitializeComponent();
            if (prueba != null)
            {
                this.bindingSource1.DataSource = prueba;
            }
        }

    }
}
