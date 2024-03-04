using System;
using System.Text;
using System.Collections.Generic;
using GP.SocialEngine.Interfaces;

namespace POV.Reactivos.BO
{
    /// <summary>
    /// Reactivo
    /// </summary>
    public class Reactivo : IAppSocial, ISocialProfile,ICloneable
    {
        private Guid? reactivoID;
        /// <summary>
        /// identificador del reactivo
        /// </summary>
        public Guid? ReactivoID
        {
            get { return this.reactivoID; }
            set { this.reactivoID = value; }
        }

        private string nombreReactivo;
        /// <summary>
        /// nombre del reactivo
        /// </summary>
        public string NombreReactivo
        {
            get { return this.nombreReactivo; }
            set { this.nombreReactivo = value; }
        }
        private string clave;
        /// <summary>
        /// clave del reactivo
        /// </summary>
        public string Clave
        {
            get { return this.clave; }
            set { this.clave = value; }        
        }
        private string plantillaReactivo;
        /// <summary>
        /// plantilla del reactivo
        /// </summary>
        public string PlantillaReactivo
        {
            get { return this.plantillaReactivo; }
            set { this.plantillaReactivo = value; }
        }

        private int? numeroIntentos;
        /// <summary>
        /// NumeroIntentos del Reactivo
        /// </summary>
        public int? NumeroIntentos
        {
            get { return this.numeroIntentos; }
            set { this.numeroIntentos = value; }
        }

        private DateTime? fechaRegistro;
        /// <summary>
        /// 
        /// </summary>
        public DateTime? FechaRegistro 
        {
            get { return this.fechaRegistro; }
            set { this.fechaRegistro = value; }
        }

        private string descripcion;
        /// <summary>
        /// descripcion del reactivo
        /// </summary>
        public string Descripcion 
        {
            get { return this.descripcion; }
            set { this.descripcion = value; }
        }

        private bool? activo;
        /// <summary>
        /// estado activo
        /// </summary>
        public bool? Activo 
        {
            get { return this.activo; }
            set { this.activo = value; }
        }
        private  bool? asignado;
        /// <summary>
        /// estado asignado
        /// </summary>
        public bool? Asignado
        {
            get { return this.asignado; }
            set { this.asignado = value; }
        }

        private List<Pregunta> preguntas;
        /// <summary>
        /// preguntas del reactivo
        /// </summary>
        public List<Pregunta> Preguntas
        {
            get { return this.preguntas; }
            set { this.preguntas = value; }
        }
        private decimal? valor;
        /// <summary>
        /// valor del reactivo
        /// </summary>
        public decimal? Valor
        {
            get { return this.valor; }
            set { this.valor = value; }
        }


        private ETipoReactivo? tipoReactivo;
        /// <summary>
        /// 
        /// </summary>
        public ETipoReactivo? TipoReactivo 
        {
            get { return this.tipoReactivo; }
            set { this.tipoReactivo = value; }
        }

        private EPresentacionPlantilla? presentacionPlantilla;
        /// <summary>
        /// Presentación del Reactivo.
        /// </summary>
        public EPresentacionPlantilla? PresentacionPlantilla
        {
            get { return this.presentacionPlantilla; }
            set { this.presentacionPlantilla = value; }
        }

        public short? ToShortToTipoReactivo
        {
            get { return (short)this.tipoReactivo; }
            set{ this.tipoReactivo= (ETipoReactivo)value;}
        }
        private Caracteristicas caracteristicas;
        /// <summary>
        /// Característica de Reactivo
        /// </summary>
        public Caracteristicas Caracteristicas
        {
            get { return caracteristicas; }
            set { caracteristicas = value; }
        }

        public string GetNombreAplicacion()
        {
            return this.NombreReactivo;
        }
        public string GetImagen()
        {
            String imageName = String.Empty; 
            return imageName;
        }

        public string GetInformacionActual()
        {
            throw new NotImplementedException();
        }
        public string GetUrlApp()
        {
            String urlApp = String.Empty;
             
            return urlApp;
        }
        public string GetAppKey()
        {
            if (this.reactivoID != null)
                return this.ReactivoID.Value.ToString("D");
            else
                return null;
        }

        public object Clone()
        {
            Reactivo nuevo = (Reactivo)this.MemberwiseClone();

            if(Preguntas!=null)
            {
                nuevo.Preguntas = new List<Pregunta>();
                foreach (Pregunta aPregunta in Preguntas)
                            nuevo.Preguntas.Add((Pregunta) aPregunta.Clone());
            }
            if (nuevo.tipoReactivo == ETipoReactivo.ModeloGenerico)
            {
                nuevo.Caracteristicas = new CaracteristicasModeloGenerico();
                (nuevo.Caracteristicas as CaracteristicasModeloGenerico).Modelo = (this.caracteristicas as CaracteristicasModeloGenerico).Modelo;
                (nuevo.Caracteristicas as CaracteristicasModeloGenerico).Clasificador = (this.caracteristicas as CaracteristicasModeloGenerico).Clasificador;
            }
            
            return nuevo;
        }

        private int? grupo;
        /// <summary>
        /// Objeto 'Reactivo' relacionado con el registro
        /// </summary>
        public int? Grupo
        {
            get { return this.grupo; }
            set { this.grupo = value; }
        }
    }
}
