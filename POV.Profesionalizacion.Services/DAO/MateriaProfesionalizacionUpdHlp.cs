using System;
using System.Data;
using System.Data.Common;
using System.Text;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Profesionalizacion.BO;

namespace POV.Profesionalizacion.DAO { 
   /// <summary>
   /// Actualiza un registro de MateriaProfesionalizacion en la BD
   /// </summary>
   internal class MateriaProfesionalizacionUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de MateriaProfesionalizacion en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="materiaProfesionalizacion">MateriaProfesionalizacion que tiene los datos nuevos</param>
      /// <param name="anterior">MateriaProfesionalizacion que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, MateriaProfesionalizacion materiaProfesionalizacion, MateriaProfesionalizacion anterior, AreaProfesionalizacion areaProfesionalizacion){
         object myFirm = new object();
         String sError = string.Empty;
         if (materiaProfesionalizacion == null)
            sError += ", MateriaProfesionalizacion";
         if (areaProfesionalizacion == null)
            sError += ", AreaProfesionalizacion";
         if (anterior == null)
            sError += ", Anterior";
         if (sError.Length > 0)
            throw new Exception("MateriaProfesionalizacionUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (materiaProfesionalizacion.Nombre == null)
            sError += ", Nombre";
         if (anterior.MateriaID == null)
            sError += ", Anterior MateriaID";
         if (anterior.Nombre == null)
            sError += ", Anterior Nombre";
         if (areaProfesionalizacion.AreaProfesionalizacionID == null)
            sError += ", AreaProfesionalizacionID";
         if (sError.Length > 0)
            throw new Exception("MateriaProfesionalizacionUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DAO", 
         "MateriaProfesionalizacionUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "MateriaProfesionalizacionUpdHlp: Hubo un error al conectarse a la base de datos", "POV.Profesionalizacion.DAO", 
         "MateriaProfesionalizacionUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE MateriaProfesionalizacion ");
         if (materiaProfesionalizacion.Nombre == null)
            sCmd.Append(" SET Nombre = NULL ");
         else{ 
            // materiaProfesionalizacion.Nombre
            sCmd.Append(" SET Nombre = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = materiaProfesionalizacion.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (materiaProfesionalizacion.Activo == null)
            sCmd.Append(" ,Activo = NULL ");
         else{ 
            // materiaProfesionalizacion.Activo
            sCmd.Append(" ,Activo = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = materiaProfesionalizacion.Activo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         sCmd.Append(" WHERE ");
         StringBuilder s_Var = new StringBuilder();
         if (anterior.MateriaID == null)
            s_Var.Append(" MateriaID = NULL ");
         else{ 
            // anterior.MateriaProfesionalizacionID
            s_Var.Append(" MateriaID = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = anterior.MateriaID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (areaProfesionalizacion.AreaProfesionalizacionID == null)
            s_Var.Append(" AND AreaProfesionalizacionID = NULL ");
         else{ 
            // areaProfesionalizacion.AreaProfesionalizacionID
            s_Var.Append(" AND AreaProfesionalizacionID = @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = areaProfesionalizacion.AreaProfesionalizacionID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.Nombre == null)
            s_Var.Append(" AND Nombre = NULL ");
         else{ 
            // anterior.Nombre
            s_Var.Append(" AND Nombre = @dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            sqlParam.Value = anterior.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.Activo == null)
            s_Var.Append(" AND Activo = NULL ");
         else{ 
            // anterior.Activo
            s_Var.Append(" AND Activo = @dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            sqlParam.Value = anterior.Activo;
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
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("MateriaProfesionalizacionUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("MateriaProfesionalizacionUpdHlp: Hubo  un Error al Actualizar el Registro .");
      }
   } 
}
