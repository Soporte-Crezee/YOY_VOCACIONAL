using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.DAO;
using POV.Localizacion.Service;
using POV.Localizacion.BO;
using POV.CentroEducativo.DA;
using POV.Seguridad.BO;
 
namespace POV.CentroEducativo.Service { 
   /// <summary>
   /// Controlador del objeto Docente
   /// </summary>
   public class DocenteCtrl { 
      /// <summary>
      /// Consulta registros de DocenteRetHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="docenteRetHlp">DocenteRetHlp que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de DocenteRetHlp generada por la consulta</returns>
      public DataSet Retrieve(IDataContext dctx, Docente docente){
         DocenteRetHlp da = new DocenteRetHlp();
         DataSet ds = da.Action(dctx, docente);
         return ds;
      }
      /// <summary>
      /// Crea un registro de DocenteInsHlp en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="docenteInsHlp">DocenteInsHlp que desea crear</param>
      public void Insert(IDataContext dctx, Docente  docente){
         DocenteInsHlp da = new DocenteInsHlp();
         da.Action(dctx,  docente);
      }
      /// <summary>
      /// Actualiza de manera optimista un registro de DocenteUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="docenteUpdHlp">DocenteUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">DocenteUpdHlp que tiene los datos anteriores</param>
      public void Update(IDataContext dctx, Docente  docente, Docente previous){
         DocenteUpdHlp da = new DocenteUpdHlp();
          if (docente.Curp != previous.Curp)
          {
              if (NoRepetido(dctx, docente))
                  da.Action(dctx, docente, previous);
              else
                  throw new Exception("Update: El curp que intenta agregar ya esta asignado a otro Docente.");
          }
          else
              da.Action(dctx, docente, previous);
      }

      /// <summary>
      /// Actualiza de manera optimista la información del Docente su Usuario y Escuelas asignadas al docente.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="docente">El docente que tiene los datos nuevos</param>
      /// <param name="previous">El docente que tiene los datos anteriores</param>
      public void UpdateComplete(IDataContext dctx,Docente docente,Docente previous)
      {
          if (docente == null)
              throw new ArgumentNullException("DocenteCtrl:docente no puede ser nulo");

          if (previous == null)
              throw new ArgumentNullException("DocenteCtrl:previous no puede ser nulo");

          DocenteEscuelaCtrl docenteEscuelaCtrl = new DocenteEscuelaCtrl();
          object myFirm = new object();
          DataSet dsDocenteEscuela = new DataSet();
          List<DocenteEscuela> lsDocenteEscuelaPrevious = new List<DocenteEscuela>();
          
          try
          {
              dctx.OpenConnection(dctx);
              dctx.BeginTransaction(myFirm);

              Update(dctx,docente,previous);              
             
              dctx.CommitTransaction(myFirm);
          }
          catch (Exception ex)
          {
              dctx.RollbackTransaction(myFirm);
              dctx.CommitTransaction(myFirm);

              throw new Exception("DocenteCtrl:UpdateComplete ocurrió un error, "+ex.Message);
          }                
      }

       /// <summary>
      /// Devuelve un docente completo
      /// </summary>
      public Docente RetrieveComplete(IDataContext dctx, Docente docente){
          
          DataSet ds = Retrieve( dctx , docente );

          DocenteEscuelaCtrl docenteEscuelaCtrl = new DocenteEscuelaCtrl( );

          if ( ds.Tables[ "Docente" ].Rows.Count == 0 )
          throw new Exception( "DocenteCtrl: No se encontró ningún Usuario con los parámetros proporcionados" );
          docente = LastDataRowToDocente( ds );          
          
          return docente;
      }
      /// <summary>
      /// Crea un objeto de Docente a partir de los datos del último DataRow del DataSet.
      /// </summary>
      /// <param name="ds">El DataSet que contiene la información de Docente</param>
      /// <returns>Un objeto de Docente creado a partir de los datos</returns>
      public Docente LastDataRowToDocente(DataSet ds) {
         if (!ds.Tables.Contains("Docente"))
            throw new Exception("LastDataRowToDocente: DataSet no tiene la tabla Docente");
         int index = ds.Tables["Docente"].Rows.Count;
         if (index < 1)
            throw new Exception("LastDataRowToDocente: El DataSet no tiene filas");
         return this.DataRowToDocente(ds.Tables["Docente"].Rows[index - 1]);
      }
      /// <summary>
      /// Crea un objeto de Docente a partir de los datos de un DataRow.
      /// </summary>
      /// <param name="row">El DataRow que contiene la información de Docente</param>
      /// <returns>Un objeto de Docente creado a partir de los datos</returns>
      public Docente DataRowToDocente(DataRow row){
         Docente docente = new Docente();         
         if (row.IsNull("DocenteID"))
            docente.DocenteID = null;
         else
            docente.DocenteID = (int)Convert.ChangeType(row["DocenteID"], typeof(int));
         if (row.IsNull("Curp"))
             docente.Curp = null;
         else
             docente.Curp = (String)Convert.ChangeType(row["Curp"], typeof(String));
         if (row.IsNull("Nombre"))
            docente.Nombre = null;
         else
            docente.Nombre = (String)Convert.ChangeType(row["Nombre"], typeof(String));
         if (row.IsNull("PrimerApellido"))
            docente.PrimerApellido = null;
         else
            docente.PrimerApellido = (String)Convert.ChangeType(row["PrimerApellido"], typeof(String));
         if (row.IsNull("SegundoApellido"))
            docente.SegundoApellido = null;
         else
            docente.SegundoApellido = (String)Convert.ChangeType(row["SegundoApellido"], typeof(String));
         if (row.IsNull("Estatus"))
            docente.Estatus = null;
         else
            docente.Estatus = (bool)Convert.ChangeType(row["Estatus"], typeof(bool));
         if (row.IsNull("FechaRegistro"))
            docente.FechaRegistro = null;
         else
            docente.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
         if (row.IsNull("FechaNacimiento"))
            docente.FechaNacimiento = null;
         else
            docente.FechaNacimiento = (DateTime)Convert.ChangeType(row["FechaNacimiento"], typeof(DateTime));
         if (row.IsNull("Sexo"))
             docente.Sexo = null;
         else
             docente.Sexo = (bool)Convert.ChangeType(row["Sexo"], typeof(bool));
         if (row.IsNull("Correo"))
             docente.Correo = null;
         else
             docente.Correo = (String)Convert.ChangeType(row["Correo"], typeof(String));
         if (row.IsNull("Clave"))
             docente.Clave = null;
         else
             docente.Clave = (String)Convert.ChangeType(row["Clave"], typeof(String));
         if (row.IsNull("EstatusIdentificacion"))
             docente.EstatusIdentificacion = null;
         else
             docente.EstatusIdentificacion = (bool)Convert.ChangeType(row["EstatusIdentificacion"], typeof(bool));

         if (row.IsNull("Cedula"))
             docente.Cedula = null;
         else
             docente.Cedula = (string)Convert.ChangeType(row["Cedula"], typeof(string));
         if (row.IsNull("NivelEstudio"))
             docente.NivelEstudio = null;
         else
             docente.NivelEstudio = (string)Convert.ChangeType(row["NivelEstudio"], typeof(string));

         if (row.IsNull("Titulo"))
             docente.Titulo = null;
         else
             docente.Titulo = (string)Convert.ChangeType(row["Titulo"], typeof(string));

         if (row.IsNull("Especialidades"))
             docente.Especialidades = null;
         else
             docente.Especialidades = (string)Convert.ChangeType(row["Especialidades"], typeof(string));

         if (row.IsNull("Experiencia"))
             docente.Experiencia = null;
         else
             docente.Experiencia = (string)Convert.ChangeType(row["Experiencia"], typeof(string));

         if (row.IsNull("Cursos"))
             docente.Cursos = null;
         else
             docente.Cursos = (string)Convert.ChangeType(row["Cursos"], typeof(string));

         if (row.IsNull("UsuarioSkype"))
             docente.UsuarioSkype = null;
         else
             docente.UsuarioSkype = (string)Convert.ChangeType(row["UsuarioSkype"], typeof(string));

         if (row.IsNull("EsPremium"))
             docente.EsPremium = null;
         else
             docente.EsPremium = (bool)Convert.ChangeType(row["EsPremium"], typeof(bool));

         return docente;
      }

      /// <summary>
      /// Identifica si existe un docente con identificado con la misma CURP
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="docente">El Docente </param>
      /// <returns></returns>
      private bool NoRepetido(IDataContext dctx,Docente docente)
      {
          DataSet ds = Retrieve(dctx, new Docente {Curp = docente.Curp});
          return ds.Tables[0].Rows.Count <= 0;
      }

      public Docente GetDocenteMenosCarga(IDataContext dctx, Docente docente = null, Usuario usuario = null)
      {
          AlumnosAsignadosDocentePortal da = new AlumnosAsignadosDocentePortal();
          DataSet dsDocentes = da.Action(dctx, docente, usuario);
          return LastDataRowToDocente(dsDocentes);
      }
   } 
}
