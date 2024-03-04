using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Framework.Base.DataAccess;
using POV.CentroEducativo.BO;
using POV.Licencias.BO;
using POV.Licencias.DA;
using POV.Licencias.DAO;
using POV.Prueba.Diagnostico.Dinamica.BO;

namespace POV.Licencias.Service
{
    public class PruebaContratoCtrl
    {
        /// <summary>
        /// Inserta un registro de prueba contrato
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="recursoContrato">Recurso contrato que se usara como filtro</param>
        /// <param name="pruebaContrato">prueba contrato que se usara como filtro</param>
        public void InsertPruebaContrato(IDataContext dctx, RecursoContrato recursoContrato, PruebaContrato pruebaContrato)
        {
            if (recursoContrato == null) throw new Exception("El RecursoContrato es requerido");
            if (recursoContrato.RecursoContratoID == null) throw new Exception("El identificador del RecursoContrato es requerido");
            if (pruebaContrato == null) throw new Exception("La pruebaContrato es requerido");
            if (pruebaContrato.Prueba == null) throw new Exception("La prueba asignada al contrato es requerido");
            if (pruebaContrato.Prueba.PruebaID == null) throw new Exception("El identificador de la prueba es requerido");
            if (pruebaContrato.TipoPruebaContrato == null) throw new Exception("El tipo de la prueba es requerido");
            List<PruebaContrato> pruebas = RetrieveListPruebaContrato(dctx, recursoContrato);

            PruebaContrato pruebaContratoValidar = new PruebaContrato { Prueba = pruebaContrato.Prueba, Activo = true };
            if (Retrieve(dctx, recursoContrato, pruebaContratoValidar).Tables[0].Rows.Count > 0)
                throw new Exception("La prueba seleccionada ya se encuentra asignada al contrato.");
            else if (pruebaContrato.Prueba.EstadoLiberacionPrueba != EEstadoLiberacionPrueba.LIBERADA)
            {
                throw new Exception("La asignación de pruebas al contrato sólo admite pruebas con estado Liberado, por favor verifique");
            }
            else
            {

                #region *** Validaciones de prueba Pivote ***
                if (pruebaContrato.TipoPruebaContrato == ETipoPruebaContrato.Pivote)
                {
                    // Validación de única prueba pivote
                    int totalPivote = pruebas.Count(item => item.TipoPruebaContrato == ETipoPruebaContrato.Pivote && item.Activo == true);

                    if (totalPivote >= 1)
                        throw new Exception("La prueba pivote ya está asignada al contrato, por favor seleccione otro");                    
                }
                #endregion

                PruebaContratoInsHlp da = new PruebaContratoInsHlp();
                da.Action(dctx, recursoContrato, pruebaContrato);
            }
        }
        /// <summary>
        /// Elimina un registro de prueba contrato
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="pruebaContrato">prueba contrato que se usara como filtro</param>
        public void DeletePruebaContrato(IDataContext dctx, PruebaContrato pruebaContrato)
        {
            PruebaContratoDelHlp da = new PruebaContratoDelHlp();
            CalendarizacionPruebaGrupoCtrl calendarizacionPruebaGrupoCtrl = new CalendarizacionPruebaGrupoCtrl();
            DataSet ds = calendarizacionPruebaGrupoCtrl.Retrieve(dctx, new CalendarizacionPruebaGrupo { PruebaContrato = pruebaContrato });
            if (ds.Tables[0].Rows.Count > 0)
                throw new Exception("La prueba no se puede eliminar, esta calendarizada a un grupo");
            else
                da.Action(dctx, pruebaContrato);
        }
        /// <summary>
        /// Consulta registro de Prueba Contrato de la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="recursoContrato">recurso contrato que se usara como filtro</param>
        /// <param name="pruebaContrato">prueba contrato que se usara como filtro</param>
        /// <returns></returns>
        public DataSet Retrieve(IDataContext dctx, RecursoContrato recursoContrato, PruebaContrato pruebaContrato)
        {
            PruebaContratoRetHlp da = new PruebaContratoRetHlp();
            DataSet dsPruebas = da.Action(dctx, recursoContrato, pruebaContrato);

            return dsPruebas;
        }
        /// <summary>
        /// Consulta una lista de prueba contrato
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="recursoContrato">recurso contrato que se usara como filtro</param>
        /// <param name="soloActivos">Opcional. filtro que indica si se requieren registros solo activos o ambos (activos/inactivos)</param>
        /// <returns></returns>
        public List<PruebaContrato> RetrieveListPruebaContrato(IDataContext dctx, RecursoContrato recursoContrato, bool soloActivos = true)
        {
            List<PruebaContrato> pruebasContrato = new List<PruebaContrato>();
            PruebaContrato pruebaContrato = new PruebaContrato();
            if (soloActivos)
                pruebaContrato.Activo = true;

            DataSet dsPruebas = Retrieve(dctx, recursoContrato, pruebaContrato);

            foreach (DataRow row in dsPruebas.Tables[0].Rows)
            {
                PruebaContrato prueba = DataRowToPruebaContrato(row);

                pruebasContrato.Add(prueba);
            }

            return pruebasContrato;
        }
        /// <summary>
        /// Consulta una lista de pruebas asignadas a un contrato.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="contrato">Contrato que se usara como filtro</param>
        /// <param name="cicloContrato">Fltro para traer solo las pruebas de ese contrato con ese ciclo escolar.
        /// <returns></returns>
        public List<PruebaContrato> RetrievePruebasAsignadoContrato(IDataContext dctx, Contrato contrato, CicloContrato cicloContrato)
        {
            List<PruebaContrato> pruebasContrato = new List<PruebaContrato>();
            PruebasContratoDARetHlp da = new PruebasContratoDARetHlp();


            DataSet dsPruebasContrato = da.Action(dctx, contrato, cicloContrato);

            foreach (DataRow row in dsPruebasContrato.Tables[0].Rows)
            {
                PruebaContrato prueba = DataRowToPruebaContrato(row);
                pruebasContrato.Add(prueba);
            }

            return pruebasContrato;
        }

        /// <summary>
        /// Consulta una lista de pruebas asignadas a una escuela.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="contrato">contrato que servirá como filtro</param>
        /// <param name="cicloContrato">cicloContrato que servirá como filtro</param>
        /// <param name="licenciaEscuela">licenciaEscuela que servirá como filtro</param>
        /// <returns>Regresa una lista de pruebas asignadas a la escuela</returns>
        public List<PruebaContrato> RetrievePruebasAsignadaEscuela(IDataContext dctx, Contrato contrato, CicloContrato cicloContrato, LicenciaEscuela licenciaEscuela)
        {
            List<PruebaContrato> pruebasContrato = new List<PruebaContrato>();
            PruebasEscuelaDARetHlp da = new PruebasEscuelaDARetHlp();

            DataSet dsPruebasContrato = da.Action(dctx, contrato, cicloContrato, licenciaEscuela);
            foreach (DataRow row in dsPruebasContrato.Tables[0].Rows)
            {
                PruebaContrato prueba = DataRowToPruebaContrato(row);
                pruebasContrato.Add(prueba);
            }

            return pruebasContrato;
        }
        /// <summary>
        /// Recupera un dataset con los registros de Pruebas asignadas a un Grupo
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="grupoCicloEscolar">Obligatorio: GrupoCicloEscolar, CicloEscolar y Escuela con identificadores para los criterios de selección</param>
        /// <param name="licenciaEscuela">Obligatorio: Contrato con identificador como criterio de selección</param>
        /// <param name="estadoLiberacion">EEstadoLiberacionPrueba como criterio de selección; se ignora si es NULL</param>
        /// <returns>DataSet con los registros que cumplen con los criterios ó vacío si no se encuentran</returns>
        public DataSet RetrievePruebasAsignadasGrupo(IDataContext dctx, GrupoCicloEscolar grupoCicloEscolar, LicenciaEscuela licenciaEscuela, EEstadoLiberacionPrueba? estadoLiberacion)
        {
            #region Validaciones
            if (grupoCicloEscolar == null) throw new ArgumentException("Parámetro requerido", "grupoCicloEscolar");
            if (grupoCicloEscolar.CicloEscolar == null) throw new ArgumentException("Parámetro requerido", "CicloEscolar");
            if (grupoCicloEscolar.Escuela == null) throw new ArgumentException("Parámetro requerido", "Escuela");
            if (licenciaEscuela == null) throw new ArgumentException("Parámetro requerido", "licenciaEscuela");
            if (licenciaEscuela.Contrato == null) throw new ArgumentException("Parámetro requerido", "Contrato");
            #endregion
            DataSet dsPruebas = new DataSet();
            PruebasGrupoDARetHlp helper = new PruebasGrupoDARetHlp();
            dsPruebas = helper.Action(dctx, grupoCicloEscolar, licenciaEscuela, estadoLiberacion);

            return dsPruebas;
        }

        public DataSet RetrieveCiclosVigentesPrueba(IDataContext dctx, Int32? pruebaID)
        {
            if (pruebaID == null) throw new ArgumentException("Parámetro requerido", "PruebaID");

            DataSet ds = new DataSet();
            PruebaEnCicloVigenteDARetHlp helper = new PruebaEnCicloVigenteDARetHlp();
            ds = helper.Action(dctx, pruebaID);

            return ds;
        }

        /// <summary>
        /// Crea un objeto de PruebaContrato a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de PruebaContrato</param>
        /// <returns>Un objeto de PruebaContrato creado a partir de los datos</returns>
        public PruebaContrato LastDataRowToPruebaContrato(DataSet ds)
        {
            if (!ds.Tables.Contains("PruebaContrato"))
                throw new Exception("LastDataRowToRecursoContrato: DataSet no tiene la tabla PruebaContrato");
            int index = ds.Tables["PruebaContrato"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToRecursoContrato: El DataSet no tiene filas");
            return this.DataRowToPruebaContrato(ds.Tables["PruebaContrato"].Rows[index - 1]);
        }
        /// <summary>
        /// Crea un registro de PruebaContrato a partir de los datos de un DataRow
        /// </summary>
        /// <param name="row">DataRow que contiene la informacion de una PruebaContrato</param>
        /// <returns>Registro de PruebaContrato</returns>
        public PruebaContrato DataRowToPruebaContrato(DataRow row)
        {
            PruebaContrato pruebaContrato = new PruebaContrato();
            if (row.IsNull("PruebaContratoID"))
                pruebaContrato.PruebaContratoID = null;
            else
                pruebaContrato.PruebaContratoID = (Int64)Convert.ChangeType(row["PruebaContratoID"], typeof(Int64));
            if (row.IsNull("PruebaID"))
                pruebaContrato.Prueba = null;
            else
            {

                if (!row.IsNull("Tipo"))
                {
                   if (row.Field<byte>("Tipo") == (byte)ETipoPrueba.Dinamica)
                    {
                        pruebaContrato.Prueba = new PruebaDinamica();
                        pruebaContrato.Prueba.PruebaID = (Int32)Convert.ChangeType(row["PruebaID"], typeof(Int32));

                        if (row.IsNull("TipoPruebaPresentacion"))
                            pruebaContrato.Prueba.TipoPruebaPresentacion = ETipoPruebaPresentacion.Dinamica;
                        else
                            pruebaContrato.Prueba.TipoPruebaPresentacion = (ETipoPruebaPresentacion)row.Field<byte>("TipoPruebaPresentacion");
                    }
                    else
                        pruebaContrato.Prueba = null;

                }
                else
                    pruebaContrato.Prueba = null;
            }
            if (row.IsNull("TipoPruebaContrato"))
                pruebaContrato.FechaRegistro = null;
            else
                pruebaContrato.TipoPruebaContrato = (ETipoPruebaContrato)row.Field<byte>("TipoPruebaContrato");
            if (row.IsNull("FechaRegistro"))
                pruebaContrato.FechaRegistro = null;
            else
                pruebaContrato.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
            if (row.IsNull("Activo"))
                pruebaContrato.Activo = null;
            else
                pruebaContrato.Activo = (Boolean)Convert.ChangeType(row["Activo"], typeof(Boolean));

            if (row.IsNull("Espremium"))
                pruebaContrato.Prueba.EsPremium = null;
            else
                pruebaContrato.Prueba.EsPremium = (Boolean)Convert.ChangeType(row["Espremium"], typeof(Boolean));              

            return pruebaContrato;
        }
    }
}
