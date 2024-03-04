using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Prueba.BO;

namespace POV.Prueba.Diagnostico.DAO
{
    /// <summary>
    /// Guarda un registro de APrueba en la BD
    /// </summary>
    internal class PruebaInsHlp
    {
        /// <summary>
        /// Crea un registro de APrueba en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="aPrueba">APrueba que desea crear</param>
        public void Action(IDataContext dctx, APrueba prueba)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (prueba == null)
                sError += ", APrueba";
            if (sError.Length > 0)
                throw new Exception("PruebaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (prueba.Modelo == null)
                sError += ", Modelo";
            if (sError.Length > 0)
                throw new Exception("PruebaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (prueba.Clave == null || prueba.Clave.Trim().Length == 0)
                sError += ", Clave";
            if (prueba.Nombre == null || prueba.Nombre.Trim().Length == 0)
                sError += ", Nombre";
            if (prueba.Instrucciones == null || prueba.Instrucciones.Trim().Length == 0)
                sError += ", Instrucciones";
            if (prueba.FechaRegistro == null)
                sError += ", FechaRegistro";
            if (prueba.EsDiagnostica == null)
                sError += ", EsDiagnostica";
            if (prueba.EstadoLiberacionPrueba == null)
                sError += ", EstadoLiberacionPrueba";
            if (sError.Length > 0)
                throw new Exception("PruebaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Prueba.Diagnostico.DAO",
                   "PruebaInsHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "PruebaInsHlp: No se pudo conectar a la base de datos", "POV.Prueba.Diagnostico.DAO",
                   "PruebaInsHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" INSERT INTO Prueba (Clave, Nombre, Instrucciones, FechaRegistro, EsDiagnostica, ModeloID, Tipo, EstadoLiberacion, EsPremium, TipoPruebaPresentacion) ");
            sCmd.Append(" VALUES ");
            sCmd.Append(" ( ");

            sCmd.Append(" @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            if (prueba.Clave == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = prueba.Clave;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // prueba.Nombre
            sCmd.Append(" ,@dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            if (prueba.Nombre == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = prueba.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // prueba.Instrucciones
            sCmd.Append(" ,@dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            if (prueba.Instrucciones == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = prueba.Instrucciones;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // prueba.FechaRegistro
            sCmd.Append(" ,@dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            if (prueba.FechaRegistro == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = prueba.FechaRegistro;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
            // prueba.EsDiagnostica
            sCmd.Append(" ,@dbp4ram7 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram7";
            if (prueba.EsDiagnostica == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = prueba.EsDiagnostica;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            // prueba.Modelo.ModeloID
            sCmd.Append(" ,@dbp4ram8 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram8";
            if (prueba.Modelo.ModeloID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = prueba.Modelo.ModeloID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            // prueba.TipoPrueba
            sCmd.Append(" ,@dbp4ram9 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram9";
            if (prueba.TipoPrueba == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = prueba.TipoPrueba;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);
            // prueba.EstadoLiberacionPrueba
            sCmd.Append(" ,@dbp4ram10 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram10";
            if (prueba.EstadoLiberacionPrueba == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = prueba.EstadoLiberacionPrueba;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);
            // prueba.EsPremium
            sCmd.Append(" ,@dbp4ram11 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram11";
            sqlParam.Value = prueba.EsPremium;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            // prueba.TipoPruebaPresentacion
            sCmd.Append(" ,@dbp4ram12 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram12";
            if (prueba.TipoPruebaPresentacion == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = prueba.TipoPruebaPresentacion;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ) ");

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
                throw new Exception("PruebaInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("PruebaInsHlp: Ocurrió un error al ingresar el registro.");
        }
    }
}
