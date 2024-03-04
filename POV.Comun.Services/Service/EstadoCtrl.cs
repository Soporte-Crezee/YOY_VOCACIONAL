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
   /// Servicios para Acceder a los Estados
   /// </summary>
   public class EstadoCtrl { 
      /// <summary>
      /// Crea un registro de Estado en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="estado">Estado que desea crear</param>
      public void Insert(IDataContext dctx, Estado estado){
          try
          {
              if (!ValidateDataForInsert(dctx, new Estado { Nombre = estado.Nombre, Pais = new Pais { PaisID = estado.Pais.PaisID } },estado))
              {
                  throw new DuplicateNameException("El nombre del Estado para este País ya se encuentra configurado en el sistema, por favor verifique.");
              }
              else
              {
                  EstadoInsHlp da = new EstadoInsHlp();
                  da.Action(dctx, estado);
              }
          }
          catch (Exception ex)
          {
              throw new DuplicateNameException(ex.Message);
          }         
      }
      /// <summary>
      /// Consulta registros de Estado en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="estado">Estado que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de Estado generada por la consulta</returns>
      public DataSet Retrieve(IDataContext dctx, Estado estado){
         EstadoRetHlp da = new EstadoRetHlp();
         DataSet ds = da.Action(dctx, estado);
         return ds;
      }
      /// <summary>
      /// Actualiza de manera optimista un registro de Estado en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="estado">Estado que tiene los datos nuevos</param>
      /// <param name="anterior">Estado que tiene los datos anteriores</param>
      public void Update(IDataContext dctx, Estado estado, Estado previous){
          try
          {
              if (!ValidateDataForInsert(dctx, new Estado { Nombre = estado.Nombre, Pais = new Pais { PaisID = estado.Pais.PaisID } },previous))
              {
                  throw new DuplicateNameException("El nombre del Estado para este País ya se encuentra configurado en el sistema, por favor verifique.");
              }
              else
              {
                  EstadoUpdHlp da = new EstadoUpdHlp();
                  da.Action(dctx, estado, previous);
              }
          }
          catch (Exception ex)
          {
              throw new DuplicateNameException(ex.Message);
          }         
      }
      /// <summary>
      /// Elimina un registro de Estado en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="estado">Estado que desea eliminar</param>
      public void Delete(IDataContext dctx, Estado estado){
         EstadoDelHlp da = new EstadoDelHlp();
         da.Action(dctx, estado);
      }
      /// <summary>
      /// Crea un objeto de Estado a partir de los datos del último DataRow del DataSet.
      /// </summary>
      /// <param name="ds">El DataSet que contiene la información de Estado</param>
      /// <returns>Un objeto de Estado creado a partir de los datos</returns>
      public Estado LastDataRowToEstado(DataSet ds) {
         if (!ds.Tables.Contains("Estado"))
            throw new Exception("LastDataRowToEstado: DataSet no tiene la tabla Estado");
         int index = ds.Tables["Estado"].Rows.Count;
         if (index < 1)
            throw new Exception("LastDataRowToEstado: El DataSet no tiene filas");
         return this.DataRowToEstado(ds.Tables["Estado"].Rows[index - 1]);
      }
      /// <summary>
      /// Crea un objeto de Estado a partir de los datos de un DataRow.
      /// </summary>
      /// <param name="row">El DataRow que contiene la información de Estado</param>
      /// <returns>Un objeto de Estado creado a partir de los datos</returns>
      public Estado DataRowToEstado(DataRow row){
         Estado estado = new Estado();
          estado.Pais = new Pais();
         if (row.IsNull("EstadoID"))
            estado.EstadoID = null;
         else
            estado.EstadoID = (int)Convert.ChangeType(row["EstadoID"], typeof(int));
         if (row.IsNull("Nombre"))
            estado.Nombre = null;
         else
            estado.Nombre = (string)Convert.ChangeType(row["Nombre"], typeof(string));
         if (row.IsNull("FechaRegistro"))
            estado.FechaRegistro = null;
         else
            estado.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
         if (row.IsNull("PaisID"))
            estado.Pais.PaisID = null;
         else
            estado.Pais.PaisID = (int)Convert.ChangeType(row["PaisID"], typeof(int));
         if (row.IsNull("Codigo"))
             estado.Codigo = null;
         else
             estado.Codigo = (string)Convert.ChangeType(row["Codigo"], typeof(string));
         return estado;
      }
      /// <summary>
      /// Valida si el Estado a insertar ya esta configurado en el sistema
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="estado">Estado a insertar</param>
      /// <returns>True si no existe el Estado</returns>
      private bool ValidateDataForInsert(IDataContext dctx, Estado estado, Estado previous)
      {
          DataSet ds = this.Retrieve(dctx, estado);
          if (ds.Tables["Estado"].Rows.Count > 0)
          {
              if (previous.EstadoID == null)
                  return false;
              Estado objectDB = LastDataRowToEstado(ds);
              if (objectDB.EstadoID != previous.EstadoID)
                  return false;
          }
          return true;
      }
    private bool GetEstadosRelacionados(IDataContext dctx, Estado estado)
    {
        DataSet ds = Retrieve(dctx, estado);
        return ds.Tables["Estado"].Rows.Count > 0;
    }
   } 
}
