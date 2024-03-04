using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.DAO;

namespace POV.CentroEducativo.DAO { 
   /// <summary>
   /// Consulta un registro de CentroComputo en la BD
   /// </summary>
   public class CentroComputoRetHlp { 
      /// <summary>
      /// Consulta registros de CentroComputo en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="centroComputo">CentroComputo que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de CentroComputo generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, Escuela escuela, CentroComputo centroComputo){
         object myFirm = new object();
         string sError = String.Empty;
         if (centroComputo == null)
            sError += ", CentroComputo";
         if (sError.Length > 0)
            throw new Exception("CentroComputoRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (escuela == null)
            sError += ", Escuela";
         if (sError.Length > 0)
            throw new Exception("CentroComputoRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", 
         "CentroComputoRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "CentroComputoRetHlp: No se pudo conectar a la base de datos", "POV.CentroEducativo.DAO", 
         "CentroComputoRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT CentroComputoID, TieneCentroComputo, NumeroComputadoras, TieneInternet, AnchoBanda, NombreProveedor, TipoContrato, Responsable, TelefonoResponsable, FechaRegistro, Activo, EscuelaID ");
         sCmd.Append(" FROM CentroComputo ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (centroComputo.CentroComputoID != null){
            s_VarWHERE.Append(" CentroComputoID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = centroComputo.CentroComputoID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (centroComputo.NumeroComputadoras != null){
            s_VarWHERE.Append(" AND NumeroComputadoras = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = centroComputo.NumeroComputadoras;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (centroComputo.TieneCentroComputo != null){
            s_VarWHERE.Append(" AND TieneCentroComputo = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = centroComputo.TieneCentroComputo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (centroComputo.TieneInternet != null){
            s_VarWHERE.Append(" AND TieneInternet = @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = centroComputo.TieneInternet;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (centroComputo.AnchoBanda != null){
            s_VarWHERE.Append(" AND AnchoBanda = @dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            sqlParam.Value = centroComputo.AnchoBanda;
            sqlParam.DbType = DbType.Decimal;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (centroComputo.NombreProveedor != null){
            s_VarWHERE.Append(" AND NombreProveedor LIKE @dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            sqlParam.Value = centroComputo.NombreProveedor;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (centroComputo.TipoContrato != null){
            s_VarWHERE.Append(" AND TipoContrato LIKE @dbp4ram7 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram7";
            sqlParam.Value = centroComputo.TipoContrato;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (centroComputo.Responsable != null){
            s_VarWHERE.Append(" AND Responsable LIKE @dbp4ram8 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram8";
            sqlParam.Value = centroComputo.Responsable;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (centroComputo.TelefonoResponsable != null){
            s_VarWHERE.Append(" AND TelefonoResponsable = @dbp4ram9 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram9";
            sqlParam.Value = centroComputo.TelefonoResponsable;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (centroComputo.FechaRegistro != null){
            s_VarWHERE.Append(" AND FechaRegistro = @dbp4ram10 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram10";
            sqlParam.Value = centroComputo.FechaRegistro;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (centroComputo.Activo != null){
            s_VarWHERE.Append(" AND Activo = @dbp4ram11 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram11";
            sqlParam.Value = centroComputo.Activo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (escuela.EscuelaID != null){
            s_VarWHERE.Append(" AND EscuelaID = @dbp4ram12 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram12";
            sqlParam.Value = escuela.EscuelaID;
            sqlParam.DbType = DbType.Int32;
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
         DataSet ds = new DataSet();
         DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
         sqlAdapter.SelectCommand = sqlCmd;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            sqlAdapter.Fill(ds, "CentroComputo");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("CentroComputoRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
