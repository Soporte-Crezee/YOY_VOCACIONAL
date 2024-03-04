using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;
using POV.Comun.BO;
using GP.SocialEngine.DAO;

namespace GP.SocialEngine.Service { 
   /// <summary>
   /// Controlador del objeto ImagenPerfil
   /// </summary>
   public class ImagenPerfilCtrl { 
      /// <summary>
      /// Consulta registros de ImagenPerfilRetHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="imagenPerfilRetHlp">ImagenPerfilRetHlp que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de ImagenPerfilRetHlp generada por la consulta</returns>
      public DataSet Retrieve(IDataContext dctx, UsuarioSocial usuarioSocial, ImagenPerfil imagenPerfil){
         ImagenPerfilRetHlp da = new ImagenPerfilRetHlp();
         DataSet ds = da.Action(dctx, usuarioSocial, imagenPerfil);
         return ds;
      }
      /// <summary>
      /// Crea un registro de ImagenPerfilInsHlp en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="imagenPerfilInsHlp">ImagenPerfilInsHlp que desea crear</param>
      public void Insert(IDataContext dctx, UsuarioSocial usuarioSocial, ImagenPerfil  imagenPerfil){
         ImagenPerfilInsHlp da = new ImagenPerfilInsHlp();
         da.Action(dctx, usuarioSocial,  imagenPerfil);
      }
      /// <summary>
      /// Crea un objeto de ImagenPerfil a partir de los datos del último DataRow del DataSet.
      /// </summary>
      /// <param name="ds">El DataSet que contiene la información de ImagenPerfil</param>
      /// <returns>Un objeto de ImagenPerfil creado a partir de los datos</returns>
      public ImagenPerfil LastDataRowToImagenPerfil(DataSet ds) {
         if (!ds.Tables.Contains("ImagenPerfil"))
            throw new Exception("LastDataRowToImagenPerfil: DataSet no tiene la tabla ImagenPerfil");
         int index = ds.Tables["ImagenPerfil"].Rows.Count;
         if (index < 1)
            throw new Exception("LastDataRowToImagenPerfil: El DataSet no tiene filas");
         return this.DataRowToImagenPerfil(ds.Tables["ImagenPerfil"].Rows[index - 1]);
      }
      /// <summary>
      /// Crea un objeto de ImagenPerfil a partir de los datos de un DataRow.
      /// </summary>
      /// <param name="row">El DataRow que contiene la información de ImagenPerfil</param>
      /// <returns>Un objeto de ImagenPerfil creado a partir de los datos</returns>
      public ImagenPerfil DataRowToImagenPerfil(DataRow row){
         ImagenPerfil imagenPerfil = new ImagenPerfil();
         imagenPerfil.AdjuntoImagen = new AdjuntoImagen();
         if (row.IsNull("AdjuntoImagenID"))
            imagenPerfil.AdjuntoImagen.AdjuntoImagenID = null;
         else
            imagenPerfil.AdjuntoImagen.AdjuntoImagenID = (long)Convert.ChangeType(row["AdjuntoImagenID"], typeof(long));
         if (row.IsNull("Estatus"))
            imagenPerfil.Estatus = null;
         else
            imagenPerfil.Estatus = (bool)Convert.ChangeType(row["Estatus"], typeof(bool));
         return imagenPerfil;
      }
   } 
}
