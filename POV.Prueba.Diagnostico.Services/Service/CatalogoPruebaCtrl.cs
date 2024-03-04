using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Modelo.BO;
using POV.Prueba.BO;
using POV.Prueba.Diagnostico.DA;
using POV.Prueba.Diagnostico.DAO;
using POV.Prueba.Diagnostico.Dinamica.Service;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Reactivos.BO;
using POV.Reactivos.Service;
using POV.Reactivos.DAO;
using POV.Modelo.Estandarizado.BO;
using POV.Modelo.Service;
using POV.Licencias.Service;

namespace POV.Prueba.Diagnostico.Service
{
    public class CatalogoPruebaCtrl
    {
        /// <summary>
        /// Consulta registros de pruebas de la base de datos
        /// </summary>
        /// <param name="dctx">Provee el acceso a la base de datos</param>
        /// <param name="prueba">Prueba con los criterios de filtrado</param>
        /// <param name="lTodas">Null || false: solamente trae las pruebas activas y liberadas ó aquellas que cumplan con la propiedad EstadoLiberacionPrueba; 
        /// true: recupera todas las pruebas sin importar su EstadoLiberacionPrueba</param>
        /// <returns>DataSet con los registros encontrados</returns>
        public DataSet Retrieve(IDataContext dctx, APrueba prueba, bool? lTodas)
        {
            PruebaRetHlp da = new PruebaRetHlp();
            DataSet ds = da.Action(dctx, prueba, lTodas);
            return ds;
        }
        /// <summary>
        /// Consulta registros de pruebas en la base de datos
        /// </summary>
        /// <param name="dctx">Provee el acceso a la base de datos</param>
        /// <param name="pruebaID">Identificador de la prueba, puede ser null</param>
        /// <param name="estadoLiberacionPrueba">Estado de liberación de la prueba, puede ser null</param>
        /// <param name="esDiagnostica">Indicador de prueba diagnóstica, puede ser nul</param>
        /// <returns>DataSet con los registros encontrados</returns>
        public DataSet Retrieve(IDataContext dctx, int? pruebaID, EEstadoLiberacionPrueba? estadoLiberacionPrueba, bool? esDiagnostica)
        {
            PruebaRetHlp da = new PruebaRetHlp();
            DataSet ds = da.Action(dctx, pruebaID, estadoLiberacionPrueba, esDiagnostica);
            return ds;
        }
        /// <summary>
        /// Consulta un registro completo de prueba
        /// </summary>
        /// <param name="dctx">Provee el acceso a la base de datos</param>
        /// <param name="prueba">Objeto PRUEBA con los criterios de filtrado</param>
        /// <param name="lTodas">Null || false: solamente trae las pruebas activas y liberadas ó aquellas que cumplan con la propiedad EstadoLiberacionPrueba; 
        /// true: recupera todas las pruebas sin importar su EstadoLiberacionPrueba</param>
        /// <returns>Registro completo de prueba</returns>
        public APrueba RetrieveComplete(IDataContext dctx, APrueba prueba, bool? lTodas)
        {
            if (prueba == null) throw new Exception("La prueba es requerida");
            if (prueba.TipoPrueba == null) throw new Exception("El tipo de prueba es requerido");
            APrueba pruebaComplete = null;

            switch (prueba.TipoPrueba)
            {
                case ETipoPrueba.Dinamica:
                    PruebaDinamicaCtrl pruebaDinamicaCtrl = new PruebaDinamicaCtrl();
                    pruebaComplete = pruebaDinamicaCtrl.RetrieveComplete(dctx, prueba as PruebaDinamica, lTodas);
                    break;
            }

            return pruebaComplete;
        }
        /// <summary>
        /// Consulta un registro simple de prueba
        /// </summary>
        /// <param name="dctx">Provee el acceso a la base de datos</param>
        /// <param name="prueba">Objeto PRUEBA con los criterios de filtrado</param>
        /// <param name="lTodas">Null || false: solamente trae las pruebas activas y liberadas ó aquellas que cumplan con la propiedad EstadoLiberacionPrueba; 
        /// true: recupera todas las pruebas sin importar su EstadoLiberacionPrueba</param>
        /// <returns>Registro simple de prueba</returns>
        public APrueba RetrieveSimple(IDataContext dctx, APrueba prueba, bool? lTodas)
        {
            if (prueba == null) throw new Exception("La prueba es requerida");
            if (prueba.TipoPrueba == null) throw new Exception("El tipo de prueba es requerido");

            DataSet dsPrueba = null;
            APrueba pruebaSimple = null;

            switch (prueba.TipoPrueba)
            {
                case ETipoPrueba.Dinamica:
                    PruebaDinamicaCtrl pruebaDinamicaCtrl = new PruebaDinamicaCtrl();
                    dsPrueba = pruebaDinamicaCtrl.Retrieve(dctx, (prueba as PruebaDinamica), lTodas);
                    if (dsPrueba != null && dsPrueba.Tables.Count > 0 && dsPrueba.Tables[0].Rows.Count > 0)
                        pruebaSimple = pruebaDinamicaCtrl.LastDataRowToPruebaDinamica(dsPrueba);
                    break;
            }

            return pruebaSimple;
        }

        /// <summary>
        /// Obtiene la lista de reactivos dependiendo del modelo y reactivo proporcionado, también si es completo o no.
        /// </summary>
        /// <param name="dctx">Provee el acceso a la base de datos</param>
        /// <param name="modelo">Modelo proporcionado</param>
        /// <param name="reactivo">Reactivo proporcionado</param>
        /// <param name="completo">Si es completo o no</param>
        /// <returns>Regresa la lista de reactivos pertenecientes al modelo proporcionado.</returns>
        public List<Reactivo> RetrieveListaReactivos(IDataContext dctx, AModelo modelo, Reactivo reactivo, bool completo)
        {
            ReactivoCtrl reactivoCtrl = new ReactivoCtrl();
            List<Reactivo> listreactivos = new List<Reactivo>();
            if (modelo == null) throw new Exception("El modelo es requerido.");

            switch (modelo.TipoModelo)
            {
                case ETipoModelo.Dinamico:
                    Reactivo reactivodinamico = new Reactivo();
                    if (reactivo == null)
                    {
                        reactivodinamico.TipoReactivo = ETipoReactivo.ModeloGenerico;
                    }
                    else if (reactivo.TipoReactivo == ETipoReactivo.ModeloGenerico)
                    {
                        reactivodinamico = reactivo;
                    }
                    else
                    {
                        throw new Exception("El reactivo debe ser igual que el modelo proporcionado.");
                    }
                    if (completo == false)
                    {
                        DataSet ds = reactivoCtrl.Retrieve(dctx, reactivodinamico);
                        if (ds.Tables["Reactivo"].Rows.Count > 0)
                        {
                            foreach (DataRow row in ds.Tables["Reactivo"].Rows)
                            {
                                if (Convert.ToInt32(row["ModeloID"]) == modelo.ModeloID)
                                {
                                    listreactivos.Add(reactivoCtrl.DataRowToReactivo(row, ETipoReactivo.ModeloGenerico));
                                }
                            }
                        }

                    }
                    else
                    {
                        DataSet dsd = reactivoCtrl.Retrieve(dctx, reactivodinamico);
                        if (dsd.Tables["Reactivo"].Rows.Count > 0)
                        {
                            foreach (DataRow row in dsd.Tables["Reactivo"].Rows)
                            {
                                if (Convert.ToInt32(row["ModeloID"]) == modelo.ModeloID)
                                {
                                    listreactivos.Add(reactivoCtrl.RetrieveReactivoComplete(dctx,
                                                                                            reactivoCtrl
                                                                                                .DataRowToReactivo(row,
                                                                                                                   ETipoReactivo
                                                                                                                       .ModeloGenerico)));
                                }
                            }
                        }

                    }
                    break;               
            }
            return listreactivos;
        }

        /// <summary>
        /// DA para Consulta de modelos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="modeloID">ModeloID que provee el criterio de selección para realizar la consulta</param>
        /// <param name="parametros">Parámetros del modelo que proveen el criterio de selección para realizar la consulta</param>
        /// <returns></returns>
        public DataSet RetrievePruebasModelo(IDataContext dctx, int? modeloID, Dictionary<string, string> parametros, EEstadoLiberacionPrueba? estadoLiberacion)
        {
            PruebasModeloDARetHlp da = new PruebasModeloDARetHlp();
            DataSet ds = da.Action(dctx, modeloID, parametros, estadoLiberacion);
            return ds;
        }

        /// <summary>
        /// Inserta un registro de Prueba según su tipo
        /// </summary>
        /// <param name="dctx">Provee el acceso a la base de datos</param>
        /// <param name="prueba">Prueba que se registrará en la base de datos</param>
        public void Insert(IDataContext dctx, APrueba prueba)
        {
            PruebaInsHlp da = new PruebaInsHlp();

            string msgErr = string.Empty;
            bool pruebaValida = this.ValidarPruebaInsUpd(dctx, prueba, null, ref msgErr);

            if (!pruebaValida)
                throw new StandardException(MessageType.Error, "Prueba no válida",
                    msgErr == string.Empty ? "Imposible guardar la prueba!" : msgErr, "POV.Prueba.Diagnostico.Service", "CatalogoPruebaCtrl", "Insert", null);

            object myFirm = new object();
            try
            {
                dctx.OpenConnection(myFirm);
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "Error de conexión", "Hubo un error al conectarse a la base de datos \n "
                    + ex.Message, "POV.Prueba.Diagnostico.Service", "CatalogoPruebaCtrl", "Insert", null, ex.InnerException);
            }

            try
            {
                #region Proceso Principal
                try
                {
                    dctx.BeginTransaction(myFirm);

                    // Valores calculados
                    prueba.FechaRegistro = DateTime.Now;
                    prueba.EstadoLiberacionPrueba = EEstadoLiberacionPrueba.ACTIVA;

                    prueba.EsDiagnostica = prueba.TipoPrueba != ETipoPrueba.Estandarizada;

                    da.Action(dctx, prueba);

                    DataSet dsPrueba = this.Retrieve(dctx, prueba, true);
                    if (dsPrueba == null || dsPrueba.Tables == null || dsPrueba.Tables.Count < 1 || dsPrueba.Tables[0].Rows.Count < 1)
                        throw new StandardException(MessageType.Error, "Error de inconsistencias.", "Imposible recuperar la prueba recién ingresada!",
                            "POV.Prueba.Diagnostico.Service", "CatalogoPruebaCtrl", "Insert", null);

                    APrueba pruebaNew;
                    switch (prueba.TipoPrueba)
                    {
                        case ETipoPrueba.Dinamica:
                            // Insertar prueba
                            PruebaDinamicaCtrl dinamicaCtrl = new PruebaDinamicaCtrl();
                            pruebaNew = dinamicaCtrl.LastDataRowToPruebaDinamica(dsPrueba);
                            prueba.PruebaID = pruebaNew.PruebaID;
                            dinamicaCtrl.Insert(dctx, prueba as PruebaDinamica);

                            // Insertar banco de reactivos
                            BancoReactivosDinamicoCtrl bancoDinamicoCtrl = new BancoReactivosDinamicoCtrl();
                            BancoReactivosDinamico bancoDinamico = new BancoReactivosDinamico();
                            bancoDinamico.Activo = true;
                            bancoDinamico.EsSeleccionOrdenada = true;
                            bancoDinamico.FechaRegistro = DateTime.Now;
                            bancoDinamico.NumeroReactivos = 0;
                            bancoDinamico.Prueba = pruebaNew;
                            bancoDinamico.TipoSeleccionBanco = ETipoSeleccionBanco.TOTAL;
                            bancoDinamico.ReactivosPorPagina = 1;
                            bancoDinamicoCtrl.Insert(dctx, bancoDinamico);
                            break;                       
                        default:
                            throw new StandardException(MessageType.Error, "Error de tipo de prueba.", "Tipo de prueba no soportado en el sistema!",
                                "POV.Prueba.Diagnostico.Service", "CatalogoPruebaCtrl", "Insert", null);
                    }

                    dctx.CommitTransaction(myFirm);
                }
                catch (Exception e2)
                {
                    msgErr = e2.Message;
                    dctx.RollbackTransaction(myFirm);
                    throw new Exception(msgErr);
                }
                #endregion // Proceso Principal
            }
            catch (Exception e1)
            {
                throw e1;
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
        }

        /// <summary>
        /// Actualiza un registro de Prueba según su tipo
        /// </summary>
        /// <param name="dctx">Provee el acceso a la base de datos</param>
        /// <param name="prueba">Prueba que se actualizará en la base de datos</param>
        /// <param name="pruebaAnterior">Prueba anterior para actualición optimista</param>
        public void Update(IDataContext dctx, APrueba prueba, APrueba pruebaAnterior)
        {
            PruebaUpdHlp da = new PruebaUpdHlp();
            bool pruebaValida;

            string msgErr = string.Empty;
            pruebaValida = this.ValidarPruebaUpdate(dctx, prueba, pruebaAnterior, ref msgErr);
            if (!pruebaValida)
                throw new StandardException(MessageType.Error, "Imposible actualizar",
                    msgErr == string.Empty ? "Error en los datos de actualización!" : msgErr, "POV.Prueba.Diagnostico.Service", "CatalogoPruebaCtrl", "Update", null);

            msgErr = string.Empty;
            pruebaValida = this.ValidarPruebaInsUpd(dctx, prueba, pruebaAnterior, ref msgErr);
            if (!pruebaValida)
                throw new StandardException(MessageType.Error, "Prueba no válida",
                    msgErr == string.Empty ? "Imposible guardar la prueba!" : msgErr, "POV.Prueba.Diagnostico.Service", "CatalogoPruebaCtrl", "Update", null);

            // Validar liberación
            if (pruebaAnterior.EstadoLiberacionPrueba == EEstadoLiberacionPrueba.ACTIVA && prueba.EstadoLiberacionPrueba == EEstadoLiberacionPrueba.LIBERADA)
                pruebaValida = this.ValidarLiberacion(dctx, prueba);

            // InitTransaction
            object myFirm = new object();
            try
            {
                dctx.OpenConnection(myFirm);
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "Error de conexión", "Hubo un error al conectarse a la base de datos \n "
                    + ex.Message, "POV.Prueba.Diagnostico.Service", "CatalogoPruebaCtrl", "Update", null, ex.InnerException);
            }

            try
            {
                #region Proceso Principal
                try
                {
                    dctx.BeginTransaction(myFirm);

                    da.Action(dctx, prueba, pruebaAnterior);

                    if (prueba.TipoPrueba != pruebaAnterior.TipoPrueba)
                    {
                        #region Borrar relación anterior
                        switch (pruebaAnterior.TipoPrueba)
                        {
                            case ETipoPrueba.Dinamica:
                                PruebaDinamicaCtrl dinamicaAntCtrl = new PruebaDinamicaCtrl();
                                dinamicaAntCtrl.Delete(dctx, pruebaAnterior as PruebaDinamica);
                                break;
                        }
                        #endregion
                        #region Generar relación nueva
                        switch (prueba.TipoPrueba)
                        {
                            case ETipoPrueba.Dinamica:
                                // Insertar prueba
                                PruebaDinamicaCtrl dinamicaCtrl = new PruebaDinamicaCtrl();
                                dinamicaCtrl.Insert(dctx, prueba as PruebaDinamica);

                                // Insertar banco de reactivos
                                BancoReactivosDinamicoCtrl bancoDinamicoCtrl = new BancoReactivosDinamicoCtrl();
                                BancoReactivosDinamico bancoDinamico = new BancoReactivosDinamico();
                                bancoDinamico.Activo = true;
                                bancoDinamico.EsSeleccionOrdenada = true;
                                bancoDinamico.FechaRegistro = DateTime.Now;
                                bancoDinamico.NumeroReactivos = 0;
                                bancoDinamico.Prueba = prueba;
                                bancoDinamico.TipoSeleccionBanco = ETipoSeleccionBanco.TOTAL;
                                bancoDinamicoCtrl.Insert(dctx, bancoDinamico);
                                break;    
                            default:
                                throw new StandardException(MessageType.Error, "Error de tipo de prueba.", "Tipo de prueba no soportado en el sistema!",
                                    "POV.Prueba.Diagnostico.Service", "CatalogoPruebaCtrl", "Update", null);
                        }
                        #endregion
                    }                   

                    dctx.CommitTransaction(myFirm);
                }
                catch (Exception e2)
                {
                    msgErr = e2.Message;
                    dctx.RollbackTransaction(myFirm);
                    throw new Exception(msgErr);
                }
                #endregion // Proceso Principal
            }
            catch (Exception e1)
            {
                throw e1;
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }

        }

        /// <summary>
        /// Realiza una baja lógica de una prueba
        /// </summary>
        /// <param name="dctx">Proporciona el acceso a la base de datos</param>
        /// <param name="prueba">Prueba que se eliminará</param>
        public void Delete(IDataContext dctx, APrueba prueba)
        {
            APrueba pruebaDelete = this.RetrieveSimple(dctx, prueba, false);

            if (pruebaDelete == null)
                throw new Exception("CatalogoPruebaCtrl: No existe la prueba que desea eliminar!!!");

            string msgErr = string.Empty;
            PruebaDelHlp da = new PruebaDelHlp();
            switch (pruebaDelete.EstadoLiberacionPrueba)
            {
                case EEstadoLiberacionPrueba.INACTIVA:
                    msgErr = "CatalogoPruebaCtrl: La prueba se encuentra inactiva!!!";
                    break;
                case EEstadoLiberacionPrueba.ACTIVA:
                    da.Action(dctx, pruebaDelete);
                    break;
                case EEstadoLiberacionPrueba.LIBERADA:
                    PruebaContratoCtrl pruebaContratoCtrl = new PruebaContratoCtrl();
                    DataSet dsCiclosVigentes = pruebaContratoCtrl.RetrieveCiclosVigentesPrueba(dctx, pruebaDelete.PruebaID);
                    if (dsCiclosVigentes != null && dsCiclosVigentes.Tables.Count > 0 && dsCiclosVigentes.Tables[0].Rows.Count > 0)
                        msgErr = "CatalogoPruebaCtrl: No se puede eliminar la prueba debido a que está asignada a un ciclo escolar vigente, por favor verifique!";
                    else
                        da.Action(dctx, pruebaDelete);
                    break;
                default:
                    msgErr = "CatalogoPruebaCtrl: Estado de la prueba incorrecto!!!";
                    break;
            }
            if (msgErr != string.Empty)
            {
                throw new StandardException(MessageType.Error, "No se puede eliminar!", msgErr,
                    "POV.Prueba.Diagnostico.Service", "CatalogoPruebaCtrl", "Insert", null);
            }
        }

        /// <summary>
        /// Obtiene el banco de reactivos de una prueba proporcionada
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="prueba">Prueba a la que pertenece el banco de reactivos</param>
        /// <returns>ABancoReactivo</returns>
        public ABancoReactivo RetrieveBancoReactivosPrueba(IDataContext dctx, APrueba prueba)
        {
            if (prueba == null || prueba.PruebaID == null) throw new ArgumentNullException("prueba", "CatalogoPruebaCtrl:prueba no puede ser nulo");
            ABancoReactivo banco = null;

            switch (prueba.TipoPrueba)
            {
                case ETipoPrueba.Dinamica:
                    BancoReactivosDinamicoCtrl bancoReactivosDinamicoCtrl = new BancoReactivosDinamicoCtrl();
                    banco = bancoReactivosDinamicoCtrl.RetrieveComplete(dctx, new BancoReactivosDinamico { Prueba = prueba });
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return banco;
        }

        /// <summary>
        /// Registra un BancoReactivo para una prueba
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="bancoReactivo">BancoReactivo que se desea registrar</param>
        public void InsertBancoReactivosPrueba(IDataContext dctx, ABancoReactivo bancoReactivo)
        {
            if (bancoReactivo == null) throw new ArgumentNullException("bancoReactivo", "CatalogoPruebaCtrl:BancoReactivo no puede ser nulo");
            if (bancoReactivo.Prueba == null) throw new ArgumentException("CatalogoPruebaCtrl:Prueba no puede ser nulo");
            switch (bancoReactivo.Prueba.TipoPrueba)
            {
                case ETipoPrueba.Dinamica:
                    if (bancoReactivo is BancoReactivosDinamico)
                    {
                        BancoReactivosDinamicoCtrl bancoReactivosDinamicoCtrl = new BancoReactivosDinamicoCtrl();
                        bancoReactivosDinamicoCtrl.Insert(dctx, (BancoReactivosDinamico)bancoReactivo);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Actualiza un BancoReactivo para una prueba
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="bancoReactivo">BancoReactivo que se desea registrar</param>
        /// <param name="previo">BancoReactivo previo</param>
        public void UpdateBancoReactivosPrueba(IDataContext dctx, ABancoReactivo bancoReactivo, ABancoReactivo previo)
        {
            if (bancoReactivo == null) throw new ArgumentNullException("bancoReactivo", "CatalogoPruebaCtrl:BancoReactivo no puede ser nulo");
            if (bancoReactivo.Prueba == null) throw new ArgumentException("CatalogoPruebaCtrl:Prueba no puede ser nulo");
            if (bancoReactivo.ListaReactivosBanco == null || bancoReactivo.ListaReactivosBanco.Count <= 0) throw new ArgumentException("CatalogoPruebaCtrl:El BancoReactivo no tiene reactivos");

            switch (bancoReactivo.Prueba.TipoPrueba)
            {
                case ETipoPrueba.Dinamica:
                    if (bancoReactivo is BancoReactivosDinamico)
                    {
                        BancoReactivosDinamicoCtrl bancoReactivosDinamicoCtrl = new BancoReactivosDinamicoCtrl();
                        bancoReactivosDinamicoCtrl.UpdateComplete(dctx, (BancoReactivosDinamico)bancoReactivo, (BancoReactivosDinamico)previo);

                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        private bool ValidarPruebaInsUpd(IDataContext dctx, APrueba prueba, APrueba pruebaAnterior, ref string msgErr)
        {
            bool lValido = true;

            AModelo modelo = null;
            ModeloCtrl modeloCtrl = new ModeloCtrl();

            // Modelo válido
            DataSet dsModelo = modeloCtrl.Retrieve(dctx, prueba.Modelo);
            if (dsModelo == null || dsModelo.Tables == null || dsModelo.Tables.Count < 1 || dsModelo.Tables[0].Rows.Count < 1)
            {
                lValido = false;
                msgErr = "El modelo no existe";
            }
            if (lValido)
            {
                try
                {
                    modelo = modeloCtrl.LastDataRowToModelo(dsModelo);
                }
                catch (Exception ex)
                {
                    lValido = false;
                    msgErr = ex.Message;
                }
            }

            //Validar correspondencia entre el modelo y el tipo de prueba
            if (lValido)
            {
                switch (prueba.TipoPrueba)
                {
                    case ETipoPrueba.Dinamica:
                        lValido = modelo is ModeloDinamico;
                        break;
                    default:
                        lValido = false;
                        break;
                }

                if (lValido == false)
                    msgErr = "Inconsistencia entre el modelo de la prueba y su tipo!";
            }

            // Validar clave duplicada para pruebas del mismo tipo
            if (lValido)
            {
                APrueba pruebaTest = this.GetNewObjectPrueba(prueba);
                if (pruebaTest == null)
                {
                    lValido = false;
                    msgErr = "Tipo de prueba no soportado!";
                }
                else
                {
                    pruebaTest.Clave = prueba.Clave;
                    if (pruebaAnterior == null || (pruebaAnterior.TipoPrueba == prueba.TipoPrueba && pruebaAnterior.Clave.ToUpper() != prueba.Clave.ToUpper()))
                    {
                        DataSet dsTest = this.Retrieve(dctx, pruebaTest, false);
                        if (dsTest != null && dsTest.Tables != null && dsTest.Tables.Count > 0 && dsTest.Tables[0].Rows.Count > 0)
                        {
                            lValido = false;
                            msgErr = "Clave de prueba duplicado, verifique";
                        }
                    }
                }
            }

            return lValido;
        }

        /// <summary>
        /// Obtiene un nuevo objeto del mismo tipo que el parámetro
        /// </summary>
        /// <param name="prueba">Prueba original</param>
        /// <returns>Objeto nuevo del mismo tipo que el original</returns>
        private APrueba GetNewObjectPrueba(APrueba prueba)
        {
            APrueba pruebaReturn = null;
            switch (prueba.TipoPrueba)
            {
                case ETipoPrueba.Dinamica:
                    pruebaReturn = new PruebaDinamica();
                    break;
            }
            return pruebaReturn;
        }

        private bool ValidarPruebaUpdate(IDataContext dctx, APrueba prueba, APrueba pruebaAnterior, ref string msgErr)
        {
            bool lValido = true;
            msgErr = string.Empty;

            // Validar cambio de tipo
            if ((prueba.TipoPrueba != pruebaAnterior.TipoPrueba))
            {

                ABancoReactivo banco = this.RetrieveBancoReactivosPrueba(dctx, prueba);
                lValido = (banco == null);

                if (!lValido)
                    msgErr = "La prueba ya está configurada, no puede cambiarse el tipo!";
            }

            return lValido;
        }


        private bool ValidarLiberacion(IDataContext dctx, APrueba prueba)
        {
            bool lValido = false;

            // Tiene reactivos
            ABancoReactivo banco = this.RetrieveBancoReactivosPrueba(dctx, prueba);
            if (banco != null && banco.ListaReactivosBanco != null && banco.ListaReactivosBanco.Count > 0)
                lValido = true;

            return lValido;
        }

    }
}
