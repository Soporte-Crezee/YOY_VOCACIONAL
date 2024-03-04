//Clase creada como adapter para almacenar los datos de un alumno junto a sus resultados de examen diagnóstico
//nivel de proclividad y tiempo que le llevo realizar el examen.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.CentroEducativo.BO;
using POV.Reactivos.BO;
using POV.Modelo.Garner.BO;
namespace POV.Social.BO
{
    /// <summary>
    /// Relación entre alumnos, sus resultados de examen diagnóstico, nivel de proclividad y tiempo de examen.
    /// </summary>
    public class ReporteResultadoDiagnosticoPorGrupo
    {
        private int numero;
        /// <summary>
        /// numero en la secuencia en que se aparece el alumno.
        /// </summary>
        public int Numero
        {
            get { return this.numero; }
            set { this.numero = value; }
        }

        private string nombreAlumno;
        /// <summary>
        /// nombre del alumno correspondiente al registro.
        /// </summary>
        /// 
        public string NombreAlumno
        {
            get { return this.nombreAlumno; }
            set { this.nombreAlumno = value; }
        }
        private long? alumnoID;
        /// <summary>
        /// identificador del alumno
        /// </summary>
        public long? AlumnoID
        {
            get { return alumnoID; }
            set { alumnoID = value; }
        }
        private string sexoAlumno;
        /// <summary>
        /// género del alumno
        /// </summary>
        public string SexoAlumno
        {
            get { return this.sexoAlumno; }
            set{ this.sexoAlumno = value; }
        }

        private int edadAlumno;
        /// <summary>
        /// edad del alumno.
        /// </summary>
        public int EdadAlumno
        {
            get { return this.edadAlumno; }
            set { this.edadAlumno = value; }
        }

        private string fechaAplicacionDiagnosticoAlumno;
        /// <summary>
        /// fecha en que fué aplicada la prueba diagnóstica.
        /// </summary>
        public string FechaAplicacionDiagnosticoAlumno
        {
            get { return this.fechaAplicacionDiagnosticoAlumno; }
            set { this.fechaAplicacionDiagnosticoAlumno = value; }
        }

        private string tiempoRequeridoAlumno;
        /// <summary>
        /// tiempo que requirió el alumno en contestar todas las preguntas del examen diagnóstico.
        /// </summary>
        public string TiempoRequeridoAlumno 
        {
            get { return this.tiempoRequeridoAlumno; }
            set { this.tiempoRequeridoAlumno = value; }
        }

        private List<TipoInteligencia> inteligenciaPredominante;
        /// <summary>
        /// tipo o tipos de inteligencia en las que el alumno tuvo un mayor puntaje.
        /// </summary>
        public List<TipoInteligencia> InteligenciaPredominante
        {
            get { return this.inteligenciaPredominante; }
            set { this.inteligenciaPredominante = value; }
        }

        private bool conResultado;
        /// <summary>
        /// booleano que indica si el alumno concluyó o presentó su examen diagnóstico para habilitar una función
        /// en el gridView.
        /// </summary>
        public bool ConResultado 
        {
            get { return this.conResultado; }
            set { this.conResultado = value; }
        }

        private int? idResultadoPrueba;
        ///<sumary>
        ///id del resultado de la prueba diagnóstica del alumno.
        ///</sumary>
        public int? IdResultadoPrueba
        {
            get { return this.idResultadoPrueba; }
            set { this.idResultadoPrueba = value; }
        }

    }
}
