using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using POV.ConfiguracionActividades.BO;

namespace POV.Modelo.Mapping.ConfiguracionActividades
{
    internal class ClasificadorResultadoDinamicaMapping : EntityTypeConfiguration<ClasificadorResultadoDinamica>
    {
        public ClasificadorResultadoDinamicaMapping()
		{

			HasRequired(c => c.Clasificador).WithMany().HasForeignKey(c => c.ClasificadorId);
			
		}
    }
}
