using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Framework.Base.DataAccess;
using POV.CentroEducativo.BO;
using System.Data.Common;
using POV.Prueba.Diagnostico.Dinamica.BO;

namespace POV.Prueba.Diagnostico.Dinamica.DA
{
    /// <summary>
    /// DA del Reporte de prueba dinamica por grupo.
    /// </summary>
    internal class ResultadoPruebaDinamicaGrupoDARetHlp
    {
        /// <summary>
        /// Obtiene la información de los alumnos, la prueba y la escuela.
        /// </summary>
        /// <param name="dctx">Proveedor de datos.</param>
        /// <param name="escuela">Escuela proporcionada</param>
        /// <param name="grupo">Grupo proporcionado</param>
        /// <param name="cicloEscolar">Ciclo escolar proporcionado.</param>
        /// <returns>Retorna un dataset con los resultados.</returns>
        public DataSet Action(IDataContext dctx, Escuela escuela, GrupoCicloEscolar grupoCicloEscolar, PruebaDinamica prueba)
        {
            #region Variables
            object myFirm = new object();
            string sError = String.Empty;
            DbCommand sqlCmd = null;
            DbParameter sqlParam;
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            StringBuilder sCmd = new StringBuilder();
            StringBuilder s_VarWHERE = new StringBuilder();
            DataSet ds = new DataSet();
            #endregion

            #region Validaciones
            if (escuela == null)
                sError += ", Escuela";
            if (prueba == null)
                sError += ", Prueba";
            if (grupoCicloEscolar == null)
                sError += ", GrupoCicloEscolar";
            if (sError.Length > 0)
                throw new Exception("ReportePruebaDinamicaGrupoDARetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new Exception("ReportePruebaDinamicaGrupoDARetHlp.Action: DataContext no puede ser nulo"); 
            #endregion

            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("ReportePruebaDinamicaGrupoDARetHlp: No se pudo conectar con la base de datos {0}", ex.Message));
            }

            sCmd.Append(" SELECT DISTINCT ");
            sCmd.Append(" PruebaClave,PruebaNombre,NombreModelo,MetodoCalificacion = NombreTipoResultadoMetodo, ");
            sCmd.Append(" NombreEscuela,Turno,CicloEscolarTitulo,GrupoNombre, ");
            sCmd.Append(" ResultadoPruebaID,NombreAlumno,AlumnoSexo,AlumnoEdad,PruebaID ");
            sCmd.Append(" FROM ViewResultadoPruebaModelo ");
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
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "ResultadoPruebaDinamicaGrupo");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("ResultadoPruebaDinamicaGrupoDARetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
