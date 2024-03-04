using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POV.ConfiguracionesPlataforma.BO
{
    public class ConfiguracionGeneral
    {
        //Identificador de la Configuracion General
        public int? ConfiguracionGeneralId { get; set; }

        //Maximo Top Alumnos
        public int? MaximoTopAlumnos { get; set; }

        //Ruta del Servidor de Contenidos
        public string RutaServidorContenido { get; set; }

        //Ruta de las plantillas en el servidor de contenidos
        public string RutaPlantillas { get; set; }

        //Ruta de las insignias en el servidor de contenidos
        public string RutaInsignias { get; set; }

        //Versionado
        public byte[] Version { get; set; }
    }
}
