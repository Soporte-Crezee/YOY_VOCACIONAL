using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Modelo.BO;

namespace POV.Prueba.Calificaciones.BO
{
    /// <summary>
    /// ResultadoMetodoPuntos
    /// </summary>
   public class ResultadoMetodoPuntos : AResultadoMetodoCalificacion
    {
       private decimal? puntos;
       /// <summary>
       /// Puntos
       /// </summary>
       public decimal? Puntos
       {
           get { return this.puntos; }
           set { this.puntos = value; }
       }
       private decimal? maximoPuntos;
       /// <summary>
       /// MaximoPuntos
       /// </summary>
       public decimal? MaximoPuntos
       {
           get { return this.maximoPuntos; }
           set { this.maximoPuntos = value; }
       }
       private bool? esAproximado;
       /// <summary>
       /// EsAproximado
       /// </summary>
       public bool? EsAproximado
       {
           get { return this.esAproximado; }
           set { this.esAproximado = value; }
       }
       private EscalaPuntajeDinamica escalaPuntajeDinamica;
       /// <summary>
       /// Escala Puntaje Dinamica
       /// </summary>
       public EscalaPuntajeDinamica EscalaPuntajeDinamica
       {
           get { return this.escalaPuntajeDinamica; }
           set { this.escalaPuntajeDinamica = value; }
       }
       public override ETipoResultadoMetodo TipoResultadoMetodo
       {
           get { return ETipoResultadoMetodo.PUNTOS; }
       }

       /// <summary>
       /// Obtiene el Clasificador de Seleccion del Metodo de Calificacion
       /// </summary>
       public override Modelo.BO.Clasificador ObtenerClasificadorPredominante()
       {
           return this.escalaPuntajeDinamica.Clasificador;
       }
    }
}
