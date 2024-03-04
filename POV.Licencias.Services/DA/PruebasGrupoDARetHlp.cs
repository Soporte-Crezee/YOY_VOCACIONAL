using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Licencias.BO;
using POV.CentroEducativo.BO;

namespace POV.Licencias.DA
{
    /// <summary>
    /// Consulta los registros de Pruebas asignadas a un grupo en la BD
    /// </summary>
    internal class PruebasGrupoDARetHlp
    {
        /// <summary>
        /// Consulta registros de Pruebas asignadas a un grupo en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="grupoCicloEscolar">GrupoCicloEscolar que provee el criterio de selección para realizar la consulta</param>
        /// <param name="licenciaEscuela">LicenciaEscuela que provee el criterio de selección para realizar la consulta</param>
        /// <param name="estadoLiberacion">EEstadoLiberacionPrueba como criterio de selección; se ignora si es NULL</param>
        /// <returns>El DataSet que contiene la información de GrupoCicloEscolar generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, GrupoCicloEscolar grupoCicloEscolar, LicenciaEscuela licenciaEscuela, EEstadoLiberacionPrueba? estadoLiberacion)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (grupoCicloEscolar == null)
                sError += ", GrupoCicloEscolar";
            if (licenciaEscuela == null)
                sError += ", LicenciaEscuela";
            if (sError.Length > 0)
                throw new Exception("PruebasGrupoDARetHlp: Los siguientes objetos no pueden ser vacios " + sError.Substring(2));
            if (grupoCicloEscolar.CicloEscolar == null)
                sError += ", CicloEscolar";
            if (grupoCicloEscolar.Escuela == null)
                sError += ", Escuela";
            if (licenciaEscuela.Contrato == null)
                sError += ", Contrato";
            if (sError.Length > 0)
                throw new Exception("PruebasGrupoDARetHlp: Los siguientes objetos no pueden ser vacios " + sError.Substring(2));
            if (grupoCicloEscolar.GrupoCicloEscolarID == null)
                sError += ", GrupoCicloEscolarID";
            if (grupoCicloEscolar.CicloEscolar.CicloEscolarID == null)
                sError += ", CicloEscolarID";
            if (grupoCicloEscolar.Escuela.EscuelaID == null)
                sError += ", EscuelaID";
            if (licenciaEscuela.Contrato.ContratoID == null)
                sError += ", ContratoID";
            if (sError.Length > 0)
                throw new Exception("PruebasGrupoDARetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Licencias.DA",
                   "PruebasGrupoDARetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "PruebasGrupoDARetHlp: No se pudo conectar a la base de datos", "POV.Licencias.DA",
                   "PruebasGrupoDARetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT ");
            sCmd.Append(" Contrato.ContratoID, Contrato.Estatus as Contrato_Estautus, ");
            sCmd.Append(" CicContr.CicloContratoID, CicContr.Activo as CicloContrato_Activo, ");
            sCmd.Append(" CicContr.CicloEscolarID, CicEsclr.Activo as CicloEscolar_Activo, ");
            sCmd.Append(" LicEscuela.LicenciaEscuelaID, LicEscuela.EscuelaID, LicEscuela.Activo as LicenciaEscuela_Activo, ");
            sCmd.Append(" RecContr.RecursoContratoID, RecContr.Activo as RecursoContrato_Activo, ");
            sCmd.Append(" PbaContrato.PruebaContratoID, PbaContrato.Activo as PruebaContrato_Activo, PbaContrato.PruebaID, ");
            sCmd.Append(" PbaContrato.TipoPruebaContrato, PbaContrato.FechaRegistro as PruebaContrato_FechaRegistro, ");
            sCmd.Append(" Prueba.Nombre as Prueba_Nombre, Prueba.Tipo as Prueba_Tipo, Prueba.Clave as Prueba_Clave, Prueba.EstadoLiberacion, ");
            sCmd.Append(" Prueba.ModeloID, Modelo.Nombre as Modelo_Nombre, ");
            sCmd.Append(" GpoCicEsclr.GrupoCicloEscolarID, GpoCicEsclr.Clave as GrupoClave, GpoCicEsclr.Activo as GrupoCicloEscolar_Activo, ");
            sCmd.Append(" CalendPbaGpo.CalendarizacionPruebaGrupoID, CalendPbaGpo.Activo as Calendarizacion_Activo, CalendPbaGpo.ConVigencia ");
            sCmd.Append(" FROM 	Contrato INNER JOIN ");
            sCmd.Append(" CicloContrato CicContr ON CicContr.ContratoID = Contrato.ContratoID INNER JOIN ");
            sCmd.Append(" CicloEscolar CicEsclr ON CicEsclr.CicloEscolarID = CicContr.CicloEscolarID INNER JOIN ");
            sCmd.Append(" LicenciaEscuela LicEscuela ON LicEscuela.ContratoID = Contrato.ContratoID AND LicEscuela.CicloEscolarID = CicEsclr.CicloEscolarID INNER JOIN ");
            sCmd.Append(" RecursoContrato RecContr ON RecContr.CicloContratoID = CicContr.CicloContratoID INNER JOIN ");
            sCmd.Append(" PruebaContrato PbaContrato ON PbaContrato.RecursoContratoID = RecContr.RecursoContratoID INNER JOIN ");
            sCmd.Append(" Prueba ON Prueba.PruebaID = PbaContrato.PruebaID INNER JOIN ");
            sCmd.Append(" Modelo ON Modelo.ModeloID = Prueba.ModeloID INNER JOIN ");
            sCmd.Append(" GrupoCicloEscolar GpoCicEsclr ON GpoCicEsclr.CicloEscolarID = CicEsclr.CicloEscolarID AND GpoCicEsclr.EscuelaID = LicEscuela.EscuelaID LEFT JOIN ");
            sCmd.Append(" CalendarizacionPruebaGrupo CalendPbaGpo ON CalendPbaGpo.PruebaContratoID = PbaContrato.PruebaContratoID AND CalendPbaGpo.GrupoCicloEscolarID = GpoCicEsclr.GrupoCicloEscolarID ");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (grupoCicloEscolar.GrupoCicloEscolarID == null)
                s_VarWHERE.Append(" GpoCicEsclr.GrupoCicloEscolarID IS NULL ");
            else
            {
                // grupoCicloEscolar.GrupoCicloEscolarID
                s_VarWHERE.Append(" GpoCicEsclr.GrupoCicloEscolarID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = grupoCicloEscolar.GrupoCicloEscolarID;
                sqlParam.DbType = DbType.Guid;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (grupoCicloEscolar.CicloEscolar.CicloEscolarID == null)
                s_VarWHERE.Append(" AND CicEsclr.CicloEscolarID IS NULL ");
            else
            {
                // grupoCicloEscolar.CicloEscolar.CicloEscolarID
                s_VarWHERE.Append(" AND CicEsclr.CicloEscolarID = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = grupoCicloEscolar.CicloEscolar.CicloEscolarID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (grupoCicloEscolar.Escuela.EscuelaID == null)
                s_VarWHERE.Append(" AND LicEscuela.EscuelaID IS NULL ");
            else
            {
                // grupoCicloEscolar.Escuela.EscuelaID
                s_VarWHERE.Append(" AND LicEscuela.EscuelaID = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = grupoCicloEscolar.Escuela.EscuelaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (licenciaEscuela.Contrato.ContratoID == null)
                s_VarWHERE.Append(" AND Contrato.ContratoID IS NULL ");
            else
            {
                // licenciaEscuela.Contrato.ContratoID
                s_VarWHERE.Append(" AND Contrato.ContratoID = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = licenciaEscuela.Contrato.ContratoID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (licenciaEscuela.Contrato.Estatus != null)
            {
                s_VarWHERE.Append(" AND Contrato.Estatus = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = licenciaEscuela.Contrato.Estatus;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (licenciaEscuela.Activo != null)
            {
                s_VarWHERE.Append(" AND LicEscuela.Activo = @dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = licenciaEscuela.Activo;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (grupoCicloEscolar.CicloEscolar.Activo != null)
            {
                s_VarWHERE.Append(" AND CicEsclr.Activo = @dbp4ram7 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram7";
                sqlParam.Value = grupoCicloEscolar.CicloEscolar.Activo;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (grupoCicloEscolar.Activo != null)
            {
                s_VarWHERE.Append(" AND GpoCicEsclr.Activo = @dbp4ram8 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram8";
                sqlParam.Value = grupoCicloEscolar.Activo;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (estadoLiberacion != null)
            {
                s_VarWHERE.Append(" AND Prueba.EstadoLiberacion = @dbp4ram9 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram9";
                sqlParam.Value = estadoLiberacion;
                sqlParam.DbType = DbType.Byte;
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
            sCmd.Append(" ORDER by PbaContrato.PruebaID ");
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "PruebasGrupo");
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
