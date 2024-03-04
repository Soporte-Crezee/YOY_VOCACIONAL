using POV.Comun.BO;
using System;

namespace POV.CentroEducativo.BO
{
    public class AsignacionEspecialistaEscuela :ICloneable,IObservableChange
    {
        private long? asignacionEspecialistaEscuelaID;
        private DateTime? fechaRegistro;
        private DateTime? fechaBaja;
        private bool? activo;
        private EspecialistaPruebas especialista;

        public long? AsignacionEspecialistaEscuelaID
        {
            get { return asignacionEspecialistaEscuelaID; }
            set
            {
                if (this.asignacionEspecialistaEscuelaID != value)
                {
                    this.asignacionEspecialistaEscuelaID = value;
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

        public EspecialistaPruebas Especialista
        {
            get { return especialista; }
            set
            {
                if (this.especialista != value)
                {
                    this.especialista = value;

                    if ((this.especialista == null || value == null) || (this.especialista.EspecialistaPruebaID != value.EspecialistaPruebaID))
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
            AsignacionEspecialistaEscuela original = (AsignacionEspecialistaEscuela)this.Clone();
            original.Especialista = (EspecialistaPruebas)especialista.Clone();

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
