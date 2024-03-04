using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Comun.BO;

namespace POV.ContenidosDigital.BO
{
    /// <summary>
    /// Referencia URL del contenido digital
    /// </summary>
    public class URLContenido : ICloneable, IObservableChange
    {
        private long? uRLContenidoID;
        /// <summary>
        /// Identificador de la url del contenido
        /// </summary>
        public long? URLContenidoID
        {
            get { return this.uRLContenidoID; }
            set { this.uRLContenidoID = value; }
        }
        private string uRL;
        /// <summary>
        /// URL
        /// </summary>
        public string URL
        {
            get { return this.uRL; }
            set {
                if (uRL != value)
                {
                    uRL = value;
                    this.Cambio();
                }
            }
        }
        private bool? esPredeterminada;
        /// <summary>
        /// Indica si la url es predeterminada o no
        /// </summary>
        public bool? EsPredeterminada
        {
            get { return this.esPredeterminada; }
            set
            {
                if (this.esPredeterminada != value)
                {
                    this.esPredeterminada = value;
                    this.Cambio();
                }
            }
        }
        private string nombre;
        /// <summary>
        /// Nombre del url
        /// </summary>
        public string Nombre
        {
            get { return this.nombre; }
            set
            {
                if (this.nombre != value)
                {
                    this.nombre = value;
                    this.Cambio();
                }
            }
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
            set
            {
                if (this.activo != value)
                {
                    this.activo = value;
                    this.Cambio();
                }
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public object CloneAll()
        {
            return Clone();
        }
        private IObserverChange observer;
        public void Registrar(IObserverChange observer)
        {
            this.observer = observer;
        }
        private void Cambio()
        {
            if (this.observer != null)
                this.observer.Cambio();
        }

    }
}
