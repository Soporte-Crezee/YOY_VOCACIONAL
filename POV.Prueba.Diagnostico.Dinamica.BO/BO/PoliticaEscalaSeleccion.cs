using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Prueba.BO;
using POV.Modelo.BO;

namespace POV.Prueba.Diagnostico.Dinamica.BO
{
    public class PoliticaEscalaSeleccion : APoliticaEscalaDinamica
    {
        public override bool Validar(PruebaDinamica pruebadinamica, AEscalaDinamica nuevo)
        {
            if (pruebadinamica == null || pruebadinamica.ListaPuntajes == null)
                throw new Exception("PruebaDinamica:No puede ser nulo");

            if (nuevo == null)
                throw new Exception("AEscalaDinamica:No puede ser nulo");

            if (nuevo.PuntajeMinimo >= nuevo.PuntajeMaximo)
                throw new Exception(String.Format(
                    "Error. Respuestas inicial no puede ser mayor o igual a Respuestas final:\n Clasificador {0}, Rango {1} - {2}", nuevo.Clasificador.Nombre,
                    nuevo.PuntajeMinimo.ToString(), nuevo.PuntajeMaximo.ToString()));
            if (nuevo.PuntajeMaximo <= 0)
                throw new Exception(String.Format(
                   "Error. Respuestas final no puede ser menor o igual a cero:\n Clasificador {0}, Rango {1} - {2}", nuevo.Clasificador.Nombre,
                   nuevo.PuntajeMinimo.ToString(), nuevo.PuntajeMaximo.ToString()));
            if (nuevo.PuntajeMaximo < 0)
                throw new Exception(String.Format(
                   "Error. Respuestas inicial no puede ser menor a cero:\n Clasificador {0}, Rango {1} - {2}", nuevo.Clasificador.Nombre,
                   nuevo.PuntajeMinimo.ToString(), nuevo.PuntajeMaximo.ToString()));

            if (!(nuevo is EscalaSeleccionDinamica))
            {
                throw new Exception("AEscalaDinamica: Debe ser de tipo EscalaSeleccionDinamica");
            }

            //puntajes para el clasificador de la escala nueva
            List<APuntaje> puntajes = pruebadinamica.ListaPuntajes.ToList().Where(item => (item as AEscalaDinamica).Clasificador.ClasificadorID == nuevo.Clasificador.ClasificadorID && item.Activo.Value).ToList();
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
                    "Error. Respuestas inicial no puede traslaparse con otros rangos: \n Clasificador {0}, Rango {1} - {2}", nuevo.Clasificador.Nombre,
                    nuevo.PuntajeMinimo.ToString(), nuevo.PuntajeMaximo.ToString()));
                }

                if (puntajeCandidatoMaximo != null && nuevo.PuntajeMaximo > puntajeCandidatoMaximo.PuntajeMinimo) //corte
                {
                    throw new Exception(String.Format(
                    "Error. Respuestas inicial no puede traslaparse con otros rangos: \n Clasificador {0}, Rango {1} - {2}", nuevo.Clasificador.Nombre,
                    nuevo.PuntajeMinimo.ToString(), nuevo.PuntajeMaximo.ToString()));
                }



                APuntaje puntajeSiguiente = puntajes.FirstOrDefault(item => item.PuntajeMinimo > nuevo.PuntajeMinimo);
                if (puntajeSiguiente != null)
                {
                    if (puntajeSiguiente.PuntajeMinimo < nuevo.PuntajeMaximo)
                    {
                        throw new Exception(String.Format(
                       "Error. Respuestas final no puede traslaparse con otros rangos: \n Clasificador {0}, Rango {1} - {2}", nuevo.Clasificador.Nombre,
                       nuevo.PuntajeMinimo.ToString(), nuevo.PuntajeMaximo.ToString()));
                    }

                    

                }

                APuntaje puntajeAnterior = puntajes.LastOrDefault(item => item.PuntajeMaximo <= nuevo.PuntajeMinimo);
                if (puntajeAnterior != null)
                {
                    if (puntajeAnterior.PuntajeMaximo != nuevo.PuntajeMinimo)
                    {
                        throw new Exception(String.Format(
                       "Error. Respuestas inicial debe iniciar en la Respuestas final del anterior rango correspondiente: \n Clasificador {0}, Rango {1} - {2}", nuevo.Clasificador.Nombre,
                       nuevo.PuntajeMinimo.ToString(), nuevo.PuntajeMaximo.ToString()));
                    }
                }
                //analisis de consecutivos
            }
            else if (nuevo.PuntajeMinimo != 0)
            {
                throw new Exception(String.Format(
                   "Error: El primer rango debe iniciar sus respuestas inicial en cero: \n Clasificador {0}, Rango {1} - {2}", nuevo.Clasificador.Nombre,
                   nuevo.PuntajeMinimo.ToString(), nuevo.PuntajeMaximo.ToString()));
            }


            

            return true;
        }
    }
}
