using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.ContenidosDigital.BO
{
    /// <summary>
    /// Clase que representa un visor de contenidos digitales genericos
    /// </summary>
    public abstract class AVisor
    {
        private int? visorID;
        /// <summary>
        /// Identificador del visor
        /// </summary>
        public int? VisorID
        {
            get { return this.visorID; }
            set { this.visorID = value; }
        }
        private string clave;
        /// <summary>
        /// Clave del visor
        /// </summary>
        public string Clave
        {
            get { return this.clave; }
            set { this.clave = value; }
        }
        private DateTime? fechaRegistro;
        /// <summary>
        /// Fecha de registro
        /// </summary>
        public DateTime? FechaRegistro
        {
            get { return this.fechaRegistro; }
            set { this.fechaRegistro = value; }
        }
        private bool? activo;
        /// <summary>
        /// Indica si el registro esta activo o no
        /// </summary>
        public bool? Activo
        {
            get { return this.activo; }
            set { this.activo = value; }
        }

        public abstract bool? EsInterno { get; }

        private List<TipoDocumento> listaTiposDocumento;

        public List<TipoDocumento> ListaTiposDocumento
        {
            get { return listaTiposDocumento; }
            set { listaTiposDocumento = value; }
        }

    }
}
