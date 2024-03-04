using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.Prueba.BO;
using POV.Seguridad.BO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace POV.Prueba.Diagnostico.Dinamica.DA
{
    internal class PruebaAlumnosSacksDARetHlp
    {
        /// <summary>
        /// Consulta registros de estudiantes que tienen reporte Sacks en la base de datos.
        /// </summary>
        /// <param name="dctx"> El DataContext que proveerá acceso a la base de datos </param>
        /// <param name="usuario"> Usuario que provee el criterio de selección para realizar la consulta </param>
        /// <param name="prueba"> APrueba que provee el criterio de selección para realizar la consulta </param>
        /// <param name="nombreCompletoAlumno"> nombreCompleto que provee el criterio de selección para realizar la consulta </param>
        /// <returns> El DataSet que contiene la información de AResultadoPrueba generada por la consulta </returns>
        public DataSet Action(IDataContext dctx, Usuario usuario, APrueba prueba, String nombreCompletoAlumno = "") 
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (prueba == null)
                sError += ", APrueba";
            if (usuario == null)
                sError += ", Alumno";
            if (sError.Length > 0)
                throw new Exception("PruebaAlumnosSacksDARetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (prueba.PruebaID == null)
                sError += ", PruebaID";
            if (usuario.UsuarioID == null)
                sError += ", UsuarioID";
            if (sError.Length > 0)
                throw new Exception("PruebaAlumnosSacksDARetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.PruebaDiagnostico.DA",
                   "PruebaAlumnosSacksDARetHlp", "Action", null, null);

            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "PruebaAlumnosSacksDARetHlp: No se pudo conectar a la base de datos", "POV.PruebaDiagnostico.DA",
                    "PruebasAlumnosDARetHlp", "Action", null, null);
            }

            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append("SELECT a.AlumnoID, a.Nombre+' '+PrimerApellido+' '+SegundoApellido NombreAlumno, p.Nombre NombrePrueba,  ");
            sCmd.Append("       rpd1.FechaInicio, rpd1.FechaFin ");
            sCmd.Append("  FROM [UsuarioExpediente] ue ");
            sCmd.Append("  inner join [Alumno] a on a.AlumnoID = ue.AlumnoID ");
            sCmd.Append("  inner join [ResultadoPruebaDinamica] rpd on rpd.AlumnoID = ue.AlumnoID ");
            sCmd.Append("  inner join [ResultadoPrueba] rp on rp.ResultadoPruebaID = rpd.ResultadoPruebaID ");
            sCmd.Append("  inner join [Prueba] p on p.PruebaID = rp.PruebaID ");
            sCmd.Append("  inner join [RegistroPruebaDinamica] rpd1 on rpd1.ResultadoPruebaID = rpd.ResultadoPruebaID ");
            sCmd.Append("  inner join [SumarioGeneralSACKS] sgs on sgs.AlumnoID = a.AlumnoID ");

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
            if (usuario.UsuarioID != null)
            {
                s_VarWHERE.Append(" AND ue.UsuarioID = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = usuario.UsuarioID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }

            s_VarWHERE.Append(" AND rpd1.EstadoPrueba = @dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            sqlParam.Value = 2;
            sqlParam.DbType = DbType.Int16;
            sqlCmd.Parameters.Add(sqlParam);

            if (nombreCompletoAlumno != "")
            {
                s_VarWHERE.Append(" AND rtrim(ltrim(a.Nombre+' '+a.PrimerApellido+' '+a.SegundoApellido)) like @dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = nombreCompletoAlumno;
                sqlParam.DbType = DbType.String;
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
                throw new Exception("PruebasAlumnosDARetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            return ds;
        }

        /// <summary>
        /// Consulta registros de estudiantes que tienen reporte Sacks en la base de datos.
        /// </summary>
        /// <param name="dctx"> El DataContext que proveerá acceso a la base de datos </param>
        /// <param name="tutor"> Tutor que provee el criterio de selección para realizar la consulta </param>
        /// <param name="prueba"> APrueba que provee el criterio de selección para realizar la consulta </param>
        /// <param name="nombreCompletoAlumno"> nombreCompletoAlumno que provee el criterio de selección para realizar la consulta </param>
        /// <returns> El DataSet que contiene la información de AResultadoPrueba generada por la consulta </returns>
        public DataSet Action(IDataContext dctx, Tutor tutor, APrueba prueba, String nombreCompletoAlumno = "")
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (prueba == null)
                sError += ", APrueba";
            if (tutor == null)
                sError += ", Alumno";
            if (sError.Length > 0)
                throw new Exception("PruebasAlumnosDARetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (prueba.PruebaID == null)
                sError += ", PruebaID";
            if (tutor.TutorID == null)
                sError += ", TutorID";
            if (sError.Length > 0)
                throw new Exception("PruebasAlumnosDARetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.PruebaDiagnostico.DA",
                   "PruebasAlumnosDARetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "PruebasAlumnosDARetHlp: No se pudo conectar a la base de datos", "POV.PruebaDiagnostico.DA",
                   "PruebasAlumnosDARetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append("SELECT a.AlumnoID, a.Nombre+' '+PrimerApellido+' '+SegundoApellido NombreAlumno, p.Nombre NombrePrueba,  ");
            sCmd.Append("       rpd1.FechaInicio, rpd1.FechaFin ");
            sCmd.Append("  FROM [TutorAlumno] ue ");
            sCmd.Append("  inner join [Alumno] a on a.AlumnoID = ue.AlumnoID ");
            sCmd.Append("  inner join [ResultadoPruebaDinamica] rpd on rpd.AlumnoID = ue.AlumnoID ");
            sCmd.Append("  inner join [ResultadoPrueba] rp on rp.ResultadoPruebaID = rpd.ResultadoPruebaID ");
            sCmd.Append("  inner join [Prueba] p on p.PruebaID = rp.PruebaID ");
            sCmd.Append("  inner join [RegistroPruebaDinamica] rpd1 on rpd1.ResultadoPruebaID = rpd.ResultadoPruebaID ");
            sCmd.Append("  inner join [SumarioGeneralSACKS] sgs on sgs.AlumnoID = a.AlumnoID ");

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
            if (tutor.TutorID != null)
            {
                s_VarWHERE.Append(" AND ue.TutorID = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = tutor.TutorID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }

            s_VarWHERE.Append(" AND rpd1.EstadoPrueba = @dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            sqlParam.Value = 2;
            sqlParam.DbType = DbType.Int16;
            sqlCmd.Parameters.Add(sqlParam);

            if (nombreCompletoAlumno != "")
            {
                s_VarWHERE.Append(" AND rtrim(ltrim(a.Nombre+' '+a.PrimerApellido+' '+a.SegundoApellido)) like @dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = nombreCompletoAlumno;
                sqlParam.DbType = DbType.String;
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
                throw new Exception("PruebasAlumnosDARetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
