using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POV.Web.PortalSocial.AppCode
{
    public class EscuelaListItem
    {
        public int? IDEscuela { get; set; }
        public string Clave { get; set; }
        public string Escuela { get; set; }
        public string Turno { get; set; }
        public string LicenciaID { get; set; }
        public List<GrupoListItem> Grupos{get; set;}
    }
}