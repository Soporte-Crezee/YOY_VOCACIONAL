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
   /// Consulta un registro de EjeTematico en la BD
   /// </summary>
   internal class EjeTematicoRetHlp { 
      /// <summary>
      /// Consulta registros de EjeTematico en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="ejeTematico">EjeTematico que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de EjeTematico generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, EjeTematico ejeTematico){
         object myFirm = new object();
         string sError = String.Empty;
         if (ejeTematico == null)
            sError += ", EjeTematico";
         if (sError.Length > 0)
            throw new Exception("EjeTematicoRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
          if(ejeTematico.AreaProfesionalizacion==null)
          ejeTematico.AreaProfesionalizacion = new AreaProfesionalizacion();
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DAO", 
         "EjeTematicoRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "EjeTematicoRetHlp: No se pudo conectar a la base de datos", "POV.Profesionalizacion.DAO", 
         "EjeTematicoRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT EjeTematicoID,Nombre,Descripcion,FechaRegistro,EstatusProfesionalizacion,AreaProfesionalizacionID ");
         sCmd.Append(" FROM EjeTematico ");
         
         StringBuilder s_Var = new StringBuilder();
         if (ejeTematico.EjeTematicoID != null){
            s_Var.Append(" EjeTematicoID= @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = ejeTematico.EjeTematicoID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (ejeTematico.EstatusProfesionalizacion != null)
         {
             s_Var.Append(" AND EstatusProfesionalizacion= @dbp4ram2 ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "dbp4ram2";
             sqlParam.Value = ejeTematico.EstatusProfesionalizacion;
             sqlParam.DbType = DbType.Byte;
             sqlCmd.Parameters.Add(sqlParam);
         }
         else {

             s_Var.Append("AND (  EstatusProfesionalizacion= @dbp4ram444 ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "dbp4ram444";
             sqlParam.Value = EEstatusProfesionalizacion.ACTIVO;
             sqlParam.DbType = DbType.Byte;
             sqlCmd.Parameters.Add(sqlParam);

             s_Var.Append(" OR EstatusProfesionalizacion= @dbp4ram555 ) ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "dbp4ram555";
             sqlParam.Value = EEstatusProfesionalizacion.MANTENIMIENTO;
             sqlParam.DbType = DbType.Byte;
             sqlCmd.Parameters.Add(sqlParam);

         }
         if (ejeTematico.Nombre != null){
            s_Var.Append(" AND Nombre LIKE  @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = ejeTematico.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (ejeTematico.Descripcion != null){
            s_Var.Append(" AND Descripcion= @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = ejeTematico.Descripcion;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (ejeTematico.AreaProfesionalizacion.AreaProfesionalizacionID != null){
            s_Var.Append(" AND AreaProfesionalizacionID= @dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            sqlParam.Value = ejeTematico.AreaProfesionalizacion.AreaProfesionalizacionID;
            sqlParam.DbType = DbType.Int32;
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
            sCmd.Append(" WHERE " + s_Varres);
         }
         DataSet ds = new DataSet();
         DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
         sqlAdapter.SelectCommand = sqlCmd;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            sqlAdapter.Fill(ds, "EjeTematico");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("EjeTematicoRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
