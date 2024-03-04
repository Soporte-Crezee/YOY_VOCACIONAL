using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.CentroEducativo.BO;
using POV.Reactivos.BO;

namespace POV.Prueba.BO
{
    public abstract class ARegistroPrueba : ICloneable
    {
        private Int32? registroPruebaID;
        /// <summary>
        /// Id. del objeto 'RegistroPruebaDiagnostico'
        /// </summary>
        public Int32? RegistroPruebaID
        {
            get { return this.registroPruebaID; }
            set { this.registroPruebaID = value; }
        }
        private DateTime? fechaInicio;
        /// <summary>
        /// Fecha en la cual inicio la prueba diagnostico
        /// </summary>
        public DateTime? FechaInicio
        {
            get { return this.fechaInicio; }
            set { this.fechaInicio = value; }
        }
        private DateTime? fechaFin;
        /// <summary>
        /// Fecha en la cual finalizo la prueba diagnostico
        /// </summary>
        public DateTime? FechaFin
        {
            get { return this.fechaFin; }
            set { this.fechaFin = value; }
        }
        private EEstadoPrueba? estadoPrueba;
        /// <summary>
        /// Estado actual de la prueba diagnostico
        /// </summary>
        public EEstadoPrueba? EstadoPrueba
        {
            get { return this.estadoPrueba; }
            set { this.estadoPrueba = value; }
        }
        private Alumno alumno;
        /// <summary>
        /// Objeto 'Alumno' relacionado a la aplicación de esta prueba diagnostico
        /// </summary>
        public Alumno Alumno
        {
            get { return this.alumno; }
            set { this.alumno = value; }
        }

        private List<ARespuestaReactivo> listaRespuestaReactivos;
        /// <summary>
        /// Listado de respuestas de reactivos registradas para la aplicación de prueba diagnostico
        /// </summary>
        public List<ARespuestaReactivo> ListaRespuestaReactivos
        {
            get { return this.listaRespuestaReactivos; }
            set { this.listaRespuestaReactivos = value; }
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

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }

        public virtual object CloneAll()
        {
            ARegistroPrueba registroPruebaClonado = (ARegistroPrueba)this.MemberwiseClone();
            registroPruebaClonado.Alumno = this.alumno;
            registroPruebaClonado.listaRespuestaReactivos = new List<ARespuestaReactivo>();

            foreach (ARespuestaReactivo respuestaReactivo in listaRespuestaReactivos)
                listaRespuestaReactivos.Add((ARespuestaReactivo)respuestaReactivo.Clone());

            return registroPruebaClonado;
        }


        public ARespuestaReactivo GetNextReactivo()
        {
            if (this.ListaRespuestaReactivos != null && this.ListaRespuestaReactivos.Count > 0)
            {
                return (ARespuestaReactivo)this.ListaRespuestaReactivos.OrderBy(item=> item.RespuestaReactivoID).FirstOrDefault(x => x.EstadoReactivo != EEstadoReactivo.CERRADO);
            }

            
            return null;
        }

        public List<ARespuestaReactivo> GetNextReactivos(int reactivosPorPagina)
        {
            if (this.ListaRespuestaReactivos != null && this.ListaRespuestaReactivos.Count > 0)
            {
                return ListaRespuestaReactivos.OrderBy(item => item.RespuestaReactivoID).Where(x => x.EstadoReactivo != EEstadoReactivo.CERRADO).Take(reactivosPorPagina).ToList();
            }


            return null;
        }

        public int GetTotalReactivosContestados()
        {
            if (this.listaRespuestaReactivos != null && this.listaRespuestaReactivos.Count > 0)
            {
                return this.listaRespuestaReactivos.OrderBy(item => item.RespuestaReactivoID).Where(x => x.EstadoReactivo == EEstadoReactivo.CERRADO).ToList().Count;
            }
            return 0;
        }

        public int GetTotalReactivosNoContestados()
        {
            if (this.listaRespuestaReactivos != null && this.listaRespuestaReactivos.Count > 0)
            {
                return this.listaRespuestaReactivos.OrderBy(item => item.RespuestaReactivoID).Where(x => x.EstadoReactivo != EEstadoReactivo.CERRADO).ToList().Count;
            }
            return 0;
        }

        public void UpdateEstadoPrueba()
        {
            if (this.ListaRespuestaReactivos != null )
            {
                int count = this.ListaRespuestaReactivos.Count;
                int countComplete = this.ListaRespuestaReactivos.Count(x => x.EstadoReactivo == EEstadoReactivo.CERRADO);

                if (count == countComplete)
                    this.EstadoPrueba = EEstadoPrueba.CERRADA;
                if (countComplete >= 1 && count > countComplete)
                    this.EstadoPrueba = EEstadoPrueba.ENCURSO;
                else
                    this.EstadoPrueba = EEstadoPrueba.NOINICIADA;
            }
        }

        public EEstadoPrueba GetEstadoPruebaActual()
        {
            if (this.ListaRespuestaReactivos != null)
            {

                int count = this.ListaRespuestaReactivos.Count;
                int countComplete = this.ListaRespuestaReactivos.Count(x => x.EstadoReactivo == EEstadoReactivo.CERRADO);

                if (count == countComplete)
                    return EEstadoPrueba.CERRADA;

                if (countComplete >= 1 && count > countComplete)
                    return EEstadoPrueba.ENCURSO;
            }

            return EEstadoPrueba.NOINICIADA;
        }
    }
}
