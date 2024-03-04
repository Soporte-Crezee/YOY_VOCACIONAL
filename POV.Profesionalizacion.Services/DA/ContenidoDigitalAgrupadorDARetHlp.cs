using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.ContenidosDigital.BO;

namespace POV.Profesionalizacion.DA
{
    /// <summary>
    /// Clase dedicada a la obtención de los contenidos digital agrupador específicos.
    /// </summary>
    internal class ContenidoDigitalAgrupadorDARetHlp
    {
        /// <summary>
        /// Método dedicado a la obtención del contenido digital agrupador relacionado con el contenido digital proporcionado.
        /// </summary>
        /// <param name="dctx">DataContext que proveerá el acceso a la base de datos.</param>
        /// <param name="contenido">Contenido digital proporcionado.</param>
        /// <returns>Regresa un dataset con los contenido digital agrupador encontrados.</returns>
        public DataSet Action(IDataContext dctx, ContenidoDigital contenido)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (contenido == null)
                sError += ", contenidoDigital";
            if (sError.Length > 0)
                throw new Exception("ContenidoDigitalAgrupadorDARetHlp: Los siguientes objetos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DA",
                   "ContenidoDigitalAgrupadorDARetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "",
                                            "ContenidoDigitalAgrupadorDARetHlp: No se pudo conectar a la base de datos",
                                            "POV.Profesionalizacion.DA",
                                            "ContenidoDigitalAgrupadorDARetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT ContenidoDigitalAgrupadorID, EjeTematicoID, SituacionAprendizajeID, AgrupadorContenidoDigitalID, ContenidoDigitalID, Activo, FechaRegistro ");
            sCmd.Append(" FROM ContenidoDigitalAgrupador ");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (contenido.ContenidoDigitalID != null)
            {
                s_VarWHERE.Append(" AND ContenidoDigitalID = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = contenido.ContenidoDigitalID;
                sqlParam.DbType = DbType.Int64;
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
                sqlAdapter.Fill(ds, "ContenidoDigitalAgrupador");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("ContenidoDigitalAgrupadorDARetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
