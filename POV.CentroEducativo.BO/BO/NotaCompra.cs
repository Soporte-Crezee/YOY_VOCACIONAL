using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.CentroEducativo.BO
{
    public class NotaCompra
    {
        public int? NotaCompraID { get; set; }

        public long? TutorID { get; set; }
        public long? AlumnoID { get; set; }
        public DateTime? FechaCompra { get; set; }
        public int? CostoProductoID { get; set; }
        public double? Cantidad { get; set; }
        public string ConceptoCompra { get; set; }
    }
}
