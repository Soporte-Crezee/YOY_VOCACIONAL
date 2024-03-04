using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Prueba.Diagnostico.Dinamica.BO;

namespace POV.Prueba.Diagnostico.Dinamica.DAO { 
   /// <summary>
   /// Inserta objeto BancoReactivosDinamico en la base de datos
   /// </summary>
   internal class BancoReactivosDinamicoInsHlp { 
      /// <summary>
      /// Crea un registro de BancoReactivosDinamico en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="bancoReactivos">BancoReactivosDinamico que desea crear</param>
      public void Action(IDataContext dctx, BancoReactivosDinamico bancoReactivos){
         object myFirm = new object();
         string sError = String.Empty;
         if (bancoReactivos == null)
            sError += ", BancoReactivosDinamico";
         if (sError.Length > 0)
            throw new Exception("BancoReactivosDinamicoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (bancoReactivos.EsSeleccionOrdenada == null)
             sError += "EsSeleccionOrdenada";
         if (bancoReactivos.NumeroReactivos == null)
             sError += "NumeroReactivos";
         if (bancoReactivos.TipoSeleccionBanco == null)
             sError += "TipoSeleccionBanco";
          if (bancoReactivos.Prueba == null)
            sError += ", Prueba";
         if (bancoReactivos.FechaRegistro == null)
            sError += ", FechaRegistro";
         if (bancoReactivos.Activo == null)
            sError += ", Activo";
         if (sError.Length > 0)
            throw new Exception("BancoReactivosDinamicoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (bancoReactivos.Prueba.PruebaID == null)
            sError += ", PruebaID";
         if (sError.Length > 0)
            throw new Exception("BancoReactivosDinamicoInsHlp:Los siguientes campos no pueden ser vacios" + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Prueba.Diagnostico.Dinamica.DAO", 
         "BancoReactivosDinamicoInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "BancoReactivosDinamicoInsHlp: No se pudo conectar a la base de datos", "POV.Prueba.Diagnostico.Dinamica.DAO", 
         "BancoReactivosDinamicoInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO BancoReactivosDinamico(PruebaID,NumeroReactivos,FechaRegistro,Activo,EsSeleccionOrdenada,TipoSeleccionBanco,ReactivosPorPagina) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         // bancoReactivos.Prueba.PruebaID
         sCmd.Append(" @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (bancoReactivos.Prueba.PruebaID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = bancoReactivos.Prueba.PruebaID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         // bancoReactivos.NumeroReactivos
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (bancoReactivos.NumeroReactivos == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = bancoReactivos.NumeroReactivos;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         // bancoReactivos.FechaRegistro
         sCmd.Append(" ,@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (bancoReactivos.FechaRegistro == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = bancoReactivos.FechaRegistro;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         // bancoReactivos.Activo
         sCmd.Append(" ,@dbp4ram4 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram4";
         if (bancoReactivos.Activo == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = bancoReactivos.Activo;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         // bancoReactivos.EsSeleccionOrdenada
         sCmd.Append(" ,@dbp4ram5 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram5";
         if (bancoReactivos.EsSeleccionOrdenada == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = bancoReactivos.EsSeleccionOrdenada;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         // bancoReactivos.TipoSeleccionBanco
         sCmd.Append(" ,@dbp4ram6 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram6";
         if (bancoReactivos.TipoSeleccionBanco == null)
             sqlParam.Value = DBNull.Value;
         else
             sqlParam.Value = bancoReactivos.TipoSeleccionBanco;
         sqlParam.DbType = DbType.Byte;
         sqlCmd.Parameters.Add(sqlParam);
         // bancoReactivos.ReactivosPorPagina
         sCmd.Append(" ,@dbp4ram7 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram7";
         if (bancoReactivos.ReactivosPorPagina == null)
             sqlParam.Value = DBNull.Value;
         else
             sqlParam.Value = bancoReactivos.ReactivosPorPagina;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ) ");
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("BancoReactivosDinamicoInsHlp: Ocurrio un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("BancoReactivosDinamicoInsHlp: Ocurrio un error al ingresar el registro.");
      }
   } 
}
