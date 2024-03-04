using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Web.DTO
{
    public class confirmaciondirectordto
    {
        public int? directorid { get; set; }
        public List<escueladto> escuelas { get; set; }
        public string nombreusuario { get; set; }

        #region variables de control
        public bool? success { get; set; }
        public string urlredirect { get; set; }
        public string infomsg { get; set; }
        public string errormsg { get; set; }
        #endregion
    }
}
