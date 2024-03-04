using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Framework.Base.DataAccess;
using POV.Expediente.Service;
using POV.Expediente.BO;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Prueba.BO;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Prueba.Diagnostico.Service;

namespace POV.DetalleExpediente.Services
{
    /// <summary>
    /// Controlador que ofrece metodos de mantemineto del paquete de Expediente y sus relaciones abstractas
    /// </summary>
    public class DetalleExpedienteCtrl
    {
        /// <summary>
        /// Recupera un registro completo de expediente escolar
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="expedienteEscolar"></param>
        /// <returns></returns>
        public ExpedienteEscolar RetrieveComplete(IDataContext dctx, ExpedienteEscolar expedienteEscolar)
        {

            ExpedienteEscolar expedienteComplete = null;
            expedienteComplete = RetrieveSimple(dctx, expedienteEscolar);
            if (expedienteComplete != null)
            {
                AlumnoCtrl alumnoCtrl = new AlumnoCtrl();
                ExpedienteEscolarCtrl expedienteEscolarCtrl = new ExpedienteEscolarCtrl();
                expedienteComplete.DetallesCicloEscolar = new List<DetalleCicloEscolar>();
                expedienteComplete.Alumno = alumnoCtrl.LastDataRowToAlumno(alumnoCtrl.Retrieve(dctx, expedienteComplete.Alumno));

                DataSet dsDetalleCiclo = expedienteEscolarCtrl.Retrieve(dctx, new DetalleCicloEscolar(), expedienteComplete);
                if (dsDetalleCiclo.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drDetalleCiclo in dsDetalleCiclo.Tables[0].Rows)
                    {
                        DetalleCicloEscolar detalleCicloEscolar = expedienteEscolarCtrl.DataRowToDetalleCicloEscolar(drDetalleCiclo);

                        detalleCicloEscolar = RetrieveComplete(dctx, expedienteComplete, new DetalleCicloEscolar { DetalleCicloEscolarID = detalleCicloEscolar.DetalleCicloEscolarID });

                        expedienteComplete.DetallesCicloEscolar.Add(detalleCicloEscolar);
                    }
                }
            }
            return expedienteComplete;
        }
        /// <summary>
        /// Recupera un registro completo de DetalleCicloEscolar
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="expedienteEscolar"></param>
        /// <param name="detalleCicloEscolar"></param>
        /// <returns></returns>
        public DetalleCicloEscolar RetrieveComplete(IDataContext dctx, ExpedienteEscolar expedienteEscolar, DetalleCicloEscolar detalleCicloEscolar)
        {
            ExpedienteEscolarCtrl expedienteEscolarCtrl = new ExpedienteEscolarCtrl();
            DataSet dsDetalleCiclo = expedienteEscolarCtrl.Retrieve(dctx, detalleCicloEscolar, expedienteEscolar);
            DetalleCicloEscolar detalleCicloEscolarComplete = null;
            if (dsDetalleCiclo.Tables[0].Rows.Count > 0)
            {
                EscuelaCtrl escuelaCtrl = new EscuelaCtrl();
                GrupoCicloEscolarCtrl grupoCicloEscolarCtrl = new GrupoCicloEscolarCtrl();
                detalleCicloEscolarComplete = expedienteEscolarCtrl.LastDataRowToDetalleCicloEscolar(dsDetalleCiclo);
                detalleCicloEscolarComplete.GrupoCicloEscolar = grupoCicloEscolarCtrl.LastDataRowToGrupoCicloEscolar(grupoCicloEscolarCtrl.Retrieve(dctx, detalleCicloEscolarComplete.GrupoCicloEscolar));
                detalleCicloEscolarComplete.Escuela = escuelaCtrl.LastDataRowToEscuela(escuelaCtrl.Retrieve(dctx, detalleCicloEscolarComplete.Escuela));

                detalleCicloEscolarComplete.AsignacionesRecurso = RetrieveListAsignacionRecurso(dctx, detalleCicloEscolarComplete);
                detalleCicloEscolarComplete.ResultadosPrueba = RetrieveListResultadoPrueba(dctx, detalleCicloEscolarComplete);

            }
            return detalleCicloEscolarComplete;
        }
        /// <summary>
        /// Recupera un lista de asignacionRecurso 
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="expedienteEscolar"></param>
        /// <param name="detalleCicloEscolar"></param>
        /// <returns></returns>
        public List<AAsignacionRecurso> RetrieveListAsignacionRecurso(IDataContext dctx, DetalleCicloEscolar detalleCicloEscolar)
        {
            if (detalleCicloEscolar == null) throw new ArgumentNullException("El detalle no puede ser nulo");
            if (detalleCicloEscolar.DetalleCicloEscolarID == null) throw new ArgumentNullException("El identificador detalle no puede ser nulo");
            ExpedienteEscolarCtrl expedienteEscolarCtrl = new ExpedienteEscolarCtrl();

            AsignacionRecursoCtrl asignacionRecursoCtrl = new AsignacionRecursoCtrl();
            List<AAsignacionRecurso> asignacionesRecurso = new List<AAsignacionRecurso>();

            DataSet dsAsignacioRecurso = asignacionRecursoCtrl.RetrieveAAsignacionRecurso(dctx, detalleCicloEscolar);
            if (dsAsignacioRecurso.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drAsignacionRecurso in dsAsignacioRecurso.Tables[0].Rows)
                {
                    var asignacionRecurso = asignacionRecursoCtrl.DataRowToAsignacionRecurso(drAsignacionRecurso);
                    asignacionesRecurso.Add(asignacionRecurso);
                }
            }
            return asignacionesRecurso;
        }
        /// <summary>
        /// Recupera un registro completo de AAsingacionRecurso
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="detalleCicloEscolar"></param>
        /// <param name="asignacionRecurso"></param>
        /// <returns></returns>
        public AAsignacionRecurso RetrieveComplete(IDataContext dctx, DetalleCicloEscolar detalleCicloEscolar, AAsignacionRecurso asignacionRecurso)
        {
            return null;
        }
        /// <summary>
        /// Consulta un lista de registros de asignacion de un detalle ciclo escolar
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="detalleCicloEscolar"></param>
        /// <returns></returns>
        public List<AResultadoPrueba> RetrieveListResultadoPrueba(IDataContext dctx, DetalleCicloEscolar detalleCicloEscolar)
        {
            ExpedienteEscolarCtrl expedienteEscolarCtrl = new ExpedienteEscolarCtrl();
            List<AResultadoPrueba> resultadosPrueba = new List<AResultadoPrueba>();
            DataSet dsResultadoPrueba = expedienteEscolarCtrl.Retrieve(dctx, detalleCicloEscolar);
            if (dsResultadoPrueba.Tables[0].Rows.Count > 0)
            {
                PruebaDiagnosticoCtrl pruebaDiagnosticoCtrl = new PruebaDiagnosticoCtrl();
                foreach (DataRow drResultadoPrueba in dsResultadoPrueba.Tables[0].Rows)
                {
                    int resultadoPruebaID = drResultadoPrueba.Field<int>("ResultadoPruebaID");
                    ETipoPrueba tipoPrueba = (ETipoPrueba)drResultadoPrueba.Field<byte>("TipoPrueba");
                    int pruebaID = drResultadoPrueba.Field<int>("PruebaID");
                    AResultadoPrueba resultadoPrueba = null;

                    switch (tipoPrueba)
                    {                       
                        case ETipoPrueba.Dinamica:
                            resultadoPrueba = new ResultadoPruebaDinamica
                                {
                                    ResultadoPruebaID = resultadoPruebaID,
                                    Prueba = new PruebaDinamica
                                        {
                                            PruebaID = pruebaID
                                        }
                                };
                            break;
                    }

                    resultadoPrueba = RetrieveComplete(dctx, detalleCicloEscolar, resultadoPrueba);

                    if (resultadoPrueba != null)
                        resultadosPrueba.Add(resultadoPrueba);
                }
            }
            return resultadosPrueba;
        }
        /// <summary>
        /// Recupera un registro completo AResultadoPrueba
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="detalleCicloEscolar"></param>
        /// <param name="resultadoPrueba"></param>
        /// <returns>AResultadoPrueba concreto, null en caso de no encontrar</returns>
        public AResultadoPrueba RetrieveComplete(IDataContext dctx, DetalleCicloEscolar detalleCicloEscolar, AResultadoPrueba resultadoPrueba)
        {
            if (detalleCicloEscolar == null) throw new ArgumentNullException("El detalle no puede ser nulo");
            if (resultadoPrueba == null) throw new ArgumentNullException("El resultado no puede ser nulo");

            AResultadoPrueba resultadoPruebaComplete = null;
            ExpedienteEscolarCtrl expedienteEscolarCtrl = new ExpedienteEscolarCtrl();
            DataSet dsResultadoPrueba = expedienteEscolarCtrl.Retrieve(dctx, resultadoPrueba, detalleCicloEscolar);

            if (dsResultadoPrueba.Tables[0].Rows.Count > 0)
            {
                PruebaDiagnosticoCtrl pruebaDiagnosticoCtrl = new PruebaDiagnosticoCtrl();

                int resultadoPruebaID = dsResultadoPrueba.Tables[0].Rows[dsResultadoPrueba.Tables[0].Rows.Count - 1].Field<int>("ResultadoPruebaID");
                ETipoPrueba tipoPrueba = (ETipoPrueba)dsResultadoPrueba.Tables[0].Rows[dsResultadoPrueba.Tables[0].Rows.Count - 1].Field<byte>("TipoPrueba");
                int pruebaID = dsResultadoPrueba.Tables[0].Rows[dsResultadoPrueba.Tables[0].Rows.Count - 1].Field<int>("PruebaID");
                switch (tipoPrueba)
                {            
                    case ETipoPrueba.Dinamica:
                        resultadoPruebaComplete = new ResultadoPruebaDinamica
                            {
                                ResultadoPruebaID = resultadoPruebaID,
                                Prueba = new PruebaDinamica
                                    {
                                        PruebaID = pruebaID
                                    }
                            };
                        return pruebaDiagnosticoCtrl.RetrieveCompleteResultadoPrueba(dctx, resultadoPruebaComplete);
                        break;
                }

            }
            return null;
        }
        /// <summary>
        /// Consulta un registro de Expediente escolar simple con sus relaciones perezosas.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="expedienteEscolar"></param>
        /// <returns></returns>
        public ExpedienteEscolar RetrieveSimple(IDataContext dctx, ExpedienteEscolar expedienteEscolar)
        {
            ExpedienteEscolarCtrl ctrl = new ExpedienteEscolarCtrl();
            DataSet dsExpediente = ctrl.Retrieve(dctx, expedienteEscolar);
            if (dsExpediente.Tables[0].Rows.Count > 0)
                return ctrl.LastDataRowToExpedienteEscolar(dsExpediente);
            else
                return null;
        }
        /// <summary>
        /// Consulta un registro de detalleEscolar simple
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="expedienteEscolar"></param>
        /// <param name="detalleCicloEscolar"></param>
        /// <returns></returns>
        public DetalleCicloEscolar RetrieveSimple(IDataContext dctx, ExpedienteEscolar expedienteEscolar, DetalleCicloEscolar detalleCicloEscolar)
        {
            ExpedienteEscolarCtrl ctrl = new ExpedienteEscolarCtrl();
            DataSet dsDetalle = ctrl.Retrieve(dctx, detalleCicloEscolar, expedienteEscolar);
            if (dsDetalle.Tables[0].Rows.Count > 0)
                return ctrl.LastDataRowToDetalleCicloEscolar(dsDetalle);
            else
                return null;
        }
        /// <summary>
        /// Consulta un registro de asignacion de recurso simple
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="detalleCicloEscolar"></param>
        /// <param name="asignacionRecurso"></param>
        /// <returns></returns>
        public AAsignacionRecurso RetrieveSimple(IDataContext dctx, DetalleCicloEscolar detalleCicloEscolar, AAsignacionRecurso asignacionRecurso)
        {
            if (detalleCicloEscolar == null) throw new ArgumentNullException("El detalle no puede ser nulo");
            if (asignacionRecurso == null) throw new ArgumentNullException("La asignacion no puede ser nula no puede ser nulo");
            if (detalleCicloEscolar.DetalleCicloEscolarID == null) throw new ArgumentNullException("El identificador detalle no puede ser nulo");
            ExpedienteEscolarCtrl expedienteEscolarCtrl = new ExpedienteEscolarCtrl();

            AsignacionRecursoCtrl asignacionRecursoCtrl = new AsignacionRecursoCtrl();



            DataSet dsAsignacioRecurso = asignacionRecursoCtrl.RetrieveAAsignacionRecurso(dctx, detalleCicloEscolar, asignacionRecurso);
            if (dsAsignacioRecurso.Tables[0].Rows.Count > 0)
            {
                return asignacionRecursoCtrl.LastDataRowToAsignacionRecurso(dsAsignacioRecurso);
            }
            return null;
        }
        /// <summary>
        /// Consulta un registro resultado prueba simple
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="detalleCicloEscolar"></param>
        /// <param name="resultadoPrueba"></param>
        /// <returns></returns>
        public AResultadoPrueba RetrieveSimple(IDataContext dctx, DetalleCicloEscolar detalleCicloEscolar, AResultadoPrueba resultadoPrueba)
        {
            return null;
        }
        /// <summary>
        /// Insera un registro completo de detalleCicloEscolar en la BD
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="detalleCicloEscolar"></param>
        public void InsertComplete(IDataContext dctx, DetalleCicloEscolar detalleCicloEscolar)
        {
        }
         /// <summary>
        /// Actualiza un registro completo de AAsignacionRecurso
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="asignacionRecurso"></param>
        /// <param name="previous"></param>
        public void UpdateComplete(IDataContext dctx, AAsignacionRecurso asignacionRecurso, AAsignacionRecurso previous)
        {

        }
        /// <summary>
        /// Inserta una asignacion de recurso para un alumno en su expediente
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="alumno">Alumno</param>
        /// <param name="escuela">Escuela del alumno</param>
        /// <param name="grupoCicloEscolar">Grupo ciclo escolar del alumno</param>
        /// <param name="asignacionRecurso">Asignacion concreta del alumno</param>
        public void InsertAAsignacionRecurso(IDataContext dctx, Alumno alumno, Escuela escuela, GrupoCicloEscolar grupoCicloEscolar, AAsignacionRecurso asignacionRecurso)
        {
            #region *** validaciones ***
            if (escuela == null) throw new ArgumentNullException("Escuela requerida");
            if (escuela.EscuelaID == null) throw new ArgumentNullException("Identificador de escuela requerido");
            if (alumno == null) throw new ArgumentNullException("Alumno requerido");
            if (alumno.AlumnoID == null) throw new ArgumentNullException("Identificador de alumno requerido");
            if (grupoCicloEscolar == null) throw new ArgumentNullException("Grupociclo escolar requerido");
            if (grupoCicloEscolar.GrupoCicloEscolarID == null) throw new ArgumentNullException("Identificador de grupo ciclo escolar requerido");
            if (asignacionRecurso == null) throw new ArgumentNullException("La asignacion es requerida");
            #endregion
            object myFirm = new object();
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            try
            {
                #region *** Expediente ***
                ExpedienteEscolarCtrl expedienteEscolarCtrl = new ExpedienteEscolarCtrl();

                ExpedienteEscolar expediente = RetrieveSimple(dctx, new ExpedienteEscolar
                {
                    Alumno = alumno
                });
                if (expediente != null)
                {
                    if (!expediente.Activo.Value)
                    {
                        expediente.Activo = true;
                        expedienteEscolarCtrl.Update(dctx, expediente, expediente);
                    }
                }
                else
                {
                    expediente = new ExpedienteEscolar
                    {
                        Activo = true,
                        FechaRegistro = DateTime.Now,
                        Alumno = alumno
                    };

                    expedienteEscolarCtrl.Insert(dctx, expediente);

                    expediente = RetrieveSimple(dctx, expediente);
                }
                #endregion

                #region *** detalle ciclo escolar ***
                DetalleCicloEscolar detalle = new DetalleCicloEscolar
                {
                    Escuela = escuela,
                    GrupoCicloEscolar = grupoCicloEscolar
                };
                DetalleCicloEscolar detalleCiclo = RetrieveSimple(dctx, expediente, detalle);
                //si existe intenta reactivarlo, si no existe lo crea.
                if (detalleCiclo != null)
                {

                    if (!detalleCiclo.Activo.Value)
                    {
                        detalleCiclo.Activo = true;
                        expedienteEscolarCtrl.Update(dctx, detalleCiclo, detalleCiclo, expediente);
                    }
                }
                else
                {
                    detalleCiclo = new DetalleCicloEscolar();
                    detalleCiclo.Escuela = escuela;
                    detalleCiclo.GrupoCicloEscolar = grupoCicloEscolar;
                    detalleCiclo.Activo = true;
                    detalleCiclo.FechaRegistro = DateTime.Now;
                    expedienteEscolarCtrl.Insert(dctx, detalleCiclo, expediente);
                    detalleCiclo = RetrieveSimple(dctx, expediente, detalleCiclo);
                }
                #endregion

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
    }
}
