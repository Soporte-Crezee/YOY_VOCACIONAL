using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using System;
using System.Data;
using System.Data.Common;
using System.Text;
namespace POV.CentroEducativo.DAO
{
    public class AsignacionEspecialistaEscuelaInsHlp
    {
        public void Action(IDataContext dctx, AsignacionEspecialistaEscuela asignacion, Escuela escuela)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (asignacion == null)
                sError += ", AsignacionEspecialistaEscuela";
            if (escuela == null)
                sError += ", Escuela";
            if (sError.Length > 0)
                throw new Exception("AsignacionEspecialistaEscuelaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (escuela.EscuelaID == null)
                sError += ", EscuelaID";
            if (asignacion.FechaRegistro == null)
                sError += ", FechaRegistro";
            if (asignacion.Activo == null)
                sError += ", Activo";
            if (asignacion.Especialista == null)
                sError += ", Especialista";
            if (sError.Length > 0)
                throw new Exception("AsignacionEspecialistaEscuelaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (asignacion.Especialista.EspecialistaPruebaID == null)
                sError += ", Especialista.EspecialistaPruebaID";
            if (sError.Length > 0)
                throw new Exception("AsignacionEspecialistaEscuelaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", "AsignacionEspecialistaEscuelaInsHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "AsignacionDocenteEscuelaInsHlp: No se pudo conectar a la base de datos", "POV.CentroEducativo.DAO", "AsignacionDocenteEscuelaInsHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" INSERT INTO ESPECIALISTAESCUELA (ESCUELAID,ESPECIALISTAID,ESTATUS,FECHAREGISTRO) ");
            // escuela.EscuelaID
            sCmd.Append(" VALUES (@dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            if (escuela.EscuelaID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = escuela.EscuelaID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            // asignacion.Especialista.EspecialistaPruebaID
            sCmd.Append(" ,@dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            if (asignacion.Especialista.EspecialistaPruebaID == null)
                sqlParam.Value = DBNull.Value;
            else
            sqlParam.Value = asignacion.Especialista.EspecialistaPruebaID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            // asignacion.Activo
            sCmd.Append(" ,@dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            if (asignacion.Activo == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = asignacion.Activo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            // asignacion.FechaRegistro
            sCmd.Append(" ,@dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            if (asignacion.FechaRegistro == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = asignacion.FechaRegistro;
            sqlParam.DbType = DbType.DateTime;
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
                throw new Exception("AsignacionEspecialistaEscuelaInsHlp: Ocurrio un error al ingresar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("AsignacionEspecialistaEscuelaInsHlp: Ocurrio un error al ingresar el registro.");
        }
    }
}
