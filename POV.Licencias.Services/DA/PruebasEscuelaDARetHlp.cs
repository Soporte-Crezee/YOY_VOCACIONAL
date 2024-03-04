using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Licencias.BO;

namespace POV.Licencias.DA { 
   /// <summary>
   /// Consulta un registro de PruebasEscuela en la BD
   /// </summary>
   public class PruebasEscuelaDARetHlp { 
      /// <summary>
      /// Consulta registros de pruebas asignadas a la escuela en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="contrato">contrato que servirá como filtro</param>
      /// <param name="cicloContrato">cicloContrato que servirá como filtro</param>
      /// <param name="licenciaEscuela">licenciaEscuela que servirá como filtro</param>
      /// <returns>El DataSet que contiene la información de PruebasEscuela generada por la consulta</returns>
       public DataSet Action(IDataContext dctx, Contrato contrato, CicloContrato cicloContrato, LicenciaEscuela licenciaEscela)
       {
         object myFirm = new object();
         string sError = String.Empty;

         if (contrato == null)
            sError += ", Contrato";
         if (cicloContrato == null)
             sError += ", CicloContrato";
         if (licenciaEscela == null)
             sError += ", LicenciaEscuela";
         if (sError.Length > 0)
            throw new Exception("PruebasEscuelaDARetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));

         if (contrato.ContratoID == null)
             sError += ", Contrato";
         if (cicloContrato.CicloEscolar == null)
             sError += ", Ciclo escolar del cicloContrato";
         if (licenciaEscela.Escuela == null)
             sError += ", Escuela de la licenciaEscuela";
         if (sError.Length > 0)
             throw new Exception("PruebasContratoDARetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));

         if (cicloContrato.CicloEscolar.CicloEscolarID == null)
             sError += ", Identificador del ciclo escolar";
         if (licenciaEscela.Escuela.EscuelaID == null)
             sError += ", Identificador de la escuela";
         if (sError.Length > 0)
             throw new Exception("PruebasContratoDARetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));

         if (dctx == null)
            throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Licencias.DA", 
                "PruebasEscuelaDARetHlp", "Action", null, null);

         DbCommand sqlCmd = null;
         try
         {
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         }
         catch(Exception ex)
         {
            throw new StandardException(MessageType.Error, "", "PruebasEscuelaDARetHlp: No se pudo conectar a la base de datos", "POV.Licencias.DA", 
                "PruebasEscuelaDARetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();

         sCmd.Append(" SELECT ");
         sCmd.Append(" le.CicloEscolarID, le.ContratoID, le.EscuelaID, p.Nombre, p.Tipo, ");
         sCmd.Append(" pc.PruebaContratoID, pc.RecursoContratoID, pc.PruebaID, pc.TipoPruebaContrato, pc.FechaRegistro, pc.Activo ");
         sCmd.Append(" FROM LicenciaEscuela le ");
         sCmd.Append(" INNER JOIN Contrato c ON le.ContratoID = c.ContratoID ");
         sCmd.Append(" INNER JOIN CicloContrato cc ON c.ContratoID = cc.ContratoID ");
         sCmd.Append(" INNER JOIN RecursoContrato rc ON cc.CicloContratoID = rc.CicloContratoID ");
         sCmd.Append(" INNER JOIN PruebaContrato pc ON rc.RecursoContratoID = pc.RecursoContratoID ");
         sCmd.Append(" INNER JOIN Prueba p ON pc.PruebaID = p.PruebaID ");
         
         StringBuilder s_VarWHERE = new StringBuilder();
         if (contrato.ContratoID != null)
         {
             s_VarWHERE.Append(" c.ContratoID = @dbp4ram1 ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "dbp4ram1";
             sqlParam.Value = contrato.ContratoID;
             sqlParam.DbType = DbType.Int64;
             sqlCmd.Parameters.Add(sqlParam);
         }
         if (cicloContrato.CicloEscolar != null && cicloContrato.CicloEscolar.CicloEscolarID != null)
         {
             s_VarWHERE.Append(" AND cc.CicloEscolarID = @dbp4ram2 ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "dbp4ram2";
             sqlParam.Value = cicloContrato.CicloEscolar.CicloEscolarID;
             sqlParam.DbType = DbType.Int64;
             sqlCmd.Parameters.Add(sqlParam);
         }
         if (licenciaEscela.Escuela != null && licenciaEscela.Escuela.EscuelaID != null)
         {
             s_VarWHERE.Append(" AND le.EscuelaID = @dbp4ram3 ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "dbp4ram3";
             sqlParam.Value = licenciaEscela.Escuela.EscuelaID;
             sqlParam.DbType = DbType.Int64;
             sqlCmd.Parameters.Add(sqlParam);
         }

         s_VarWHERE.Append(" AND le.Activo = @dbp4ram4");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram4";
         sqlParam.Value = true;
         sqlParam.DbType = DbType.Byte;
         sqlCmd.Parameters.Add(sqlParam);

         s_VarWHERE.Append(" AND pc.Activo = @dbp4ram5");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram5";
         sqlParam.Value = true;
         sqlParam.DbType = DbType.Byte;
         sqlCmd.Parameters.Add(sqlParam);
         
         s_VarWHERE.Append("  ");
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
            sqlAdapter.Fill(ds, "PruebasEscuela");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("PruebasEscuelaDARetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
