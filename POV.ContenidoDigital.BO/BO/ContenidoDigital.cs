using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Comun.BO;
using GP.SocialEngine.Interfaces;

namespace POV.ContenidosDigital.BO
{
    /// <summary>
    /// Representa un contenido digital
    /// </summary>
    public class ContenidoDigital : ICloneable, IAppSocial
    {

        public ContenidoDigital() { this.listaURLContenido = new ObjetoListado<URLContenido>(); }
        public ContenidoDigital(IEnumerable<URLContenido> urlContenido) { this.listaURLContenido = new ObjetoListado<URLContenido>(urlContenido); }
        private long? contenidoDigitalID;
        /// <summary>
        /// Identificador del contenido digital
        /// </summary>
        public long? ContenidoDigitalID
        {
            get { return this.contenidoDigitalID; }
            set { this.contenidoDigitalID = value; }
        }
        private string clave;
        /// <summary>
        /// Clave del contenido
        /// </summary>
        public string Clave
        {
            get { return this.clave; }
            set { this.clave = value; }
        }
        private string nombre;
        /// <summary>
        /// Nombre del contenido digital
        /// </summary>
        public string Nombre
        {
            get { return this.nombre; }
            set { this.nombre = value; }
        }
        private bool? esInterno;
        /// <summary>
        /// Indica si el contenido es interno o externo
        /// </summary>
        public bool? EsInterno
        {
            get { return this.esInterno; }
            set { this.esInterno = value; }
        }
        private bool? esDescargable;
        /// <summary>
        /// Indica si el contenido es descargable o no
        /// </summary>
        public bool? EsDescargable
        {
            get { return this.esDescargable; }
            set { this.esDescargable = value; }
        }
        private InstitucionOrigen institucionOrigen;
        /// <summary>
        /// Institucion de origen del contenido
        /// </summary>
        public InstitucionOrigen InstitucionOrigen
        {
            get { return this.institucionOrigen; }
            set { this.institucionOrigen = value; }
        }
        private TipoDocumento tipoDocumento;
        /// <summary>
        /// Tipo de documento
        /// </summary>
        public virtual TipoDocumento TipoDocumento
        {
            get { return this.tipoDocumento; }
            set { this.tipoDocumento = value; }
        }
        

        private string tags;
        /// <summary>
        /// Etiquetas del contenido digital
        /// </summary>
        public string Tags
        {
            get { return this.tags; }
            set { this.tags = value; }
        }

        private EEstatusContenido? estatusContenido;
        /// <summary>
        /// Representa el estatus del contenido digital
        /// </summary>
        public EEstatusContenido? EstatusContenido
        {
            get { return this.estatusContenido; }
            set { this.estatusContenido = value; }
        }

        private DateTime? fechaRegistro;

        public DateTime? FechaRegistro
        {
            get { return fechaRegistro; }
            set { fechaRegistro = value; }
        }

        private ObjetoListado<URLContenido> listaURLContenido;
        /// <summary>
        /// Indica la lista de las referencias URL del contenido digital
        /// </summary>
        public IEnumerable<URLContenido> ListaURLContenido
        {
            get { return this.listaURLContenido.Objetos; }
            set { this.listaURLContenido = new ObjetoListado<URLContenido>(value); }
        }

        /// <summary>
        /// Propiedad auxiliar para el mapeo con Entity framework, para el framework plenumsoft utilizar la propiedad ListaURLContenido
        /// </summary>
        public List<URLContenido> URLContenidosProxy
        {
            get { return this.listaURLContenido.Objetos.ToList(); }
            set { this.listaURLContenido = new ObjetoListado<URLContenido>(value); }
        }

        public int? TipoDocumentoId { get; set; }
       
        /// <summary>
        /// Agrega URLContenido
        /// </summary>
        public URLContenido URLContenidoAgregar(URLContenido urlContenido)
        {
            URLContenido urlContenidoAux = this.listaURLContenido.Objetos.FirstOrDefault(item => item.EsPredeterminada.Value);
            if (urlContenidoAux != null && listaURLContenido.Estado(urlContenidoAux) != EObjetoEstado.ELIMINADO && urlContenido.EsPredeterminada.Value)
                throw new Exception("No es posible agregar la dirección,  ya existe una direccion predeterminada");
            return this.listaURLContenido.Agregar(URLContenidoIdentificar(urlContenido), urlContenido);
        }
        /// <summary>
        /// Obtener URLContenido basado en el indice
        /// </summary>
        public URLContenido URLContenidoObtener(int indice)
        {
            return this.listaURLContenido.Obtener(indice);
        }
        /// <summary>
        /// Elimina URLContenido
        /// </summary>
        public URLContenido RutaEliminar(URLContenido urlContenido)
        {
            return this.listaURLContenido.Eliminar(URLContenidoIdentificar(urlContenido), urlContenido);
        }
        /// <summary>
        /// Elimina URLContenido basado en el indice
        /// </summary>
        public URLContenido URLContenidoEliminar(int indice)
        {
            return this.listaURLContenido.Eliminar(indice);
        }
        /// <summary>
        /// Obtiene el numero de elementos de la lista de URLContenido
        /// </summary>
        public int URLContenidoElementos()
        {
            return this.listaURLContenido.Elementos();
        }
        /// <summary>
        /// Obtiene el numero completo de elementos de la lista de URLContenido
        /// </summary>
        public int URLContenidoElementosTodos()
        {
            return this.listaURLContenido.ElementosTodos();
        }
        /// <summary>
        /// Elimina todos los elementos del lista de URLContenido
        /// </summary>
        public void URLContenidoLimpiar()
        {
            this.listaURLContenido.Limpiar();
        }
        /// <summary>
        /// Obtener el estado de la URLContenido
        /// </summary>
        public EObjetoEstado URLContenidoEstado(URLContenido urlContenido)
        {
            return this.listaURLContenido.Estado(urlContenido);
        }
        /// <summary>
        /// Obtener el  objeto original del URLContenido
        /// </summary>
        public URLContenido URLContenidoOriginal(URLContenido urlContenido)
        {
            return this.listaURLContenido.Original(urlContenido);
        }
        /// <summary>
        /// Definir la expresión que identificara al URLContenido
        /// </summary>
        private static Func<URLContenido, bool> URLContenidoIdentificar(URLContenido urlContenido)
        {
            return (uo => urlContenido.URLContenidoID != null && uo.URLContenidoID == urlContenido.URLContenidoID);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public object CloneAll()
        {

            List<URLContenido> rutasCopy = new List<URLContenido>();


            foreach (URLContenido urlContenido in new List<URLContenido>(listaURLContenido.Objetos.ToList()))
            {
                rutasCopy.Add((URLContenido)urlContenido.Clone());
            }

            ContenidoDigital contenidoDigital = new ContenidoDigital(rutasCopy);
            contenidoDigital.ContenidoDigitalID = contenidoDigitalID;
            contenidoDigital.Clave = this.clave;
            contenidoDigital.Nombre = this.nombre;
            contenidoDigital.EsInterno = this.esInterno;
            contenidoDigital.EstatusContenido = this.estatusContenido;
            contenidoDigital.FechaRegistro = this.fechaRegistro;
            contenidoDigital.Tags = this.tags;
            contenidoDigital.EsDescargable = this.esDescargable;
            contenidoDigital.InstitucionOrigen = this.institucionOrigen.Clone() as InstitucionOrigen;
            contenidoDigital.TipoDocumento = this.tipoDocumento;
            return contenidoDigital;
        }
        

        public string GetNombreAplicacion()
        {
            throw new NotImplementedException();
        }

        public string GetImagen()
        {
            throw new NotImplementedException();
        }

        public string GetInformacionActual()
        {
            throw new NotImplementedException();
        }

        public string GetUrlApp()
        {
            throw new NotImplementedException();
        }

        public string GetAppKey()
        {
            throw new NotImplementedException();
        }
    }
}
