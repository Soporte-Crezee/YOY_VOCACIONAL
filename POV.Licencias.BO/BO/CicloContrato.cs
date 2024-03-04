using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.CentroEducativo.BO;
using POV.Comun.BO;

namespace POV.Licencias.BO
{
    public class CicloContrato : ICloneable, IObservableChange
    {
        private long? cicloContratoID;
        /// <summary>
        /// Identificador del ciclo contrato
        /// </summary>
        public long? CicloContratoID
        {
            get { return cicloContratoID; }
            set { cicloContratoID = value; }
        }
        private bool? estaLiberado;
        /// <summary>
        /// Determina si el ciclo ha sido liberado
        /// </summary>
        public bool? EstaLiberado
        {
            get { return estaLiberado; }
            set
            {
                if (estaLiberado != value)
                {
                    estaLiberado = value;
                    this.Cambio();
                }
            }
        }
        private bool? activo;
        /// <summary>
        /// Determina si el ciclo esta activo
        /// </summary>
        public bool? Activo
        {
            get { return activo; }
            set {
                if (activo != value)
                {
                    activo = value;
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
            get { return fechaRegistro; }
            set { fechaRegistro = value; }
        }
        private CicloEscolar cicloEscolar;
        /// <summary>
        /// Ciclo escolar del ciclo contrato
        /// </summary>
        public CicloEscolar CicloEscolar
        {
            get { return cicloEscolar; }
            set { cicloEscolar = value; }
        }

        private RecursoContrato recursoContrato;
        /// <summary>
        /// RecursoContrato del ciclo contrato
        /// </summary>
        public RecursoContrato RecursoContrato
        {
            get { return recursoContrato; }
            set { recursoContrato = value; }
        }
        /// <summary>
        /// Verifica si el ciclo escolar esta vigente de acuerdo a la fecha actual del sistema
        /// </summary>
        /// <returns>true si es vigente de acuerdo a la fecha actual del sistema, false en caso contrario</returns>
        public bool EsCicloEscolarVigente()
        {
            bool vigente = false;
            if (cicloEscolar != null)
            {
                DateTime fechaActual = DateTime.Now;
                DateTime inicioCiclo = cicloEscolar.InicioCiclo.Value;
                DateTime finCiclo = cicloEscolar.FinCiclo.Value;
                if (DateTime.Compare(fechaActual, inicioCiclo) >= 0 && DateTime.Compare(fechaActual, finCiclo) <= 0)
                    return true;
            }

            return vigente;
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public object CloneAll()
        {
            CicloContrato ciclo = this.Clone() as CicloContrato;
            if (this.cicloEscolar != null)
                ciclo.cicloEscolar = this.cicloEscolar.Clone() as CicloEscolar;
            if (this.recursoContrato != null)
                ciclo.RecursoContrato = this.recursoContrato.CloneAll() as RecursoContrato;

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
