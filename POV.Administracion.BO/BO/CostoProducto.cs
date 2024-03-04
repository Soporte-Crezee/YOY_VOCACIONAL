using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Administracion.BO
{
    public class CostoProducto
    {
        public int? CostoProductoId { get; set; }
        public double? Precio { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public virtual ProductoCosteo Producto { get; set; }
        public int? ProductoID { get; set; }
    }
}
