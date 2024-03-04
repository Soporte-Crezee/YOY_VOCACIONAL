using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Profesionalizacion.BO;
using POV.Profesionalizacion.DAO;

namespace POV.Profesionalizacion.DAO { 
   /// <summary>
   /// Actualiza un registro de EjeTematico en la BD
   /// </summary>
   internal class EjeTematicoUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de EjeTematicoUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="ejeTematicoUpdHlp">EjeTematicoUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">EjeTematicoUpdHlp que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, EjeTematico ejeTematico, EjeTematico anterior){
         object myFirm = new object();
         String sError = string.Empty;
         if (ejeTematico == null)
            sError += ", EjeTematico";
         if (anterior == null)
            sError += ", Anterior";
         if (sError.Length > 0)
            throw new Exception("EjeTematicoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (anterior.EjeTematicoID == null)
            sError += ", Anterior EjeTematicoID";
         if (sError.Length > 0)
            throw new Exception("EjeTematicoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (anterior.AreaProfesionalizacion == null)
            sError += ", Anterior Profesionalizacion";
         if (ejeTematico.AreaProfesionalizacion == null)
            sError += ", EjeTematico Profesionalizacion";
         if (sError.Length > 0)
            throw new Exception("EjeTematicoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (anterior.AreaProfesionalizacion.AreaProfesionalizacionID == null)
            sError += ", Anterior ProfesionalizacionID";
         if (anterior.Nombre == null)
            sError += ", Anterior Nombre";
         if (ejeTematico.AreaProfesionalizacion.AreaProfesionalizacionID == null)
            sError += ", EjeTematico ProfesionalizacionID";
         if (ejeTematico.Nombre == null)
            sError += ", EjeTematico Nombre";
         if (sError.Length > 0)
            throw new Exception("EjeTematicoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DAO", 
         "EjeTematicoUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "EjeTematicoUpdHlpUpdHl: Hubo un error al conectarse a la base de datos", "POV.Profesionalizacion.DAO", 
         "EjeTematicoUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE EjeTematico ");
         if (ejeTematico.Nombre != null){
            sCmd.Append(" SET Nombre= @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = ejeTematico.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         // ejeTematico.Descripcion
         sCmd.Append(" ,Descripcion= @dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (ejeTematico.Descripcion == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = ejeTematico.Descripcion;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         if (ejeTematico.EstatusProfesionalizacion != null){
            sCmd.Append(" ,EstatusProfesionalizacion= @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = ejeTematico.EstatusProfesionalizacion;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (ejeTematico.AreaProfesionalizacion.AreaProfesionalizacionID != null){
            sCmd.Append(" ,AreaProfesionalizacionID= @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = ejeTematico.AreaProfesionalizacion.AreaProfesionalizacionID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         StringBuilder s_VarWHERE = new StringBuilder();
         if (anterior.EjeTematicoID != null)
         {
             s_VarWHERE.Append(" EjeTematicoID= @dbp4ram5 ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "dbp4ram5";
             sqlParam.Value = anterior.EjeTematicoID;
             sqlParam.DbType = DbType.Int64;
             sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.Nombre != null){
            s_VarWHERE.Append("AND Nombre= @dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            sqlParam.Value = anterior.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.Descripcion != null){
            s_VarWHERE.Append("AND Descripcion= @dbp4ram7 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram7";
            sqlParam.Value = anterior.Descripcion;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.AreaProfesionalizacion.AreaProfesionalizacionID != null){
            s_VarWHERE.Append("AND AreaProfesionalizacionID= @dbp4ram8 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram8";
            sqlParam.Value = anterior.AreaProfesionalizacion.AreaProfesionalizacionID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.EstatusProfesionalizacion != null){
            s_VarWHERE.Append("AND EstatusProfesionalizacion= @dbp4ram9 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram9";
            sqlParam.Value = anterior.EstatusProfesionalizacion;
            sqlParam.DbType = DbType.Byte;
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
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("EjeTematicoUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("EjeTematicoUpdHlp: Hubo  un Error al Actualizar el Registro .");
      }
   } 
}
