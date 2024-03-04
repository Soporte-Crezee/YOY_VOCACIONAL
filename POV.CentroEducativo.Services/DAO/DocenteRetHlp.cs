// Clase motor de red social
using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.DAO;
using POV.Localizacion.Service;
using POV.Localizacion.BO;

namespace POV.CentroEducativo.DAO
{
    /// <summary>
    /// Consulta un registro de Docente en la BD
    /// </summary>
    public class DocenteRetHlp
    {
        /// <summary>
        /// Consulta registros de Docente en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="docente">Docente que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de Docente generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, Docente docente)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (docente == null)
                sError += ", Docente";
            if (sError.Length > 0)
                throw new Exception("DocenteRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));

            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO",
                   "DocenteRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "DocenteRetHlp: No se pudo conectar a la base de datos", "POV.CentroEducativo.DAO",
                   "DocenteRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT DocenteID, CURP, Nombre, PrimerApellido, SegundoApellido, Estatus, FechaRegistro, FechaNacimiento, Sexo, Correo, Clave, EstatusIdentificacion,");
            sCmd.Append(" Cedula,NivelEstudio,Titulo,Especialidades,Experiencia,Cursos,UsuarioSkype,EsPremium,PermiteAsignaciones"); 
            sCmd.Append(" FROM Docente ");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (docente.DocenteID != null)
            {
                s_VarWHERE.Append(" DocenteID = @docente_DocenteID ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "docente_DocenteID";
                sqlParam.Value = docente.DocenteID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (docente.Curp != null)
            {
                s_VarWHERE.Append(" AND Curp = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = docente.Curp;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (docente.Nombre != null)
            {
                s_VarWHERE.Append(" AND Nombre = @docente_Nombre ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "docente_Nombre";
                sqlParam.Value = docente.Nombre;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (docente.PrimerApellido != null)
            {
                s_VarWHERE.Append(" AND PrimerApellido = @docente_PrimerApellido ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "docente_PrimerApellido";
                sqlParam.Value = docente.PrimerApellido;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (docente.SegundoApellido != null)
            {
                s_VarWHERE.Append(" AND SegundoApellido = @docente_SegundoApellido ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "docente_SegundoApellido";
                sqlParam.Value = docente.SegundoApellido;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (docente.Estatus != null)
            {
                s_VarWHERE.Append(" AND Estatus = @docente_Estatus ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "docente_Estatus";
                sqlParam.Value = docente.Estatus;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }

            if (docente.FechaRegistro != null)
            {
                s_VarWHERE.Append(" AND FechaRegistro = @docente_FechaRegistro ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "docente_FechaRegistro";
                sqlParam.Value = docente.FechaRegistro;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (docente.FechaNacimiento != null)
            {
                s_VarWHERE.Append(" AND FechaNacimiento = @docente_FechaNacimiento ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "docente_FechaNacimiento";
                sqlParam.Value = docente.FechaNacimiento;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (docente.Sexo != null)
            {
                s_VarWHERE.Append(" AND Sexo = @docente_Sexo ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "docente_Sexo";
                sqlParam.Value = docente.Sexo;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (docente.Correo != null)
            {
                s_VarWHERE.Append(" AND Correo = @docente_Correo ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "docente_Correo";
                sqlParam.Value = docente.Correo;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (docente.Clave != null)
            {
                s_VarWHERE.Append(" AND Clave = @docente_Clave ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "docente_Clave";
                sqlParam.Value = docente.Clave;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (docente.EstatusIdentificacion != null)
            {
                s_VarWHERE.Append(" AND EstatusIdentificacion = @docente_EstatusIdentificacion ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "docente_EstatusIdentificacion";
                sqlParam.Value = docente.EstatusIdentificacion;
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
                sqlAdapter.Fill(ds, "Docente");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("DocenteRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
