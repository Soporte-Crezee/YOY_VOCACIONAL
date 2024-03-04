using POV.CentroEducativo.BO;
using POV.Prueba.Diagnostico.Dinamica.BO;
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
    internal class RespuestaAlumnoRetHlp
    {
        public DataSet Action(IDataContext dctx, Alumno alumno, PruebaDinamica pruebaPivote, EEstadoPrueba estadoPrueba) 
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (alumno == null)
                sError += ", Alumno";
            if (sError.Length > 0)
                throw new Exception("RespuestaAlumnoRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.Services.DAO",
                   "RespuestaAlumnoRetHlp", "Action", null, null);

            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "RespuestaAlumnoRetHlp: No se pudo conectar a la base de datos", "POV.CentroEducativo.Services.DAO",
                   "RespuestaAlumnoRetHlp", "Action", null, null);
            }

            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();

            sCmd.Append("select e.AlumnoID, rp.PruebaID, p.Nombre, rpd.EstadoPrueba, p.TipoPruebaPresentacion, p.Espremium, rpd.ResultadoPruebaID, sum(ord.PorcentajeCalificacion) As Calificacion");
            sCmd.Append("  from dbo.ExpedienteEscolar e");
            sCmd.Append(" inner join dbo.DetalleCicloEscolar d on e.ExpedienteEscolarID = d.ExpedienteEscolarID");
            sCmd.Append(" inner join dbo.ResultadoPrueba rp on rp.DetalleCicloEscolarID = d.DetalleCicloEscolarID");
            sCmd.Append(" inner join dbo.Prueba p on p.PruebaID = rp.PruebaID");
            sCmd.Append(" inner join dbo.PruebaContrato pc on pc.PruebaID = rp.PruebaID");
            sCmd.Append(" inner join dbo.RegistroPruebaDinamica rpd on rpd.ResultadoPruebaID = rp.ResultadoPruebaID");
            sCmd.Append(" inner join dbo.RespuestaReactivoDinamica rrd on rrd.RegistroPruebaID = rpd.RegistroPruebaID");
            sCmd.Append(" inner join dbo.RespuestaPreguntaDinamica resp on resp.RespuestaReactivoID = rrd.RespuestaReactivoID");
            sCmd.Append(" left outer join dbo.RespuestaOpcionDinamicaDetalle rod on rod.RespuestaPreguntaID = resp.RespuestaPreguntaID");
            sCmd.Append(" left outer join dbo.OpcionRespuestaPlantillaDinamico ord on ord.OpcionRespuestaPlantillaID = rod.OpcionRespuestaPlantillaID and ord.EsInteres = 0");
            sCmd.Append(" left outer join dbo.Clasificador clas on clas.ClasificadorID = ord.ClasificadorID");

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
            
            if (estadoPrueba != null)
            {
                s_VarWHERE.Append(" AND rpd.EstadoPrueba = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = estadoPrueba;
                sqlParam.DbType = DbType.Byte;
                sqlCmd.Parameters.Add(sqlParam);
            }

            string s_VarWHEREres = s_VarWHERE.ToString().Trim();
            string s_VarGroup = " GROUP BY e.AlumnoID, rp.PruebaID, p.Nombre,rpd.EstadoPrueba, p.TipoPruebaPresentacion, p.Espremium, rpd.ResultadoPruebaID";
            if (s_VarWHEREres.Length > 0)
            {
                if (s_VarWHEREres.StartsWith("AND "))
                    s_VarWHEREres = s_VarWHEREres.Substring(4);
                else if (s_VarWHEREres.StartsWith("OR "))
                    s_VarWHEREres = s_VarWHEREres.Substring(3);
                else if (s_VarWHEREres.StartsWith(","))
                    s_VarWHEREres = s_VarWHEREres.Substring(1);
                sCmd.Append(" WHERE " + s_VarWHEREres + s_VarGroup);
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
                throw new Exception("RespuestaAlumnoRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            return ds;
        }

        public DataSet Action(IDataContext dctx, Alumno alumno, string pruebaPresentacion, EEstadoPrueba estadoPrueba)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (alumno == null)
                sError += ", Alumno";
            if (sError.Length > 0)
                throw new Exception("RespuestaAlumnoRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.Services.DAO",
                   "RespuestaAlumnoRetHlp", "Action", null, null);

            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "RespuestaAlumnoRetHlp: No se pudo conectar a la base de datos", "POV.CentroEducativo.Services.DAO",
                   "RespuestaAlumnoRetHlp", "Action", null, null);
            }

            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();

            sCmd.Append("select e.AlumnoID, rp.PruebaID, p.Nombre, rpd.EstadoPrueba, p.TipoPruebaPresentacion, p.Espremium, rpd.ResultadoPruebaID, sum(ord.PorcentajeCalificacion) As Calificacion");
            sCmd.Append("  from dbo.ExpedienteEscolar e");
            sCmd.Append(" inner join dbo.DetalleCicloEscolar d on e.ExpedienteEscolarID = d.ExpedienteEscolarID");
            sCmd.Append(" inner join dbo.ResultadoPrueba rp on rp.DetalleCicloEscolarID = d.DetalleCicloEscolarID");
            sCmd.Append(" inner join dbo.Prueba p on p.PruebaID = rp.PruebaID");
            sCmd.Append(" inner join dbo.PruebaContrato pc on pc.PruebaID = rp.PruebaID");
            sCmd.Append(" inner join dbo.RegistroPruebaDinamica rpd on rpd.ResultadoPruebaID = rp.ResultadoPruebaID");
            sCmd.Append(" inner join dbo.RespuestaReactivoDinamica rrd on rrd.RegistroPruebaID = rpd.RegistroPruebaID");
            sCmd.Append(" inner join dbo.RespuestaPreguntaDinamica resp on resp.RespuestaReactivoID = rrd.RespuestaReactivoID");
            sCmd.Append(" left outer join dbo.RespuestaOpcionDinamicaDetalle rod on rod.RespuestaPreguntaID = resp.RespuestaPreguntaID");
            sCmd.Append(" left outer join dbo.OpcionRespuestaPlantillaDinamico ord on ord.OpcionRespuestaPlantillaID = rod.OpcionRespuestaPlantillaID and ord.EsInteres = 0");
            sCmd.Append(" left outer join dbo.Clasificador clas on clas.ClasificadorID = ord.ClasificadorID");

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

            if (pruebaPresentacion != "")
            {
                s_VarWHERE.Append(" AND p.tipopruebapresentacion in(" + pruebaPresentacion + ")");
                //sqlParam = sqlCmd.CreateParameter();
                //sqlParam.ParameterName = "dbp4ram2";
                //sqlParam.Value = pruebaPivote.PruebaID;
                //sqlParam.DbType = DbType.Int32;
                //sqlCmd.Parameters.Add(sqlParam);
            }

            if (estadoPrueba != null)
            {
                s_VarWHERE.Append(" AND rpd.EstadoPrueba = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = estadoPrueba;
                sqlParam.DbType = DbType.Byte;
                sqlCmd.Parameters.Add(sqlParam);
            }

            string s_VarWHEREres = s_VarWHERE.ToString().Trim();
            string s_VarGroup = " GROUP BY e.AlumnoID, rp.PruebaID, p.Nombre,rpd.EstadoPrueba, p.TipoPruebaPresentacion, p.Espremium, rpd.ResultadoPruebaID";
            if (s_VarWHEREres.Length > 0)
            {
                if (s_VarWHEREres.StartsWith("AND "))
                    s_VarWHEREres = s_VarWHEREres.Substring(4);
                else if (s_VarWHEREres.StartsWith("OR "))
                    s_VarWHEREres = s_VarWHEREres.Substring(3);
                else if (s_VarWHEREres.StartsWith(","))
                    s_VarWHEREres = s_VarWHEREres.Substring(1);
                sCmd.Append(" WHERE " + s_VarWHEREres + s_VarGroup);
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
                throw new Exception("RespuestaAlumnoRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
