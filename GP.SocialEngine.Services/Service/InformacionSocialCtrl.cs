using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;
using GP.SocialEngine.DAO;

namespace GP.SocialEngine.Service { 
   /// <summary>
   /// Controlador del objeto InformacionSocial
   /// </summary>
   public class InformacionSocialCtrl { 
      /// <summary>
      /// Consulta registros de InformacionSocialRetHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
      /// <param name="informacionSocialRetHlp">InformacionSocialRetHlp que provee el criterio de selecciÃ³n para realizar la consulta</param>
      /// <returns>El DataSet que contiene la informaciÃ³n de InformacionSocialRetHlp generada por la consulta</returns>
      public DataSet Retrieve(IDataContext dctx, InformacionSocial informacionSocial){
         InformacionSocialRetHlp da = new InformacionSocialRetHlp();
         DataSet ds = da.Action(dctx, informacionSocial);
         return ds;
      }
      /// <summary>
      /// Crea un registro de InformacionSocialInsHlp en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
      /// <param name="informacionSocialInsHlp">InformacionSocialInsHlp que desea crear</param>
      public void Insert(IDataContext dctx, InformacionSocial  informacionSocial){
         InformacionSocialInsHlp da = new InformacionSocialInsHlp();
         da.Action(dctx,  informacionSocial);
      }
      /// <summary>
      /// Actualiza de manera optimista un registro de InformacionSocialUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
      /// <param name="informacionSocialUpdHlp">InformacionSocialUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">InformacionSocialUpdHlp que tiene los datos anteriores</param>
      public void Update(IDataContext dctx, InformacionSocial  informacionSocial, InformacionSocial previous){
         InformacionSocialUpdHlp da = new InformacionSocialUpdHlp();
         da.Action(dctx,  informacionSocial, previous);
      }
      /// <summary>
      /// Elimina un registro de Elimina un registro de informacion Social en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
      /// <param name="eliminaunregistrodeinformacionSocial">Elimina un registro de informacion Social que desea eliminar</param>
      public void Delete(IDataContext dctx, InformacionSocial informacionSocial)
      {
          InformacionSocialDelHlp da = new InformacionSocialDelHlp();
          da.Action(dctx, informacionSocial);
      }

      /// <summary>
      /// Crea un objeto de InformacionSocial a partir de los datos del Ãºltimo DataRow del DataSet.
      /// </summary>
      /// <param name="ds">El DataSet que contiene la informaciÃ³n de InformacionSocial</param>
      /// <returns>Un objeto de InformacionSocial creado a partir de los datos</returns>
      public InformacionSocial LastDataRowToInformacionSocial(DataSet ds) {
         if (!ds.Tables.Contains("InformacionSocial"))
            throw new Exception("LastDataRowToInformacionSocial: DataSet no tiene la tabla InformacionSocial");
         int index = ds.Tables["InformacionSocial"].Rows.Count;
         if (index < 1)
            throw new Exception("LastDataRowToInformacionSocial: El DataSet no tiene filas");
         return this.DataRowToInformacionSocial(ds.Tables["InformacionSocial"].Rows[index - 1]);
      }
      /// <summary>
      /// Crea un objeto de InformacionSocial a partir de los datos de un DataRow.
      /// </summary>
      /// <param name="row">El DataRow que contiene la informaciÃ³n de InformacionSocial</param>
      /// <returns>Un objeto de InformacionSocial creado a partir de los datos</returns>
      public InformacionSocial DataRowToInformacionSocial(DataRow row){
         InformacionSocial informacionSocial = new InformacionSocial();
         if (row.IsNull("InformacionSocialID"))
            informacionSocial.InformacionSocialID = null;
         else
            informacionSocial.InformacionSocialID = (long)Convert.ChangeType(row["InformacionSocialID"], typeof(long));
         if (row.IsNull("IMMSN"))
            informacionSocial.IMMSN = null;
         else
            informacionSocial.IMMSN = (string)Convert.ChangeType(row["IMMSN"], typeof(string));
         if (row.IsNull("IMSkype"))
            informacionSocial.IMSkype = null;
         else
            informacionSocial.IMSkype = (string)Convert.ChangeType(row["IMSkype"], typeof(string));
         if (row.IsNull("IMAOL"))
            informacionSocial.IMAOL = null;
         else
            informacionSocial.IMAOL = (string)Convert.ChangeType(row["IMAOL"], typeof(string));
         if (row.IsNull("IMICQ"))
            informacionSocial.IMICQ = null;
         else
            informacionSocial.IMICQ = (string)Convert.ChangeType(row["IMICQ"], typeof(string));
         if (row.IsNull("IMYahoo"))
            informacionSocial.IMYahoo = null;
         else
            informacionSocial.IMYahoo = (string)Convert.ChangeType(row["IMYahoo"], typeof(string));
         if (row.IsNull("Firma"))
            informacionSocial.Firma = null;
         else
            informacionSocial.Firma = (string)Convert.ChangeType(row["Firma"], typeof(string));
         if (row.IsNull("FechaRegistro"))
            informacionSocial.FechaRegistro = null;
         else
            informacionSocial.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
         return informacionSocial;
      }
   } 
}
