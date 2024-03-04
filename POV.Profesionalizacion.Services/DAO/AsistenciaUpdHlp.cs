using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Profesionalizacion.BO;

namespace POV.Profesionalizacion.DAO { 
   /// <summary>
   /// Actualiza en la base de datos
   /// </summary>
   public class AsistenciaUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de Asistencia en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="asistencia">Asistencia que tiene los datos nuevos</param>
      /// <param name="anterior">Asistencia que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, Asistencia asistencia, Asistencia anterior){
         object myFirm = new object();
         string sError = String.Empty;
         if (asistencia == null)
            sError += ", asistencia";
         if (anterior == null)
            sError += ", anterior";
         if (sError.Length > 0)
            throw new Exception("AsistenciaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (asistencia.AgrupadorContenidoDigitalID == null)
            sError += ", AgrupadorContenidoDigitalID";
         if (asistencia.Nombre == null || asistencia.Nombre.Trim().Length == 0)
            sError += ", Nombre";
         if (asistencia.Estatus == null)
            sError += ", Estado";
         if (asistencia.TemaAsistencia == null)
            sError += ", TemaAsistencia";
         if (sError.Length > 0)
            throw new Exception("AsistenciaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (anterior.TemaAsistencia == null)
            sError += ", TemaAsistencia";
         if (anterior.AgrupadorContenidoDigitalID == null)
            sError += ", Anterior AgrupadorContenidoDigitalID";
         if (anterior.Nombre == null || anterior.Nombre.Trim().Length == 0)
            sError += ", Anterior Nombre";
         if (anterior.Estatus == null)
            sError += ", Anterior Estado";
         if (sError.Length > 0)
            throw new Exception("AsistenciaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (asistencia.TemaAsistencia.TemaAsistenciaID == null)
            sError += ", TemaAsistenciaID";
         if (anterior.TemaAsistencia.TemaAsistenciaID == null)
            sError += ", TemaAsistenciaID";
         if (sError.Length > 0)
            throw new Exception("AsistenciaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DAO", 
         "AsistenciaUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "AsistenciaUpdHlp: Ocurrió un error al conectarse a la base de datos", "POV.Profesionalizacion.DAO", 
         "AsistenciaUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE Asistencia ");
         // asistencia.Nombre
         sCmd.Append(" SET Nombre =@dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (asistencia.Nombre == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = asistencia.Nombre;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // asistencia.Descripcion
         sCmd.Append(" ,Descripcion =@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (asistencia.Descripcion == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = asistencia.Descripcion;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);

         // asistencia.Estatus
         sCmd.Append(" ,EstatusProfesionalizacion =@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3 ";
         if (asistencia.Estatus == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = (byte)asistencia.Estatus;
         sqlParam.DbType = DbType.Byte;
         sqlCmd.Parameters.Add(sqlParam);

         // asistencia.TemaAsistencia.TemaAsistenciaID
         sCmd.Append(" ,TemaAsistenciaID =@dbp4ram4 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram4";
         if (asistencia.TemaAsistencia.TemaAsistenciaID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = asistencia.TemaAsistencia.TemaAsistenciaID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);

         // asistencia.EsPredeterminado
         sCmd.Append(" ,EsPredeterminado =@dbp4ram5 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram5";
          if (asistencia.EsPredeterminado == null)
              sqlParam.Value = DBNull.Value;
          else
              sqlParam.Value = asistencia.EsPredeterminado;
          sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);


         // anterior.AgrupadorContenidoDigitalID
         sCmd.Append(" WHERE AgrupadorContenidoDigitalID = @dbp4ram6 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram6";
         sqlParam.Value = anterior.AgrupadorContenidoDigitalID;
         sqlParam.DbType = DbType.Int64;
         sqlCmd.Parameters.Add(sqlParam);

         // anterior.Nombre
         sCmd.Append(" AND Nombre =@dbp4ram7 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram7";
         if (anterior.Nombre == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = anterior.Nombre;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);

         // anterior.TemaAsistencia.TemaAsistenciaID
         sCmd.Append(" AND TemaAsistenciaID =@dbp4ram8 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram8";
         if (anterior.TemaAsistencia.TemaAsistenciaID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = anterior.TemaAsistencia.TemaAsistenciaID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);

         // anterior.Estatus
         sCmd.Append(" AND EstatusProfesionalizacion =@dbp4ram9 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram9";
         if (anterior.Estatus == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = (byte)anterior.Estatus;
         sqlParam.DbType = DbType.Byte;
         sqlCmd.Parameters.Add(sqlParam);

         // anterior.Estatus
         sCmd.Append(" AND EsPredeterminado =@dbp5ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp5ram1";
         if (anterior.EsPredeterminado == null)
             sqlParam.Value = DBNull.Value;
         else
             sqlParam.Value = anterior.EsPredeterminado;
          sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);

         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("AsistenciaUpdHlp: Ocurrio un error al actualizar el Asistencia o fue modificado mientras era editado. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("AsistenciaUpdHlp: Ocurrio un error al actualizar el Asistencia o fue modificado mientras era editado.");
      }


       /// <summary>
       /// Actualiza de manera optimista un registro de AAgrupadorContenidoDigital para Asistencia en la base de datos.
       /// </summary>
       /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
       /// <param name="agrupador"> AAgrupadorContenidoDigital que tiene los datos nuevos</param>
       /// <param name="anterior"> AAgrupadorContenidoDigital que tiene los datos anteriores</param>
      /// <param name="agrupadorPadreID">AgrupadorPadreID del  AAgrupadorContenidoDigital</param>
       public void Action(IDataContext dctx, AAgrupadorContenidoDigital agrupador, AAgrupadorContenidoDigital anterior,long? agrupadorPadreID)
      {
          object myFirm = new object();
          string sError = String.Empty;
          if (agrupador == null)
              sError += ", Agrupador";
          if (anterior == null)
              sError += ", anterior";
          if (agrupadorPadreID == null)
              sError += ", AgrupadorPadreID";
          if (sError.Length > 0)
              throw new Exception("AsistenciaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
          if (agrupador.AgrupadorContenidoDigitalID == null)
              sError += ", AgrupadorContenidoDigitalID";
          if (agrupador.Nombre == null || agrupador.Nombre.Trim().Length == 0)
              sError += ", Nombre";
          if (agrupador.Estatus == null)
              sError += ", Estado";
          if (agrupador.EsPredeterminado == null)
              sError += ", EsPredeterminado";

          if (sError.Length > 0)
              throw new Exception("AsistenciaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));

          if (anterior.AgrupadorContenidoDigitalID == null)
              sError += ", Anterior AgrupadorContenidoDigitalID";
          if (anterior.Nombre == null || anterior.Nombre.Trim().Length == 0)
              sError += ", Anterior Nombre";
          if (anterior.Estatus == null)
              sError += ", Anterior Estado";
          if (anterior.EsPredeterminado == null)
              sError += ", Anterior EsPredeterminado";

          if (sError.Length > 0)
              throw new Exception("AsistenciaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));


          if (dctx == null)
              throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DAO",
                 "AsistenciaUpdHlp", "Action", null, null);
          DbCommand sqlCmd = null;
          try
          {
              dctx.OpenConnection(myFirm);
              sqlCmd = dctx.CreateCommand();
          }
          catch (Exception ex)
          {
              throw new StandardException(MessageType.Error, "", "AsistenciaUpdHlp: Ocurrió un error al conectarse a la base de datos", "POV.Profesionalizacion.DAO",
                 "AsistenciaUpdHlp", "Action", null, null);
          }
          DbParameter sqlParam;
          StringBuilder sCmd = new StringBuilder();
          sCmd.Append(" UPDATE Asistencia ");
          // asistencia.Nombre
          sCmd.Append(" SET Nombre =@dbp4ram1 ");
          sqlParam = sqlCmd.CreateParameter();
          sqlParam.ParameterName = "dbp4ram1";
          if (agrupador.Nombre == null)
              sqlParam.Value = DBNull.Value;
          else
              sqlParam.Value = agrupador.Nombre;
          sqlParam.DbType = DbType.String;
          sqlCmd.Parameters.Add(sqlParam);
  
          // asistencia.Estatus
          sCmd.Append(" ,EstatusProfesionalizacion =@dbp4ram3 ");
          sqlParam = sqlCmd.CreateParameter();
          sqlParam.ParameterName = "dbp4ram3";
          if (agrupador.Estatus == null)
              sqlParam.Value = DBNull.Value;
          else
              sqlParam.Value =(byte)agrupador.Estatus;
          sqlParam.DbType = DbType.Byte;
          sqlCmd.Parameters.Add(sqlParam);

          // asistencia.EsPredeterminado
          sCmd.Append(" ,EsPredeterminado =@dbp4ram4 ");
          sqlParam = sqlCmd.CreateParameter();
          sqlParam.ParameterName = "dbp4ram4";
          if (agrupador.EsPredeterminado == null)
              sqlParam.Value = DBNull.Value;
          else
              sqlParam.Value = agrupador.EsPredeterminado;
          sqlParam.DbType = DbType.Boolean;
          sqlCmd.Parameters.Add(sqlParam);


          // anterior.AgrupadorContenidoDigitalID
          sCmd.Append(" WHERE AgrupadorContenidoDigitalID =@dbp4ram5 ");
          sqlParam = sqlCmd.CreateParameter();
          sqlParam.ParameterName = "dbp4ram5";
          sqlParam.Value = anterior.AgrupadorContenidoDigitalID;
          sqlParam.DbType = DbType.Int64;
          sqlCmd.Parameters.Add(sqlParam);

          // anterior.Nombre
          sCmd.Append(" AND Nombre =@dbp4ram6 ");
          sqlParam = sqlCmd.CreateParameter();
          sqlParam.ParameterName = "dbp4ram6";
          if (anterior.Nombre == null)
              sqlParam.Value = DBNull.Value;
          else
              sqlParam.Value = anterior.Nombre;
          sqlParam.DbType = DbType.String;
          sqlCmd.Parameters.Add(sqlParam);


          // anterior.Estatus
          sCmd.Append(" AND EstatusProfesionalizacion =@dbp4ram7 ");
          sqlParam = sqlCmd.CreateParameter();
          sqlParam.ParameterName = "dbp4ram7";
          if (anterior.Estatus == null)
              sqlParam.Value = DBNull.Value;
          else
              sqlParam.Value = (byte)anterior.Estatus;
          sqlParam.DbType = DbType.Byte;
          sqlCmd.Parameters.Add(sqlParam);

          // anterior.EsPredeterminado
          sCmd.Append(" AND EsPredeterminado =@dbp4ram8 ");
          sqlParam = sqlCmd.CreateParameter();
          sqlParam.ParameterName = "dbp4ram8";
          if (anterior.Estatus == null)
              sqlParam.Value = DBNull.Value;
          else
              sqlParam.Value = anterior.EsPredeterminado;
          sqlParam.DbType = DbType.Boolean;
          sqlCmd.Parameters.Add(sqlParam);

          // AgrupadorPadreID
          sCmd.Append(" AND AgrupadorPadreID =@dbp4ram9 ");
          sqlParam = sqlCmd.CreateParameter();
          sqlParam.ParameterName = "dbp4ram9";
          sqlParam.Value = agrupadorPadreID;
          sqlParam.DbType = DbType.Int64;
          sqlCmd.Parameters.Add(sqlParam);

          int iRes = 0;
          try
          {
              sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
              iRes = sqlCmd.ExecuteNonQuery();
          }
          catch (Exception ex)
          {
              string exmsg = ex.Message;
              try { dctx.CloseConnection(myFirm); }
              catch (Exception) { }
              throw new Exception("AsistenciaUpdHlp: Ocurrio un error al actualizar el Asistencia o fue modificado mientras era editado. " + exmsg);
          }
          finally
          {
              try { dctx.CloseConnection(myFirm); }
              catch (Exception) { }
          }
          if (iRes < 1)
              throw new Exception("AsistenciaUpdHlp: Ocurrio un error al actualizar el Asistencia o fue modificado mientras era editado.");
      }
   } 
}
