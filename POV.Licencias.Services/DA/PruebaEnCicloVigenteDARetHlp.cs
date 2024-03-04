using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using System.Data.Common;

namespace POV.Licencias.DA
{
    internal class PruebaEnCicloVigenteDARetHlp
    {
        public DataSet Action(IDataContext dctx, Int32? pruebaID)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (pruebaID == null)
                throw new Exception("PruebaEnCicloVigenteDARetHlp: El identificador de prueba PRUEBAID no puede ser nulo.");
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Licencias.DA",
                   "PruebaEnCicloVigenteDARetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "PruebaEnCicloVigenteDARetHlp: No se pudo conectar a la base de datos", "POV.Licencias.DA",
                   "PruebaEnCicloVigenteDARetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(@" SELECT
	                            Prueba.PruebaID, Prueba.EstadoLiberacion, Prueba.ModeloID, Prueba.Tipo,
	                            PbaContrato.PruebaContratoID, PbaContrato.Activo,
	                            RecContrato.RecursoContratoID, RecContrato.Activo,
	                            CicContrato.CicloContratoID, CicContrato.Activo, CicContrato.EstaLiberado,
	                            Contrato.ContratoID, Contrato.Estatus, Contrato.InicioContrato, Contrato.FinContrato
                            FROM
	                            Prueba INNER JOIN
	                            PruebaContrato PbaContrato ON PbaContrato.PruebaID = Prueba.PruebaID INNER JOIN
	                            RecursoContrato RecContrato ON RecContrato.RecursoContratoID = PbaContrato.RecursoContratoID INNER JOIN
	                            CicloContrato CicContrato ON CicContrato.CicloContratoID = RecContrato.CicloContratoID INNER JOIN
	                            Contrato ON Contrato.ContratoID = CicContrato.ContratoID
                            ");
            StringBuilder s_VarWHERE = new StringBuilder();

            // grupoCicloEscolar.GrupoCicloEscolarID
            s_VarWHERE.Append(" Prueba.PruebaID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = pruebaID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);

            s_VarWHERE.Append(" AND (PbaContrato.Activo = 1 or CicContrato.Activo = 1 or Contrato.Estatus = 1) ");

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
            sCmd.Append(" ORDER by Prueba.PruebaID ");
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "CiclosVigentesPrueba");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("PruebasGrupoDARetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
