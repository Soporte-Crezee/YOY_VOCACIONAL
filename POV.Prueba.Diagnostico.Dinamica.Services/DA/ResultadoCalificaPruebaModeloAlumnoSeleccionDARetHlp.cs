using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.Prueba.BO;
using POV.Prueba.Diagnostico.Dinamica.BO;

namespace POV.Prueba.Diagnostico.Dinamica.DA
{
    class ResultadoCalificaPruebaModeloAlumnoSeleccionDARetHlp
    {
        /// <summary>
        /// Consulta registros con información de Resultado de la Prueba en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="escuela">Escuela que provee el criterio de selección para realizar la consulta</param>
        /// <param name="grupoCicloEscolar">grupoCicloEscolar que provee el criterio de selección para realizar la consulta</param>
        /// <param name="prueba">Prueba que provee el criterio de selección para realizar la consulta</param>
        /// <param name="alumno">Alumno que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de Resultado de la Prueba generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, Escuela escuela, GrupoCicloEscolar grupoCicloEscolar, PruebaDinamica prueba, Alumno alumno)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (escuela == null || escuela.EscuelaID == null)
                sError += ", EscuelaID";
            if (prueba == null || prueba.PruebaID == null)
                sError += ", PruebaID";
            if (sError.Length > 0)
                throw new Exception("ResultadoCalificaPruebaModeloAlumnoSeleccionDARetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));

            if (dctx == null)
                throw new Exception("ResultadoCalificaPruebaModeloAlumnoSeleccionDARetHlp.Action: DataContext no puede ser nulo");

            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new Exception("ResultadoCalificaPruebaModeloAlumnoSeleccionDARetHlp.Action: Hubo un error al conectarse a la Base de Datos\n " + ex.Message);
            }

            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();

            sCmd.Append(@" SELECT DISTINCT ResultadoPruebaID, AlumnoID, GrupoCicloEscolarID, CicloEscolarID, 
                            RegistroPruebaID, PruebaID, ResultadoMetodoCalificacionID, 
                            DetResulS_Valor, DetResulS_EsAproximado, 
                            EscalaDS_PuntajeMinimo, EscalaDS_PuntajeMaximo, EscalaDS_EsPorcentaje, EscalaDS_EsPredominante, 
                            EscuelaID, EscuelaClave, NombreEscuela, Turno");

            sCmd.Append(" FROM ViewResultadoPruebaModelo");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (prueba.PruebaID != null)
            {
                s_VarWHERE.Append(" AND PruebaID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = prueba.PruebaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (escuela.EscuelaID != null)
            {
                s_VarWHERE.Append(" AND EscuelaID = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = escuela.EscuelaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (grupoCicloEscolar.GrupoCicloEscolarID != null)
            {
                s_VarWHERE.Append(" AND GrupoCicloEscolarID = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = grupoCicloEscolar.GrupoCicloEscolarID;
                sqlParam.DbType = DbType.Guid;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (alumno.AlumnoID != null)
            {
                s_VarWHERE.Append(" AND AlumnoID = @dbp4ram4 ");
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
                sqlAdapter.Fill(ds, "ResultadoCalificaSeleccion");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("ResultadoCalificaPruebaModeloAlumnoSeleccionDARetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
