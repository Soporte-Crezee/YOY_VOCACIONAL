using System;
using System.Collections.Generic;

namespace POV.Profesionalizacion.BO
{
    /// <summary>
    /// Eje tematico del area de profesionalización
    /// </summary>
    public class EjeTematico
    {
        private long? ejeTematicoID;
        /// <summary>
        /// Identificador autonumérico del Eje Temático
        /// </summary>
        public long? EjeTematicoID
        {
            get { return this.ejeTematicoID; }
            set { this.ejeTematicoID = value; }
        }
        private string nombre;
        /// <summary>
        /// Nombre del Eje Temático
        /// </summary>
        public string Nombre
        {
            get { return this.nombre; }
            set { this.nombre = value; }
        }
        private string descripcion;
        /// <summary>
        /// Descripción del Eje Temático
        /// </summary>
        public string Descripcion
        {
            get { return this.descripcion; }
            set { this.descripcion = value; }
        }
        private AreaProfesionalizacion areaProfesionalizacion;
        /// <summary>
        /// Area de Profesionalización del Eje Temático
        /// </summary>
        public AreaProfesionalizacion AreaProfesionalizacion
        {
            get { return this.areaProfesionalizacion; }
            set { this.areaProfesionalizacion = value; }
        }
        private EEstatusProfesionalizacion? estatusProfesionalizacion;
        /// <summary>
        /// Estatus del Eje Temático
        /// </summary>
        public EEstatusProfesionalizacion? EstatusProfesionalizacion
        {
            get { return this.estatusProfesionalizacion; }
            set { this.estatusProfesionalizacion = value; }
        }
        private IEnumerable<MateriaProfesionalizacion> materiasProfesionalizacion;
        /// <summary>
        /// Materias de Profesionalización del Eje Temático
        /// </summary>
        public IEnumerable<MateriaProfesionalizacion> MateriasProfesionalizacion
        {
            get { return this.materiasProfesionalizacion; }
            set { this.materiasProfesionalizacion = value; }
        }
        private DateTime? fechaRegistro;
        /// <summary>
        /// Fecha de Registro del Eje Temático
        /// </summary>
        public DateTime? FechaRegistro
        {
            get { return this.fechaRegistro; }
            set { this.fechaRegistro = value; }
        }
        private IEnumerable<SituacionAprendizaje> situacionesAprendizaje;
        /// <summary>
        /// Situaciones de Aprendizaje del Eje Temático
        /// </summary>
        public IEnumerable<SituacionAprendizaje> SituacionesAprendizaje
        {
            get { return this.situacionesAprendizaje; }
            set { this.situacionesAprendizaje = value; }
        }
        ///<summarry>
        ///Constructor del Eje Temático
        ///</summary$gt;
        public EjeTematico()
        {
            situacionesAprendizaje = new List<SituacionAprendizaje>();
            materiasProfesionalizacion = new List<MateriaProfesionalizacion>();
        }
     
    }
}
