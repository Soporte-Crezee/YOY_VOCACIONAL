// Clase motor de red social
using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Comun.BO;

namespace POV.Comun.DAO { 
   /// <summary>
   /// Consulta un registro de Colonia en la BD
   /// </summary>
   public class ColoniaRetHlp { 
      /// <summary>
      /// Consulta registros de Colonia en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="colonia">Colonia que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de Colonia generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, Colonia colonia){
         object myFirm = new object();
         string sError = String.Empty;
         if (colonia == null)
            sError += ", Colonia";
         if (sError.Length > 0)
            throw new Exception("ColoniaRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (colonia.Localidad == null)
            sError += ", Localidad";
         if (sError.Length > 0)
            throw new Exception("ColoniaRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (colonia.Localidad.LocalidadID == null)
            sError += ", LocalidadID";
         if (sError.Length > 0)
            throw new Exception("ColoniaRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Comun.DAO", 
         "ColoniaRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "ColoniaRetHlp: No se pudo conectar a la base de datos", "POV.Comun.DAO", 
         "ColoniaRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT ColoniaID, Nombre, FechaRegistro, LocalidadID ");
         sCmd.Append(" FROM Colonia ");
         sCmd.Append(" WHERE ");
         StringBuilder s_Var = new StringBuilder();
         if (colonia.ColoniaID != null){
            s_Var.Append(" ColoniaID = @colonia_ColoniaID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "colonia_ColoniaID";
            sqlParam.Value = colonia.ColoniaID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (colonia.Nombre != null){
            s_Var.Append(" AND Nombre= @colonia_Nombre ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "colonia_Nombre";
            sqlParam.Value = colonia.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (colonia.FechaRegistro != null){
            s_Var.Append(" AND FechaRegistro= @colonia_FechaRegistro ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "colonia_FechaRegistro";
            sqlParam.Value = colonia.FechaRegistro;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (colonia.Localidad.LocalidadID != null){
            s_Var.Append(" AND LocalidadID= @colonia_Localidad_LocalidadID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "colonia_Localidad_LocalidadID";
            sqlParam.Value = colonia.Localidad.LocalidadID;
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
            sCmd.Append("  " + s_Varres);
         }
         DataSet ds = new DataSet();
         DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
         sqlAdapter.SelectCommand = sqlCmd;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            sqlAdapter.Fill(ds, "Colonia");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("ColoniaRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
