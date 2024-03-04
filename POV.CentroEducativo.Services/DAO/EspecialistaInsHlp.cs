using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using System;
using System.Data;
using System.Data.Common;
using System.Text;

namespace POV.CentroEducativo.DAO
{
    public class EspecialistaInsHlp
    {
        public void Action(IDataContext dctx, EspecialistaPruebas especialista)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (especialista == null)
                sError += ", Especialista";
            if (sError.Length > 0)
                throw new Exception("EspecialistaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));

            if (sError.Length > 0)
                throw new Exception("EspecialistaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", "EspecialistaInsHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "EspecialistaInsHlp: No se pudo conectar a la base de datos", "POV.CentroEducativo.DAO", "EspecialistaInsHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" INSERT INTO EspecialistaPruebas ( Nombre, CURP, PrimerApellido, SegundoApellido, Estatus, FechaRegistro, FechaNacimiento, Sexo, Correo, Clave, EstatusIdentificacion ) ");
            sCmd.Append(" VALUES ");
            sCmd.Append(" ( ");
            // especialista.Nombre
            sCmd.Append(" @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            if (especialista.Nombre == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = especialista.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // especialista.Curp
            sCmd.Append(" ,@dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            if (especialista.Curp == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = especialista.Curp;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // especialista.PrimerApellido
            sCmd.Append(" ,@dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            if (especialista.PrimerApellido == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = especialista.PrimerApellido;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // especialista.SegundoApellido
            sCmd.Append(" ,@dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            if (especialista.SegundoApellido == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = especialista.SegundoApellido;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // especialista.Estatus
            sCmd.Append(" ,@dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            if (especialista.Estatus == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = especialista.Estatus;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            // especialista.FechaRegistro
            sCmd.Append(" ,@dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            if (especialista.FechaRegistro == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = especialista.FechaRegistro;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
            // especialista.FechaNacimiento
            sCmd.Append(" ,@dbp4ram7 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram7";
            if (especialista.FechaNacimiento == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = especialista.FechaNacimiento;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
            // especialista.Sexo
            sCmd.Append(" ,@dbp4ram8 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram8";
            if (especialista.Sexo == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = especialista.Sexo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            // especialista.Correo
            sCmd.Append(" ,@dbp4ram9 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram9";
            if (especialista.Correo == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = especialista.Correo;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // especialista.Clave
            sCmd.Append(" ,@dbp4ram10 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram10";
            if (especialista.Clave == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = especialista.Clave;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // especialista.EstatusIdentificacion
            sCmd.Append(" ,@dbp4ram11 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram11";
            if (especialista.EstatusIdentificacion == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = especialista.EstatusIdentificacion;
            sqlParam.DbType = DbType.Boolean;
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
                throw new Exception("EspecialistaInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("EspecialistaInsHlp: Ocurrió un error al ingresar el registro.");

        }
    }
}
