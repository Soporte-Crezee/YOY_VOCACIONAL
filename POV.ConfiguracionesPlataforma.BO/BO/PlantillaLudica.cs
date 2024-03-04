using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POV.ConfiguracionesPlataforma.BO
{
    public class PlantillaLudica
    {
        //Identificador de la Plantilla
        public int? PlantillaLudicaId { get; set; }

        //Nombre de la Plantilla
        public string Nombre { get; set; }

        //Imagen Fondo de Plantilla
        public string ImagenFondo { get; set; }

        //Imagen de Actividad Pendiente
        public string ImagenPendiente { get; set; }

        //Imagen de Actividad Iniciada
        public string ImagenIniciado { get; set; }

        //Imagen de Actividad Finalizado
        public string ImagenFinalizado { get; set; }

        //Imagen de Actividad No Disponible
        public string ImagenNoDisponible { get; set; }

        //Imagen de Flecha Arriba
        public string ImagenFlechaArriba { get; set; }

        //Imagen de Flecha Abajo
        public string ImagenFlechaAbajo { get; set; }

        //La plantilla es predeterminada?
        public bool? EsPredeterminado { get; set; }

        //Fecha de Registro de la Plantilla
        public DateTime? FechaRegistro { get; set; }

        //Activo o Inactivo de la plantilla
        public bool? Activo { get; set; }

        //La plantilla esta compuesto de una serie de posiciones donde iran las actividades a presentarse
        virtual public List<PosicionActividad> PosicionesActividades { get; set; }

        //Versionado
        public byte[] Version { get; set; }
    }
}
