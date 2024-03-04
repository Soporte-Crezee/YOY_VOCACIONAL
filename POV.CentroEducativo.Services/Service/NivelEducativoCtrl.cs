using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.DAO;

namespace POV.CentroEducativo.Service { 
   /// <summary>
   /// Controlador del objeto NivelEducativo
   /// </summary>
   public class NivelEducativoCtrl { 
      /// <summary>
      /// Consulta registros de NivelEducativoRetHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="nivelEducativo">NivelEducativo que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de NivelEducativoRetHlp generada por la consulta</returns>
      public DataSet Retrieve(IDataContext dctx, NivelEducativo nivelEducativo){
         NivelEducativoRetHlp da = new NivelEducativoRetHlp();
         DataSet ds = da.Action(dctx, nivelEducativo);
         return ds;
      }
      /// <summary>
      /// Crea un registro de NivelEducativoInsHlp en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="nivelEducativo">NivelEducativoInsHlp que desea crear</param>
      public void Insert(IDataContext dctx, NivelEducativo nivelEducativo){
         NivelEducativoInsHlp da = new NivelEducativoInsHlp();
         da.Action(dctx, nivelEducativo);
      }
      /// <summary>
      /// Crea un objeto de NivelEducativo a partir de los datos del último DataRow del DataSet.
      /// </summary>
      /// <param name="ds">El DataSet que contiene la información de NivelEducativo</param>
      /// <returns>Un objeto de NivelEducativo creado a partir de los datos</returns>
      public NivelEducativo LastDataRowToNivelEducativo(DataSet ds) {
         if (!ds.Tables.Contains("NivelEducativo"))
            throw new Exception("LastDataRowToNivelEducativo: DataSet no tiene la tabla NivelEducativo");
         int index = ds.Tables["NivelEducativo"].Rows.Count;
         if (index < 1)
            throw new Exception("LastDataRowToNivelEducativo: El DataSet no tiene filas");
         return this.DataRowToNivelEducativo(ds.Tables["NivelEducativo"].Rows[index - 1]);
      }
      /// <summary>
      /// Crea un objeto de NivelEducativo a partir de los datos de un DataRow.
      /// </summary>
      /// <param name="row">El DataRow que contiene la información de NivelEducativo</param>
      /// <returns>Un objeto de NivelEducativo creado a partir de los datos</returns>
      public NivelEducativo DataRowToNivelEducativo(DataRow row){
         NivelEducativo nivelEducativo = new NivelEducativo();
      if (nivelEducativo.TipoNivelEducativoID==null) {
         nivelEducativo.TipoNivelEducativoID = new TipoNivelEducativo();
      }
         if (row.IsNull("NivelEducativoID"))
            nivelEducativo.NivelEducativoID = null;
         else
            nivelEducativo.NivelEducativoID = (int)Convert.ChangeType(row["NivelEducativoID"], typeof(int));
         if (row.IsNull("Titulo"))
            nivelEducativo.Titulo = null;
         else
            nivelEducativo.Titulo = (string)Convert.ChangeType(row["Titulo"], typeof(string));
         if (row.IsNull("Descripcion"))
            nivelEducativo.Descripcion = null;
         else
            nivelEducativo.Descripcion = (string)Convert.ChangeType(row["Descripcion"], typeof(string));
         if (row.IsNull("NumeroGrados"))
            nivelEducativo.NumeroGrados = null;
         else
            nivelEducativo.NumeroGrados = (byte)Convert.ChangeType(row["NumeroGrados"], typeof(byte));
         if (row.IsNull("TipoNivelEducativoID"))
            nivelEducativo.TipoNivelEducativoID.TipoNivelEducativoID = null;
         else
            nivelEducativo.TipoNivelEducativoID.TipoNivelEducativoID = (int)Convert.ChangeType(row["TipoNivelEducativoID"], typeof(int));
         return nivelEducativo;
      }
       /// <summary>
       /// Consulta el NivelEducativo y su TipoNivelEducativo
       /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
       /// <param name="nivelEducativo">NivelEducativo que desea consultar</param>
       /// <returns>NivelEducativo que se consulta</returns>
       public NivelEducativo RetriveComplete(IDataContext dctx,NivelEducativo nivelEducativo)
       {
           if(nivelEducativo==null)
               throw new Exception("RetriveComplete:NivelEducativo no puede ser nulo");
           if(nivelEducativo.NivelEducativoID==null)
               throw new Exception("RetriveComplete:NivelEducativoID no puede ser nulo");

           DataSet ds = Retrieve(dctx,new NivelEducativo {NivelEducativoID = nivelEducativo.NivelEducativoID});
           if(ds.Tables[0].Rows.Count!=1)
               throw new Exception("RetriveComplete:No se encontró el nivel educativo solicitado o se encontró mas de un resultado");
           
           NivelEducativo dNivelEducativo = LastDataRowToNivelEducativo(ds);
           TipoNivelEducativoCtrl tipoNivelEducativoCtrl = new TipoNivelEducativoCtrl();
           ds.Clear();
           ds = tipoNivelEducativoCtrl.Retrieve(dctx,new TipoNivelEducativo{TipoNivelEducativoID = dNivelEducativo.TipoNivelEducativoID.TipoNivelEducativoID});
           if(ds.Tables[0].Rows.Count==1)
               dNivelEducativo.TipoNivelEducativoID = tipoNivelEducativoCtrl.LastDataRowToTipoNivelEducativo(ds);

           return dNivelEducativo;
       }
   } 
}
