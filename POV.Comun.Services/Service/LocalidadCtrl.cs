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
   /// Servicios para Acceder a las Localidades
   /// </summary>
   public class LocalidadCtrl { 
      /// <summary>
      /// Crea un registro de Localidad en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="localidad">Localidad que desea crear</param>
      public void Insert(IDataContext dctx, Localidad localidad){
          try
          {
              if (!ValidateDataForInsert(dctx, new Localidad { Nombre = localidad.Nombre, Ciudad = new Ciudad { CiudadID = localidad.Ciudad.CiudadID } },localidad))
              {
                  throw new DuplicateNameException("El nombre de la Localidad para esta Ciudad ya se encuentra configurado en el sistema, por favor verifique.");
              }
              else
              {
                  LocalidadInsHlp da = new LocalidadInsHlp();
                  da.Action(dctx, localidad);
              }
          }
          catch (Exception ex)
          {
              throw new DuplicateNameException(ex.Message);
          }
      }
      /// <summary>
      /// Consulta registros de Localidad en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="localidad">Localidad que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de Localidad generada por la consulta</returns>
      public DataSet Retrieve(IDataContext dctx, Localidad localidad){
         LocalidadRetHlp da = new LocalidadRetHlp();
         DataSet ds = da.Action(dctx, localidad);
         return ds;
      }
      /// <summary>
      /// Actualiza de manera optimista un registro de Localidad en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="localidad">Localidad que tiene los datos nuevos</param>
      /// <param name="anterior">Localidad que tiene los datos anteriores</param>
      public void Update(IDataContext dctx, Localidad localidad, Localidad previous){
          try
          {
              if (!ValidateDataForInsert(dctx, new Localidad { Nombre = localidad.Nombre, Ciudad = new Ciudad { CiudadID = localidad.Ciudad.CiudadID } },previous))
              {
                  throw new DuplicateNameException("El nombre de la Localidad para esta Ciudad ya se encuentra configurado en el sistema, por favor verifique.");
              }
              else
              {
                  LocalidadUpdHlp da = new LocalidadUpdHlp();
                  da.Action(dctx, localidad, previous);
              }
          }
          catch (Exception ex)
          {
              throw new DuplicateNameException(ex.Message);
          }
      }
      /// <summary>
      /// Elimina un registro de Localidad en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="localidad">Localidad que desea eliminar</param>
      public void Delete(IDataContext dctx, Localidad localidad){
         LocalidadDelHlp da = new LocalidadDelHlp();
         da.Action(dctx, localidad);
      }
      /// <summary>
      /// Crea un objeto de Localidad a partir de los datos del último DataRow del DataSet.
      /// </summary>
      /// <param name="ds">El DataSet que contiene la información de Localidad</param>
      /// <returns>Un objeto de Localidad creado a partir de los datos</returns>
      public Localidad LastDataRowToLocalidad(DataSet ds) {
         if (!ds.Tables.Contains("Localidad"))
            throw new Exception("LastDataRowToLocalidad: DataSet no tiene la tabla Localidad");
         int index = ds.Tables["Localidad"].Rows.Count;
         if (index < 1)
            throw new Exception("LastDataRowToLocalidad: El DataSet no tiene filas");
         return this.DataRowToLocalidad(ds.Tables["Localidad"].Rows[index - 1]);
      }
      /// <summary>
      /// Crea un objeto de Localidad a partir de los datos de un DataRow.
      /// </summary>
      /// <param name="row">El DataRow que contiene la información de Localidad</param>
      /// <returns>Un objeto de Localidad creado a partir de los datos</returns>
      public Localidad DataRowToLocalidad(DataRow row){
         Localidad localidad = new Localidad();
          localidad.Ciudad = new Ciudad();
         if (row.IsNull("LocalidadID"))
            localidad.LocalidadID = null;
         else
            localidad.LocalidadID = (int)Convert.ChangeType(row["LocalidadID"], typeof(int));
         if (row.IsNull("Nombre"))
            localidad.Nombre = null;
         else
            localidad.Nombre = (string)Convert.ChangeType(row["Nombre"], typeof(string));
         if (row.IsNull("FechaRegistro"))
            localidad.FechaRegistro = null;
         else
            localidad.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
         if (row.IsNull("CiudadID"))
            localidad.Ciudad.CiudadID = null;
         else
            localidad.Ciudad.CiudadID = (int)Convert.ChangeType(row["CiudadID"], typeof(int));
         if (row.IsNull("Codigo"))
             localidad.Codigo = null;
         else
             localidad.Codigo = (string)Convert.ChangeType(row["Codigo"], typeof(string));
         return localidad;
      }
      /// <summary>
      /// Valida si la Localidad a insertar ya esta configurada en el sistema
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="localidad">Localidad a insertar</param>
      /// <returns>True si no existe la Localidad</returns>
      private bool ValidateDataForInsert(IDataContext dctx, Localidad localidad,Localidad previous)
      {
          DataSet ds = this.Retrieve(dctx, localidad);
          if (ds.Tables["Localidad"].Rows.Count > 0)
          {
              if (previous.LocalidadID == null)
                  return false;
              Localidad objectDB = LastDataRowToLocalidad(ds);
              if (objectDB.LocalidadID != previous.LocalidadID)
                  return false;
          }
          return true;
      }
   } 
}
