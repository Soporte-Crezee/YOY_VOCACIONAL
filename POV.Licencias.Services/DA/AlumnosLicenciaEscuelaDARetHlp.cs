using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Licencias.BO;
using POV.CentroEducativo.BO;
using POV.Licencias.DA;

namespace POV.Licencias.DA { 
   /// <summary>
   /// Consulta un registros de Alumno dentro de una licencia escuela en la BD
   /// </summary>
   public class AlumnosLicenciaEscuelaDARetHlp { 
      /// <summary>
      /// Consulta registros de Alumno en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="alumno">Alumno que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de Alumno generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, LicenciaEscuela licenciaEscuela, Alumno alumno){
         object myFirm = new object();
         string sError = String.Empty;
         if (alumno == null)
            sError += ", Alumno";
         if (sError.Length > 0)
            throw new Exception("AlumnosLicenciaEscuelaRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (licenciaEscuela == null)
            sError += ", LicenciaEscuela";
         if (sError.Length > 0)
            throw new Exception("AlumnosLicenciaEscuelaRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
      if (licenciaEscuela.CicloEscolar == null) {
         licenciaEscuela.CicloEscolar = new CicloEscolar();
      }
      if (licenciaEscuela.Escuela == null) {
         licenciaEscuela.Escuela = new Escuela();
      }
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Licencias.DA", 
         "AlumnosLicenciaEscuelaDARetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "AlumnosLicenciaEscuelaRetHlp: No se pudo conectar a la base de datos", "POV.Licencias.DA", 
         "AlumnosLicenciaEscuelaDARetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT al.AlumnoID,al.Curp,al.Matricula,al.Nombre,al.PrimerApellido,al.SegundoApellido,al.FechaNacimiento,al.Direccion,al.NombreCompletoTutor,al.Estatus,al.FechaRegistro,al.Sexo ");
         sCmd.Append(" FROM LicenciaEscuela licEsc ");
         sCmd.Append(" INNER JOIN Licencia lic ON (lic.LicenciaEscuelaID=licEsc.LicenciaEscuelaID) ");
         sCmd.Append(" INNER JOIN LicenciaReferencia licRef ON (licRef.LicenciaID=lic.LicenciaID) ");
         sCmd.Append(" INNER JOIN Alumno al ON (al.AlumnoID=licRef.AlumnoID) ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (alumno.AlumnoID != null){
            s_VarWHERE.Append(" al.AlumnoID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = alumno.AlumnoID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (alumno.Curp != null){
            s_VarWHERE.Append(" AND al.Curp LIKE @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = alumno.Curp;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (alumno.Matricula != null){
            s_VarWHERE.Append(" AND al.Matricula LIKE @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = alumno.Matricula;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (alumno.Nombre != null){
            s_VarWHERE.Append(" AND al.Nombre LIKE @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = alumno.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (alumno.PrimerApellido != null){
            s_VarWHERE.Append(" AND al.PrimerApellido LIKE @dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            sqlParam.Value = alumno.PrimerApellido;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (alumno.SegundoApellido != null){
            s_VarWHERE.Append(" AND al.SegundoApellido LIKE @dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            sqlParam.Value = alumno.SegundoApellido;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (licenciaEscuela.CicloEscolar.CicloEscolarID != null){
            s_VarWHERE.Append(" AND licEsc.CicloEscolarID = @dbp4ram7 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram7";
            sqlParam.Value = licenciaEscuela.CicloEscolar.CicloEscolarID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (licenciaEscuela.Escuela.EscuelaID != null){
            s_VarWHERE.Append(" AND licEsc.EscuelaID = @dbp4ram8 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram8";
            sqlParam.Value = licenciaEscuela.Escuela.EscuelaID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (licenciaEscuela.LicenciaEscuelaID != null)
         {
             s_VarWHERE.Append(" AND licEsc.LicenciaEscuelaID = @dbp4ram9 ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "dbp4ram9";
             sqlParam.Value = licenciaEscuela.LicenciaEscuelaID;
             sqlParam.DbType = DbType.Int64;
             sqlCmd.Parameters.Add(sqlParam);
         }
        s_VarWHERE.Append(" AND licEsc.Activo =  1 ");
        s_VarWHERE.Append(" AND lic.Activo =  1 ");
            
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
            sqlAdapter.Fill(ds, "Alumno");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("AlumnosLicenciaEscuelaRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
