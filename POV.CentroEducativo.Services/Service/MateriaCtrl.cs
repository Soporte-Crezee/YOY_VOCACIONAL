using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.DAO;
using POV.Modelo.Estandarizado.BO;
using POV.Modelo.Estandarizado.Service;

namespace POV.CentroEducativo.Service { 
   /// <summary>
   /// Controlador del objeto Materia
   /// </summary>
   public class MateriaCtrl { 
      /// <summary>
      /// Consulta registros de MateriaRetHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="materiaRetHlp">MateriaRetHlp que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de MateriaRetHlp generada por la consulta</returns>
      public DataSet Retrieve(IDataContext dctx, Materia materia){
         MateriaRetHlp da = new MateriaRetHlp();
         DataSet ds = da.Action(dctx, materia);
         return ds;
      }

      /// <summary>
      /// Actualiza de manera optimista un registro de MateriaUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="materiaUpdHlp">MateriaUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">MateriaUpdHlp que tiene los datos anteriores</param>
      public void Update(IDataContext dctx, Materia materia, Materia previous)
      {
          MateriaUpdHlp da = new MateriaUpdHlp();
          da.Action(dctx, materia, previous);
      }

      /// <summary>
      /// Crea un objeto de Materia a partir de los datos del último DataRow del DataSet.
      /// </summary>
      /// <param name="ds">El DataSet que contiene la información de Materia</param>
      /// <returns>Un objeto de Materia creado a partir de los datos</returns>
      public Materia LastDataRowToMateria(DataSet ds) {
         if (!ds.Tables.Contains("Materia"))
            throw new Exception("LastDataRowToMateria: DataSet no tiene la tabla Materia");
         int index = ds.Tables["Materia"].Rows.Count;
         if (index < 1)
            throw new Exception("LastDataRowToMateria: El DataSet no tiene filas");
         return this.DataRowToMateria(ds.Tables["Materia"].Rows[index - 1]);
      }
      /// <summary>
      /// Crea un objeto de Materia a partir de los datos de un DataRow.
      /// </summary>
      /// <param name="row">El DataRow que contiene la información de Materia</param>
      /// <returns>Un objeto de Materia creado a partir de los datos</returns>
      public Materia DataRowToMateria(DataRow row){
         Materia materia = new Materia();
          materia.AreaAplicacion = new AreaAplicacion();
         if (row.IsNull("MateriaID"))
            materia.MateriaID = null;
         else
            materia.MateriaID = (int)Convert.ChangeType(row["MateriaID"], typeof(int));
         if (row.IsNull("Clave"))
            materia.Clave = null;
         else
            materia.Clave = (string)Convert.ChangeType(row["Clave"], typeof(string));
         if (row.IsNull("Titulo"))
            materia.Titulo = null;
         else
            materia.Titulo = (string)Convert.ChangeType(row["Titulo"], typeof(string));
         if (row.IsNull("Grado"))
            materia.Grado = null;
         else
            materia.Grado = (byte)Convert.ChangeType(row["Grado"], typeof(byte));
         if (row.IsNull("AreaAplicacionID"))
            materia.AreaAplicacion.AreaAplicacionID = null;
         else
            materia.AreaAplicacion.AreaAplicacionID = (int)Convert.ChangeType(row["AreaAplicacionID"], typeof(int));
         return materia;
      }

       /// <summary>
       /// Consulta una materia con su área de aplicación
       /// </summary>
       /// <param name="dctx">El DataContext que dará acceso a la base de datos</param>
       /// <param name="materia">materia que desea consultar</param>
       /// <returns>materia consultada</returns>
       public Materia RetriveComplete(IDataContext dctx,Materia materia)
       {
           if(materia==null)
               throw new Exception("RetriveComplete:Materia no puede ser nulo");
           if(materia.MateriaID==null)
               throw new Exception("RetriveComplete:MateriaID no puede ser nulo");
           DataSet ds = Retrieve(dctx, new Materia {MateriaID = materia.MateriaID});
           if (ds.Tables[0].Rows.Count != 1)
               throw new Exception(
                   "RetriveComplete:la Materia solicitada no fue encontrada o se encontró más de un resultado");
           Materia dMateria = LastDataRowToMateria(ds);
           ds.Clear();
           AreaAplicacionCtrl areaAplicacionCtrl = new AreaAplicacionCtrl();
           ds = areaAplicacionCtrl.Retrieve(dctx,  new AreaAplicacion   {AreaAplicacionID = dMateria.AreaAplicacion.AreaAplicacionID});

           if (ds.Tables[0].Rows.Count == 1)
               dMateria.AreaAplicacion = areaAplicacionCtrl.LastDataRowToAreaAplicacion(ds);
           return dMateria;
       }

       public DataSet retrieveMateriaPlanEducativo(IDataContext dctx, Materia materia, PlanEducativo planEducativo, bool? estatus)
       {
           MateriaPlanEducativoRetHlp dda = new MateriaPlanEducativoRetHlp();
           DataSet ds = dda.Action(dctx, materia, planEducativo, estatus);
           return ds;
       }

       public void InsertMateria(IDataContext dctx ,Materia materia)
       {       
           //Insertar Materia de un planEducativo
           MateriaInsHlp materiaInsHlp = new MateriaInsHlp();
           materiaInsHlp.Action(dctx,materia);

       }
   } 
}
