using System;
using System.Collections.Generic;
using POV.Profesionalizacion.BO;

namespace POV.ConfiguracionActividades.BO
{
    public class EjeTematicoRef
    {
        public long? EjeTematicoId { get; set; }
        public String NombreEjeTematico { get; set; }
        public int? AreaProfesionalizacionId { get; set; }
        public String NombreArea { get; set; }
        public virtual List<MateriaProfesionalizacion> MateriasList { get; set; }
        public String NombreMaterias { get; set; }
        public String Clasificador { get; set; }
        public long? SituacionAprendizajeId { get; set; }
        public String NombreSituacion { get; set; }
        public long? ContenidoDigitalId { get; set; }
        public String ClaveContenido { get; set; }
        public String NombreContenido { get; set; }
        public int? TipoDocumentoId { get; set; }
        public String NombreTipoDocumento { get; set; }
        public String ExtencionTipoDocumento { get; set; }
        
        public byte? Grado { get; set; }
        public string Competencia { get; set; }
        public string Aprendizaje { get; set; }
    }
}
