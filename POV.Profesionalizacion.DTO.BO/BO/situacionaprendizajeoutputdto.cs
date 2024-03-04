using System.Collections.Generic;

namespace POV.Profesionalizacion.DTO.BO
{
   public class situacionaprendizajeoutputdto
    {
        
        public long? ejetematicoid { get; set; }
        public List<situacionaprendizajedto> situaciones { get; set; }

        //paginación
        public int? currentpage { get; set; }
      
    }
}
