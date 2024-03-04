using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Modelo.BO;

namespace POV.Prueba.Calificaciones.BO
{
    /// <summary>
    /// ResultadoMetodoClasificacion
    /// </summary>
    public class ResultadoMetodoClasificacion: AResultadoMetodoCalificacion
    {
        /// <summary>
        /// Lista de detalle del resultado de clasificación.
        /// </summary>
        private List<DetalleResultadoClasificacion> listaDetalleResultadoClasificacion;
        /// <summary>
        /// ListaDetalleResultadoClasificacion
        /// </summary>
        public List<DetalleResultadoClasificacion> ListaDetalleResultadoClasificacion
        {
            get { return this.listaDetalleResultadoClasificacion; }
            set { this.listaDetalleResultadoClasificacion = value; }
        }
        /// <summary>
        /// Tipo de resultado de la prueba.
        /// </summary>
        public override ETipoResultadoMetodo TipoResultadoMetodo
        {
            get { return ETipoResultadoMetodo.CLASIFICACION; }
        }
        /// <summary>
        /// Calcula el resultado de calificación predominante en la prueba.
        /// </summary>
        /// <returns></returns>
        public List<DetalleResultadoClasificacion> CalcularResultadosCalificacionPredominante()
        {
            List<DetalleResultadoClasificacion> listadetalle = this.listaDetalleResultadoClasificacion;
            if (listadetalle == null)
                return listadetalle;
            List<DetalleResultadoClasificacion> listafir = null;
            List<DetalleResultadoClasificacion> listaf = new List<DetalleResultadoClasificacion>();
            listafir = listaDetalleResultadoClasificacion.Where(z => z.EscalaClasificacionDinamica.EsPredominante == true).ToList<DetalleResultadoClasificacion>();
            if (listafir != null)
            if (listafir.Count <= 0)
            {
                listafir = listadetalle.OrderByDescending(item => item.Valor).ToList<DetalleResultadoClasificacion>();
                decimal mayor = 0;
                bool primero = true;
                foreach (DetalleResultadoClasificacion detalle in listafir)
                {
                    if (primero)
                    {
                        mayor = detalle.Valor.Value;
                        listaf.Add(detalle);
                    }
                    else
                    {
                        if (detalle.Valor.Value == mayor)
                        {
                            listaf.Add(detalle);
                        }
                        else
                        {
                            break;
                        }
                    }
                    primero = false;
                }
                return listaf;
            }
            
            return listafir;
        }
        /// <summary>
        /// Obtiene el clasificador indicado para la selección del paquete de juegos.
        /// </summary>
        /// <returns>Clasificador</returns>
        public override Modelo.BO.Clasificador ObtenerClasificadorPredominante()
        {
            DetalleResultadoClasificacion detalle = null;
            var ordenAleatorio = new Random(new Object().GetHashCode());
            List<DetalleResultadoClasificacion> list = this.CalcularResultadosCalificacionPredominante();
            if (list == null)
                return null;
            list = list.OrderBy(r => ordenAleatorio.Next()).ToList();
            if (list != null)
            {
                if (list.Count > 0)
                {
                    detalle = list.FirstOrDefault();
                    return detalle.EscalaClasificacionDinamica.Clasificador;
                }
            }
            return null;
        }
    }
}
