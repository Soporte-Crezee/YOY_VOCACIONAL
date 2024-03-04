using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Modelo.BO;
using POV.Prueba.BO;

namespace POV.Prueba.Diagnostico.Dinamica.DAO
{
    /// <summary>
    /// Consulta un registro de AEscalaDinamica en la BD
    /// </summary>
    internal class EscalaDinamicaRetHlp
    {
        /// <summary>
        /// Consulta registros de AEscalaDinamica en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="prueba">Prueba relacionada</param>
        /// <param name="aEscalaDinamica">AEscalaDinamica que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de AEscalaDinamica generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, PruebaDinamica prueba, AEscalaDinamica escalaDinamica)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (escalaDinamica == null)
                sError += ", AEscalaDinamica";
            if (sError.Length > 0)
                throw new Exception("EscalaDinamicaRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (prueba == null)
                sError += ", PruebaDinamica";
            if (sError.Length > 0)
                throw new Exception("EscalaDinamicaRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (escalaDinamica.Clasificador == null)
            {
                escalaDinamica.Clasificador = new Clasificador();
            }
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Prueba.Diagnostico.Dinamica.DAO",
                   "EscalaDinamicaRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "EscalaDinamicaRetHlp: No se pudo conectar a la base de datos", "POV.Prueba.Diagnostico.Dinamica.DAO",
                   "EscalaDinamicaRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT PuntajeID, PruebaID, PuntajeMinimo, PuntajeMaximo, EsPorcentaje, EsPredominante, ClasificadorID, Nombre, Descripcion, Activo, TipoEscalaDinamica ");
            sCmd.Append(" FROM EscalaDinamica ");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (escalaDinamica.PuntajeID != null)
            {
                s_VarWHERE.Append(" PuntajeID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = escalaDinamica.PuntajeID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (prueba.PruebaID != null)
            {
                s_VarWHERE.Append(" AND PruebaID = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = prueba.PruebaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (escalaDinamica.PuntajeMinimo != null)
            {
                s_VarWHERE.Append(" AND PuntajeMinimo = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = escalaDinamica.PuntajeMinimo;
                sqlParam.DbType = DbType.Decimal;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (escalaDinamica.PuntajeMaximo != null)
            {
                s_VarWHERE.Append(" AND PuntajeMaximo = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = escalaDinamica.PuntajeMaximo;
                sqlParam.DbType = DbType.Decimal;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (escalaDinamica.EsPorcentaje != null)
            {
                s_VarWHERE.Append(" AND EsPorcentaje = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = escalaDinamica.EsPorcentaje;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (escalaDinamica.EsPredominante != null)
            {
                s_VarWHERE.Append(" AND EsPredominante = @dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = escalaDinamica.EsPredominante;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (escalaDinamica.Clasificador.ClasificadorID != null)
            {
                s_VarWHERE.Append(" AND ClasificadorID = @dbp4ram7 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram7";
                sqlParam.Value = escalaDinamica.Clasificador.ClasificadorID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (escalaDinamica.Nombre != null)
            {
                s_VarWHERE.Append(" AND Nombre = @dbp4ram8 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram8";
                sqlParam.Value = escalaDinamica.Nombre;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (escalaDinamica.Activo != null)
            {
                s_VarWHERE.Append(" AND Activo = @dbp4ram9 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram9";
                sqlParam.Value = escalaDinamica.Activo;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (escalaDinamica.TipoEscalaDinamica != null)
            {
                s_VarWHERE.Append(" AND TipoEscalaDinamica = @dbp4ram10 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram10";
                sqlParam.Value = escalaDinamica.TipoEscalaDinamica;
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
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "EscalaDinamica");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("EscalaDinamicaRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
