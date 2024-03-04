using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.DAO;
using POV.CentroEducativo.DA;

namespace POV.CentroEducativo.Service
{
    /// <summary>
    /// Controlador del objeto PlanEducativo
    /// </summary>
    public class PlanEducativoCtrl
    {
        /// <summary>
        /// Consulta registros de PlanEducativoRetHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="planEducativoRetHlp">PlanEducativoRetHlp que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de PlanEducativoRetHlp generada por la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, PlanEducativo planEducativo)
        {
            PlanEducativoRetHlp da = new PlanEducativoRetHlp();
            DataSet ds = da.Action(dctx, planEducativo);
            return ds;
        }
        /// <summary>
        /// Crea un registro de PlanEducativoInsHlp en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="planEducativoInsHlp">PlanEducativoInsHlp que desea crear</param>
        public void Insert(IDataContext dctx, PlanEducativo planEducativo)
        {
            PlanEducativoInsHlp da = new PlanEducativoInsHlp();
            da.Action(dctx, planEducativo);
        }
        /// <summary>
        /// Actualiza de manera optimista un registro de PlanEducativoUpdHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="planEducativoUpdHlp">PlanEducativoUpdHlp que tiene los datos nuevos</param>
        /// <param name="anterior">PlanEducativoUpdHlp que tiene los datos anteriores</param>
        public void Update(IDataContext dctx, PlanEducativo planEducativo, PlanEducativo previous)
        {
            PlanEducativoUpdHlp da = new PlanEducativoUpdHlp();
            da.Action(dctx, planEducativo, previous);
        }

        public void Delete(IDataContext dctx, PlanEducativo planEducativo)
        {
            PlanEducativoDelHlp da = new PlanEducativoDelHlp();
            da.Action(dctx, planEducativo);
        }

        /// <summary>
        /// Consultar un PlanEducativo con sus materias asignadas
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="planEducativo"></param>
        /// <returns></returns>
        public PlanEducativo RetriveComplete(IDataContext dctx, PlanEducativo planEducativo)
        {
            if (planEducativo == null)
                throw new Exception("RetriveComplete:PlanEducativo no puede ser nulo");
            if (planEducativo.PlanEducativoID == null)
                throw new Exception("RetriveComplete:PlanEducativoID no puede ser nulo");
            DataSet dsPlanEducativo = Retrieve(dctx, new PlanEducativo { PlanEducativoID = planEducativo.PlanEducativoID });
            if (dsPlanEducativo.Tables[0].Rows.Count != 1)
                throw new Exception("RetriveComplete:No se encontró el planEducativo proporcionado");

            PlanEducativo dplanEducativo = LastDataRowToPlanEducativo(dsPlanEducativo);
            List<Materia> lsMaterias = RetrivePlanEducativoMaterias(dctx, new PlanEducativo { PlanEducativoID = dplanEducativo.PlanEducativoID }, new Materia());

            NivelEducativoCtrl nivelEducativoCtrl = new NivelEducativoCtrl();
            dplanEducativo.NivelEducativo = nivelEducativoCtrl.RetriveComplete(dctx, new NivelEducativo { NivelEducativoID = dplanEducativo.NivelEducativo.NivelEducativoID });
            foreach (Materia lsMateria in lsMaterias)
            {
                dplanEducativo.MateriaAgregar(lsMateria);
            }
            return dplanEducativo;
        }

        public List<Materia> RetrivePlanEducativoMaterias(IDataContext dctx, PlanEducativo planEducativo, Materia materia)
        {
            if (planEducativo == null)
                throw new Exception("RetrivePlanEducativoMaterias:PlanEducativo no puede ser nulo");
            if (planEducativo.PlanEducativoID == null)
                throw new Exception("RetriveMaterias:PlanEducativoID no puede ser nulo");

            DataSet dsplan = Retrieve(dctx, planEducativo);
            if (dsplan.Tables[0].Rows.Count != 1)
                throw new Exception("RetrivePlanEducativoMaterias:PlanEducativo no puede se encontrar");

            List<Materia> lsMaterias = new List<Materia>();

            MateriaPlanEducativoRetHlp dda = new MateriaPlanEducativoRetHlp();
            DataSet ds = dda.Action(dctx, materia, new PlanEducativo { PlanEducativoID = planEducativo.PlanEducativoID }, null);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Materia dmateria = (new MateriaCtrl()).DataRowToMateria(dr);
                lsMaterias.Add(dmateria);
            }

            return lsMaterias;
        }

        public PlanEducativo RetrievePlanEducativo(IDataContext dctx, DateTime fecha)
        {
            PlanEducativo planEducativo = null;

            PlanEducativoRetHlp da = new PlanEducativoRetHlp();
            DataSet ds = da.ActionActual(dctx, fecha);

            if (ds.Tables[0].Rows.Count != 0)
                planEducativo = this.LastDataRowToPlanEducativo(ds);

            return planEducativo;
        }

        public void UpdatePlanEducativoMateria(IDataContext dctx, Materia materia, Materia materiaPrevia, PlanEducativo planEducativo, PlanEducativo planPrevio, Boolean status)
        {
            MateriaPlanEducativoUpdHlp da = new MateriaPlanEducativoUpdHlp();
            da.Action(dctx, materia, materiaPrevia, planEducativo, planPrevio, status);
        }

        public void InsertPlanEducativoMateria(IDataContext dctx, Materia materia, PlanEducativo planEducativo)
        {
            MateriaPlanEducativoInsHlp da = new MateriaPlanEducativoInsHlp();
            da.Action(dctx, materia, planEducativo);
        }

        public void InsertComplete(IDataContext dctx, PlanEducativo planEducativo)
        {
            MateriaCtrl materiaCtrl = new MateriaCtrl();
            object myFirm = new object();
            try
            {
                dctx.OpenConnection(myFirm);
                dctx.BeginTransaction(myFirm);
                planEducativo.Estatus = true;

                this.Insert(dctx, planEducativo);
                DataSet ds = Retrieve(dctx, planEducativo);
                PlanEducativo planTemp = LastDataRowToPlanEducativo(ds);
                planEducativo.PlanEducativoID = planTemp.PlanEducativoID;

                if (planEducativo.Materias != null)
                {
                    foreach (Materia mat in planEducativo.Materias)
                    {
                        materiaCtrl.InsertMateria(dctx, mat);
                    }
                }
                dctx.CommitTransaction(myFirm);

                if (planEducativo.Materias != null)
                {
                    foreach (Materia mat in planEducativo.Materias)
                    {
                        DataSet dsMat = materiaCtrl.Retrieve(dctx, mat);
                        Materia matTemp = materiaCtrl.LastDataRowToMateria(dsMat);
                        mat.MateriaID = matTemp.MateriaID;

                        InsertPlanEducativoMateria(dctx, mat, planEducativo);
                    }
                }
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

        public void UpdateComplete(IDataContext dctx, PlanEducativo planEducativo, DataTable dtRelacionesExistentes)
        {
            MateriaCtrl materiaCtrl = new MateriaCtrl();
            object myFirm = new object();
            try
            {
                dctx.OpenConnection(myFirm);
                dctx.BeginTransaction(myFirm);
                PlanEducativo planOriginal = new PlanEducativo();
                planOriginal.PlanEducativoID = planEducativo.PlanEducativoID;
                planOriginal = LastDataRowToPlanEducativo(Retrieve(dctx, planOriginal));

                Update(dctx, planEducativo, planOriginal);

                if (planEducativo.Materias != null)
                {
                    foreach (Materia mat in planEducativo.Materias)
                    {
                        if (mat.MateriaID == null)
                        {
                            materiaCtrl.InsertMateria(dctx, mat);
                        }
                        else
                        {
                            Materia matOriginal = new Materia();
                            matOriginal.MateriaID = mat.MateriaID;
                            matOriginal = materiaCtrl.LastDataRowToMateria(materiaCtrl.Retrieve(dctx, matOriginal));
                            materiaCtrl.Update(dctx, mat, matOriginal);

                            foreach (DataRow data in dtRelacionesExistentes.Rows)
                            {
                                bool? status = null;
                                if (data["Clave"].ToString() == mat.Clave)
                                {
                                    if (data["Estatus"] == "Desactivada")
                                        status = false;
                                    else
                                        status = true;
                                    this.UpdatePlanEducativoMateria(dctx, mat, matOriginal, planEducativo, planOriginal, status.Value);
                                }
                            }
                        }
                    }
                }
                dctx.CommitTransaction(myFirm);

                if (planEducativo.Materias != null)
                {
                    foreach (Materia mat in planEducativo.Materias)
                    {
                        if (mat.MateriaID == null)
                        {
                            DataSet dsMat = materiaCtrl.Retrieve(dctx, mat);
                            Materia matTemp = materiaCtrl.LastDataRowToMateria(dsMat);
                            mat.MateriaID = matTemp.MateriaID;

                            InsertPlanEducativoMateria(dctx, mat, planEducativo);
                        }
                    }
                }
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
        /// Crea un objeto de PlanEducativo a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de PlanEducativo</param>
        /// <returns>Un objeto de PlanEducativo creado a partir de los datos</returns>
        public PlanEducativo LastDataRowToPlanEducativo(DataSet ds)
        {
            if (!ds.Tables.Contains("PlanEducativo"))
                throw new Exception("LastDataRowToPlanEducativo: DataSet no tiene la tabla PlanEducativo");
            int index = ds.Tables["PlanEducativo"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToPlanEducativo: El DataSet no tiene filas");
            return this.DataRowToPlanEducativo(ds.Tables["PlanEducativo"].Rows[index - 1]);
        }
        /// <summary>
        /// Crea un objeto de PlanEducativo a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de PlanEducativo</param>
        /// <returns>Un objeto de PlanEducativo creado a partir de los datos</returns>
        public PlanEducativo DataRowToPlanEducativo(DataRow row)
        {
            PlanEducativo planEducativo = new PlanEducativo();
            if (planEducativo.NivelEducativo == null)
            {
                planEducativo.NivelEducativo = new NivelEducativo();
            }
            if (row.IsNull("PlanEducativoID"))
                planEducativo.PlanEducativoID = null;
            else
                planEducativo.PlanEducativoID = (int)Convert.ChangeType(row["PlanEducativoID"], typeof(int));
            if (row.IsNull("Titulo"))
                planEducativo.Titulo = null;
            else
                planEducativo.Titulo = (string)Convert.ChangeType(row["Titulo"], typeof(string));
            if (row.IsNull("Descripcion"))
                planEducativo.Descripcion = null;
            else
                planEducativo.Descripcion = (string)Convert.ChangeType(row["Descripcion"], typeof(string));
            if (row.IsNull("ValidoDesde"))
                planEducativo.ValidoDesde = null;
            else
                planEducativo.ValidoDesde = (DateTime)Convert.ChangeType(row["ValidoDesde"], typeof(DateTime));
            if (row.IsNull("ValidoHasta"))
                planEducativo.ValidoHasta = null;
            else
                planEducativo.ValidoHasta = (DateTime)Convert.ChangeType(row["ValidoHasta"], typeof(DateTime));
            if (row.IsNull("NivelEducativoID"))
                planEducativo.NivelEducativo.NivelEducativoID = null;
            else
                planEducativo.NivelEducativo.NivelEducativoID = (int)Convert.ChangeType(row["NivelEducativoID"], typeof(int));
            if (row.IsNull("Estatus"))
                planEducativo.Estatus = null;
            else
                planEducativo.Estatus = (Boolean)Convert.ChangeType(row["Estatus"], typeof(Boolean));
            return planEducativo;
        }
    }
}
