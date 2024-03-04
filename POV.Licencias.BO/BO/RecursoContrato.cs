using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Comun.BO;

namespace POV.Licencias.BO
{
    public class RecursoContrato : ICloneable
    {
        
        public RecursoContrato() 
        {
            this.pruebasContrato = new ObjetoListado<PruebaContrato>();
        }

        public RecursoContrato(IEnumerable<PruebaContrato> pruebasContrato)
        {
            this.pruebasContrato = new ObjetoListado<PruebaContrato>(pruebasContrato);
        }
        private long? recursoContratoID;
        /// <summary>
        /// Identificador de la prueba contrato
        /// </summary>
        public long? RecursoContratoID
        {
            get { return recursoContratoID; }
            set { recursoContratoID = value; }
        }
        
        private bool? esAsignacionManual;
        /// <summary>
        /// Determina si la asignacion de paquetes es manual o no lo es.
        /// </summary>
        public bool? EsAsignacionManual
        {
            get { return esAsignacionManual; }
            set { esAsignacionManual = value; }
        }

        private bool? esPaquetePorPruebaPivote;
        /// <summary>
        /// Determina si la asignacion de paquetes es a partir del resultado de una prueba pivote
        /// </summary>
        public bool? EsPaquetePorPruebaPivote
        {
            get { return esPaquetePorPruebaPivote; }
            set { esPaquetePorPruebaPivote = value; }
        }
        private bool? activo;
        /// <summary>
        /// Determina si el ciclo esta activo
        /// </summary>
        public bool? Activo
        {
            get { return activo; }
            set
            {
                    activo = value;

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
        private ObjetoListado<PruebaContrato> pruebasContrato;
        /// <summary>
        /// lista de PruebaContrato
        /// </summary>
        public IEnumerable<PruebaContrato> PruebasContrato
        {
            get { return this.pruebasContrato.Objetos; }
        }

        #region *** metodos de lista de pruebaContrato ***
        /// <summary>
        /// Agrega PruebaContrato
        /// </summary>
        public PruebaContrato PruebaContratoAgregar(PruebaContrato pruebaContrato)
        {
            return this.pruebasContrato.Agregar(PruebaContratoIdentificar(pruebaContrato), pruebaContrato);
        }
        /// <summary>
        /// Obtener PruebaContrato basado en el indice
        /// </summary>
        public PruebaContrato PruebaContratoObtener(int indice)
        {
            return this.pruebasContrato.Obtener(indice);
        }
        /// <summary>
        /// Elimina PruebaContrato
        /// </summary>
        public PruebaContrato PruebaContratoEliminar(PruebaContrato pruebaContrato)
        {
            return this.pruebasContrato.Eliminar(PruebaContratoIdentificar(pruebaContrato), pruebaContrato);
        }
        /// <summary>
        /// Elimina PruebaContrato basado en el indice
        /// </summary>
        public PruebaContrato PruebaContratoEliminar(int indice)
        {
            return this.pruebasContrato.Eliminar(indice);
        }
        /// <summary>
        /// Obtiene el numero de elementos de la lista de PruebaContrato
        /// </summary>
        public int PruebaContratoElementos()
        {
            return this.pruebasContrato.Elementos();
        }
        /// <summary>
        /// Obtiene el numero completo de elementos de la lista de PruebaContrato
        /// </summary>
        public int PruebaContratoElementosTodos()
        {
            return this.pruebasContrato.ElementosTodos();
        }
        /// <summary>
        /// Elimina todos los elementos del lista de PruebaContrato
        /// </summary>
        public void PruebaContratoLimpiar()
        {
            this.pruebasContrato.Limpiar();
        }
        /// <summary>
        /// Obtener el estado de la PruebaContrato
        /// </summary>
        public EObjetoEstado PruebaContratoEstado(PruebaContrato pruebaContrato)
        {
            return this.pruebasContrato.Estado(pruebaContrato);
        }
        /// <summary>
        /// Obtener el  objeto original del PruebaContrato
        /// </summary>
        public PruebaContrato PruebaContratoOriginal(PruebaContrato pruebaContrato)
        {
            return this.pruebasContrato.Original(pruebaContrato);
        }
        /// <summary>
        /// Definir la expresión que identificara al PruebaContrato
        /// </summary>
        private static Func<PruebaContrato, bool> PruebaContratoIdentificar(PruebaContrato pruebaContrato)
        {
            return (uo => pruebaContrato.PruebaContratoID != null && uo.PruebaContratoID == pruebaContrato.PruebaContratoID);
        }

        #endregion

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public object CloneAll()
        {
            return this.Clone();
        }
    }
}
