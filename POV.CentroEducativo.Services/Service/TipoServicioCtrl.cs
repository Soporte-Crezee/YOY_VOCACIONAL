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
   /// Controlador del objeto TipoServicio
   /// </summary>
   public class TipoServicioCtrl { 
      /// <summary> 
      /// Consulta registros de TipoServicioRetHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="tipoServicioRetHlp">TipoServicioRetHlp que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de TipoServicioRetHlp generada por la consulta</returns>
      public DataSet Retrieve(IDataContext dctx, TipoServicio tipoServicio){
         TipoServicioRetHlp da = new TipoServicioRetHlp();
         DataSet ds = da.Action(dctx, tipoServicio);
         return ds;
      }
      /// <summary>
      /// Crea un registro de TipoServicioInsHlp en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="tipoServicioInsHlp">TipoServicioInsHlp que desea crear</param>
      public void Insert(IDataContext dctx, TipoServicio  tipoServicio){
         TipoServicioInsHlp da = new TipoServicioInsHlp();
         da.Action(dctx,  tipoServicio);
      }
      /// <summary>
      /// Actualiza de manera optimista un registro de TipoServicioUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="tipoServicioUpdHlp">TipoServicioUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">TipoServicioUpdHlp que tiene los datos anteriores</param>
      public void Update(IDataContext dctx, TipoServicio  tipoServicio, TipoServicio previous){
         TipoServicioUpdHlp da = new TipoServicioUpdHlp();
         da.Action(dctx,  tipoServicio, previous);
      }
       /// <summary>
       /// Consulta un Tipo de Servicio con su Nivel Educativo correspondiente
       /// </summary>
       /// <param name="dctx">El DataConetext que proveerá acceso a la base de datos</param>
       /// <param name="tipoServicio">TipoServicio a consultar</param>
       /// <returns>TipoServicio</returns>
      public TipoServicio RetriveComplete(IDataContext dctx,TipoServicio tipoServicio)
      {
          if(tipoServicio==null)
              throw new Exception("RetriveComplete:TipoServicio no puede ser nulo");
          if(tipoServicio.TipoServicioID==null)
              throw new Exception("RetriveComplete:TipoServicoID no puede ser nulo");
          DataSet dsTipoSerivicio = Retrieve(dctx, new TipoServicio {TipoServicioID = tipoServicio.TipoServicioID});
          if(dsTipoSerivicio.Tables[0].Rows.Count!=1)
              throw new Exception("RetriveComplete:No se encontró el tipo de servicio solicitado o se encontró más de un tipo de servicio");

          TipoServicio dtipoServicio = LastDataRowToTipoServicio(dsTipoSerivicio);

          dtipoServicio.NivelEducativoID = (new NivelEducativoCtrl()).RetriveComplete(dctx, new NivelEducativo { NivelEducativoID = dtipoServicio.NivelEducativoID.NivelEducativoID });
          return dtipoServicio;
      }

       /// <summary>
      /// Crea un objeto de TipoServicio a partir de los datos del último DataRow del DataSet.
      /// </summary>
      /// <param name="ds">El DataSet que contiene la información de TipoServicio</param>
      /// <returns>Un objeto de TipoServicio creado a partir de los datos</returns>
      public TipoServicio LastDataRowToTipoServicio(DataSet ds) {
         if (!ds.Tables.Contains("TipoServicio"))
            throw new Exception("LastDataRowToTipoServicio: DataSet no tiene la tabla TipoServicio");
         int index = ds.Tables["TipoServicio"].Rows.Count;
         if (index < 1)
            throw new Exception("LastDataRowToTipoServicio: El DataSet no tiene filas");
         return this.DataRowToTipoServicio(ds.Tables["TipoServicio"].Rows[index - 1]);
      }
      /// <summary>
      /// Crea un objeto de TipoServicio a partir de los datos de un DataRow.
      /// </summary>
      /// <param name="row">El DataRow que contiene la información de TipoServicio</param>
      /// <returns>Un objeto de TipoServicio creado a partir de los datos</returns>
      public TipoServicio DataRowToTipoServicio(DataRow row){
         TipoServicio tipoServicio = new TipoServicio();
         if (tipoServicio.NivelEducativoID == null)
             tipoServicio.NivelEducativoID = new NivelEducativo();
         if (row.IsNull("TipoServicioID"))
            tipoServicio.TipoServicioID = null;
         else
            tipoServicio.TipoServicioID = (int)Convert.ChangeType(row["TipoServicioID"], typeof(int));
         if (row.IsNull("Clave"))
             tipoServicio.Clave = null;
         else
             tipoServicio.Clave = (string)Convert.ChangeType(row["Clave"], typeof(string));
         if (row.IsNull("Nombre"))
             tipoServicio.Nombre = null;
         else
             tipoServicio.Nombre = (string)Convert.ChangeType(row["Nombre"], typeof(string));
         if (row.IsNull("NivelEducativoID"))
             tipoServicio.NivelEducativoID.NivelEducativoID = null;
         else
             tipoServicio.NivelEducativoID.NivelEducativoID = (int)Convert.ChangeType(row["NivelEducativoID"], typeof(int)); 
          
          return tipoServicio;
      }
   } 
}
