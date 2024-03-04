using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;

namespace POV.CentroEducativo.DAO
{
    /// <summary>
    /// Consultar de la base de datos
    /// </summary>
    public class GrupoCicloEscolarRetHlp
    {
        /// <summary>
        /// Consulta registros de GrupoCicloEscolar en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="grupoCicloEscolar">GrupoCicloEscolar que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de GrupoCicloEscolar generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, GrupoCicloEscolar grupoCicloEscolar)
        {
            object myFirm = new object();
            string sError = "";
            if (grupoCicloEscolar == null)
                sError += ", grupoCicloEscolar";
            if (sError.Length > 0)
                throw new Exception("GrupoCicloEscolarRetHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
            if (grupoCicloEscolar.CicloEscolar == null)
            {
                grupoCicloEscolar.CicloEscolar = new CicloEscolar();
            }
            if (grupoCicloEscolar.PlanEducativo == null)
            {
                grupoCicloEscolar.PlanEducativo = new PlanEducativo();
            }
            if (grupoCicloEscolar.Escuela == null)
            {
                grupoCicloEscolar.Escuela = new Escuela();
            }
            if (grupoCicloEscolar.Grupo == null)
            {
                grupoCicloEscolar.Grupo = new Grupo();
            }
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", "GrupoCicloEscolarRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "GrupoCicloEscolarRetHlp: Hubo un error al conectarse a la base de datos", "POV.CentroEducativo.DAO", "GrupoCicloEscolarRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT GRUPOCICLOESCOLARID,GRUPOSOCIALID,CLAVE,CICLOESCOLARID,ESCUELAID,GRUPOID,PLANEDUCATIVOID,ACTIVO ");
            sCmd.Append(" FROM GRUPOCICLOESCOLAR ");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (grupoCicloEscolar.Escuela.EscuelaID != null)
            {
                s_VarWHERE.Append(" ESCUELAID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = grupoCicloEscolar.Escuela.EscuelaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (grupoCicloEscolar.Grupo.GrupoID != null)
            {
                s_VarWHERE.Append(" AND GRUPOID = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = grupoCicloEscolar.Grupo.GrupoID;
                sqlParam.DbType = DbType.Guid;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (grupoCicloEscolar.GrupoCicloEscolarID  != null)
            {
                s_VarWHERE.Append(" AND GRUPOCICLOESCOLARID = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = grupoCicloEscolar.GrupoCicloEscolarID;
                sqlParam.DbType = DbType.Guid;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (grupoCicloEscolar.GrupoSocialID != null)
            {
                s_VarWHERE.Append(" AND GRUPOSOCIALID = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = grupoCicloEscolar.GrupoSocialID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (grupoCicloEscolar.CicloEscolar.CicloEscolarID != null)
            {
                s_VarWHERE.Append(" AND CICLOESCOLARID = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = grupoCicloEscolar.CicloEscolar.CicloEscolarID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (grupoCicloEscolar.PlanEducativo.PlanEducativoID != null)
            {
                s_VarWHERE.Append(" AND PLANEDUCATIVOID = @dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = grupoCicloEscolar.PlanEducativo.PlanEducativoID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (grupoCicloEscolar.Activo != null)
            {
                s_VarWHERE.Append(" AND ACTIVO =  @dbp4ram7");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram7";
                sqlParam.Value = grupoCicloEscolar.Activo;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }

            string s_VarWHEREres = s_VarWHERE.ToString().Trim();
            if (s_VarWHEREres.Length > 0)
            {
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
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "GrupoCicloEscolar");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("GrupoCicloEscolarRetHlp: Hubo un error al consultar los registros. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            return ds;
        }
    }
}
