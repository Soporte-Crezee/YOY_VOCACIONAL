using System;
using System.Collections.Generic;
using POV.Comun.BO;

namespace POV.CentroEducativo.BO
{
    /// <summary>
    /// Clase para grupo por ciclo escolar
    /// </summary>
    public class GrupoCicloEscolar
    {
        public GrupoCicloEscolar()
        {
            this.asignacionAlumnos = new ObjetoListado<AsignacionAlumnoGrupo>();
            this.asignacionMaterias = new ObjetoListado<AsignacionMateriaGrupo>();
        }

        public GrupoCicloEscolar(IEnumerable<AsignacionAlumnoGrupo> asignacionAlumno, IEnumerable<AsignacionMateriaGrupo> asignacionMateria )
        {
            this.asignacionAlumnos = new ObjetoListado<AsignacionAlumnoGrupo>(asignacionAlumno);
            this.asignacionMaterias = new ObjetoListado<AsignacionMateriaGrupo>(asignacionMateria);
        }

        private Guid? grupoCicloEscolarID;
        /// <summary>
        /// Identificador grupo ciclo escolar
        /// </summary>
        public Guid? GrupoCicloEscolarID
        {
            get { return this.grupoCicloEscolarID; }
            set { this.grupoCicloEscolarID = value; }
        }
        private long? grupoSocialID;
        /// <summary>
        /// Identificador del grupo social
        /// </summary>
        public long? GrupoSocialID
        {
            get { return this.grupoSocialID; }
            set { this.grupoSocialID = value; }
        }
        private CicloEscolar cicloEscolar;
        /// <summary>
        /// CicloEscolar de GrupoCicloEscolar
        /// </summary>
        public CicloEscolar CicloEscolar
        {
            get { return this.cicloEscolar; }
            set { this.cicloEscolar = value; }
        }
        private Escuela escuela;
        /// <summary>
        /// Escuela de GrupoCicloEscolar
        /// </summary>
        public Escuela Escuela
        {
            get { return this.escuela; }
            set { this.escuela = value; }
        }
        private Grupo grupo;
        /// <summary>
        /// Grupo  de GrupoCicloEscolar
        /// </summary>
        public Grupo Grupo
        {
            get { return this.grupo; }
            set { this.grupo = value; }
        }
        private PlanEducativo planEducativo;
        /// <summary>
        /// PlanEducativo de GrupoCicloEscolar
        /// </summary>
        public PlanEducativo PlanEducativo
        {
            get { return this.planEducativo; }
            set { this.planEducativo = value; }
        }
        /// <summary>
        /// Clave  de GrupoCicloEscolar
        /// </summary>
        private string clave;
        public string Clave
        {
            get { return clave; }
            set { clave = value; }
        }

        /// <summary>
        /// Estado del GrupoCicloEscolar
        /// </summary>

        private bool? activo;
        public bool? Activo
        {
            get { return activo; }
            set { activo = value; }
        }

        private ObjetoListado<AsignacionAlumnoGrupo> asignacionAlumnos;
        public IEnumerable<AsignacionAlumnoGrupo> AsignacionAlumnos
        {
            get { return this.asignacionAlumnos.Objetos; }
        }
        /// <summary>
        /// Agrega AsignacionAlumnoGrupo
        /// </summary>
        public AsignacionAlumnoGrupo AsignacionAlumnoAgregar(AsignacionAlumnoGrupo asignacionAlumno)
        {
            return this.asignacionAlumnos.Agregar(AsignacionAlumnoIdentificar(asignacionAlumno), asignacionAlumno);
        }
        /// <summary>
        /// Obtener AsignacionAlumnoGrupo basado en el indice
        /// </summary>
        public AsignacionAlumnoGrupo AsignacionAlumnoObtener(int indice)
        {
            return this.asignacionAlumnos.Obtener(indice);
        }
        /// <summary>
        /// Elimina AsignacionAlumnoGrupo
        /// </summary>
        public AsignacionAlumnoGrupo AsignacionAlumnoEliminar(AsignacionAlumnoGrupo asignacionAlumno)
        {
            return this.asignacionAlumnos.Eliminar(AsignacionAlumnoIdentificar(asignacionAlumno), asignacionAlumno);
        }
        /// <summary>
        /// Elimina AsignacionAlumnoGrupo basado en el indice
        /// </summary>
        public AsignacionAlumnoGrupo AsignacionAlumnoEliminar(int indice)
        {
            return this.asignacionAlumnos.Eliminar(indice);
        }
        /// <summary>
        /// Obtiene el numero de elementos de la lista de AsignacionAlumnoGrupo
        /// </summary>
        public int AsignacionAlumnoElementos()
        {
            return this.asignacionAlumnos.Elementos();
        }
        /// <summary>
        /// Obtiene el numero completo de elementos de la lista de AsignacionAlumnoGrupo
        /// </summary>
        public int AsignacionAlumnoElementosTodos()
        {
            return this.asignacionAlumnos.ElementosTodos();
        }
        /// <summary>
        /// Elimina todos los elementos del lista de AsignacionAlumnoGrupo
        /// </summary>
        public void AsignacionAlumnoLimpiar()
        {
            this.asignacionAlumnos.Limpiar();
        }
        /// <summary>
        /// Obtener el estado del AsignacionAlumnoGrupo
        /// </summary>
        public EObjetoEstado AsignacionAlumnoEstado(AsignacionAlumnoGrupo asignacionAlumno)
        {
            return this.asignacionAlumnos.Estado(asignacionAlumno);
        }
        /// <summary>
        /// Obtener el  objeto original del AsignacionAlumnoGrupo
        /// </summary>
        public AsignacionAlumnoGrupo AsignacionAlumnoOriginal(AsignacionAlumnoGrupo asignacionAlumno)
        {
            return this.asignacionAlumnos.Original(asignacionAlumno);
        }
        /// <summary>
        /// Definir la expresión que identificara a la AsignacionAlumnoGrupo
        /// </summary>
        private static Func<AsignacionAlumnoGrupo, bool> AsignacionAlumnoIdentificar(AsignacionAlumnoGrupo asignacionAlumno)
        {
            return (uo => uo.AsignacionAlumnoGrupoID == asignacionAlumno.AsignacionAlumnoGrupoID);
        }

        private ObjetoListado<AsignacionMateriaGrupo> asignacionMaterias;
        public IEnumerable<AsignacionMateriaGrupo> AsignacionMaterias
        {
            get { return this.asignacionMaterias.Objetos; }
        }
        /// <summary>
        /// Agrega AsignacionAlumnoGrupo
        /// </summary>
        public AsignacionMateriaGrupo AsignacionMateriaAgregar(AsignacionMateriaGrupo asignacionMateria)
        {
            return this.asignacionMaterias.Agregar(AsignacionMateriaIdentificar(asignacionMateria), asignacionMateria);
        }
        /// <summary>
        /// Obtener AsignacionAlumnoGrupo basado en el indice
        /// </summary>
        public AsignacionMateriaGrupo AsignacionMateriaObtener(int indice)
        {
            return this.asignacionMaterias.Obtener(indice);
        }
        /// <summary>
        /// Elimina AsignacionAlumnoGrupo
        /// </summary>
        public AsignacionMateriaGrupo AsignacionMateriaEliminar(AsignacionMateriaGrupo asignacionMateria)
        {
            return this.asignacionMaterias.Eliminar(AsignacionMateriaIdentificar(asignacionMateria), asignacionMateria);
        }
        /// <summary>
        /// Elimina AsignacionAlumnoGrupo basado en el indice
        /// </summary>
        public AsignacionMateriaGrupo AsignacionMateriaEliminar(int indice)
        {
            return this.asignacionMaterias.Eliminar(indice);
        }
        /// <summary>
        /// Obtiene el numero de elementos de la lista de AsignacionAlumnoGrupo
        /// </summary>
        public int AsignacionMateriaElementos()
        {
            return this.asignacionMaterias.Elementos();
        }
        /// <summary>
        /// Obtiene el numero completo de elementos de la lista de AsignacionAlumnoGrupo
        /// </summary>
        public int AsignacionMateriaElementosTodos()
        {
            return this.asignacionMaterias.ElementosTodos();
        }
        /// <summary>
        /// Elimina todos los elementos del lista de AsignacionAlumnoGrupo
        /// </summary>
        public void AsignacionMateriaLimpiar()
        {
            this.asignacionMaterias.Limpiar();
        }
        /// <summary>
        /// Obtener el estado del AsignacionAlumnoGrupo
        /// </summary>
        public EObjetoEstado AsignacionMateriaEstado(AsignacionMateriaGrupo asignacionMateria)
        {
            return this.asignacionMaterias.Estado(asignacionMateria);
        }
        /// <summary>
        /// Obtener el  objeto original del AsignacionAlumnoGrupo
        /// </summary>
        public AsignacionMateriaGrupo AsignacionMateriaOriginal(AsignacionMateriaGrupo asignacionMateria)
        {
            return this.asignacionMaterias.Original(asignacionMateria);
        }
        /// <summary>
        /// Definir la expresión que identificara a la AsignacionAlumnoGrupo
        /// </summary>
        private static Func<AsignacionMateriaGrupo, bool> AsignacionMateriaIdentificar(AsignacionMateriaGrupo asignacionMateria)
        {
            return (uo => uo.AsignacionMateriaGrupoID == asignacionMateria.AsignacionMateriaGrupoID);
        }
    }
}
