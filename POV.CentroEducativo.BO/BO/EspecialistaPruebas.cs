using System;

namespace POV.CentroEducativo.BO
{
    public class EspecialistaPruebas : ICloneable
    {

        #region EspecialistaPruebaID
        private int? especialistaPruebaID;

        public int? EspecialistaPruebaID
        {
            get { return especialistaPruebaID; }
            set { especialistaPruebaID = value; }
        }
        #endregion

        #region CURP

        private string curp;

        public string Curp
        {
            get { return curp; }
            set { curp = value; }
        }
        #endregion

        #region NOMBRE
        private string nombre;

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }
        #endregion

        #region PrimerApellido

        private string primerApellido;

        public string PrimerApellido
        {
            get { return primerApellido; }
            set { primerApellido = value; }
        }
        #endregion

        #region SegundoApellido

        private string segundoApellido;

        public string SegundoApellido
        {
            get { return segundoApellido; }
            set { segundoApellido = value; }
        }
        #endregion

        #region Estatus
        private bool? estatus;
        /// <summary>
        /// Estatus para el docente el en sistema
        /// </summary>
        public bool? Estatus
        {
            get { return this.estatus; }
            set { this.estatus = value; }
        }

        #endregion

        #region FechaRegistro
        private DateTime? fechaRegistro;
        /// <summary>
        /// FechaRegistro para el docente el en sistema
        /// </summary>
        public DateTime? FechaRegistro
        {
            get { return this.fechaRegistro; }
            set { this.fechaRegistro = value; }
        }

        #endregion

        #region FechaNacimiento
        private DateTime? fechaNacimiento;
        /// <summary>
        /// Fecha de nacimiento para el docente el en sistema
        /// </summary>
        public DateTime? FechaNacimiento
        {
            get { return this.fechaNacimiento; }
            set { this.fechaNacimiento = value; }
        }

        #endregion

        #region Sexo
        private bool? sexo;
        public bool? Sexo
        {
            get { return sexo; }
            set { sexo = value; }
        }
        #endregion

        #region Correo

        private string correo;
        public string Correo
        {
            get { return correo; }
            set { correo = value; }
        }
        #endregion

        #region Clave
        private string clave;
        public string Clave
        {
            get { return clave; }
            set { clave = value; }
        }
        #endregion

        #region EstatusIdentificacion
        private bool? estatusIdentificacion;
        public bool? EstatusIdentificacion
        {
            get { return estatusIdentificacion; }
            set { estatusIdentificacion = value; }
        }

        #endregion

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
