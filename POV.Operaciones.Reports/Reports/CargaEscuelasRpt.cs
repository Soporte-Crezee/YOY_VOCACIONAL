using System;
using System.Data;
using System.Drawing.Printing;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Text;
using DevExpress.XtraReports.UI;
using POV.Comun.BO;
using POV.Licencias.BO;

namespace POV.Operaciones.Reports
{
    public partial class CargaEscuelasRpt : DevExpress.XtraReports.UI.XtraReport
    {
        private DataTable datos;
        private Pais pais;
        private Estado estado;
        private DataTable inconsistencias;
        private Contrato contrato;
        public CargaEscuelasRpt()
        {
            InitializeComponent();
        }

        public CargaEscuelasRpt(Pais pais, Estado estado,Contrato contrato, DataTable carga, DataTable resultados)
        {
            InitializeComponent();

            this.SetData(pais, estado,contrato, carga, resultados);
        }

        public void SetData(Pais pais, Estado estado, Contrato contrato, DataTable carga, DataTable resultados)
        {
            this.pais = pais;
            this.estado = estado;
            this.datos = carga;
            this.inconsistencias = resultados;
            this.contrato = contrato;

            this.cPais.Text = pais.Nombre;
            this.cEstado.Text = estado.Nombre;
            this.cContratoCve.Text = contrato.Clave;
            this.cNombreCliente.Text = contrato.Cliente.Nombre;
            this.cRepresentante.Text = contrato.Cliente.Representante;
            this.cInicioContrato.Text = String.Format("{0:dd/MM/yyyy}", contrato.InicioContrato);
            this.cFinContrato.Text = String.Format("{0:dd/MM/yyyy}", contrato.FinContrato);
            //Escuelas cargadas
            DataSet registradas = this.FiltrarRegistradas();
            this.DCargadas.DataSource = registradas;


            //this.GHMunicipio.GroupFields[0].FieldName = "NombreMunicipio";
            //this.GHMunicipio.GroupFields[0].SortOrder = XRColumnSortOrder.Ascending;
            //this.GHMunicipio.GroupFields[1].FieldName = "NombreLocalidad";
            //this.GHMunicipio.GroupFields[1].SortOrder = XRColumnSortOrder.Ascending;

            //cMunicipioClave.DataBindings.Add("Text", registradas, "ClaveMunicipio");
            //cMunicipioNombre.DataBindings.Add("Text", registradas, "NombreMunicipio");
            //cLocalidadClave.DataBindings.Add("Text",registradas, "ClaveLocalidad");
            //cLocalidadNombre.DataBindings.Add("Text",registradas, "NombreLocalidad");

            Cargadas.FindControl("cMunicipioClave", true).DataBindings.Add("Text", registradas, "ClaveMunicipio");
            Cargadas.FindControl("cMunicipioNombre", true).DataBindings.Add("Text", registradas, "NombreMunicipio");
            Cargadas.FindControl("cLocalidadClave", true).DataBindings.Add("Text", registradas, "ClaveLocalidad");
            Cargadas.FindControl("cLocalidadNombre", true).DataBindings.Add("Text", registradas, "NombreLocalidad");

            Cargadas.FindControl("cEscuelaClave", true).DataBindings.Add("Text", registradas, "Clave");
            Cargadas.FindControl("cEscuelaNombre", true).DataBindings.Add("Text", registradas, "NombreEscuela");
            Cargadas.FindControl("cZonaClave", true).DataBindings.Add("Text", registradas, "ClaveZona");
            Cargadas.FindControl("cZonaNombre", true).DataBindings.Add("Text", registradas, "NombreZona");

            Cargadas.FindControl("cTurno", true).DataBindings.Add("Text", registradas, "Turno");
            Cargadas.FindControl("cTurnoNombre", true).DataBindings.Add("Text", registradas, "NombreTurno");
            Cargadas.FindControl("cAmbitoCve", true).DataBindings.Add("Text", registradas, "Ambito");
            Cargadas.FindControl("cAmbitoNombre", true).DataBindings.Add("Text", registradas, "NombreAmbito");

            Cargadas.FindControl("cCtrlClave", true).DataBindings.Add("Text", registradas, "Control");
            Cargadas.FindControl("CControl", true).DataBindings.Add("Text", registradas, "NombreControl");
            Cargadas.FindControl("cTipoSrvClv", true).DataBindings.Add("Text", registradas, "ClaveTipoServicio");
            Cargadas.FindControl("cTipoServicioNombre", true).DataBindings.Add("Text", registradas, "NombreTipoServicio");

            Cargadas.FindControl("cCurp", true).DataBindings.Add("Text", registradas, "DirectorCURP");
            Cargadas.FindControl("cNombre", true).DataBindings.Add("Text", registradas, "DirectorNombre");
            Cargadas.FindControl("cPrimerApellido", true).DataBindings.Add("Text", registradas, "DirectorPrimerApellido");
            Cargadas.FindControl("cSegundoApellido", true).DataBindings.Add("Text", registradas, "DirectorSegundoApellido");
            Cargadas.FindControl("cNivelEscolar", true).DataBindings.Add("Text", registradas, "DirectorNivelEscolar");
            Cargadas.FindControl("cFechaNacimiento", true).DataBindings.Add("Text", registradas, "DirectorFechaNacimiento");
            Cargadas.FindControl("cSexo", true).DataBindings.Add("Text", registradas, "DirectorSexo");
            Cargadas.FindControl("cTelefono", true).DataBindings.Add("Text", registradas, "DirectorTelefono");
            Cargadas.FindControl("cCorreo", true).DataBindings.Add("Text", registradas, "DirectorCorreo");

            //Escuelas no cargadas
            DataSet noRegistradas = this.FiltrarNoRegistradas();
            this.DNoCargadas.DataSource = noRegistradas;

         //   this.GHMunicipioNA.GroupFields[0].FieldName = "NombreMunicipio";
         //   this.GHMunicipioNA.GroupFields[0].SortOrder = XRColumnSortOrder.Ascending;
         //   this.GHMunicipioNA.GroupFields[0].FieldName = "NombreLocalidad";
         //   this.GHMunicipioNA.GroupFields[0].SortOrder = XRColumnSortOrder.Ascending;

            //cMunicipioClaveNA.DataBindings.Add("Text",noRegistradas, "ClaveMunicipio");
            //cMunicipioNombreNA.DataBindings.Add("Text",noRegistradas , "NombreMunicipio");
            //cLocalidadClaveNA.DataBindings.Add("Text",noRegistradas, "ClaveLocalidad");
            //cLocalidadNombreNA.DataBindings.Add("Text",noRegistradas, "NombreLocalidad");

            NoCargadas.FindControl("cMunicipioClaveNA", true).DataBindings.Add("Text", noRegistradas, "ClaveMunicipio");
            NoCargadas.FindControl("cMunicipioNombreNA", true).DataBindings.Add("Text", noRegistradas, "NombreMunicipio");
            NoCargadas.FindControl("cLocalidadClaveNA", true).DataBindings.Add("Text", noRegistradas, "ClaveLocalidad");
            NoCargadas.FindControl("cLocalidadNombreNA", true).DataBindings.Add("Text", noRegistradas, "NombreLocalidad");


            NoCargadas.FindControl("cEscuelaClaveNA", true).DataBindings.Add("Text", noRegistradas, "Clave");
            NoCargadas.FindControl("cEscuelaNombreNA", true).DataBindings.Add("Text", noRegistradas, "NombreEscuela");
            NoCargadas.FindControl("cZonaClaveNA", true).DataBindings.Add("Text", noRegistradas, "ClaveZona");

            NoCargadas.FindControl("cZonaNombreNA", true).DataBindings.Add("Text", noRegistradas, "NombreZona");
            NoCargadas.FindControl("cTurnoNA", true).DataBindings.Add("Text", noRegistradas, "Turno");
            NoCargadas.FindControl("cTurnoNombreNA", true).DataBindings.Add("Text", noRegistradas, "NombreTurno");
            NoCargadas.FindControl("cAmbitoCveNA", true).DataBindings.Add("Text", noRegistradas, "Ambito");
            NoCargadas.FindControl("cAmbitoNombreNA", true).DataBindings.Add("Text", noRegistradas, "NombreAmbito");

            NoCargadas.FindControl("cCtrlClaveNA", true).DataBindings.Add("Text", noRegistradas, "Control");
            NoCargadas.FindControl("CControlNA", true).DataBindings.Add("Text", noRegistradas, "NombreControl");
            NoCargadas.FindControl("cTipoSrvClvNA", true).DataBindings.Add("Text", noRegistradas, "ClaveTipoServicio");
            NoCargadas.FindControl("cTipoServicioNombreNA", true).DataBindings.Add("Text", noRegistradas, "NombreTipoServicio");

            NoCargadas.FindControl("cCurpNA", true).DataBindings.Add("Text", noRegistradas, "DirectorCURP");
            NoCargadas.FindControl("cNombreNA", true).DataBindings.Add("Text", noRegistradas, "DirectorNombre");
            NoCargadas.FindControl("cPrimerApellidoNA", true).DataBindings.Add("Text", noRegistradas, "DirectorPrimerApellido");
            NoCargadas.FindControl("cSegundoApellidoNA", true).DataBindings.Add("Text", noRegistradas, "DirectorSegundoApellido");
            NoCargadas.FindControl("cNivelEscolarNA", true).DataBindings.Add("Text", noRegistradas, "DirectorNivelEscolar");
            NoCargadas.FindControl("cFechaNacimientoNA", true).DataBindings.Add("Text", noRegistradas, "DirectorFechaNacimiento");
            NoCargadas.FindControl("cSexoNA", true).DataBindings.Add("Text", noRegistradas, "DirectorSexo");
            NoCargadas.FindControl("cTelefonoNA", true).DataBindings.Add("Text", noRegistradas, "DirectorTelefono");
            NoCargadas.FindControl("cCorreoNA", true).DataBindings.Add("Text", noRegistradas, "DirectorCorreo"); 
        }

        private DataSet FiltrarNoRegistradas()
        {
            DataRow[] query = (from parte in this.datos.AsEnumerable()
                                       where parte.Field<bool?>("Cargado") == false
                                       select parte).ToArray();

            DataSet dataSet = new DataSet();
            DataTable dataTable = !query.Any() ? this.datos.Clone() : query.CopyToDataTable();
            dataSet.Tables.Add(dataTable);

            return dataSet;
        }

        private DataSet FiltrarRegistradas()
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
            DataRowView rView = (DataRowView)this.DNoCargadas.GetCurrentRow();
            DataRow row = this.inconsistencias.AsEnumerable().FirstOrDefault(r => r.Field<int>("RowIndex") == rView.Row.Field<int>("RowIndex"));
            if (row != null)
                this.ImprimirObservaciones((sender as XRControl), row);
        }

        private void ImprimirObservaciones(XRControl cObervaciones, DataRow row)
        {
            StringBuilder observaciones = new StringBuilder();

            if (!row.IsNull("ExisteMunicipio") && !row.Field<bool>("ExisteMunicipio"))
                observaciones.Append(". No existe la clave de Municipio");
            if (!row.IsNull("ExisteZona") && !row.Field<bool>("ExisteZona"))
                observaciones.Append(". No existe la clave de Zona");
            if (!row.IsNull("ExisteLocalidad") && !row.Field<bool>("ExisteLocalidad"))
                observaciones.Append(". No existe la clave de Localidad");
            if (!row.IsNull("ExisteTipoServicio") && !row.Field<bool>("ExisteTipoServicio"))
                observaciones.Append(". No existe la clave de Tipo de Servicio");
            if (!row.IsNull("ValidoTurno") && !row.Field<bool>("ValidoTurno"))
                observaciones.Append(". Turno incorrecto");
            if (!row.IsNull("ValidoAmbito") && !row.Field<bool>("ValidoAmbito"))
                observaciones.Append(". Ámbito incorrecto"); ;
            if (!row.IsNull("ValidoControl") && !row.Field<bool>("ValidoControl"))
                observaciones.Append(". Control incorrecto"); ;
            if (!row.IsNull("Inconsistencia") && row.Field<string>("Inconsistencia").Trim().Length != 0)
                observaciones.Append(". " + row.Field<string>("Inconsistencia").Trim());

            cObervaciones.Text = observaciones.ToString().Substring(1);
        }
    }
}
