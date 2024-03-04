using POV.Modelo.BO;
using System;
using System.Collections.Generic;


namespace POV.CentroEducativo.BO
{
    /// <summary>
    /// Catalogo de Carreras disponibles en el sistema
    /// </summary>
    public class Carrera: ICloneable
    {
        public long? CarreraID {get; set;}
        public string NombreCarrera { get; set; }
        public string Descripcion { get; set; }
        public bool? Activo { get; set; }
        public List<Universidad> Universidades { get; set; }
        public virtual Clasificador Clasificador { get; set; }
        public int? ClasificadorID { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
