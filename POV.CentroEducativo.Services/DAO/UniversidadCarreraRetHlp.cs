using POV.CentroEducativo.BO;
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
    /// Consulta un registro de CarreraUnivrersidad en la BD
    /// </summary>
    public class UniversidadCarreraRetHlp
    {
        
            /// <summary>
            /// Consulta registros de UniversidadCarrera en la base de datos.
            /// </summary>
            /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
            /// <param name="universidadCarrera">UniversidadCarrera que provee el criterio de selección para realizar la consulta</param>
            /// <returns>El DataSet que contiene la información de UniversidadCarrera generada por la consulta</returns>
            public DataSet Action(IDataContext dctx, UniversidadCarrera universidadCarrera)
            {
                object myFirm = new object();
                string sError = String.Empty;
                if (universidadCarrera == null)
                    sError += ", UniversidadCarrera";
                if (sError.Length > 0)
                    throw new Exception("UniversidadCarreraRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));

                

                if (dctx == null)
                    throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", "UniversidadCarreraRetHlp", "Action", null, null);
                DbCommand sqlCmd = null;
                try
                {
                    dctx.OpenConnection(myFirm);
                    sqlCmd = dctx.CreateCommand();
                }
                catch (Exception ex)
                {
                    throw new StandardException(MessageType.Error, "", "UniversidadCarreraRetHlp: No se pudo conectar a la base de datos", "POV.CentroEducativo.DAO", "UniversidadCarreraRetHlp", "Action", null, null);
                }
                DbParameter sqlParam;
                StringBuilder sCmd = new StringBuilder();
                sCmd.Append(" SELECT UniversidadCarreraID,UniversidadID,CarreraID ");
                sCmd.Append(" FROM UniversidadCarrera ");
                StringBuilder s_Var = new StringBuilder();
                if (universidadCarrera.UniversidadCarreraID != null)
                {
                    s_Var.Append(" UniversidadCarreraID = @dbp4ram1 ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "dbp4ram1";
                    sqlParam.Value = universidadCarrera.UniversidadCarreraID;
                    sqlParam.DbType = DbType.Int32;
                    sqlCmd.Parameters.Add(sqlParam);
                }
                if (universidadCarrera.UniversidadID != null)
                {
                    s_Var.Append(" AND UniversidadID = @dbp4ram2 ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "dbp4ram2";
                    sqlParam.Value = universidadCarrera.UniversidadID;
                    sqlParam.DbType = DbType.Int64;
                    sqlCmd.Parameters.Add(sqlParam);
                }
                if (universidadCarrera.CarreraID != null)
                {
                    s_Var.Append(" AND CarreraID = @dbp4ram3 ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "dbp4ram3";
                    sqlParam.Value = universidadCarrera.CarreraID;
                    sqlParam.DbType = DbType.Int64;
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
                    sqlAdapter.Fill(ds, "UniversidadCarrera");
                }
                catch (Exception ex)
                {
                    string exmsg = ex.Message;
                    try { dctx.CloseConnection(myFirm); }
                    catch (Exception) { }
                    throw new Exception("UniversidadCarreraRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
