using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.CentroEducativo.BO;

namespace POV.Seguridad.BO
{
    public class UsuarioEscolarPrivilegios : UsuarioPrivilegios
    {
        private Escuela escuela;

        public Escuela Escuela
        {
            get { return this.escuela; }
            set { this.escuela = value; }
        }

        private CicloEscolar cicloEscolar;

        public CicloEscolar CicloEscolar
        {
            get { return this.cicloEscolar; }
            set { this.cicloEscolar = value; }
        }

        public override ETipoUsuarioPrivilegios Tipo
        {
            get { return ETipoUsuarioPrivilegios.USUARIO_ESCOLAR; }
        }
    }
}
