namespace POV.Profesionalizacion.DTO.BO
{
    public class situacionaprendizajedto
    {
        public long? situacionid {get;set;}
        public long? ejetematicoid { get; set; }
        public string nombresituacion { get; set; }
        public string descripcionsituacion { get; set; }
        public string nombreeje { get; set; }
        public agrupadoroutputdto agrupadorcontenido { get; set; }

        //variables de control
        public bool? success { get; set; }
        public string errors { get; set; }
        public bool? rendersuscribir { get; set; }
        public bool? renderestatus { set; get; }
    }
}
