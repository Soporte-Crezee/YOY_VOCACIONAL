// Clase docente para la red social
using System;
using POV.Localizacion.BO;
using System.Collections.Generic;

namespace POV.CentroEducativo.BO
{
    /// <summary>
    /// Catalogo de Docentes disponibles en el sistema
    /// </summary>
    public class Docente : ICloneable
    {
        private int? docenteID;
        /// <summary>
        /// Identificador autonumerico de sistema para la el docente
        /// </summary>
        public int? DocenteID
        {
            get { return this.docenteID; }
            set { this.docenteID = value; }
        }
        private string curp;
        /// <summary>
        /// Curp del docente
        /// </summary>
        public string Curp
        {
            get { return this.curp; }
            set { this.curp = value; }
        }
        private string nombre;
        /// <summary>
        /// Nombre del docente
        /// </summary>
        public string Nombre
        {
            get { return this.nombre; }
            set { this.nombre = value; }
        }
        private string primerApellido;
        /// <summary>
        /// Primer Apellido para el docente
        /// </summary>
        public string PrimerApellido
        {
            get { return this.primerApellido; }
            set { this.primerApellido = value; }
        }
        private string segundoApellido;
        /// <summary>
        /// Segundo Apellido para el docente
        /// </summary>
        public string SegundoApellido
        {
            get { return this.segundoApellido; }
            set { this.segundoApellido = value; }
        }
        private bool? estatus;
        /// <summary>
        /// Estatus para el docente el en sistema
        /// </summary>
        public bool? Estatus
        {
            get { return this.estatus; }
            set { this.estatus = value; }
        }
        private DateTime? fechaRegistro;
        /// <summary>
        /// FechaRegistro para el docente el en sistema
        /// </summary>
        public DateTime? FechaRegistro
        {
            get { return this.fechaRegistro; }
            set { this.fechaRegistro = value; }
        }
        private DateTime? fechaNacimiento;
        /// <summary>
        /// Fecha de nacimiento para el docente el en sistema
        /// </summary>
        public DateTime? FechaNacimiento
        {
            get { return this.fechaNacimiento; }
            set { this.fechaNacimiento = value; }
        }
        private bool? sexo;
        public bool? Sexo
        {
            get { return sexo; }
            set { sexo = value; }
        }
        private string correo;
        public string Correo
        {
            get { return correo; }
            set { correo = value; }
        }
        private string clave;
        public string Clave
        {
            get { return clave; }
            set { clave = value; }
        }
        private bool? estatusIdentificacion;
        public bool? EstatusIdentificacion
        {
            get { return estatusIdentificacion; }
            set { estatusIdentificacion = value; }
        }

        public string Cedula { get; set; }
        public string NivelEstudio { get; set; }
        public string Titulo { get; set; }
        public string Especialidades { get; set; }
        public string Experiencia { get; set; }
        public string Cursos { get; set; }
        public string UsuarioSkype { get; set; }
        public bool? EsPremium { get; set; }
        public bool? PermiteAsignaciones { get; set; }

        public string NombreUsuario { get; set; }

        public string NombreCompletoDocente
        {
            get { return (this.nombre + " " + primerApellido + " " + segundoApellido).Trim(); }
        }

        //public virtual List<Alumno> Alumnos { get; set; }

        public virtual List<Universidad> Universidades { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public virtual List<SesionOrientacion> Sesiones { get; set; }
    }
}
