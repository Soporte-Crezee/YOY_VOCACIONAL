using System;
namespace POV.Comun.BO
{
    /// <summary>
    /// Refencia a Imagenes en la base de datos
    /// </summary>
    public class AdjuntoImagen: ICloneable
    {
        private long? adjuntoImagenID;
        /// <summary>
        /// Identificador autonumérico de la imagen
        /// </summary>
        public long? AdjuntoImagenID
        {
            get { return this.adjuntoImagenID; }
            set { this.adjuntoImagenID = value; }
        }
        private string extension;
        /// <summary>
        /// Extension de la imagen
        /// </summary>
        public string Extension
        {
            get { return this.extension; }
            set { this.extension = value; }
        }
        private string mIME;
        /// <summary>
        /// tipo MIME de la imagen
        /// </summary>
        public string MIME
        {
            get { return this.mIME; }
            set { this.mIME = value; }
        }
        private string nombreImagen;
        /// <summary>
        /// Nombre de la imagen
        /// </summary>
        public string NombreImagen
        {
            get { return this.nombreImagen; }
            set { this.nombreImagen = value; }
        }
        private string nombreThumb;
        /// <summary>
        /// Imagen minificada
        /// </summary>
        public string NombreThumb
        {
            get { return this.nombreThumb; }
            set { this.nombreThumb = value; }
        }
        private ECarpetaSistema? carpetaID;
        /// <summary>
        /// Carpeta donde se aloja la imagen
        /// </summary>
        public ECarpetaSistema? CarpetaID
        {
            get { return this.carpetaID; }
            set { this.carpetaID = value; }
        }
        private ETipoImagen tipoImagen;
        /// <summary>
        /// Tipo de la imagen
        /// </summary>
        public ETipoImagen TipoImagen
        {
            get { return this.tipoImagen; }
            set { this.tipoImagen = value; }
        }

        public string ThumbNailUrl
        {
            get { return folderUrl + "Thumbs/" + nombreThumb; }
        }
        public string ImageUrl
        {
            get { return folderUrl + "Normal/" + nombreImagen; }
        }

        private string folderUrl;
        public string FolderUrl
        {
            get { return folderUrl; }
            set { folderUrl = value; }
        }

        #region Miembros de ICloneable

        public object Clone ( )
        {
            return this.MemberwiseClone( );
        }

        #endregion
    }
}
