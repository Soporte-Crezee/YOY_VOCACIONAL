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
    public class GrupoRetHlp
    {
        /// <summary>
        /// Consulta registros de Grupo en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="grupo">Grupo que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de Grupo generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, Grupo grupo, Escuela escuela)
        {
            object myFirm = new object();
            string sError = "";
            if (escuela == null)
                sError += ", Escuela";
            if (sError.Length > 0)
                throw new Exception("GrupoRetHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
            if (escuela.EscuelaID == null)
                sError += ", EscuelaID";
            if (sError.Length > 0)
                throw new Exception("GrupoRetHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
            if (grupo == null)
            {
                grupo = new Grupo();
            }
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", "GrupoRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "GrupoRetHlp: Hubo un error al conectarse a la base de datos", "POV.CentroEducativo.DAO", "GrupoRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT GRUPOID,NOMBRE,GRADO,ESCUELAID ");
            sCmd.Append(" FROM GRUPO ");
            if (escuela.EscuelaID == null)
                sCmd.Append(" WHERE ESCUELAID IS NULL ");
            else
            {
                sCmd.Append(" WHERE ESCUELAID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = escuela.EscuelaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (grupo.GrupoID != null)
            {
                sCmd.Append(" AND GRUPOID = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = grupo.GrupoID;
                sqlParam.DbType = DbType.Guid;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (grupo.Nombre != null)
            {
                sCmd.Append(" AND NOMBRE = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = grupo.Nombre;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (grupo.Grado != null)
            {
                sCmd.Append(" AND GRADO = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = grupo.Grado;
                sqlParam.DbType = DbType.Int16;
                sqlCmd.Parameters.Add(sqlParam);
            }
            sCmd.Append(" ORDER BY GRADO, NOMBRE ");
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "Grupo");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("GrupoRetHlp: Hubo un error al consultar los registros. " + exmsg);
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
