using System;
using System.Collections.Generic;
using System.Data;
using Framework.Base.DataAccess;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Comun.BO;
using POV.Profesionalizacion.BO;
using POV.Profesionalizacion.DAO;
using System.Linq;

namespace POV.Profesionalizacion.Service { 
    /// <summary>
    /// Controlador del objeto AreaProfesionalizacion
    /// </summary>
    public class AreaProfesionalizacionCtrl
    {
        #region AreaProfesionalizacion
        /// <summary>
        /// Consulta registros de AreaProfesionalizacionRetHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="areaProfesionalizacionRetHlp">AreaProfesionalizacionRetHlp que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de AreaProfesionalizacionRetHlp generada por la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, AreaProfesionalizacion areaProfesionalizacion){
            AreaProfesionalizacionRetHlp da = new AreaProfesionalizacionRetHlp();
            DataSet ds = da.Action(dctx, areaProfesionalizacion);
            return ds;
        }
        /// <summary>
        /// Consulta registros completos de AreaProfesionalizacion en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="areaProfesionalizacion">AreaProfesionalizacion que provee los datos del criterio para realizar la consulta.</param>
        /// <returns></returns>
        public AreaProfesionalizacion RetrieveComplete(IDataContext dctx, AreaProfesionalizacion areaProfesionalizacion)
        {
            AreaProfesionalizacion areaProfesionalizacionComplete = null;
            if(areaProfesionalizacion == null) throw new Exception("El objeto área profesionalización no puede ser nulo.");
            
            DataSet ds = Retrieve(dctx, areaProfesionalizacion);
            areaProfesionalizacionComplete = LastDataRowToAreaProfesionalizacion(ds);
            DataSet dsMaterias = MateriaProfesionalizacionRetrieve(dctx, new MateriaProfesionalizacion(), areaProfesionalizacionComplete);

            if (dsMaterias.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drMateriaP in dsMaterias.Tables[0].Rows)
                {
                    MateriaProfesionalizacion materiaProfesionalizacion = new MateriaProfesionalizacion();
                    materiaProfesionalizacion.MateriaID = Convert.ToInt32(drMateriaP["MateriaID"]);
                    materiaProfesionalizacion.Nombre = drMateriaP["Nombre"].ToString();
                    materiaProfesionalizacion.FechaRegistro = Convert.ToDateTime(drMateriaP["FechaRegistro"]);
                    areaProfesionalizacionComplete.MateriaProfesionalizacionAgregar(materiaProfesionalizacion);
                    materiaProfesionalizacion.Activo = Convert.ToBoolean(drMateriaP["Activo"]);
                }
            }
            NivelEducativoCtrl nivelEducativoCtrl = new NivelEducativoCtrl();
            areaProfesionalizacionComplete.NivelEducativo = nivelEducativoCtrl.RetriveComplete(dctx, new NivelEducativo { NivelEducativoID = areaProfesionalizacionComplete.NivelEducativo.NivelEducativoID });
            return areaProfesionalizacionComplete;
        }
        /// <summary>
        /// Crea un registro de AreaProfesionalizacionInsHlp en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="areaProfesionalizacionInsHlp">AreaProfesionalizacionInsHlp que desea crear</param>
        public void Insert(IDataContext dctx, AreaProfesionalizacion  areaProfesionalizacion){
            AreaProfesionalizacionInsHlp da = new AreaProfesionalizacionInsHlp();
            da.Action(dctx,  areaProfesionalizacion);
        }
        /// <summary>
        /// Actualiza de manera optimista un registro de AreaProfesionalizacionUpdHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="areaProfesionalizacionUpdHlp">AreaProfesionalizacionUpdHlp que tiene los datos nuevos</param>
        /// <param name="anterior">AreaProfesionalizacionUpdHlp que tiene los datos anteriores</param>
        public void Update(IDataContext dctx, AreaProfesionalizacion  areaProfesionalizacion, AreaProfesionalizacion previous){
            AreaProfesionalizacionUpdHlp da = new AreaProfesionalizacionUpdHlp();
            da.Action(dctx,  areaProfesionalizacion, previous);
        }
        /// <summary>
        /// Crea un objeto de AreaProfesionalizacion a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de AreaProfesionalizacion</param>
        /// <returns>Un objeto de AreaProfesionalizacion creado a partir de los datos</returns>
        public AreaProfesionalizacion LastDataRowToAreaProfesionalizacion(DataSet ds) {
            if (!ds.Tables.Contains("AreaProfesionalizacion"))
            throw new Exception("LastDataRowToAreaProfesionalizacion: DataSet no tiene la tabla AreaProfesionalizacion");
            int index = ds.Tables["AreaProfesionalizacion"].Rows.Count;
            if (index < 1)
            throw new Exception("LastDataRowToAreaProfesionalizacion: El DataSet no tiene filas");
            return this.DataRowToAreaProfesionalizacion(ds.Tables["AreaProfesionalizacion"].Rows[index - 1]);
        }
        /// <summary>
        /// Crea un objeto de AreaProfesionalizacion a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de AreaProfesionalizacion</param>
        /// <returns>Un objeto de AreaProfesionalizacion creado a partir de los datos</returns>
        public AreaProfesionalizacion DataRowToAreaProfesionalizacion(DataRow row){
            AreaProfesionalizacion areaProfesionalizacion = new AreaProfesionalizacion();
            if (areaProfesionalizacion.NivelEducativo == null)
            {
                areaProfesionalizacion.NivelEducativo = new NivelEducativo();
            }
            if (row.IsNull("AreaProfesionalizacionID"))
            areaProfesionalizacion.AreaProfesionalizacionID = null;
            else
            areaProfesionalizacion.AreaProfesionalizacionID = (int)Convert.ChangeType(row["AreaProfesionalizacionID"], typeof(int));
            if (row.IsNull("Nombre"))
            areaProfesionalizacion.Nombre = null;
            else
            areaProfesionalizacion.Nombre = (string)Convert.ChangeType(row["Nombre"], typeof(string));
            if (row.IsNull("Descripcion"))
            areaProfesionalizacion.Descripcion = null;
            else
            areaProfesionalizacion.Descripcion = (string)Convert.ChangeType(row["Descripcion"], typeof(string));
            if (row.IsNull("FechaRegistro"))
            areaProfesionalizacion.FechaRegistro = null;
            else
            areaProfesionalizacion.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
            if (row.IsNull("Activo"))
            areaProfesionalizacion.Activo = null;
            else
            areaProfesionalizacion.Activo = (bool)Convert.ChangeType(row["Activo"], typeof(bool));
            if (row.IsNull("NivelEducativoID"))
                areaProfesionalizacion.NivelEducativo.NivelEducativoID = null;
            else
                areaProfesionalizacion.NivelEducativo.NivelEducativoID = (int)Convert.ChangeType(row["NivelEducativoID"], typeof(int));
            if (row.IsNull("Grado"))
                areaProfesionalizacion.Grado = null;
            else
                areaProfesionalizacion.Grado = (byte)Convert.ChangeType(row["Grado"], typeof(byte));
            return areaProfesionalizacion;
        }   
        #endregion

        #region MateriaProfesionalizacion
        /// <summary>
        /// Consulta registros de MateriaProfesionalizacionRetHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="materiaProfesionalizacionRetHlp">MateriaProfesionalizacionRetHlp que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de MateriaProfesionalizacionRetHlp generada por la consulta</returns>
        public DataSet MateriaProfesionalizacionRetrieve(IDataContext dctx, MateriaProfesionalizacion materiaProfesionalizacion, AreaProfesionalizacion areaProfesionalizacion)
        {
            MateriaProfesionalizacionRetHlp da = new MateriaProfesionalizacionRetHlp();
            DataSet ds = da.Action(dctx, materiaProfesionalizacion, areaProfesionalizacion);
            return ds;
        }
        public MateriaProfesionalizacion MateriaProfesionalizacionRetrieve(IDataContext dctx,
                                                                           MateriaProfesionalizacion
                                                                               materiaProfesionalizacion)
        {
            MateriaProfesionalizacion materiaProfesionalizacionComplete = new MateriaProfesionalizacion();
            if (materiaProfesionalizacion == null) throw new Exception("El objeto materia profesionalización no puede ser nulo.");

            DataSet ds = MateriaProfesionalizacionRetrieve(dctx, materiaProfesionalizacion, new AreaProfesionalizacion());
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    materiaProfesionalizacionComplete.MateriaID = Convert.ToInt32(row["MateriaID"]);
                    materiaProfesionalizacionComplete.Nombre = row["Nombre"].ToString();
                    materiaProfesionalizacionComplete.FechaRegistro = Convert.ToDateTime(row["FechaRegistro"]);
                    materiaProfesionalizacionComplete.Activo = Convert.ToBoolean(row["Activo"]);
                }
            }
            return materiaProfesionalizacionComplete;
        }
        /// <summary>
        /// Crea un registro de MateriaProfesionalizacionInsHlp en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="materiaProfesionalizacionInsHlp">MateriaProfesionalizacionInsHlp que desea crear</param>
        public void MateriaProfesionalizacionInsert(IDataContext dctx, MateriaProfesionalizacion materiaProfesionalizacion, AreaProfesionalizacion areaProfesionalizacion)
        {
            MateriaProfesionalizacionInsHlp da = new MateriaProfesionalizacionInsHlp();
            da.Action(dctx, materiaProfesionalizacion, areaProfesionalizacion);
        }
        /// <summary>
        /// Actualiza de manera optimista un registro de MateriaProfesionalizacionUpdHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="materiaProfesionalizacionUpdHlp">MateriaProfesionalizacionUpdHlp que tiene los datos nuevos</param>
        /// <param name="anterior">MateriaProfesionalizacionUpdHlp que tiene los datos anteriores</param>
        public void MateriaProfesionalizacionUpdate(IDataContext dctx, MateriaProfesionalizacion materiaProfesionalizacion, MateriaProfesionalizacion previous, AreaProfesionalizacion areaProfesionalizacion)
        {
            MateriaProfesionalizacionUpdHlp da = new MateriaProfesionalizacionUpdHlp();
            da.Action(dctx, materiaProfesionalizacion, previous, areaProfesionalizacion);
        }

        /// <summary>
        /// Crea un objeto de MateriaProfesionalizacion a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de MateriaProfesionalizacion</param>
        /// <returns>Un objeto de MateriaProfesionalizacion creado a partir de los datos</returns>
        public MateriaProfesionalizacion LastDataRowToMateriaProfesionalizacion(DataSet ds)
        {
            if (!ds.Tables.Contains("MateriaProfesionalizacion"))
                throw new Exception("LastDataRowToMateriaProfesionalizacion: DataSet no tiene la tabla MateriaProfesionalizacion");
            int index = ds.Tables["MateriaProfesionalizacion"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToMateriaProfesionalizacion: El DataSet no tiene filas");
            return this.DataRowToMateriaProfesionalizacion(ds.Tables["MateriaProfesionalizacion"].Rows[index - 1]);
        }
        /// <summary>
        /// Crea un objeto de MateriaProfesionalizacion a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de MateriaProfesionalizacion</param>
        /// <returns>Un objeto de MateriaProfesionalizacion creado a partir de los datos</returns>
        public MateriaProfesionalizacion DataRowToMateriaProfesionalizacion(DataRow row)
        {
            MateriaProfesionalizacion materiaProfesionalizacion = new MateriaProfesionalizacion();
            if (row.IsNull("MateriaID"))
                materiaProfesionalizacion.MateriaID = null;
            else
                materiaProfesionalizacion.MateriaID = (int)Convert.ChangeType(row["MateriaID"], typeof(int));
            if (row.IsNull("Nombre"))
                materiaProfesionalizacion.Nombre = null;
            else
                materiaProfesionalizacion.Nombre = (string)Convert.ChangeType(row["Nombre"], typeof(string));
            if (row.IsNull("FechaRegistro"))
                materiaProfesionalizacion.FechaRegistro = null;
            else
                materiaProfesionalizacion.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
            if (row.IsNull("Activo"))
                materiaProfesionalizacion.Activo = null;
            else
                materiaProfesionalizacion.Activo = (bool)Convert.ChangeType(row["Activo"], typeof(bool));
            return materiaProfesionalizacion;
        }   
        #endregion

        #region Métodos completos de AreaProfesionalizacion y MateriaProfesionalizacion.
        /// <summary>
        /// Inserta un nuevo registro completo de AreProfesionalizacion con su(s) respectiva(s) MateriasProfesionalizacion.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="areaProfesionalizacion">Contiene los datos a insertar de AreProfesionalizacion</param>
        /// <param name="materiaProfesionalizacion">Contiene los datos a insertar de MateriaProfesionalizacion</param>
        public void InsertComplete(IDataContext dctx, AreaProfesionalizacion areaProfesionalizacion)
        {
            AreaProfesionalizacionCtrl areaProfesionalizacionCtrl = new AreaProfesionalizacionCtrl();
            AreaProfesionalizacionCtrl materiaProfesionalizacionCtrl = new AreaProfesionalizacionCtrl();

            this.Insert(dctx, areaProfesionalizacion);

            AreaProfesionalizacion areaProfesionalizacionResult =
                LastDataRowToAreaProfesionalizacion(this.Retrieve(dctx, areaProfesionalizacion));

            foreach (MateriaProfesionalizacion materia in areaProfesionalizacion.MateriasProfesionalizacion)
            {
                MateriaProfesionalizacion materiaInsert = new MateriaProfesionalizacion();
                DataSet ds = areaProfesionalizacionCtrl.MateriaProfesionalizacionRetrieve(dctx, materia, areaProfesionalizacionResult);
                if (ds.Tables[0].Rows.Count == 1)
                {
                    DataRow drMateria = ds.Tables[0].Rows[0];
                    materiaInsert.MateriaID = Convert.ToInt32(drMateria["MateriaID"]);
                    materiaInsert.Nombre = drMateria["Nombre"].ToString();
                    materiaInsert.FechaRegistro = Convert.ToDateTime(drMateria["FechaRegistro"]);

                }
                else
                {
                    materiaInsert.Nombre = materia.Nombre;
                    materiaInsert.FechaRegistro = DateTime.Now;
                    materiaInsert.Activo = true;
                }
                materiaProfesionalizacionCtrl.MateriaProfesionalizacionInsert(dctx, materiaInsert, areaProfesionalizacionResult);
            }
        }
        /// <summary>
        /// Actualiza el registro de AreaProfesionalizacion o su(s) respectiva(s) MateriaProfesionalizacion.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="areaProfesionalizacion">Los nuevos datos con los que se actualizará el AreaProfesionalizacion</param>
        /// <param name="apprevious">El registro original de AreaProfesionalizacion que se actualizará.</param>
        /// <param name="materiaProfesionalizacion">Los nuevos datos con los que se actualizará la MateriaProfesionalizacion</param>
        /// <param name="mpprevious">El registro original de MateriaProfesionalizacion que se actualizará.</param>
        public void UpdateComplete(IDataContext dctx, AreaProfesionalizacion areaProfesionalizacion,
                                   AreaProfesionalizacion apprevious)
        {
            this.Update(dctx, areaProfesionalizacion, apprevious);
            this.UpdateMateriasProfesionalizacion(dctx, areaProfesionalizacion, apprevious);
        }
        /// <summary>
        /// Método que actualiza cada una de la(s) MateriaProfesionalizacion pertenecientes al AreaProfesionalizacion indicado.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="areaProfesionalizacion">AreaProfesionalizacion que contiene los nuevos datos de la MateriaProfesionalizacion</param>
        /// <param name="apprevious">AreaProfesionalizacion anterior que contiene los datos originales de la MateriaProfesionalizacion</param>
        public void UpdateMateriasProfesionalizacion(IDataContext dctx, AreaProfesionalizacion areaProfesionalizacion, AreaProfesionalizacion apprevious)
        {
            MateriaProfesionalizacionUpdHlp areaProfesionalizacionUpdHlp = new MateriaProfesionalizacionUpdHlp();
            MateriaProfesionalizacionInsHlp areaProfesionalizacionInsHlp = new MateriaProfesionalizacionInsHlp();

            foreach (MateriaProfesionalizacion materiaProfesionalizacion in areaProfesionalizacion.MateriasProfesionalizacion)
            {
                DataSet ds = this.MateriaProfesionalizacionRetrieve(dctx,
                                                                    new MateriaProfesionalizacion()
                                                                        {
                                                                            MateriaID =
                                                                                Convert.ToInt32(
                                                                                    materiaProfesionalizacion.MateriaID)
                                                                        },
                                                                    apprevious);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    MateriaProfesionalizacion materiacont = this.LastDataRowToMateriaProfesionalizacion(ds);
                    if (materiacont.MateriaID != null)
                    {
                        if (materiaProfesionalizacion.MateriaEstado == EObjetoEstado.ELIMINADO)
                        {
                            materiaProfesionalizacion.Activo = false;
                            areaProfesionalizacionUpdHlp.Action(dctx, materiaProfesionalizacion, materiacont, areaProfesionalizacion);
                        }
                        else
                        {
                            areaProfesionalizacionUpdHlp.Action(dctx, materiaProfesionalizacion, materiacont, areaProfesionalizacion);
                        }
                    }
                }//Nuevo
                else
                {
                        areaProfesionalizacionInsHlp.Action(dctx, materiaProfesionalizacion, areaProfesionalizacion);                    
                }
            }
        }
        public void UpdateMateriaProfesionalizacion(IDataContext dctx,
                                                    MateriaProfesionalizacion materiaProfesionalizacion,
                                                    MateriaProfesionalizacion previous,
                                                    AreaProfesionalizacion areaProfesionalizacion)
        {
            MateriaProfesionalizacionUpdHlp da = new MateriaProfesionalizacionUpdHlp();
            da.Action(dctx,materiaProfesionalizacion, previous,areaProfesionalizacion);
        }
        /// <summary>
        /// Método que hace la baja lógica del AreaProfesionalizacion con su(s) respectiva(s) MateriaProfesionalizacion.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="areaProfesionalizacion">AreaProfesionalizacion que se desea dar de baja.</param>
        public void DeleteComplete(IDataContext dctx, AreaProfesionalizacion areaProfesionalizacion)
        {
            
            this.Update(dctx,areaProfesionalizacion,new AreaProfesionalizacion
                {
                    AreaProfesionalizacionID = areaProfesionalizacion.AreaProfesionalizacionID,
                    Nombre = areaProfesionalizacion.Nombre,
                    Descripcion = areaProfesionalizacion.Descripcion,
                    Activo = true,
                    NivelEducativo = areaProfesionalizacion.NivelEducativo,
                    Grado = areaProfesionalizacion.Grado
                });
            this.DeleteMateriasProfesionalizacion(dctx,areaProfesionalizacion);

        }
        /// <summary>
        /// Método que hace la baja lógica de cada MateriaProfesionalizacion perteneciente al AreaProfesionalizacion.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="areaProfesionalizacion">AreaProfesionalizacion que contiene la(s) MateriaProfesionalizacion a dar de baja.</param>
        public void DeleteMateriasProfesionalizacion(IDataContext dctx, AreaProfesionalizacion areaProfesionalizacion)
        {
            AreaProfesionalizacion areaProfesionalizacionprevious = areaProfesionalizacion;
            areaProfesionalizacionprevious.Activo = true;
            this.UpdateMateriasProfesionalizacion(dctx, areaProfesionalizacion, areaProfesionalizacionprevious);
        }
        #endregion
    }
}
