using GP.SocialEngine.BO;
using POV.Seguridad.BO;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace POV.Modelo.Mapping.Seguridad
{
    public class UsuarioMapping : EntityTypeConfiguration<Usuario>
    {
        public UsuarioMapping()
        {
            ToTable("Usuario");
            HasKey(x => x.UsuarioID);

            Property(x => x.NombreUsuario).IsRequired();

            Ignore(i => i.Password);
            Ignore(i => i.EsActivo);
            Ignore(i => i.FechaCreacion);
            Ignore(i => i.FechaUltimoAcceso);
            Ignore(i => i.FechaUltimoCambioPassword);
            Ignore(i => i.Comentario);
            Ignore(i => i.PasswordTemp);
            Ignore(i => i.Email);
            Ignore(i => i.EmailVerificado);
            Ignore(i => i.EmailAlternativo);
            Ignore(i => i.TelefonoReferencia);
            Ignore(i => i.TelefonoCasa);
            Ignore(i => i.AceptoTerminos);
            Ignore(i => i.Termino);

        }
    }
}
