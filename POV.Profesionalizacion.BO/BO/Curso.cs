using System;
using System.Collections.Generic;

namespace POV.Profesionalizacion.BO
{
    /// <summary>
    /// Curso
    /// </summary>
    public class Curso : AgrupadorCompuesto
    {
        private TemaCurso temaCurso;
        /// <summary>
        /// Tema del Curso
        /// </summary>
        public TemaCurso TemaCurso
        {
            get { return this.temaCurso; }
            set { this.temaCurso = value; }
        }
        private EPresencial? presencial;
        /// <summary>
        /// Enumerable para elegir la modalidad de grupo entre PRESENCIAL, SEMI-PRESENCIAL y NO-PRESENCIAL
        /// </summary>
        public EPresencial? Presencial
        {
            get { return this.presencial; }
            set { this.presencial = value; }
        }

        public override ETipoAgrupador TipoAgrupador
        {
            get
            {
                return ETipoAgrupador.COMPUESTO_CURSO;
            }
        }

        private String informacion;
        /// <summary>
        /// Archivo de información adicional
        /// </summary>
        public String Informacion
        {
            get
            {
                return this.informacion;
            }
            set
            {
            	this.informacion = value;
            }
        }

    }
}
