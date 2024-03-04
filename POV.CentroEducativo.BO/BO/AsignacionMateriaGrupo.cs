using System;
using POV.Comun.BO;

namespace POV.CentroEducativo.BO
{
    /// <summary>
    /// Clase para asingar alumnos a un grupo
    /// </summary>
    public class AsignacionMateriaGrupo : ICloneable, IObservableChange
    {
        private long? asignacionMateriaGrupoID;
        /// <summary>
        /// Identificador de asignacion
        /// </summary>
        public long? AsignacionMateriaGrupoID
        {
            get { return this.asignacionMateriaGrupoID; }
            set
            {
                if (this.asignacionMateriaGrupoID != value)
                {
                    this.asignacionMateriaGrupoID = value;
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
        private Docente docente;
        /// <summary>
        /// Docente
        /// </summary>
        public Docente Docente
        {
            get { return this.docente; }
            set {
                if (this.docente != value)
                {
                    this.docente = value;

                    if ((this.docente == null || value == null) || (this.docente.DocenteID != value.DocenteID))
                        this.Cambio();
                }
            }
        }
        private Materia materia;
        /// <summary>
        /// Materia
        /// </summary>
        public Materia Materia
        {
            get { return materia; }
            set {
                if (this.materia != value)
                {
                    this.materia = value;

                    if ((this.materia == null || value == null) || (this.materia.MateriaID != value.MateriaID))
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
            AsignacionMateriaGrupo clone = (AsignacionMateriaGrupo)this.Clone();
            clone.Docente = (Docente)this.docente.Clone();
            clone.Materia = (Materia)this.materia.Clone();

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
