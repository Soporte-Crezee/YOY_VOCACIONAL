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
    /// AsignacionMateriaGrupo
    /// </summary>
    public class AsignacionMateriaGrupoRetHlp
    {
        /// <summary>
        /// Consulta registros de AsignacionMateriaGrupo en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="asignacion">AsignacionMateriaGrupo que provee el criterio de selección para realizar la consulta</param>
        /// <param name="grupoCicloEscolar">GrupoCicloEscolar que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de AsignacionMateriaGrupo generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, AsignacionMateriaGrupo asignacion, GrupoCicloEscolar grupoCicloEscolar)
        {
            object myFirm = new object();
            string sError = "";
            if (grupoCicloEscolar == null)
                sError += ", GrupoCicloEscolar";
            if (sError.Length > 0)
                throw new Exception("AsignacionMateriaGrupoRetHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
            if (grupoCicloEscolar.GrupoCicloEscolarID == null)
                sError += ", GrupoCicloEscolarID";
            if (sError.Length > 0)
                throw new Exception("AsignacionMateriaGrupoRetHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
            if (asignacion == null)
            {
                asignacion = new AsignacionMateriaGrupo();
            }
            if (asignacion.Docente == null)
            {
                asignacion.Docente = new Docente();
            }
            if (asignacion.Materia == null)
            {
                asignacion.Materia = new Materia();
            }
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", "AsignacionMateriaGrupoRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "AsignacionMateriaGrupoRetHlp: Hubo un error al conectarse a la base de datos", "POV.CentroEducativo.DAO", "AsignacionMateriaGrupoRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT ASIGNACIONMATERIAGRUPOID, DOCENTEID, MATERIAID, ACTIVO, FECHAREGISTRO, FECHABAJA, GRUPOCICLOESCOLARID ");
            sCmd.Append(" FROM ASIGNACIONMATERIAGRUPO ");
            if (grupoCicloEscolar.GrupoCicloEscolarID == null)
                sCmd.Append(" WHERE GRUPOCICLOESCOLARID IS NULL ");
            else
            {
                // grupoCicloEscolar.GrupoCicloEscolarID
                sCmd.Append(" WHERE GRUPOCICLOESCOLARID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = grupoCicloEscolar.GrupoCicloEscolarID;
                sqlParam.DbType = DbType.Guid;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (asignacion.AsignacionMateriaGrupoID != null)
            {
                sCmd.Append(" AND ASIGNACIONMATERIAGRUPOID = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = asignacion.AsignacionMateriaGrupoID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (asignacion.Docente.DocenteID != null)
            {
                sCmd.Append(" AND DOCENTEID = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = asignacion.Docente.DocenteID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (asignacion.Materia.MateriaID != null)
            {
                sCmd.Append(" AND MATERIAID = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = asignacion.Materia.MateriaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (asignacion.Activo != null)
            {
                sCmd.Append(" AND ACTIVO = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = asignacion.Activo;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (asignacion.FechaRegistro != null)
            {
                sCmd.Append(" AND FECHAREGISTRO = @dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = asignacion.FechaRegistro;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (asignacion.FechaBaja != null)
            {
                sCmd.Append(" AND FECHABAJA = @dbp4ram7 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram7";
                sqlParam.Value = asignacion.FechaBaja;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "AsignacionMateriaGrupo");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("AsignacionMateriaGrupoRetHlp: Hubo un error al consultar los registros. " + exmsg);
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
