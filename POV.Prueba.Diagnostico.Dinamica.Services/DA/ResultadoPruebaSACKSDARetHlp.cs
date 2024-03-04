using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.Prueba.BO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace POV.Prueba.Diagnostico.Dinamica.DA
{
    internal class ResultadoPruebaSACKSDARetHlp
    {
        /// <summary>
        /// Consulta registros de AResultadoPrueba en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="aResultadoPrueba">AResultadoPrueba que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de AResultadoPrueba generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, Alumno alumno, APrueba prueba)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (prueba == null)
                sError += ", APrueba";
            if (alumno == null)
                sError += ", Alumno";
            if (sError.Length > 0)
                throw new Exception("ResultadoPruebaSACKSDARetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (prueba.PruebaID == null)
                sError += ", PruebaID";
            if (alumno.AlumnoID == null)
                sError += ", AlumnoID";
            if (sError.Length > 0)
                throw new Exception("ResultadoPruebaSACKSDARetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.PruebaDiagnostico.DA",
                   "ResultadoPruebaSACKSDARetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "ResultadoPruebaSACKSDARetHlp: No se pudo conectar a la base de datos", "POV.PruebaDiagnostico.DA",
                   "ResultadoPruebaSACKSDARetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append("SELECT  rpd1.AlumnoID, c.Descripcion, pd.TextoPregunta, rpd.TextoRespuesta ");
            sCmd.Append("  FROM [RespuestaPreguntaDinamica] rpd ");
            sCmd.Append(" inner join [RespuestaPlantillaAbiertaDinamico] rpad on rpad.RespuestaPlantillaID = rpd.RespuestaPlantillaID ");
            sCmd.Append(" inner join [Clasificador] c on c.ClasificadorID = rpad.ClasificadorID ");
            sCmd.Append(" inner join [PreguntaDinamico] pd on rpd.PreguntaID = pd.PreguntaID ");
            sCmd.Append(" inner join [ReactivoDinamico] rd on rd.ReactivoID = pd.ReactivoID ");
            sCmd.Append(" inner join [RespuestaReactivoDinamica] rrd on rrd.RespuestaReactivoID = rpd .RespuestaReactivoID  ");
            sCmd.Append(" inner join [ReactivosBancoReactivosDinamico] rbrd on rbrd.ReactivoID = rd.ReactivoID ");
            sCmd.Append(" inner join [BancoReactivosDinamico] brd on brd.BancoReactivoID = rbrd.BancoReactivoID ");
            sCmd.Append(" inner join [RegistroPruebaDinamica] rpd1 on rpd1.RegistroPruebaID = rrd.RegistroPruebaID ");

            StringBuilder s_VarWHERE = new StringBuilder();
            if (prueba.PruebaID != null)
            {
                s_VarWHERE.Append(" brd.PruebaID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = prueba.PruebaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (alumno.AlumnoID != null)
            {
                s_VarWHERE.Append(" AND rpd1.AlumnoID = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = alumno.AlumnoID;
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
                sqlAdapter.Fill(ds, "ResultadoPruebaSACKS");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("ResultadoPruebaSACKSDARetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
