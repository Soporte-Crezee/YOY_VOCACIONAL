using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Modelo.BO;
using POV.Prueba.BO;
using POV.Prueba.Diagnostico.Dinamica.BO;

namespace POV.Prueba.Calificaciones.BO
{
    /// <summary>
    /// Califica a una prueba Dinámica.
    /// </summary>
    public class MetodoClasificacion: IPoliticaCalificacion
    {
        /// <summary>
        /// Calcula la calificación de una prueba Dinámica.
        /// </summary>
        /// <param name="resultadoPrueba">Resultado de la prueba asignada.</param>
        /// <param name="prueba">Prueba relacionada a ese resultado.</param>
        /// <returns>regresa el Resultado del método de calificación. Si regresa null es porque no está bien configurado el método de calificación.</returns>
        public AResultadoMetodoCalificacion Calcular(IResultadoPrueba resultadoPrueba, APrueba prueba)
        {
            ResultadoMetodoClasificacion resultado = new ResultadoMetodoClasificacion();
            resultado.ListaDetalleResultadoClasificacion = new List<DetalleResultadoClasificacion>();
            if (prueba == null)
                return null;
            if (prueba.ListaPuntajes == null)
                return null;
            if (!prueba.ListaPuntajes.Any())
                return null;
            IDictionary<int, decimal> result = null;
            IEnumerable<APuntaje> listpuntaje = prueba.ListaPuntajes;
            try
            {
                result = this.CalcularResultadoMetodoClasificacion(resultadoPrueba, prueba);
            }
            catch (Exception ex)
            {
                return null;
            }
            if (result == null)
                return null;
            foreach (KeyValuePair<int, decimal> elemento in result)
            {
                try
                {
                     DetalleResultadoClasificacion clasificacion = ObtenerDetalleResultadoClasificacion(elemento, prueba,
                                                                                                               resultadoPrueba);   
                    if (clasificacion == null)
                        return null;
                    resultado.ListaDetalleResultadoClasificacion.Add(clasificacion);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            if (listpuntaje!=null)
            {
                Dictionary<int,decimal> pnoconfig = ObtenerClasificadores(listpuntaje, resultado);

                foreach (KeyValuePair<int, decimal> elemento in pnoconfig)
                {
                    try
                    {
                        DetalleResultadoClasificacion det = ObtenerDetalleResultadoClasificacion(elemento, prueba,
                                                                                                 resultadoPrueba);
                        if (det==null)
                        {
                            return null;
                        }
                        resultado.ListaDetalleResultadoClasificacion.Add(det);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            resultado.ResultadoPrueba = resultadoPrueba as ResultadoPruebaDinamica;
            return resultado;
        }
        private Dictionary<int, decimal> ObtenerClasificadores(IEnumerable<APuntaje> puntajesConfigurados, ResultadoMetodoClasificacion resultado)
        {
            Dictionary<int, decimal> puntajesNoConfiguradosValor = new Dictionary<int, decimal>();
            foreach (APuntaje puntaje in puntajesConfigurados)
            {
                AEscalaDinamica escalaDinamica = puntaje as AEscalaDinamica;
                List<DetalleResultadoClasificacion> resultadoClasificador = null;
                if (escalaDinamica != null)
                {
                    resultadoClasificador = resultado.ListaDetalleResultadoClasificacion.Where(x => x.EscalaClasificacionDinamica.Clasificador.ClasificadorID == escalaDinamica.Clasificador.ClasificadorID).ToList<DetalleResultadoClasificacion>();

                    if (resultadoClasificador.Count <= 0)
                    {
                        if (!puntajesNoConfiguradosValor.ContainsKey(escalaDinamica.Clasificador.ClasificadorID.Value))
                            puntajesNoConfiguradosValor.Add(escalaDinamica.Clasificador.ClasificadorID.Value, 0);
                    }
                }
            }
            return puntajesNoConfiguradosValor;
        }

        private DetalleResultadoClasificacion ObtenerDetalleResultadoClasificacion(KeyValuePair<int, decimal> element,
                                                                                   APrueba prueba,
                                                                                   IResultadoPrueba resultadoPrueba)
        {
            AResultadoMetodoCalificacion resultadoMetodo = new ResultadoMetodoClasificacion();
            ResultadoMetodoClasificacion resultado =
                resultadoMetodo.EncontrarEscalaFromValor(prueba, element.Value, element.Key) as
                ResultadoMetodoClasificacion;
            if (resultado == null)
                return null;
            if (resultado.ListaDetalleResultadoClasificacion == null)
                return null;
            if (resultado.ListaDetalleResultadoClasificacion.Count <= 0)
                return null;

            return resultado.ListaDetalleResultadoClasificacion[0];
        }
        /// <summary>
        /// Calcula el resultado de una prueba Dinámica.
        /// </summary>
        /// <param name="resultadoPrueba">Resultado de la prueba asignada</param>
        /// <param name="prueba">Prueba relacionada al resultado proporcionado</param>
        /// <returns>Regresa el resultado del método de calificación.</returns>
        public IDictionary<int, decimal> CalcularResultadoMetodoClasificacion(IResultadoPrueba resultadoPrueba,
                                                                              APrueba prueba)
        {
            if (resultadoPrueba == null || (resultadoPrueba as ResultadoPruebaDinamica) == null) throw new ArgumentException("MetodoClasificacion:el resultado prueba nulo o diferente a PruebaDinamica", "resultadoPrueba");
            if (prueba == null || (prueba as PruebaDinamica) == null) throw new ArgumentException("Metodo por seleccción de Clasificacion:prueba nulo o diferente a PruebaDinamica", "prueba");
            ARegistroPrueba registroPrueba = resultadoPrueba.RegistroPrueba;
            if(registroPrueba == null)
                throw new Exception("No existe un registro prueba");

            IDictionary<int, decimal> result = null;
            if (registroPrueba is RegistroPruebaDinamica)
            {
                RegistroPruebaDinamica registroPruebaDinamica = registroPrueba as RegistroPruebaDinamica;

                if (registroPrueba.GetEstadoPruebaActual() != EEstadoPrueba.CERRADA)
                    throw new Exception("MetodoClasificacion, la prueba no ha sido cerrada.");
                try
                {
                    result = registroPruebaDinamica.ResultadoMetodoCalificacionClasificacion();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                throw new Exception("La prueba, no es el tipo que se espera.");
            }

            return result;
        }
    }
}
