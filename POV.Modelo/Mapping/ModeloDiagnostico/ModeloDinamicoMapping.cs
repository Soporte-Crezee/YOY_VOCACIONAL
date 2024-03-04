using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using POV.Modelo.BO;

namespace POV.Modelo.Mapping.ModeloDiagnostico
{
    internal class ModeloDinamicoMapping : EntityTypeConfiguration<ModeloDinamico>
    {
        public ModeloDinamicoMapping()
        {
            Property(p => p.MetodoCalificacion).HasColumnName("MetodoCalificacion");
            HasMany(p => p.ListaClasificador).WithRequired().Map(m => m.MapKey("ModeloID")); 
            Ignore(m => m.ListaPropiedadPersonalizada);
            
        }
    }
}
