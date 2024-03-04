// Clase motor de red social
using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Comun.BO;
using POV.Localizacion.BO;

namespace POV.Localizacion.DAO { 
   /// <summary>
   /// Guarda un registro de Ubicacion en la BD
   /// </summary>
   public class UbicacionInsHlp { 
      /// <summary>
      /// Crea un registro de Ubicacion en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="ubicacion">Ubicacion que desea crear</param>
      public void Action(IDataContext dctx, Ubicacion ubicacion){
         object myFirm = new object();
         string sError = String.Empty;
         if (ubicacion == null)
            sError += ", Ubicacion";
         if (sError.Length > 0)
            throw new Exception("UbicacionInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (ubicacion.Pais == null)
            sError += ", Pais";
         if (ubicacion.Estado == null)
            sError += ", Estado";
         if (ubicacion.Ciudad == null)
            sError += ", Ciudad";
         if ( ubicacion.FechaRegistro == null )
             sError += ", FechaRegistro ";
         if (sError.Length > 0)
            throw new Exception("UbicacionInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Localizacion.DAO", 
         "UbicacionInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "UbicacionInsHlp: No se pudo conectar a la base de datos", "POV.Localizacion.DAO", 
         "UbicacionInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append( " INSERT INTO Ubicacion ( PaisID, EstadoID, CiudadID, LocalidadID, ColoniaID, FechaRegistro ) " );
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         sCmd.Append(" @ubicacion_Pais_PaisID ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "ubicacion_Pais_PaisID";
         if (ubicacion.Pais.PaisID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = ubicacion.Pais.PaisID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@ubicacion_Estado_EstadoID ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "ubicacion_Estado_EstadoID";
         if (ubicacion.Estado.EstadoID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = ubicacion.Estado.EstadoID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@ubicacion_Ciudad_CiudadID ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "ubicacion_Ciudad_CiudadID";
         if (ubicacion.Ciudad.CiudadID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = ubicacion.Ciudad.CiudadID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@ubicacion_Localidad_LocalidadID ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "ubicacion_Localidad_LocalidadID";
         if (ubicacion.Localidad.LocalidadID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = ubicacion.Localidad.LocalidadID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@ubicacion_Colonia_ColoniaID ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "ubicacion_Colonia_ColoniaID";
         if (ubicacion.Colonia.ColoniaID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = ubicacion.Colonia.ColoniaID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append( " ,@ubicacion_FechaRegistro " );
         sqlParam = sqlCmd.CreateParameter( );
         sqlParam.ParameterName = "ubicacion_FechaRegistro";
         if ( ubicacion.FechaRegistro == null )
             sqlParam.Value = DBNull.Value;
         else
             sqlParam.Value = ubicacion.FechaRegistro;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add( sqlParam );
         sCmd.Append(" ) ");
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("UbicacionInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("UbicacionInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
