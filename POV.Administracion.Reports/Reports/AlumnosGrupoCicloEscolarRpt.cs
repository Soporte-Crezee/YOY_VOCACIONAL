using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace POV.Administracion.Reports
{
    public partial class AlumnosGrupoCicloEscolarRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public AlumnosGrupoCicloEscolarRpt()
        {
            InitializeComponent();
        }
        public AlumnosGrupoCicloEscolarRpt(DataSet dataSet)
        {
            InitializeComponent();

            if (dataSet != null && dataSet.Tables.Contains("GrupoCicloEscolar") && dataSet.Tables.Contains("AlumnosAsignados"))
            {
                this.DataSource = dataSet;
                this.DataMember = "AlumnosAsignados";
                this.xrlblEscuela.Text = DateTime.Now.ToShortDateString();
                this.xrlblEscuela.DataBindings.Add("Text", this.DataSource, "GrupoCicloEscolar.Escuela");
                this.xrlblCicloEscolar.DataBindings.Add("Text", this.DataSource, "GrupoCicloEscolar.CicloEscolarTitulo");
                this.xrlblClavePrueba.DataBindings.Add("Text", this.DataSource, "GrupoCicloEscolar.GrupoCicloEscolarClave");
                this.xrlblGrado.DataBindings.Add("Text", this.DataSource, "GrupoCicloEscolar.GrupoGrado");
                this.xrlblGrupo.DataBindings.Add("Text", this.DataSource, "GrupoCicloEscolar.GrupoNombre");

                this.xrTableCell1.DataBindings.Add("Text", this.DataSource, "AlumnosAsignados.Curp");
                this.xrTableCell2.DataBindings.Add("Text", this.DataSource, "AlumnosAsignados.NombreCompleto");
                this.xrTableCell3.DataBindings.Add("Text", this.DataSource, "AlumnosAsignados.UsuarioID");
                this.xrTableCell4.DataBindings.Add("Text", this.DataSource, "AlumnosAsignados.NombreUsuario");
                this.xrTableCell5.DataBindings.Add("Text", this.DataSource, "AlumnosAsignados.NuevoPassword");
            }
        }

    }
}
