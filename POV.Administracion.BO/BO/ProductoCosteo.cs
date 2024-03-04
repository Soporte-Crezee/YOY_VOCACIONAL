using POV.Prueba.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Administracion.BO
{
    public class ProductoCosteo
    {
        public int? ProductoID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        /// <summary>
        /// Determinar el tipo de producto,
        /// Expediente
        /// </summary> 
        public ETipoProducto? TipoProducto { get; set; }

        public List<CostoProducto> CostoProducto { get; set; }

        public int? PruebaID { get; set; }
    }
}
