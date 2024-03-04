using System;
using POV.Comun.BO;
using POV.Localizacion.BO;
using System.Collections.Generic;

namespace POV.CentroEducativo.BO
{
    /// <summary>
    /// Catalogo de escuelas disponibles en el sistema
    /// </summary>
    public class Escuela : ICloneable
    {
        public Escuela()
        {
            this.grupos = new ObjetoListado<Grupo>();
            this.asignacionDocentes = new ObjetoListado<AsignacionDocenteEscuela>();
            this.asignacionEspecialista = new ObjetoListado<AsignacionEspecialistaEscuela>();

        }

        public Escuela(IEnumerable<Grupo> grupos, IEnumerable<AsignacionDocenteEscuela> asignacionDocentes)
        {
            this.grupos = new ObjetoListado<Grupo>(grupos);
            this.asignacionDocentes = new ObjetoListado<AsignacionDocenteEscuela>(asignacionDocentes);
        }

        private int? escuelaID;
        /// <summary>
        /// Identificador autonumerico de sistema para la escuela
        /// </summary>
        public int? EscuelaID
        {
            get { return this.escuelaID; }
            set { this.escuelaID = value; }
        }
        private string clave;
        /// <summary>
        /// Clave para la escuela
        /// </summary>
        public string Clave
        {
            get { return this.clave; }
            set { this.clave = value; }
        }
        private string nombreEscuela;
        /// <summary>
        /// Nombre de la escuela
        /// </summary>
        public string NombreEscuela
        {
            get { return this.nombreEscuela; }
            set { this.nombreEscuela = value; }
        }
        private DateTime? fechaRegistro;
        /// <summary>
        /// Fecha de registro de la escuela
        /// </summary>
        public DateTime? FechaRegistro
        {
            get { return this.fechaRegistro; }
            set { this.fechaRegistro = value; }
        }
        private bool? estatus;
        /// <summary>
        /// Estatus de la escuela
        /// </summary>
        public bool? Estatus
        {
            get { return this.estatus; }
            set { this.estatus = value; }
        }
        private ETurno? turno;
        /// <summary>
        /// Turno de la escuela
        /// </summary>
        public ETurno? Turno
        {
            get { return this.turno; }
            set { this.turno = value; }
        }
        public short? ToShortTurno
        {
            get { return (short)this.turno; }
            set
            {
                this.turno = (ETurno)value;
            }
        }
        private Ubicacion ubicacion;
        /// <summary>
        /// Ubicacion de la escuela
        /// </summary>
        public Ubicacion Ubicacion
        {
            get { return this.ubicacion; }
            set { this.ubicacion = value; }
        }
        /// <summary>
        /// Tipo de Servicion de la alumno
        /// </summary>
        private TipoServicio tipoServicio;
        /// <summary>
        /// Tipo de servicio de la escuela
        /// </summary>
        public TipoServicio TipoServicio
        {
            get { return this.tipoServicio; }
            set { this.tipoServicio = value; }
        }
        private Zona zonaID;
        /// <summary>
        /// Zona a la que pertenece la escuela
        /// </summary>
        public Zona ZonaID
        {
            get { return this.zonaID; }
            set { this.zonaID = value; }
        }
        private Director directorID;
        /// <summary>
        /// Director asignado a la escuela
        /// </summary>
        public Director DirectorID
        {
            get { return this.directorID; }
            set { this.directorID = value; }
        }
        private EAmbito? ambito;
        /// <summary>
        /// Ambito de la escuela
        /// </summary>
        public EAmbito? Ambito
        {
            get { return ambito; }
            set { ambito = value; }
        }
        private EControl? control;
        /// <summary>
        /// Control de la escuela
        /// </summary>
        public EControl? Control
        {
            get { return control; }
            set { control = value; }
        }

        public short? ToShortAmbito
        {
            get { return (short)this.ambito; }
            set { this.ambito = (EAmbito)value; }
        }
        public short? ToShortControl
        {
            get { return (short)this.control; }
            set { this.control = (EControl)value; }
        }


        private CentroComputo centroComputo;

        public CentroComputo CentroComputo
        {
            get { return this.centroComputo; }
            set { this.centroComputo = value; }
        }
        private ObjetoListado<Grupo> grupos;
        public IEnumerable<Grupo> Grupos
        {
            get { return this.grupos.Objetos; }
        }
        /// <summary>
        /// Agrega Grupo
        /// </summary>
        public Grupo GrupoAgregar(Grupo grupo)
        {
            return this.grupos.Agregar(GrupoIdentificar(grupo), grupo);
        }
        /// <summary>
        /// Obtener Grupo basado en el indice
        /// </summary>
        public Grupo GrupoObtener(int indice)
        {
            return this.grupos.Obtener(indice);
        }
        /// <summary>
        /// Elimina Grupo
        /// </summary>
        public Grupo GrupoEliminar(Grupo grupo)
        {
            return this.grupos.Eliminar(GrupoIdentificar(grupo), grupo);
        }
        /// <summary>
        /// Elimina Grupo basado en el indice
        /// </summary>
        public Grupo GrupoEliminar(int indice)
        {
            return this.grupos.Eliminar(indice);
        }
        /// <summary>
        /// Obtiene el numero de elementos de la lista de Grupo
        /// </summary>
        public int GrupoElementos()
        {
            return this.grupos.Elementos();
        }
        /// <summary>
        /// Obtiene el numero completo de elementos de la lista de Grupo
        /// </summary>
        public int GrupoElementosTodos()
        {
            return this.grupos.ElementosTodos();
        }
        /// <summary>
        /// Elimina todos los elementos del lista de Grupo
        /// </summary>
        public void GrupoLimpiar()
        {
            this.grupos.Limpiar();
        }
        /// <summary>
        /// Obtener el estado del Grupo
        /// </summary>
        public EObjetoEstado GrupoEstado(Grupo grupo)
        {
            return this.grupos.Estado(grupo);
        }
        /// <summary>
        /// Obtener el  objeto original del Grupo
        /// </summary>
        public Grupo GrupoOriginal(Grupo grupo)
        {
            return this.grupos.Original(grupo);
        }
        /// <summary>
        /// Definir la expresión que identificara al Grupo
        /// </summary>
        private static Func<Grupo, bool> GrupoIdentificar(Grupo grupo)
        {
            return (uo => uo.GrupoID == grupo.GrupoID);
        }

        private ObjetoListado<AsignacionDocenteEscuela> asignacionDocentes;
        public IEnumerable<AsignacionDocenteEscuela> AsignacionDocentes
        {
            get { return this.asignacionDocentes.Objetos; }
        }

        private ObjetoListado<AsignacionEspecialistaEscuela> asignacionEspecialista;

        public IEnumerable<AsignacionEspecialistaEscuela> AsignacionEspecialista
        {
            get { return asignacionEspecialista.Objetos; }
        }
        /// <summary>
        /// Agrega AsignacionDocenteEscuela
        /// </summary>
        public AsignacionDocenteEscuela AsignacionDocenteAgregar(AsignacionDocenteEscuela asignacionDocente)
        {
            return this.asignacionDocentes.Agregar(AsignacionDocenteIdentificar(asignacionDocente), asignacionDocente);
        }
        /// <summary>
        /// Obtener AsignacionDocenteEscuela basado en el indice
        /// </summary>
        public AsignacionDocenteEscuela AsignacionDocenteObtener(int indice)
        {
            return this.asignacionDocentes.Obtener(indice);
        }
        /// <summary>
        /// Elimina AsignacionDocenteEscuela
        /// </summary>
        public AsignacionDocenteEscuela AsignacionDocenteEliminar(AsignacionDocenteEscuela asignacionDocente)
        {
            return this.asignacionDocentes.Eliminar(AsignacionDocenteIdentificar(asignacionDocente), asignacionDocente);
        }
        /// <summary>
        /// Elimina AsignacionDocenteEscuela basado en el indice
        /// </summary>
        public AsignacionDocenteEscuela AsignacionDocenteEliminar(int indice)
        {
            return this.asignacionDocentes.Eliminar(indice);
        }
        /// <summary>
        /// Obtiene el numero de elementos de la lista de AsignacionDocenteEscuela
        /// </summary>
        public int AsignacionDocenteElementos()
        {
            return this.asignacionDocentes.Elementos();
        }
        /// <summary>
        /// Obtiene el numero completo de elementos de la lista de AsignacionDocenteEscuela
        /// </summary>
        public int AsignacionDocenteElementosTodos()
        {
            return this.asignacionDocentes.ElementosTodos();
        }
        /// <summary>
        /// Elimina todos los elementos del lista de AsignacionDocenteEscuela
        /// </summary>
        public void AsignacionDocenteLimpiar()
        {
            this.asignacionDocentes.Limpiar();
        }
        /// <summary>
        /// Obtener el estado del AsignacionDocenteEscuela
        /// </summary>
        public EObjetoEstado AsignacionDocenteEstado(AsignacionDocenteEscuela asignacionDocente)
        {
            return this.asignacionDocentes.Estado(asignacionDocente);
        }

        public EObjetoEstado AsignacionEspecialistaEstado(AsignacionEspecialistaEscuela asignacionEspecialista)
        {
            return this.asignacionEspecialista.Estado(asignacionEspecialista);
        }
        /// <summary>
        /// Obtener el  objeto original del AsignacionDocenteEscuela
        /// </summary>
        public AsignacionDocenteEscuela AsignacionDocenteOriginal(AsignacionDocenteEscuela asignacionDocente)
        {
            return this.asignacionDocentes.Original(asignacionDocente);
        }
        /// <summary>
        /// Definir la expresión que identificara a la AsignacionDocenteEscuela
        /// </summary>
        private static Func<AsignacionDocenteEscuela, bool> AsignacionDocenteIdentificar(AsignacionDocenteEscuela asignacionDocente)
        {
            return (uo => uo.AsignacionDocenteEscuelaID == asignacionDocente.AsignacionDocenteEscuelaID);
        }

        private static Func<AsignacionEspecialistaEscuela, bool> AsignacionEspecialistaIdentificar(AsignacionEspecialistaEscuela asignacionEspecialista)
        {
            return (uo => uo.AsignacionEspecialistaEscuelaID == asignacionEspecialista.AsignacionEspecialistaEscuelaID);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public object CloneAll()
        {
            Escuela clone = (Escuela)this.Clone();
            clone.Ubicacion = (Ubicacion)this.ubicacion.Clone();
            clone.TipoServicio = (TipoServicio)this.tipoServicio.Clone();
            clone.ZonaID = (Zona)this.zonaID;
            clone.DirectorID = (Director)this.directorID;

            return clone;
        }
    }
}
