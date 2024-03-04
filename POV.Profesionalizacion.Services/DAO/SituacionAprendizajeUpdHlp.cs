using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Profesionalizacion.BO;

namespace POV.Profesionalizacion.DAO
{
    /// <summary>
    /// Actualiza un registro de SituacionAprendizaje en la BD
    /// </summary>
    internal class SituacionAprendizajeUpdHlp
    {
        /// <summary>
        /// Actualiza un registro de SituacionAprendizaje en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="ejeTematico">EjetTematico de la Situación de Aprendizaje</param>
        /// <param name="situacionAprendizaje">SituacionAprendizaje que desea crear</param>
        /// <param name="anterior">SituacionAprendizaje Anterior</param>
        public void Action(IDataContext dctx, EjeTematico ejeTematico, SituacionAprendizaje situacionAprendizaje, SituacionAprendizaje anterior)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (ejeTematico == null)
                sError += ", EjeTematico";
            if (situacionAprendizaje == null)
                sError += ", SituacionAprendizaje";
            if (anterior == null)
                sError += ", SituacionAprendizaje anterior";
            if (sError.Length > 0)
                throw new Exception("SituacionAprendizajeInsHlp: Los siguientes objetos no pueden ser vacios " + sError.Substring(2));
            if (ejeTematico.EjeTematicoID == null)
                sError += ", EjeTematicoID";
            if (situacionAprendizaje.SituacionAprendizajeID == null)
                sError += ", SituacionAprendizajeID";
            if (anterior.SituacionAprendizajeID == null)
                sError += ", anterior.SituacionAprendizajeID";
            if (situacionAprendizaje.Nombre == null || situacionAprendizaje.Nombre.Trim().Length == 0)
                sError += ", Nombre";
            if (situacionAprendizaje.AgrupadorContenidoDigital.AgrupadorContenidoDigitalID == null || situacionAprendizaje.AgrupadorContenidoDigital.AgrupadorContenidoDigitalID == 0)
                sError += ", AgrupadorContenidoDigital.AgrupadorContenidoDigitalID";
            if (situacionAprendizaje.FechaRegistro == null)
                sError += ", FechaRegistro";
            if (situacionAprendizaje.EstatusProfesionalizacion == null)
                sError += ", EstatusProfesionalizacion";
            if (sError.Length > 0)
                throw new Exception("SituacionAprendizajeUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DAO",
                   "SituacionAprendizajeUpdHlp", "Action", null, null);
            if (situacionAprendizaje.SituacionAprendizajeID != anterior.SituacionAprendizajeID)
                throw new StandardException(MessageType.Error, "Campos incongruentes", "Los identificadores para actualizar no coinciden", "POV.Profesionalizacion.DAO", "SituacionAprendizajeUpdHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "SituacionAprendizajeUpdHlp: No se pudo conectar a la base de datos", "POV.Profesionalizacion.DAO",
                   "SituacionAprendizajeUpdHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" Update SituacionAprendizaje");
            if (situacionAprendizaje.Nombre == null)
                sCmd.Append(" SET Nombre = NULL ");
            else
            {
                // situacionAprendizaje.Nombre
                sCmd.Append(" SET Nombre = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = situacionAprendizaje.Nombre;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (situacionAprendizaje.Descripcion == null)
                sCmd.Append(" ,Descripcion = NULL ");
            else
            {
                // situacionAprendizaje.Descripcion
                sCmd.Append(" ,Descripcion = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = situacionAprendizaje.Descripcion;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (situacionAprendizaje.FechaRegistro == null)
                sCmd.Append(" ,FechaRegistro = NULL ");
            else
            {
                // situacionAprendizaje.FechaRegistro
                sCmd.Append(" ,FechaRegistro = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = situacionAprendizaje.FechaRegistro;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (situacionAprendizaje.EstatusProfesionalizacion == null)
                sCmd.Append(" ,EstatusProfesionalizacion = NULL ");
            else
            {
                // situacionAprendizaje.EstatusProfesionalizacion
                sCmd.Append(" ,EstatusProfesionalizacion = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = situacionAprendizaje.EstatusProfesionalizacion;
                sqlParam.DbType = DbType.Byte;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (situacionAprendizaje.AgrupadorContenidoDigital.AgrupadorContenidoDigitalID == null)
                sCmd.Append(" ,AgrupadorContenidoDigitalID = NULL ");
            else
            {
                // situacionAprendizaje.AgrupadorContenidoDigitalID
                sCmd.Append(" ,AgrupadorContenidoDigitalID = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = situacionAprendizaje.AgrupadorContenidoDigital.AgrupadorContenidoDigitalID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (ejeTematico.EjeTematicoID == null)
                sCmd.Append(" ,EjeTematicoID = NULL ");
            else
            {
                 //ejeTematico.EjeTematicoID
                sCmd.Append(" ,EjeTematicoID = @dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = ejeTematico.EjeTematicoID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.SituacionAprendizajeID == null)
                sCmd.Append(" WHERE SituacionAprendizajeID = NULL ");
            else
            {
                // anterior.SituacionAprendizajeID
                sCmd.Append(" WHERE SituacionAprendizajeID = @dbp4ram7 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram7";
                sqlParam.Value = anterior.SituacionAprendizajeID;
                sqlParam.DbType = DbType.Int64;
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
                throw new Exception("SituacionAprendizajeUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("SituacionAprendizajeUpdHlp: Hubo  un Error al Actualizar el Registro .");
        }
    }
}
