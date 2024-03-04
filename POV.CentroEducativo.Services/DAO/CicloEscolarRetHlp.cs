// Ciclo escolar
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.Localizacion.BO;

namespace POV.CentroEducativo.DAO
{
    /// <summary>
    /// CicloEscolarRetHlp
    /// </summary>
    public class CicloEscolarRetHlp
    {
        /// <summary>
        /// Consulta registros de CicloEscolar en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="cicloEscolar">CicloEscolar que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de CicloEscolar generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, CicloEscolar cicloEscolar)
        {
            object myFirm = new object();
            string sError = "";
            if (cicloEscolar == null)
                sError += ", CicloEscolar";
            if (sError.Length > 0)
                throw new Exception("CicloEscolarRetHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (cicloEscolar.UbicacionID == null)
                cicloEscolar.UbicacionID = new Ubicacion();

            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", "CicloEscolarRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "CicloEscolarRetHlp: Hubo un error al conectarse a la base de datos", "POV.CentroEducativo.DAO", "CicloEscolarRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT CicloEscolarID, Titulo, Descripcion, InicioCiclo, FinCiclo, UbicacionID,Activo ");
            sCmd.Append(" FROM CicloEscolar ");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (cicloEscolar.CicloEscolarID != null)
            {
                s_VarWHERE.Append(" CicloEscolarID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = cicloEscolar.CicloEscolarID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (cicloEscolar.UbicacionID.UbicacionID != null)
            {
                s_VarWHERE.Append(" AND UbicacionID =@dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = cicloEscolar.UbicacionID.UbicacionID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (cicloEscolar.Activo != null)
            {
                s_VarWHERE.Append(" AND Activo = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = cicloEscolar.Activo;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (cicloEscolar.Titulo != null)
            {
                s_VarWHERE.Append(" AND Titulo LIKE @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = cicloEscolar.Titulo;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (cicloEscolar.InicioCiclo != null)
            {
                s_VarWHERE.Append(" AND InicioCiclo = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = cicloEscolar.InicioCiclo;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (cicloEscolar.FinCiclo != null)
            {
                s_VarWHERE.Append(" AND FinCiclo = @dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = cicloEscolar.FinCiclo;
                sqlParam.DbType = DbType.DateTime;
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
            sCmd.Append(" ORDER BY CicloEscolarID ASC ");
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "CicloEscolar");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("CicloEscolarRetHlp: Hubo un error al consultar los registros. " + exmsg);
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
