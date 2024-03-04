using System;
using System.Collections.Generic;
using System.Text;
using POV.Modelo.BO;
using POV.Prueba.BO;

namespace POV.Prueba.Diagnostico.Dinamica.BO
{
    /// <summary>
    /// Clase Abstracta Escala Dinamica
    /// </summary>
    public abstract class AEscalaDinamica : APuntaje
    {
        private string nombre;
        /// <summary>
        /// Nombre de la escala dinamica
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
        private string descripcion;
        /// <summary>
        /// Descripcion de la escala dinamica
        /// </summary>
        public string Descripcion
        {
            get { return this.descripcion; }
            set
            {
                if (this.descripcion != value)
                {
                    this.descripcion = value;
                    this.Cambio();
                }
            }
        }
        private Clasificador clasificador;
        /// <summary>
        /// Descripcion de la escala dinamica
        /// </summary>
        public Clasificador Clasificador
        {
            get { return this.clasificador; }
            set
            {
                if (this.clasificador != null)
                {
                    if (value != null && this.clasificador.ClasificadorID != value.ClasificadorID)
                    {
                        this.Cambio();
                    }
                    this.clasificador = value;
                }
                else
                {
                    this.clasificador = value;
                }
            }
        }

        /// <summary>
        /// Enumerable del tipo de prueba dinamica
        /// </summary>
        public abstract ETipoEscalaDinamica? TipoEscalaDinamica
        {
            get;
        }
    }
}
