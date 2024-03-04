using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Prueba.BO
{
    public abstract class ARespuestaAlumno:ICloneable
    {
        private Int64? respuestaAlumnoID;
        /// <summary>
        /// Identificador de la respuesta alumno
        /// </summary>
        public Int64? RespuestaAlumnoID
        {
            get { return this.respuestaAlumnoID; }
            set { this.respuestaAlumnoID = value; }
        }
        private DateTime? fechaRegistro;
        /// <summary>
        /// Fecha de registro del elemento
        /// </summary>
        public DateTime? FechaRegistro
        {
            get { return this.fechaRegistro; }
            set { this.fechaRegistro = value; }
        }

        private Int32? tiempo;
        /// <summary>
        /// Tiempo(Segundos) en el cual se resolvio la pregunta
        /// </summary>
        public Int32? Tiempo
        {
            get { return this.tiempo; }
            set { this.tiempo = value; }
        }

        private ETipoRespuestaPlantilla? tipoRespuestaPlantilla;

        /// <summary>
        /// Tipo respuesta plantilla
        /// </summary>
        public ETipoRespuestaPlantilla? TipoRespuestaPlantilla
        {
            get { return tipoRespuestaPlantilla; }
            set { tipoRespuestaPlantilla = value; }
        }

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
