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
   internal class TemaAsistenciaRetHlp { 
      /// <summary>
      /// Consulta registros de TemaAsistencia en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="temaAsistencia">TemaAsistencia que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de TemaAsistencia generada por la consulta</returns>
      internal DataSet Action(IDataContext dctx, TemaAsistencia temaasistencia){
         object myFirm = new object();
         string sError = "";
         if (sError.Length > 0)
            throw new Exception("RetHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DAO", 
         "TemaAsistenciaRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "TemaAsistenciaRetHlp: Hubo un error al conectarse a la base de datos", "POV.Profesionalizacion.DAO", 
         "TemaAsistenciaRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT TemaAsistenciaID,Nombre,Descripcion,FechaRegistro,Activo ");
         sCmd.Append(" FROM TemaAsistencia ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (temaasistencia.TemaAsistenciaID != null){
            s_VarWHERE.Append(" TemaAsistenciaID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = temaasistencia.TemaAsistenciaID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (temaasistencia.Nombre != null){
             s_VarWHERE.Append(" AND UPPER(NOMBRE) LIKE UPPER(@dbp4ram2) COLLATE Modern_Spanish_CI_AI");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = temaasistencia.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (temaasistencia.Activo != null){
            s_VarWHERE.Append(" AND ACTIVO = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = temaasistencia.Activo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (temaasistencia.FechaRegistro != null){
            s_VarWHERE.Append(" AND FECHAREGISTRO = @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = temaasistencia.FechaRegistro;
            sqlParam.DbType = DbType.DateTime;
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
         sCmd.Append(" ORDER BY TemaAsistenciaID ASC ");
         DataSet ds = new DataSet();
         DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
         sqlAdapter.SelectCommand = sqlCmd;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            sqlAdapter.Fill(ds, "TemaAsistencia");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("TemaAsistenciaRetHlp: Hubo un error al consultar los registros. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
