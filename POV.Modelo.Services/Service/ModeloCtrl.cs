using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Modelo.BO;
using POV.Modelo.Estandarizado.BO;
using POV.Modelo.Estandarizado.Service;
using POV.Modelo.DAO;
using POV.Modelo.DA;

namespace POV.Modelo.Service { 
   /// <summary>
   /// Controlador del objeto AModelo
   /// </summary>
   public class ModeloCtrl
   {
       #region ModeloDinamico
       /// <summary>
       /// Crea un registro de ModeloDinamicoInsHlp en la base de datos
       /// </summary>
       /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
       /// <param name="modelo">ModeloDinamico que desea crear</param>       
       public void InsertModeloDinamico(IDataContext dctx, ModeloDinamico modelo)
       {
           ModeloInsHlp da = new ModeloInsHlp();
           da.Action(dctx, modelo);
       }

       /// <summary>
       /// Actualiza de manera optimista un registro de ModeloUpdHlp en la base de datos.
       /// </summary>
       /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
       /// <param name="modeloDinamico">ModeloDinamico que tiene los datos nuevos</param>
       /// <param name="previous">ModeloDinamico que tiene los datos anteriores</param>
       public void UpdateModeloDinamico(IDataContext dctx, ModeloDinamico modeloDinamico, ModeloDinamico previous)
       {
           try
           {
               ModeloUpdHlp da = new ModeloUpdHlp();
               da.Action(dctx, modeloDinamico, previous);               
           }
           catch (Exception ex)
           {
               throw new DuplicateNameException(ex.Message);
           }
       }

       /// <summary>
       /// Elimina un registro de ModeloPrueba en la base de datos.
       /// </summary>
       /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
       /// <param name="modeloPrueba">ModeloDinamico que desea eliminar</param>
       public void DeleteModeloDinamico(IDataContext dctx, ModeloDinamico modeloPrueba)
       {
           ModeloDelHlp da = new ModeloDelHlp();
           da.Action(dctx, modeloPrueba);
       }

       /// <summary>
      /// Consulta registros de ModeloRetHlp en la base de datos.
      /// </summary>
      /// <param name="dctx"></param>
      /// <param name="modeloRetHlp"></param>
      /// <returns>El DataSet que contiene la información de ModeloRetHlp generada por la consulta</returns>
      public DataSet Retrieve(IDataContext dctx, AModelo modelo){
         ModeloRetHlp da = new ModeloRetHlp();
         DataSet ds = da.Action(dctx, modelo);
         return ds;
      }

      /// <summary>
      /// DA para Consulta de modelos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="modeloID">ModeloID que provee el criterio de selección para realizar la consulta</param>
      /// <param name="parametros">Parámetros del modelo que proveen el criterio de selección para realizar la consulta.
      /// Llaves adminitidas convertidas a string: TipoModelo(int), Nombre(string), EsEditable(bool), Activo(bool), ModeloDiagnostico(int)
      /// </param>
      /// <returns>DataSet con los modelos encontrados</returns>
      public DataSet Retrieve(IDataContext dctx, int? modeloID, Dictionary<string,string> parametros)
      {
          ModeloDARetHlp da = new ModeloDARetHlp();
          DataSet ds = da.Action(dctx, modeloID, parametros);
          return ds;
      }

      /// <summary>
      /// Devuelve un registro completo de modelo
      /// </summary>
      public AModelo RetrieveComplete(IDataContext dctx, AModelo modelo){
         if(modelo==null) throw  new Exception("ModeloCtrl:El parámetro Modelo, no puede ser vacio");
         if(modelo.TipoModelo==null)throw  new Exception("ModeloCtrl:El campo TipoModelo no puede ser vacio");
         AModelo ci = null;

         DataSet ds = Retrieve(dctx, modelo);
          if (ds.Tables[0].Rows.Count == 1){

                ci = this.LastDataRowToModelo(ds);
                if (ci.TipoModelo == ETipoModelo.Dinamico)
                {
                    (ci as ModeloDinamico).ListaClasificador = RetrieveClasificadoresModeloDinamico(dctx, ci as ModeloDinamico);
                    (ci as ModeloDinamico).ListaPropiedadPersonalizada = RetrieveListaPropiedadesModeloDinamico(dctx, ci as ModeloDinamico);
                }
          }
              
        return ci;
      }

       /// <summary>
       /// Crea un objeto de AModelo a partir de los datos del último DataRow del DataSet.
       /// </summary>
       /// <param name="ds">El DataSet que contiene la información de AModelo</param>
       /// <param name="tipo"> </param>
       /// <returns>Un objeto de AModelo creado a partir de los datos</returns>
       public AModelo LastDataRowToModelo(DataSet ds) {
         if (!ds.Tables.Contains("Modelo"))
            throw new Exception("LastDataRowToAModelo: DataSet no tiene la tabla AModelo");
         int index = ds.Tables["Modelo"].Rows.Count;
         if (index < 1)
            throw new Exception("LastDataRowToAModelo: El DataSet no tiene filas");
         return this.DataRowToAModelo(ds.Tables["Modelo"].Rows[index - 1]);
      }

       /// <summary>
       /// Crea un objeto de AModelo a partir de los datos de un DataRow.
       /// </summary>
       /// <param name="row">El DataRow que contiene la información de AModelo</param>
       /// <returns>Un objeto de AModelo creado a partir de los datos</returns>
      public AModelo DataRowToAModelo(DataRow row)
       {
          AModelo aModelo = null;
          
          if (row.IsNull("TipoModelo"))
               throw new Exception("DataRowToAModelo: El modelo no puede ser nulo");
          
          ETipoModelo tipo = (ETipoModelo)(int) Convert.ChangeType(row["TipoModelo"], typeof (int));
           switch (tipo)
          {
              case ETipoModelo.Dinamico:
                  aModelo = new ModeloDinamico();
                  break;
              default:
                  throw new ArgumentOutOfRangeException();
          }

          #region Parametros generales

           if (row.IsNull("ModeloID"))
               aModelo.ModeloID = null;
           else
               aModelo.ModeloID = (int)Convert.ChangeType(row["ModeloID"], typeof(int));
           if (row.IsNull("Nombre"))
               aModelo.Nombre = null;
           else
               aModelo.Nombre = (string)Convert.ChangeType(row["Nombre"], typeof(string));
           if (row.IsNull("Descripcion"))
               aModelo.Descripcion = null;
           else
               aModelo.Descripcion = (string)Convert.ChangeType(row["Descripcion"], typeof(string));
           if (row.IsNull("Activo"))
               aModelo.Estatus = null;
           else
               aModelo.Estatus = (bool)Convert.ChangeType(row["Activo"], typeof(bool));
           if (row.IsNull("FechaRegistro"))
               aModelo.FechaRegistro = null;
           else
               aModelo.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
           if (row.IsNull("EsEditable"))
               aModelo.EsEditable = null;
           else
               aModelo.EsEditable = (bool)Convert.ChangeType(row["EsEditable"], typeof(bool));

           if (aModelo.TipoModelo == ETipoModelo.Dinamico)
           {
               if (row.IsNull("MetodoCalificacion"))
                   (aModelo as ModeloDinamico).MetodoCalificacion = null;
               else
                   (aModelo as ModeloDinamico).MetodoCalificacion = (EMetodoCalificacion) ((byte)Convert.ChangeType(row["MetodoCalificacion"], typeof(byte)));
           }

           #endregion
                  
         return aModelo;
      }
       #endregion

      /// <summary>
       /// Consulta los clasificadores para un modelo estático
       /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
       /// <param name="modelo">Modelo a consultar</param>
       /// <returns>ArrayList con objeto genérico Clasificador(ClasificadorID,Nombre)</returns>
       public ArrayList RetrieveClasificadoresModelo(IDataContext dctx, int? modeloID)
       {
           if(modeloID==null)throw new Exception("ModeloCtrl:El parámetro modelo no puede ser vacio");
                       
            ArrayList clasificadores = new ArrayList();
            AModelo modelo = null;
            DataSet ds = Retrieve(dctx, modeloID, new Dictionary<string, string>());

           if (ds.Tables[0].Rows.Count >= 1)
               modelo = LastDataRowToModelo(ds);

           if (modelo is ModeloDinamico)
           {
               return new ArrayList(RetrieveClasificadoresModeloDinamico(dctx, modelo as ModeloDinamico));               
           }

           return clasificadores;
       }

       /// <summary>
       /// Lista de Clasificadores para un modelo dinámico
       /// </summary>
       /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
       /// <param name="modelo">Modelo a consultar</param>
       /// <returns>Lista con Clasificadores para un modelo dinámico</returns>
       public List<Clasificador> RetrieveClasificadoresModeloDinamico(IDataContext dctx, ModeloDinamico modelo)
       {            
           List<Clasificador> clasificadores = new List<Clasificador>();
            DataSet ds = RetrieveClasificador(dctx, new Clasificador() { Activo = true }, modelo);
               foreach (DataRow dr in ds.Tables[0].Rows)
               {
                   Clasificador clasificador = DataRowToClasificador(dr);
                   clasificadores.Add(RetrieveCompleteClasificador(dctx, modelo as ModeloDinamico, new Clasificador(){ClasificadorID = clasificador.ClasificadorID}));
               }

           return clasificadores;
       }

       /// <summary>
       /// Lista de PropiedadesPersonalizadas para un modelo dinámico
       /// </summary>
       /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
       /// <param name="modelo">Modelo a consultar</param>
       /// <returns>Lista con PropiedadesPersonalizadas para un modelo dinámico</returns>
       public List<PropiedadPersonalizada> RetrieveListaPropiedadesModeloDinamico(IDataContext dctx, ModeloDinamico modelo)
       {            
           List<PropiedadPersonalizada> propiedades = new List<PropiedadPersonalizada>();
            DataSet ds = RetrievePropiedadPersonalizada(dctx, new PropiedadPersonalizada() { Activo = true }, modelo);
               foreach (DataRow dr in ds.Tables[0].Rows)
               {
                   PropiedadPersonalizada propiedad = DataRowToPropiedadPersonalizada(dr);
                   propiedades.Add(LastDataRowToPropiedadPersonalizada(RetrievePropiedadPersonalizada(dctx, propiedad, modelo)));
               }

           return propiedades;
       }

       #region Clasificador ModeloPrueba
       /// <summary>
       /// Crea un registro de ClasificadorInsHlp en la base de datos
       /// </summary>
       /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
       /// <param name="modelo">ModeloDinamico que desea crear</param>
       /// <param name="clasificador">Clasificador que desea crear</param>
       public void InsertClasificador(IDataContext dctx, ModeloDinamico modelo, Clasificador clasificador)
       {
           ClasificadorInsHlp da = new ClasificadorInsHlp();
           da.Action(dctx, modelo, clasificador);
       }

       /// <summary>
       /// Actualiza de manera optimista un registro de ClasificadorUpdHlp en la base de datos.
       /// </summary>
       /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
       /// <param name="clasificador">Clasificador del Modelo que tiene los datos nuevos</param>
       /// <param name="previous">Clasificador del Modelo que tiene los datos anteriores</param>
       public void UpdateClasificador(IDataContext dctx, Clasificador clasificador, Clasificador previous)
       {
           try
           {
               ClasificadorUpdHlp da = new ClasificadorUpdHlp();
               da.Action(dctx, clasificador, previous);
           }
           catch (Exception ex)
           {
               throw new DuplicateNameException(ex.Message);
           }
       }

       /// <summary>
       /// Consulta registros de ClasificadorRetHlp en la base de datos.
       /// </summary>
       /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
       /// <param name="clasificador">Clasificador a consultar</param>
       /// <pasra name="modelo">Modelo Dinámico a consultar</pasra>       
       /// <returns>El DataSet que contiene la información de ClasificadorRetHlp generada por la consulta</returns>
       public DataSet RetrieveClasificador(IDataContext dctx, Clasificador clasificador, ModeloDinamico modelo)
       {
           ClasificadorRetHlp da = new ClasificadorRetHlp();
           DataSet ds = da.Action(dctx, clasificador, modelo);
           return ds;
       }

       /// <summary>
       /// Elimina un registro de Clasificador en la base de datos.
       /// </summary>
       /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
       /// <param name="clasificador">Clasificador del ModeloPrueba que desea eliminar</param>
       public void DeleteClasificador(IDataContext dctx, Clasificador clasificador)
       {
           ClasificadorDelHlp da = new ClasificadorDelHlp();
           da.Action(dctx, clasificador);
       }

       /// <summary>
       /// Consulta un registro de Clasificador completo de la base de datos
       /// </summary>
       /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
       /// <param name="modelo">Modelo Dinámico que provee el criterio de selección para realizar la consulta</param>
       /// <param name="clasificador">Clasificador que provee el criterio de selección para realizar la consulta</param>
       /// <returns>Clasificador o null en caso de no encontrarse</returns>
       public Clasificador RetrieveCompleteClasificador(IDataContext dctx, ModeloDinamico modelo, Clasificador clasificador) 
       {
           if (clasificador == null) throw new ArgumentNullException("ModeloCtrl: clasificador no puede ser nulo");

           Clasificador clasificadorComplete = null;

           DataSet dsClasificador = RetrieveClasificador(dctx, clasificador, modelo);
           if (dsClasificador.Tables[0].Rows.Count > 0)
           {
               int index = dsClasificador.Tables[0].Rows.Count;
               DataRow drClasificador = dsClasificador.Tables[0].Rows[index - 1];

               clasificadorComplete = DataRowToClasificador(drClasificador);     

               List<PropiedadClasificador> propiedades = new List<PropiedadClasificador>();

               DataSet dsPropiedades = RetrievePropiedadClasificador(dctx, clasificador, new PropiedadClasificador() { Activo = true });
               foreach (DataRow dr in dsPropiedades.Tables[0].Rows)
               {
                   PropiedadClasificador propiedad = DataRowToPropiedadClasificador(dr);
                   propiedad.Propiedad = LastDataRowToPropiedadPersonalizada(RetrievePropiedadPersonalizada(dctx, propiedad.Propiedad, new ModeloDinamico()));

                   propiedades.Add(propiedad);
               }
                                            
               clasificadorComplete.ListaPropiedadClasificador = propiedades;
           }           

           return clasificadorComplete;
       }

       /// <summary>
       /// Crea un objeto de Clasificador a partir de los datos del último DataRow del DataSet.
       /// </summary>
       /// <param name="ds">El DataSet que contiene la información de Clasificador</param>
       /// <returns>Un objeto de Clasificador creado a partir de los datos</returns>
       public Clasificador LastDataRowToClasificador(DataSet ds)
       {
           if (!ds.Tables.Contains("Clasificador"))
               throw new Exception("LastDataRowToClasificador: DataSet no tiene la tabla Clasificador");
           int index = ds.Tables["Clasificador"].Rows.Count;
           if (index < 1)
               throw new Exception("LastDataRowToClasificador: El DataSet no tiene filas");
           return this.DataRowToClasificador(ds.Tables["Clasificador"].Rows[index - 1]);
       }

       /// <summary>
       /// Crea un objeto de Clasificador a partir de los datos de un DataRow.
       /// </summary>
       /// <param name="row">El DataRow que contiene la información de Clasificador</param>
       /// <returns>Un objeto de Clasificador creado a partir de los datos</returns>
       public Clasificador DataRowToClasificador(DataRow row)
       {
           Clasificador clasificador = new Clasificador();           

           if (row.IsNull("ClasificadorID"))
               clasificador.ClasificadorID = null;
           else
               clasificador.ClasificadorID = (int)Convert.ChangeType(row["ClasificadorID"], typeof(int));

           if (row.IsNull("Nombre"))
               clasificador.Nombre = null;
           else
               clasificador.Nombre = (string)Convert.ChangeType(row["Nombre"], typeof(string));

           if (row.IsNull("Descripcion"))
               clasificador.Descripcion = null;
           else
               clasificador.Descripcion = (string)Convert.ChangeType(row["Descripcion"], typeof(string));

           if (row.IsNull("FechaRegistro"))
               clasificador.FechaRegistro = null;
           else
               clasificador.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));

           if (row.IsNull("Activo"))
               clasificador.Activo = null;
           else
               clasificador.Activo = (bool)Convert.ChangeType(row["Activo"], typeof(bool));

           return clasificador;
       }
       #endregion

       #region PropiedadPersonalizada
       /// <summary>
       /// Crea un registro de PropiedadClasificadorInsHlp en la base de datos
       /// </summary>
       /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
       /// <param name="modelo">ModeloDinamico que desea crear</param>
       /// <param name="propiedad">PropiedadPersonalizada que desea crear</param>       
       public void InsertPropiedadPersonalizada(IDataContext dctx, ModeloDinamico modelo, PropiedadPersonalizada propiedad)
       {
           PropiedadPersonalizadaInsHlp da = new PropiedadPersonalizadaInsHlp();
           da.Action(dctx, modelo, propiedad);
       }

       /// <summary>
       /// Actualiza de manera optimista un registro de PropiedadPersonalizadaUpdHlp en la base de datos.
       /// </summary>
       /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
       /// <param name="propiedad">PropiedadPersonalizada que tiene los datos nuevos</param>
       /// <param name="previous">PropiedadPersonalizada que tiene los datos anteriores</param>
       public void UpdatePropiedadPersonalizada(IDataContext dctx, PropiedadPersonalizada propiedad, PropiedadPersonalizada previous)
       {
           try
           {
               PropiedadPersonalizadaUpdHlp da = new PropiedadPersonalizadaUpdHlp();
               da.Action(dctx, propiedad, previous);
           }
           catch (Exception ex)
           {
               throw new DuplicateNameException(ex.Message);
           }
       }

       /// <summary>
       /// Consulta registros de PropiedadPersonalizadaRetHlp en la base de datos.
       /// </summary>
       /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
       /// <param name="propiedad">Propiedad Personalizada que provee el criterio de selección para realizar la consulta</param>
       /// <pasra name="modelo">Modelo Dinámico que provee el criterio de selección para realizar la consulta</pasra>       
       /// <returns>El DataSet que contiene la información de PropiedadPersonalizadaRetHlp generada por la consulta</returns>
       public DataSet RetrievePropiedadPersonalizada(IDataContext dctx, PropiedadPersonalizada propiedad, ModeloDinamico modelo)
       {
           PropiedadPersonalizadaRetHlp da = new PropiedadPersonalizadaRetHlp();
           DataSet ds = da.Action(dctx, propiedad, modelo);
           return ds;        
       }

       /// <summary>
       /// Crea un objeto de PropiedadPersonalizada a partir de los datos del último DataRow del DataSet.
       /// </summary>
       /// <param name="ds">El DataSet que contiene la información de PropiedadPersonalizada</param>
       /// <returns>Un objeto de PropiedadPersonalizada creado a partir de los datos</returns>
       public PropiedadPersonalizada LastDataRowToPropiedadPersonalizada(DataSet ds)
       {
           if (!ds.Tables.Contains("PropiedadPersonalizada"))
               throw new Exception("LastDataRowToPropiedadPersonalizada: DataSet no tiene la tabla PropiedadPersonalizada");
           int index = ds.Tables["PropiedadPersonalizada"].Rows.Count;
           if (index < 1)
               throw new Exception("LastDataRowToPropiedadPersonalizada: El DataSet no tiene filas");
           return this.DataRowToPropiedadPersonalizada(ds.Tables["PropiedadPersonalizada"].Rows[index - 1]);
       }

       /// <summary>
       /// Crea un objeto de PropiedadPersonalizada a partir de los datos de un DataRow.
       /// </summary>
       /// <param name="row">El DataRow que contiene la información de PropiedadPersonalizada</param>
       /// <returns>Un objeto de PropiedadPersonalizada creado a partir de los datos</returns>
       public PropiedadPersonalizada DataRowToPropiedadPersonalizada(DataRow row)
       {
           PropiedadPersonalizada propiedadPersonalizada = new PropiedadPersonalizada();
           if (row.IsNull("PropiedadID"))
               propiedadPersonalizada.PropiedadID = null;
           else
               propiedadPersonalizada.PropiedadID = (int)Convert.ChangeType(row["PropiedadID"], typeof(int));
           if (row.IsNull("Nombre"))
               propiedadPersonalizada.Nombre = null;
           else
               propiedadPersonalizada.Nombre = (String)Convert.ChangeType(row["Nombre"], typeof(String));
           if (row.IsNull("Descripcion"))
               propiedadPersonalizada.Descripcion = null;
           else
               propiedadPersonalizada.Descripcion = (String)Convert.ChangeType(row["Descripcion"], typeof(String));
           if (row.IsNull("EsVisible"))
               propiedadPersonalizada.EsVisible = null;
           else
               propiedadPersonalizada.EsVisible = (bool)Convert.ChangeType(row["EsVisible"], typeof(bool));
           if (row.IsNull("Activo"))
               propiedadPersonalizada.Activo = null;
           else
               propiedadPersonalizada.Activo = (bool)Convert.ChangeType(row["Activo"], typeof(bool));
           if (row.IsNull("FechaRegistro"))
               propiedadPersonalizada.FechaRegistro = null;
           else
               propiedadPersonalizada.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
           return propiedadPersonalizada;
       }
       #endregion

       #region PropiedadClasificador
       /// <summary>
       /// Crea un registro de PropiedadClasificadorInsHlp en la base de datos
       /// </summary>
       /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
       /// <param name="clasificador">Clasificador que desea crear</param>
       /// <param name="propiedad">PropiedadClasificador que desea crear</param>       
       public void InsertPropiedadClasificador(IDataContext dctx, Clasificador clasificador, PropiedadClasificador propiedad)
       {
           PropiedadClasificadorInsHlp da = new PropiedadClasificadorInsHlp();
           da.Action(dctx, clasificador, propiedad);
       }

       /// <summary>
       /// Actualiza de manera optimista un registro de PropiedadPersonalizadaUpdHlp en la base de datos.
       /// </summary>
       /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
       /// <param name="clasificador">Clasificador a la que pertenece la propiedad</param>
       /// <param name="propiedad">PropiedadPersonalizada que tiene los datos nuevos</param>
       /// <param name="previous">PropiedadPersonalizada que tiene los datos anteriores</param>
       public void UpdatePropiedadClasificador(IDataContext dctx,Clasificador clasificador, PropiedadClasificador propiedad, PropiedadClasificador previous)
       {
           try
           {
               PropiedadClasificadorUpdHlp da = new PropiedadClasificadorUpdHlp();
               da.Action(dctx,clasificador, propiedad, previous);
           }
           catch (Exception ex)
           {
               throw new DuplicateNameException(ex.Message);
           }
       }

       /// <summary>
       /// Consulta registros de PropiedadClasificadorRetHlp en la base de datos.
       /// </summary>
       /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
       /// <param name="clasificador">Clasificador que provee el criterio de selección para realizar la consulta</param>
       /// <pasra name="propiedad">Propiedad Clasificador que provee el criterio de selección para realizar la consulta</pasra>       
       /// <returns>El DataSet que contiene la información de PropiedadClasificadorRetHlp generada por la consulta</returns>
       public DataSet RetrievePropiedadClasificador(IDataContext dctx, Clasificador clasificador, PropiedadClasificador propiedad)
       {
           PropiedadClasificadorRetHlp da = new PropiedadClasificadorRetHlp();
           DataSet ds = da.Action(dctx, clasificador, propiedad );
           return ds;
       }       

       /// <summary>
       /// Crea un objeto de PropiedadClasificador a partir de los datos del último DataRow del DataSet.
       /// </summary>
       /// <param name="ds">El DataSet que contiene la información de PropiedadClasificador</param>
       /// <returns>Un objeto de PropiedadClasificador creado a partir de los datos</returns>
       public PropiedadClasificador LastDataRowToPropiedadClasificador(DataSet ds)
       {
           if (!ds.Tables.Contains("PropiedadClasificador"))
               throw new Exception("LastDataRowToPropiedadClasificador: DataSet no tiene la tabla PropiedadClasificador");
           int index = ds.Tables["PropiedadClasificador"].Rows.Count;
           if (index < 1)
               throw new Exception("LastDataRowToPropiedadClasificador: El DataSet no tiene filas");
           return this.DataRowToPropiedadClasificador(ds.Tables["PropiedadClasificador"].Rows[index - 1]);
       }

       /// <summary>
       /// Crea un objeto de PropiedadClasificador a partir de los datos de un DataRow.
       /// </summary>
       /// <param name="row">El DataRow que contiene la información de PropiedadClasificador</param>
       /// <returns>Un objeto de PropiedadClasificador creado a partir de los datos</returns>
       public PropiedadClasificador DataRowToPropiedadClasificador(DataRow row)
       {
           PropiedadClasificador propiedadClasificador = new PropiedadClasificador();
           propiedadClasificador.Propiedad = new PropiedadPersonalizada();
           if (row.IsNull("PropiedadID"))
               propiedadClasificador.Propiedad.PropiedadID = null;
           else
               propiedadClasificador.Propiedad.PropiedadID = (int)Convert.ChangeType(row["PropiedadID"], typeof(int));
           if (row.IsNull("Descripcion"))
               propiedadClasificador.Descripcion = null;
           else
               propiedadClasificador.Descripcion = (String)Convert.ChangeType(row["Descripcion"], typeof(String));
           if (row.IsNull("Activo"))
               propiedadClasificador.Activo = null;
           else
               propiedadClasificador.Activo = (bool)Convert.ChangeType(row["Activo"], typeof(bool));
           if (row.IsNull("FechaRegistro"))
               propiedadClasificador.FechaRegistro = null;
           else
               propiedadClasificador.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
           return propiedadClasificador;
       }
       #endregion
              
   } 
}
