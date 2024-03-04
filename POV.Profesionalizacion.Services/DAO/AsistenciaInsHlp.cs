using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Profesionalizacion.BO;

namespace POV.Profesionalizacion.DAO { 
   /// <summary>
   /// Inserta Asistencia en la base de datos
   /// </summary>
   public class AsistenciaInsHlp { 
      /// <summary>
      /// Crea un registro de Asistencia en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="asistencia">Asistencia que desea crear</param>
      public void Action(IDataContext dctx, Asistencia asistencia){
         object myFirm = new object();
         string sError = "";
         if (asistencia == null)
            sError += ", Asistencia";
         if (sError.Length > 0)
            throw new Exception("AsistenciaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
       
         if (asistencia.Nombre == null || asistencia.Nombre.Trim().Length == 0)
            sError += ", Nombre";
         if (asistencia.FechaRegistro == null)
            sError += ", FechaRegistro";
         if (asistencia.Estatus == null)
            sError += ", EstatusProfesionalizacion";

          if (asistencia.TemaAsistencia == null)
              sError += ", TemaAsistencia";
         if (sError.Length > 0)
            throw new Exception("AsistenciaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));

         if (asistencia.TemaAsistencia.TemaAsistenciaID == null)
             sError += ", TemaAsistenciaID";
         if (sError.Length > 0)
             throw new Exception("AsistenciaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
          
             
          if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DAO", 
         "AsistenciaInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "AsistenciaInsHlp: No se pudo conectar a la base de datos", "POV.Profesionalizacion.DAO", 
         "AsistenciaInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO Asistencia (Nombre, Descripcion, FechaRegistro, EstatusProfesionalizacion, TipoAgrupador, TemaAsistenciaID,EsPredeterminado) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         // asistencia.Nombre
         sCmd.Append(" @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (asistencia.Nombre == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = asistencia.Nombre;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // asistencia.Descripcion
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (asistencia.Descripcion == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = asistencia.Descripcion;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // asistencia.FechaRegistro
         sCmd.Append(" ,@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (asistencia.FechaRegistro == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = asistencia.FechaRegistro;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         // asistencia.Estatus
         sCmd.Append(" ,@dbp4ram4 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram4";
         if (asistencia.Estatus == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = asistencia.Estatus;
         sqlParam.DbType = DbType.Byte;
         sqlCmd.Parameters.Add(sqlParam);
         // asistencia.TipoAgrupador
         sCmd.Append(" ,@dbp4ram5 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram5";
         if (asistencia.TipoAgrupador == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = asistencia.TipoAgrupador;
         sqlParam.DbType = DbType.Byte;
         sqlCmd.Parameters.Add(sqlParam);
         // asistencia.TemaAsistencia.TemaAsistenciaID
         sCmd.Append(" ,@dbp4ram6 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram6";
         if (asistencia.TemaAsistencia.TemaAsistenciaID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = asistencia.TemaAsistencia.TemaAsistenciaID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);

         // asistencia.EsPrederterminado
         sCmd.Append(" ,@dbp4ram7 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram7";
         if (asistencia.EsPredeterminado == null)
             sqlParam.Value = DBNull.Value;
         else
             sqlParam.Value = asistencia.EsPredeterminado;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);

         sCmd.Append(" ) ");
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("AsistenciaInsHlp: Ocurrio un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("AsistenciaInsHlp: Ocurrio un error al ingresar el registro.");
      }

       /// <summary>
       /// Crea un registro de Asistencia en la base de datos
       /// </summary>
       /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
       /// <param name="agrupador">Asistencia que desea crear</param>
       /// <param name="agrupadorPadreID">AgrupadorPadreID de la asistencia</param>
       public void Action(IDataContext dctx, AAgrupadorContenidoDigital agrupador,long? agrupadorPadreID){
         object myFirm = new object();
         string sError = String.Empty;
         if (agrupador == null)
             sError += ", AAgrupadorContenidoDigital";
         if (sError.Length > 0)
            throw new Exception("AsistenciaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (agrupadorPadreID == null)
            sError += ", agrupadorPadreID";
         if (agrupador.Nombre == null || agrupador.Nombre.Trim().Length == 0)
            sError += ", Nombre";
         if (agrupador.FechaRegistro == null)
            sError += ", FechaRegistro";
         if (agrupador.Estatus == null)
            sError += ", EstatusProfesionalizacion";

         if (sError.Length > 0)
            throw new Exception("AsistenciaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
      
           if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DAO", 
         "AsistenciaInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "AsistenciaInsHlp: No se pudo conectar a la base de datos", "POV.Profesionalizacion.DAO", 
         "AsistenciaInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO Asistencia (AgrupadorPadreID,Nombre, FechaRegistro, EstatusProfesionalizacion, TipoAgrupador,EsPredeterminado) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         // agrupadorPadreID
         sCmd.Append(" @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (agrupadorPadreID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = agrupadorPadreID;
         sqlParam.DbType = DbType.Int64;
         sqlCmd.Parameters.Add(sqlParam);
         // asistencia.Nombre
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (agrupador.Nombre == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = agrupador.Nombre;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
        
         // asistencia.FechaRegistro
         sCmd.Append(" ,@dbp4ram4 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram4";
         if (agrupador.FechaRegistro == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = agrupador.FechaRegistro;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         // asistencia.Estatus
         sCmd.Append(" ,@dbp4ram5 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram5";
         if (agrupador.Estatus == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = agrupador.Estatus;
         sqlParam.DbType = DbType.Byte;
         sqlCmd.Parameters.Add(sqlParam);
         // asistencia.TipoAgrupador
         sCmd.Append(" ,@dbp4ram6 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram6";
         if (agrupador.TipoAgrupador == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = agrupador.TipoAgrupador;
         sqlParam.DbType = DbType.Byte;
         sqlCmd.Parameters.Add(sqlParam);

         // asistencia.EsPrederterminado
         sCmd.Append(" ,@dbp4ram7 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram7";
         if (agrupador.EsPredeterminado == null)
             sqlParam.Value = DBNull.Value;
         else
             sqlParam.Value = agrupador.EsPredeterminado;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ) ");
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("AsistenciaInsHlp: Ocurrio un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("AsistenciaInsHlp: Ocurrio un error al ingresar el registro.");
      }
   } 
}
