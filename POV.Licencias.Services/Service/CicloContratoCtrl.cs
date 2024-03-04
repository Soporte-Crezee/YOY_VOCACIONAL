using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Licencias.BO;
using POV.Licencias.DAO;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Localizacion.Service;
using POV.Localizacion.BO;

namespace POV.Licencias.Service
{
    /// <summary>
    /// Controlador del objeto CicloContrato
    /// </summary>
    public class CicloContratoCtrl
    {
        /// <summary>
        /// Consulta registros de CicloContratoRetHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="cicloContratoRetHlp">CicloContratoRetHlp que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de CicloContratoRetHlp generada por la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, Contrato contrato, CicloContrato cicloContrato)
        {
            CicloContratoRetHlp da = new CicloContratoRetHlp();
            DataSet ds = da.Action(dctx, contrato, cicloContrato);
            return ds;
        }
        /// <summary>
        /// Consulta un registro completo de ciclo contrato, en caso de no encontrarlo devuelve null
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="contrato">Contrato relacionado al ciclo contrato</param>
        /// <param name="cicloContrato">Ciclo Contrato que se desea buscar</param>
        /// <returns>Registro completo de ciclo contrato, null en caso de no encontrarlo</returns>
        public CicloContrato RetrieveComplete(IDataContext dctx, Contrato contrato, CicloContrato cicloContrato)
        {
            if (contrato == null) throw new Exception("El contrato no puede ser nulo.");
            if (cicloContrato == null) throw new Exception("El ciclo contrato no puede ser nulo");

            CicloContrato cicloComplete = null;

            DataSet dsCiclo = Retrieve(dctx, contrato, cicloContrato);

            if (dsCiclo.Tables[0].Rows.Count > 0)
            {
                cicloComplete = LastDataRowToCicloContrato(dsCiclo);

                //Informacion de ciclo escolar
                CicloEscolarCtrl cicloEscolarCtrl = new CicloEscolarCtrl();
                cicloComplete.CicloEscolar = cicloEscolarCtrl.RetrieveComplete(dctx, cicloComplete.CicloEscolar);


                //informacion del recurso del contrato
                RecursoContratoCtrl recursoContratoCtrl = new RecursoContratoCtrl();
                cicloComplete.RecursoContrato = recursoContratoCtrl.RetrieveComplete(dctx, cicloComplete, new RecursoContrato());

            }

            return cicloComplete;
        }
        /// <summary>
        /// Consulta la lista completa de los registros de ciclo contrato de un contrato
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="contrato">Contrato</param>
        /// <param name="soloActivos">Opcional: indica si solo consulta los activos</param>
        /// <returns>Lista completa de ciclo contrato</returns>
        public List<CicloContrato> RetrieveListCicloContratoComplete(IDataContext dctx, Contrato contrato, bool soloActivos = true)
        {
            if (contrato == null) throw new Exception("El contrato no puede ser nulo");
            if (contrato.ContratoID == null) throw new Exception("El identificador del contrato no puede ser nulo");

            List<CicloContrato> ciclosContrato = new List<CicloContrato>();

            //filtro de activos o ambos (activos/inactivos)
            CicloContrato cicloContrato = new CicloContrato();
            if (soloActivos)
                cicloContrato.Activo = true;

            DataSet dsCiclos = Retrieve(dctx, contrato, cicloContrato);
            if (dsCiclos.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drCiclo in dsCiclos.Tables[0].Rows)
                {
                    CicloContrato cicloComplete = new CicloContrato { CicloContratoID = DataRowToCicloContrato(drCiclo).CicloContratoID };
                    cicloComplete = RetrieveComplete(dctx, contrato, cicloComplete);
                    ciclosContrato.Add(cicloComplete);
                }
            }

            return ciclosContrato;
        }

        /// <summary>
        /// Crea un registro de CicloContratoInsHlp en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="cicloContratoInsHlp">CicloContratoInsHlp que desea crear</param>
        private void Insert(IDataContext dctx, Contrato contrato, CicloContrato cicloContrato)
        {
            CicloContratoInsHlp da = new CicloContratoInsHlp();
            da.Action(dctx, contrato, cicloContrato);
        }
        /// <summary>
        /// Inserta un registro completo de ciclo contrato en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="contrato">Contrato relacionado al ciclo</param>
        /// <param name="cicloContrato">Registro de ciclo contrato completo que se desea insertar</param>
        public void InsertComplete(IDataContext dctx, Contrato contrato, CicloContrato cicloContrato)
        {
            if (contrato == null) throw new Exception("El contrato no puede ser nulo");
            if (contrato.ContratoID == null) throw new Exception("El identificador del contrato no puede ser nulo");
            if (cicloContrato == null) throw new Exception("El ciclo contrato no puede ser nulo");
            if (cicloContrato.CicloEscolar == null) throw new Exception("El ciclo escolar no puede ser nulo");
            if (cicloContrato.CicloEscolar.UbicacionID == null) throw new Exception("La ubicacion del ciclo escolar no puede ser nulo");
            if (cicloContrato.RecursoContrato == null) throw new Exception("El recurso contrato no puede ser nulo");


            if (DateTime.Compare(cicloContrato.CicloEscolar.InicioCiclo.Value,cicloContrato.CicloEscolar.FinCiclo.Value) >= 0)
                throw new Exception("El inicio del ciclo no puede ser mayor o igual que el fin del ciclo");

            object myFirm = new object();
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            try
            {
                ValidateCicloContrato(dctx, contrato, cicloContrato);
                RecursoContratoCtrl recursoContratoCtrl = new RecursoContratoCtrl();
                CicloEscolarCtrl cicloEscolarCtrl = new CicloEscolarCtrl();
                UbicacionCtrl ubicacionCtrl = new UbicacionCtrl();

                DataSet ds = ubicacionCtrl.RetrieveExacto(dctx, cicloContrato.CicloEscolar.UbicacionID);
                if (ds.Tables[0].Rows.Count == 1)
                    cicloContrato.CicloEscolar.UbicacionID = ubicacionCtrl.LastDataRowToUbicacion(ds);
                else
                {
                    cicloContrato.CicloEscolar.UbicacionID.FechaRegistro = DateTime.Now;
                    ubicacionCtrl.Insert(dctx, cicloContrato.CicloEscolar.UbicacionID);
                    DataSet dsUbicacion = ubicacionCtrl.RetrieveExacto(dctx, cicloContrato.CicloEscolar.UbicacionID);
                    cicloContrato.CicloEscolar.UbicacionID = ubicacionCtrl.LastDataRowToUbicacion(dsUbicacion);
                }

                cicloEscolarCtrl.Insert(dctx, cicloContrato.CicloEscolar);

                cicloContrato.CicloEscolar.CicloEscolarID = cicloEscolarCtrl.LastDataRowToCicloEscolar(cicloEscolarCtrl.Retrieve(dctx, cicloContrato.CicloEscolar)).CicloEscolarID;

                Insert(dctx, contrato, cicloContrato);

                cicloContrato.CicloContratoID = LastDataRowToCicloContrato(Retrieve(dctx, contrato, cicloContrato)).CicloContratoID;

                recursoContratoCtrl.Insert(dctx, cicloContrato, cicloContrato.RecursoContrato);

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
        /// Actualiza de manera optimista un registro de CicloContratoUpdHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="cicloContratoUpdHlp">CicloContratoUpdHlp que tiene los datos nuevos</param>
        /// <param name="anterior">CicloContratoUpdHlp que tiene los datos anteriores</param>
        private void Update(IDataContext dctx, CicloContrato cicloContrato, CicloContrato previous)
        {
            CicloContratoUpdHlp da = new CicloContratoUpdHlp();
            da.Action(dctx, cicloContrato, previous);
        }
        /// <summary>
        /// Actualiza de manera optimista un ciclo contrato en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="contrato">Contrato</param>
        /// <param name="cicloContrato">CicloContrato</param>
        /// <param name="previous">Ciclocontrato previo</param>
        public void UpdateComplete(IDataContext dctx, Contrato contrato, CicloContrato cicloContrato, CicloContrato previous)
        {
            if (contrato == null) throw new Exception("El contrato no puede ser nulo");
            if (contrato.ContratoID == null) throw new Exception("El identificador del contrato no puede ser nulo");
            if (previous == null) throw new Exception("El ciclo contrato previo no puede ser nulo");
            if (previous.CicloContratoID == null) throw new Exception("El identificador del ciclo contrato previo no puede ser nulo");

            if (previous.CicloEscolar == null) throw new Exception("El ciclo escolar no puede ser nulo");
            if (previous.CicloEscolar.CicloEscolarID == null) throw new Exception("El identificador del ciclo escolar previo no puede ser nulo");

            if (cicloContrato == null) throw new Exception("El ciclo contrato no puede ser nulo");
            if (cicloContrato.CicloEscolar == null) throw new Exception("El ciclo escolar no puede ser nulo");
            if (cicloContrato.CicloEscolar.UbicacionID == null) throw new Exception("La ubicación del ciclo escolar no puede ser nulo");
            if (cicloContrato.RecursoContrato == null) throw new Exception("El recurso contrato no puede ser nulo");

            object myFirm = new object();
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            try
            {
                ValidateCicloContrato(dctx, contrato, cicloContrato);

                CicloEscolarCtrl cicloEscolarCtrl = new CicloEscolarCtrl();
                RecursoContratoCtrl recursoContratoCtrl = new RecursoContratoCtrl();
                UbicacionCtrl ubicacionCtrl = new UbicacionCtrl();

                DataSet ds = ubicacionCtrl.RetrieveExacto(dctx, cicloContrato.CicloEscolar.UbicacionID);
                if (ds.Tables[0].Rows.Count == 1)
                    cicloContrato.CicloEscolar.UbicacionID = ubicacionCtrl.LastDataRowToUbicacion(ds);
                else
                {
                    cicloContrato.CicloEscolar.UbicacionID.FechaRegistro = DateTime.Now;
                    ubicacionCtrl.Insert(dctx, cicloContrato.CicloEscolar.UbicacionID);
                    DataSet dsUbicacion = ubicacionCtrl.RetrieveExacto(dctx, cicloContrato.CicloEscolar.UbicacionID);
                    cicloContrato.CicloEscolar.UbicacionID = ubicacionCtrl.LastDataRowToUbicacion(dsUbicacion);
                }

                Update(dctx, cicloContrato, previous);

                cicloEscolarCtrl.Update(dctx, cicloContrato.CicloEscolar, previous.CicloEscolar);

                recursoContratoCtrl.Update(dctx, cicloContrato.RecursoContrato, previous.RecursoContrato);

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
        /// Elimina un registro de CicloContratoDelHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="cicloContratoDelHlp">CicloContratoDelHlp que desea eliminar</param>
        private void Delete(IDataContext dctx, CicloContrato cicloContrato)
        {
            CicloContratoDelHlp da = new CicloContratoDelHlp();
            da.Action(dctx, cicloContrato);
        }

        /// <summary>
        /// Elimina un registro de ciclo contrato en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="contrato">Contrato del ciclo contrato</param>
        /// <param name="cicloContrato">Ciclo contrato relacionado</param>
        public void DeleteComplete(IDataContext dctx, Contrato contrato, CicloContrato cicloContrato)
        {
            if (contrato == null) throw new Exception("El contrato no puede ser nulo.");
            if (cicloContrato == null) throw new Exception("El ciclo contrato no puede ser nulo");
            if (cicloContrato.CicloContratoID == null) throw new Exception("El identificador del ciclo contrato no puede ser nulo");
            object myFirm = new object();
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            try
            {
                CicloEscolarCtrl cicloEscolarCtrl = new CicloEscolarCtrl();
                RecursoContratoCtrl recursoContratoCtrl = new RecursoContratoCtrl();

                cicloContrato = RetrieveComplete(dctx,contrato, new CicloContrato { CicloContratoID = cicloContrato.CicloContratoID });

                //eliminamos el ciclo contrato
                Delete(dctx, cicloContrato);

                //eliminamos el ciclo escolar
                CicloEscolar copy = cicloContrato.CicloEscolar.Clone() as CicloEscolar;
                copy.Activo = false;
                cicloEscolarCtrl.Update(dctx, copy, cicloContrato.CicloEscolar);

                //eliminamos el recurso contrato
                recursoContratoCtrl.Delete(dctx, cicloContrato.RecursoContrato);

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
        /// Valida que un ciclo contrato se encuentre dentro las fechas del contrato
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="contrato">contrato relacionado</param>
        /// <param name="cicloContrato">ciclo contrato</param>
        private void ValidateCicloContrato(IDataContext dctx, Contrato contrato, CicloContrato cicloContrato)
        {
            ContratoCtrl contratoCtrl = new ContratoCtrl();

            contrato = contratoCtrl.LastDataRowToContrato(contratoCtrl.Retrieve(dctx, contrato));

            if (DateTime.Compare(contrato.InicioContrato.Value, cicloContrato.CicloEscolar.InicioCiclo.Value) >= 0)
                throw new Exception("El inicio del ciclo no puede ser menor al inicio del contrato");

            if (DateTime.Compare(cicloContrato.CicloEscolar.FinCiclo.Value, contrato.FinContrato.Value) >= 0)
                throw new Exception("El fin del ciclo no puede ser mayor al fin del contrato");

        }
        /// <summary>
        /// Crea un objeto de CicloContrato a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de CicloContrato</param>
        /// <returns>Un objeto de CicloContrato creado a partir de los datos</returns>
        public CicloContrato LastDataRowToCicloContrato(DataSet ds)
        {
            if (!ds.Tables.Contains("CicloContrato"))
                throw new Exception("LastDataRowToCicloContrato: DataSet no tiene la tabla CicloContrato");
            int index = ds.Tables["CicloContrato"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToCicloContrato: El DataSet no tiene filas");
            return this.DataRowToCicloContrato(ds.Tables["CicloContrato"].Rows[index - 1]);
        }
        /// <summary>
        /// Crea un objeto de CicloContrato a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de CicloContrato</param>
        /// <returns>Un objeto de CicloContrato creado a partir de los datos</returns>
        public CicloContrato DataRowToCicloContrato(DataRow row)
        {
            CicloContrato cicloContrato = new CicloContrato();
            cicloContrato.CicloEscolar = new CicloEscolar();
            if (row.IsNull("CicloContratoID"))
                cicloContrato.CicloContratoID = null;
            else
                cicloContrato.CicloContratoID = (long)Convert.ChangeType(row["CicloContratoID"], typeof(long));
            if (row.IsNull("CicloEscolarID"))
                cicloContrato.CicloEscolar.CicloEscolarID = null;
            else
                cicloContrato.CicloEscolar.CicloEscolarID = (int)Convert.ChangeType(row["CicloEscolarID"], typeof(int));
            if (row.IsNull("EstaLiberado"))
                cicloContrato.EstaLiberado = null;
            else
                cicloContrato.EstaLiberado = (bool)Convert.ChangeType(row["EstaLiberado"], typeof(bool));
            if (row.IsNull("Activo"))
                cicloContrato.Activo = null;
            else
                cicloContrato.Activo = (bool)Convert.ChangeType(row["Activo"], typeof(bool));
            if (row.IsNull("FechaRegistro"))
                cicloContrato.FechaRegistro = null;
            else
                cicloContrato.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
            return cicloContrato;
        }
    }
}
