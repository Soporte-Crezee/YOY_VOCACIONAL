// Clase Ubicacion
using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Comun.BO;
using POV.Localizacion.BO;

namespace POV.Localizacion.DAO
{
    /// <summary>
    /// Consulta un registro de Ubicacion en la BD
    /// </summary>
    public class UbicacionExactoRetHlp
    {
        /// <summary>
        /// Consulta registros de Ubicacion en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="ubicacion">Ubicacion que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de Ubicacion generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, Ubicacion ubicacion)
        {
            object myFirm = new object();
            string sError = String.Empty;

            if (ubicacion == null)
                sError += ", Ubicacion";
            if (sError.Length > 0)
                throw new Exception("UbicacionExactoRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));

            if (ubicacion.Pais == null)
            {
                ubicacion.Pais = new Pais();
            }
            if (ubicacion.Estado == null)
            {
                ubicacion.Estado = new Estado();
            }
            if (ubicacion.Ciudad == null)
            {
                ubicacion.Ciudad = new Ciudad();
            }
            if (ubicacion.Localidad == null)
            {
                ubicacion.Localidad = new Localidad();
            }
            if (ubicacion.Colonia == null)
            {
                ubicacion.Colonia = new Colonia();
            }

            if (sError.Length > 0)
                throw new Exception("UbicacionExactoRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Localizacion.DAO", "UbicacionExactoRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "UbicacionExactoRetHlp: No se pudo conectar a la base de datos", "POV.Localizacion.DAO", "UbicacionExactoRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT UbicacionID, PaisID, EstadoID, CiudadID, LocalidadID, ColoniaID, FechaRegistro");
            sCmd.Append(" FROM Ubicacion ");
            sCmd.Append(" WHERE ");
            StringBuilder s_Var = new StringBuilder();
            if (ubicacion.Pais.PaisID == null)
                s_Var.Append(" AND PaisID IS NULL ");
            else
            {
                s_Var.Append(" AND PaisID= @ubicacion_Pais_PaisID ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "ubicacion_Pais_PaisID";
                sqlParam.Value = ubicacion.Pais.PaisID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (ubicacion.Estado.EstadoID == null)
                s_Var.Append(" AND EstadoID IS NULL ");
            else
            {
                s_Var.Append(" AND EstadoID= @ubicacion_Estado_EstadoID ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "ubicacion_Estado_EstadoID";
                sqlParam.Value = ubicacion.Estado.EstadoID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (ubicacion.Ciudad.CiudadID == null)
                s_Var.Append(" AND CiudadID IS NULL ");
            else
            {
                s_Var.Append(" AND CiudadID= @ubicacion_Ciudad_CiudadID ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "ubicacion_Ciudad_CiudadID";
                sqlParam.Value = ubicacion.Ciudad.CiudadID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (ubicacion.Localidad.LocalidadID == null)
                s_Var.Append(" AND LocalidadID IS NULL ");
            else
            {
                s_Var.Append(" AND LocalidadID= @ubicacion_Localidad_LocalidadID ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "ubicacion_Localidad_LocalidadID";
                sqlParam.Value = ubicacion.Localidad.LocalidadID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (ubicacion.Colonia.ColoniaID == null)
                s_Var.Append(" AND ColoniaID IS NULL ");
            else
            {
                s_Var.Append(" AND ColoniaID = @ubicacion_Colonia_ColoniaID ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "ubicacion_Colonia_ColoniaID";
                sqlParam.Value = ubicacion.Colonia.ColoniaID;
                sqlParam.DbType = DbType.Int32;
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
                sCmd.Append("  " + s_Varres);
            }
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "Ubicacion");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("UbicacionExactoRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
