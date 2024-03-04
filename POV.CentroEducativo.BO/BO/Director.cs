using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using POV.CentroEducativo.BO;

namespace POV.CentroEducativo.BO
{
    /// <summary>
    /// Clase tipo director
    /// </summary>
    public class Director : ICloneable
    {
        private int? directorID;
        /// <summary>
        /// Identificador del director
        /// </summary>
        public int? DirectorID
        {
            get { return this.directorID; }
            set { this.directorID = value; }
        }
        private string nombre;
        /// <summary>
        /// Nombre del director
        /// </summary>
        public string Nombre
        {
            get { return this.nombre; }
            set { this.nombre = value; }
        }
        private string curp;

        public string Curp
        {
            get { return curp; }
            set { curp = value; }
        }
        private string primerApellido;

        public string PrimerApellido
        {
            get { return primerApellido; }
            set { primerApellido = value; }
        }
        private string segundoApellido;

        public string SegundoApellido
        {
            get { return segundoApellido; }
            set { segundoApellido = value; }
        }
        private DateTime? fechaNacimiento;

        public DateTime? FechaNacimiento
        {
            get { return fechaNacimiento; }
            set { fechaNacimiento = value; }
        }
        private bool? sexo;

        public bool? Sexo
        {
            get { return sexo; }
            set { sexo = value; }
        }
        private string nivelEscolar;

        public string NivelEscolar
        {
            get { return nivelEscolar; }
            set { nivelEscolar = value; }
        }
        private string correo;

        public string Correo
        {
            get { return correo; }
            set { correo = value; }
        }
        private string telefono;

        public string Telefono
        {
            get { return telefono; }
            set { telefono = value; }
        }
        private DateTime? fechaRegistro;

        public DateTime? FechaRegistro
        {
            get { return fechaRegistro; }
            set { fechaRegistro = value; }
        }
        private bool? estatus;

        public bool? Estatus
        {
            get { return estatus; }
            set { estatus = value; }
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

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
