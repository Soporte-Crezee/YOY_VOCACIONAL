using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Reactivos.BO;

namespace POV.Prueba.BO
{
    public abstract class ARespuestaPregunta:ICloneable
    {
        private Int64? respuestaPreguntaID;
        /// <summary>
        /// Id. del objeto 'RespuestaPreguntaAlumno'
        /// </summary>
        public Int64? RespuestaPreguntaID
        {
            get { return this.respuestaPreguntaID; }
            set { this.respuestaPreguntaID = value; }
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
        private Pregunta pregunta;
        /// <summary>
        /// Objeto 'Pregunta' asociado con el reactivo y respuesta
        /// </summary>
        public Pregunta Pregunta
        {
            get { return this.pregunta; }
            set { this.pregunta = value; }
        }
        private EEstadoRespuesta? estadoRespuesta;
        /// <summary>
        /// Estado actual de la pregunta
        /// </summary>
        public EEstadoRespuesta? EstadoRespuesta
        {
            get { return this.estadoRespuesta; }
            set { this.estadoRespuesta = value; }
        }

        private ARespuestaAlumno respuestaAlumno;

        public ARespuestaAlumno RespuestaAlumno
        {
            get { return this.respuestaAlumno; }
            set { this.respuestaAlumno = value; }
        }

        public virtual object Clone()
        {
            ARespuestaPregunta nuevo = (ARespuestaPregunta)this.MemberwiseClone();
            if(this.Pregunta!=null)
                nuevo.Pregunta = (Pregunta)this.Pregunta.Clone();

            if(this.RespuestaAlumno!=null)
            nuevo.RespuestaAlumno = (ARespuestaAlumno) this.RespuestaAlumno.Clone();
            return nuevo;
        }
    }
}
