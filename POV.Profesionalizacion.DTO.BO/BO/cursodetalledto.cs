using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.ContenidosDigital.DTO.BO;

namespace POV.Profesionalizacion.DTO.BO
{
    public class cursodetalledto
    {
        public int? cursoid { get; set; }
        public int currentpage { get; set; }
        public List<contenidodto> contenidos { get; set; }
       
    }
}

