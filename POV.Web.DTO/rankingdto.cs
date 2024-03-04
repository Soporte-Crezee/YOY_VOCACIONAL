using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Web.DTO
{
    public class rankingdto
    {
        public string rankingid { get; set; }
        public int? vote { get; set; }
        public List<contactodto> peoples { get; set; }
    }
}
