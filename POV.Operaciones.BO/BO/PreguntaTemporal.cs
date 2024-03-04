using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Reactivos.BO;

namespace POV.Operaciones.BO
{
    public class PreguntaTemporal
    {
        public Pregunta Pregunta { get; set; }

        public int Orden { get; set; }

        public RecursoImagenPregunta ImagenEnunciado { get; set; }

        public RecursoImagenPregunta ImagenOpcion1 { get; set; }

        public RecursoImagenPregunta ImagenOpcion2 { get; set; }

        public RecursoImagenPregunta ImagenOpcion3 { get; set; }

        public RecursoImagenPregunta ImagenOpcion4 { get; set; }

        public RecursoImagenPregunta ImagenOpcion5 { get; set; }

        public RecursoImagenPregunta ImagenOpcion6 { get; set; }

        /// <summary>
        /// Obtiene una nueva instancia de pregunta de opcion multiple
        /// </summary>
        /// <returns></returns>
        public Pregunta GetNewPregunta()
        {
            Pregunta pregunta = new Pregunta();
            pregunta.RespuestaPlantilla = new RespuestaPlantillaOpcionMultiple();
            pregunta.RespuestaPlantilla.TipoRespuestaPlantilla = ETipoRespuestaPlantilla.OPCION_MULTIPLE;
            pregunta.Activo = true;
            pregunta.PuedeOmitir = true;
            pregunta.SoloImagen = false;

            (pregunta.RespuestaPlantilla as RespuestaPlantillaOpcionMultiple).ModoSeleccion = EModoSeleccion.UNICA;
            (pregunta.RespuestaPlantilla as RespuestaPlantillaOpcionMultiple).NumeroSeleccionablesMaximo = 1;
            (pregunta.RespuestaPlantilla as RespuestaPlantillaOpcionMultiple).NumeroSeleccionablesMinimo = 1;
            (pregunta.RespuestaPlantilla as RespuestaPlantillaOpcionMultiple).Estatus = true;
            (pregunta.RespuestaPlantilla as RespuestaPlantillaOpcionMultiple).ListaOpcionRespuestaPlantilla = new List<OpcionRespuestaPlantilla>();
            (pregunta.RespuestaPlantilla as RespuestaPlantillaOpcionMultiple).FechaRegistro = DateTime.Now;

            (pregunta.RespuestaPlantilla as RespuestaPlantillaOpcionMultiple).ListaOpcionRespuestaPlantilla.Add(new OpcionRespuestaPlantilla());
            (pregunta.RespuestaPlantilla as RespuestaPlantillaOpcionMultiple).ListaOpcionRespuestaPlantilla.Add(new OpcionRespuestaPlantilla());
            (pregunta.RespuestaPlantilla as RespuestaPlantillaOpcionMultiple).ListaOpcionRespuestaPlantilla.Add(new OpcionRespuestaPlantilla());

            return pregunta;
        }

    }
}
