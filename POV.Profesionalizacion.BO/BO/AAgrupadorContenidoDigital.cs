using System;
using System.Collections.Generic;
using POV.ContenidosDigital.BO;

namespace POV.Profesionalizacion.BO
{
    /// <summary>
    /// Agrupador de Contenido Digital
    /// </summary>
    public abstract class AAgrupadorContenidoDigital : ICloneable
    {
        #region Propiedades
        private Int64? agrupadorContenidoDigitalID;
        /// <summary>
        /// Identificador del Contenido Digital
        /// </summary>
        public Int64? AgrupadorContenidoDigitalID
        {
            get { return this.agrupadorContenidoDigitalID; }
            set { this.agrupadorContenidoDigitalID = value; }
        }
        private string nombre;
        /// <summary>
        /// Nombre del Agrupador de Contenido Digital
        /// </summary>
        public string Nombre
        {
            get { return this.nombre; }
            set { this.nombre = value; }
        }
        private string competencias;
        /// <summary>
        /// Nombre del Agrupador de Contenido Digital
        /// </summary>
        public string Competencias
        {
            get { return this.competencias; }
            set { this.competencias = value; }
        }
        private string aprendizajes;
        /// <summary>
        /// Nombre del Agrupador de Contenido Digital
        /// </summary>
        public string Aprendizajes
        {
            get { return this.aprendizajes; }
            set { this.aprendizajes = value; }
        }
        private bool? esPredeterminado;
        /// <summary>
        /// Agrupador Predeterminado
        /// </summary>
        public bool? EsPredeterminado
        {
            get { return this.esPredeterminado; }
            set { this.esPredeterminado = value; }
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
        private EEstatusProfesionalizacion? estatus;
        /// <summary>
        /// Estatus del Agrupador
        /// </summary>
        public EEstatusProfesionalizacion? Estatus
        {
            get { return this.estatus; }
            set { this.estatus = value; }
        }

        /// <summary>
        /// Tipo de Agrupador
        /// </summary>
        public abstract ETipoAgrupador TipoAgrupador { get; }
        #endregion
        /// <summary>
        /// Lista de ContenidoDigital
        /// </summary>
        public List<ContenidoDigital> ContenidosDigitales
        {
            get
            {
                return contenidosDigitales;
            }
            set
            {
                contenidosDigitales = value;
            }
        }
        private List<ContenidoDigital> contenidosDigitales;

        #region Métodos
        public abstract void Agregar(AAgrupadorContenidoDigital agrupadorContenido);
        public abstract void Eliminar(AAgrupadorContenidoDigital agrupadorContenido);
        #endregion

        #region Miembros de ICloneable

        public object Clone()
        {
            return (AAgrupadorContenidoDigital)this.MemberwiseClone();
        }

        public virtual AAgrupadorContenidoDigital CloneAll()
        {
            if (this is Asistencia)
                throw new NotImplementedException("Característica no implementada para Asistencias");

            if (this is Curso)
                throw new NotImplementedException("Característica no implementada para Cursos");

            AAgrupadorContenidoDigital agrupadorCopiaRet = null;
            if (this is AgrupadorCompuesto)
            {
                AgrupadorCompuesto agrupadorCopia = this.Clone() as AgrupadorCompuesto;

                agrupadorCopia.InitList();

                foreach (AAgrupadorContenidoDigital agrupadorHijo in ((AgrupadorCompuesto)this).AgrupadoresContenido)
                {
                    agrupadorCopia.Agregar(agrupadorHijo.CloneAll());
                }
                // Agregar Contenidos Digitales
                agrupadorCopia.ContenidosDigitales = this.CloneContenidos();

                return agrupadorCopia;
            }

            if (this is AgrupadorSimple)
            {
                AgrupadorSimple agrupadorCopia = this.Clone() as AgrupadorSimple;
                // Agregar Contenidos Digitales
                agrupadorCopia.ContenidosDigitales = this.CloneContenidos();

                return agrupadorCopia;
            }

            return agrupadorCopiaRet;
        }

        private List<ContenidoDigital> CloneContenidos()
        {
            List<ContenidoDigital> contenidosReturn = new List<ContenidoDigital>();
            if (this.contenidosDigitales != null && this.contenidosDigitales.Count > 0)
            {
                foreach (ContenidoDigital contenido in this.contenidosDigitales)
                {
                    contenidosReturn.Add(contenido.Clone() as ContenidoDigital);
                }
            }

            return contenidosReturn;
        }
        #endregion
    }
}
