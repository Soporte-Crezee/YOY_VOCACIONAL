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

namespace POV.CentroEducativo.DAO { 
   /// <summary>
   /// Consulta un registro de Materia en la BD
   /// </summary>
   public class MateriaRetHlp { 
      /// <summary>
      /// Consulta registros de Materia en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="materia">Materia que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de Materia generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, Materia materia){
         object myFirm = new object();
         string sError = String.Empty;
         if (materia == null)
            sError += ", Materia";
         if (sError.Length > 0)
            throw new Exception("MateriaRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
      if (materia.AreaAplicacion == null) {
         materia.AreaAplicacion = new AreaAplicacion();
      }
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", 
         "MateriaRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "MateriaRetHlp: No se pudo conectar a la base de datos", "POV.CentroEducativo.DAO", 
         "MateriaRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT MateriaID, Clave, Titulo, Grado, PlanEducativoID, AreaAplicacionID ");
         sCmd.Append(" FROM Materia ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (materia.MateriaID != null){
            s_VarWHERE.Append(" MateriaID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = materia.MateriaID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (materia.Clave != null){
            s_VarWHERE.Append(" AND Clave LIKE @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = materia.Clave;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (materia.Titulo != null){
            s_VarWHERE.Append(" AND Titulo LIKE @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = materia.Titulo;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (materia.Grado != null){
            s_VarWHERE.Append(" AND Grado = @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = materia.Grado;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (materia.AreaAplicacion.AreaAplicacionID != null){
            s_VarWHERE.Append(" AND AreaAplicacionID = @dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
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
            sqlAdapter.Fill(ds, "Materia");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("MateriaRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
