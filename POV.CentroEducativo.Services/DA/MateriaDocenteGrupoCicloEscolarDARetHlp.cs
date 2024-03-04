using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.DAO;

namespace POV.CentroEducativo.DA { 
   /// <summary>
   /// Consulta un registro de Materia en la BD
   /// </summary>
   public class MateriaDocenteGrupoCicloEscolarDARetHlp { 
      /// <summary>
      /// Consulta registros de Docente en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="docente">Docente que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de Docente generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, Docente docente, GrupoCicloEscolar grupoCicloEscolar){
         object myFirm = new object();
         string sError = String.Empty;
         if (grupoCicloEscolar == null)
            sError += ", GrupoCicloEscolar";
         if (sError.Length > 0)
            throw new Exception("MateriaDocenteGrupoCicloEscolarDARetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (docente == null)
            sError += ", Docente";
         if (sError.Length > 0)
            throw new Exception("MateriaDocenteGrupoCicloEscolarDARetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (docente.DocenteID == null)
            sError += ", DocenteID";
         if (sError.Length > 0)
            throw new Exception("MateriaDocenteGrupoCicloEscolarDARetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
          if (grupoCicloEscolar.CicloEscolar == null)
            grupoCicloEscolar.CicloEscolar = new CicloEscolar();
         if (grupoCicloEscolar.Escuela == null)
             grupoCicloEscolar.Escuela = new Escuela();
         if (grupoCicloEscolar.Grupo == null)
             grupoCicloEscolar.Grupo = new Grupo();
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DA", 
         "MateriaDocenteGrupoCicloEscolarDARetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "MateriaDocenteGrupoCicloEscolarDARetHlp: No se pudo conectar a la base de datos", "POV.CentroEducativo.DA", 
         "MateriaDocenteGrupoCicloEscolarDARetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT amg.GrupoCicloEscolarID,  amg.MateriaID ");
         sCmd.Append(" FROM AsignacionMateriaGrupo amg ");
         sCmd.Append(" JOIN GrupoCicloEscolar gce ON (gce.GrupoCicloEscolarID = amg.GrupoCicloEscolarID) ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (docente.DocenteID != null){
            s_VarWHERE.Append(" amg.DocenteID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = docente.DocenteID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (grupoCicloEscolar.GrupoCicloEscolarID != null){
            s_VarWHERE.Append(" AND gce.GrupoCicloEscolarID = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = grupoCicloEscolar.GrupoCicloEscolarID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (grupoCicloEscolar.CicloEscolar.CicloEscolarID != null){
            s_VarWHERE.Append(" AND gce.CicloEscolarID = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = grupoCicloEscolar.CicloEscolar.CicloEscolarID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (grupoCicloEscolar.Escuela.EscuelaID != null){
            s_VarWHERE.Append(" AND gce.EscuelaID = @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = grupoCicloEscolar.Escuela.EscuelaID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (grupoCicloEscolar.Grupo.GrupoID != null){
            s_VarWHERE.Append(" AND gce.GrupoID = @dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            sqlParam.Value = grupoCicloEscolar.Grupo.GrupoID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (grupoCicloEscolar.GrupoSocialID != null){
            s_VarWHERE.Append(" AND gce.GrupoSocialID = @dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            sqlParam.Value = grupoCicloEscolar.GrupoSocialID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         s_VarWHERE.Append(" AND amg.Activo = 1 ");
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
            sqlAdapter.Fill(ds, "Materia");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("MateriaDocenteGrupoCicloEscolarDARetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
