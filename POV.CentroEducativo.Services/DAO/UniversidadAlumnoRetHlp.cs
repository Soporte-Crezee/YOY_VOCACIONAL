using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace POV.CentroEducativo.DAO
{
    /// <summary>
    /// Consulta un registro de AlumnoUniversidad en la BD
    /// </summary>
    internal class UniversidadAlumnoRetHlp
    {
        /// <summary>
        /// Consulta registro de UniversidadAlumno en la base de datos
        /// </summary>
        /// <param name="dctx"> El DataContext que proveeerá acceso a la base de datos </param>
        /// <param name="usuarioExpediente"> UniversidadAlumno que porvee el criterio de selección para realizar la consulta </param>
        /// <returns> El DataSet que contiene la información de UniversidadAlumno generada por la consulta </returns>
        public DataSet Action(IDataContext dctx, UniversidadAlumno universidadAlumno)
        {
            object myFirm = new object();
            string sError = String.Empty;

            if (universidadAlumno == null)
                sError += ", AlumnoUniversidad";
            if (sError.Length > 0)
                throw new Exception("UniversidadAlumnoRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", "UniversidadAlumnoRetHlp", "Action", null, null);

            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "UniversidadAlumnoRetHlp: No se pudo conectar a la base de datos", "POV.Expediente.DAO", "UniversidadAlumnoRetHlp", "Action", null, null);
            }

            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT UniversidadID, AlumnoID ");
            sCmd.Append(" FROM UniversidadAlumno ");

            StringBuilder s_VarWHERE = new StringBuilder();

            if (universidadAlumno.UniversidadID != null)
            {
                s_VarWHERE.Append(" UniversidadID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = universidadAlumno.UniversidadID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }

            if (universidadAlumno.AlumnoID != null)
            {
                s_VarWHERE.Append(" AND AlumnoID = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = universidadAlumno.AlumnoID;
                sqlParam.DbType = DbType.Int32;
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
                sqlAdapter.Fill(ds, "UniversidadAlumno");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("UniversidadAlumnoRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
