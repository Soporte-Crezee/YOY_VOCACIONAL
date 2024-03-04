using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Expediente.BO;
using POV.Expediente.DAO;
using POV.CentroEducativo.BO;
using POV.Prueba.Diagnostico.Dinamica.BO;

namespace POV.Expediente.DAO
{
    internal class InteresesAspiranteRetHlp
    {
        /// <summary>
        /// Consulta registros de AAsignacionRecurso en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="aAsignacionRecurso">AAsignacionRecurso que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de AAsignacionRecurso generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, Alumno alumno, PruebaDinamica pruebaPivote)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (alumno == null)
                sError += ", Alumno";
            if (sError.Length > 0)
                throw new Exception("InteresesAspiranteRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Expediente.DAO",
                   "InteresesAspiranteRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "InteresesAspiranteRetHlp: No se pudo conectar a la base de datos", "POV.Expediente.DAO",
                   "InteresesAspiranteRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append("select rod.RespuestaPreguntaID InteresID, ord.Texto NombreInteres, ord.Texto Descripcion, ord.ClasificadorID, clas.Nombre, clas.Descripcion");
            sCmd.Append("  from ExpedienteEscolar e");
            sCmd.Append(" inner join DetalleCicloEscolar d on e.ExpedienteEscolarID = d.ExpedienteEscolarID");
            sCmd.Append(" inner join ResultadoPrueba rp on rp.DetalleCicloEscolarID = d.DetalleCicloEscolarID");
            sCmd.Append(" inner join RegistroPruebaDinamica rpd on rpd.ResultadoPruebaID = rp.ResultadoPruebaID");
            sCmd.Append(" inner join RespuestaReactivoDinamica rrd on rrd.RegistroPruebaID = rpd.RegistroPruebaID");
            sCmd.Append(" inner join RespuestaPreguntaDinamica resp on resp.RespuestaReactivoID = rrd.RespuestaReactivoID");
            sCmd.Append(" inner join RespuestaOpcionDinamicaDetalle rod on rod.RespuestaPreguntaID = resp.RespuestaPreguntaID");
            sCmd.Append(" inner join OpcionRespuestaPlantillaDinamico ord on ord.OpcionRespuestaPlantillaID = rod.OpcionRespuestaPlantillaID AND ord.EsInteres=1");
            sCmd.Append(" inner join Clasificador clas on clas.ClasificadorID = ord.ClasificadorID");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (alumno.AlumnoID != null)
            {
                s_VarWHERE.Append(" e.alumnoid = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = alumno.AlumnoID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (pruebaPivote.PruebaID != null)
            {
                s_VarWHERE.Append(" AND rp.pruebaid = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = pruebaPivote.PruebaID;
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
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "AsignacionRecurso");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("InteresesAspiranteRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
