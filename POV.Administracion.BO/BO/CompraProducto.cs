using POV.CentroEducativo.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Administracion.BO
{
    public class CompraProducto
    {
        public int? CompraProductoID { get; set; }        
        public long? UniversidadID { get; set; }
        public DateTime? FechaCompra { get; set; }
        public string CodigoCompra { get; set; }
        public double? CostoCompra { get; set; }

        public Universidad Universidad { get; set; }
    }
}
