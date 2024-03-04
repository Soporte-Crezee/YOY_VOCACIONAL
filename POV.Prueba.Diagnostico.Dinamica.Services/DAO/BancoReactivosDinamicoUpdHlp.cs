using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Prueba.Diagnostico.Dinamica.BO;

namespace POV.Prueba.Diagnostico.Dinamica.DAO { 
   /// <summary>
   /// Actualiza los BancoReactivosDinamico en la base de datos
   /// </summary>
   internal class BancoReactivosDinamicoUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de BancoReactivosDinamico en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="bancoReactivos">BancoReactivosDinamico que tiene los datos nuevos</param>
      /// <param name="anterior">BancoReactivosDinamico que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, BancoReactivosDinamico bancoReactivos,BancoReactivosDinamico anterior){
         object myFirm = new object();
         string sError = String.Empty;
         if (bancoReactivos == null)
            sError += ", BancoReactivosDinamico";
         if (anterior == null)
            sError += ", anterior";
         if (sError.Length > 0)
            throw new Exception("BancoReactivosDinamicoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (bancoReactivos.BancoReactivoID == null)
            sError += ", BancoReactivoID";
         if (bancoReactivos.Activo == null)
            sError += ", Activo";
         if (bancoReactivos.EsSeleccionOrdenada == null)
            sError += ", EsSeleccionOrdenada";
         if (bancoReactivos.TipoSeleccionBanco == null)
            sError += ", TipoSeleccionBanco";
         if (bancoReactivos.NumeroReactivos == null)
            sError += ", NumeroReactivos";
         if (bancoReactivos.ReactivosPorPagina == null)
             sError += ", ReactivosPorPagina";
         if (sError.Length > 0)
            throw new Exception("BancoReactivosEstandarizadoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (bancoReactivos.Prueba.PruebaID == null)
            sError += ", PruebaID";
         if (sError.Length > 0)
            throw new Exception("BancoReactivosEstandarizadoInsHlp:Los siguientes campos no pueden ser vacios" + sError.Substring(2));
         if (anterior.BancoReactivoID == null)
            sError += ", BancoReactivoID";
         if (anterior.Prueba == null)
            sError += ", Prueba";
         if (anterior.FechaRegistro == null)
            sError += ", FechaRegistro";
         if (anterior.Activo == null)
            sError += ", Activo";
         if (anterior.EsSeleccionOrdenada == null)
            sError += ", EsSeleccionOrdenada";
         if (anterior.NumeroReactivos == null)
             sError += ", NumeroReactivos";
         if (anterior.ReactivosPorPagina == null)
             sError += ", ReactivosPorPagina";
         if (sError.Length > 0)
            throw new Exception("BancoReactivosEstandarizadoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (anterior.Prueba.PruebaID == null)
            sError += ", PruebaID";
         if (sError.Length > 0)
            throw new Exception("BancoReactivosEstandarizadoInsHlp:Los siguientes campos no pueden ser vacios" + sError.Substring(2));
      if (bancoReactivos.BancoReactivoID != anterior.BancoReactivoID) {
         sError = "Los parametros no coinciden";
      }
         if (sError.Length > 0)
            throw new Exception("BancoReactivosDinamicoUpdHlp: " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Prueba.Diagnostico.Dinamica.DAO", 
         "BancoReactivosDinamicoUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "BancoReactivosDinamicoUpdHlp: Ocurrió un error al conectarse a la base de datos", "POV.Prueba.Diagnostico.Dinamica.DAO", 
         "BancoReactivosDinamicoUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE BancoReactivosDinamico ");
         sCmd.Append(" SET ");
         // anterior.NumeroReactivos
         sCmd.Append(" NumeroReactivos =@dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (bancoReactivos.NumeroReactivos == null)
            sqlParam.Value = DBNull.Value;
         else
             sqlParam.Value = bancoReactivos.NumeroReactivos;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);

         // anterior.Activo
         sCmd.Append(" ,Activo =@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (bancoReactivos.Activo == null)
            sqlParam.Value = DBNull.Value;
         else
             sqlParam.Value = bancoReactivos.Activo;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);

         // anterior.EsSeleccionOrdenada
         sCmd.Append(" ,EsSeleccionOrdenada =@dbp4ram4 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram4";
         if (bancoReactivos.EsSeleccionOrdenada == null)
            sqlParam.Value = DBNull.Value;
         else
             sqlParam.Value = bancoReactivos.EsSeleccionOrdenada;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);

         // anterior.TipoSeleccionBanco
         sCmd.Append(" ,TipoSeleccionBanco =@dbp4ram5 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram5";
         if (bancoReactivos.TipoSeleccionBanco == null)
             sqlParam.Value = DBNull.Value;
         else
             sqlParam.Value = bancoReactivos.TipoSeleccionBanco;
         sqlParam.DbType = DbType.Byte;
         sqlCmd.Parameters.Add(sqlParam);

         // anterior.ReactivosPorPagina
         sCmd.Append(" ,ReactivosPorPagina =@dbp4ram9 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram9";
         if (bancoReactivos.ReactivosPorPagina == null)
             sqlParam.Value = DBNull.Value;
         else
             sqlParam.Value = bancoReactivos.ReactivosPorPagina;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         // anterior.EsPorGrupo
         sCmd.Append(" ,EsPorGrupo =@dbp4ram12 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram12";
         if (bancoReactivos.EsPorGrupo == null)
             sqlParam.Value = DBNull.Value;
         else
             sqlParam.Value = bancoReactivos.EsPorGrupo;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);

         if (anterior.BancoReactivoID == null)
            sCmd.Append(" WHERE BancoReactivoID IS NULL ");
         else{ 
            // anterior.BancoReactivoID
            sCmd.Append(" WHERE BancoReactivoID = @dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            sqlParam.Value = anterior.BancoReactivoID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         // anterior.Prueba.PruebaID
         sCmd.Append(" AND PruebaID =@dbp4ram7 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram7";
         if (anterior.Prueba.PruebaID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = anterior.Prueba.PruebaID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         // anterior.NumeroReactivos
         sCmd.Append(" AND NumeroReactivos =@dbp4ram8 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram8";
         if (anterior.NumeroReactivos == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = anterior.NumeroReactivos;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         // anterior.Activo
         sCmd.Append(" AND Activo =@dbp4ram10 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram10";
         if (anterior.Activo == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = anterior.Activo;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         // anterior.EsSeleccionOrdenada
         sCmd.Append(" AND EsSeleccionOrdenada =@dbp4ram11 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram11";
         if (anterior.EsSeleccionOrdenada == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = anterior.EsSeleccionOrdenada;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("BancoReactivosDinamicoUpdHlp: Ocurrio un error al actualizar el objeto o fue modificado mientras era editado. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("BancoReactivosDinamicoUpdHlp: Ocurrio un error al actualizar el objeto o fue modificado mientras era editado.");
      }
   } 
}
