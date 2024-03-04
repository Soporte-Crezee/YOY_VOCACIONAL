using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Profesionalizacion.BO;

namespace POV.Profesionalizacion.DAO { 
   /// <summary>
   /// Consultar de la base de datos
   /// </summary>
   internal class SituacionAprendizajeRetHlp { 
      /// <summary>
      /// Consulta registros de SituacionAprendizaje en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="situacionAprendizaje">SituacionAprendizaje que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de SituacionAprendizaje generada por la consulta</returns>
      internal DataSet Action(IDataContext dctx, EjeTematico ejeTematico,SituacionAprendizaje situacion){
         object myFirm = new object();
         string sError = "";
         if (situacion == null)
            sError += ", SituacionAprendizaje";
         if (ejeTematico == null)
            sError += ", EjeTematico";
         if (sError.Length > 0)
            throw new Exception("RetHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DAO", 
         "SituacionAprendizajeRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "SituacionAprendizajeRetHlp: Hubo un error al conectarse a la base de datos", "POV.Profesionalizacion.DAO", 
         "SituacionAprendizajeRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT SituacionAprendizajeID,Nombre,Descripcion,EstatusProfesionalizacion,FechaRegistro,EjeTematicoID,AgrupadorContenidoDigitalID ");
         sCmd.Append(" FROM SituacionAprendizaje ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (situacion.SituacionAprendizajeID != null){
            s_VarWHERE.Append(" SituacionAprendizajeID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = situacion.SituacionAprendizajeID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (situacion.Nombre != null){
            s_VarWHERE.Append(" AND Nombre LIKE @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = situacion.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
        
         if (situacion.FechaRegistro != null){
            s_VarWHERE.Append(" AND FechaRegistro = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = situacion.FechaRegistro;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (ejeTematico.EjeTematicoID != null){
            s_VarWHERE.Append(" AND EjeTematicoID = @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = ejeTematico.EjeTematicoID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }

         if (situacion.AgrupadorContenidoDigital != null)
         {
             if (situacion.AgrupadorContenidoDigital.AgrupadorContenidoDigitalID != null)
             {
                 s_VarWHERE.Append(" AND AgrupadorContenidoDigitalID =@dbp4ram5 ");
                 sqlParam = sqlCmd.CreateParameter();
                 sqlParam.ParameterName = "dbp4ram5";
                 sqlParam.Value = situacion.AgrupadorContenidoDigital.AgrupadorContenidoDigitalID;
                 sqlParam.DbType = DbType.Int64;
                 sqlCmd.Parameters.Add(sqlParam);
             }   
         }

         if (situacion.EstatusProfesionalizacion != null)
         {
             s_VarWHERE.Append(" AND EstatusProfesionalizacion = @dbp4ram6 ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "dbp4ram6";
             sqlParam.Value = situacion.EstatusProfesionalizacion;
             sqlParam.DbType = DbType.Byte;
             sqlCmd.Parameters.Add(sqlParam);
         }
         else 
         {
             //Si el estado es nulo se consultaran las situacionesAprendizaje con estado Activo o  Mantenimiento.
             s_VarWHERE.Append(" AND (EstatusProfesionalizacion = @dbp4ram7  OR EstatusProfesionalizacion = @dbp4ram8 )");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "dbp4ram7";
             sqlParam.Value = EEstatusProfesionalizacion.ACTIVO;
             sqlParam.DbType = DbType.Byte;
             sqlCmd.Parameters.Add(sqlParam);

             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "dbp4ram8";
             sqlParam.Value = EEstatusProfesionalizacion.MANTENIMIENTO;
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
         sCmd.Append(" ORDER BY SituacionAprendizajeID ASC ");
         DataSet ds = new DataSet();
         DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
         sqlAdapter.SelectCommand = sqlCmd;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            sqlAdapter.Fill(ds, "SituacionAprendizaje");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("SituacionAprendizajeRetHlp: Hubo un error al consultar los registros. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
