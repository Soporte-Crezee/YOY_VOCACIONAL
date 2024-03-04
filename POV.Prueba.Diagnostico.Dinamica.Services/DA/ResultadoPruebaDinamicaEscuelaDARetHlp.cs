using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Framework.Base.DataAccess;
using POV.CentroEducativo.BO;
using POV.Prueba.Diagnostico.Dinamica.BO;
using System.Data.Common;

namespace POV.Prueba.Diagnostico.Dinamica.DA
{
  internal  class ResultadoPruebaDinamicaEscuelaDARetHlp
    {
        /// <summary>
        /// Consulta registros con información de Resultado de la Prueba en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="escuela">Escuela que provee el criterio de selección para realizar la consulta</param>
        /// <param name="grupoCicloEscolar">grupoCicloEscolar que provee el criterio de selección para realizar la consulta</param>
        /// <param name="prueba">Prueba que provee el criterio de selección para realizar la consulta</param>      
        /// <returns>El DataSet que contiene la información de Resultado de la Prueba generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, Escuela escuela, GrupoCicloEscolar grupoCicloEscolar, PruebaDinamica prueba)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (escuela == null || escuela.EscuelaID == null)
                sError += ", EscuelaID";
            if (prueba == null || prueba.PruebaID == null)
                sError += ", PruebaID";
            if(grupoCicloEscolar==null)
                sError += ", GrupoCicloEscolar";
            if(grupoCicloEscolar.CicloEscolar ==null || grupoCicloEscolar.CicloEscolar.CicloEscolarID==null)
                sError += ", CicloEscolarID";
          

            if (sError.Length > 0)
                throw new Exception("ReportePruebaDinamicaDARetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));

            if (dctx == null)
                throw new Exception("ReportePruebaDinamicaDARetHlp.Action: DataContext no puede ser nulo");

            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new Exception("ReportePruebaDinamicaDARetHlp.Action: Hubo un error al conectarse a la Base de Datos\n " + ex.Message);
            }

            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();

            sCmd.Append(@" SELECT COUNT( CASE WHEN (ResultadoPruebaID IS NOT NULL AND RegistroPruebaID IS NOT NULL AND RegPbaD_EstadoPrueba=2) THEN AlumnoID END) as NumeroAlumnos,EscuelaID, GrupoCicloEscolarID, CicloEscolarID,CicloEscolarTitulo, GrupoNombre, 
                   PruebaClave,NombreModelo,NombreTipoResultadoMetodo,
                   PruebaNombre, NombreClasificadorDI,NombreClasificadorDS,NombreClasificadorDC,
                   DescripcionClasificadorDI,EscuelaClave, NombreEscuela, Turno,EscalaDS_Nombre,EscalaDC_Nombre,PuntajeID,DetResulC_PuntajeID,DetResulS_PuntajeID
                   FROM
                     (SELECT DISTINCT TOP (100) PERCENT 
	                  ResultadoPruebaID, AlumnoID, GrupoCicloEscolarID, CicloEscolarID,vw_CicloEscolarID,CicloEscolarTitulo,GrupoNombre,
                      RegistroPruebaID, PruebaID,PruebaClave,NombreModelo,NombreTipoResultadoMetodo,
                      PruebaNombre, ResultadoMetodoCalificacionID,  
                      DetResulC_DetalleResultadoID,DetResulC_PuntajeID,DetResulC_Valor,DetResulS_DetalleResultadoID,
                      DetResulS_PuntajeID,DetResulS_Valor,EscalaDI_PuntajeMinimo,EscalaDC_Nombre,EscalaDS_Nombre,
                      EscalaDI_PuntajeMaximo,NombreClasificadorDI,DescripcionClasificadorDI,
                      NombreClasificadorDC,NombreClasificadorDS,DescripcionClasificadorDS,PuntajeID,EscalaDC_PuntajeMinimo,EscalaDC_PuntajeMaximo,
                      DetResulS_EsAproximado, NombreClasificadorDI   EscalaDS_PuntajeMinimo, EscalaDS_PuntajeMaximo, EscalaDS_EsPorcentaje,
                      EscalaDS_EsPredominante,EscuelaID, EscuelaClave, NombreEscuela, Turno,RegPbaD_EstadoPrueba");
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
            if(grupoCicloEscolar.Grupo!=null)
            if (grupoCicloEscolar.Grupo.GrupoID != null)
            {
                s_VarWHERE.Append(" AND GrupoID = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = grupoCicloEscolar.Grupo.GrupoID;
                sqlParam.DbType = DbType.Guid;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (grupoCicloEscolar.CicloEscolar.CicloEscolarID != null)
            {
                s_VarWHERE.Append(" AND CicloEscolarID = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = grupoCicloEscolar.CicloEscolar.CicloEscolarID;
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
            sCmd.Append(@" ) AS Q1 GROUP BY
	                        EscuelaID,GrupoCicloEscolarID, CicloEscolarID,CicloEscolarTitulo, GrupoNombre, 
                            PruebaClave,NombreModelo,NombreTipoResultadoMetodo, 
                            PruebaNombre, NombreClasificadorDI,NombreClasificadorDC,NombreClasificadorDS,DescripcionClasificadorDI,
                            EscuelaClave, NombreEscuela, Turno,EscalaDS_Nombre,EscalaDC_Nombre,PuntajeID,DetResulC_PuntajeID,DetResulS_PuntajeID ORDER BY NumeroAlumnos");
  
        

            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "ReportePruebaDinamica");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("ReportePruebaDinamicaDARetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
