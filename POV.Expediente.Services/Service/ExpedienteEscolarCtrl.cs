using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.Expediente.BO;
using POV.Expediente.DAO;
using POV.Modelo.BO;
using POV.Prueba.Diagnostico.Dinamica.BO;


namespace POV.Expediente.Service { 
   /// <summary>
   /// Controlador del objeto ExpedienteEscolar
   /// </summary>
   public class ExpedienteEscolarCtrl { 
      /// <summary>
      /// Consulta registros de ExpedienteEscolarRetHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="expedienteEscolarRetHlp">ExpedienteEscolarRetHlp que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de ExpedienteEscolarRetHlp generada por la consulta</returns>
      public DataSet Retrieve(IDataContext dctx, ExpedienteEscolar expedienteEscolar){
         ExpedienteEscolarRetHlp da = new ExpedienteEscolarRetHlp();
         DataSet ds = da.Action(dctx, expedienteEscolar);
         return ds;
      }
      /// <summary>
      /// Crea un registro de ExpedienteEscolarInsHlp en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="expedienteEscolarInsHlp">ExpedienteEscolarInsHlp que desea crear</param>
      public void Insert(IDataContext dctx, ExpedienteEscolar  expedienteEscolar){
         ExpedienteEscolarInsHlp da = new ExpedienteEscolarInsHlp();
         da.Action(dctx,  expedienteEscolar);
      }

       /// <summary>
       /// Inserta un expediente de alumno, si el expediente existe y esta desactivado lo activa.
       /// </summary>
       /// <param name="dctx"></param>
       /// <param name="alumno"></param>
      public void InsertExpedienteAlumno(IDataContext dctx, Alumno alumno)
      {
          if (alumno.AlumnoID == null)
              throw new Exception("El identificador del alumno es requerido.");
          object myFirm = new object();
          dctx.OpenConnection(myFirm);
          dctx.BeginTransaction(myFirm);
          try
          {
              //Verificamos que exista el expediente del alumno

              ExpedienteEscolar expediente = new ExpedienteEscolar { Alumno = alumno };

              #region Expediente escolar
              DataSet dsExpediente = Retrieve(dctx, expediente);
              if (dsExpediente.Tables[0].Rows.Count > 0)
              {
                  expediente = LastDataRowToExpedienteEscolar(dsExpediente);

                  if (!expediente.Activo.Value)
                  {
                      expediente.Activo = true;
                      Update(dctx, expediente, expediente);
                  }


              }
              else
              {
                  expediente.Activo = true;
                  expediente.FechaRegistro = DateTime.Now;

                  Insert(dctx, expediente);

              }
              #endregion
              dctx.CommitTransaction(myFirm);
          }
          catch (Exception ex)
          {
              dctx.RollbackTransaction(myFirm);
              dctx.CloseConnection(myFirm);
              throw ex;
          }
          finally
          {
              if (dctx.ConnectionState == ConnectionState.Open)
                  dctx.CloseConnection(myFirm);
          }
      }

       /// <summary>
       /// Inserta un detalle de resultado de prueba de un expediente de alumno
       /// </summary>
       /// <param name="dctx"></param>
       /// <param name="alumno">alumno propietario del expediente</param>
       /// <param name="grupoCicloEscolar">grupo ciclo excolar del detalle</param>
       /// <param name="escuela"> escuela donde ocurrio el detalle</param>
       /// <param name="resultado">resultado de la prueba</param>
      public void InsertDetalleCicloEscolar(IDataContext dctx, Alumno alumno, GrupoCicloEscolar grupoCicloEscolar, Escuela escuela, AResultadoPrueba resultado)
      {
          if (alumno.AlumnoID == null)
              throw new Exception("El identificador del alumno es requerido.");
          if (grupoCicloEscolar.GrupoCicloEscolarID == null)
              throw new Exception("El identificador del grupo ciclo escolar es requerido");
          if (escuela.EscuelaID == null)
              throw new Exception("El identificador de la escuela es requerido");

          object myFirm = new object();
          dctx.OpenConnection(myFirm);
          dctx.BeginTransaction(myFirm);
          try
          {
              //Verificamos que exista el expediente del alumno

              ExpedienteEscolar expediente = new ExpedienteEscolar { Alumno = alumno };

              #region Expediente escolar
              DataSet dsExpediente = Retrieve(dctx, expediente);
              if (dsExpediente.Tables[0].Rows.Count > 0)
              {
                  expediente = LastDataRowToExpedienteEscolar(dsExpediente);

                  if (!expediente.Activo.Value)
                  {
                      expediente.Activo = true;
                      Update(dctx, expediente, expediente);
                  }


              }
              else
              {
                  expediente.Activo = true;
                  expediente.FechaRegistro = DateTime.Now;

                  Insert(dctx, expediente);

                  expediente = LastDataRowToExpedienteEscolar(Retrieve(dctx, expediente));
              }
              #endregion

              //verificamos que exista el detalle
              DetalleCicloEscolar detalle = new DetalleCicloEscolar { Escuela = escuela, 
                  GrupoCicloEscolar = grupoCicloEscolar};
              #region detalle ciclo escolar
              DataSet dsDetalle = Retrieve(dctx, detalle, expediente);
              if (dsDetalle.Tables[0].Rows.Count > 0)
              {
                  detalle = LastDataRowToDetalleCicloEscolar(dsDetalle);

                  if (!detalle.Activo.Value)
                  {
                      detalle.Activo = true;
                      Update(dctx, detalle, detalle, expediente);
                  }
              }
              else
              {
                  detalle.Activo = true;
                  detalle.FechaRegistro = DateTime.Now;
                  Insert(dctx, detalle, expediente);
                  detalle = LastDataRowToDetalleCicloEscolar(Retrieve(dctx,detalle, expediente));
              }
              #endregion
              
              Insert(dctx, resultado, detalle);

              dctx.CommitTransaction(myFirm);
          }
          catch (Exception ex)
          {
              dctx.RollbackTransaction(myFirm);
              dctx.CloseConnection(myFirm);
              throw ex;
          }
          finally
          {
              if (dctx.ConnectionState == ConnectionState.Open)
                  dctx.CloseConnection(myFirm);
          }
      }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="dctx"></param>
       /// <param name="detalleCicloEscolar"></param>
       /// <param name="resultadoPrueba"></param>
      private void ValidarPruebaRepetidaDetalleCicloEscolar(IDataContext dctx, DetalleCicloEscolar detalleCicloEscolar, AResultadoPrueba resultadoPrueba)
      {
          
          resultadoPrueba.FechaRegistro = null;
          
          DataSet ds = Retrieve(dctx, resultadoPrueba, detalleCicloEscolar);

          if (ds.Tables[0].Rows.Count > 0)
          {
              throw new Exception("ExpedienteEscolarCtrl: Ya existe un resultado para la prueba que se desea insertar en el ciclo escolar ");
          }
      }

      /// <summary>
      /// Actualiza de manera optimista un registro de ExpedienteEscolarUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="expedienteEscolarUpdHlp">ExpedienteEscolarUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">ExpedienteEscolarUpdHlp que tiene los datos anteriores</param>
      public void Update(IDataContext dctx, ExpedienteEscolar  expedienteEscolar, ExpedienteEscolar previous){
         ExpedienteEscolarUpdHlp da = new ExpedienteEscolarUpdHlp();
         da.Action(dctx,  expedienteEscolar, previous);
      }
      /// <summary>
      /// Consulta registros de AResultadoPruebaRetHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="aResultadoPruebaRetHlp">AResultadoPruebaRetHlp que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de AResultadoPruebaRetHlp generada por la consulta</returns>
      public DataSet Retrieve(IDataContext dctx, AResultadoPrueba resultadoPrueba, DetalleCicloEscolar detalleCicloEscolar){
         ResultadoPruebaRetHlp da = new ResultadoPruebaRetHlp();
         DataSet ds = da.Action(dctx, resultadoPrueba, detalleCicloEscolar);
         return ds;
      }
       /// <summary>
       /// Consulta registros de ResultadoPrueba
       /// </summary>
       /// <param name="dctx"></param>
       /// <param name="detalleCicloEscolar"></param>
       /// <returns></returns>
      public DataSet Retrieve(IDataContext dctx, DetalleCicloEscolar detalleCicloEscolar)
      {
          ResultadoPruebaRetHlp da = new ResultadoPruebaRetHlp();
          DataSet ds = da.Action(dctx,  detalleCicloEscolar);
          return ds;
      }
      /// <summary>
      /// Crea un registro de AResultadoPruebaInsHlp en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="aResultadoPruebaInsHlp">AResultadoPruebaInsHlp que desea crear</param>
      public void Insert(IDataContext dctx, AResultadoPrueba  resultadoPrueba, DetalleCicloEscolar detalleCicloEscolar){
         ResultadoPruebaInsHlp da = new ResultadoPruebaInsHlp();
         da.Action(dctx,  resultadoPrueba, detalleCicloEscolar);
      }
      /// <summary>
      /// Consulta registros de DetalleCicloEscolarRetHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="detalleCicloEscolarRetHlp">DetalleCicloEscolarRetHlp que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de DetalleCicloEscolarRetHlp generada por la consulta</returns>
      public DataSet Retrieve(IDataContext dctx, DetalleCicloEscolar detalleCicloEscolar, ExpedienteEscolar  expedienteEscolar){
         DetalleCicloEscolarRetHlp da = new DetalleCicloEscolarRetHlp();
         DataSet ds = da.Action(dctx, detalleCicloEscolar,  expedienteEscolar);
         return ds;
      }
      /// <summary>
      /// Crea un registro de DetalleCicloEscolarInsHlp en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="detalleCicloEscolarInsHlp">DetalleCicloEscolarInsHlp que desea crear</param>
      public void Insert(IDataContext dctx, DetalleCicloEscolar  detalleCicloEscolar, ExpedienteEscolar  expedienteEscolar){
         DetalleCicloEscolarInsHlp da = new DetalleCicloEscolarInsHlp();
         da.Action(dctx,  detalleCicloEscolar,  expedienteEscolar);
      }
      /// <summary>
      /// Actualiza de manera optimista un registro de DetalleCicloEscolarUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="detalleCicloEscolarUpdHlp">DetalleCicloEscolarUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">DetalleCicloEscolarUpdHlp que tiene los datos anteriores</param>
      public void Update(IDataContext dctx, DetalleCicloEscolar detalleCicloEscolar, DetalleCicloEscolar previous, ExpedienteEscolar expedienteEscolar)
      {
         DetalleCicloEscolarUpdHlp da = new DetalleCicloEscolarUpdHlp();
         da.Action(dctx,  detalleCicloEscolar, previous,  expedienteEscolar);
      }

      public List<InteresAspirante> RetrieveInteresesAspirante(IDataContext dctx, Alumno alumno, PruebaDinamica prueba)
      {
          List<InteresAspirante> lstIntAspirante = new List<InteresAspirante>();
          InteresesAspiranteRetHlp da = new InteresesAspiranteRetHlp();
          var ds = da.Action(dctx, alumno, prueba);

          foreach (DataRow row in ds.Tables[0].Rows)
          {
              lstIntAspirante.Add(DataRowToInteresAspirante(row));
          }

          return lstIntAspirante;
      }

      public DataSet DsRetrieveInteresesAspirante(IDataContext dctx, Alumno alumno, PruebaDinamica prueba)
      {
          InteresesAspiranteRetHlp da = new InteresesAspiranteRetHlp();
          var ds = da.Action(dctx, alumno, prueba);

          return ds;
      }

      /// <summary>
      /// Crea un objeto de DetalleCicloEscolar a partir de los datos del último DataRow del DataSet.
      /// </summary>
      /// <param name="ds">El DataSet que contiene la información de DetalleCicloEscolar</param>
      /// <returns>Un objeto de DetalleCicloEscolar creado a partir de los datos</returns>
      public DetalleCicloEscolar LastDataRowToDetalleCicloEscolar(DataSet ds) {
         if (!ds.Tables.Contains("DetalleCicloEscolar"))
            throw new Exception("LastDataRowToDetalleCicloEscolar: DataSet no tiene la tabla DetalleCicloEscolar");
         int index = ds.Tables["DetalleCicloEscolar"].Rows.Count;
         if (index < 1)
            throw new Exception("LastDataRowToDetalleCicloEscolar: El DataSet no tiene filas");
         return this.DataRowToDetalleCicloEscolar(ds.Tables["DetalleCicloEscolar"].Rows[index - 1]);
      }
      /// <summary>
      /// Crea un objeto de DetalleCicloEscolar a partir de los datos de un DataRow.
      /// </summary>
      /// <param name="row">El DataRow que contiene la información de DetalleCicloEscolar</param>
      /// <returns>Un objeto de DetalleCicloEscolar creado a partir de los datos</returns>
      public DetalleCicloEscolar DataRowToDetalleCicloEscolar(DataRow row){
         DetalleCicloEscolar detalleCicloEscolar = new DetalleCicloEscolar();
         detalleCicloEscolar.GrupoCicloEscolar = new GrupoCicloEscolar();
         detalleCicloEscolar.Escuela = new Escuela();
         if (row.IsNull("DetalleCicloEscolarID"))
            detalleCicloEscolar.DetalleCicloEscolarID = null;
         else
            detalleCicloEscolar.DetalleCicloEscolarID = (int)Convert.ChangeType(row["DetalleCicloEscolarID"], typeof(int));
         if (row.IsNull("GrupoCicloEscolarID"))
            detalleCicloEscolar.GrupoCicloEscolar.GrupoCicloEscolarID = null;
         else
            detalleCicloEscolar.GrupoCicloEscolar.GrupoCicloEscolarID = (Guid)Convert.ChangeType(row["GrupoCicloEscolarID"], typeof(Guid));
         if (row.IsNull("EscuelaID"))
            detalleCicloEscolar.Escuela.EscuelaID = null;
         else
            detalleCicloEscolar.Escuela.EscuelaID = (int)Convert.ChangeType(row["EscuelaID"], typeof(int));
         if (row.IsNull("Activo"))
            detalleCicloEscolar.Activo = null;
         else
            detalleCicloEscolar.Activo = (bool)Convert.ChangeType(row["Activo"], typeof(bool));
         if (row.IsNull("FechaRegistro"))
             detalleCicloEscolar.FechaRegistro = null;
         else
             detalleCicloEscolar.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
         return detalleCicloEscolar;
      }
      /// <summary>
      /// Crea un objeto de ExpedienteEscolar a partir de los datos del último DataRow del DataSet.
      /// </summary>
      /// <param name="ds">El DataSet que contiene la información de ExpedienteEscolar</param>
      /// <returns>Un objeto de ExpedienteEscolar creado a partir de los datos</returns>
      public ExpedienteEscolar LastDataRowToExpedienteEscolar(DataSet ds) {
         if (!ds.Tables.Contains("ExpedienteEscolar"))
            throw new Exception("LastDataRowToExpedienteEscolar: DataSet no tiene la tabla ExpedienteEscolar");
         int index = ds.Tables["ExpedienteEscolar"].Rows.Count;
         if (index < 1)
            throw new Exception("LastDataRowToExpedienteEscolar: El DataSet no tiene filas");
         return this.DataRowToExpedienteEscolar(ds.Tables["ExpedienteEscolar"].Rows[index - 1]);
      }
      /// <summary>
      /// Crea un objeto de ExpedienteEscolar a partir de los datos de un DataRow.
      /// </summary>
      /// <param name="row">El DataRow que contiene la información de ExpedienteEscolar</param>
      /// <returns>Un objeto de ExpedienteEscolar creado a partir de los datos</returns>
      public ExpedienteEscolar DataRowToExpedienteEscolar(DataRow row)
      {
          ExpedienteEscolar expedienteEscolar = new ExpedienteEscolar();
          expedienteEscolar.Alumno = new Alumno();
          if (row.IsNull("ExpedienteEscolarID"))
              expedienteEscolar.ExpedienteEscolarID = null;
          else
              expedienteEscolar.ExpedienteEscolarID = (int)Convert.ChangeType(row["ExpedienteEscolarID"], typeof(int));
          if (row.IsNull("AlumnoID"))
              expedienteEscolar.Alumno.AlumnoID = null;
          else
              expedienteEscolar.Alumno.AlumnoID = (int)Convert.ChangeType(row["AlumnoID"], typeof(int));
          if (row.IsNull("Activo"))
              expedienteEscolar.Activo = null;
          else
              expedienteEscolar.Activo = (bool)Convert.ChangeType(row["Activo"], typeof(bool));
          if (row.IsNull("FechaRegistro"))
              expedienteEscolar.FechaRegistro = null;
          else
              expedienteEscolar.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
          if (row.IsNull("Apuntes"))
              expedienteEscolar.Apuntes = null;
          else
              expedienteEscolar.Apuntes = (String)Convert.ChangeType(row["Apuntes"], typeof(String));
          return expedienteEscolar;
      }
      /// <summary>
      /// Crea un objeto de InteresAspirante a partir de los datos de un DataRow.
      /// </summary>
      /// <param name="row">El DataRow que contiene la información de ExpedienteEscolar</param>
      /// <returns>Un objeto de InteresAspirante creado a partir de los datos</returns>
      public InteresAspirante DataRowToInteresAspirante(DataRow row)
      {
          InteresAspirante obj = new InteresAspirante();
          obj.clasificador = new Clasificador();

          if (row.IsNull("InteresID"))
              obj.InteresID = null;
          else
              obj.InteresID = (int)Convert.ChangeType(row["InteresID"], typeof(int));
          
          if (row.IsNull("NombreInteres"))
              obj.NombreInteres = null;
          else
              obj.NombreInteres = (String)Convert.ChangeType(row["NombreInteres"], typeof(String));
          
          if (row.IsNull("Descripcion"))
              obj.Descripcion = null;
          else
              obj.Descripcion = (string)Convert.ChangeType(row["Descripcion"], typeof(string));
          
          if (row.IsNull("ClasificadorID"))
              obj.clasificador.ClasificadorID = null;
          else
              obj.clasificador.ClasificadorID = (Int32)Convert.ChangeType(row["ClasificadorID"], typeof(Int32));
          
          if (row.IsNull("Nombre"))
              obj.clasificador.Nombre = null;
          else
              obj.clasificador.Nombre = (String)Convert.ChangeType(row["Nombre"], typeof(String));
          
          if (row.IsNull("Descripcion1"))
              obj.clasificador.Descripcion = null;
          else
              obj.clasificador.Descripcion = (String)Convert.ChangeType(row["Descripcion1"], typeof(String));
          return obj;
      }
   } 
}
