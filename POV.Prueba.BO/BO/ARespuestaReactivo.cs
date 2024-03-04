using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Reactivos.BO;

namespace POV.Prueba.BO
{
    public abstract class ARespuestaReactivo:ICloneable
    {
        private Int64? respuestaReactivoID;
        /// <summary>
        /// Id. del objeto 'RespuestaReactivoAlumno'
        /// </summary>
        public Int64? RespuestaReactivoID
        {
            get { return this.respuestaReactivoID; }
            set { this.respuestaReactivoID = value; }
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
        private Reactivo reactivo;
        /// <summary>
        /// Objeto 'Reactivo' relacionado con el registro
        /// </summary>
        public Reactivo Reactivo
        {
            get { return this.reactivo; }
            set { this.reactivo = value; }
        }
        private List<ARespuestaPregunta> listaRespuestaPreguntas;
        /// <summary>
        /// Listado de preguntas relacionadas con el reactivo registrado.
        /// </summary>
        public List<ARespuestaPregunta> ListaRespuestaPreguntas
        {
            get { return this.listaRespuestaPreguntas; }
            set { this.listaRespuestaPreguntas = value; }
        }
        private EEstadoReactivo? estadoReactivo;
        /// <summary>
        /// Estado actual del reactivo
        /// </summary>
        public EEstadoReactivo? EstadoReactivo
        {
            get { return this.estadoReactivo; }
            set { this.estadoReactivo = value; }
        }

        private int? tiempo;
        /// <summary>
        /// Tiempo
        /// </summary>
        public int? Tiempo
        {
            get { return this.tiempo; }
            set { this.tiempo = value; }
        }

        public virtual object Clone()
        {
            ARespuestaReactivo nuevo = (ARespuestaReactivo) this.MemberwiseClone();

            if(this.Reactivo!=null)
                nuevo.Reactivo = (Reactivo)this.Reactivo.Clone(); 
           
            if(this.ListaRespuestaPreguntas!=null)
            {
                nuevo.ListaRespuestaPreguntas= new List<ARespuestaPregunta>();

                foreach (ARespuestaPregunta aPregunta in ListaRespuestaPreguntas)
                {
                    nuevo.ListaRespuestaPreguntas.Add((ARespuestaPregunta)aPregunta.Clone());
                }
            }
            return nuevo;
        }
    }
}
