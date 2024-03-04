using System;
using System.Collections.Generic;
using System.Linq;
using POV.CentroEducativo.BO;
using POV.Comun.BO;

namespace POV.Profesionalizacion.BO { 
    /// <summary>
    /// Representa un objeto del tipo AreaProfesionalizacion
    /// </summary>
    public class AreaProfesionalizacion : ICloneable
    {
        #region Constructores
        /// <summary>
        /// Constructor
        /// </summary>
        public AreaProfesionalizacion()
        {
            this.materiasProfesionalizacion = new ObjetoListado<MateriaProfesionalizacion>();
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="materiaProfesionalizacion"></param>
        public AreaProfesionalizacion(IEnumerable<MateriaProfesionalizacion> materiaProfesionalizacion)
        {
            this.materiasProfesionalizacion = new ObjetoListado<MateriaProfesionalizacion>(materiaProfesionalizacion);
        }
        #endregion

        private int? areaProfesionalizacionID;
        /// <summary>
        /// Identificador del AreaProfesionalizacion
        /// </summary>
        public int? AreaProfesionalizacionID{
            get{ return this.areaProfesionalizacionID; }
            set{ this.areaProfesionalizacionID = value; }
        }
        private string nombre;
        /// <summary>
        /// Nombre del objeto AreaProfesionalizacion
        /// </summary>
        public string Nombre{
            get{ return this.nombre; }
            set{ this.nombre = value; }
        }
        private string descripcion;
        /// <summary>
        /// Nombre del objeto AreaProfesionalizacion
        /// </summary>
        public string Descripcion
        {
            get { return this.descripcion; }
            set { this.descripcion = value; }
        }
        private DateTime? fechaRegistro;
        /// <summary>
        /// FechaRegistro del objeto AreaProfesionalizacion
        /// </summary>
        public DateTime? FechaRegistro{
            get{ return this.fechaRegistro; }
            set{ this.fechaRegistro = value; }
        }
        private bool? activo;
        /// <summary>
        /// Activo del objeto AreaProfesionalizacion
        /// </summary>
        public bool? Activo{
            get{ return this.activo; }
            set{ this.activo = value; }
        }
        private NivelEducativo nivelEducativo;
        /// <summary>
        /// NivelEducativo del objeto AreaProfesionalizacion
        /// </summary>
        public NivelEducativo NivelEducativo
        {
            get { return nivelEducativo; }
            set { nivelEducativo = value; }
        }
        private byte? grado;
        /// <summary>
        /// grado del objeto AreaProfesionalizacion
        /// </summary>
        public byte? Grado
        {
            get { return grado; }
            set { grado = value; }
        }
        private ObjetoListado<MateriaProfesionalizacion> materiasProfesionalizacion;
        /// <summary>
        /// MateriasProfesionalizacion relacionadas al AreaProfesionalizacion
        /// </summary>
        public IEnumerable<MateriaProfesionalizacion> MateriasProfesionalizacion
        {
            get { return this.materiasProfesionalizacion.Objetos; }
        }
        
        #region Métodos para el manejo de la Lista MateriasProfesionalizacion
        /// <summary>
        /// Obtiene MateriaProfesionalizacion de la lista en base a su ID
        /// </summary>
        /// <param name="ID">ID del objeto MateriaProfesionalizacion</param>
        /// <returns></returns>
        public MateriaProfesionalizacion MateriaProfesionalizacionObtenerByID(int ID)
        {
            return this.MateriasProfesionalizacion.FirstOrDefault(x => x.MateriaID == ID);
        }
        /// <summary>
        /// Obtener MateriaProfesionalizacion de la lista basado en su indice
        /// </summary>
        /// <param name="indice"></param>
        /// <returns></returns>
        public MateriaProfesionalizacion MateriaProfesionalizacionObtener(int indice)
        {
            return this.materiasProfesionalizacion.Obtener(indice);
        }
        /// <summary>
        /// Agrega MateriaProfesionalizacion a la lista
        /// </summary>
        /// <param name="materiaProfesionalizacion">MateriaProfesionalizacion a agregar</param>
        /// <returns></returns>
        public MateriaProfesionalizacion MateriaProfesionalizacionAgregar(MateriaProfesionalizacion materiaProfesionalizacion)
        {
            materiaProfesionalizacion.Activo = true;
            return this.materiasProfesionalizacion.Agregar(MateriaProfesionalizacionIdentificar(materiaProfesionalizacion), materiaProfesionalizacion);
        }
        /// <summary>
        /// Elimina MateriaProfesionalizacion de la lista
        /// </summary>
        /// <param name="materiaProfesionalizacion">MateriaProfesionalizacion a eliminar</param>
        /// <returns></returns>
        public MateriaProfesionalizacion MateriaProfesionalizacionEliminar(MateriaProfesionalizacion materiaProfesionalizacion)
        {
            MateriaProfesionalizacion dtReturn = this.materiasProfesionalizacion.Eliminar(MateriaProfesionalizacionIdentificar(materiaProfesionalizacion), materiaProfesionalizacion);
            return dtReturn;
        }
        /// <summary>
        /// Elimina MateriaProfesionalizacion de la lista basado en su indice
        /// </summary>
        /// <param name="indice"></param>
        /// <returns></returns>
        public MateriaProfesionalizacion MateriaProfesionalizacionEliminar(int indice)
        {
            MateriaProfesionalizacion dtReturn = this.materiasProfesionalizacion.Eliminar(indice);
            return dtReturn;
        }
        /// <summary>
        /// Obtiene el numero de elementos de la lista de MateriaProfesionalizacion
        /// </summary>
        public int MateriaProfesionalizacionElementos()
        {
            return this.materiasProfesionalizacion.Elementos();
        }
        /// <summary>
        /// Obtiene el numero completo de elementos de la lista de MateriaProfesionalizacion
        /// </summary>
        public int MateriaProfesionalizacionElementosTodos()
        {
            return this.materiasProfesionalizacion.ElementosTodos();
        }
        /// <summary>
        /// Elimina todos los elementos de la lista de MateriaProfesionalizacion
        /// </summary>
        public void MateriaProfesionalizacionLimpiar()
        {
            this.materiasProfesionalizacion.Limpiar();
        }
        /// <summary>
        /// Obtener el estado del objeto MateriaProfesionalizacion
        /// </summary>
        public EObjetoEstado MateriaProfesionalizacionEstado(MateriaProfesionalizacion materiaProfesionalizacion)
        {
            return this.materiasProfesionalizacion.Estado(materiaProfesionalizacion);
        }
        /// <summary>
        /// Obtener el  objeto original del MateriaProfesionalizacion
        /// </summary>
        public MateriaProfesionalizacion MateriaProfesionalizacionOriginal(MateriaProfesionalizacion materiaProfesionalizacion)
        {
            return this.materiasProfesionalizacion.Original(materiaProfesionalizacion);
        }
        /// <summary>
        /// Definir la expresión que identificará al MateriaProfesionalizacion
        /// </summary>
        private static Func<MateriaProfesionalizacion, bool> MateriaProfesionalizacionIdentificar(MateriaProfesionalizacion materiaProfesionalizacion)
        {

            return (x => materiaProfesionalizacion.MateriaID != null && x.MateriaID == materiaProfesionalizacion.MateriaID);
        }
        
        /// <summary>
        /// Indica si algún objeto MateriaProfesionalizacion a modificado su estado.
        /// </summary>
        /// <returns></returns>
        public bool HayMateriasProfesionalizacionModificados()
        {
            return this.MateriasProfesionalizacion.FirstOrDefault(d => this.MateriaProfesionalizacionEstado(d) != EObjetoEstado.SINCAMBIO) != null;
        }
        #endregion

        /// <summary>
        /// crea una copia del objeto AreaProfesionalizacion
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        /// <summary>
        /// crea una copia del objeto AreaProfesionalizacion
        /// </summary>
        /// <returns></returns>
        public object CloneAll()
        {
            return this.Clone();
        }
    } 
}
