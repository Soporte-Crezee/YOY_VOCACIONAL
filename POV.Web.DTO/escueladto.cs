using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Web.DTO
{
    public class escueladto
    {
        public int? escuelaid { get; set; }
        public string clave { get; set; }
        public string nombre { get; set; }
        public string turno { get; set; }
        public string ambito { get; set; }
        public string control { get; set; }
        public string nivel { get; set; }
        public string tiposervicio { get; set; }
        public string zona { get; set; }
        public string ubicacion { get; set; }
        public centrocomputodto centrocomputo { get; set; }
    }
}
