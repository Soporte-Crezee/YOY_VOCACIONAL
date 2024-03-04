using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Localizacion.BO;
using POV.Localizacion.DAO;
using POV.Comun.Service;
using POV.Comun.BO;

namespace POV.Localizacion.Service { 
   /// <summary>
   /// Controlador del objeto Ubicacion
   /// </summary>
   public class UbicacionCtrl { 
      /// <summary>
      /// Consulta registros de UbicacionRetHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="ubicacionRetHlp">UbicacionRetHlp que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de UbicacionRetHlp generada por la consulta</returns>
      public DataSet Retrieve(IDataContext dctx, Ubicacion ubicacion){
         UbicacionRetHlp da = new UbicacionRetHlp();
         DataSet ds = da.Action(dctx, ubicacion);
         return ds;
      }

      public DataSet RetrieveExacto(IDataContext dctx, Ubicacion ubicacion)
      {
          UbicacionExactoRetHlp da = new UbicacionExactoRetHlp();
          DataSet ds = da.Action(dctx, ubicacion);
          return ds;
      }

      /// <summary>
      /// Crea un registro de UbicacionInsHlp en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="ubicacionInsHlp">UbicacionInsHlp que desea crear</param>
      public void Insert(IDataContext dctx, Ubicacion  ubicacion){
         UbicacionInsHlp da = new UbicacionInsHlp();
         da.Action(dctx,  ubicacion);
      }
      /// <summary>
      /// Actualiza de manera optimista un registro de UbicacionUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="ubicacionUpdHlp">UbicacionUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">UbicacionUpdHlp que tiene los datos anteriores</param>
      public void Update(IDataContext dctx, Ubicacion  ubicacion, Ubicacion previous){
         UbicacionUpdHlp da = new UbicacionUpdHlp();
         da.Action(dctx,  ubicacion, previous);
      }
      /// <summary>
      /// Regresa un resgistro completo de Ubicacion
      /// </summary>
      public Ubicacion RetrieveComplete(IDataContext dctx, Ubicacion  ubicacion){
          PaisCtrl paisCtrl = new PaisCtrl();
          EstadoCtrl estadoCtrl = new EstadoCtrl();
          CiudadCtrl ciudadCtrl = new CiudadCtrl( );
          LocalidadCtrl localidadCtrl = new LocalidadCtrl();
          ColoniaCtrl coloniaCtrl = new ColoniaCtrl( );
          try{
          ubicacion=LastDataRowToUbicacion(Retrieve(dctx, new Ubicacion {UbicacionID = ubicacion.UbicacionID}));
          //traer los objetos agregados
          ubicacion.Pais=paisCtrl.LastDataRowToPais(paisCtrl.Retrieve(dctx, new Pais { PaisID = ubicacion.Pais.PaisID }));
          ubicacion.Estado=estadoCtrl.LastDataRowToEstado(estadoCtrl.Retrieve(dctx, new Estado { EstadoID = ubicacion.Estado.EstadoID }));
          ubicacion.Ciudad=ciudadCtrl.LastDataRowToCiudad(ciudadCtrl.Retrieve(dctx, new Ciudad { CiudadID = ubicacion.Ciudad.CiudadID }));     
                

          if ( ubicacion.Localidad.LocalidadID!=null )
          {
              System.Data.DataSet dsLocaliad = localidadCtrl.Retrieve( dctx , new Localidad { LocalidadID = ubicacion.Localidad.LocalidadID } );

              if ( dsLocaliad.Tables[ "Localidad" ].Rows.Count > 0 )
              {
                  ubicacion.Localidad = localidadCtrl.LastDataRowToLocalidad( dsLocaliad );

                  if (ubicacion.Colonia.ColoniaID != null)
                  {
                      DataSet dsColonia = coloniaCtrl.Retrieve(dctx, new Colonia { ColoniaID = ubicacion.Colonia.ColoniaID, Localidad = ubicacion.Localidad });

                      if (dsColonia.Tables["Colonia"].Rows.Count > 0)
                      {
                          ubicacion.Colonia = coloniaCtrl.LastDataRowToColonia(dsColonia);
                      }
                  }
              }
          }
                    
          

          
          
          return ubicacion;
          }
          catch(Exception ex){
          throw new Exception(ex.Message);
          }
      }

    /// <summary>
    /// Verifica si existe una ubicacion para ese pais, estado, municipio  y ciudad.
    /// </summary>
    /// <param name="ubicacion">Objeto que contiene información del pais, estado, municipio y ciudad</param>
    /// <returns>Si la ubicacion existe regresa un true</returns>
      public Boolean ValidarExisteUbicacion(IDataContext dctx, Ubicacion ubicacion)
      {
          DataSet ds = this.Retrieve(dctx, ubicacion);
          if (ds.Tables["Ubicacion"].Rows.Count <= 0)
              return false;
          else
              return true;
      }


      /// <summary>
      /// Crea un objeto de Ubicacion a partir de los datos del último DataRow del DataSet.
      /// </summary>
      /// <param name="ds">El DataSet que contiene la información de Ubicacion</param>
      /// <returns>Un objeto de Ubicacion creado a partir de los datos</returns>
      public Ubicacion LastDataRowToUbicacion(DataSet ds) {
         if (!ds.Tables.Contains("Ubicacion"))
            throw new Exception("LastDataRowToUbicacion: DataSet no tiene la tabla Ubicacion");
         int index = ds.Tables["Ubicacion"].Rows.Count;
         if (index < 1)
            throw new Exception("LastDataRowToUbicacion: El DataSet no tiene filas");
         return this.DataRowToUbicacion(ds.Tables["Ubicacion"].Rows[index - 1]);
      }
      /// <summary>
      /// Crea un objeto de Ubicacion a partir de los datos de un DataRow.
      /// </summary>
      /// <param name="row">El DataRow que contiene la información de Ubicacion</param>
      /// <returns>Un objeto de Ubicacion creado a partir de los datos</returns>
      public Ubicacion DataRowToUbicacion(DataRow row){
         Ubicacion ubicacion = new Ubicacion();

         if ( ubicacion.Pais == null )
         {
             ubicacion.Pais = new Pais( );
         }
         if ( ubicacion.Estado == null )
         {
             ubicacion.Estado = new Estado( );
         }
         if ( ubicacion.Ciudad == null )
         {
             ubicacion.Ciudad = new Ciudad( );
         }
         if ( ubicacion.Localidad == null )
         {
             ubicacion.Localidad = new Localidad( );
         }
         if ( ubicacion.Colonia == null )
         {
             ubicacion.Colonia = new Colonia( );
         }

         if (row.IsNull("UbicacionID"))
            ubicacion.UbicacionID = null;
         else
            ubicacion.UbicacionID = (int)Convert.ChangeType(row["UbicacionID"], typeof(int));
         if (row.IsNull("PaisID"))
            ubicacion.Pais.PaisID = null;
         else
            ubicacion.Pais.PaisID = (int)Convert.ChangeType(row["PaisID"], typeof(int));
         if (row.IsNull("EstadoID"))
            ubicacion.Estado.EstadoID = null;
         else
            ubicacion.Estado.EstadoID = (int)Convert.ChangeType(row["EstadoID"], typeof(int));
         if (row.IsNull("CiudadID"))
            ubicacion.Ciudad.CiudadID = null;
         else
            ubicacion.Ciudad.CiudadID = (int)Convert.ChangeType(row["CiudadID"], typeof(int));
         if (row.IsNull("LocalidadID"))
            ubicacion.Localidad.LocalidadID = null;
         else
            ubicacion.Localidad.LocalidadID = (int)Convert.ChangeType(row["LocalidadID"], typeof(int));
         if (row.IsNull("ColoniaID"))
            ubicacion.Colonia.ColoniaID = null;
         else
            ubicacion.Colonia.ColoniaID = (int)Convert.ChangeType(row["ColoniaID"], typeof(int));
         if ( row.IsNull( "FechaRegistro" ) )
             ubicacion.FechaRegistro = null;
         else
             ubicacion.FechaRegistro = ( DateTime )Convert.ChangeType( row[ "FechaRegistro" ] , typeof( DateTime ) );
         return ubicacion;
      }
   } 
}
