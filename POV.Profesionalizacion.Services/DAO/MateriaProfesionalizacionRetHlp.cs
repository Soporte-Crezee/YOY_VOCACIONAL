using System;
using System.Data;
using System.Data.Common;
using System.Text;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Profesionalizacion.BO;

namespace POV.Profesionalizacion.DAO { 
   /// <summary>
   /// Consulta un registro de MateriaProfesionalizacion en la BD
   /// </summary>
   internal class MateriaProfesionalizacionRetHlp { 
      /// <summary>
      /// Consulta registros de MateriaProfesionalizacion en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="materiaProfesionalizacion">MateriaProfesionalizacion que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de MateriaProfesionalizacion generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, MateriaProfesionalizacion materiaProfesionalizacion, AreaProfesionalizacion areaProfesionalizacion){
         object myFirm = new object();
         string sError = String.Empty;
         if (materiaProfesionalizacion == null)
            sError += ", MateriaProfesionalizacion";
         if (sError.Length > 0)
            throw new Exception("MateriaProfesionalizacionRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
      if (areaProfesionalizacion==null) {
         areaProfesionalizacion = new AreaProfesionalizacion();
      }
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DAO", 
         "MateriaProfesionalizacionRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "MateriaProfesionalizacionRetHlp: No se pudo conectar a la base de datos", "POV.Profesionalizacion.DAO", 
         "MateriaProfesionalizacionRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT MateriaID, Nombre, FechaRegistro, Activo ");
         sCmd.Append(" FROM MateriaProfesionalizacion ");
         sCmd.Append(" WHERE ");
         StringBuilder s_Var = new StringBuilder();
         if (materiaProfesionalizacion.MateriaID != null){
            s_Var.Append(" MateriaID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = materiaProfesionalizacion.MateriaID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (areaProfesionalizacion.AreaProfesionalizacionID != null){
            s_Var.Append(" AND AreaProfesionalizacionID = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = areaProfesionalizacion.AreaProfesionalizacionID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (materiaProfesionalizacion.Nombre != null){
            s_Var.Append(" AND Nombre = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = materiaProfesionalizacion.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (materiaProfesionalizacion.FechaRegistro != null){
            s_Var.Append(" AND FechaRegistro = @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = materiaProfesionalizacion.FechaRegistro;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (materiaProfesionalizacion.Activo != null){
            s_Var.Append(" AND Activo = @dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            sqlParam.Value = materiaProfesionalizacion.Activo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         s_Var.Append("  ");
         string s_Varres = s_Var.ToString().Trim();
         if (s_Varres.Length > 0){
            if (s_Varres.StartsWith("AND "))
               s_Varres = s_Varres.Substring(4);
            else if (s_Varres.StartsWith("OR "))
               s_Varres = s_Varres.Substring(3);
            else if (s_Varres.StartsWith(","))
               s_Varres = s_Varres.Substring(1);
            sCmd.Append("  " + s_Varres);
         }
         else
         {
             sCmd.Remove(sCmd.Length - 7, 7);
         }
         DataSet ds = new DataSet();
         DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
         sqlAdapter.SelectCommand = sqlCmd;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            sqlAdapter.Fill(ds, "MateriaProfesionalizacion");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("MateriaProfesionalizacionRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
