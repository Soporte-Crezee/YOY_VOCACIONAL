using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using System;
using System.Data;
using System.Data.Common;
using System.Text;
namespace POV.CentroEducativo.DAO
{   
    public class EspecialistaRetHlp
    {
        public DataSet Action(IDataContext dctx, EspecialistaPruebas especialista)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (especialista == null)
                sError += ", Especialista";
            if (sError.Length > 0)
                throw new Exception("EspecialistaRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));

            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO",
                   "EspecialistaRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "EspecialistaRetHlp: No se pudo conectar a la base de datos", "POV.CentroEducativo.DAO",
                   "EspecialistaRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT EspecialistaID, CURP, Nombre, PrimerApellido, SegundoApellido, Estatus, FechaRegistro, FechaNacimiento, Sexo, Correo, Clave, EstatusIdentificacion ");
            sCmd.Append(" FROM EspecialistaPruebas ");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (especialista.EspecialistaPruebaID != null)
            {
                s_VarWHERE.Append(" EspecialistaID = @especialista_EspecialistaID ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "especialista_EspecialistaID";
                sqlParam.Value = especialista.EspecialistaPruebaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (especialista.Curp != null)
            {
                s_VarWHERE.Append(" AND Curp = @especialista_Curp ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "especialista_Curp";
                sqlParam.Value = especialista.Curp;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (especialista.Nombre != null)
            {
                s_VarWHERE.Append(" AND Nombre = @especialista_Nombre ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "especialista_Nombre";
                sqlParam.Value = especialista.Nombre;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (especialista.PrimerApellido != null)
            {
                s_VarWHERE.Append(" AND PrimerApellido = @especialista_PrimerApellido ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "especialista_PrimerApellido";
                sqlParam.Value = especialista.PrimerApellido;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (especialista.SegundoApellido != null)
            {
                s_VarWHERE.Append(" AND SegundoApellido = @especialista_SegundoApellido ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "especialista_SegundoApellido";
                sqlParam.Value = especialista.SegundoApellido;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (especialista.Estatus != null)
            {
                s_VarWHERE.Append(" AND Estatus = @especialista_Estatus ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "especialista_Estatus";
                sqlParam.Value = especialista.Estatus;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }

            if (especialista.FechaRegistro != null)
            {
                s_VarWHERE.Append(" AND FechaRegistro = @especialista_FechaRegistro ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "especialista_FechaRegistro";
                sqlParam.Value = especialista.FechaRegistro;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (especialista.FechaNacimiento != null)
            {
                s_VarWHERE.Append(" AND FechaNacimiento = @especialista_FechaNacimiento ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "especialista_FechaNacimiento";
                sqlParam.Value = especialista.FechaNacimiento;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (especialista.Sexo != null)
            {
                s_VarWHERE.Append(" AND Sexo = @especialista_Sexo ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "especialista_Sexo";
                sqlParam.Value = especialista.Sexo;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (especialista.Correo != null)
            {
                s_VarWHERE.Append(" AND Correo = @especialista_Correo ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "especialista_Correo";
                sqlParam.Value = especialista.Correo;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (especialista.Clave != null)
            {
                s_VarWHERE.Append(" AND Clave = @especialista_Clave ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "especialista_Clave";
                sqlParam.Value = especialista.Clave;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (especialista.EstatusIdentificacion != null)
            {
                s_VarWHERE.Append(" AND EstatusIdentificacion = @especialista_EstatusIdentificacion ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "especialista_EstatusIdentificacion";
                sqlParam.Value = especialista.EstatusIdentificacion;
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
                sqlAdapter.Fill(ds, "EspecialistaPruebas");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("EspecialistaRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
