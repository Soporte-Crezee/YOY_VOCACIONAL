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
    /// Actualiza un grupo en la base de datos
    /// </summary>
    public class GrupoUpdHlp
    {
        /// <summary>
        /// Actualiza de manera optimista un registro de Actualizar Grupo en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="actualizarGrupo">Actualizar Grupo que tiene los datos nuevos</param>
        /// <param name="anterior">Actualizar Grupo que tiene los datos anteriores</param>
        public void Action(IDataContext dctx, Grupo grupo, Grupo anterior, Escuela escuela)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (grupo == null)
                sError += ", Grupo";
            if (anterior == null)
                sError += ", Anterior";
            if (escuela == null)
                sError += ", Escuela";
            if (sError.Length > 0)
                throw new Exception("GrupoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (escuela.EscuelaID == null)
                sError += ", EscuelaID";
            if (grupo.GrupoID == null)
                sError += ", GrupoID";
            if (grupo.Nombre == null || grupo.Nombre.Trim().Length == 0)
                sError += ", Nombre";
            if (grupo.Grado == null)
                sError += ", Grado";
            if (anterior.GrupoID == null)
                sError += ", GrupoID anterior";
            if (anterior.Nombre == null || anterior.Nombre.Trim().Length == 0)
                sError += ", Nombre anterior";
            if (anterior.Grado == null)
                sError += ", Grado anterior";
            if (sError.Length > 0)
                throw new Exception("GrupoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (grupo.GrupoID != anterior.GrupoID)
            {
                sError = "Los parametros no coinciden";
            }
            if (sError.Length > 0)
                throw new Exception("GrupoUpdHlp: " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", "GrupoUpdHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "GrupoUpdHlp: Ocurrió un error al conectarse a la base de datos", "POV.CentroEducativo.DAO", "GrupoUpdHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" UPDATE GRUPO ");
            // grupo.Nombre
            sCmd.Append(" SET NOMBRE =@dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            if (grupo.Nombre == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = grupo.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // grupo.Grado
            sCmd.Append(" ,GRADO =@dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            if (grupo.Grado == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = grupo.Grado;
            sqlParam.DbType = DbType.Int16;
            sqlCmd.Parameters.Add(sqlParam);
            if (anterior.GrupoID == null)
                sCmd.Append(" WHERE GRUPOID IS NULL ");
            else
            {
                // anterior.GrupoID
                sCmd.Append(" WHERE GRUPOID = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = anterior.GrupoID;
                sqlParam.DbType = DbType.Guid;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.Nombre == null)
                sCmd.Append(" AND NOMBRE IS NULL ");
            else
            {
                // anterior.Nombre
                sCmd.Append(" AND NOMBRE = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = anterior.Nombre;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.Grado == null)
                sCmd.Append(" AND GRADO IS NULL ");
            else
            {
                // anterior.Grado
                sCmd.Append(" AND GRADO = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = anterior.Grado;
                sqlParam.DbType = DbType.Int16;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (escuela.EscuelaID == null)
                sCmd.Append(" AND ESCUELAID IS NULL ");
            else
            {
                // escuela.EscuelaID
                sCmd.Append(" AND ESCUELAID = @dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = escuela.EscuelaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            int iRes = 0;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                iRes = sqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("GrupoUpdHlp: Ocurrio un error al actualizar el objeto o fue modificado mientras era editado. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("GrupoUpdHlp: Ocurrio un error al actualizar el objeto o fue modificado mientras era editado.");
        }
    }
}
