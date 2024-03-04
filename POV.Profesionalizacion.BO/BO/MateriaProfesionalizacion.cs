using System;
using System.Collections.Generic;
using System.Linq;
using POV.Comun.BO;

namespace POV.Profesionalizacion.BO { 
    /// <summary>
    /// Representa un objeto del tipo MateriaProfesionalizacion
    /// </summary>
    public class MateriaProfesionalizacion : ICloneable, IObservableChange
    { 
        private int? materiaID;
        /// <summary>
        /// Identificador de la MateriaProfesionalizacion
        /// </summary>
        public int? MateriaID{
            get{ return this.materiaID; }
            set{ this.materiaID = value; }
        }
        private string nombre;
        /// <summary>
        /// Nombre del objeto MateriaProfesionalizacion
        /// </summary>
        public string Nombre{
            get{ return this.nombre; }
            set
            {
                if(this.nombre != value)
                {
                    this.nombre = value;
                    this.Cambio();
                }
            }
        }
        private DateTime? fechaRegistro;
        /// <summary>
        /// FechaRegistro del objeto MateriaProfesionalizacion
        /// </summary>
        public DateTime? FechaRegistro{
            get{ return this.fechaRegistro; }
            set{ this.fechaRegistro = value;}
        }
        private bool? activo;
        /// <summary>
        /// Estatus del objeto MateriaProfesionalizacion
        /// </summary>
        public bool? Activo{
            get { return this.activo; }
            set
            {
                if (this.activo != value)
                {
                    this.activo = value;
                    this.Cambio();
                }
            }
        }

        private EObjetoEstado materiaestado;
        public EObjetoEstado MateriaEstado
        {
            get{return this.materiaestado;}
            set { if (this.materiaestado != value)
            {
                this.materiaestado = value;
            } }
        }
        /// <summary>
        /// crea una copia del objeto MateriaProfesionalizacion
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        /// <summary>
        /// crea una copia del objeto MateriaProfesionalizacion
        /// </summary>
        /// <returns></returns>
        public object CloneAll()
        {
            return this.Clone();
        }
        private IObserverChange observer;
        /// <summary>
        /// registra a un objeto IObserverChange
        /// </summary>
        /// <param name="observer"></param>
        public void Registrar(IObserverChange observer)
        {
            this.observer = observer;
        }
        /// <summary>
        /// 
        /// </summary>
        private void Cambio()
        {
            if (this.observer != null)
                this.observer.Cambio();
        }
    } 
}
