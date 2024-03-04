using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Framework.Base.DataAccess;
using POV.Comun.BO;
using POV.Comun.DAO;

namespace POV.Comun.Service
{ 
   /// <summary>
   /// Servicios para Acceder a las Ciudades
   /// </summary>
   public class CiudadCtrl { 
      /// <summary>
      /// Crea un registro de Ciudad en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="ciudad">Ciudad que desea crear</param>
      public void Insert(IDataContext dctx, Ciudad ciudad){
          try
          {
              if (!ValidateDataForInsert(dctx, new Ciudad { Nombre = ciudad.Nombre, Estado = new Estado { EstadoID = ciudad.Estado.EstadoID } },ciudad))
              {
                  throw new DuplicateNameException("El nombre de la Ciudad para este Estado ya se encuentra configurado en el sistema, por favor verifique.");
              }
              else
              {
                  CiudadInsHlp da = new CiudadInsHlp();
                  da.Action(dctx, ciudad);
              }
          }
          catch (Exception ex)
          {
              throw new DuplicateNameException(ex.Message);
          }        
      }
      /// <summary>
      /// Consulta registros de Ciudad en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="ciudad">Ciudad que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de Ciudad generada por la consulta</returns>
      public DataSet Retrieve(IDataContext dctx, Ciudad ciudad){
         CiudadRetHlp da = new CiudadRetHlp();
         DataSet ds = da.Action(dctx, ciudad);
         return ds;
      }
      /// <summary>
      /// Actualiza de manera optimista un registro de Ciudad en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="ciudad">Ciudad que tiene los datos nuevos</param>
      /// <param name="anterior">Ciudad que tiene los datos anteriores</param>
      public void Update(IDataContext dctx, Ciudad ciudad, Ciudad previous){
          try
          {
              if (!ValidateDataForInsert(dctx, new Ciudad { Nombre = ciudad.Nombre, Estado = new Estado { EstadoID = ciudad.Estado.EstadoID } },previous))
              {
                  throw new DuplicateNameException("El nombre de la Ciudad para este Estado ya se encuentra configurado en el sistema, por favor verifique.");
              }
              else
              {
                  CiudadUpdHlp da = new CiudadUpdHlp();
                  da.Action(dctx, ciudad, previous);
              }
          }
          catch (Exception ex)
          {
              throw new DuplicateNameException(ex.Message);
          }         
      }
      /// <summary>
      /// Elimina un registro de Ciudad en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="ciudad">Ciudad que desea eliminar</param>
      public void Delete(IDataContext dctx, Ciudad ciudad){
         CiudadDelHlp da = new CiudadDelHlp();
         da.Action(dctx, ciudad);
      }
      /// <summary>
      /// Crea un objeto de Ciudad a partir de los datos del último DataRow del DataSet.
      /// </summary>
      /// <param name="ds">El DataSet que contiene la información de Ciudad</param>
      /// <returns>Un objeto de Ciudad creado a partir de los datos</returns>
      public Ciudad LastDataRowToCiudad(DataSet ds) {
         if (!ds.Tables.Contains("Ciudad"))
            throw new Exception("LastDataRowToCiudad: DataSet no tiene la tabla Ciudad");
         int index = ds.Tables["Ciudad"].Rows.Count;
         if (index < 1)
            throw new Exception("LastDataRowToCiudad: El DataSet no tiene filas");
         return this.DataRowToCiudad(ds.Tables["Ciudad"].Rows[index - 1]);
      }
      /// <summary>
      /// Crea un objeto de Ciudad a partir de los datos de un DataRow.
      /// </summary>
      /// <param name="row">El DataRow que contiene la información de Ciudad</param>
      /// <returns>Un objeto de Ciudad creado a partir de los datos</returns>
      public Ciudad DataRowToCiudad(DataRow row){
         Ciudad ciudad = new Ciudad();
          ciudad.Estado = new Estado();
         if (row.IsNull("CiudadID"))
            ciudad.CiudadID = null;
         else
            ciudad.CiudadID = (int)Convert.ChangeType(row["CiudadID"], typeof(int));
         if (row.IsNull("Nombre"))
            ciudad.Nombre = null;
         else
            ciudad.Nombre = (string)Convert.ChangeType(row["Nombre"], typeof(string));
         if (row.IsNull("Codigo"))
             ciudad.Codigo = null;
         else
             ciudad.Codigo = (string)Convert.ChangeType(row["Codigo"], typeof(string));
         if (row.IsNull("FechaRegistro"))
            ciudad.FechaRegistro = null;
         else
            ciudad.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
         if (row.IsNull("EstadoID"))
            ciudad.Estado.EstadoID = null;
         else
            ciudad.Estado.EstadoID = (int)Convert.ChangeType(row["EstadoID"], typeof(int));
         return ciudad;
      }
      /// <summary>
      /// Valida si la Ciudad a insertar ya esta configurada en el sistema
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="ciudad">Ciudad a insertar</param>
      /// <returns>True si no existe la Ciudad</returns>
      private bool ValidateDataForInsert(IDataContext dctx, Ciudad ciudad, Ciudad previous)
      {
          DataSet ds = this.Retrieve(dctx, ciudad);
          if (ds.Tables["Ciudad"].Rows.Count > 0)
          {
              if (previous.CiudadID == null)
                  return false;
              Ciudad objectDB = LastDataRowToCiudad(ds);
              if (objectDB.CiudadID != previous.CiudadID)
                  return false;
          }
          return true;
      }
   } 
}
