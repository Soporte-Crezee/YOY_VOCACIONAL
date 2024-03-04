using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Web.DTO
{
    public class contactoinputdto
    {
        public long? usuariosociaid { get; set; }
        public long? socialhubid { get; set; }

        public long? usuariosocialsessionid { get; set; }
        public long? socialhubsessionid { get; set; }
        public long? groupid { get; set; }

        public string screenname { get; set; }
        public string nombrecompleto { get; set; }
        public int? escuelaid { get; set; }
        public int? estadoid { get; set; }
        public int? municipioid { get; set; }
        public int? localidadid { get; set; }

        public short? tipo {get;set; }


        public int pagesize { get; set; }
        public int currentpage { get; set; }
        public string sort { get; set; }
        public string order { get; set; }
    }
}
