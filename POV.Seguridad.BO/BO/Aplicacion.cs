namespace POV.Seguridad.BO { 
   /// <summary>
   /// Catalogo de Aplicaciones disponibles
   /// </summary>
   public class Aplicacion {
       private int? aplicacionID;
       /// <summary>
       /// Identificador autonumerico de Aplicacion
       /// </summary>
       public int? AplicacionID
       {
           get { return this.aplicacionID; }
           set { this.aplicacionID = value; }
       }
       private string nombre;
       /// <summary>
       /// Nombre descriptivo de la Aplicacion
       /// </summary>
       public string Nombre
       {
           get { return this.nombre; }
           set { this.nombre = value; }
       }
       private bool? activo;
       /// <summary>
       /// Estatus de la Aplicacion
       /// </summary>
       public bool? Activo
       {
           get { return this.activo; }
           set { this.activo = value; }
       }
   } 
}
