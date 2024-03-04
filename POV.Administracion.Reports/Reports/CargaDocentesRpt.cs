using System;
using System.Data;
using System.Drawing.Printing;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Text;
using DevExpress.XtraReports.UI;
using POV.CentroEducativo.BO;

namespace POV.Operaciones.Reports
{
    public partial class CargaDocentesRpt : DevExpress.XtraReports.UI.XtraReport
    {
        private DataTable datos;
        private Escuela escuela;
        private CicloEscolar cicloEscolar;
        private DataTable inconsistencias;

        public CargaDocentesRpt()
        {
            InitializeComponent();
        }

        public CargaDocentesRpt(Escuela escuela, CicloEscolar cicloEscolar, DataTable carga, DataTable resultados)
        {
            InitializeComponent();

            this.SetData(escuela, cicloEscolar, carga, resultados);
        }

        public void SetData(Escuela escuela, CicloEscolar cicloEscolar, DataTable carga, DataTable resultados)
        {
            this.escuela = escuela;
            this.cicloEscolar = cicloEscolar;
            this.datos = carga;
            this.inconsistencias = resultados;

            this.cEscuela.Text = this.escuela.NombreEscuela;
            this.cCiclo.Text = this.cicloEscolar.InicioCiclo.Value.ToString("MMM yy") + " - " + this.cicloEscolar.FinCiclo.Value.ToString("MMM yy");

            //Docentes cargados
            DataSet registradas = this.FiltrarRegistrados();
            this.DCargados.DataSource = registradas;

            Cargados.FindControl("cCURP", true).DataBindings.Add("Text", registradas, "Curp");
            Cargados.FindControl("cNombre", true).DataBindings.Add("Text", registradas, "Nombre");
            Cargados.FindControl("cPrimerApellido", true).DataBindings.Add("Text", registradas, "PrimerApellido");
            Cargados.FindControl("cSegundoApellido", true).DataBindings.Add("Text", registradas, "SegundoApellido");

            Cargados.FindControl("cCorreo", true).DataBindings.Add("Text", registradas, "DocenteCorreo");
            Cargados.FindControl("cFechaNacimiento", true).DataBindings.Add("Text", registradas, "FechaNacimiento");
            Cargados.FindControl("cSexo", true).DataBindings.Add("Text", registradas, "Sexo");

            //Docentes no cargados
            DataSet noRegistradas = this.FiltrarNoRegistrados();
            this.DNoCargados.DataSource = noRegistradas;

            NoCargados.FindControl("cCURPNA", true).DataBindings.Add("Text", noRegistradas, "Curp");
            NoCargados.FindControl("cNombreNA", true).DataBindings.Add("Text", noRegistradas, "Nombre");
            NoCargados.FindControl("cPrimerApellidoNA", true).DataBindings.Add("Text", noRegistradas, "PrimerApellido");
            NoCargados.FindControl("cSegundoApellidoNA", true).DataBindings.Add("Text", noRegistradas, "SegundoApellido");

            NoCargados.FindControl("cCorreoNA", true).DataBindings.Add("Text", noRegistradas, "DocenteCorreo");
            NoCargados.FindControl("cFechaNacimientoNA", true).DataBindings.Add("Text", noRegistradas, "FechaNacimiento");
            NoCargados.FindControl("cSexoNA", true).DataBindings.Add("Text", noRegistradas, "Sexo");
        }

        private DataSet FiltrarNoRegistrados()
        {
            DataRow[] query = (from parte in this.datos.AsEnumerable()
                                       where parte.Field<bool?>("Cargado") == false
                                       select parte).ToArray();

            DataSet dataSet = new DataSet();
            DataTable dataTable = !query.Any() ? this.datos.Clone() : query.CopyToDataTable();
            dataSet.Tables.Add(dataTable);

            return dataSet;
        }

        private DataSet FiltrarRegistrados()
        {

            DataRow[] query = (from parte in this.datos.AsEnumerable()
                                       where parte.Field<bool?>("Cargado") == true
                                       select parte).ToArray();

            DataSet dataSet = new DataSet();
            DataTable dataTable = !query.Any() ? this.datos.Clone() : query.CopyToDataTable();
            dataSet.Tables.Add(dataTable);

            return dataSet;
        }

        private void cObervaciones_BeforePrint(object sender, PrintEventArgs e)
        {
            DataRowView rView = (DataRowView)this.DNoCargados.GetCurrentRow();
            DataRow row = this.inconsistencias.AsEnumerable().FirstOrDefault(r => r.Field<int>("RowIndex") == rView.Row.Field<int>("RowIndex"));
            if (row != null)
                this.ImprimirObservaciones((sender as XRControl), row);
        }

        private void ImprimirObservaciones(XRControl observacion, DataRow row)
        {
            StringBuilder observaciones = new StringBuilder();

            if (!row.IsNull("Inconsistencia") && row.Field<string>("Inconsistencia").Trim().Length != 0)
                observaciones.Append(". " + row.Field<string>("Inconsistencia").Trim());

            observacion.Text = observaciones.ToString().Substring(1);
        }
    }
}
