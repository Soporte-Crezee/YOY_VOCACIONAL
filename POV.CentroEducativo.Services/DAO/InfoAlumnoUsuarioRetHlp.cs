using POV.CentroEducativo.BO;
using POV.Prueba.Diagnostico.Dinamica.BO;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace POV.CentroEducativo.DAO
{
    /// <summary>
    /// Consulta una vista con datos de alumno y usuario en la BD
    /// </summary>
    class InfoAlumnoUsuarioRetHlp
    {
        public DataSet Action(IDataContext dctx, InfoAlumnoUsuario infoAlumnoUsuario) 
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (infoAlumnoUsuario == null)
                sError += ", Alumno";
            if (sError.Length > 0)
                throw new Exception("InfoAlumnoUsuarioRetHlp : Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.Services.DAO", "InfoAlumnoUsuarioRetHlp", "Action", null, null);

            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "InfoAlumnoUsuarioRetHlp: No se pudo conectar a la base de datos", "POV.CentroEducativo.Services.DAO", "InfoAlumnoUsuarioRetHlp", "Action", null, null);
            }

            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT AlumnoID, Nombre, PrimerApellido, SegundoApellido, Escuela, Grado, EstadoID, Premium, RecibirInformacion, Email, AreaConocimientoID, UsuarioID, DatosCompletos ");
            sCmd.Append(" FROM ViewExpedienteAlumno ");

            StringBuilder s_VarWHERE = new StringBuilder();
            if (infoAlumnoUsuario.clasificador.ClasificadorID != null)
            {
                s_VarWHERE.Append(" AreaConocimientoID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = infoAlumnoUsuario.clasificador.ClasificadorID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (infoAlumnoUsuario.Escuela != null)
            {
                s_VarWHERE.Append(" AND Escuela = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = infoAlumnoUsuario.Escuela;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (infoAlumnoUsuario.estado.EstadoID != null)
            {
                s_VarWHERE.Append(" AND EstadoID = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = infoAlumnoUsuario.estado.EstadoID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (infoAlumnoUsuario.Premium != null)
            {
                s_VarWHERE.Append(" AND Premium = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = infoAlumnoUsuario.Premium;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (infoAlumnoUsuario.Grado != null)
            {
                s_VarWHERE.Append(" AND Grado = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = infoAlumnoUsuario.Grado;
                sqlParam.DbType = DbType.Byte;
                sqlCmd.Parameters.Add(sqlParam);
            }

            if (infoAlumnoUsuario.Email != null)
            {
                s_VarWHERE.Append(" AND Email = @dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = infoAlumnoUsuario.Email;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            
            if (infoAlumnoUsuario.RecibirInformacion != null)
            {
                s_VarWHERE.Append(" AND DocenteID = @dbp4ram8 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram8";
                sqlParam.Value = infoAlumnoUsuario.RecibirInformacion;
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
                sqlAdapter.Fill(ds, "ViewExpedienteAlumno");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("InfoAlumnoUsuarioRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
