using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Reactivos.BO;

namespace POV.Prueba.BO
{
    /// <summary>
    /// Representa el banco de reactivos asignados
    /// </summary>
    public class ReactivoBanco
    {
        private long? reactivoBancoID;
        /// <summary>
        /// Identificador del ReactivoBanco
        /// </summary>
        public long? ReactivoBancoID
        {
            get { return this.reactivoBancoID; }
            set { this.reactivoBancoID = value; }
        }

        private Reactivo reactivoOriginal;
        /// <summary>
        /// Identificador del reactivo original del catálogo de reactivos
        /// </summary>
        public Reactivo ReactivoOriginal
        {
            get { return this.reactivoOriginal; }
            set { this.reactivoOriginal = value; }
        }

        private int? orden;
        /// <summary>
        /// Orden de la presentacion del reactivo
        /// </summary>
        public int? Orden
        {
            get { return this.orden; }
            set { this.orden = value; }
        }
        private bool? estaSeleccionado;
        /// <summary>
        /// Esta seleccionado el reactivo
        /// </summary>
        public bool? EstaSeleccionado
        {
            get { return this.estaSeleccionado; }
            set { this.estaSeleccionado = value; }
        }
        private bool? activo;
        /// <summary>
        /// Estatus de la asignacion
        /// </summary>
        public bool? Activo
        {
            get { return this.activo; }
            set { this.activo = value; }
        }
        private Reactivo reactivo;
        /// <summary>
        /// Reactivo
        /// </summary>
        public Reactivo Reactivo 
        {
            get { return this.reactivo; }
            set { this.reactivo = value; }
        }
    }
}
