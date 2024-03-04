using System;
using System.Collections.Generic;
using System.Text;

namespace POV.ContenidosDigital.BO
{
    /// <summary>
    /// Representa un tipo de documento del contenido digital
    /// </summary>
    public class TipoDocumento
    {
        private int? tipoDocumentoID;
        /// <summary>
        /// Identificador del tipo de documento
        /// </summary>
        public int? TipoDocumentoID
        {
            get { return this.tipoDocumentoID; }
            set { this.tipoDocumentoID = value; }
        }
        private string nombre;
        /// <summary>
        /// Nombre del tipo de docuemento
        /// </summary>
        public string Nombre
        {
            get { return this.nombre; }
            set { this.nombre = value; }
        }
        private string extension;
        /// <summary>
        /// Extension del tipo de documento
        /// </summary>
        public string Extension
        {
            get { return this.extension; }
            set { this.extension = value; }
        }
        private string mIME;
        /// <summary>
        /// MIME del tipo de documento
        /// </summary>
        public string MIME
        {
            get { return this.mIME; }
            set { this.mIME = value; }
        }
        private bool? esEditable;
        /// <summary>
        /// Representa si es o no editable el tipo de documento
        /// </summary>
        public bool? EsEditable
        {
            get { return this.esEditable; }
            set { this.esEditable = value; }
        }
        private string fuente;
        /// <summary>
        /// Fuente del tipo de documento
        /// </summary>
        public string Fuente
        {
            get { return this.fuente; }
            set { this.fuente = value; }
        }
        private bool? activo;
        /// <summary>
        /// Estado del tipo de documento
        /// </summary>
        public bool? Activo
        {
            get { return this.activo; }
            set { this.activo = value; }
        }
        private DateTime? fechaRegistro;
        /// <summary>
        /// Fecha de registro del tipo de documento
        /// </summary>
        public DateTime? FechaRegistro
        {
            get { return this.fechaRegistro; }
            set { this.fechaRegistro = value; }
        }

        private string imagenDocumento;
        /// <summary>
        /// Fecha de registro del tipo de documento
        /// </summary>
        public string ImagenDocumento
        {
            get { return this.imagenDocumento; }
            set { this.imagenDocumento = value; }
        }
    }
}
