using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Comun.BO;

namespace POV.CentroEducativo.BO
{
    public class AsignacionDocenteEscuela : ICloneable, IObservableChange
    {
        private long? asignacionDocenteEscuelaID;
        private DateTime? fechaRegistro;
        private DateTime? fechaBaja;
        private bool? activo;
        private Docente docente;

        public long? AsignacionDocenteEscuelaID
        {
            get { return asignacionDocenteEscuelaID; }
            set
            {
                if (this.asignacionDocenteEscuelaID != value)
                {
                    this.asignacionDocenteEscuelaID = value;
                    this.Cambio();
                }
            }
        }

        public DateTime? FechaRegistro
        {
            get { return fechaRegistro; }
            set
            {
                if (this.fechaRegistro != value)
                {
                    fechaRegistro = value;
                    this.Cambio();
                }
            }
        }

        public DateTime? FechaBaja
        {
            get { return fechaBaja; }
            set
            {
                if (this.fechaBaja != value)
                {
                    fechaBaja = value;
                    this.Cambio();
                }
            }
        }

        public bool? Activo
        {
            get { return activo; }
            set
            {
                if (this.activo != value)
                {
                    activo = value;
                    this.Cambio();
                }
            }
        }

        public Docente Docente
        {
            get { return docente; }
            set
            {
                if (this.docente != value)
                {
                    this.docente = value;

                    if ((this.docente == null || value == null) || (this.docente.DocenteID != value.DocenteID))
                    {
                        this.Cambio();
                    }
                }
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public object CloneAll()
        {
            AsignacionDocenteEscuela original = (AsignacionDocenteEscuela)this.Clone();
            original.Docente = (Docente)docente.Clone();

            return original;
        }

        private IObserverChange observer;
        public void Registrar(IObserverChange observer)
        {
            this.observer = observer;
        }
        private void Cambio()
        {
            if (this.observer != null)
                this.observer.Cambio();
        }
    }
}
