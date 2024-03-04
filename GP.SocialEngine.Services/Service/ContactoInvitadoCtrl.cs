using GP.SocialEngine.BO;
using GP.SocialEngine.DAO;
using System.Data;
using System;
using Framework.Base.DataAccess;

namespace GP.SocialEngine.Service { 
   /// <summary>
   /// Controlador del Contacto Invitado
   /// </summary>
   public class ContactoInvitadoCtrl { 
      /// <summary>
      /// Crea un registro de Insert en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="insert">Insert que desea crear</param>
      public void Insert(IDataContext dctx, ContactoInvitado contactoInvitado){
         ContactoInvitadoInsHlp da = new ContactoInvitadoInsHlp();
         da.Action(dctx, contactoInvitado);
      }
      /// <summary>
      /// Consulta registros de Retrieve en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="retrieve">Retrieve que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de Retrieve generada por la consulta</returns>
      public DataSet Retrieve(IDataContext dctx, ContactoInvitado contactoInvitado){
         ContactoInvitadoRetHlp da = new ContactoInvitadoRetHlp();
         DataSet ds = da.Action(dctx, contactoInvitado);
         return ds;
      }
      /// <summary>
      /// Crea un objeto de DataRowToContactoInvitado a partir de los datos del último DataRow del DataSet.
      /// </summary>
      /// <param name="ds">El DataSet que contiene la información de DataRowToContactoInvitado</param>
      /// <returns>Un objeto de DataRowToContactoInvitado creado a partir de los datos</returns>
      public ContactoInvitado LastDataRowToContactoInvitado(DataSet ds) {
         if (!ds.Tables.Contains("ContactoInvitado"))
            throw new Exception("LastDataRowToContactoInvitado: DataSet no tiene la tabla ContactoInvitado");
         int index = ds.Tables["ContactoInvitado"].Rows.Count;
         if (index < 1)
            throw new Exception("LastDataRowToContactoInvitado: El DataSet no tiene filas");
         return this.DataRowToContactoInvitado(ds.Tables["ContactoInvitado"].Rows[index - 1]);
      }
      /// <summary>
      /// Crea un objeto de DataRowToContactoInvitado a partir de los datos de un DataRow.
      /// </summary>
      /// <param name="row">El DataRow que contiene la información de DataRowToContactoInvitado</param>
      /// <returns>Un objeto de DataRowToContactoInvitado creado a partir de los datos</returns>
      public ContactoInvitado DataRowToContactoInvitado(DataRow row){
         ContactoInvitado contactoInvitado = new ContactoInvitado();
         if (row.IsNull("ContactoInvitadoID"))
            contactoInvitado.ContactoInvitadoID = null;
         else
            contactoInvitado.ContactoInvitadoID = (int)Convert.ChangeType(row["ContactoInvitadoID"], typeof(int));
         if (row.IsNull("NombreCompleto"))
            contactoInvitado.NombreCompleto = null;
         else
            contactoInvitado.NombreCompleto = (string)Convert.ChangeType(row["NombreCompleto"], typeof(string));
         if (row.IsNull("CorreoElectronico"))
            contactoInvitado.CorreoElectronico = null;
         else
            contactoInvitado.CorreoElectronico = (string)Convert.ChangeType(row["CorreoElectronico"], typeof(string));
         return contactoInvitado;
      }
   } 
}
