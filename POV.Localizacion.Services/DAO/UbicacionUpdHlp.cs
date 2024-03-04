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
   /// Actualiza un registro de Ubicacion en la BD
   /// </summary>
   public class UbicacionUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de UbicacionUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="ubicacionUpdHlp">UbicacionUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">UbicacionUpdHlp que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, Ubicacion ubicacion, Ubicacion anterior){
         object myFirm = new object();
         String sError = string.Empty;
         if (ubicacion == null)
            sError += ", Ubicacion";
         if (anterior == null)
            sError += ", Anterior";
         if (sError.Length > 0)
            throw new Exception("UbicacionUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (anterior.UbicacionID == null)
            sError += ", Anterior UbicacionID";
         if (sError.Length > 0)
            throw new Exception("UbicacionUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Localizacion.DAO", 
         "UbicacionUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "UbicacionUpdHlpUpdHl: Hubo un error al conectarse a la base de datos", "POV.Localizacion.DAO", 
         "UbicacionUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE Ubicacion ");
         if (ubicacion.Pais.PaisID == null)
            sCmd.Append(" SET PaisID = NULL ");
         else{ 
            sCmd.Append(" SET PaisID = @ubicacion_Pais_PaisID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "ubicacion_Pais_PaisID";
            sqlParam.Value = ubicacion.Pais.PaisID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (ubicacion.Estado.EstadoID == null)
            sCmd.Append(" ,EstadoID = NULL ");
         else{ 
            sCmd.Append(" ,EstadoID = @ubicacion_Estado_EstadoID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "ubicacion_Estado_EstadoID";
            sqlParam.Value = ubicacion.Estado.EstadoID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         sCmd.Append(" ,CiudadID=@ubicacion_Ciudad_CiudadID ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "ubicacion_Ciudad_CiudadID";
         if (ubicacion.Ciudad.CiudadID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = ubicacion.Ciudad.CiudadID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,Localidad=@ubicacion_Localidad_LocalidadID ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "ubicacion_Localidad_LocalidadID";
         if (ubicacion.Localidad.LocalidadID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = ubicacion.Localidad.LocalidadID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,Colonia=@ubicacion_Colonia_ColoniaID ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "ubicacion_Colonia_ColoniaID";
         if (ubicacion.Colonia.ColoniaID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = ubicacion.Colonia.ColoniaID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         if (anterior.UbicacionID == null)
            sCmd.Append(" ,WHERE ubicacionID = NULL ");
         else{ 
            sCmd.Append(" ,WHERE ubicacionID = @anterior_UbicacionID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "anterior_UbicacionID";
            sqlParam.Value = anterior.UbicacionID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("UbicacionUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("UbicacionUpdHlp: Hubo  un Error al Actualizar el Registro .");
      }
   } 
}
