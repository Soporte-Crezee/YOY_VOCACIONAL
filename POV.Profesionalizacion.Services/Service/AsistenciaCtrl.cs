using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.ContenidosDigital.BO;
using POV.ContenidosDigital.Service;
using POV.Profesionalizacion.BO;
using POV.Profesionalizacion.DA;
using POV.Profesionalizacion.DAO;

namespace POV.Profesionalizacion.Service { 
   /// <summary>
   /// Controlador del objeto Asistencia
   /// Catalogo de Asistencia 
   /// </summary>
   public class AsistenciaCtrl
   {


      #region Retrieve Asistencias

      /// <summary>
      /// Consulta registros de AsistenciaRetHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
       /// <param name="agrupador">AAgrupadorContenidoDigital que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de AsistenciaRetHlp generada por la consulta</returns>
      public DataSet Retrieve(IDataContext dctx, AAgrupadorContenidoDigital agrupador){
         AsistenciaRetHlp da = new AsistenciaRetHlp();
         DataSet ds = da.Action(dctx, agrupador);
         return ds;
      }

      /// <summary>
      /// Consulta registros de AsistenciaRetHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="agrupadorPadreId">AgrupadorPadreID que provee el criterio de selección</param>
      /// <returns>El DataSet que contiene la información de AsistenciaRetHlp generada por la consulta</returns>
      public DataSet Retrieve(IDataContext dctx, long? agrupadorPadreId)
      {
          AsistenciaRetHlp da = new AsistenciaRetHlp();
          DataSet ds = da.Action(dctx, agrupadorPadreId);
          return ds;
      }

       public DataSet Retrieve(IDataContext dctx, AAgrupadorContenidoDigital aAgrupadorContenidoDigital,
                               long? contenidoDigitalID)
       {
           AsistenciaContenidoDetalleDARetHlp da = new AsistenciaContenidoDetalleDARetHlp();
           DataSet ds = da.Action(dctx, aAgrupadorContenidoDigital, contenidoDigitalID);
           return ds;
       }
       /// <summary>
       /// Obtiene los contenidos digitales asociados a una asistencia.
       /// </summary>
       /// <param name="dctx">DataContext que proveerá el acceso a la base de datos.</param>
       /// <param name="contenidoDigital">Contenido digital del que se desea saber la asistencia.</param>
       /// <returns>Regresa un dataset con los resultados.</returns>
       public DataSet RetrieveAsistencia(IDataContext dctx, ContenidoDigital contenidoDigital)
       {
           AsistenciaDARetHlp da = new AsistenciaDARetHlp();
           DataSet ds = da.Action(dctx, contenidoDigital);
           return ds;
       }

     /// <summary>
      /// Devuelve el árbol completo de un objeto AAgrupadorContenidoDigital
     /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="agrupador">AgrupadorContenidoDigital que provee el criterio de selección</param>
      /// <returns>AAgrupadorContenidoDigital que contiene la información generada por la consulta</returns>
      public AAgrupadorContenidoDigital RetrieveSimple(IDataContext dctx, AAgrupadorContenidoDigital agrupador)
      {
          if(agrupador==null) throw new ArgumentNullException("asistencia","AsitenciaCtrl:Asistencia no puede ser nulo");

          AAgrupadorContenidoDigital agrupadorContenido = null;
          DataSet ds = Retrieve(dctx, agrupador);

          if(ds.Tables[0].Rows.Count>1)
              throw new Exception("AsistenciaCtrl:La consulta devolvió más de un registro para las condiciones proporcionadas");

          if (ds.Tables[0].Rows.Count > 0)
          {
              agrupadorContenido = this.LastDataRowToAgrupadorContenido(ds);

              if (agrupadorContenido is Asistencia)
              {
                  //consultar tema de asistencia
              }
              if (agrupadorContenido is Asistencia || agrupadorContenido is AgrupadorCompuesto)
              {
                  //Consultar hijos
                  ds = Retrieve(dctx, agrupadorContenido.AgrupadorContenidoDigitalID);
                  int index = ds.Tables[0].Rows.Count;
                  if (index > 0)
                  {
                      foreach (DataRow row in ds.Tables[0].Rows) {
                          AAgrupadorContenidoDigital aAgrupador = DataRowToAgrupadorContenidoDigital(row);
                          aAgrupador = RetrieveSimple(dctx, aAgrupador);
                          agrupadorContenido.Agregar(aAgrupador);
                      }
                  }
              }
              


          }
          return agrupadorContenido;
      }

      /// <summary>
      /// Devuelve el árbol completo de un objeto concreto Asistencia con sus contenidos Digitales
      /// </summary>
      public AAgrupadorContenidoDigital RetrieveComplete(IDataContext dctx, AAgrupadorContenidoDigital agrupador)
      {
          if (agrupador == null) throw new ArgumentNullException("asistencia", "AsitenciaCtrl:Asistencia no puede ser nulo");

          AAgrupadorContenidoDigital agrupadorcontenido = RetrieveSimple(dctx, agrupador);
          if (agrupadorcontenido is Asistencia)
          {
              TemaAsistenciaCtrl temaAsistenciaCtrl =  new TemaAsistenciaCtrl();
              DataSet ds = temaAsistenciaCtrl.Retrieve(dctx, (agrupadorcontenido as Asistencia).TemaAsistencia);
              if (ds.Tables[0].Rows.Count > 0)
              {
                  (agrupadorcontenido as Asistencia).TemaAsistencia = temaAsistenciaCtrl.LastDataRowToTemaAsistencia(ds);
              }
              
          }

          AddContenidoDigital(dctx,agrupadorcontenido);

          return agrupadorcontenido;
      }


       #endregion

      #region Insert Asistencias

       /// <summary>
       ///     Crea un registro de Asistencia en la base de datos
       /// </summary>
       /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
       /// <param name="asistencia">Asistencia que desea crear</param>
       public void Insert(IDataContext dctx, Asistencia asistencia)
       {
           var da = new AsistenciaInsHlp();
           da.Action(dctx, asistencia);
       }

       /// <summary>
       ///     Crea un registro de Asistencia junto con sus clasificadores en la base de datos
       /// </summary>
       /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
       /// <param name="asistencia">Asistencia que desea crear</param>
       public void InsertComplete(IDataContext dctx, Asistencia asistencia)
       {
           if (asistencia == null) throw new ArgumentNullException("asistencia");

           #region *** Conexion a DB****
           object con = new object();
           try
           {
               dctx.OpenConnection(con);
           }
           catch (Exception ex)
           {
               throw new Exception("Inconsistencias al conectarse a la base de datos.");
           }
           #endregion

           object firma = new object();
           try
           {
             
               dctx.BeginTransaction(firma);
               Insert(dctx, asistencia);
               DataSet ds = Retrieve(dctx, asistencia);
               if (ds.Tables[0].Rows.Count <= 0) throw new Exception("AsistenciaCtrl:Ocurrió un error al registrar la asistencia");
               AAgrupadorContenidoDigital registroAsistencia = (Asistencia) LastDataRowToAgrupadorContenido(ds);

                   if (asistencia.AgrupadoresContenido != null)
                       foreach (AAgrupadorContenidoDigital aAgrupador in asistencia.AgrupadoresContenido)
                       {
                           InsertAgrupadorContenido(dctx, aAgrupador, registroAsistencia.AgrupadorContenidoDigitalID);
                       }
               dctx.CommitTransaction(firma);
           }
           catch (Exception ex)
           {
               dctx.RollbackTransaction(firma);

               if (dctx.ConnectionState == ConnectionState.Open)
                   dctx.CloseConnection(con);
               throw;
           }
           finally
           {
               if (dctx.ConnectionState == ConnectionState.Open)
                   dctx.CloseConnection(con);
           }
       }

       /// <summary>
       /// Crea un registro de AAgrupadorContenidoDigital para asistencias en la base de datos
       /// </summary>
       /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
       /// <param name="agrupador">AAgrupadorContenidoDigital que desea crear</param>
       /// <param name="agrupadorPadreID">AgrupadorContenidoPadre</param>
       private void InsertAgrupadorContenido(IDataContext dctx, AAgrupadorContenidoDigital agrupador, long? agrupadorPadreID)
       {
           if (agrupador is Asistencia) throw new Exception("AsistenciaCtrl:Ocurrió un error al registrar la Asistencia como AgrupadorContendio. Operación no soportada");

           AsistenciaInsHlp da = new AsistenciaInsHlp();
           da.Action(dctx, agrupador, agrupadorPadreID);
           
           if (agrupador is AgrupadorCompuesto)
           {
               throw new Exception("AsistenciaCtrl:Operación no soportada");
           }

       }

       #endregion

      #region Update Asistencia

       /// <summary>
       /// Actualiza un registro de Asistencia en la base de datos
       /// </summary>
       /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
       /// <param name="asistencia">Asistencia que se desea actualizar</param>
       /// <param name="previous">Asistencia anterior</param>
       public void Update(IDataContext dctx, Asistencia asistencia, Asistencia previous)
       {
           AsistenciaUpdHlp da = new AsistenciaUpdHlp();
           da.Action(dctx,asistencia,previous);
       }

       /// <summary>
       ///     Actualiza un registro de AAgrupadorContenidoDigital de una asistencia
       /// </summary>
       /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
       /// <param name="aAgrupador">AAgrupadorContenidoDigital con los datos que desea actualizar</param>
       /// <param name="previo">AAgrupadorContenidoDigital previo</param>
       /// <param name="agrupadorPadreID">AgrupadorPadreID</param>
       private void UpdateAgrupadorContenido(IDataContext dctx, AAgrupadorContenidoDigital aAgrupador,AAgrupadorContenidoDigital previo, long? agrupadorPadreID)
       {
           AsistenciaUpdHlp da =new AsistenciaUpdHlp();
           da.Action(dctx,aAgrupador,previo,agrupadorPadreID);

           if (aAgrupador is AgrupadorCompuesto)
           {
               throw new Exception("AsistenciaCtrl:Operación no soportada");
           }
       }

       /// <summary>
       /// Actualiza una Asistencia junto con sus clasificadores en la base de datos
       /// </summary>
       /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
       /// <param name="asistencia">Asistencia que se desea actualizar</param>
       /// <param name="previous">Asistencia anterior</param>
       public void UpdateComplete(IDataContext dctx, Asistencia asistencia, Asistencia previous)
       {
           if (asistencia == null || asistencia.AgrupadorContenidoDigitalID==null) throw new ArgumentNullException("asistencia","AsistenciaCtrl: AgrupadorContenidoDigitalID no puede ser nulo");
           var firm = new object();
           try
           {
               dctx.OpenConnection(firm);
               dctx.BeginTransaction(firm);

               Update(dctx,asistencia,previous);

               if (asistencia.AgrupadoresContenido != null)
               {
                   foreach (AAgrupadorContenidoDigital aAgrupador in asistencia.AgrupadoresContenido)
                   {
                       //Registrar agrupador
                       if (aAgrupador.AgrupadorContenidoDigitalID == null)
                       {
                           InsertAgrupadorContenido(dctx,aAgrupador,asistencia.AgrupadorContenidoDigitalID);
                       }
                       else
                       {
                           //Editar agrupador
                           AAgrupadorContenidoDigital aAgrupadorPrevio = previous.AgrupadoresContenido.FirstOrDefault(x => x.AgrupadorContenidoDigitalID == aAgrupador.AgrupadorContenidoDigitalID);
                           if (aAgrupadorPrevio != null)
                           {
                               UpdateAgrupadorContenido(dctx, aAgrupador, aAgrupadorPrevio, asistencia.AgrupadorContenidoDigitalID);
                           }
                       }
                   }
                   foreach (AAgrupadorContenidoDigital aAgrupador in previous.AgrupadoresContenido)
                   {
                       AAgrupadorContenidoDigital aAgrupadorPrevio = asistencia.AgrupadoresContenido.FirstOrDefault(x => x.AgrupadorContenidoDigitalID == aAgrupador.AgrupadorContenidoDigitalID);
                       if (aAgrupadorPrevio == null)
                       {
                           //Eliminar agrupador
                           DeleteAgrupadorContenido(dctx,aAgrupador,asistencia.AgrupadorContenidoDigitalID);
                       }
                   }
               }

               dctx.CommitTransaction(firm);
           }
           catch (Exception ex)
           {
               dctx.RollbackTransaction(firm);
               if (dctx.ConnectionState == ConnectionState.Open)
                   dctx.CloseConnection(firm);
               throw;
           }
           finally
           {
               if(dctx.ConnectionState== ConnectionState.Open)
                   dctx.CloseConnection(firm);
           }

       }
       #endregion

      #region Delete Asistencia

       /// <summary>
       /// Elimina un registro de Asistencia (baja lógica)
       /// </summary>
       /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
       /// <param name="asistencia">Asistencia que se desea eliminar</param>
       public void Delete(IDataContext dctx, Asistencia asistencia)
       {
           AsistenciaDelHlp da = new AsistenciaDelHlp();
           da.Action(dctx,asistencia);
       }

       /// <summary>
       /// Elimina un AAgrupadorContenidoDigital para una Asistencia (baja lógica)
       /// </summary>
       /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
       /// <param name="aAgrupador">AAgrupadorContenidoDigital que se desea eliminar</param>
       /// <param name="agrupadorPadreID">AgrupadorContenidoPadre</param>
       private void DeleteAgrupadorContenido(IDataContext dctx, AAgrupadorContenidoDigital aAgrupador,long? agrupadorPadreID)
       {
           AsistenciaDelHlp da = new AsistenciaDelHlp();
           da.Action(dctx,aAgrupador,agrupadorPadreID);

           //Eliminar el contenido digital asignado al Agrupador
           List<ContenidoDigital> lscontenido = RetrieveListContenidoDigital(dctx, aAgrupador.AgrupadorContenidoDigitalID);
           if (lscontenido != null)
           {
               foreach (ContenidoDigital contenidoDigital in lscontenido)
               {
                   DeleteContenidoDigital(dctx, aAgrupador, contenidoDigital.ContenidoDigitalID);
               }
           }

           if (aAgrupador is AgrupadorCompuesto)
           {
               throw new Exception("Operación no soportada");
           }
       }

       /// <summary>
       /// Eliminar un registro de Asistencia (baja lógica)
       /// </summary>
       /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
       /// <param name="asistencia">Asistencia que se desea eliminar</param>
       public void DeleteComplete(IDataContext dctx, Asistencia asistencia)
       {
           if(asistencia==null || asistencia.AgrupadorContenidoDigitalID==null) throw new ArgumentNullException("asistencia","AsistenciaCtrl:Asistencia no puede ser nulo");
          
           object firm = new object();
           try
           {
               dctx.OpenConnection(firm);
               dctx.BeginTransaction(firm);
               asistencia = (Asistencia)RetrieveComplete(dctx, asistencia);

               Delete(dctx, asistencia);
               foreach (AAgrupadorContenidoDigital aAgrupador in asistencia.AgrupadoresContenido)
               {
                   DeleteAgrupadorContenido(dctx, aAgrupador, asistencia.AgrupadorContenidoDigitalID);
               }

               //Eliminar el contenido digital asignado a la asistencia
               List<ContenidoDigital> lscontenido = RetrieveListContenidoDigital(dctx, asistencia.AgrupadorContenidoDigitalID);
               if (lscontenido != null)
               {
                   foreach (ContenidoDigital contenidoDigital in lscontenido)
                   {
                       DeleteContenidoDigital(dctx,asistencia,contenidoDigital.ContenidoDigitalID);
                   }
               }

               dctx.CommitTransaction(firm);
           }
           catch (Exception ex)
           {
               dctx.RollbackTransaction(firm);
               if (dctx.ConnectionState == ConnectionState.Open)
                   dctx.CloseConnection(firm);
               throw;
           }
           finally
           {
               if(dctx.ConnectionState== ConnectionState.Open)
                   dctx.CloseConnection(firm);
           }
       }

       #endregion

      #region Contenidos Digitales Asistencia

      /// <summary>
      /// Consulta el Agrega Contenido digital para un AAgrupadorContenidoDigital proporcionado 
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="aAgrupadorContenido">AAgrupadorContenidoDigital que provee el criterio de consulta</param>
      private void AddContenidoDigital(IDataContext dctx, AAgrupadorContenidoDigital aAgrupadorContenido)
      {
          if (aAgrupadorContenido == null) throw new ArgumentNullException("aAgrupadorContenido", "AsistenciaCtrl:AAgrupadorContendio no puede ser nulo");

          aAgrupadorContenido.ContenidosDigitales = RetrieveListContenidoDigital(dctx, aAgrupadorContenido);
          if (aAgrupadorContenido is Asistencia)
          {
              Asistencia aAsistencia = aAgrupadorContenido as Asistencia;
              foreach (AAgrupadorContenidoDigital acont in aAsistencia.AgrupadoresContenido)
              {
                  AddContenidoDigital(dctx, acont);
              }
          }
          else
          {
              if (aAgrupadorContenido is AgrupadorCompuesto)
              {
                  AgrupadorCompuesto aAgrupadorCompuesto = aAgrupadorContenido as AgrupadorCompuesto;
                  foreach (AAgrupadorContenidoDigital acont in aAgrupadorCompuesto.AgrupadoresContenido)
                  {
                      AddContenidoDigital(dctx, acont);
                  }
              }
          }

      }

      /// <summary>
      /// Consulta los contenidos digitales Activos o en Mantenimiento asignados a un AAgrupadorContenidoDigital proporcionado
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="agrupador">AAgrupadorContenidoDigital que provee el criterio de consulta</param>
      /// <returns>Lista de contenidos digitales asignados al AAgrupadorContenidoDigital o nulo</returns>
      public List<ContenidoDigital> RetrieveListContenidoDigital(IDataContext dctx, AAgrupadorContenidoDigital agrupador)
      {
          if (agrupador == null || agrupador.AgrupadorContenidoDigitalID == null) throw new ArgumentNullException("agrupador", "AgrupadorContenidoDigitalID no puede ser nulo");
          AsistenciaContenidoDetalleDARetHlp asistenciaContenido = new AsistenciaContenidoDetalleDARetHlp();
          List<ContenidoDigital> contls = null;
          DataSet ds = null;

          ds = asistenciaContenido.Action(dctx, agrupador);
          if (ds.Tables[0].Rows.Count > 0)
          {
              ContenidoDigitalCtrl contenidoDigitalCtrl = new ContenidoDigitalCtrl();
              contls = new List<ContenidoDigital>();
              foreach (DataRow cont in ds.Tables[0].Rows)
              {
                  ContenidoDigital contenido = new ContenidoDigital();
                  contenido.ContenidoDigitalID = cont.Field<long>("ContenidoDigitalID");
                  DataSet dsContenido = contenidoDigitalCtrl.Retrieve(dctx, contenido);
                  int index = dsContenido.Tables[0].Rows.Count;
                  if (index > 0)
                  {
                      contenido = contenidoDigitalCtrl.LastDataRowToContenidoDigital(dsContenido);
                      contls.Add(contenido);
                  }
              
              }
          }
          return contls;
      }

      /// <summary>
      /// Consulta los contenidos digitales asignados a un AgrupadorContenidoDigitalID proporcionado
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="agrupadorContenidoID">AgrupadorContenidoDigitalID que provee el criterio de consulta</param>
      /// <returns>Lista de contenidos digitales asignados al AAgrupadorContenidoDigital o nulo</returns>
      private List<ContenidoDigital> RetrieveListContenidoDigital(IDataContext dctx,long? agrupadorContenidoID)
      {
          if (agrupadorContenidoID == null) throw new ArgumentNullException("agrupadorContenidoID", "AgrupadorContenidoDigitalID no puede ser nulo");
          AsistenciaContenidoDetalleDARetHlp asistenciaContenido = new AsistenciaContenidoDetalleDARetHlp();
          List<ContenidoDigital> contls = null;
          DataSet ds = null;

          ds = asistenciaContenido.Action(dctx, new Asistencia{AgrupadorContenidoDigitalID = agrupadorContenidoID});
          if (ds.Tables[0].Rows.Count > 0)
          {
              contls = new List<ContenidoDigital>();
              foreach (DataRow cont in ds.Tables[0].Rows)
              {
                  ContenidoDigital contenido = new ContenidoDigital();
                  contenido.ContenidoDigitalID = cont.Field<long>("ContenidoDigitalID");
                  contls.Add(contenido);
              }
          }
          return contls;
      }

    
       /// <summary>
      /// Elimina el contenido Digital de un AAgrupadorContenidoDigital
       /// </summary>
       /// <param name="dctx"></param>
       /// <param name="aAgrupador"></param>
       /// <param name="contenidoDigitalId"></param>
      private void DeleteContenidoDigital(IDataContext dctx, AAgrupadorContenidoDigital aAgrupador,long? contenidoDigitalId)
      {
          AsistenciaContenidoDetalleDADelHlp da = new AsistenciaContenidoDetalleDADelHlp();
          da.Action(dctx,aAgrupador,contenidoDigitalId);
      }

 
      #endregion


      /// <summary>
       /// Crea un objeto AAgrupadorContenidoDigital  a partir de los datos del último DataRow del DataSet.
       /// </summary>
       /// <param name="ds">El DataSet que contiene la información de Asistencia</param>
       /// <returns>AAgrupadorContenidoDigital creado a partir de los datos</returns>
       public AAgrupadorContenidoDigital LastDataRowToAgrupadorContenido(DataSet ds)
       {
           if (!ds.Tables.Contains("Asistencia"))
               throw new Exception("LastDataRowToAsistencia: DataSet no tiene la tabla Asistencia");
           int index = ds.Tables["Asistencia"].Rows.Count;
           if (index < 1)
               throw new Exception("LastDataRowToAsistencia: El DataSet no tiene filas");
           return this.DataRowToAgrupadorContenidoDigital(ds.Tables["Asistencia"].Rows[index - 1]);
       }

       /// <summary>
       /// Crea un objeto de Asistencia a partir de los datos de un DataRow.
       /// </summary>
       /// <param name="row">El DataRow que contiene la información de Asistencia</param>
       /// <returns>Un objeto de Asistencia creado a partir de los datos</returns>
       public Asistencia DataRowToAsistencia(DataRow row)
       {

           Asistencia asistencia = new Asistencia();

           if (row.IsNull("AgrupadorContenidoDigitalID"))
               asistencia.AgrupadorContenidoDigitalID = null;
           else
               asistencia.AgrupadorContenidoDigitalID = (long)Convert.ChangeType(row["AgrupadorContenidoDigitalID"], typeof(long));
           if (row.IsNull("Nombre"))
               asistencia.Nombre = null;
           else
               asistencia.Nombre = (string)Convert.ChangeType(row["Nombre"], typeof(string));
           if (row.IsNull("EstatusProfesionalizacion"))
               asistencia.Estatus = null;
           else
           {
               byte estado = (byte)Convert.ChangeType(row["EstatusProfesionalizacion"], typeof(byte));
               asistencia.Estatus = (EEstatusProfesionalizacion?)estado;
           }
           if (row.IsNull("FechaRegistro"))
               asistencia.FechaRegistro = null;
           else
               asistencia.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
           if (row.IsNull("Descripcion"))
               asistencia.Descripcion = null;
           else
               asistencia.Descripcion = (string)Convert.ChangeType(row["Descripcion"], typeof(string));
           if (row.IsNull("TemaAsistenciaID"))
               asistencia.TemaAsistencia = null;
           else
           {
               asistencia.TemaAsistencia = new TemaAsistencia();
               asistencia.TemaAsistencia.TemaAsistenciaID = (int)Convert.ChangeType(row["TemaAsistenciaID"], typeof(int));
           }
           if (row.IsNull("EsPredeterminado"))
               asistencia.EsPredeterminado = null;
           else
               asistencia.EsPredeterminado = (bool)Convert.ChangeType(row["EsPredeterminado"], typeof(bool));
           return asistencia;
       }

       /// <summary>
       /// Crea un objeto AAgrupadorContenidoDigital a partir de los datos de un DataRow.
       /// </summary>
       /// <param name="row">El DataRow que contiene la información de Asistencia</param>
       /// <returns>AAgrupadorContenidoDigital creado a partir de los datos</returns>
       public AAgrupadorContenidoDigital DataRowToAgrupadorContenidoDigital(DataRow row)
       {

           AAgrupadorContenidoDigital agrupador = null;
           if (!row.IsNull("TipoAgrupador"))
           {
               byte tp = (byte)Convert.ChangeType(row["TipoAgrupador"], typeof(byte));
               ETipoAgrupador tipo = (ETipoAgrupador)tp;
               switch (tipo)
               {
                   case ETipoAgrupador.SIMPLE:
                       agrupador = DataRowToAgrupadorContenidoDigital(tipo, row) as AgrupadorSimple;
                       break;
                   case ETipoAgrupador.COMPUESTO:
                       agrupador = DataRowToAgrupadorContenidoDigital(tipo, row) as AgrupadorCompuesto;
                       break;
                   case ETipoAgrupador.COMPUESTO_ASISTENCIA:
                       agrupador = DataRowToAsistencia(row);
                       break;
               }
           }

           return agrupador;
       }

       /// <summary>
       /// Crea un objeto AAgrupadorContenidoDigital (AgrupadorSimple o AgrupadorCompuesto) a partir de los datos de un DataRow.
       /// </summary>
       /// <param name="tipo">Tipo de agrupador</param>
       /// <param name="row">El DataRow que contiene la información del Agrupador</param>
       /// <returns>Un objeto AAgrupadorContenidoDigital creado a partir de los datos</returns>
       private AAgrupadorContenidoDigital DataRowToAgrupadorContenidoDigital(ETipoAgrupador tipo, DataRow row)
       {
           AAgrupadorContenidoDigital aAgrupador = null;
           switch (tipo)
           {
               case ETipoAgrupador.SIMPLE:
                   aAgrupador= new AgrupadorSimple();
                   break;
               case ETipoAgrupador.COMPUESTO:
                   aAgrupador = new AgrupadorCompuesto();
                   break;
               default:
                   throw new ArgumentOutOfRangeException("tipo");
           }

           if (row.IsNull("AgrupadorContenidoDigitalID"))
               aAgrupador.AgrupadorContenidoDigitalID = null;
           else
               aAgrupador.AgrupadorContenidoDigitalID = (long)Convert.ChangeType(row["AgrupadorContenidoDigitalID"], typeof(long));
           if (row.IsNull("Nombre"))
               aAgrupador.Nombre = null;
           else
               aAgrupador.Nombre = (string)Convert.ChangeType(row["Nombre"], typeof(string));
           if (row.IsNull("EstatusProfesionalizacion"))
               aAgrupador.Estatus = null;
           else
           {
               byte estado = (byte)Convert.ChangeType(row["EstatusProfesionalizacion"], typeof(byte));
               aAgrupador.Estatus = (EEstatusProfesionalizacion?)estado;
           }
           if (row.IsNull("EsPredeterminado"))
               aAgrupador.EsPredeterminado = null;
           else
               aAgrupador.EsPredeterminado = (bool) Convert.ChangeType(row["EsPredeterminado"], typeof (bool));
  
           if (row.IsNull("FechaRegistro"))
               aAgrupador.FechaRegistro = null;
           else
               aAgrupador.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));

           return aAgrupador;
       }

       #region red social.
       /// <summary>
       /// Consulta los registros completos de Asistencia con sus respectivos contenidos digitales.
       /// </summary>
       /// <param name="dctx">El DataContext que proveerá acceso a la base de datos.</param>
       /// <param name="pageSize">Número del tamaño de la página (cantidad de registros por página)</param>
       /// <param name="currentPage">Número de página se desea visualizar.</param>
       /// <param name="sortColumn">Columna encargada del ordenamiento de la consulta.</param>
       /// <param name="sortorder">Tipo de ordenamiento (ASC o DESC)</param>
       /// <param name="parametros">Diccionario de parámetros encargados del filtro de la consulta.</param>
       /// <returns></returns>
       public DataSet RetrieveComplete(IDataContext dctx, int pageSize, int currentPage, string sortColumn, string sortorder,
           Dictionary<string, string> parametros)
       {
           AsistenciaDARetHlp da = new AsistenciaDARetHlp();
           DataSet ds = da.Action(dctx, pageSize, currentPage, sortColumn, sortorder, parametros);
           return ds;
       }
       public DataSet RetrieveDetalle(IDataContext dctx, int pageSize, int currentPage, string sortColumn, string sortorder,
           Dictionary<string, string> parametros)
       {
           AsistenciaContenidoDetalleDARetHlp da = new AsistenciaContenidoDetalleDARetHlp();
           DataSet ds = da.Action(dctx, pageSize, currentPage, sortColumn, sortorder, parametros);
           return ds;
       }
       #endregion
       #region Asignar Contenidos Digitales A Asistencia
       /// <summary>
       /// Registra el contenido Digital de un AAgrupadorContenidoDigital a una Asistencia
       /// </summary>
       /// <param name="dctx"></param>
       /// <param name="aAgrupadorContenidoDigital"></param>
       /// <param name="contenidoDigital"></param>
       public void InsertContenidoAsistencia(IDataContext dctx, AAgrupadorContenidoDigital aAgrupadorContenidoDigital, ContenidoDigital contenidoDigital)
       {
           AsistenciaContenidoDetalleDARetHlp da = new AsistenciaContenidoDetalleDARetHlp();
           DataSet ds = da.Action(dctx,aAgrupadorContenidoDigital,contenidoDigital.ContenidoDigitalID);
           if(ds.Tables["Asistencia"].Rows.Count <= 0){
               AsistenciaContenidoDetalleDAInsHlp daAsistencia = new AsistenciaContenidoDetalleDAInsHlp();
               daAsistencia.Action(dctx, aAgrupadorContenidoDigital, contenidoDigital.ContenidoDigitalID);
           }
           else
               throw new DuplicateNameException("El contenido digital ya se encuentra asignado a la asistencia, por favor verifique.");
       }
       /// <summary>
       /// Elimina el contenido Digital de un AAgrupadorContenidoDigital de una Asistencia
       /// </summary>
       /// <param name="dctx"></param>
       /// <param name="aAgrupadorContenidoDigital"></param>
       /// <param name="contenidoDigital"></param>
       public void DeleteContenidoAsistencia(IDataContext dctx, AAgrupadorContenidoDigital aAgrupadorContenidoDigital, ContenidoDigital contenidoDigital)
       {
           DeleteContenidoDigital(dctx,aAgrupadorContenidoDigital,contenidoDigital.ContenidoDigitalID);
       }
       #endregion
   } 
}
