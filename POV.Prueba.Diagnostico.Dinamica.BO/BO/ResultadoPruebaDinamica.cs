using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Prueba.BO;
using POV.Expediente.BO;
using POV.CentroEducativo.BO;

namespace POV.Prueba.Diagnostico.Dinamica.BO
{
    /// <summary>
    /// Representa un resulta de prueba dinamica
    /// </summary>
    public class ResultadoPruebaDinamica : AResultadoPrueba, IResultadoPrueba
    {
        private Alumno alumno;
        private RegistroPruebaDinamica registroPrueba;

        public Alumno Alumno
        {
            get
            {
                return this.alumno;
            }
            set
            {
                this.alumno = value;
            }
        }

        public ARegistroPrueba RegistroPrueba
        {
            get
            {
                return this.registroPrueba;
            }
            set
            {
                if (value != null)
                {
                    if (value is RegistroPruebaDinamica)
                        this.registroPrueba = value as RegistroPruebaDinamica;
                    else
                        throw new ArgumentException("ResultadoPruebaDinamica: el RegistroPrueba tiene que ser de tipo RegistroPruebaDinamica");
                }
                else
                    this.registroPrueba = null;
            }
        }


        public override string Nombre()
        {
            throw new NotImplementedException();
        }

        public override List<string> Resultados()
        {
            throw new NotImplementedException();
        }
    }
}
