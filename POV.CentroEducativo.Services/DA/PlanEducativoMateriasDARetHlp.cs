using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.DAO;
using POV.Modelo.Estandarizado.BO;
using POV.Modelo.Estandarizado.Service;
namespace POV.CentroEducativo.DA { 
   /// <summary>
   /// Consulta un registro de las Matarias que correspondan a un plan Educativo
   /// </summary>
   public class PlanEducativoMateriasDARetHlp { 
      /// <summary>
      /// Consulta registros de PlanEducativoMaterias en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="planEducativo">PlanEducativoMaterias que provee el criterio de selección para realizar la consulta</param>
      /// <param name="materia">Materia que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de PlanEducativoMaterias generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, PlanEducativo planEducativo,Materia materia){
         object myFirm = new object();
         string sError = String.Empty;
         if (planEducativo == null)
            sError += ", PlanEducativo";
         if (sError.Length > 0)
            throw new Exception("PlanEducatioMateriasDARetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (planEducativo.PlanEducativoID == null)
            sError += ", PlanEducativoID";
         if (sError.Length > 0)
            throw new Exception("PlanEducatioMateriasDARetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
      if (materia == null) {
         materia = new Materia();
      }
      if (materia.AreaAplicacion==null) {
         materia.AreaAplicacion = new AreaAplicacion();
      }
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DA", 
         "PlanEducatioMateriasDARetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "PlanEducatioMateriasDARetHlp: No se pudo conectar a la base de datos", "POV.CentroEducativo.DA", 
         "PlanEducatioMateriasDARetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT MateriaID, Clave, Titulo, Grado, PlanEducativoID, AreaAplicacionID ");
         sCmd.Append(" FROM Materia ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (planEducativo.PlanEducativoID != null){
            s_VarWHERE.Append(" PlanEducativoID =@dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = planEducativo.PlanEducativoID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (materia.MateriaID != null){
            s_VarWHERE.Append(" AND MateriaID = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = materia.MateriaID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (materia.Clave != null){
            s_VarWHERE.Append(" AND Clave LIKE @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = materia.Clave;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (materia.Titulo != null){
            s_VarWHERE.Append(" AND Titulo LIKE @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = materia.Titulo;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (materia.Grado != null){
            s_VarWHERE.Append(" AND Grado = @dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            sqlParam.Value = materia.Grado;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (materia.AreaAplicacion.AreaAplicacionID != null){
            s_VarWHERE.Append(" AND AreaAplicacionID = @dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            sqlParam.Value = materia.AreaAplicacion.AreaAplicacionID;
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
            sqlAdapter.Fill(ds, "PlanEducativoMaterias");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("PlanEducatioMateriasDARetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
