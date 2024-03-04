using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Profesionalizacion.BO;

namespace POV.Profesionalizacion.DAO { 
   /// <summary>
   /// Consultar de la base de datos
   /// </summary>
   public class AsistenciaRetHlp { 
      /// <summary>
      /// Consulta registros de Asistencia en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="agrupador">Asistencia que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de Asistencia generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, AAgrupadorContenidoDigital agrupador){
         object myFirm = new object();
         string sError = "";
         if (agrupador == null)
             sError += ", AAgrupadorContenidoDigital";
         if (sError.Length > 0)
            throw new Exception("AsistenciaRetHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
     
        
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DAO", 
         "AsistenciaRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "AsistenciaRetHlp: Hubo un error al conectarse a la base de datos", "POV.Profesionalizacion.DAO", 
         "AsistenciaRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT AgrupadorContenidoDigitalID, AgrupadorPadreID, Nombre, Descripcion, FechaRegistro, EstatusProfesionalizacion, TipoAgrupador, TemaAsistenciaID,EsPredeterminado ");
         sCmd.Append(" FROM Asistencia ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (agrupador.AgrupadorContenidoDigitalID != null){
            s_VarWHERE.Append(" AgrupadorContenidoDigitalID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = agrupador.AgrupadorContenidoDigitalID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (agrupador.Nombre != null){
            s_VarWHERE.Append(" AND Nombre LIKE @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = agrupador.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (agrupador.Estatus != null)
         {
             s_VarWHERE.Append(" AND EstatusProfesionalizacion = @dbp4ram3 ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "dbp4ram3";
             sqlParam.Value = agrupador.Estatus;
             sqlParam.DbType = DbType.Byte;
             sqlCmd.Parameters.Add(sqlParam);
         }
         else
         {
             //Si el estado es nulo se consultaran los Agrupadores con estado Activo o  Mantenimiento.
             s_VarWHERE.Append(" AND (EstatusProfesionalizacion = @dbp4ram7  OR EstatusProfesionalizacion = @dbp4ram8 )");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "dbp4ram7";
             sqlParam.Value = EEstatusProfesionalizacion.ACTIVO;
             sqlParam.DbType = DbType.Byte;
             sqlCmd.Parameters.Add(sqlParam);

             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "dbp4ram8";
             sqlParam.Value = EEstatusProfesionalizacion.MANTENIMIENTO;
             sqlParam.DbType = DbType.Byte;
             sqlCmd.Parameters.Add(sqlParam);
         }
         if (agrupador.FechaRegistro != null){
            s_VarWHERE.Append(" AND FechaRegistro =@dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = agrupador.FechaRegistro;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (agrupador.TipoAgrupador != null){
            s_VarWHERE.Append(" AND TipoAgrupador =@dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            sqlParam.Value = agrupador.TipoAgrupador;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);
         }
          Asistencia asistencia = agrupador as Asistencia;
          if (asistencia!=null)
          {
              if (asistencia.TemaAsistencia != null)
              {
                  if (asistencia.TemaAsistencia.TemaAsistenciaID != null)
                  {
                      s_VarWHERE.Append(" AND TemaAsistenciaID =@dbp4ram6 ");
                      sqlParam = sqlCmd.CreateParameter();
                      sqlParam.ParameterName = "dbp4ram6";
                      sqlParam.Value = asistencia.TemaAsistencia.TemaAsistenciaID;
                      sqlParam.DbType = DbType.Int32;
                      sqlCmd.Parameters.Add(sqlParam);
                  }
              }
          }
          if (agrupador.EsPredeterminado != null)
          {
              s_VarWHERE.Append(" AND EsPredeterminado =@dbp4ram7 ");
              sqlParam = sqlCmd.CreateParameter();
              sqlParam.ParameterName = "dbp4ram7";
              sqlParam.Value = agrupador.EsPredeterminado;
              sqlParam.DbType = DbType.Byte;
              sqlCmd.Parameters.Add(sqlParam);
          }
         string s_VarWHEREres = s_VarWHERE.ToString().Trim();
         if (s_VarWHEREres.Length > 0){
            if (s_VarWHEREres.StartsWith("AND "))
               s_VarWHEREres = s_VarWHEREres.Substring(4);
            else if (s_VarWHEREres.StartsWith("OR "))
               s_VarWHEREres = s_VarWHEREres.Substring(3);
            else if (s_VarWHEREres.StartsWith(","))
               s_VarWHEREres = s_VarWHEREres.Substring(1);
            sCmd.Append(" WHERE " + s_VarWHEREres);
         }
         sCmd.Append(" ORDER BY AgrupadorContenidoDigitalID ASC ");
         DataSet ds = new DataSet();
         DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
         sqlAdapter.SelectCommand = sqlCmd;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            sqlAdapter.Fill(ds, "Asistencia");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("AsistenciaRetHlp: Hubo un error al consultar los registros. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
      /// <summary>
      /// Consulta registros de Asistencia en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="asistencia">Asistencia que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de Asistencia generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, long? agrupadorPadreID){
         object myFirm = new object();
         string sError = "";
        
         if (sError.Length > 0)
            throw new Exception("AsistenciaRetHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
     
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DAO", 
         "AsistenciaRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "AsistenciaRetHlp: Hubo un error al conectarse a la base de datos", "POV.Profesionalizacion.DAO", 
         "AsistenciaRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT AgrupadorContenidoDigitalID, AgrupadorPadreID, Nombre, Descripcion, FechaRegistro, EstatusProfesionalizacion, TipoAgrupador, TemaAsistenciaID,EsPredeterminado ");
         sCmd.Append(" FROM Asistencia ");
         StringBuilder s_VarWHERE = new StringBuilder();
       
         if (agrupadorPadreID != null){
            s_VarWHERE.Append(" AND AgrupadorPadreID =@dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = agrupadorPadreID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }

              //Si el estado es nulo se consultaran los Agrupadores con estado Activo o  Mantenimiento.
             s_VarWHERE.Append(" AND (EstatusProfesionalizacion = @dbp4ram7  OR EstatusProfesionalizacion = @dbp4ram8 )");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "dbp4ram7";
             sqlParam.Value = EEstatusProfesionalizacion.ACTIVO;
             sqlParam.DbType = DbType.Byte;
             sqlCmd.Parameters.Add(sqlParam);

             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "dbp4ram8";
             sqlParam.Value = EEstatusProfesionalizacion.MANTENIMIENTO;
             sqlParam.DbType = DbType.Byte;
             sqlCmd.Parameters.Add(sqlParam);


      
         string s_VarWHEREres = s_VarWHERE.ToString().Trim();
         if (s_VarWHEREres.Length > 0){
            if (s_VarWHEREres.StartsWith("AND "))
               s_VarWHEREres = s_VarWHEREres.Substring(4);
            else if (s_VarWHEREres.StartsWith("OR "))
               s_VarWHEREres = s_VarWHEREres.Substring(3);
            else if (s_VarWHEREres.StartsWith(","))
               s_VarWHEREres = s_VarWHEREres.Substring(1);
            sCmd.Append(" WHERE " + s_VarWHEREres);
         }
         sCmd.Append(" ORDER BY AgrupadorContenidoDigitalID ASC ");
         DataSet ds = new DataSet();
         DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
         sqlAdapter.SelectCommand = sqlCmd;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            sqlAdapter.Fill(ds, "Asistencia");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("AsistenciaRetHlp: Hubo un error al consultar los registros. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
