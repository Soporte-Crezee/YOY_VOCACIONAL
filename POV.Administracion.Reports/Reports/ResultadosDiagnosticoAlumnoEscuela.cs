using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.XtraCharts;


namespace POV.Administracion.Reports
{
    public partial class ResultadosDiagnosticoAlumnoEscuela : DevExpress.XtraReports.UI.XtraReport
    {

        public ResultadosDiagnosticoAlumnoEscuela(DataSet dataSet, DataSet inteligencias)
        {
            InitializeComponent(inteligencias);

            if (dataSet != null && dataSet.Tables.Contains("DatosReporte") && dataSet.Tables.Contains("DatosGenerales"))
            {
                this.DataSource = dataSet;
                this.xrlblDirector.DataBindings.Add("Text", this.DataSource, "DatosGenerales.Director");
                this.xrlblEscuela.DataBindings.Add("Text", this.DataSource, "DatosGenerales.Escuela");
                this.xrlblCicloEscolar.DataBindings.Add("Text", this.DataSource, "DatosGenerales.CicloEscolar");
                this.xrtblcllGrado.DataBindings.Add("Text", this.DataSource, "DatosReporte.Grado");
                this.xrtblcllGrupo.DataBindings.Add("Text", this.DataSource, "DatosReporte.Grupo");
                this.xrtblcllPConcluida.DataBindings.Add("Text", this.DataSource, "DatosReporte.PruebaTerminada");
                this.xrtblcllPInconclusa.DataBindings.Add("Text", this.DataSource, "DatosReporte.PruebaPendiente");
                this.xrtblcllTotalAlumno.DataBindings.Add("Text", this.DataSource, "DatosReporte.NumAlumnos");


                foreach (DataRow row in inteligencias.Tables[0].Rows)
                {
                    foreach (XRTableCell cell in this.xrtblrwInteligencias.Cells)
                    {
                        if (cell.Name == "xrtblcll" + row["Nombre"].ToString())
                        { 
                            cell.DataBindings.Add("Text", this.DataSource, "DatosReporte."+row["Nombre"]);
                        }

                        if (cell.Name == "xrtblcllAlumno" + row["Nombre"].ToString())
                        {
                            cell.DataBindings.Add("Text", this.DataSource, "DatosReporte.#" + row["Nombre"]);
                        }

                        if (cell.Name == "xrtblcllPuntaje" + row["Nombre"].ToString())
                        {
                            cell.DataBindings.Add("Text", this.DataSource, "DatosReporte.#" + row["Nombre"]);
                        }
                    }

                    Series newSerie = new Series( row["Nombre"].ToString(), ViewType.StackedBar);
                    this.xrcrtAlumnoInteligencias.Series.Add(newSerie);
                    this.xrcrtAlumnoInteligencias.Series[row["Nombre"].ToString()].DataSource = this.DataSource;
                    this.xrcrtAlumnoInteligencias.Series[row["Nombre"].ToString()].ArgumentDataMember = "DatosReporte.GradoGrupo";
                    this.xrcrtAlumnoInteligencias.Series[row["Nombre"].ToString()].ValueDataMembers.AddRange(new string[] { "DatosReporte."+ row["Nombre"].ToString() });
                   
                    Series newSerieAlumno = new Series(row["Nombre"].ToString(), ViewType.StackedBar);
                    this.xrcrtAlumnoInteligencia.Series.Add(newSerieAlumno);
                    this.xrcrtAlumnoInteligencia.Series[row["Nombre"].ToString()].DataSource = this.DataSource;
                    this.xrcrtAlumnoInteligencia.Series[row["Nombre"].ToString()].ArgumentDataMember = "DatosReporte.GradoGrupo";
                    this.xrcrtAlumnoInteligencia.Series[row["Nombre"].ToString()].ValueDataMembers.AddRange(new string[] { "DatosReporte.#" + row["Nombre"].ToString() });
                }

                this.xrcrtAlumnoPrueba.Series["Prueba terminada"].DataSource = this.DataSource;
                this.xrcrtAlumnoPrueba.Series["Prueba terminada"].ArgumentDataMember = "DatosReporte.GradoGrupo";
                this.xrcrtAlumnoPrueba.Series["Prueba terminada"].ValueDataMembers.AddRange(new string[] { "DatosReporte.PruebaTerminada" });

                this.xrcrtAlumnoPrueba.Series["Prueba pendiente"].DataSource = this.DataSource;
                this.xrcrtAlumnoPrueba.Series["Prueba pendiente"].ArgumentDataMember = "DatosReporte.GradoGrupo";
                this.xrcrtAlumnoPrueba.Series["Prueba pendiente"].ValueDataMembers.AddRange(new string[] { "DatosReporte.PruebaPendiente" });

                this.xrcrtAlumnoPrueba.Series["Total alumnos"].DataSource = this.DataSource;
                this.xrcrtAlumnoPrueba.Series["Total alumnos"].ArgumentDataMember = "DatosReporte.GradoGrupo";
                this.xrcrtAlumnoPrueba.Series["Total alumnos"].ValueDataMembers.AddRange(new string[] { "DatosReporte.NumAlumnos" });
               
            }
        }
    }
}
