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
    /// Consulta un registro de Universidad en la BD
    /// </summary>
    internal class UniversidadRetHlp
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dctx"> El DataContext que proveerá acceso a la base de datos </param>
        /// <param name="universidad"> Universidad que provee el criterio de selección para realizar la consulta </param>
        /// <returns> El DataSet que contiene la información de la Universidad generada por la consulta </returns>
        public DataSet Action(IDataContext dctx, Universidad universidad) 
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (universidad == null)
                sError += ", Universidad";
            if (sError.Length > 0)
                throw new Exception("UniversidadRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));

            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", "UniversidadRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "UniversidadRetHlp: No se pudo conectar a la base de datos", "POV.CentroEducativo.DAO", "UniversidadRetHlp", "Action", null, null);
            }

            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT UniversidadID,NombreUniversidad,Descripcion,Direccion,PaginaWEB,CoordinadorCarrera,Activo,ClaveEscolar,Siglas,NivelEscolar ");
            sCmd.Append(" FROM Universidad ");
            StringBuilder s_Var = new StringBuilder();
            if (universidad.UniversidadID != null)
            {
                s_Var.Append(" UniversidadID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = universidad.UniversidadID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (universidad.NombreUniversidad != null)
            {
                s_Var.Append(" AND NombreUniversidad = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = universidad.NombreUniversidad;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (universidad.Descripcion != null)
            {
                s_Var.Append(" AND Descripcion = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = universidad.Descripcion;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (universidad.Direccion != null)
            {
                s_Var.Append(" AND Direccion = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = universidad.Direccion;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (universidad.PaginaWEB != null)
            {
                s_Var.Append(" AND PaginaWEB = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = universidad.PaginaWEB;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }            
            if (universidad.Activo != null)
            {
                s_Var.Append(" AND Activo = @dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = universidad.Activo;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (universidad.ClaveEscolar != null)
            {
                s_Var.Append(" AND ClaveEscolar = @dbp4ram7 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram7";
                sqlParam.Value = universidad.ClaveEscolar;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (universidad.Siglas != null)
            {
                s_Var.Append(" AND Siglas = @dbp4ram8 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram8";
                sqlParam.Value = universidad.Siglas;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }  
            if (universidad.NivelEscolar != null)
            {
                s_Var.Append(" AND NivelEscolar = @dbp4ram9 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram9";
                sqlParam.Value = universidad.NivelEscolar;
                sqlParam.DbType = DbType.Byte;
                sqlCmd.Parameters.Add(sqlParam);
            }

            s_Var.Append("  ");
            string s_Varres = s_Var.ToString().Trim();
            if (s_Varres.Length > 0)
            {
                if (s_Varres.StartsWith("AND "))
                    s_Varres = s_Varres.Substring(4);
                else if (s_Varres.StartsWith("OR "))
                    s_Varres = s_Varres.Substring(3);
                else if (s_Varres.StartsWith(","))
                    s_Varres = s_Varres.Substring(1);
                sCmd.Append(" WHERE " + s_Varres);
            }
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "Universidad");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("UniversidadRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
