using System;

namespace POV.CentroEducativo.BO
{
    public class EspecialistaEscuela:ICloneable
    {
        private long? especialistaEscuelaID;

        public long? EspecialistaEscuelaID
        {
            get { return this.especialistaEscuelaID; }
            set { this.especialistaEscuelaID = value; }
        }
        private int? escuelaID;
        
        public int? EscuelaID
        {
            get { return this.escuelaID; }
            set { this.escuelaID = value; }
        }
        private int? especialistaID;

        public int? EspecialistaID
        {
            get { return this.especialistaID; }
            set { this.especialistaID = value; }
        }
        private bool? estatus;
        
        public bool? Estatus
        {
            get { return this.estatus; }
            set { this.estatus = value; }
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
