using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Base.DataAccess;
using POV.Licencias.BO;
using Framework.Base.Exceptions;
using System.Data.Common;
using System.Data;

namespace POV.Licencias.DA
{
    /// <summary>
    /// Consulta registros de pruebas de un contrato y un ciclo escolar
    /// </summary>
    internal class PruebasContratoDARetHlp
    {

        /// <summary>
        ///  Consulta registros de pruebas de un contrato y un ciclo escolar
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="cicloContrato">Ciclo contrato que servirá como filtro</param>
        /// <param name="contrato">contrato que servirá como filtro</param>
        public DataSet Action(IDataContext dctx, Contrato contrato, CicloContrato cicloContrato)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (contrato == null)
                sError += ", Contrato";
            if (cicloContrato == null)
                sError += ",Ciclo Contrato";
            if (sError.Length > 0)
                throw new Exception("PruebasContratoDARetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (contrato.ContratoID == null)
                sError += ", Contrato";
            if (cicloContrato.CicloEscolar == null)
                sError += ", Ciclo escolar del ciclocontrato";
            if (sError.Length > 0)
                throw new Exception("PruebasContratoDARetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (cicloContrato.CicloEscolar.CicloEscolarID == null)
                sError += ", Identificador del ciclo escolar";
            if (sError.Length > 0)
                throw new Exception("PruebasContratoDARetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Licencias.DA",
                   "PruebasContratoDARetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "PruebasContratoDARetHlp: No se pudo conectar a la base de datos", "POV.Licencias.DA",
                   "PruebasContratoDARetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" select pruebacontrato.TipoPruebaContrato, pruebacontrato.Activo, pruebacontrato.FechaRegistro,pruebacontrato.PruebaContratoID,pruebacontrato.PruebaID,pruebacontrato.RecursoContratoID,prueba.Tipo,prueba.PruebaID, prueba.TipoPruebaPresentacion, prueba.Espremium from Contrato c");
            sCmd.Append("  inner join CicloContrato ciclocontrato on c.ContratoID = ciclocontrato.ContratoID ");
            if (contrato.ContratoID != null)
            {
                sCmd.Append(" AND c.ContratoID = @dbp4ram2000 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2000";
                sqlParam.Value = contrato.ContratoID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (cicloContrato.CicloEscolar.CicloEscolarID != null)
            {
                sCmd.Append(" AND ciclocontrato.CicloEscolarID = @dbp4ram2111 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2111";
                sqlParam.Value = cicloContrato.CicloEscolar.CicloEscolarID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            sCmd.Append(" inner join RecursoContrato recursocontrato on recursocontrato.CicloContratoID = ciclocontrato.CicloContratoID inner join PruebaContrato pruebacontrato on pruebacontrato.RecursoContratoID = recursocontrato.RecursoContratoID");
            
            Boolean pruebaContratoActivo = true;
            sCmd.Append(" AND pruebacontrato.Activo = @dbp4ram222 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram222";
            sqlParam.Value = pruebaContratoActivo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);

            sCmd.Append(" inner join Prueba prueba on prueba.PruebaID = pruebacontrato.PruebaID");

            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "PruebaContrato");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("PruebasContratoRetHlp: Hubo un error al consultar los registros. " + exmsg);
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
