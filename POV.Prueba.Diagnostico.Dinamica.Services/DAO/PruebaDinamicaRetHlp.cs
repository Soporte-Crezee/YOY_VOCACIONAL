using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Prueba.BO;
using POV.Prueba.Diagnostico.Dinamica.BO;

namespace POV.Prueba.Diagnostico.Dinamica.DAO { 
   /// <summary>
   /// Consulta un registro de PruebaDinamica en la BD
   /// </summary>
   internal class PruebaDinamicaRetHlp { 
      /// <summary>
      /// Consulta registros de PruebaDinamica en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="pruebaDinamica">PruebaDinamica que provee el criterio de selección para realizar la consulta</param>
       /// <param name="lTodas">Null || false: solamente trae las pruebas activas y liberadas ó aquellas que cumplan con la propiedad EstadoLiberacionPrueba; 
       /// true: recupera todas las pruebas sin importar su EstadoLiberacionPrueba</param>
      /// <returns>El DataSet que contiene la información de PruebaDinamica generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, PruebaDinamica pruebaDinamica, bool? lTodas){
         object myFirm = new object();
         string sError = String.Empty;
         if (pruebaDinamica == null)
            sError += ", PruebaDinamica";
         if (sError.Length > 0)
            throw new Exception("PruebaDinamicaRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Prueba.Diagnostico.Dinamica.DAO", 
         "PruebaDinamicaRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "PruebaDinamicaRetHlp: No se pudo conectar a la base de datos", "POV.Prueba.Diagnostico.Dinamica.DAO", 
         "PruebaDinamicaRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT p.PruebaID, p.Clave, p.Nombre, p.Instrucciones, p.FechaRegistro, p.EsDiagnostica, p.ModeloID, p.Tipo, p.EstadoLiberacion, p.EsPremium , p.TipoPruebaPresentacion ");
         sCmd.Append(" FROM Prueba p ");
         sCmd.Append(" INNER JOIN PruebaDinamica pe ON p.PruebaID = pe.PruebaID ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (pruebaDinamica.PruebaID != null){
            s_VarWHERE.Append(" p.PruebaID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = pruebaDinamica.PruebaID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (pruebaDinamica.Clave != null){
            s_VarWHERE.Append(" AND p.Clave = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = pruebaDinamica.Clave;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (pruebaDinamica.Nombre != null){
            s_VarWHERE.Append(" AND p.Nombre = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = pruebaDinamica.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (pruebaDinamica.FechaRegistro != null){
            s_VarWHERE.Append(" AND p.FechaRegistro = @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = pruebaDinamica.FechaRegistro;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (pruebaDinamica.EsDiagnostica != null){
            s_VarWHERE.Append(" AND p.EsDiagnostica = @dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            sqlParam.Value = pruebaDinamica.EsDiagnostica;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (pruebaDinamica.TipoPrueba != null){
            s_VarWHERE.Append(" AND p.Tipo = @dbp4ram7 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram7";
            sqlParam.Value = pruebaDinamica.TipoPrueba;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (lTodas == null || lTodas == false)
         {
             if (pruebaDinamica.EstadoLiberacionPrueba != null)
             {
                 s_VarWHERE.Append(" AND p.EstadoLiberacion = @dbp4ram8 ");
                 sqlParam = sqlCmd.CreateParameter();
                 sqlParam.ParameterName = "dbp4ram8";
                 sqlParam.Value = pruebaDinamica.EstadoLiberacionPrueba;
                 sqlParam.DbType = DbType.Byte;
                 sqlCmd.Parameters.Add(sqlParam);
             }
             else
             {
                 s_VarWHERE.Append(string.Format(" AND p.EstadoLiberacion IN ({0},{1}) ", 
                     (Byte)EEstadoLiberacionPrueba.ACTIVA, (Byte)EEstadoLiberacionPrueba.LIBERADA));
             }
         }
         if (pruebaDinamica.EsPremium != null)
         {
             s_VarWHERE.Append(" AND p.EsPremium = @dbp4ram9 ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "dbp4ram9";
             sqlParam.Value = pruebaDinamica.EsPremium;
             sqlParam.DbType = DbType.Boolean;
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
            sqlAdapter.Fill(ds, "Prueba");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("PruebaDinamicaRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
