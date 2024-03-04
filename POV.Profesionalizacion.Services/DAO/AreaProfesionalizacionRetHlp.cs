using System;
using System.Data;
using System.Data.Common;
using System.Text;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.Profesionalizacion.BO;

namespace POV.Profesionalizacion.DAO { 
   /// <summary>
   /// Consulta un registro de AreaProfesionalizacion en la BD
   /// </summary>
   internal class AreaProfesionalizacionRetHlp { 
      /// <summary>
      /// Consulta registros de AreaProfesionalizacion en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="areaProfesionalizacion">AreaProfesionalizacion que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de AreaProfesionalizacion generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, AreaProfesionalizacion areaProfesionalizacion){
         object myFirm = new object();
         string sError = String.Empty;
         if (areaProfesionalizacion == null)
            sError += ", AreaProfesionalizacion";
         if (areaProfesionalizacion.NivelEducativo == null)
         {
             areaProfesionalizacion.NivelEducativo = new NivelEducativo();
         }
         if (sError.Length > 0)
            throw new Exception("AreaProfesionalizacionRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "Namespace.TipoControlador", 
         "AreaProfesionalizacionRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "AreaProfesionalizacionRetHlp: No se pudo conectar a la base de datos", "Namespace.TipoControlador", 
         "AreaProfesionalizacionRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT AreaProfesionalizacionID, Nombre, Descripcion, FechaRegistro, Activo, NivelEducativoID, Grado ");
         sCmd.Append(" FROM AreaProfesionalizacion ");
         
         StringBuilder s_Var = new StringBuilder();
         if (areaProfesionalizacion.AreaProfesionalizacionID != null){
            s_Var.Append(" AreaProfesionalizacionID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = areaProfesionalizacion.AreaProfesionalizacionID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (areaProfesionalizacion.Nombre != null){
            s_Var.Append(" AND Nombre LIKE @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = areaProfesionalizacion.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (areaProfesionalizacion.Descripcion != null){
            s_Var.Append(" AND Descripcion LIKE @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = areaProfesionalizacion.Descripcion;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (areaProfesionalizacion.FechaRegistro != null){
            s_Var.Append(" AND FechaRegistro = @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = areaProfesionalizacion.FechaRegistro;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (areaProfesionalizacion.Activo != null){
            s_Var.Append(" AND Activo = @dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            sqlParam.Value = areaProfesionalizacion.Activo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (areaProfesionalizacion.NivelEducativo.NivelEducativoID != null)
         {
             s_Var.Append(" AND NivelEducativoID = @dbp4ram6 ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "dbp4ram6";
             sqlParam.Value = areaProfesionalizacion.NivelEducativo.NivelEducativoID;
             sqlParam.DbType = DbType.Int32;
             sqlCmd.Parameters.Add(sqlParam);
         }
         if (areaProfesionalizacion.Grado != null)
         {
             s_Var.Append(" AND Grado = @dbp4ram7 ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "dbp4ram7";
             sqlParam.Value = areaProfesionalizacion.Grado;
             sqlParam.DbType = DbType.Byte;
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
            sCmd.Append(" WHERE  " + s_Varres);
         }
         DataSet ds = new DataSet();
         DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
         sqlAdapter.SelectCommand = sqlCmd;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            sqlAdapter.Fill(ds, "AreaProfesionalizacion");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("AreaProfesionalizacionRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
