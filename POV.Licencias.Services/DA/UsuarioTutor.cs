// Licencias de Tutor
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;

using POV.CentroEducativo.BO;
using POV.Seguridad.BO;

namespace POV.Licencias.DA
{
    /// <summary>
    /// UsuarioTutor
    /// </summary>
    internal class UsuarioTutor
    {
        /// <summary>
        /// Consulta registros de LicenciaTutor en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="Tutor">Tutor que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de LicenciaTutor generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, Usuario usuario, Tutor tutor)
        {
            object myFirm = new object();
            string sError = "";
            if (usuario == null && tutor==null)
                sError += ", usuario, tutor";
            if (sError.Length > 0)
                throw new Exception("UsuarioTutor: Almenos uno de los siguientes campos es necesario: " + sError.Substring(2)); 
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Licencias.DA", "UsuarioTutor", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "UsuarioTutor: Hubo un error al conectarse a la base de datos", "POV.Licencias.DA", "UsuarioTutor", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT DISTINCT us.UsuarioID, us.NombreUsuario, tut.TutorID, tut.nombre, us.AceptoTerminos, us.Email");
            sCmd.Append(" FROM Licencia lic ");
            sCmd.Append(" INNER JOIN LicenciaReferencia licRef ON (licRef.LicenciaID = lic.LicenciaID) ");
            sCmd.Append(" INNER JOIN Tutor tut ON (tut.TutorID = licRef.TutorID) ");
            sCmd.Append(" INNER JOIN Usuario us ON (us.UsuarioID = lic.UsuarioID) ");
            StringBuilder s_VarWHERE = new StringBuilder();

            if (usuario != null)
            {
                if (usuario.UsuarioID != null)
                {
                    s_VarWHERE.Append(" AND us.UsuarioID =@dbp4ram1 ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "dbp4ram1";
                    sqlParam.Value = usuario.UsuarioID;
                    sqlParam.DbType = DbType.String;
                    sqlCmd.Parameters.Add(sqlParam);
                }

                if (usuario.NombreUsuario != null)
                {
                    s_VarWHERE.Append(" AND us.NombreUsuario =@dbp4ram2 ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "dbp4ram2";
                    sqlParam.Value = usuario.NombreUsuario;
                    sqlParam.DbType = DbType.String;
                    sqlCmd.Parameters.Add(sqlParam);
                }

                if (usuario.Email != null)
                {
                    s_VarWHERE.Append(" AND us.Email =@dbp4ram3 ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "dbp4ram3";
                    sqlParam.Value = usuario.Email;
                    sqlParam.DbType = DbType.String;
                    sqlCmd.Parameters.Add(sqlParam);
                }
            }
            if (tutor != null)
            {
                if (tutor.TutorID != null)
                {
                    s_VarWHERE.Append(" AND tut.TutorID =@dbp4ram4 ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "dbp4ram4";
                    sqlParam.Value = tutor.TutorID;
                    sqlParam.DbType = DbType.String;
                    sqlCmd.Parameters.Add(sqlParam);
                }

                if (tutor.Nombre != null)
                {
                    s_VarWHERE.Append(" AND tut.Nombre =@dbp4ram5 ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "dbp4ram5";
                    sqlParam.Value = tutor.Nombre;
                    sqlParam.DbType = DbType.String;
                    sqlCmd.Parameters.Add(sqlParam);
                }
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
            sCmd.Append(" ORDER BY UsuarioID ASC ");
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "UsuarioTutor");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("UsuarioTutor: Hubo un error al consultar los registros. " + exmsg);
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
