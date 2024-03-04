using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using POV.Modelo.BO;
using POV.Comun.BO;

namespace POV.Prueba.BO
{
    /// <summary>
    /// Representa una prueba abstracta
    /// </summary>
    public abstract class APrueba
    {
        public APrueba()
        {
            this.listaPuntajes = new ObjetoListado<APuntaje>();
        }
        public APrueba(IEnumerable<APuntaje> listaPuntajes)
        {
            this.listaPuntajes = new ObjetoListado<APuntaje>(listaPuntajes);
        }

        private int? pruebaID;
        /// <summary>
        /// Identificador de la prueba
        /// </summary>
        public int? PruebaID
        {
            get { return this.pruebaID; }
            set { this.pruebaID = value; }
        }
        private string clave;
        /// <summary>
        /// Clave de la prueba
        /// </summary>
        public string Clave
        {
            get { return this.clave; }
            set { this.clave = value; }
        }
        private string nombre;
        /// <summary>
        /// Nombre  de la prueba
        /// </summary>
        public string Nombre
        {
            get { return this.nombre; }
            set { this.nombre = value; }
        }
        private string instrucciones;
        /// <summary>
        /// Instrucciones de la prueba
        /// </summary>
        public string Instrucciones
        {
            get { return this.instrucciones; }
            set { this.instrucciones = value; }
        }
        private DateTime? fechaRegistro;
        /// <summary>
        /// Fecha de registro
        /// </summary>
        public DateTime? FechaRegistro
        {
            get { return this.fechaRegistro; }
            set { this.fechaRegistro = value; }
        }
        private bool? esDiagnostica;
        /// <summary>
        /// Es prueba diagnostica
        /// </summary>
        public bool? EsDiagnostica
        {
            get { return this.esDiagnostica; }
            set { this.esDiagnostica = value; }
        }
        private AModelo modelo;
        /// <summary>
        /// Modelo de la prueba
        /// </summary>
        public AModelo Modelo
        {
            get { return this.modelo; }
            set { this.modelo = value; }
        }
        private EEstadoLiberacionPrueba? estadoLiberacionPrueba;
        public EEstadoLiberacionPrueba? EstadoLiberacionPrueba
        {
            get { return this.estadoLiberacionPrueba; }
            set { this.estadoLiberacionPrueba = value; }
        }
        private ObjetoListado<APuntaje> listaPuntajes;
        /// <summary>
        /// Lista de puntajes
        /// </summary>
        public IEnumerable<APuntaje> ListaPuntajes
        {
            get { return this.listaPuntajes.Objetos; }
        }

        /// <summary>
        /// Agrega Puntaje
        /// </summary>
        public APuntaje PuntajeAgregar(APuntaje puntaje)
        {
            return this.listaPuntajes.Agregar(PuntajeIdentificar(puntaje), puntaje);
        }
        /// <summary>
        /// Obtener Puntaje basado en el indice
        /// </summary>
        public APuntaje PuntajeObtener(int indice)
        {
            return this.listaPuntajes.Obtener(indice);
        }
        /// <summary>
        /// Elimina Puntaje
        /// </summary>
        public APuntaje PuntajeEliminar(APuntaje puntaje)
        {
            return this.listaPuntajes.Eliminar(PuntajeIdentificar(puntaje), puntaje);
        }
        /// <summary>
        /// Elimina Puntaje basado en el indice
        /// </summary>
        public APuntaje PuntajeEliminar(int indice)
        {
            return this.listaPuntajes.Eliminar(indice);
        }
        /// <summary>
        /// Obtiene el numero de elementos de la lista de Puntaje
        /// </summary>
        public int PuntajeElementos()
        {
            return this.listaPuntajes.Elementos();
        }
        /// <summary>
        /// Obtiene el numero completo de elementos de la lista de Puntaje
        /// </summary>
        public int PuntajeElementosTodos()
        {
            return this.listaPuntajes.ElementosTodos();
        }
        /// <summary>
        /// Elimina todos los elementos del lista de Puntaje
        /// </summary>
        public void PuntajeLimpiar()
        {
            this.listaPuntajes.Limpiar();
        }
        /// <summary>
        /// Obtener el estado del Puntaje
        /// </summary>
        public EObjetoEstado PuntajeEstado(APuntaje puntaje)
        {
            return this.listaPuntajes.Estado(puntaje);
        }
        /// <summary>
        /// Obtener el  objeto original del Puntaje
        /// </summary>
        public APuntaje PuntajeOriginal(APuntaje puntaje)
        {
            return this.listaPuntajes.Original(puntaje);
        }
        /// <summary>
        /// Definir la expresión que identificara al Puntaje
        /// </summary>
        private static Func<APuntaje, bool> PuntajeIdentificar(APuntaje puntaje)
        {
            return (uo => puntaje.PuntajeID != null && uo.PuntajeID == puntaje.PuntajeID);
        }

        /// <summary>
        /// Determinar el tipo de prueba
        /// 0 Dinamica,
        /// 1 Felder,
        /// 3 Estandarizada,
        /// </summary>
        public abstract ETipoPrueba TipoPrueba { get; }

        /// <summary>
        /// Determinar si la prueba es Premium ó Gratuita
        /// </summary>
        public Boolean? EsPremium { get; set; }

        /// <summary>
        /// Determinar el tipo de prueba
        /// 0 Dinamica,
        /// 4 HabitosEstudio,
        /// 5 Dominos.
        /// </summary>        
        public ETipoPruebaPresentacion TipoPruebaPresentacion { get; set; }
    }
}
