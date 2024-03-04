using System;
using POV.Comun.BO;

namespace POV.CentroEducativo.BO
{
    /// <summary>
    /// Clase para asingar alumnos a un grupo
    /// </summary>
    public class AsignacionAlumnoGrupo : ICloneable, IObservableChange
    {
        private Guid? asignacionAlumnoGrupoID;
        /// <summary>
        /// Identificador de asignacion
        /// </summary>
        public Guid? AsignacionAlumnoGrupoID
        {
            get { return this.asignacionAlumnoGrupoID; }
            set
            {
                if (this.asignacionAlumnoGrupoID != value)
                {
                    this.asignacionAlumnoGrupoID = value;
                    this.Cambio();
                }
            }
        }
        private DateTime? fechaRegistro;
        /// <summary>
        /// Fecha en que se hace el registro
        /// </summary>
        public DateTime? FechaRegistro
        {
            get { return this.fechaRegistro; }
            set
            {
                if (this.fechaRegistro != value)
                {
                    this.fechaRegistro = value;
                    this.Cambio();
                }
            }
        }
        private DateTime? fechaBaja;
        /// <summary>
        /// Fecha en que se da de baja
        /// </summary>
        public DateTime? FechaBaja
        {
            get { return this.fechaBaja; }
            set
            {
                if (this.fechaBaja != value)
                {
                    this.fechaBaja = value;
                    this.Cambio();
                }
            }
        }
        private bool? activo;
        /// <summary>
        /// Estatus
        /// </summary>
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
        private Alumno alumno;
        /// <summary>
        /// Identificador del alumno
        /// </summary>
        public Alumno Alumno
        {
            get { return this.alumno; }
            set {
                if (this.alumno != value)
                {
                    this.alumno = value;

                    if ((this.alumno == null || value == null) || (this.alumno.AlumnoID != value.AlumnoID))
                        this.Cambio();
                }
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public object CloneAll()
        {
            AsignacionAlumnoGrupo clone = (AsignacionAlumnoGrupo)this.Clone();
            clone.Alumno = (Alumno)alumno.Clone();
            return clone;
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
