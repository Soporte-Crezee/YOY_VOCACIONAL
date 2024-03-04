// Clase alumno para la red social
using System;
using POV.Localizacion.BO;
using System.Collections.Generic;

namespace POV.CentroEducativo.BO
{
    /// <summary>
    /// Catalogo de alumnos disponibles en el sistema
    /// </summary>
    public class Alumno : ICloneable
    {
        private long? alumnoID;
        /// <summary>
        /// Identificador del alumno
        /// </summary>
        public long? AlumnoID
        {
            get { return this.alumnoID; }
            set { this.alumnoID = value; }
        }
        private string curp;
        /// <summary>
        /// Curp del alumno
        /// </summary>
        public string Curp
        {
            get { return this.curp; }
            set { this.curp = value; }
        }
        private string matricula;

        public string Matricula
        {
            get { return matricula; }
            set { matricula = value; }
        }
        private string nombre;
        /// <summary>
        /// Nombre del alumno
        /// </summary>
        public string Nombre
        {
            get { return this.nombre; }
            set { this.nombre = value; }
        }
        private string primerApellido;
        /// <summary>
        /// Primer Apellido del alumno
        /// </summary>
        public string PrimerApellido
        {
            get { return this.primerApellido; }
            set { this.primerApellido = value; }
        }
        private string segundoApellido;
        /// <summary>
        /// Segundo Apellido del alumno
        /// </summary>
        public string SegundoApellido
        {
            get { return this.segundoApellido; }
            set { this.segundoApellido = value; }
        }
        private DateTime? fechaNacimiento;
        /// <summary>
        /// Fecha de nacimiento del alumno
        /// </summary>
        public DateTime? FechaNacimiento
        {
            get { return this.fechaNacimiento; }
            set { this.fechaNacimiento = value; }
        }
        private string direccion;
        /// <summary>
        /// Direccion del alumno
        /// </summary>
        public string Direccion
        {
            get { return this.direccion; }
            set { this.direccion = value; }
        }
        private string nombreCompletoTutor;
        /// <summary>
        /// Nombre de la persona que sea su tutor
        /// </summary>
        public string NombreCompletoTutor
        {
            get { return this.nombreCompletoTutor; }
            set { this.nombreCompletoTutor = value; }
        }
        private string nombreCompletoTutorDos;
        /// <summary>
        /// Nombre de la persona que sea su segundo tutor
        /// </summary>
        public string NombreCompletoTutorDos
        {
            get { return this.nombreCompletoTutorDos; }
            set { this.nombreCompletoTutorDos = value; }
        }
        /// <summary>
        /// Nombre completo del alumno
        /// </summary>
        public string NombreCompletoAlumno
        {
            get { return (this.nombre + " " + primerApellido + " " + segundoApellido).Trim(); }
        }

        private bool? estatus;
        /// <summary>
        /// Estatus
        /// </summary>
        public bool? Estatus
        {
            get { return this.estatus; }
            set { this.estatus = value; }
        }
        private DateTime? fechaRegistro;
        /// <summary>
        /// Fecha en que se realizo el registro
        /// </summary>
        public DateTime? FechaRegistro
        {
            get { return this.fechaRegistro; }
            set { this.fechaRegistro = value; }
        }
        private bool? sexo;
        /// <summary>
        /// Sexo del alumno
        /// </summary>
        public bool? Sexo
        {
            get { return this.sexo; }
            set { this.sexo = value; }
        }
        private bool? estatusIdentificacion;
        public bool? EstatusIdentificacion
        {
            get { return estatusIdentificacion; }
            set { estatusIdentificacion = value; }
        }

        private bool? correoConfirmado;
        public bool? CorreoConfirmado
        {
            get { return correoConfirmado; }
            set { correoConfirmado = value; }
        }

        private Ubicacion ubicacion;
        /// <summary>
        /// Ubicacion del alumno
        /// </summary>
        public Ubicacion Ubicacion
        {
            get { return this.ubicacion; }
            set { this.ubicacion = value; }
        }

        private string escuela;
        /// <summary>
        /// Escuela de procedencia
        /// </summary>
        public string Escuela
        {
            get { return this.escuela; }
            set { this.escuela = value; }
        }

        public EGrado? Grado { get; set; }

        private bool? recibirInformacion;
        /// <summary>
        /// Recibir informacion
        /// </summary>
        public bool? RecibirInformacion
        {
            get { return recibirInformacion; }
            set { recibirInformacion = value; }
        }

        private bool? datosCompletos;
        /// <summary>
        /// Datos completos
        /// </summary>
        public bool? DatosCompletos
        {
            get { return datosCompletos; }
            set { datosCompletos = value; }
        }

        private bool? carreraSeleccionada;
        /// <summary>
        /// Carrera Seleccionada
        /// </summary>
        public bool? CarreraSeleccionada
        {
            get { return carreraSeleccionada; }
            set { carreraSeleccionada = value; }
        }

        public List<AreaConocimiento> AreasConocimiento { get; set; }

        public virtual List<TutorAlumno> Tutores { get; set; }

        public virtual List<Universidad> Universidades { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        /// <summary>
        /// Devuelve la edad del Alumno expresada en años calculada con la fecha actual
        /// </summary>
        /// <returns>int edadEnAnios</returns>
        public int EdadEnAnios()
        {
            DateTime dFechaActual = System.DateTime.Today;
            return EdadEnAnios(dFechaActual);
        }

        /// <summary>
        /// Devuelve la edad del Alumno expresada en años calculada con una fecha actual específica
        /// </summary>
        /// <param name="dFechaActual">fecha hasta la que se requiere calcular la edad</param>
        /// <returns>int edadEnAnios</returns>
        public int EdadEnAnios(DateTime dFechaActual)
        {
            DateTime dFechaNac = (DateTime)this.fechaNacimiento;
            int nAnios = dFechaActual.Year - dFechaNac.Year;
            DateTime dFechaComp = dFechaNac.Date.AddYears(nAnios);               // sólo la parte de fecha en dFechaNac
            return dFechaActual.Date.CompareTo(dFechaComp) < 0 ? --nAnios : nAnios;     // sólo la parte de fecha en dFechaActual
        }

        ///<summary>
        /// Devuelve si el Alumno tiene paquete premium
        ///</summary>
        public bool? Premium { get; set; }

        public double? Credito { get; set; }
        public double? CreditoUsado { get; set; }
        public double? Saldo { get; set; }

        public virtual List<SesionOrientacion> Sesiones { get; set; }

        public ENivelEscolar? NivelEscolar { get; set; }
        public EEstadoPago? EstatusPago { get; set; }
        public string IDReferenciaOXXO { get; set; }
        public string ReferenciaOXXO { get; set; }

        public bool? NotificacionPago { get; set; }
    }
}
