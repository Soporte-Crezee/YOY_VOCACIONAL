using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Prueba.Diagnostico.DA;
using POV.Prueba.BO;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Expediente.BO;
using POV.Expediente.Service;
using POV.Licencias.BO;
using POV.Licencias.Service;
using POV.Comun.BO;
using POV.Prueba.Diagnostico.Dinamica.Service;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Prueba.Calificaciones.BO;
using POV.Prueba.Calificaciones.Services;

namespace POV.Prueba.Diagnostico.Service
{
    public class PruebaDiagnosticoCtrl
    {        

        /// <summary>
        ///1.Encontrar primero la prueba pivote que esté pendiente, si existe esa prueba pivote; Fin.
        ///2.Encontrar las pruebas asignadas al  grupo y que esten pendientes
        ///2.1 Las ordenamos por fecha de registro.
        ///2.1.1 Encontrar las pruebas que tienen vigencia. Si existe por lo menos una y ésta se puede presentar; FIN
        ///2.1.2 Encontrar las pruebas que NO tienen vigencia. Si existe por lo menos una; FIN
        /// </summary>
        /// <param name="dctx">Los parámetros de configuracion de acceso a base de datos</param>
        /// <param name="contrato">El contrato que está en uso</param>
        /// <param name="alumno">Alumno que desea presentar una prueba</param>
        /// <param name="escuela">Escuela a la que pertenece el alumno</param>
        /// <param name="grupoCicloEscolar">Grupo y Ciclo escolar del alumno en la escuela</param>
        /// <returns>Prueba pendiente del alumno</returns>
        public List<APrueba> RetrievePruebaPendiente(IDataContext dctx, Contrato contrato, Alumno alumno, Escuela escuela, GrupoCicloEscolar grupoCicloEscolar, bool esPruebaPivote)
        {
            #region *** validaciones ***
            if (dctx == null) throw new Exception("El DataContext es requerido");
            if (contrato == null) throw new Exception("El contrato es requerido");
            if (contrato.ContratoID == null) throw new Exception("El identificador del contrato es requerido");
            if (alumno == null) throw new Exception("El alumno es requerido");
            if (alumno.AlumnoID == null) throw new Exception("El identificador del alumno es requerido");
            if (escuela == null) throw new Exception("La escuela es requerida");
            if (escuela.EscuelaID == null) throw new Exception("el identificador de la escuela es requerido");
            if (grupoCicloEscolar == null) throw new Exception("El grupo ciclo escolar es requerido");
            if (grupoCicloEscolar.GrupoCicloEscolarID == null) throw new Exception("El identificador del ciclo escolar es requerido");
            #endregion            
            List<APrueba> pruebaPendiente = new List<APrueba>();
            GrupoCicloEscolarCtrl grupoCicloEscolarCtrl = new GrupoCicloEscolarCtrl();
            DataSet dsGrupoCicloEscolar= grupoCicloEscolarCtrl.Retrieve(dctx, grupoCicloEscolar);
            if (dsGrupoCicloEscolar.Tables[0].Rows.Count > 0)
                grupoCicloEscolar = grupoCicloEscolarCtrl.LastDataRowToGrupoCicloEscolar(grupoCicloEscolarCtrl.Retrieve(dctx, grupoCicloEscolar));
            else throw new Exception("No se pudo realizar la consulta, por que no existe un grupo asignado al cicloEscolar");
           
            CicloContratoCtrl cicloContratoCtrl = new CicloContratoCtrl();
            ContratoCtrl contratoCtrl = new ContratoCtrl();
            List<PruebaContrato> pruebas = contratoCtrl.RetrievePruebasAsignadoContrato(dctx, contrato, new CicloContrato { CicloEscolar = grupoCicloEscolar.CicloEscolar });
            
            #region ***Consultamos la prueba pivote del contrato***
            if (esPruebaPivote)
            {
                PruebaContrato pruebaPivoteContrato = pruebas.FirstOrDefault(item => item.TipoPruebaContrato == ETipoPruebaContrato.Pivote);

                if (pruebaPivoteContrato != null && pruebaPivoteContrato.Activo.Value) //si existe una prueba pivote en el contrato activa se verifica si esta PENDIENTE
                {
                    APrueba pruebaPivote = null;
                    List<APrueba> pPivote = new List<APrueba>();
                    if (pruebaPivoteContrato.Activo != null)
                        if (pruebaPivoteContrato.Activo.Value)
                        {
                            pruebaPivote = pruebaPivoteContrato.Prueba;
                            pruebaPivote = TienePruebaPendiente(pruebaPivote, dctx, alumno, escuela, grupoCicloEscolar, ETipoResultadoPrueba.PRUEBA_DIAGNOSTICA);
                            pPivote.Add(pruebaPivote);
                            return pPivote;                            
                        }
                }
            }
            #endregion

            #region ***Consultamos la pruebas asignadas a un grupo***
            else
            {
                CalendarizacionPruebaGrupoCtrl calendarizacionPruebaGrupoCtrl = new CalendarizacionPruebaGrupoCtrl();
                List<CalendarizacionPruebaGrupo> listCalendarizacion = calendarizacionPruebaGrupoCtrl.RetrieveListaCalendarizacionPruebaGrupo(dctx, grupoCicloEscolar);
                listCalendarizacion = listCalendarizacion.Where(x => x.Activo == true).ToList();
                List<CalendarizacionPruebaGrupo> listCalendarizacionPruebaGrupoConVigencia = (listCalendarizacion.OrderBy(x => x.FechaRegistro)).Where(y => y.ConVigencia == true).ToList<CalendarizacionPruebaGrupo>();
                List<CalendarizacionPruebaGrupo> listCalendarizacionPruebaGrupoSinVigencia = (listCalendarizacion.OrderBy(x => x.FechaRegistro)).Where(y => y.ConVigencia == false).ToList<CalendarizacionPruebaGrupo>();
                List<PruebaContrato> pruebasNoPivote = pruebas.Where(p => p.TipoPruebaContrato != ETipoPruebaContrato.Pivote && (bool)p.Activo).ToList<PruebaContrato>();


                pruebaPendiente = (GetPruebaPendienteByCalendarizacion(listCalendarizacionPruebaGrupoSinVigencia, pruebasNoPivote, dctx, alumno, escuela, grupoCicloEscolar)).ToList();
                if (pruebaPendiente.Count > 0)
                    return pruebaPendiente;

                return GetPruebaPendienteByCalendarizacion(listCalendarizacionPruebaGrupoConVigencia, pruebasNoPivote, dctx, alumno, escuela, grupoCicloEscolar);
            }
            #endregion

            return null;
        }
        private List<APrueba> GetPruebaPendienteByCalendarizacion(List<CalendarizacionPruebaGrupo> listCalendarizaciones,List<PruebaContrato> pruebasNoPivote,IDataContext dctx,Alumno alumno,Escuela escuela,GrupoCicloEscolar grupoCicloEscolar) {
            PruebaContrato temp = null;
            APrueba pruebaPendiente = null;
            DateTime fechaHoy= DateTime.Now;
            List<APrueba> pruebaPendienteByCalendarizacion = new List<APrueba>();
            foreach (CalendarizacionPruebaGrupo calendarizacionPruebaGrupo in listCalendarizaciones)
            {
                if (calendarizacionPruebaGrupo.ConVigencia != null)
                {
                    if ((bool)calendarizacionPruebaGrupo.ConVigencia)
                    {
                        if (calendarizacionPruebaGrupo.FechaFinVigencia != null && calendarizacionPruebaGrupo.FechaInicioVigencia != null)
                        {
                            if (calendarizacionPruebaGrupo.FechaInicioVigencia <= fechaHoy && fechaHoy <= calendarizacionPruebaGrupo.FechaFinVigencia)
                            {
                                temp = GetPruebaContratoFromCalendarizacionPruebaGrupo(pruebasNoPivote, calendarizacionPruebaGrupo);
                                pruebaPendiente = temp.Prueba;
								pruebaPendienteByCalendarizacion.Add(pruebaPendiente);
                            }
                        }
                    }
                    else
                    {
                        temp = GetPruebaContratoFromCalendarizacionPruebaGrupo(pruebasNoPivote, calendarizacionPruebaGrupo);

                        if (temp != null)
                        {
                            pruebaPendiente = temp.Prueba;
							pruebaPendienteByCalendarizacion.Add(pruebaPendiente);
                        }
                    }                
                }               
            }
            return pruebaPendienteByCalendarizacion;
        }

        private PruebaContrato GetPruebaContratoFromCalendarizacionPruebaGrupo(List<PruebaContrato> pruebas, CalendarizacionPruebaGrupo calendarizacion) {
            
            var result= pruebas.Where(p=> p.PruebaContratoID == calendarizacion.PruebaContrato.PruebaContratoID).ToList<PruebaContrato>();
            if (result != null)
                if (result.Count > 0)
                    return result[result.Count-1];

            return null;
        }
        public APrueba TienePruebaPendiente(APrueba prueba,IDataContext dctx,Alumno alumno,Escuela escuela,GrupoCicloEscolar grupoCicloEscolar, ETipoResultadoPrueba tipoResultado) {
            bool estaPendiente = false;
            switch (prueba.TipoPrueba)
            {
                case ETipoPrueba.Dinamica:
                    ResultadoPruebaDinamicaCtrl resultadoDinamicaCtrl = new ResultadoPruebaDinamicaCtrl();
                    estaPendiente = resultadoDinamicaCtrl.TienePruebaPendiente(dctx, alumno, escuela, grupoCicloEscolar, prueba as PruebaDinamica, tipoResultado);
                    break;    
            }
            if (estaPendiente) return prueba;
           
            return null;
        }

        /// <summary>
        /// Crea y devuelve un resultado de prueba pendiente de acuerdo al tipo de prueba que se recibe
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="pruebaPendiente"></param>
        /// <param name="alumno"></param>
        /// <param name="escuela"></param>
        /// <param name="grupoCicloEscolar"></param>
        /// <param name="tipo">Tipo de Resultado -> Para el ajuste AN2</param>
        /// <returns></returns>
        public AResultadoPrueba CreateResultadoPruebaPendiente(IDataContext dctx, APrueba pruebaPendiente, Alumno alumno, Escuela escuela, GrupoCicloEscolar grupoCicloEscolar, ETipoResultadoPrueba tipo)
        {


            #region *** validaciones ***
            if (dctx == null) throw new Exception("El DataContext es requerido");
            if (pruebaPendiente == null) throw new Exception("La prueba pendiente es requerido");
            if (pruebaPendiente.PruebaID == null) throw new Exception("El identificador de la prueba es requerido");

            if (alumno == null) throw new Exception("El alumno es requerido");
            if (alumno.AlumnoID == null) throw new Exception("El identificador del alumno es requerido");
            if (escuela == null) throw new Exception("La escuela es requerida");
            if (escuela.EscuelaID == null) throw new Exception("el identificador de la escuela es requerido");
            if (grupoCicloEscolar == null) throw new Exception("El grupo ciclo escolar es requerido");
            if (grupoCicloEscolar.GrupoCicloEscolarID == null) throw new Exception("El identificador del ciclo escolar es requerido");
            #endregion

            AResultadoPrueba resultadoPrueba = null;

            #region *** begin transaction ***
            object myFirm = new object();
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            try
            {
            #endregion

                switch (pruebaPendiente.TipoPrueba)
                {
                    case ETipoPrueba.Dinamica:
                        ResultadoPruebaDinamicaCtrl resultadoPruebaDinamicaCtrl = new ResultadoPruebaDinamicaCtrl();
                        resultadoPrueba = resultadoPruebaDinamicaCtrl.RetrieveResultadoPruebaDinamica(dctx, alumno, escuela, grupoCicloEscolar, pruebaPendiente as PruebaDinamica, tipo);
                        if (resultadoPrueba == null || resultadoPrueba.ResultadoPruebaID == null)
                            resultadoPrueba = resultadoPruebaDinamicaCtrl.CreatePrueba(dctx, alumno, escuela, grupoCicloEscolar, pruebaPendiente as PruebaDinamica, tipo);
                        
                        break;
                }



                #region *** commit transaction ***
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
                #endregion
            return resultadoPrueba;
        }


        public AResultadoPrueba InsertResultadoPruebaAsignada(IDataContext dctx, APrueba pruebaPendiente, Alumno alumno, Escuela escuela, GrupoCicloEscolar grupoCicloEscolar, ETipoResultadoPrueba tipo)
        {


            #region *** validaciones ***
            if (dctx == null) throw new Exception("El DataContext es requerido");
            if (pruebaPendiente == null) throw new Exception("La prueba pendiente es requerido");
            if (pruebaPendiente.PruebaID == null) throw new Exception("El identificador de la prueba es requerido");

            if (alumno == null) throw new Exception("El alumno es requerido");
            if (alumno.AlumnoID == null) throw new Exception("El identificador del alumno es requerido");
            if (escuela == null) throw new Exception("La escuela es requerida");
            if (escuela.EscuelaID == null) throw new Exception("el identificador de la escuela es requerido");
            if (grupoCicloEscolar == null) throw new Exception("El grupo ciclo escolar es requerido");
            if (grupoCicloEscolar.GrupoCicloEscolarID == null) throw new Exception("El identificador del ciclo escolar es requerido");
            #endregion

            AResultadoPrueba resultadoPrueba = null;

            #region *** begin transaction ***
            object myFirm = new object();
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            try
            {
            #endregion

                switch (pruebaPendiente.TipoPrueba)
                {
                    case ETipoPrueba.Dinamica:
                        ResultadoPruebaDinamicaCtrl resultadoPruebaDinamicaCtrl = new ResultadoPruebaDinamicaCtrl();
                        resultadoPrueba = resultadoPruebaDinamicaCtrl.CreatePrueba(dctx, alumno, escuela, grupoCicloEscolar, pruebaPendiente as PruebaDinamica, tipo);

                        break;
                }



                #region *** commit transaction ***
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
                #endregion
            return resultadoPrueba;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="resultadoPrueba"></param>
        public void FinalizarPrueba(IDataContext dctx, AResultadoPrueba resultadoPrueba)
        {
            if (resultadoPrueba == null) throw new Exception("El resultado de la prueba es requerido.");
            if (resultadoPrueba.ResultadoPruebaID == null) throw new Exception("El identificador del resultado de la prueba es requerido");
            if (resultadoPrueba.Prueba == null) throw new Exception("La prueba es requerida.");
            if (resultadoPrueba.Prueba.PruebaID == null) throw new Exception("El identificador de la prueba es requerido");
            if (resultadoPrueba.Prueba.TipoPrueba == null) throw new Exception("El tipo de la prueba es requerido");
            

            #region *** begin transaction ***
            object myFirm = new object();
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            try
            {
            #endregion

                switch (resultadoPrueba.Prueba.TipoPrueba)
                {
                    case ETipoPrueba.Dinamica:
                        ResultadoPruebaDinamicaCtrl resultadoPruebaDinamicaCtrl = new ResultadoPruebaDinamicaCtrl();
                        resultadoPruebaDinamicaCtrl.FinalizarPrueba(dctx, resultadoPrueba as ResultadoPruebaDinamica);
                        break;
                }

            #region *** commit transaction ***
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
                #endregion

        


        }
        /// <summary>
        /// Consulta un registro completo de AResultadoPrueba 
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="resultadoPrueba"></param>
        /// <returns></returns>
        public AResultadoPrueba RetrieveCompleteResultadoPrueba(IDataContext dctx, AResultadoPrueba resultadoPrueba)
        {
            if (resultadoPrueba == null) throw new Exception("El resultado de la prueba es requerido.");
            if (resultadoPrueba.ResultadoPruebaID == null) throw new Exception("El identificador del resultado de la prueba es requerido");
            if (resultadoPrueba.Prueba == null) throw new Exception("La prueba es requerida.");
            if (resultadoPrueba.Prueba.PruebaID == null) throw new Exception("El identificador de la prueba es requerido");
            if (resultadoPrueba.Prueba.TipoPrueba == null) throw new Exception("El tipo de la prueba es requerido");

            AResultadoPrueba resultadoComplete = null;

            switch (resultadoPrueba.Prueba.TipoPrueba)
            {
                case ETipoPrueba.Dinamica:
                ResultadoPruebaDinamicaCtrl resultadoPruebaDinamicaCtrl = new ResultadoPruebaDinamicaCtrl();
                    resultadoComplete = resultadoPruebaDinamicaCtrl.RetrieveComplete(dctx, new DetalleCicloEscolar(), resultadoPrueba as ResultadoPruebaDinamica);
                    break;
          }

            return resultadoComplete;
        }
        /// <summary>
        /// Obtiene un registro completo concreto de AResultadoPrueba de un alumno, null si no encuentra resultado
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="alumno"></param>
        /// <param name="escuela"></param>
        /// <param name="grupoCicloEscolar"></param>
        /// <param name="prueba"></param>
        /// <returns>AResultadoPrueba concreto completo, null en caso contrario</returns>
        public AResultadoPrueba RetrieveCompleteResultadoPrueba(IDataContext dctx, Alumno alumno, Escuela escuela, GrupoCicloEscolar grupoCicloEscolar, APrueba prueba, ETipoResultadoPrueba inTipoResultado)
        {
            if (alumno == null) throw new ArgumentNullException("El alumno no puede ser nulo");
            if (alumno.AlumnoID == null) throw new ArgumentNullException("El identificador alumno no puede ser nulo");
            if (escuela == null) throw new ArgumentNullException("La escuela no puede ser nula");
            if (escuela.EscuelaID == null) throw new ArgumentNullException("El identificador de la escuela no puede ser nulo");
            if (grupoCicloEscolar == null) throw new ArgumentNullException("El grupo ciclo escolar no puede ser nulo");
            if (grupoCicloEscolar.GrupoCicloEscolarID == null) throw new ArgumentNullException("El identificador del grupo ciclo escolar no puede ser nulo");
            if (prueba == null) throw new ArgumentNullException("La prueba no puede ser nula");
            if (prueba.PruebaID == null) throw new ArgumentNullException("El identificador de la prueba no puede ser nulo");

            ResultadoPruebaDARetHlp da = new ResultadoPruebaDARetHlp();

            DataSet dsResultado = da.Action(dctx, alumno, escuela, grupoCicloEscolar, prueba, inTipoResultado);

            if (dsResultado.Tables[0].Rows.Count > 0)
            {
                int resultadoPruebaID = dsResultado.Tables[0].Rows[dsResultado.Tables[0].Rows.Count - 1].Field<int>("ResultadoPruebaID");
                byte tipoPrueba = dsResultado.Tables[0].Rows[dsResultado.Tables[0].Rows.Count - 1].Field<byte>("TipoPrueba");
                byte tipoResultado = dsResultado.Tables[0].Rows[dsResultado.Tables[0].Rows.Count - 1].Field<byte>("Tipo");

                AResultadoPrueba resultadoPrueba = null;

                if ((ETipoPrueba)tipoPrueba == ETipoPrueba.Dinamica)
                {
                    resultadoPrueba = new ResultadoPruebaDinamica
                    {
                        ResultadoPruebaID = resultadoPruebaID,
                        Prueba = prueba,
                        Tipo = (ETipoResultadoPrueba)tipoResultado
                    };
                }

                if (resultadoPrueba != null)
                    return RetrieveCompleteResultadoPrueba(dctx, resultadoPrueba);
            }
            return null;
        }
        /// <summary>
        /// Obtiene un registro simple concreto de AResultadoPrueba de un alumno, null si no encuentra resultado
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="alumno"></param>
        /// <param name="escuela"></param>
        /// <param name="grupoCicloEscolar"></param>
        /// <param name="prueba"></param>
        /// <returns>AResultadoPrueba concreto completo, null en caso contrario</returns>
        public AResultadoPrueba RetrieveSimpleResultadoPrueba(IDataContext dctx, Alumno alumno, Escuela escuela, GrupoCicloEscolar grupoCicloEscolar, APrueba prueba, ETipoResultadoPrueba? tipoResultado)
        {
            if (alumno == null) throw new ArgumentNullException("El alumno no puede ser nulo");
            if (alumno.AlumnoID == null) throw new ArgumentNullException("El identificador alumno no puede ser nulo");
            if (escuela == null) throw new ArgumentNullException("La escuela no puede ser nula");
            if (escuela.EscuelaID == null) throw new ArgumentNullException("El identificador de la escuela no puede ser nulo");
            if (grupoCicloEscolar == null) throw new ArgumentNullException("El grupo ciclo escolar no puede ser nulo");
            if (grupoCicloEscolar.GrupoCicloEscolarID == null) throw new ArgumentNullException("El identificador del grupo ciclo escolar no puede ser nulo");
            if (prueba == null) throw new ArgumentNullException("La prueba no puede ser nula");
            if (prueba.PruebaID == null) throw new ArgumentNullException("El identificador de la prueba no puede ser nulo");

            ResultadoPruebaDARetHlp da = new ResultadoPruebaDARetHlp();
            CatalogoPruebaCtrl catalogoPruebatrl = new CatalogoPruebaCtrl();
            DataSet dsResultado = da.Action(dctx, alumno, escuela, grupoCicloEscolar, prueba, tipoResultado);

            if (dsResultado.Tables[0].Rows.Count > 0)
            {
                int resultadoPruebaID = dsResultado.Tables[0].Rows[dsResultado.Tables[0].Rows.Count - 1].Field<int>("ResultadoPruebaID");
                byte tipoPrueba = dsResultado.Tables[0].Rows[dsResultado.Tables[0].Rows.Count - 1].Field<byte>("TipoPrueba");
                byte tipoResPrueba = dsResultado.Tables[0].Rows[dsResultado.Tables[0].Rows.Count - 1].Field<byte>("Tipo");

                AResultadoPrueba resultadoPrueba = null;

                if ((ETipoPrueba)tipoPrueba == ETipoPrueba.Dinamica)
                {
                    resultadoPrueba = new ResultadoPruebaDinamica
                    {
                        ResultadoPruebaID = resultadoPruebaID,
                        Prueba = catalogoPruebatrl.RetrieveComplete(dctx, prueba, true),
                        Tipo = (ETipoResultadoPrueba)tipoResPrueba
                    };
                }

                if (resultadoPrueba != null)
                    return resultadoPrueba;
            }
            return null;
        }
        /// <summary>
        /// Consulta un registro completo de resultado clasificador concreto en base a un resultado prueba concreto
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="resultadoPrueba"></param>
        /// <returns>ResultadoClasificador concreto, null en caso de no encontrar</returns>
        public AResultadoClasificador RetrieveCompleteByResultadoPrueba(IDataContext dctx, AResultadoPrueba resultadoPrueba)
        {
            if (resultadoPrueba == null) throw new ArgumentNullException("El resultado de la prueba no puede ser nulo");
            if (resultadoPrueba.ResultadoPruebaID == null) throw new ArgumentNullException("El identificador resultado de la prueba no puede ser nulo");
            if (resultadoPrueba.Prueba == null) throw new ArgumentNullException("La prueba es requerido");
            if (resultadoPrueba.Prueba.TipoPrueba == null) throw new ArgumentNullException("El tipo de la prueba es requerido");
            AResultadoClasificador resultadoClasificador = null;
            switch (resultadoPrueba.Prueba.TipoPrueba)
            {
                case ETipoPrueba.Dinamica:
                    ResultadoClasificadorDinamicaCtrl resultadoClasificadorDinamicoCtrl = new ResultadoClasificadorDinamicaCtrl();
                    resultadoClasificador = resultadoClasificadorDinamicoCtrl.RetrieveComplete(dctx, new ResultadoClasificadorDinamica { ResultadoPrueba = resultadoPrueba });
                    break;
                default:
                    throw new NotImplementedException("Tipo no implementado");
            }


            return resultadoClasificador;
        }
        /// <summary>
        /// Consulta un registro simple de resultado clasificador concreto en base a un resultado prueba concreto
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="resultadoPrueba"></param>
        /// <returns>ResultadoClasificador concreto, null en caso de no encontrar</returns>
        public AResultadoClasificador RetrieveSimpleByResultadoPrueba(IDataContext dctx, AResultadoPrueba resultadoPrueba)
        {
            if (resultadoPrueba == null) throw new ArgumentNullException("El resultado de la prueba no puede ser nulo");
            if (resultadoPrueba.ResultadoPruebaID == null) throw new ArgumentNullException("El identificador resultado de la prueba no puede ser nulo");
            if (resultadoPrueba.Prueba == null) throw new ArgumentNullException("La prueba es requerido");
            if (resultadoPrueba.Prueba.TipoPrueba == null) throw new ArgumentNullException("El tipo de la prueba es requerido");
            AResultadoClasificador resultadoClasificador = null;
            switch (resultadoPrueba.Prueba.TipoPrueba)
            {
                case ETipoPrueba.Dinamica:
                    ResultadoClasificadorDinamicaCtrl resultadoClasificadorDinamicoCtrl = new ResultadoClasificadorDinamicaCtrl();
                    resultadoClasificador = resultadoClasificadorDinamicoCtrl.RetrieveComplete(dctx, new ResultadoClasificadorDinamica { ResultadoPrueba = resultadoPrueba });
                    break;
                default:
                    throw new NotImplementedException("Tipo no implementado");
            }


            return resultadoClasificador;
        }
        /// <summary>
        /// Recupera un registro completo AResultadoClasificador
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="resultadoClasificador"></param>
        /// <returns>ResultadoClasificador concreto, null en caso de no encontrar</returns>
        public AResultadoClasificador RetrieveComplete(IDataContext dctx, AResultadoClasificador resultadoClasificador)
        {
            if (resultadoClasificador == null) throw new ArgumentNullException("El resultado de clasificador no puede ser nulo");
            if (resultadoClasificador.TipoResultadoClasificador == null) throw new ArgumentNullException("El tipo de resultado de clasificador no puede ser nulo");

            switch (resultadoClasificador.TipoResultadoClasificador)
            {
                case ETipoResultadoClasificador.DINAMICA:
                    ResultadoClasificadorDinamicaCtrl resultadoClasificadorDinamicoCtrl = new ResultadoClasificadorDinamicaCtrl();
                    resultadoClasificador = resultadoClasificadorDinamicoCtrl.RetrieveComplete(dctx, resultadoClasificador);
                    break;
                default:
                    throw new NotImplementedException("Tipo no implementado");
            }

            return null;
        }
        /// <summary>
        /// Crea e inserta un Resultado Clasificador de la prueba 
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="resultadoPrueba"></param>
        public void InsertResultadoClasificador(IDataContext dctx, AResultadoPrueba resultadoPrueba)
        {
            if (resultadoPrueba == null) throw new ArgumentNullException("El resultado de la prueba no puede ser nulo");
            if (resultadoPrueba.ResultadoPruebaID == null) throw new ArgumentNullException("El identificador resultado de la prueba no puede ser nulo");
            if (resultadoPrueba.Prueba == null) throw new ArgumentNullException("La prueba no puede ser nulo");
            switch (resultadoPrueba.Prueba.TipoPrueba)
            {
                case ETipoPrueba.Dinamica:
                    

                    throw new NotImplementedException("");
            }
        }
        /// <summary>
        /// Consulta un AResultadoMetodoCalificacion completo en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="resultadoMetodoCalificacion">AResultadoMetodoCalificacion que provee el criterio de selección para realizar la consulta</param>
        /// <returns>Un objeto AResultadoMetodoCalificacion con todas sus relaciones</returns>
        public AResultadoMetodoCalificacion RetrieveComplete(IDataContext dctx, AResultadoMetodoCalificacion resultadoMetodoCalificacion)
        {
            ResultadoMetodoCalificacionCtrl resultadoMetoCalificacionCtrl = new ResultadoMetodoCalificacionCtrl();
            if (resultadoMetodoCalificacion == null) throw new ArgumentNullException("RespuestaReactivoDinamicaCtrl: DetalleCicloEscolar no puede ser nulo");
            AResultadoMetodoCalificacion resultadoMetodoCompleto = null;
            DataSet dsResultadoMetodo = resultadoMetoCalificacionCtrl.Retrieve(dctx, resultadoMetodoCalificacion);
            if (dsResultadoMetodo.Tables[0].Rows.Count > 0)
            {
                int? puntaje;
                PruebaDinamicaCtrl pruebaDinamicaCtrl = new PruebaDinamicaCtrl();
                APrueba prueba = resultadoMetodoCalificacion.ResultadoPrueba.Prueba;
                switch (resultadoMetodoCalificacion.TipoResultadoMetodo)
                {
                    case ETipoResultadoMetodo.CLASIFICACION:
                        resultadoMetodoCompleto = new ResultadoMetodoClasificacion();
                        //ResultadoMetodoCalificacion
                        resultadoMetodoCompleto = resultadoMetoCalificacionCtrl.LastDataRowToResultadoMetodoCalificacion(dsResultadoMetodo);
                        List<DetalleResultadoClasificacion> listaDetalleResultadoClasificacion = new List<DetalleResultadoClasificacion>();
                        DetalleResultadoClasificacionCtrl detalleResultadoClasificadorCtrl = new DetalleResultadoClasificacionCtrl();
                        DataSet dsDetalleResultadoClasificador = detalleResultadoClasificadorCtrl.Retrieve(dctx, (resultadoMetodoCompleto as ResultadoMetodoClasificacion), null);
                        if (dsDetalleResultadoClasificador.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in dsDetalleResultadoClasificador.Tables[0].Rows)
                            {
                                EscalaClasificacionDinamica escalaClasificacionDinamica = new EscalaClasificacionDinamica();
                                escalaClasificacionDinamica.PuntajeID = Convert.ToInt32(row["PuntajeID"]);
                                DetalleResultadoClasificacion detalleResultadoClasificacion = new DetalleResultadoClasificacion();

                                detalleResultadoClasificacion = detalleResultadoClasificadorCtrl.DataRowToDetalleResultadoClasificacion(row);
                                escalaClasificacionDinamica = (EscalaClasificacionDinamica)pruebaDinamicaCtrl.RetrieveEscalaDinamicaComplete(dctx, (PruebaDinamica)prueba, escalaClasificacionDinamica);
                                detalleResultadoClasificacion.EscalaClasificacionDinamica = escalaClasificacionDinamica;
                                listaDetalleResultadoClasificacion.Add(detalleResultadoClasificacion);
                            }
                            (resultadoMetodoCompleto as ResultadoMetodoClasificacion).ListaDetalleResultadoClasificacion = listaDetalleResultadoClasificacion;
                        }
                        break;
                    case ETipoResultadoMetodo.PORCENTAJE:
                        resultadoMetodoCompleto = new ResultadoMetodoPorcentaje();
                        resultadoMetodoCompleto = resultadoMetoCalificacionCtrl.LastDataRowToResultadoMetodoCalificacion(dsResultadoMetodo);
                        EscalaPorcentajeDinamica escalaPorcentajeDinamica = new EscalaPorcentajeDinamica();
                        DataRow rowPor = dsResultadoMetodo.Tables[0].Rows[0];
                        escalaPorcentajeDinamica.PuntajeID = Convert.ToInt32(rowPor["PuntajeID"]);
                        escalaPorcentajeDinamica = (EscalaPorcentajeDinamica)pruebaDinamicaCtrl.RetrieveEscalaDinamicaComplete(dctx, (PruebaDinamica)prueba, escalaPorcentajeDinamica);
                        (resultadoMetodoCompleto as ResultadoMetodoPorcentaje).EscalaPorcentajeDinamica = escalaPorcentajeDinamica;
                        break;
                    case ETipoResultadoMetodo.PUNTOS:
                        resultadoMetodoCompleto = new ResultadoMetodoPuntos();
                        resultadoMetodoCompleto = resultadoMetoCalificacionCtrl.LastDataRowToResultadoMetodoCalificacion(dsResultadoMetodo);
                        EscalaPuntajeDinamica escalaPuntajeDinamica = new EscalaPuntajeDinamica();
                        DataRow rowPun = dsResultadoMetodo.Tables[0].Rows[0];
                        escalaPuntajeDinamica.PuntajeID = Convert.ToInt32(rowPun["PuntajeID"]);
                        escalaPuntajeDinamica = (EscalaPuntajeDinamica)pruebaDinamicaCtrl.RetrieveEscalaDinamicaComplete(dctx, (PruebaDinamica)prueba, escalaPuntajeDinamica);
                        (resultadoMetodoCompleto as ResultadoMetodoPuntos).EscalaPuntajeDinamica = escalaPuntajeDinamica;
                        break;
                    case ETipoResultadoMetodo.SELECCION:
                        resultadoMetodoCompleto = new ResultadoMetodoSeleccion();
                        //ResultadoMetodoCalificacion
                        resultadoMetodoCompleto = resultadoMetoCalificacionCtrl.LastDataRowToResultadoMetodoCalificacion(dsResultadoMetodo);
                        List<DetalleResultadoSeleccion> listaDetalleResultadoSeleccion = new List<DetalleResultadoSeleccion>();
                        DetalleResultadoSeleccionCtrl detalleResultadoSeleccionCtrl = new DetalleResultadoSeleccionCtrl();
                        DataSet dsDetalleResultadoSeleccion = detalleResultadoSeleccionCtrl.Retrieve(dctx, (resultadoMetodoCompleto as ResultadoMetodoSeleccion), null);
                        if (dsDetalleResultadoSeleccion.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in dsDetalleResultadoSeleccion.Tables[0].Rows)
                            {
                                EscalaSeleccionDinamica escalaSeleccionDinamica = new EscalaSeleccionDinamica();
                                escalaSeleccionDinamica.PuntajeID = Convert.ToInt32(row["PuntajeID"]);
                                DetalleResultadoSeleccion detalleResultadoSeleccion = new DetalleResultadoSeleccion();
                                detalleResultadoSeleccion = detalleResultadoSeleccionCtrl.DataRowToDetalleResultadoSeleccion(row);
                                escalaSeleccionDinamica = (EscalaSeleccionDinamica)pruebaDinamicaCtrl.RetrieveEscalaDinamicaComplete(dctx, (PruebaDinamica)prueba, escalaSeleccionDinamica);
                                detalleResultadoSeleccion.EscalaSeleccionDinamica = escalaSeleccionDinamica;
                                listaDetalleResultadoSeleccion.Add(detalleResultadoSeleccion);
                            }
                            (resultadoMetodoCompleto as ResultadoMetodoSeleccion).ListaDetalleResultadoSeleccion = listaDetalleResultadoSeleccion;
                        }
                        break;
                }
            }
            return resultadoMetodoCompleto;
        }
    }
}
