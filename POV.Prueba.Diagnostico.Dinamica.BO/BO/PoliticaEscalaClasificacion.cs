using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Prueba.BO;

namespace POV.Prueba.Diagnostico.Dinamica.BO
{
    /// <summary>
    /// Controlador de la política de calificación.
    /// </summary>
    public class PoliticaEscalaClasificacion : APoliticaEscalaDinamica
    {
        /// <summary>
        /// Tiene las validaciones necesarias de la política de calificación por clasificación. 
        /// </summary>
        /// <param name="pruebadinamica">Prueba dinámica proporcionada.</param>
        /// <param name="nuevo">Nueva escala dinámica.</param>
        /// <returns>Regresa true si pasaron las validaciones o la excepción indicada.</returns>
        public override bool Validar(PruebaDinamica pruebadinamica, AEscalaDinamica nuevo)
        {
            if (pruebadinamica == null || pruebadinamica.ListaPuntajes == null)
                throw new Exception("PruebaDinamica:No puede ser nulo");

            if (nuevo == null)
                throw new Exception("AEscalaDinamica:No puede ser nulo");

            if (nuevo.PuntajeMinimo >= nuevo.PuntajeMaximo)
                throw new Exception(String.Format(
                    "Rango inicial no puede ser mayor o igual al rango final:\n Rango {0} - {1}",
                    nuevo.PuntajeMinimo.ToString(), nuevo.PuntajeMaximo.ToString()));
            if (nuevo.PuntajeMaximo <= 0)
                throw new Exception(String.Format(
                   "Rango final no puede ser menor o igual a cero:\n Clasificador {0}, Rango {1} - {2}", nuevo.Clasificador.Nombre,
                   nuevo.PuntajeMinimo.ToString(), nuevo.PuntajeMaximo.ToString()));
            if(nuevo.PuntajeMinimo == nuevo.PuntajeMaximo)
                throw new Exception(string.Format("El rango inicial no debe ser el mismo que el rango final.\n Rango {0} - {1}", nuevo.PuntajeMinimo.ToString(), nuevo.PuntajeMaximo.ToString()));
            if (!(nuevo is EscalaClasificacionDinamica))
            {
                throw new Exception("Debe ser de tipo EscalaClasificacionDinamica");
            }
            
            if (!pruebadinamica.ListaPuntajes.Any())
            {
                //Si no hay ninguna escala aún en la lista, la primera a insertar debe tener como puntaje mínimo 0.
                if (nuevo.PuntajeMinimo == 0)
                {
                    return true;
                }
                else
                    throw new Exception("El primer rango de la prueba, debe tener como puntuación inicial '0' (cero).");
            }

            List<APuntaje> puntajes = pruebadinamica.ListaPuntajes.ToList().Where(item => (item as AEscalaDinamica).Clasificador.ClasificadorID == nuevo.Clasificador.ClasificadorID && item.Activo.Value).ToList();
            //Se ordena la ListaEscalaDinamica por PuntajeMinimo
            puntajes = puntajes.OrderBy(item => item.PuntajeMinimo.Value).ToList();

            bool existePredominante = puntajes.Any(item => item.EsPredominante.Value);

            if (existePredominante && nuevo.EsPredominante.Value)
            {
                    throw new Exception(String.Format(
                        "Error. Solo se permite un rango predominante"));
            }

            if (puntajes.Count > 0)
            {
                //analisis de traslapes
                puntajes = puntajes.OrderBy(item => item.PuntajeMinimo).ToList();

                APuntaje puntajeCandidatoMinimo = puntajes.FirstOrDefault(item => item.PuntajeMinimo <= nuevo.PuntajeMinimo && nuevo.PuntajeMinimo < item.PuntajeMaximo);
                APuntaje puntajeCandidatoMaximo = puntajes.FirstOrDefault(item => item.PuntajeMinimo <= nuevo.PuntajeMaximo && nuevo.PuntajeMaximo < item.PuntajeMaximo);

                if (puntajeCandidatoMinimo != null && puntajeCandidatoMinimo.PuntajeMaximo > nuevo.PuntajeMinimo) //corte
                {
                    throw new Exception(String.Format(
                    "Ocurrió un error con el rango proporcionado. Los siguientes rangos se traslapan: \n Clasificador: {0}, Rango {1} - {2}", nuevo.Clasificador.Nombre,
                    nuevo.PuntajeMinimo.ToString(), nuevo.PuntajeMaximo.ToString()) + String.Format("\n Clasificador: {0}, Rango {1} - {2}",nuevo.Clasificador.Nombre,puntajeCandidatoMinimo.PuntajeMinimo.ToString(),puntajeCandidatoMinimo.PuntajeMaximo.ToString()));
                }

                if (puntajeCandidatoMaximo != null && nuevo.PuntajeMaximo > puntajeCandidatoMaximo.PuntajeMinimo) //corte
                {
                    throw new Exception(String.Format(
                    "Ocurrió un error con el rango proporcionado. Los siguientes rangos se traslapan: \n Clasificador: {0}, Rango {1} - {2}", nuevo.Clasificador.Nombre,
                    nuevo.PuntajeMinimo.ToString(), nuevo.PuntajeMaximo.ToString()) + String.Format("\n Clasificador: {0}, Rango {1} - {2}", nuevo.Clasificador.Nombre, puntajeCandidatoMaximo.PuntajeMinimo.ToString(), puntajeCandidatoMaximo.PuntajeMaximo.ToString()));
                }

                APuntaje puntajeSiguiente = puntajes.FirstOrDefault(item => item.PuntajeMinimo > nuevo.PuntajeMinimo);
                if (puntajeSiguiente != null)
                {
                    if (puntajeSiguiente.PuntajeMinimo < nuevo.PuntajeMaximo)
                    {
                        throw new Exception(String.Format(
                       "Rango final no puede traslaparse con otros rangos: \n Clasificador {0}, Rango {1} - {2}", nuevo.Clasificador.Nombre,
                       nuevo.PuntajeMinimo.ToString(), nuevo.PuntajeMaximo.ToString()));
                    }
                }

                APuntaje puntajeAnterior = puntajes.LastOrDefault(item => item.PuntajeMaximo <= nuevo.PuntajeMinimo);
                if (puntajeAnterior != null)
                {
                    if (puntajeAnterior.PuntajeMaximo != nuevo.PuntajeMinimo)
                    {
                        throw new Exception(String.Format(
                       "Rango inicial debe iniciar en el rango final del anterior rango correspondiente: \n Clasificador {0}, Rango {1} - {2}", nuevo.Clasificador.Nombre,
                       nuevo.PuntajeMinimo.ToString(), nuevo.PuntajeMaximo.ToString()));
                    }
                }
            } else if (nuevo.PuntajeMinimo != 0)
            {
                throw new Exception(String.Format(
                   "El primer rango debe iniciar en cero: \n Clasificador {0}, Rango {1} - {2}", nuevo.Clasificador.Nombre,
                   nuevo.PuntajeMinimo.ToString(), nuevo.PuntajeMaximo.ToString()));
            }

            return true;
        }
    }
}
