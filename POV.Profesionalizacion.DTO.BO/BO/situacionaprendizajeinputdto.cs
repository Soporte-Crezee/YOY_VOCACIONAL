namespace POV.Profesionalizacion.DTO.BO
{
	public class situacionaprendizajeinputdto
	{
        public long? situacionid { get; set; }
		public long? ejetematicoid {get; set;}
		
		//Filtros de búsqueda.
        public string nombresituacion { get; set; }
		
		//paginación
        public int? pagesize { get; set; }
        public int? currentpage { get; set; }
        public string sort { get; set; }
        public string order { get; set; }

        //variables de control
        public bool? success { get; set; }
        public string errors { get; set; }
	}
}
