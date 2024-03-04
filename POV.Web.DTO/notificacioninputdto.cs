using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Web.DTO
{
    public class notificacioninputdto
    {
        public int pagesize { get; set; }
        public int currentpage { get; set; }
        public string sort { get; set; }
        public string order { get; set; }
        public long? emisorid { get; set; }
        public long? receptorid { get; set; }

    }
}
