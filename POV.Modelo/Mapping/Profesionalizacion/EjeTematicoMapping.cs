using System.Data.Entity.ModelConfiguration;
using POV.Profesionalizacion.BO;

namespace POV.Modelo.Mapping.Profesionalizacion
{
	
	public class EjeTematicoMapping : EntityTypeConfiguration<EjeTematico>
	{
		public EjeTematicoMapping()
		{
			ToTable("EjeTematico");
			Ignore(e => e.AreaProfesionalizacion);
			Ignore(e => e.MateriasProfesionalizacion);
			Ignore(e => e.SituacionesAprendizaje);
			
		}
	}
}
