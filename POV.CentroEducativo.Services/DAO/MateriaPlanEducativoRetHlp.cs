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
   /// Consultar las Materia del plan educativo proporcionado
   /// </summary>
   public class MateriaPlanEducativoRetHlp { 
      /// <summary>
      /// Consulta registros de Mensaje en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="mensaje">Mensaje que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de Mensaje generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, Materia materia,PlanEducativo planEducativo, bool? estatus){
         object myFirm = new object();
         string sError = "";
         if (planEducativo == null)
            sError += ", PlanEducativo";
         if (sError.Length > 0)
            throw new Exception("MateriaPlanEducativoRetHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
      if (materia == null) {
         materia = new Materia();
      }
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", 
         "MateriaPlanEducativoRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "MateriaPlanEducativoRetHlp: Hubo un error al conectarse a la base de datos", "POV.CentroEducativo.DAO", 
         "MateriaPlanEducativoRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT MateriaPlanEducativo.MateriaPlanEducativoID, MateriaPlanEducativo.PlanEducativoID, MateriaPlanEducativo.MateriaID, MateriaPlanEducativo.Estatus, ");
         sCmd.Append(" Materia.Clave, Materia.Titulo, Materia.Grado,Materia.AreaAplicacionID ");
         sCmd.Append(" FROM MateriaPlanEducativo INNER JOIN Materia ON Materia.MateriaID = MateriaPlanEducativo.MateriaID ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (planEducativo.PlanEducativoID != null){
            s_VarWHERE.Append(" MateriaPlanEducativo.PlanEducativoID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = planEducativo.PlanEducativoID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (materia.MateriaID != null){
            s_VarWHERE.Append(" AND MateriaPlanEducativo.MateriaID = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = materia.MateriaID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (estatus != null)
         {
             s_VarWHERE.Append(" AND MateriaPlanEducativo.Estatus = @dbp4ram3 ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "dbp4ram3";
             sqlParam.Value = estatus.Value;
             sqlParam.DbType = DbType.Boolean;
             sqlCmd.Parameters.Add(sqlParam);
         }
         if (materia.Clave != null){
            s_VarWHERE.Append(" AND Materia.Clave = @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = materia.Clave;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (materia.Titulo != null){
            s_VarWHERE.Append(" AND Materia.Titulo = @dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            sqlParam.Value = materia.Titulo;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (materia.Grado != null){
            s_VarWHERE.Append(" AND Materia.Grado = @dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            sqlParam.Value = materia.Grado;
            sqlParam.DbType = DbType.Int16;
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
         sCmd.Append(" ORDER BY MateriaPlanEducativo.MateriaID ASC ");
         DataSet ds = new DataSet();
         DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
         sqlAdapter.SelectCommand = sqlCmd;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            sqlAdapter.Fill(ds, "MateriaPlanEducativo");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("MateriaPlanEducativoRetHlp: Hubo un error al consultar los registros. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
