using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;

namespace POV.CentroEducativo.DAO
{
    /// <summary>
    /// AsignacionMateriaGrupo
    /// </summary>
    public class AsignacionMateriaGrupoUpdHlp
    {
        /// <summary>
        /// Actualiza de manera optimista un registro de AsignacionMateriaGrupo en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="asignacion">AsignacionMateriaGrupo que tiene los datos nuevos </param>
        /// <param name="anterior">AsignacionMateriaGrupo que tiene los datos anteriores</param>
        public void Action(IDataContext dctx, AsignacionMateriaGrupo asignacion, AsignacionMateriaGrupo anterior, GrupoCicloEscolar grupoCicloEscolar)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (asignacion == null)
                sError += ", AsignacionMateriaGrupo";
            if (anterior == null)
                sError += ", Anterior";
            if (grupoCicloEscolar == null)
                sError += ", GrupoCicloEscolar";
            if (sError.Length > 0)
                throw new Exception("AsignacionMateriaGrupoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (grupoCicloEscolar.GrupoCicloEscolarID == null)
                sError += ", GrupoCicloEscolarID";
            if (asignacion.AsignacionMateriaGrupoID == null)
                sError += ", AsignacionMateriaGrupoID";
            if (asignacion.FechaRegistro == null)
                sError += ", FechaRegistro";
            if (asignacion.Activo == null)
                sError += ", Activo";
            if (asignacion.Docente == null)
                sError += ", Docente";
            if (asignacion.Materia == null)
                sError += ", Materia";
            if (anterior.AsignacionMateriaGrupoID == null)
                sError += ", AsignacionMateriaGrupoID anterior";
            if (anterior.FechaRegistro == null)
                sError += ", FechaRegistro anterior";
            if (anterior.Activo == null)
                sError += ", Activo anterior";
            if (anterior.Docente == null)
                sError += ", Docente anterior";
            if (anterior.Materia == null)
                sError += ", Materia anterior";
            if (sError.Length > 0)
                throw new Exception("AsignacionMateriaGrupoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (asignacion.Docente.DocenteID == null)
                sError += ", Docente.DocenteID";
            if (anterior.Docente.DocenteID == null)
                sError += ", Docente.DocenteID anterior";
            if (asignacion.Materia.MateriaID == null)
                sError += ", Materia.MateriaID";
            if (anterior.Materia.MateriaID == null)
                sError += ", Docente.DocenteID anterior";
            if (sError.Length > 0)
                throw new Exception("AsignacionMateriaGrupoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (asignacion.AsignacionMateriaGrupoID != anterior.AsignacionMateriaGrupoID)
            {
                sError = "Los parametros no coinciden";
            }
            if (sError.Length > 0)
                throw new Exception("AsignacionMateriaGrupoUpdHlp: " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", "AsignacionMateriaGrupoUpdHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "AsignacionMateriaGrupoUpdHlp: Ocurrió un error al conectarse a la base de datos", "POV.CentroEducativo.DAO", "AsignacionMateriaGrupoUpdHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" UPDATE ASIGNACIONMATERIAGRUPO ");
            // asignacion.Activo
            if (asignacion.Activo == null)
                sCmd.Append(" SET Activo = NULL ");
            else
            {
                sCmd.Append(" SET Activo =@dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = asignacion.Activo;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            // asignacion.FechaBaja
            if (asignacion.FechaBaja == null)
                sCmd.Append(" ,FechaBaja = NULL ");
            else
            {
                sCmd.Append(" ,FechaBaja =@dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = asignacion.FechaBaja;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            // anterior.AsignacionMateriaGrupoID

            if (anterior.AsignacionMateriaGrupoID == null)
                sCmd.Append(" WHERE AsignacionMateriaGrupoID IS NULL ");
            else
            {
                sCmd.Append(" WHERE AsignacionMateriaGrupoID =@dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = anterior.AsignacionMateriaGrupoID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            // grupoCicloEscolar.GrupoCicloEscolarID
            if (grupoCicloEscolar.GrupoCicloEscolarID == null)
                sCmd.Append(" AND GrupoCicloEscolarID IS NULL ");
            else
            {
                sCmd.Append(" AND GrupoCicloEscolarID =@dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = grupoCicloEscolar.GrupoCicloEscolarID;
                sqlParam.DbType = DbType.Guid;
                sqlCmd.Parameters.Add(sqlParam);
            }
            // anterior.Docente.DocenteID
            if (anterior.Docente.DocenteID == null)
                sCmd.Append(" AND DocenteID IS NULL ");
            else
            {
                sCmd.Append(" AND DocenteID =@dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = anterior.Docente.DocenteID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            // anterior.Materia.MateriaID
            if (anterior.Materia.MateriaID == null)
                sCmd.Append(" AND MateriaID IS NULL ");
            else
            {
                sCmd.Append(" AND MateriaID =@dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = anterior.Materia.MateriaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            // anterior.Activo
            if (anterior.Activo == null)
                sCmd.Append(" AND Activo IS NULL ");
            else
            {
                sCmd.Append(" AND Activo =@dbp4ram7 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram7";
                sqlParam.Value = anterior.Activo;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            // anterior.FechaRegistro
            if (anterior.FechaRegistro == null)
                sCmd.Append(" AND FechaRegistro IS NULL ");
            else
            {
                sCmd.Append(" AND FechaRegistro =@dbp4ram8 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram8";
                sqlParam.Value = anterior.FechaRegistro;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            // anterior.FechaBaja
            if (anterior.FechaBaja == null)
                sCmd.Append(" AND FechaBaja IS NULL ");
            else
            {
                sCmd.Append(" AND FechaBaja =@dbp4ram9 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram9";
                sqlParam.Value = anterior.FechaBaja;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            int iRes = 0;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                iRes = sqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("AsignacionMateriaGrupoUpdHlp: Ocurrio un error al actualizar el objeto o fue modificado mientras era editado. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("AsignacionMateriaGrupoUpdHlp: Ocurrio un error al actualizar el objeto o fue modificado mientras era editado.");
        }
    }
}
