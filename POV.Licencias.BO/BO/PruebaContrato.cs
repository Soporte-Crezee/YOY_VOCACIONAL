using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Prueba.BO;
using POV.Comun.BO;

namespace POV.Licencias.BO
{
    /// <summary>
    /// Prueba asignada al contrato
    /// </summary>
    public class PruebaContrato : ICloneable, IObservableChange
    {

        private long? pruebaContratoID;
        /// <summary>
        /// Identificador de la prueba contrato
        /// </summary>
        public long? PruebaContratoID
        {
            get { return pruebaContratoID; }
            set { pruebaContratoID = value; }
        }

        private APrueba prueba;
        /// <summary>
        /// Prueba asignada al contrato
        /// </summary>
        public APrueba Prueba
        {
            get { return this.prueba; }
            set { this.prueba = value; }
        }

        private ETipoPruebaContrato? tipoPruebaContrato;
        /// <summary>
        /// Tipo de prueba asignada
        /// </summary>
        public ETipoPruebaContrato? TipoPruebaContrato
        {
            get { return this.tipoPruebaContrato; }
            set { this.tipoPruebaContrato = value; }
        }

        private DateTime? fechaRegistro;
        /// <summary>
        /// Fecha de Registro
        /// </summary>
        public DateTime? FechaRegistro
        {
            get { return this.fechaRegistro; }
            set { this.fechaRegistro = value; }
        }

        private bool? activo;
        /// <summary>
        /// Indica si el registro esta activo
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
            return this.Clone();
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
