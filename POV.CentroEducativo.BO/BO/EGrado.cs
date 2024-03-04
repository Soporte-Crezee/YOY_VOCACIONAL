using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace POV.CentroEducativo.BO
{
    public enum EGrado : byte
    {
        [Description("1 Semestre")]
        SEMESTRE_1 = 1,
        SEMESTRE_2 = 2,
        SEMESTRE_3 = 3,
        SEMESTRE_4 = 4,
        SEMESTRE_5 = 5,
        SEMESTRE_6 = 6
    }
}
