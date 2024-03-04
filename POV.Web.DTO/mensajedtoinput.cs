using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Web.DTO
{
   public class mensajedtoinput
    {
       public string mensajeid { get; set; }
       public int? espadre { get; set; }
       public long? currentusuarioid { get; set; }
       public int pagesize { get; set; }
        public int currentpage { get; set; }
        public string sort { get; set; }
        public string order { get; set; }
        public long? remitenteid { get; set; }
        public long? destinatarioid { get; set; }
        public int? estatus { get; set; }

        public int? recordcount { get; set; }
    }
}
