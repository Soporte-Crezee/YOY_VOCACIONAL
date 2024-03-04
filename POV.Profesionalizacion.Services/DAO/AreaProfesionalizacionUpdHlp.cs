using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Profesionalizacion.BO;

namespace POV.Profesionalizacion.DAO { 
   /// <summary>
   /// Actualiza un registro de AreaProfesionalizacion en la BD
   /// </summary>
   internal class AreaProfesionalizacionUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de AreaProfesionalizacionUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="areaProfesionalizacionUpdHlp">AreaProfesionalizacionUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">AreaProfesionalizacionUpdHlp que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, AreaProfesionalizacion areaProfesionalizacion, AreaProfesionalizacion anterior){
         object myFirm = new object();
         String sError = string.Empty;
         if (areaProfesionalizacion == null)
            sError += ", AreaProfesionalizacion";
         if (anterior == null)
            sError += ", Anterior";
         if (sError.Length > 0)
            throw new Exception("AreaProfesionalizacionUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (areaProfesionalizacion.Nombre == null)
            sError += ", Nombre";
         if (areaProfesionalizacion.NivelEducativo == null)
             sError += ", NivelEducativo";
         if (areaProfesionalizacion.NivelEducativo.NivelEducativoID == null)
             sError += ", NivelEducativoID";
         if (areaProfesionalizacion.Grado == null)
             sError += ", Grado";
         if (anterior.AreaProfesionalizacionID == null)
            sError += ", Anterior AreaProfesionalizacionID";
         if (anterior.Nombre == null)
            sError += ", Anterior Nombre";
         if (anterior.NivelEducativo == null)
             sError += ", Anterior NivelEducativo";
         if (anterior.NivelEducativo.NivelEducativoID == null)
             sError += ", Anterior NivelEducativoID";
         if (anterior.Grado == null)
             sError += ", Anterior Grado";
         if (sError.Length > 0)
            throw new Exception("AreaProfesionalizacionUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "Namespace.TipoControlador", 
         "AreaProfesionalizacionUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "AreaProfesionalizacionUpdHlp: Hubo un error al conectarse a la base de datos", "Namespace.TipoControlador", 
         "AreaProfesionalizacionUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE AreaProfesionalizacion ");
         if (areaProfesionalizacion.Nombre == null)
            sCmd.Append(" SET Nombre = NULL ");
         else{ 
            // areaProfesionalizacion.Nombre
            sCmd.Append(" SET Nombre = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = areaProfesionalizacion.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (areaProfesionalizacion.Descripcion == null)
            sCmd.Append(" ,Descripcion = NULL ");
         else{ 
            // areaProfesionalizacion.Descripcion
            sCmd.Append(" ,Descripcion = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = areaProfesionalizacion.Descripcion;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (areaProfesionalizacion.Activo == null)
            sCmd.Append(" ,Activo = NULL ");
         else{ 
            // areaProfesionalizacion.Activo
            sCmd.Append(" ,Activo = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = areaProfesionalizacion.Activo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (areaProfesionalizacion.NivelEducativo == null)
             sCmd.Append(" ,NivelEducativoID = NULL ");
         else
         {
             // areaProfesionalizacion.NivelEducativoID
             sCmd.Append(" ,NivelEducativoID = @dbp4ram4 ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "dbp4ram4";
             sqlParam.Value = areaProfesionalizacion.NivelEducativo.NivelEducativoID;
             sqlParam.DbType = DbType.Int32;
             sqlCmd.Parameters.Add(sqlParam);
         }
         if (areaProfesionalizacion.Grado == null)
             sCmd.Append(" ,Grado = NULL ");
         else
         {
             // areaProfesionalizacion.Grado
             sCmd.Append(" ,Grado = @dbp4ram5 ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "dbp4ram5";
             sqlParam.Value = areaProfesionalizacion.Grado;
             sqlParam.DbType = DbType.Int16;
             sqlCmd.Parameters.Add(sqlParam);
         }
         sCmd.Append(" WHERE ");
         StringBuilder s_Var = new StringBuilder();
         if (anterior.AreaProfesionalizacionID == null)
            s_Var.Append(" AreaProfesionalizacionID is NULL ");
         else{ 
            // anterior.AreaProfesionalizacionID
            s_Var.Append(" AreaProfesionalizacionID = @dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            sqlParam.Value = anterior.AreaProfesionalizacionID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.Nombre == null)
            s_Var.Append(" AND Nombre is NULL ");
         else{ 
            // anterior.Nombre
            s_Var.Append(" AND Nombre = @dbp4ram7 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram7";
            sqlParam.Value = anterior.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.Descripcion == null)
            s_Var.Append(" AND Descripcion is NULL ");
         else{ 
            // anterior.Descripcion
            s_Var.Append(" AND Descripcion = @dbp4ram8 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram8";
            sqlParam.Value = anterior.Descripcion;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.Activo == null)
            s_Var.Append(" AND Activo is NULL ");
         else{ 
            // anterior.Activo
            s_Var.Append(" AND Activo = @dbp4ram9 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram9";
            sqlParam.Value = anterior.Activo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.NivelEducativo.NivelEducativoID == null)
             s_Var.Append(" AND NivelEducativoID is NULL ");
         else
         {
             // anterior..NivelEducativo.NivelEducativoID
             s_Var.Append(" AND NivelEducativoID = @dbp4ram10 ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "dbp4ram10";
             sqlParam.Value = anterior.NivelEducativo.NivelEducativoID;
             sqlParam.DbType = DbType.Int32;
             sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.Grado == null)
             s_Var.Append(" AND Grado is NULL ");
         else
         {
             // anterior.Grado
             s_Var.Append(" AND Grado = @dbp4ram11 ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "dbp4ram11";
             sqlParam.Value = anterior.Grado;
             sqlParam.DbType = DbType.Int16;
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
            throw new Exception("AreaProfesionalizacionUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("AreaProfesionalizacionUpdHlp: Hubo  un Error al Actualizar el Registro .");
      }
   } 
}
