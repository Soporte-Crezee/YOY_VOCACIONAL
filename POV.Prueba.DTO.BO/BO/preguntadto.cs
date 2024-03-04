using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Prueba.DTO.BO
{
    public class preguntadto
    {
        public int? preguntaid { get; set; }

        public string texto { get; set; }
        public string imagenurl { get; set; }
        public byte? tipopresentacion { get; set; }
        public byte? tipopruebapresentacion { get; set; }

        public short? tiporespuesta { get; set; }

        //abierta
        public bool? escorta { get; set; }
        public int? maximocaracteres { get; set; }
        public int? minimocaracteres { get; set; }
        //numerica

        //opcionmultiple
        public short? tiposeleccion { get; set; }
        public byte? presentacionopcion { get; set; }
        public List<opciondto> opciones { get; set; }

        public long? respuestareactivoid { get; set; }
        public long? respuestapreguntaid { get; set; }

        // asignale el clasificador a la pregunta
        public int? clasificadorid { get; set; }
    }
}
