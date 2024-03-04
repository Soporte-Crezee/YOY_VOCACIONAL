 using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Prueba.Diagnostico.Dinamica.DAO;
using POV.Prueba.Diagnostico.Dinamica.DA;
using POV.Expediente.BO;
using POV.Expediente.Service;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Prueba.Calificaciones.BO;
using POV.Modelo.BO;
using POV.Prueba.Calificaciones.Services;
using POV.Prueba.BO;
using POV.Seguridad.BO;

namespace POV.Prueba.Diagnostico.Dinamica.Service
{
    /// <summary>
    /// Controlador del objeto ResultadoPruebaDinamica
    /// </summary>
    public class ResultadoPruebaDinamicaCtrl
    {

        /// <summary>
        /// Consulta un registro completo de resultado prueba dinamica
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="detalleCicloEscolar">detalle ciclo escolar que se usara como filtro</param>
        /// <param name="resultadoPrueba">resultado de prueba que se usara como filtro</param>
        /// <returns>Resultado de prueba dinamica, null en caso de no encontrar coincidencias</returns>
        public ResultadoPruebaDinamica RetrieveComplete(IDataContext dctx, DetalleCicloEscolar detalleCicloEscolar, ResultadoPruebaDinamica resultadoPrueba)
        {
            if (detalleCicloEscolar == null) throw new ArgumentNullException("RespuestaReactivoDinamicaCtrl: DetalleCicloEscolar no puede ser nulo");
            if (resultadoPrueba == null) throw new ArgumentNullException("RespuestaReactivoDinamicaCtrl: ResultadoPruebaDinamica no puede ser nulo");

            ResultadoPruebaDinamica resultadoCompleto = new ResultadoPruebaDinamica();
            DataSet dsResultadoPrueba = Retrieve(dctx, detalleCicloEscolar, resultadoPrueba);
            if (dsResultadoPrueba.Tables[0].Rows.Count > 0)
            {
                resultadoCompleto = LastDataRowToResultadoPruebaDinamica(dsResultadoPrueba);

                RegistroPruebaDinamicaCtrl registroCtrl = new RegistroPruebaDinamicaCtrl();
                resultadoCompleto.RegistroPrueba = registroCtrl.RetrieveComplete(dctx, resultadoCompleto, new RegistroPruebaDinamica());

                PruebaDinamicaCtrl pruebaCtrl = new PruebaDinamicaCtrl();
                resultadoCompleto.Prueba = pruebaCtrl.RetrieveComplete(dctx, resultadoCompleto.Prueba as PruebaDinamica, false);

                AlumnoCtrl alumnoCtrl = new AlumnoCtrl();
                resultadoCompleto.Alumno = alumnoCtrl.LastDataRowToAlumno(alumnoCtrl.Retrieve(dctx, resultadoCompleto.Alumno));

            }

            return resultadoCompleto;
        }
        /// <summary>
        /// Recupera un registro completo de resultado prueba dinamica en base a los filtros del expediente escolar
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos (Requerido)<</param>
        /// <param name="alumno">Alumno que se usara como filtro de busqueda (Requerido)</param>
        /// <param name="escuela">escuela que se usara como filtro de busqueda (Requerido)<</param>
        /// <param name="grupoCicloEscolar">grupo ciclo escolar que se usara como filtro de busqueda (Requerido)<</param>
        /// <param name="prueba">prueba que se usara como filtro de busqueda (Requerido)<</param>
        /// <returns>ResultadoPruebaDinamica de la base de datos, null en caso de no encontrar coincidencias</returns>
        public ResultadoPruebaDinamica RetrieveResultadoPruebaDinamica(IDataContext dctx, Alumno alumno, 
            Escuela escuela, GrupoCicloEscolar grupoCicloEscolar, PruebaDinamica prueba, ETipoResultadoPrueba tipoResultado)
        {
            #region *** validaciones ***
            if (alumno == null) throw new Exception("ResultadoPruebaDinamicaCtrl: El alumno es requerido");
            if (alumno.AlumnoID == null) throw new Exception("ResultadoPruebaDinamicaCtrl: El identificador del alumno es requerido");
            if (escuela == null) throw new Exception("ResultadoPruebaDinamicaCtrl: La escuela es requerida");
            if (escuela.EscuelaID == null) throw new Exception("ResultadoPruebaDinamicaCtrl: el identificador de la escuela es requerido");
            if (grupoCicloEscolar == null) throw new Exception("ResultadoPruebaDinamicaCtrl: El grupo ciclo escolar es requerido");
            if (grupoCicloEscolar.GrupoCicloEscolarID == null) throw new Exception("ResultadoPruebaDinamicaCtrl: El identificador del ciclo escolar es requerido");
            if (prueba == null) throw new Exception("ResultadoPruebaDinamicaCtrl: La prueba asignada es requerido");
            if (prueba.PruebaID == null) throw new Exception("ResultadoPruebaDinamicaCtrl: El identificador de la prueba es requerido");

            #endregion

            ResultadoPruebaDinamica resultadoPrueba = new ResultadoPruebaDinamica();

            ExpedienteEscolarCtrl expedienteCtrl = new ExpedienteEscolarCtrl();

            DataSet dsExpediente = expedienteCtrl.Retrieve(dctx, new ExpedienteEscolar { Alumno = alumno });
            if (dsExpediente.Tables[0].Rows.Count > 0)
            {
                ExpedienteEscolar expedienteAlumno = expedienteCtrl.LastDataRowToExpedienteEscolar(dsExpediente);

                //verificamos si existe un detalle de resultado prueba diagnostica en el expediente
                DetalleCicloEscolar detalle = new DetalleCicloEscolar { Escuela = escuela, GrupoCicloEscolar = grupoCicloEscolar, Activo = true };

                DataSet dsDetalleAlumno = expedienteCtrl.Retrieve(dctx, detalle, expedienteAlumno);

                if (dsDetalleAlumno.Tables[0].Rows.Count > 0)//si tiene un detalle para este ciclo escolar
                {
                    detalle = expedienteCtrl.LastDataRowToDetalleCicloEscolar(dsDetalleAlumno);

                    //verificamos si tiene un resultado de prueba asignada
                    DataSet dsResultadoPrueba = expedienteCtrl.Retrieve(dctx, new ResultadoPruebaDinamica { Prueba = prueba, Tipo = tipoResultado }, detalle);

                    if (dsResultadoPrueba.Tables[0].Rows.Count > 0) // si tiene un resultado de prueba diagnostica
                    {
                        DataRow row = dsResultadoPrueba.Tables[0].Rows[dsResultadoPrueba.Tables[0].Rows.Count - 1];
                        resultadoPrueba.ResultadoPruebaID = int.Parse(row["ResultadoPruebaID"].ToString());

                        resultadoPrueba = RetrieveComplete(dctx, detalle, resultadoPrueba);

                    }
                }
            }

            return resultadoPrueba;
        }

        /// <summary>
        /// Consulta registros con información de Resultado de la Prueba en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="escuela">Escuela que provee el criterio de selección para realizar la consulta</param>
        /// <param name="grupoCicloEscolar">grupoCicloEscolar que provee el criterio de selección para realizar la consulta</param>
        /// <param name="prueba">Prueba que provee el criterio de selección para realizar la consulta</param>
        /// <param name="alumno">Alumno que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de Resultado de la Prueba generada por la consulta</returns>
        public DataSet RetrieveCompleteResultadoPruebaDinamica(IDataContext dctx, 
             Escuela escuela, GrupoCicloEscolar grupoCicloEscolar, PruebaDinamica prueba, Alumno alumno)
        {
            #region Validaciones
            if (escuela == null) throw new Exception("ResultadoPruebaDinamicaCtrl: El alumno es requerido");
            if (escuela.EscuelaID == null) throw new Exception("ResultadoPruebaDinamicaCtrl: El identificador del alumno es requerido");            
            if (prueba == null) throw new Exception("ResultadoPruebaDinamicaCtrl: La prueba asignada es requerido");
            if (prueba.PruebaID == null) throw new Exception("ResultadoPruebaDinamicaCtrl: El identificador de la prueba es requerido");
            if (prueba.Modelo == null) throw new Exception("ResultadoPruebaDinamicaCtrl: El modelo de la prueba es requerido");
            if ((prueba.Modelo as ModeloDinamico).MetodoCalificacion == null) throw new Exception("ResultadoPruebaDinamicaCtrl: El método de calificación del modelo de la prueba es requerido");
            #endregion
            DataSet dsReturn = new DataSet();

            ResultadoPruebaModeloAlumnoDARetHlp da = new ResultadoPruebaModeloAlumnoDARetHlp();
            DataSet ds1 = da.Action(dctx, escuela, grupoCicloEscolar, prueba, alumno);

            DataSet ds2 = new DataSet();            
            ModeloDinamico modeloDinamico = (ModeloDinamico)prueba.Modelo;
            switch (modeloDinamico.MetodoCalificacion)
            {
                case EMetodoCalificacion.CLASIFICACION:
                    ResultadoCalificaPruebaModeloAlumnoClasificaDARetHlp daResultadoClasificacion = new ResultadoCalificaPruebaModeloAlumnoClasificaDARetHlp();
                    ds2 = daResultadoClasificacion.Action(dctx, escuela, grupoCicloEscolar, prueba, alumno);
                    break;                
                case EMetodoCalificacion.SELECCION:
                    ResultadoCalificaPruebaModeloAlumnoSeleccionDARetHlp daResultadoSeleccion = new ResultadoCalificaPruebaModeloAlumnoSeleccionDARetHlp();
                    ds2 = daResultadoSeleccion.Action(dctx, escuela, grupoCicloEscolar, prueba, alumno);
                    break;
            }

            if (ds1 != null && ds1.Tables.Count > 0)
            {
                dsReturn.Tables.Add(ds1.Tables[0].Copy());
                if (ds2 != null && ds2.Tables.Count > 0)
                    dsReturn.Tables.Add(ds2.Tables[0].Copy());
            }

            return dsReturn;
        }
        /// <summary>
        /// Consulta los resultados de una prueba dinamica por  escuela, grupo,prueba y ciclo escolar
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos </param>
        /// <param name="escuela">Escuela que se usará como filtro de búsqueda, requerido</param>
        /// <param name="prueba">Prueba que se usará como filtro de búsqueda,requerido</param>
        /// <param name="grupoCicloEscolar">GrupoCicloEscolar que se usará como filtro de búsqueda,opcional</param>
        /// <returns>El DataSet que contiene los resultados de la consulta</returns>
        public DataSet RetrieveResultadoPruebaDinamicaEscuela(IDataContext dctx, Escuela escuela, PruebaDinamica prueba, GrupoCicloEscolar grupoCicloEscolar)
        {

            if (escuela == null) throw new Exception("ReportePruebaDinamicaCtrl: La escuela es requerida");
            if (escuela.EscuelaID == null) throw new Exception("ReportePruebaDinamicaCtrl: el identificador de la escuela es requerido");
            if (prueba == null) throw new Exception("ReportePruebaDinamicaCtrl: La prueba asignada es requerido");
            if (prueba.PruebaID == null) throw new Exception("ReportePruebaDinamicaCtrl: El identificador de la prueba es requerido");
            if (grupoCicloEscolar == null)
                throw new Exception("ReportePruebaDinamicaCtrl: Grupo ciclo escolar es requerido");
            if (grupoCicloEscolar.CicloEscolar == null)
                throw new Exception("ReportePruebaDinamicaCtrl: Ciclo escolar no puede ser vacío");
            if (grupoCicloEscolar.CicloEscolar.CicloEscolarID == null)
                throw new Exception("ReportePruebaDinamicaCtrl: Identificador del ciclo escolar  no puede ser vacío");

            ResultadoPruebaDinamicaEscuelaDARetHlp da = new ResultadoPruebaDinamicaEscuelaDARetHlp();
            DataSet ds = da.Action(dctx, escuela, grupoCicloEscolar, prueba);
            return ds;
        }
        /// <summary>
        /// Consulta los resultados de una prueba dinamica por  escuela, grupo,prueba y ciclo escolar
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos </param>
        /// <param name="escuela">Escuela que se usará como filtro de búsqueda, requerido</param>
        /// <param name="prueba">Prueba que se usará como filtro de búsqueda,requerido</param>
        /// <param name="grupoCicloEscolar">GrupoCicloEscolar que se usará como filtro de búsqueda,opcional</param>
        /// <returns>El DataSet que contiene los resultados de la consulta</returns>
        public DataSet RetrieveResultadoPruebaDinamicaGrupo(IDataContext dctx, Escuela escuela, PruebaDinamica prueba, GrupoCicloEscolar grupoCicloEscolar)
        {

            if (escuela == null) throw new Exception("ReportePruebaDinamicaCtrl: La escuela es requerida");
            if (escuela.EscuelaID == null) throw new Exception("ReportePruebaDinamicaCtrl: el identificador de la escuela es requerido");
            if (prueba == null) throw new Exception("ReportePruebaDinamicaCtrl: La prueba asignada es requerido");
            if (prueba.PruebaID == null) throw new Exception("ReportePruebaDinamicaCtrl: El identificador de la prueba es requerido");
            if (grupoCicloEscolar == null)
                throw new Exception("ReportePruebaDinamicaCtrl: Grupo ciclo escolar es requerido");
            if (grupoCicloEscolar.CicloEscolar == null)
                throw new Exception("ReportePruebaDinamicaCtrl: Ciclo escolar no puede ser vacío");
            if (grupoCicloEscolar.CicloEscolar.CicloEscolarID == null)
                throw new Exception("ReportePruebaDinamicaCtrl: Identificador del ciclo escolar  no puede ser vacío");

            ResultadoPruebaDinamicaGrupoDARetHlp da = new ResultadoPruebaDinamicaGrupoDARetHlp();
            DataSet ds = da.Action(dctx, escuela, grupoCicloEscolar, prueba);
            return ds;
        }

        public DataSet RetrieveResultadoPruebaDinamicaGrupo(IDataContext dctx, Escuela escuela, GrupoCicloEscolar grupoCicloEscolar, PruebaDinamica prueba)
        {
            if (escuela == null) throw new Exception("ReportePruebaDinamicaCtrl: La escuela es requerida");
            if (escuela.EscuelaID == null) throw new Exception("ReportePruebaDinamicaCtrl: el identificador de la escuela es requerido");
            if (prueba == null) throw new Exception("ReportePruebaDinamicaCtrl: La prueba asignada es requerido");
            if (prueba.PruebaID == null) throw new Exception("ReportePruebaDinamicaCtrl: El identificador de la prueba es requerido");
            if (grupoCicloEscolar == null)
                throw new Exception("ReportePruebaDinamicaCtrl: Grupo ciclo escolar es requerido");
            if (grupoCicloEscolar.CicloEscolar == null)
                throw new Exception("ReportePruebaDinamicaCtrl: Ciclo escolar no puede ser vacío");
            if (grupoCicloEscolar.CicloEscolar.CicloEscolarID == null)
                throw new Exception("ReportePruebaDinamicaCtrl: Identificador del ciclo escolar  no puede ser vacío");

            ResultadoPruebaDinamicaGrupoDARetHlp da = new ResultadoPruebaDinamicaGrupoDARetHlp();
            DataSet ds = da.Action(dctx, escuela, grupoCicloEscolar, prueba);
            return ds;
        }

        public DataSet RetrieveResultadorPruebaSACKS(IDataContext dctx, Alumno alumno, APrueba prueba)
        {
            ResultadoPruebaSACKSDARetHlp da = new ResultadoPruebaSACKSDARetHlp();
            DataSet ds = da.Action(dctx, alumno, prueba);
            return ds;
        }

        public DataSet RetrieveResultadorPruebaFrasesVocacionales(IDataContext dctx, Alumno alumno, APrueba prueba)
        {
            ResultadoPruebaFrasesVocacionalesDARetHlp da = new ResultadoPruebaFrasesVocacionalesDARetHlp();
            DataSet ds = da.Action(dctx, alumno, prueba);
            return ds;
        }

        public DataSet RetrieveAlumnosPruebas(IDataContext dctx, Usuario usuario, APrueba prueba, String nombreCompletoAlumno = "")
        {
            PruebasAlumnosDARetHlp da = new PruebasAlumnosDARetHlp();
            DataSet ds = da.Action(dctx, usuario, prueba, nombreCompletoAlumno);
            return ds;
        }

        
 
        public DataSet RetrieveAlumnosPruebas(IDataContext dctx, Tutor tutor, APrueba prueba, String nombreCompletoAlumno = "")
        {
            PruebasAlumnosDARetHlp da = new PruebasAlumnosDARetHlp();
            DataSet ds = da.Action(dctx, tutor, prueba, nombreCompletoAlumno);
            return ds;
        }

        public DataSet RetrieveAlumnosPruebas(IDataContext dctx, Alumno alumno, APrueba prueba, String nombreCompletoAlumno = "")
        {
            PruebasAlumnosDARetHlp da = new PruebasAlumnosDARetHlp();
            DataSet ds = da.Action(dctx, alumno, prueba, nombreCompletoAlumno);
            return ds;
        }

        public DataSet RetrieveAlumnosPruebasSacks(IDataContext dctx, Usuario usuario, APrueba prueba, String nombreCompletoAlumno = "")
        {
            PruebaAlumnosSacksDARetHlp da = new PruebaAlumnosSacksDARetHlp();
            DataSet ds = da.Action(dctx, usuario, prueba, nombreCompletoAlumno);
            return ds;
        }

        public DataSet RetrieveAlumnosPruebasSacks(IDataContext dctx, Tutor tutor, APrueba prueba, String nombreCompletoAlumno = "")
        {
            PruebaAlumnosSacksDARetHlp da = new PruebaAlumnosSacksDARetHlp();
            DataSet ds = da.Action(dctx, tutor, prueba, nombreCompletoAlumno);
            return ds;
        }

        public DataSet RetrieveAlumnosPruebasFrasesVocacionales(IDataContext dctx, Usuario usuario, APrueba prueba, String nombreCompletoAlumno = "")
        {
            PruebaAlumnosFrasesVocacionalesDARetHlp da = new PruebaAlumnosFrasesVocacionalesDARetHlp();
            DataSet ds = da.Action(dctx, usuario, prueba, nombreCompletoAlumno);
            return ds;
        }

        public DataSet RetrieveAlumnosPruebasFrasesVocacionales(IDataContext dctx, Tutor tutor, APrueba prueba, String nombreCompletoAlumno = "")
        {
            PruebaAlumnosFrasesVocacionalesDARetHlp da = new PruebaAlumnosFrasesVocacionalesDARetHlp();
            DataSet ds = da.Action(dctx, tutor, prueba, nombreCompletoAlumno);
            return ds;
        }

        #region Cleaver
        public DataSet RetrieveResultadoPruebaCleaver(IDataContext dctx, Alumno alumno, APrueba prueba)
        {
            ResultadoPruebaCleaverDARetHlp da = new ResultadoPruebaCleaverDARetHlp();
            DataSet ds = da.Action(dctx, alumno, prueba);
            return ds;
        }
        public DataSet RetrievePlantillaResultadoCleaver(IDataContext dctx, PlantillaResultadoCleaver result)
        {
            PlantillaResultadoCleaverRetHlp da = new PlantillaResultadoCleaverRetHlp();
            DataSet ds = da.Action(dctx, result);
            return ds;
        }
        /// <summary>
        /// Crea un objeto de ResultadoPruebaDinamica a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de ResultadoPruebaDinamica</param>
        /// <returns>Un objeto de ResultadoPruebaDinamica creado a partir de los datos</returns>
        public PlantillaResultadoCleaver LastDataRowToPlantillaResultadoCleaver(DataSet ds)
        {
            if (!ds.Tables.Contains("PlantillaResultadoCleaver"))
                throw new Exception("LastDataRowToPlantillaResultadoCleaver: DataSet no tiene la tabla PlantillaResultadoCleaver");
            int index = ds.Tables["PlantillaResultadoCleaver"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToPlantillaResultadoCleaver: El DataSet no tiene filas");
            return this.DataRowToPlantillaResultadoCleaver(ds.Tables["PlantillaResultadoCleaver"].Rows[index - 1]);
        }
        /// <summary>
        /// Crea un objeto de ResultadoPruebaDinamica a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de ResultadoPruebaDinamica</param>
        /// <returns>Un objeto de ResultadoPruebaDinamica creado a partir de los datos</returns>
        public PlantillaResultadoCleaver DataRowToPlantillaResultadoCleaver(DataRow row)
        {
            PlantillaResultadoCleaver PlantillaResultadoCleaver = new PlantillaResultadoCleaver();
            if (row.IsNull("PlantillaResultadoCleaverID"))
                PlantillaResultadoCleaver.PlantillaResultadoCleaverID = null;
            else
                PlantillaResultadoCleaver.PlantillaResultadoCleaverID = (int)Convert.ChangeType(row["PlantillaResultadoCleaverID"], typeof(int));
            if (row.IsNull("Tag"))
                PlantillaResultadoCleaver.Tag = null;
            else
                PlantillaResultadoCleaver.Tag = (String)Convert.ChangeType(row["Tag"], typeof(String));
            if (row.IsNull("Opcion"))
                PlantillaResultadoCleaver.Opcion = null;
            else
                PlantillaResultadoCleaver.Opcion= (String)Convert.ChangeType(row["Opcion"], typeof(String));
            if (row.IsNull("Valor"))
                PlantillaResultadoCleaver.Valor = null;
            else
                PlantillaResultadoCleaver.Valor = (int)Convert.ChangeType(row["Valor"], typeof(int));
            if (row.IsNull("Porcentaje"))
                PlantillaResultadoCleaver.Porcentaje = null;
            else
                PlantillaResultadoCleaver.Porcentaje = (int)Convert.ChangeType(row["Porcentaje"], typeof(int));

            return PlantillaResultadoCleaver;
        }
        #endregion

        #region CHASIDE
        public DataSet RetrieveResultadoPruebaCHASIDE(IDataContext dctx, Alumno alumno, APrueba prueba, int? grupo)
        {
            ResultadoPruebaCHASIDEDARetHlp da = new ResultadoPruebaCHASIDEDARetHlp();
            DataSet ds = da.Action(dctx, alumno, prueba, grupo);
            return ds;
        }
        /// <summary>
        /// Crea un objeto de ResultadoPruebaDinamica a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de ResultadoPruebaDinamica</param>
        /// <returns>Un objeto de ResultadoPruebaDinamica creado a partir de los datos</returns>
        public ResultadoPruebaChaside LastDataRowToPlantillaResultadoChaside(DataSet ds)
        {
            if (!ds.Tables.Contains("ResultadoPruebaChaside"))
                throw new Exception("LastDataRowToPlantillaResultadoChaside: DataSet no tiene la tabla PlantillaResultadoCleaver");
            int index = ds.Tables["ResultadoPruebaChaside"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToPlantillaResultadoChaside: El DataSet no tiene filas");
            return this.DataRowToResultadoPruebaChaside(ds.Tables["ResultadoPruebaChaside"].Rows[index - 1]);
        }
        /// <summary>
        /// Crea un objeto de ResultadoPruebaDinamica a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de ResultadoPruebaDinamica</param>
        /// <returns>Un objeto de ResultadoPruebaDinamica creado a partir de los datos</returns>
        public ResultadoPruebaChaside DataRowToResultadoPruebaChaside(DataRow row)
        {
            ResultadoPruebaChaside obj = new ResultadoPruebaChaside();
            if (row.IsNull("ClasificadorID"))
                obj.ClasificadorID = null;
            else
                obj.ClasificadorID = (int)Convert.ChangeType(row["ClasificadorID"], typeof(int));
            if (row.IsNull("Grupo"))
                obj.Grupo = null;
            else
                obj.Grupo = (String)Convert.ChangeType(row["Grupo"], typeof(String));
            if (row.IsNull("Nombre"))
                obj.Nombre = null;
            else
                obj.Nombre = (String)Convert.ChangeType(row["Nombre"], typeof(String));
            if (row.IsNull("Calificacion"))
                obj.Calificacion = null;
            else
                obj.Calificacion = (int)Convert.ChangeType(row["Calificacion"], typeof(int));

            return obj;
        }
        #endregion

        #region Bullying
        public DataSet RetrieveAlumnosPruebasBullying(IDataContext dctx, Usuario usuario, List<PruebaDinamica> prueba, String nombreCompletoAlumno = "")
        {
            PruebasAlumnosDARetHlp da = new PruebasAlumnosDARetHlp();

            string strPruebas = string.Empty;
            if (prueba.Count > 0)
            {
                foreach (var item in prueba)
                {
                    strPruebas += "," + item.PruebaID;
                }
                if (strPruebas.StartsWith(","))
                    strPruebas = strPruebas.Substring(1);
            }

            DataSet ds = da.Action(dctx, usuario, strPruebas, nombreCompletoAlumno);
            return ds;
        }
        #endregion

        #region Metodos de aplicacion de una prueba
        /// <summary>
        /// Verifica si para un alumno dentro su expediente existe la prueba dinamica pendiente en el sistema
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos (Requerido)<</param>
        /// <param name="alumno">Alumno que se usara como filtro de busqueda (Requerido)</param>
        /// <param name="escuela">escuela que se usara como filtro de busqueda (Requerido)<</param>
        /// <param name="grupoCicloEscolar">grupo ciclo escolar que se usara como filtro de busqueda (Requerido)<</param>
        /// <param name="prueba">prueba que se usara como filtro de busqueda (Requerido)<</param>
        /// <returns></returns>
        public bool TienePruebaPendiente(IDataContext dctx, Alumno alumno, Escuela escuela, GrupoCicloEscolar grupoCicloEscolar, PruebaDinamica prueba, ETipoResultadoPrueba tipoResultado)
        {
            bool pruebaPendiente = true;
            //consultamos el resultado de la prueba
            ResultadoPruebaDinamica resultadoPrueba = RetrieveResultadoPruebaDinamica(dctx, alumno, escuela, grupoCicloEscolar, prueba, tipoResultado);

            if (resultadoPrueba.ResultadoPruebaID != null) // si existe el resultado
            {
                //verificamos el estado de la prueba del alumno

                pruebaPendiente = resultadoPrueba.RegistroPrueba.EstadoPrueba != EEstadoPrueba.CERRADA;
            }

            return pruebaPendiente;
        }

        /// <summary>
        /// Crea un registro de la prueba por aplicar para un alumno dentro de su expediente
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos (Requerido)<</param>
        /// <param name="alumno">Alumno (Requerido)</param>
        /// <param name="escuela">escuela que se usara (Requerido)<</param>
        /// <param name="grupoCicloEscolar">grupo ciclo escolar del alumno(Requerido)<</param>
        /// <param name="prueba">prueba que se quiere crear (Requerido)<</param>
        /// <returns>ResultadoPruebaDinamica de la base de datos, null en caso de no encontrar coincidencias</returns>
        public ResultadoPruebaDinamica CreatePrueba(IDataContext dctx, Alumno alumno, Escuela escuela,
            GrupoCicloEscolar grupoCicloEscolar, PruebaDinamica prueba, ETipoResultadoPrueba tipo)
        {
            #region *** validaciones ***
            if (alumno == null) throw new Exception("ResultadoPruebaDinamicaCtrl: El alumno es requerido");
            if (alumno.AlumnoID == null) throw new Exception("ResultadoPruebaDinamicaCtrl: El identificador del alumno es requerido");
            if (escuela == null) throw new Exception("ResultadoPruebaDinamicaCtrl: La escuela es requerida");
            if (escuela.EscuelaID == null) throw new Exception("ResultadoPruebaDinamicaCtrl: el identificador de la escuela es requerido");
            if (grupoCicloEscolar == null) throw new Exception("ResultadoPruebaDinamicaCtrl: El grupo ciclo escolar es requerido");
            if (grupoCicloEscolar.GrupoCicloEscolarID == null) throw new Exception("ResultadoPruebaDinamicaCtrl: El identificador del ciclo escolar es requerido");
            if (prueba == null) throw new Exception("ResultadoPruebaDinamicaCtrl: La prueba asignada es requerido");
            if (prueba.PruebaID == null) throw new Exception("ResultadoPruebaDinamicaCtrl: El identificador de la prueba es requerido");

            #endregion

            PruebaDinamicaCtrl pruebaDinamicaCtrl = new PruebaDinamicaCtrl();
            BancoReactivosDinamicoCtrl bancoReactivosDinamicoCtrl = new BancoReactivosDinamicoCtrl();


            //Consultar prueba completa

            prueba = pruebaDinamicaCtrl.LastDataRowToPruebaDinamica(pruebaDinamicaCtrl.Retrieve(dctx, prueba, false));

            BancoReactivosDinamico bancoReactivos = bancoReactivosDinamicoCtrl.RetrieveComplete(dctx, new BancoReactivosDinamico { Prueba = prueba });

            ResultadoPruebaDinamicaFactory factory = new ResultadoPruebaDinamicaFactory();

            ResultadoPruebaDinamica resultadoPrueba = factory.CreateResultadoPrueba(alumno, bancoReactivos.GenerarListaReactivosPruebaConcreta()) as ResultadoPruebaDinamica;
            resultadoPrueba.Prueba = prueba;
            resultadoPrueba.Tipo = tipo;

            #region *** begin transaction ***
            object myFirm = new object();
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            try
            {
            #endregion

                ExpedienteEscolarCtrl expedienteCtrl = new ExpedienteEscolarCtrl();

                expedienteCtrl.InsertDetalleCicloEscolar(dctx, alumno, grupoCicloEscolar, escuela, resultadoPrueba);
                DataSet ds = expedienteCtrl.Retrieve(dctx, resultadoPrueba, new DetalleCicloEscolar());

                resultadoPrueba.ResultadoPruebaID = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1].Field<int>("ResultadoPruebaID");
                InsertComplete(dctx, resultadoPrueba);

                resultadoPrueba = RetrieveComplete(dctx, new DetalleCicloEscolar(), resultadoPrueba);



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
        /// Realiza el proceso de finalizar una prueba en curso
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos (Requerido)</param>
        /// <param name="resultadoPrueba">Registro de resultado de la prueba dinamica (Requerido)</param>
        public void FinalizarPrueba(IDataContext dctx, ResultadoPruebaDinamica resultadoPrueba)
        {
            if (resultadoPrueba == null)
            {
                throw new Exception("El resultado de la Prueba es Requerido");
            }

            if (resultadoPrueba.ResultadoPruebaID == null)
            {
                throw new Exception("El Identificador del Resultado de la Prueba es Requerido");
            }

            #region *** begin transaction ***
            object myFirm = new object();
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            try
            {
            #endregion
                ResultadoPruebaDinamica resultadoPruebaDinamica = RetrieveComplete(dctx, new DetalleCicloEscolar(), resultadoPrueba);
                RegistroPruebaDinamica registroPrueba = (resultadoPruebaDinamica.RegistroPrueba as RegistroPruebaDinamica);
                //Validamos que los registros esten finalizados
                if (registroPrueba.GetEstadoPruebaActual() != EEstadoPrueba.CERRADA)
                    throw new Exception("La Prueba no se Puede Finalizar");
                RegistroPruebaDinamicaCtrl registroPruebaCtrl = new RegistroPruebaDinamicaCtrl();

                //Identificamos la politica de calificación
                IPoliticaCalificacion politicaCalificacion = null;
                AResultadoMetodoCalificacion resultadoMetodoCalificacion= null;
                ModeloDinamico modeloDinamico = (ModeloDinamico)resultadoPruebaDinamica.Prueba.Modelo;
                switch (modeloDinamico.MetodoCalificacion)
                {
                    case EMetodoCalificacion.CLASIFICACION:
                        politicaCalificacion = new MetodoClasificacion();
                        resultadoMetodoCalificacion = (politicaCalificacion as MetodoClasificacion).Calcular(resultadoPruebaDinamica, resultadoPruebaDinamica.Prueba);
                        break;
                    case EMetodoCalificacion.PORCENTAJE:
                        politicaCalificacion = new MetodoPorcentaje();
                        resultadoMetodoCalificacion = (politicaCalificacion as MetodoPorcentaje).Calcular(resultadoPruebaDinamica, resultadoPruebaDinamica.Prueba);
                        break;
                    case EMetodoCalificacion.PUNTOS:
                        politicaCalificacion = new MetodoPuntos();
                        resultadoMetodoCalificacion = (politicaCalificacion as MetodoPuntos).Calcular(resultadoPruebaDinamica, resultadoPruebaDinamica.Prueba);
                        break;
                    case EMetodoCalificacion.SELECCION:
                        politicaCalificacion = new MetodoSeleccion();
                        resultadoMetodoCalificacion = (politicaCalificacion as MetodoSeleccion).Calcular(resultadoPruebaDinamica, resultadoPruebaDinamica.Prueba);
                        break;
                }

                //registramos el resultado del clasificador
                if (resultadoMetodoCalificacion != null)
                {
                    resultadoMetodoCalificacion.ResultadoPrueba = resultadoPruebaDinamica;

                    ResultadoMetodoCalificacionCtrl resultadoMetodoCtrl = new ResultadoMetodoCalificacionCtrl();
                    
                    ResultadoClasificadorDinamica resultadoClasificadorDinamica = new ResultadoClasificadorDinamica();
                    ResultadoClasificadorDinamicaCtrl resultadoClasificadorDinamicaCtrl = new ResultadoClasificadorDinamicaCtrl();
                    resultadoClasificadorDinamica.FechaRegistro = DateTime.Now;
                    resultadoClasificadorDinamica.Modelo = resultadoPruebaDinamica.Prueba.Modelo;
                    resultadoClasificadorDinamica.ResultadoPrueba = resultadoPruebaDinamica;
                    resultadoClasificadorDinamica.Clasificador = resultadoMetodoCalificacion.ObtenerClasificadorPredominante();
                    resultadoClasificadorDinamicaCtrl.InsertComplete(dctx, resultadoClasificadorDinamica);
                    resultadoMetodoCtrl.InsertComplete(dctx, resultadoMetodoCalificacion);
                }
                //Actualizamos la fecha y el estado de la prueba
                RegistroPruebaDinamica registroPruebaClonado = (RegistroPruebaDinamica)registroPrueba.Clone();
                registroPruebaClonado.FechaFin = DateTime.Now;
                registroPruebaClonado.EstadoPrueba = EEstadoPrueba.CERRADA;
                registroPruebaCtrl.Update(dctx, registroPruebaClonado, registroPrueba);
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
        #endregion

        /// <summary>
        /// Consulta registros de ResultadoPruebaDinamicaRetHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="resultadoPruebaDinamicaRetHlp">ResultadoPruebaDinamicaRetHlp que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de ResultadoPruebaDinamicaRetHlp generada por la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, DetalleCicloEscolar detalleCicloEscolar, ResultadoPruebaDinamica resultadoPrueba)
        {
            ResultadoPruebaDinamicaRetHlp da = new ResultadoPruebaDinamicaRetHlp();
            DataSet ds = da.Action(dctx, detalleCicloEscolar, resultadoPrueba);
            return ds;
        }
        /// <summary>
        /// Crea un registro de ResultadoPruebaDinamicaInsHlp en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="resultadoPruebaDinamicaInsHlp">ResultadoPruebaDinamicaInsHlp que desea crear</param>
        public void Insert(IDataContext dctx, ResultadoPruebaDinamica resultadoPrueba)
        {
            ResultadoPruebaDinamicaInsHlp da = new ResultadoPruebaDinamicaInsHlp();
            da.Action(dctx, resultadoPrueba);
        }
        /// <summary>
        /// Inserta un registro completo de un resultado de prueba dinamica
        /// </summary>
        /// <param name="dctx">El DataSet que contiene la información de ResultadoPruebaDinamica (Requerido)</param>
        /// <param name="resultadoPrueba">ResultadoPruebaDinamica que se desea insertar (Requerido)</param>
        public void InsertComplete(IDataContext dctx, ResultadoPruebaDinamica resultadoPrueba)
        {
            #region *** validaciones ***

            if (resultadoPrueba == null) throw new Exception("El resultado prueba es requerido");
            if (resultadoPrueba.ResultadoPruebaID == null) throw new Exception("El identificador del resultado prueba es requerido");
            if (resultadoPrueba.RegistroPrueba == null) throw new Exception("El registro de prueba es requerido");
            if (resultadoPrueba.RegistroPrueba.ListaRespuestaReactivos == null) throw new Exception("La lista de registros de reactivo es requerido");
            if (resultadoPrueba.RegistroPrueba.ListaRespuestaReactivos.Count == 0) throw new Exception("La lista de registros de reactivo no puede estar vacia");
            #endregion


            #region *** begin transaction ***
            object myFirm = new object();
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            try
            {
            #endregion

                Insert(dctx, resultadoPrueba);

                RegistroPruebaDinamicaCtrl registroPruebaCtrl = new RegistroPruebaDinamicaCtrl();
                registroPruebaCtrl.InsertComplete(dctx, resultadoPrueba, resultadoPrueba.RegistroPrueba as RegistroPruebaDinamica);

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
        /// Crea un objeto de ResultadoPruebaDinamica a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de ResultadoPruebaDinamica</param>
        /// <returns>Un objeto de ResultadoPruebaDinamica creado a partir de los datos</returns>
        public ResultadoPruebaDinamica LastDataRowToResultadoPruebaDinamica(DataSet ds)
        {
            if (!ds.Tables.Contains("ResultadoPrueba"))
                throw new Exception("LastDataRowToResultadoPruebaDinamica: DataSet no tiene la tabla ResultadoPruebaDinamica");
            int index = ds.Tables["ResultadoPrueba"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToResultadoPruebaDinamica: El DataSet no tiene filas");
            return this.DataRowToResultadoPruebaDinamica(ds.Tables["ResultadoPrueba"].Rows[index - 1]);
        }
        /// <summary>
        /// Crea un objeto de ResultadoPruebaDinamica a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de ResultadoPruebaDinamica</param>
        /// <returns>Un objeto de ResultadoPruebaDinamica creado a partir de los datos</returns>
        public ResultadoPruebaDinamica DataRowToResultadoPruebaDinamica(DataRow row)
        {
            ResultadoPruebaDinamica resultadoPruebaDinamica = new ResultadoPruebaDinamica();
            resultadoPruebaDinamica.Alumno = new Alumno();
            resultadoPruebaDinamica.Prueba = new PruebaDinamica();
            if (row.IsNull("ResultadoPruebaID"))
                resultadoPruebaDinamica.ResultadoPruebaID = null;
            else
                resultadoPruebaDinamica.ResultadoPruebaID = (int)Convert.ChangeType(row["ResultadoPruebaID"], typeof(int));
            if (row.IsNull("FechaRegistro"))
                resultadoPruebaDinamica.FechaRegistro = null;
            else
                resultadoPruebaDinamica.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
            if (row.IsNull("PruebaID"))
                resultadoPruebaDinamica.Prueba.PruebaID = null;
            else
                resultadoPruebaDinamica.Prueba.PruebaID = (int)Convert.ChangeType(row["PruebaID"], typeof(int));
            if (row.IsNull("AlumnoID"))
                resultadoPruebaDinamica.Alumno.AlumnoID = null;
            else
                resultadoPruebaDinamica.Alumno.AlumnoID = (long)Convert.ChangeType(row["AlumnoID"], typeof(long));
            if (row.IsNull("Tipo"))
                resultadoPruebaDinamica.Tipo = null;
            else
                resultadoPruebaDinamica.Tipo = (ETipoResultadoPrueba)(byte)Convert.ChangeType(row["Tipo"], typeof(byte));
            
            return resultadoPruebaDinamica;
        }
    }
}
