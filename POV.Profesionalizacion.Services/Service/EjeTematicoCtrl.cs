using System;
using System.Collections.Generic;
using System.Data;
using Framework.Base.DataAccess;
using POV.Profesionalizacion.BO;
using POV.Profesionalizacion.DAO;
using System.Linq;
using POV.Profesionalizacion.DA;

namespace POV.Profesionalizacion.Service { 
   /// <summary>
   /// Controlador del objeto EjeTematico
   /// </summary>
   public class EjeTematicoCtrl { 
      /// <summary>
      /// Consulta registros de EjeTematicoRetHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="ejeTematicoRetHlp">EjeTematicoRetHlp que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de EjeTematicoRetHlp generada por la consulta</returns>
      public DataSet Retrieve(IDataContext dctx, EjeTematico ejeTematico){
         EjeTematicoRetHlp da = new EjeTematicoRetHlp();
         DataSet ds = da.Action(dctx, ejeTematico);
         return ds;

      }
      /// <summary>
      /// Consulta registros de EjeTematicoDARetHlp en la base de datos.
      /// </summary>      
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="pageSize"></param>
      /// <param name="currentPage"></param>
      /// <param name="sortColumn"></param>
      /// <param name="sortorder"></param>
      /// <param name="parametros"></param>      
      /// <returns>El DataSet que contiene la información de EjeTematicoDARetHlp generada por la consulta</returns>
      public DataSet RetrieveEjesTematicos(IDataContext dctx, int pageSize, int currentPage, string sortColumn, string sortorder, Dictionary<string, string> parametros)
      {
          EjeTematicoDARetHlp da = new EjeTematicoDARetHlp();
          DataSet ds = da.Action(dctx, pageSize, currentPage, sortColumn, sortorder, parametros);
          return ds;
      }
      /// <summary>
      /// Consulta registros de EjeTematico en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="ejeTematicoRetHlp">EjeTematicoRetHlp que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El Eje Temático  que contiene la información completa</returns>
      /// <summary>
      /// 
      private List<EjeTematico> DataSetToLista(IDataContext dctx, DataSet dsEjesTematicos) {
          List<EjeTematico> result = new List<EjeTematico>();
           foreach (DataRow dr in dsEjesTematicos.Tables[0].Rows) {
              result.Add(this.DataRowToEjeTematico(dr));
          }
          return result;
      }      
       /// <summary>
      /// Consulta registros de EjeTematico en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="ejeTematicoRetHlp">EjeTematicoRetHlp que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El Eje Temático  que contiene la información completa</returns>
      /// <summary>     
       
       public EjeTematico RetrieveComplete(IDataContext dctx, EjeTematico ejeTematico)
      {

          if (ejeTematico == null)
              throw new Exception("No se permiten búsquedas, con objetos nulos" + "RetrieveComplete de EjeTematico");
          AreaProfesionalizacionCtrl ctrlAreaProfesionalizacion = new AreaProfesionalizacionCtrl();
          SituacionAprendizajeCtrl ctrlSituacionesAprendizaje = new SituacionAprendizajeCtrl();
          DataSet dsEjeTematico = this.Retrieve(dctx, ejeTematico);
          AreaProfesionalizacion areaProfesionalizacionResult = null;
          EjeTematico nuevoEjeTematico = null;
          try
          {
              nuevoEjeTematico = LastDataRowToEjeTematico(dsEjeTematico);
              areaProfesionalizacionResult = ctrlAreaProfesionalizacion.RetrieveComplete(dctx, nuevoEjeTematico.AreaProfesionalizacion);
          }catch(Exception e){
              throw e;
          }
           nuevoEjeTematico.AreaProfesionalizacion = areaProfesionalizacionResult;

           List<MateriaProfesionalizacion> materiasPerezosas = this.RetrieveMateriaEjeTematico(dctx, nuevoEjeTematico, new MateriaProfesionalizacion() { Activo = true });
          List<MateriaProfesionalizacion> materiasCompletas = new List<MateriaProfesionalizacion>();
          foreach (MateriaProfesionalizacion materia in materiasPerezosas) {

              DataSet dsMateria =    ctrlAreaProfesionalizacion.MateriaProfesionalizacionRetrieve(dctx, materia, areaProfesionalizacionResult);
              if (dsMateria.Tables[0].Rows.Count == 1) {
                  DataRow dr = dsMateria.Tables[0].Rows[0];
                  MateriaProfesionalizacion materiaProfesionalizacion = new MateriaProfesionalizacion();
                  materiaProfesionalizacion.MateriaID = Convert.ToInt32(dr["MateriaID"]);
                  materiaProfesionalizacion.Nombre = dr["Nombre"].ToString();
                  materiaProfesionalizacion.FechaRegistro = Convert.ToDateTime(dr["FechaRegistro"]);
                  materiaProfesionalizacion.Activo = Convert.ToBoolean(dr["Activo"]);
                  materiasCompletas.Add(materiaProfesionalizacion);
            }
          
          }
          nuevoEjeTematico.MateriasProfesionalizacion = materiasCompletas;
          DataSet dsSituaciones =  ctrlSituacionesAprendizaje.Retrieve(dctx, ejeTematico, new SituacionAprendizaje()) ;
          foreach (DataRow drSituacion in dsSituaciones.Tables[0].Rows) {
              nuevoEjeTematico.SituacionesAprendizaje.ToList().Add(ctrlSituacionesAprendizaje.DataRowToSituacionAprendizaje(drSituacion));
          }
          
          
          return nuevoEjeTematico;
      }
      private void TieneMaterias(EjeTematico ejeTematico)
      {
          if (ejeTematico.MateriasProfesionalizacion == null)
              throw new Exception("El Eje Temático debe Tener por lo Menos una Materia");
          if (ejeTematico.MateriasProfesionalizacion.ToList() == null)
              throw new Exception("El Eje Temático debe Tener por lo Menos una Materia");
          if (ejeTematico.MateriasProfesionalizacion.ToList().Count == 0)
              throw new Exception("El Eje Temático debe Tener por lo Menos una Materia");
      }

      /// Consulta registros de EjeTematicoRetHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="ejeTematicoRetHlp">EjeTematicoRetHlp que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de EjeTematicoRetHlp generada por la consulta</returns>
      public List<EjeTematico> RetrieveComplete(IDataContext dctx, EjeTematico ejeTematico,MateriaProfesionalizacion materiaProfesionalizacion)
      {

        if (ejeTematico == null)
            throw new Exception("No se permiten búsquedas con objetos nulos" + "RetrieveComplete de EjeTematico");
            List<EjeTematico> ejesTematicos = new List<EjeTematico>();
            EjeTematico nuevoEjeTematico = null;
            DataSet dsEjeTematico = this.Retrieve(dctx, ejeTematico);

            foreach (DataRow drEjeTematico in dsEjeTematico.Tables[0].Rows)
            {
                nuevoEjeTematico = this.RetrieveComplete(dctx,this.DataRowToEjeTematico(drEjeTematico));
                ejesTematicos.Add(nuevoEjeTematico);

                if (materiaProfesionalizacion != null)
                    if (materiaProfesionalizacion.MateriaID != null)
                        if (ejeTematico.AreaProfesionalizacion.AreaProfesionalizacionID != null)
                        {
                            var result=  nuevoEjeTematico.MateriasProfesionalizacion.Where(m => m.MateriaID == materiaProfesionalizacion.MateriaID);
                            if (result.ToList()!=null) 
                            {
                                if (result.ToList().Count <= 0)
                                {
                                    EjeTematico temp = this.SearchEjeTematico(nuevoEjeTematico, ejesTematicos);
                                    int index = ejesTematicos.FindIndex(x => x.EjeTematicoID == temp.EjeTematicoID);
                                    if (index >= 0 && index < ejesTematicos.Count)
                                        ejesTematicos.RemoveAt(index);
                                }
                            }
                        }
            }

            if (ejeTematico.AreaProfesionalizacion != null &&
                    ejeTematico.AreaProfesionalizacion.NivelEducativo != null &&
                    ejeTematico.AreaProfesionalizacion.NivelEducativo.NivelEducativoID != null)
            {
                ejesTematicos = ejesTematicos.Where( x => x.AreaProfesionalizacion.NivelEducativo.NivelEducativoID ==
                        ejeTematico.AreaProfesionalizacion.NivelEducativo.NivelEducativoID).ToList();
            }

            if (ejeTematico.AreaProfesionalizacion != null && ejeTematico.AreaProfesionalizacion.Grado != null)
            {
                ejesTematicos = ejesTematicos.Where( x => x.AreaProfesionalizacion.Grado == 
                        ejeTematico.AreaProfesionalizacion.Grado).ToList();
            }

        return ejesTematicos;
   
      }

      ///<sumarry>
      ///Obtiene la lista de Materias  que pertenecen a un Eje tematico y que cumple los criterios de materia
      /// <param name="EjeTematico"> El eje Temático </param>
      /// <returns>  Materia a utilizar como filtro</returns>
      /// </sumarry>
      public List<MateriaProfesionalizacion> RetrieveMateriaEjeTematico(IDataContext dctx, EjeTematico ejeTematico, MateriaProfesionalizacion materia)
      {
          EjeTematicoMateriaProfesionalizacionDARetHlp da = new EjeTematicoMateriaProfesionalizacionDARetHlp();
          List<MateriaProfesionalizacion> materias = new List<MateriaProfesionalizacion>();
          DataSet ds = da.Action(dctx, ejeTematico, materia);
          foreach (DataRow dr in ds.Tables[0].Rows)
          {
              materias.Add(new MateriaProfesionalizacion()
              {
                  MateriaID = Convert.ToInt32(dr["MateriaID"]),
                  Nombre = dr["Nombre"].ToString()
              });
          }
          return materias;

      }
      ///<sumarry>
      ///Devuelve null si el ejetematico  no esta en la lista, de lo contrario devuelve el ejetematico
      /// <param name="ejeTematico"> Eje tematico a  utilizar como filtro</param>
      /// <param name="ejesTematicos">Lista de  Ejes tematicos </param>
      /// <returns>  EjeTematico encontrado o no</returns>
      /// </sumarry>
      private EjeTematico SearchEjeTematico(EjeTematico ejeTematico, List<EjeTematico> ejesTematicos)
      {

          List<EjeTematico> result = ejesTematicos.Where(r => r.EjeTematicoID == ejeTematico.EjeTematicoID).ToList();
          if (result != null)
          {
              if (result.Count > 0)
              {
                  return result.First();
              }
          }
          return null;
      }

      /// <summary>
      /// Crea un registro de EjeTematico en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="ejeTematicoInsHlp">EjeTematicoInsHlp que desea crear</param>
      public void Insert(IDataContext dctx, EjeTematico  ejeTematico){

          if(!this.isValidForInsert(dctx, ejeTematico))
              throw new DuplicateNameException("El nombre del eje temático ya está registrado en el sistema con la misma asignatura y el mismo bloque, por favor verifique.");
          TieneMaterias(ejeTematico);

          EjeTematicoInsHlp da = new EjeTematicoInsHlp();
           da.Action(dctx,  ejeTematico);
      }
 
      /// <summary>
      /// Crea un registro de EjeTematico completo en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="ejeTematicoInsHlp">EjeTematicoInsHlp que desea crear</param>
      public void InsertComplete(IDataContext dctx, EjeTematico ejeTematico)
      {

          #region Servicios
          AreaProfesionalizacionCtrl areaProfesionalizacionCtrl = new AreaProfesionalizacionCtrl();
         
          #endregion
          #region Da´s
          EjeTematicoMateriaProfesionalizacionDAInsHlp daInsertEjeTematicoMateriaProfesionalizacion = new EjeTematicoMateriaProfesionalizacionDAInsHlp();
          #endregion
        
          object miFirma = new object();
          dctx.OpenConnection(miFirma);
          dctx.BeginTransaction(miFirma);
          try
          {
              this.Insert(dctx, ejeTematico);


              EjeTematico ejeTematicoResult = LastDataRowToEjeTematico(this.Retrieve(dctx, ejeTematico));

              AreaProfesionalizacion areaProfesioalizacionComplete = areaProfesionalizacionCtrl.RetrieveComplete(dctx, ejeTematico.AreaProfesionalizacion);
              if(areaProfesioalizacionComplete==null)
                  throw new Exception("No se permite guardar  un Eje Temático sin Área de profesionalizacion");  
              
              if (areaProfesioalizacionComplete.MateriasProfesionalizacion == null)
                  throw new Exception("No se permite guardar  un Eje Temático sin materias de profesionalizacion");  
              if (areaProfesioalizacionComplete.MateriasProfesionalizacion.ToList() == null)
                  throw new Exception("No se permite guardar  un Eje Temático sin materias de profesionalizacion");  
              if(areaProfesioalizacionComplete.MateriasProfesionalizacion.ToList().Count==0)
                  throw new Exception("No se permite guardar  un Eje Temático sin materias de profesionalizacion");  

              List<MateriaProfesionalizacion> materiasAreaProfesionalizacion = areaProfesioalizacionComplete.MateriasProfesionalizacion.ToList();

              if (ejeTematico.MateriasProfesionalizacion == null)
                  throw new Exception("Un eje temático tiene que tener por lo menos una materia de profesionalización");
              if (ejeTematico.MateriasProfesionalizacion.ToList() == null)
                  throw new Exception("Un eje temático tiene que tener por lo menos una materia de profesionalización");                 
              if (ejeTematico.MateriasProfesionalizacion.ToList().Count==0)
                  throw new Exception("Un eje temático tiene que tener por lo menos una materia de profesionalización");
           
              
              foreach (MateriaProfesionalizacion materia in ejeTematico.MateriasProfesionalizacion){
                         DataSet ds = areaProfesionalizacionCtrl.MateriaProfesionalizacionRetrieve(dctx, materia, areaProfesioalizacionComplete);
                   if (ds.Tables[0].Rows.Count == 1){

                                      DataRow drMateria = ds.Tables[0].Rows[0];
                                      MateriaProfesionalizacion materiaInsert = new MateriaProfesionalizacion()
                                      {
                                          MateriaID = Convert.ToInt32(drMateria["MateriaID"])
                                      };

                                      daInsertEjeTematicoMateriaProfesionalizacion.Action(dctx, ejeTematicoResult, materiaInsert);
                  }
             }
                  
              
          }
          catch (Exception e)
          {
              dctx.RollbackTransaction(miFirma);
              dctx.CloseConnection(miFirma);
              throw e;
          }
          dctx.CommitTransaction(miFirma);
          dctx.CloseConnection(miFirma);
       }

      /// <summary>
      /// Valida si registro de EjeTematico existe en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="ejeTematico">EjeTematico que desea validar</param>

      private bool isValidForInsert(IDataContext dctx, EjeTematico ejeTematico)
      {

          List<EjeTematico> ejesActivos = RetrieveComplete(dctx, new EjeTematico { Nombre = ejeTematico.Nombre, AreaProfesionalizacion = ejeTematico.AreaProfesionalizacion, EstatusProfesionalizacion = EEstatusProfesionalizacion.ACTIVO}, ejeTematico.MateriasProfesionalizacion.FirstOrDefault());
          List<EjeTematico> ejesMantenimiento = RetrieveComplete(dctx, new EjeTematico { Nombre = ejeTematico.Nombre, AreaProfesionalizacion = ejeTematico.AreaProfesionalizacion, EstatusProfesionalizacion = EEstatusProfesionalizacion.MANTENIMIENTO }, ejeTematico.MateriasProfesionalizacion.FirstOrDefault());

          return ejesActivos.Count <= 0 && ejesMantenimiento.Count <= 0;
      }

      private bool isValidForUpdate(IDataContext dctx, EjeTematico ejeTematico)
      {

          List<EjeTematico> ejesActivos = RetrieveComplete(dctx, new EjeTematico { Nombre = ejeTematico.Nombre, AreaProfesionalizacion = ejeTematico.AreaProfesionalizacion, EstatusProfesionalizacion = EEstatusProfesionalizacion.ACTIVO }, ejeTematico.MateriasProfesionalizacion.FirstOrDefault());
          List<EjeTematico> ejesMantenimiento = RetrieveComplete(dctx, new EjeTematico { Nombre = ejeTematico.Nombre, AreaProfesionalizacion = ejeTematico.AreaProfesionalizacion, EstatusProfesionalizacion = EEstatusProfesionalizacion.MANTENIMIENTO }, ejeTematico.MateriasProfesionalizacion.FirstOrDefault());

          ejesActivos = ejesActivos.Where(x => x.EjeTematicoID != ejeTematico.EjeTematicoID).ToList();
          ejesMantenimiento = ejesMantenimiento.Where(x => x.EjeTematicoID != ejeTematico.EjeTematicoID).ToList();

          return ejesActivos.Count <= 0 && ejesMantenimiento.Count <= 0;
      }
      /// <summary>
      /// Actualiza de manera optimista un registro de EjeTematicoUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="ejeTematicoUpdHlp">EjeTematicoUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">EjeTematicoUpdHlp que tiene los datos anteriores</param>
      public void Update(IDataContext dctx, EjeTematico  ejeTematico, EjeTematico previous){
         EjeTematicoUpdHlp da = new EjeTematicoUpdHlp();
          if (isValidForUpdate(dctx, ejeTematico))
             {
                 TieneMaterias(ejeTematico);
                 da.Action(dctx, ejeTematico, previous);
             }
             else
                 throw new DuplicateNameException("El nombre del eje temático ya está registrado en el sistema con la misma asignatura y el mismo bloque, por favor verifique.");
      }
      /// <summary>
      /// Actualiza de manera optimista un registro de EjeTematicoUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="ejeTematicoUpdHlp">EjeTematicoUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">EjeTematicoUpdHlp que tiene los datos anteriores</param>
      public void UpdateComplete(IDataContext dctx, EjeTematico ejeTematico, EjeTematico previous)
      { 
          
          object miFirma = new object();
          dctx.OpenConnection(miFirma);
          dctx.BeginTransaction(miFirma);
          try
          {
              this.Update(dctx, ejeTematico, previous);
              this.DeleteMateriasEjeTematico(dctx, previous);
              this.InsertMateriasEjeTematico(dctx, ejeTematico);
          }
          catch (Exception e)
          {
              dctx.RollbackTransaction(miFirma);
              dctx.CloseConnection(miFirma);
              throw e;
          }
          dctx.CommitTransaction(miFirma);
          dctx.CloseConnection(miFirma);
      }
      /// <summary>
      /// Elimina un registro de EjeTematicoDelHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="ejeTematicoDelHlp">EjeTematicoDelHlp que desea eliminar</param>
      public void Delete(IDataContext dctx, EjeTematico ejeTematico)
      {
          EjeTematicoDelHlp da = new EjeTematicoDelHlp();
          da.Action(dctx, ejeTematico);
      }
 
      /// <summary>
      /// Elimina las materias de un eje tematico.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="ejeTematico">EjeTematico que contiene las materias</param>
      public void DeleteMateriasEjeTematico(IDataContext dctx,EjeTematico ejeTematico) {
          EjeTematicoMateriaProfesionalizacionDADelHlp da = new EjeTematicoMateriaProfesionalizacionDADelHlp();
          foreach (MateriaProfesionalizacion materia in ejeTematico.MateriasProfesionalizacion) {
              da.Action(dctx, ejeTematico, materia);
          }
      }
      /// Guarda registros de las materias de un eje tematico.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="ejeTematico">EjeTematico que contiene las materias</param>
      public void InsertMateriasEjeTematico(IDataContext dctx, EjeTematico ejeTematico)
      {
          EjeTematicoMateriaProfesionalizacionDAInsHlp da = new EjeTematicoMateriaProfesionalizacionDAInsHlp();
          foreach (MateriaProfesionalizacion materia in ejeTematico.MateriasProfesionalizacion){
              da.Action(dctx, ejeTematico, materia);
          }
      }

   
      /// <summary>
      /// Elimina un registro completo de EjeTematicoDelHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="ejeTematicoDelHlp">EjeTematicoDelHlp que desea eliminar</param>
      public void DeleteComplete(IDataContext dctx, EjeTematico ejeTematico)
      {
          object miFirma = new object();
          dctx.OpenConnection(miFirma);
          dctx.BeginTransaction(miFirma);
          try
          {
              this.Delete(dctx, ejeTematico);
              this.DeleteMateriasEjeTematico(dctx, ejeTematico);
          }
          catch (Exception e)
          {
              dctx.RollbackTransaction(miFirma);
              dctx.CloseConnection(miFirma);
              throw e;
          }
          dctx.CommitTransaction(miFirma);
          dctx.CloseConnection(miFirma);
      }
      /// <summary>
      /// Crea un objeto de EjeTematico a partir de los datos del último DataRow del DataSet.
      /// </summary>
      /// <param name="ds">El DataSet que contiene la información de EjeTematico</param>
      /// <returns>Un objeto de EjeTematico creado a partir de los datos</returns>
      public EjeTematico LastDataRowToEjeTematico(DataSet ds) {
         if (!ds.Tables.Contains("EjeTematico"))
            throw new Exception("LastDataRowToEjeTematico: DataSet no tiene la tabla EjeTematico");
         int index = ds.Tables["EjeTematico"].Rows.Count;
         if (index < 1)
            throw new Exception("LastDataRowToEjeTematico: El DataSet no tiene filas");
         return this.DataRowToEjeTematico(ds.Tables["EjeTematico"].Rows[index - 1]);
      }
      /// <summary>
      /// Crea un objeto de EjeTematico a partir de los datos de un DataRow.
      /// </summary>
      /// <param name="row">El DataRow que contiene la información de EjeTematico</param>
      /// <returns>Un objeto de EjeTematico creado a partir de los datos</returns>
      public EjeTematico DataRowToEjeTematico(DataRow row){
         EjeTematico ejeTematico = new EjeTematico();
         ejeTematico.AreaProfesionalizacion = new AreaProfesionalizacion();
         if (row.IsNull("EjeTematicoID"))
            ejeTematico.EjeTematicoID = null;
         else
            ejeTematico.EjeTematicoID = (long)Convert.ChangeType(row["EjeTematicoID"], typeof(long));
         if (row.IsNull("Nombre"))
            ejeTematico.Nombre = null;
         else
            ejeTematico.Nombre = (string)Convert.ChangeType(row["Nombre"], typeof(string));
         if (row.IsNull("Descripcion"))
            ejeTematico.Descripcion = null;
         else
            ejeTematico.Descripcion = (string)Convert.ChangeType(row["Descripcion"], typeof(string));
        
            ejeTematico.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
        
            ejeTematico.EstatusProfesionalizacion = (EEstatusProfesionalizacion)Convert.ChangeType(row["EstatusProfesionalizacion"], typeof(Byte));
        
          if (row.IsNull("AreaProfesionalizacionID"))
            ejeTematico.AreaProfesionalizacion.AreaProfesionalizacionID = null;
         else
            ejeTematico.AreaProfesionalizacion.AreaProfesionalizacionID = (int)Convert.ChangeType(row["AreaProfesionalizacionID"], typeof(int));
         return ejeTematico;
      }
   } 
}
