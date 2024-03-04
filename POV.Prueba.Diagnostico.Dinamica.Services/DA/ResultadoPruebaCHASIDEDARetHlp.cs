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
    class ResultadoPruebaCHASIDEDARetHlp
    {
        /// <summary>
        /// Consulta registros de AResultadoPrueba en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="aResultadoPrueba">AResultadoPrueba que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de AResultadoPrueba generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, Alumno alumno, APrueba prueba, int? grupo)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (prueba == null)
                sError += ", APrueba";
            if (alumno == null)
                sError += ", Alumno";
            if (sError.Length > 0)
                throw new Exception("ResultadoPruebaCHASIDEDARetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (prueba.PruebaID == null)
                sError += ", PruebaID";
            if (alumno.AlumnoID == null)
                sError += ", AlumnoID";
            if (grupo == null)
                sError += ", Grupo";
            if (sError.Length > 0)
                throw new Exception("ResultadoPruebaCHASIDEDARetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.PruebaDiagnostico.DA",
                   "ResultadoPruebaCHASIDEDARetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "ResultadoPruebaCHASIDEDARetHlp: No se pudo conectar a la base de datos", "POV.PruebaDiagnostico.DA",
                   "ResultadoPruebaCHASIDEDARetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append("SELECT TOP 2 c.ClasificadorID, case WHEN rd.grupo = 1 THEN 'Intereses' WHEN rd.grupo = 2 THEN 'Aptitudes' END Grupo, c.Nombre, sum(orpd.PorcentajeCalificacion) Calificacion ");
            sCmd.Append("  FROM [RespuestaOpcionDinamicaDetalle] rodd  ");
            sCmd.Append(" inner join [RespuestaPreguntaDinamica] rprd on rprd.RespuestaPreguntaID = rodd.RespuestaPreguntaID ");
            sCmd.Append(" inner join [RespuestaReactivoDinamica] rrd on rrd.RespuestaReactivoID = rprd.RespuestaReactivoID ");
            sCmd.Append(" inner join [RegistroPruebaDinamica] repd on repd.RegistroPruebaID = rrd.RegistroPruebaID ");
            sCmd.Append(" inner join [ResultadoPrueba] rp on rp.ResultadoPruebaID = repd.ResultadoPruebaID ");
            sCmd.Append(" inner join [OpcionRespuestaPlantillaDinamico] orpd on orpd.OpcionRespuestaPlantillaID = rodd.OpcionRespuestaPlantillaID ");
            sCmd.Append(" inner join [RespuestaPlantillaDinamico] rpd on rpd.RespuestaPlantillaID = orpd.RespuestaPlantillaID ");
            sCmd.Append(" inner join [PreguntaDinamico] pd on pd.PreguntaID = rpd.PreguntaID ");
            sCmd.Append(" inner join [Clasificador] c on c.ClasificadorID = orpd.ClasificadorID ");
            sCmd.Append(" inner join [ReactivoDinamico] rd on rd.ReactivoID = rrd.ReactivoID ");

            StringBuilder s_VarWHERE = new StringBuilder();
            if (prueba.PruebaID != null)
            {
                s_VarWHERE.Append(" rp.PruebaID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = prueba.PruebaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (alumno.AlumnoID != null)
            {
                s_VarWHERE.Append(" AND repd.AlumnoID = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = alumno.AlumnoID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (grupo != null)
            {
                s_VarWHERE.Append(" AND rd.Grupo = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = grupo;
                sqlParam.DbType = DbType.Int32;
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

            sCmd.Append(" group by orpd.Texto, c.ClasificadorID, rd.Grupo, c.Nombre ");
            sCmd.Append(" order by Calificacion desc");


            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "ResultadoPruebaChaside");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("ResultadoPruebaCHASIDEDARetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
