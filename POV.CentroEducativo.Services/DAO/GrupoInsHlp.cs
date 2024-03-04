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
    /// Inserta un Grupo en la base de datos
    /// </summary>
    public class GrupoInsHlp
    {
        /// <summary>
        /// Crea un registro de Grupo en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="grupo">Grupo que desea crear</param>
        public void Action(IDataContext dctx, Grupo grupo, Escuela escuela)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (grupo == null)
                sError += ", grupo";
            if (escuela == null)
                sError += ", Escuela";
            if (sError.Length > 0)
                throw new Exception("GrupoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (escuela.EscuelaID == null)
                sError += ", EscuelaID";
            if (grupo.GrupoID == null)
                sError += ", GrupoID";
            if (grupo.Nombre == null || grupo.Nombre.Trim().Length == 0)
                sError += ", Nombre";
            if (grupo.Grado == null)
                sError += ", Grado";
            if (sError.Length > 0)
                throw new Exception("GrupoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", "GrupoInsHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "GrupoInsHlp: No se pudo conectar a la base de datos", "POV.CentroEducativo.DAO", "GrupoInsHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" INSERT INTO GRUPO (GRUPOID,NOMBRE,GRADO,ESCUELAID) ");
            // grupo.GrupoID
            sCmd.Append(" VALUES (@dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            if (grupo.GrupoID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = grupo.GrupoID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);
            // grupo.Nombre
            sCmd.Append(" ,@dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            if (grupo.Nombre == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = grupo.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // grupo.Grado
            sCmd.Append(" ,@dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            if (grupo.Grado == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = grupo.Grado;
            sqlParam.DbType = DbType.Int16;
            sqlCmd.Parameters.Add(sqlParam);
            // escuela.EscuelaID
            sCmd.Append(" ,@dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            if (escuela.EscuelaID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = escuela.EscuelaID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ) ");
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
                throw new Exception("GrupoInsHlp: Ocurrio un error al ingresar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("GrupoInsHlp: Ocurrio un error al ingresar el registro.");
        }
    }
}
