using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.CentroEducativo.BO
{
    public class TutorAlumno : ICloneable
    {
        public Int64? TutorID { get; set; }
        public Int64? AlumnoID { get; set; }
        public Int16? Parentesco { get; set; }

        public virtual Alumno Alumno { get; set; }
        public virtual Tutor Tutor { get; set; }

        private string descripcionParentesco;
        public string DescripcionParentesco
        {
            get
            {
                string res = string.Empty;
                switch (Parentesco) { 
                    case 1:
                        res = "PADRE";
                        break;
                    case 2:
                        res = "MADRE";
                        break;
                    case 3:
                        res = "OTRO";
                        break;
                }
                return res;
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
