using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POV.Reactivos.BO;

namespace POV.Modelo.Mapping.Reactivos
{
	
	public class ReactivoMapping : EntityTypeConfiguration<Reactivo>
	{
		public ReactivoMapping()
		{
			ToTable("Reactivo");
			HasKey(r => r.ReactivoID);

			Ignore(r => r.Clave);
			Ignore(r => r.Caracteristicas);
			Ignore(r => r.ToShortToTipoReactivo);
			Ignore(r => r.Asignado);
			Ignore(r => r.FechaRegistro);
			Ignore(r => r.Preguntas);


		}
	}
}
