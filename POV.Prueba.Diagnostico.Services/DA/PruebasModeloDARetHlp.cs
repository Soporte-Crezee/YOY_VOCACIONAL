using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Modelo.BO;
using POV.Prueba.BO;

namespace POV.Prueba.Diagnostico.DA
{
    /// <summary>
    /// Consultar de la base de datos las pruebas de un modelo
    /// </summary>
    internal class PruebasModeloDARetHlp
    {
        /// <summary>
        /// Consulta registros de Modelo en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="modeloID">ID del Modelo que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de las pruebas del Modelo generada por la consulta</returns>        
        internal DataSet Action(IDataContext dctx, int? modeloID, Dictionary<string, string> parametros, EEstadoLiberacionPrueba? estadoLiberacion)
        {
            object myFirm = new object();
            string sError = "";

            if (parametros == null)
                sError += ", parámetros";
            if (sError.Length > 0)
                throw new Exception("PruebasModeloDARetHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Prueba.Diagnostico.DA",
                   "PruebasModeloDARetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "PruebasModeloDARetHlp: Hubo un error al conectarse a la base de datos", "POV.Prueba.Diagnostico.DA",
                   "PruebasModeloDARetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT DISTINCT PruebaID, Clave, Nombre, EstadoLiberacion, EsDiagnostica, ModeloID, Tipo ");
            sCmd.Append(" FROM Prueba ");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (modeloID != null)
            {
                s_VarWHERE.Append(" ModeloID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = modeloID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (parametros.ContainsKey("TipoPrueba"))
            {
                s_VarWHERE.Append(" AND Tipo = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = int.Parse(parametros["TipoPrueba"]);
                sqlParam.DbType = DbType.Int16;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (parametros.ContainsKey("EsDiagnostica"))
            {
                s_VarWHERE.Append(" AND EsDiagnostica = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = bool.Parse(parametros["EsDiagnostica"]);
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (estadoLiberacion != null)
            {
                s_VarWHERE.Append(" AND EstadoLiberacion = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = estadoLiberacion;
                sqlParam.DbType = DbType.Byte;
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
            sCmd.Append(" ORDER BY ModeloID ASC ");
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "PruebasModelo");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("PruebasModeloDARetHlp: Hubo un error al consultar los registros. " + exmsg);
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
