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
    /// Consulta un registro de Escuela en la BD
    /// </summary>
    public class EscuelaRetHlp
    {
        /// <summary>
        /// Consulta registros de Escuela en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="escuela">Escuela que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de Escuela generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, Escuela escuela)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (escuela == null)
                sError += ", Escuela";
            if (sError.Length > 0)
                throw new Exception("EscuelaRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            
            if (escuela.Ubicacion == null)
                escuela.Ubicacion = new Ubicacion();
            if (escuela.DirectorID == null)
                escuela.DirectorID = new Director();
            if (escuela.TipoServicio == null)
                escuela.TipoServicio = new TipoServicio();
            if (escuela.ZonaID == null)
                escuela.ZonaID = new Zona();

            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", "EscuelaRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "EscuelaRetHlp: No se pudo conectar a la base de datos", "POV.CentroEducativo.DAO", "EscuelaRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT EscuelaID,Clave,NombreEscuela,FechaRegistro,UbicacionID,Estatus,Turno,TipoServicioID,ZonaID,DirectorID,Ambito,Control ");
            sCmd.Append(" FROM Escuela ");
            StringBuilder s_Var = new StringBuilder();
            if (escuela.EscuelaID != null)
            {
                s_Var.Append(" EscuelaID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = escuela.EscuelaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (escuela.Clave != null)
            {
                s_Var.Append(" AND Clave= @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = escuela.Clave;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (escuela.NombreEscuela != null)
            {
                s_Var.Append(" AND NombreEscuela LIKE @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = escuela.NombreEscuela;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (escuela.FechaRegistro != null)
            {
                s_Var.Append(" AND FechaRegistro= @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = escuela.FechaRegistro;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (escuela.Ubicacion.UbicacionID != null)
            {
                s_Var.Append(" AND UbicacionID= @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = escuela.Ubicacion.UbicacionID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (escuela.Estatus != null)
            {
                s_Var.Append(" AND Estatus= @dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = escuela.Estatus;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (escuela.Turno != null)
            {
                s_Var.Append(" AND Turno= @dbp4ram7 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram7";
                sqlParam.Value = escuela.ToShortTurno;
                sqlParam.DbType = DbType.Byte;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (escuela.TipoServicio.TipoServicioID != null)
            {
                s_Var.Append(" AND TipoServicioID= @dbp4ram8 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram8";
                sqlParam.Value = escuela.TipoServicio.TipoServicioID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (escuela.ZonaID.ZonaID != null)
            {
                s_Var.Append(" AND ZonaID= @dbp4ram9 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram9";
                sqlParam.Value = escuela.ZonaID.ZonaID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (escuela.DirectorID.DirectorID != null)
            {
                s_Var.Append(" AND DirectorID= @dbp4ram10 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram10";
                sqlParam.Value = escuela.DirectorID.DirectorID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (escuela.Ambito != null)
            {
                s_Var.Append(" AND Ambito= @dbp4ram11 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram11";
                sqlParam.Value = escuela.ToShortAmbito;
                sqlParam.DbType = DbType.Byte;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (escuela.Control != null)
            {
                s_Var.Append(" AND Control= @dbp4ram12 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram12";
                sqlParam.Value = escuela.ToShortControl;
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
                sqlAdapter.Fill(ds, "Escuela");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("EscuelaRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
