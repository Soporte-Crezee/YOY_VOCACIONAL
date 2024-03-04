using System;
using System.Collections.Generic;
using POV.Expediente.BO;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Prueba.BO;
using System.Linq;
using POV.Modelo.BO;

namespace POV.Prueba.Calificaciones.BO
{
    /// <summary>
    /// Consulta de AResultadoMetodoCalificacion 
    /// </summary>
    public abstract class AResultadoMetodoCalificacion
    {
        private int? resultadoMetodoCalificacionID;
        /// <summary>
        /// Identificador unico
        /// </summary>
        public int? ResultadoMetodoCalificacionID
        {
            get { return this.resultadoMetodoCalificacionID; }
            set { this.resultadoMetodoCalificacionID = value; }
        }
        private DateTime? fechaRegistro;
        /// <summary>
        /// Fecha Alta
        /// </summary>
        public DateTime? FechaRegistro
        {
            get { return this.fechaRegistro; }
            set { this.fechaRegistro = value; }
        }
        private AResultadoPrueba resultadoPrueba;
        /// <summary>
        /// ResultadoPrueba
        /// </summary>
        public AResultadoPrueba ResultadoPrueba
        {
            get { return this.resultadoPrueba; }
            set { this.resultadoPrueba = value; }
        }
        /// <summary>
        /// Tipo de resultado del método de calificacion.
        /// </summary>
        public abstract ETipoResultadoMetodo TipoResultadoMetodo { get; }
        /// <summary>
        /// Encuentra la escala en la que cae un determinado valor
        /// </summary>
        /// <param name="prueba">La prueba que contiene el método de calificacion, y las escalas</param>
        /// <param name="elemento">el valor a ser evaluado</param>
        /// <returns>el resultado de la calificacion con sus escalas</returns>
       
        public AResultadoMetodoCalificacion EncontrarEscalaFromValor(APrueba prueba, decimal elemento,int?clasificadorID)
        {
            List<AEscalaDinamica> escalasGenericas = new List<AEscalaDinamica>();
            List<APuntaje> puntajes = prueba.ListaPuntajes.ToList<APuntaje>();
            
            foreach (APuntaje puntaje in puntajes)
            {

                AEscalaDinamica escalaGenerico = puntaje as AEscalaDinamica;
                if (puntaje != null)
                {
                    if (escalaGenerico.Clasificador == null)
                        return null;
                    if (escalaGenerico.Clasificador.ClasificadorID == null)
                        return null;
                    
                    if(escalaGenerico.Clasificador.ClasificadorID==clasificadorID){
                        escalasGenericas.Add(escalaGenerico);
                    }
                }
                
            }
            AEscalaDinamica currentEscala = null;
            AEscalaDinamica nextEscala = null;


            if (escalasGenericas == null)
                return null;

            if (escalasGenericas.Count <= 0)
                return null;
            int length = escalasGenericas.Count;


            escalasGenericas = escalasGenericas.OrderBy(x => x.PuntajeMinimo).ToList<AEscalaDinamica>();

            AEscalaDinamica escalaGenericaMaxima = escalasGenericas[length - 1];
            AEscalaDinamica escalaGenericaMinima = escalasGenericas[0];
            if (escalaGenericaMaxima.PuntajeMaximo == null)
                return null;
            if (escalaGenericaMinima.PuntajeMinimo == null)
                return null;
         
            if (elemento < escalaGenericaMinima.PuntajeMinimo)
                return ObtenerResultadoMetodoCalificacionByEscalaDinamica(escalaGenericaMinima, true, elemento);
            if (elemento > escalaGenericaMaxima.PuntajeMaximo)
                return ObtenerResultadoMetodoCalificacionByEscalaDinamica(escalaGenericaMaxima, true, elemento);

            for (int i = 0; i < length; i++)
            {

                currentEscala = escalasGenericas[i];
                if (i < length - 1)
                {
                    nextEscala = escalasGenericas[i + 1];
                }
                else {
                    ///Si es la última escala, las escalas se consideran cerradas.
                    nextEscala = null;
                    if (elemento >= currentEscala.PuntajeMinimo && elemento <= currentEscala.PuntajeMaximo)
                    {
                        return ObtenerResultadoMetodoCalificacionByEscalaDinamica(currentEscala, false, elemento);
                    }
                }

                if (elemento >= currentEscala.PuntajeMinimo && elemento < currentEscala.PuntajeMaximo)
                {
                    return ObtenerResultadoMetodoCalificacionByEscalaDinamica(currentEscala, false, elemento);
                }

                if(nextEscala!=null){
                if (currentEscala.PuntajeMaximo != nextEscala.PuntajeMinimo)
                {
                    if (elemento >= currentEscala.PuntajeMaximo && elemento < nextEscala.PuntajeMinimo)
                    {
                        return ObtenerResultadoMetodoCalificacionByEscalaDinamica(nextEscala, true,elemento);
                    }
                   }
                }
            }
           return null;
       }
        private AResultadoMetodoCalificacion ObtenerResultadoMetodoCalificacionByEscalaDinamica(AEscalaDinamica escala, bool esAproximado,decimal? valor) { 
           AResultadoMetodoCalificacion resultado=null;
           if (escala.TipoEscalaDinamica == null)
               return null;
            switch(escala.TipoEscalaDinamica){
            
                case ETipoEscalaDinamica.SELECCION:
                
                     resultado = new ResultadoMetodoSeleccion();
                     List<DetalleResultadoSeleccion> lista = new List<DetalleResultadoSeleccion>();
                     lista.Add(new DetalleResultadoSeleccion(){EscalaSeleccionDinamica= escala as EscalaSeleccionDinamica,EsAproximado= esAproximado,Valor=valor});
                     (resultado as ResultadoMetodoSeleccion).ListaDetalleResultadoSeleccion =lista;
                     break;
                case ETipoEscalaDinamica.CLASIFICACION:
                    resultado = new ResultadoMetodoClasificacion();
                    List<DetalleResultadoClasificacion> list = new List<DetalleResultadoClasificacion>();
                    list.Add(new DetalleResultadoClasificacion(){EscalaClasificacionDinamica = escala as EscalaClasificacionDinamica,EsAproximado = esAproximado,Valor = valor});
                    (resultado as ResultadoMetodoClasificacion).ListaDetalleResultadoClasificacion = list;
                    break;
            }
            return resultado;
        }


        /// <summary>
        /// Obtiene el clasificador predominante
        /// </summary>
        /// <returns>Clasificador predominante, si el valor es null, entonces no está bien configurado los reactivos</returns>
        public abstract Clasificador ObtenerClasificadorPredominante();
    }
}
