using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Comun.BO;
using POV.Licencias.BO;
using POV.Licencias.DAO;

namespace POV.Licencias.Service
{
    /// <summary>
    /// Controlador del objeto RecursoContrato
    /// </summary>
    public class RecursoContratoCtrl
    {
        /// <summary>
        /// Consulta un registro de recurso contrato en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="cicloContrato">Ciclo contato que se usara como filtro</param>
        /// <param name="recursoContrato">Recurso contrato que se usara como filtro</param>
        /// <returns>DataSet que contiene la informacíon del recurso contrato</returns>
        public DataSet Retrieve(IDataContext dctx, CicloContrato cicloContrato, RecursoContrato recursoContrato)
        {
            RecursoContratoRetHlp da = new RecursoContratoRetHlp();
            DataSet ds = da.Action(dctx, cicloContrato, recursoContrato);
            return ds;
        }
        /// <summary>
        /// Consulta un registro complete de recurso complete, en caso contrario devuelve nulo.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="cicloContrato">Ciclo contato que se usara como filtro</param>
        /// <param name="recursoContrato">Recurso contrato que se usara como filtro</param>
        /// <returns>Recurso completo, nulo en caso contrario</returns>
        public RecursoContrato RetrieveComplete(IDataContext dctx, CicloContrato cicloContrato, RecursoContrato recursoContrato)
        {
            if (cicloContrato == null) throw new Exception("El ciclo contrato no puede ser nulo");
            if (recursoContrato == null) throw new Exception("El recurso contrato no puede ser nulo");

            RecursoContrato recursoComplete = null;

            DataSet dsRecursoContrato = Retrieve(dctx, cicloContrato, recursoContrato);
            if (dsRecursoContrato.Tables[0].Rows.Count > 0)
            {
                int index = dsRecursoContrato.Tables[0].Rows.Count;
                DataRow drRecurso = dsRecursoContrato.Tables[0].Rows[index - 1];
                recursoComplete = DataRowToRecursoContrato(drRecurso);

                //Obtener Lista de pruebas
                PruebaContratoCtrl pruebaContratoCtrl = new PruebaContratoCtrl();
                List<PruebaContrato> pruebas = pruebaContratoCtrl.RetrieveListPruebaContrato(dctx, recursoComplete);

                recursoComplete = DataRowToRecursoContrato(drRecurso, pruebas);
            }

            return recursoComplete;
        }
        /// <summary>
        /// Crea un registro de RecursoContratoInsHlp en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="recursoContratoInsHlp">RecursoContratoInsHlp que desea crear</param>
        public void Insert(IDataContext dctx, CicloContrato cicloContrato, RecursoContrato recursoContrato)
        {
            RecursoContratoInsHlp da = new RecursoContratoInsHlp();
            da.Action(dctx, cicloContrato, recursoContrato);
        }
        /// <summary>
        /// Actualiza de manera optimista un registro de RecursoContratoUpdHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="recursoContratoUpdHlp">RecursoContratoUpdHlp que tiene los datos nuevos</param>
        /// <param name="anterior">RecursoContratoUpdHlp que tiene los datos anteriores</param>
        public void Update(IDataContext dctx, RecursoContrato recursoContrato, RecursoContrato previous)
        {
            RecursoContratoUpdHlp da = new RecursoContratoUpdHlp();
            da.Action(dctx, recursoContrato, previous);
        }
        /// <summary>
        /// Elimina un registro de RecursoContratoDelHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="recursoContratoDelHlp">RecursoContratoDelHlp que desea eliminar</param>
        public void Delete(IDataContext dctx, RecursoContrato recursoContrato)
        {
            RecursoContratoDelHlp da = new RecursoContratoDelHlp();
            da.Action(dctx, recursoContrato);
        }
        /// <summary>
        /// Crea un objeto de RecursoContrato a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de RecursoContrato</param>
        /// <returns>Un objeto de RecursoContrato creado a partir de los datos</returns>
        public RecursoContrato LastDataRowToRecursoContrato(DataSet ds)
        {
            if (!ds.Tables.Contains("RecursoContrato"))
                throw new Exception("LastDataRowToRecursoContrato: DataSet no tiene la tabla RecursoContrato");
            int index = ds.Tables["RecursoContrato"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToRecursoContrato: El DataSet no tiene filas");
            return this.DataRowToRecursoContrato(ds.Tables["RecursoContrato"].Rows[index - 1]);
        }
        /// <summary>
        /// Crea un objeto de RecursoContrato a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de RecursoContrato</param>
        /// <param name="pruebas">Opcional. Lista de pruebas del recurso</param>
        /// <param name="juegoContrato">Opcional. Lista de pruebas del recurso </param>
        /// <returns>Un objeto de RecursoContrato creado a partir de los datos</returns>
        public RecursoContrato DataRowToRecursoContrato(DataRow row, List<PruebaContrato> pruebas = null)
        {
            if (pruebas == null)
                pruebas = new List<PruebaContrato>();


            RecursoContrato recursoContrato = new RecursoContrato(pruebas);
            if (row.IsNull("RecursoContratoID"))
                recursoContrato.RecursoContratoID = null;
            else
                recursoContrato.RecursoContratoID = (long)Convert.ChangeType(row["RecursoContratoID"], typeof(long));
            if (row.IsNull("EsAsignacionManual"))
                recursoContrato.EsAsignacionManual = null;
            else
                recursoContrato.EsAsignacionManual = (bool)Convert.ChangeType(row["EsAsignacionManual"], typeof(bool));
            if (row.IsNull("EsPaquetePorPruebaPivote"))
                recursoContrato.EsPaquetePorPruebaPivote = null;
            else
                recursoContrato.EsPaquetePorPruebaPivote = (bool)Convert.ChangeType(row["EsPaquetePorPruebaPivote"], typeof(bool));
            if (row.IsNull("Activo"))
                recursoContrato.Activo = null;
            else
                recursoContrato.Activo = (bool)Convert.ChangeType(row["Activo"], typeof(bool));
            if (row.IsNull("FechaRegistro"))
                recursoContrato.FechaRegistro = null;
            else
                recursoContrato.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));

            
            return recursoContrato;
        }
   }
}
