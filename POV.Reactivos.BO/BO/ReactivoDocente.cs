using System; 
using POV.CentroEducativo.BO;

namespace POV.Reactivos.BO
{
    public class ReactivoDocente : Reactivo
    {
        private Docente docente;

        public Docente Docente
        {
            get { return docente;  }
            set { docente = value; }
        }
    }
}
