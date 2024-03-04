using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Linq;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Modelo.BO;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Prueba.Diagnostico.Dinamica.DAO;
using POV.Prueba.BO;
using POV.Modelo.Service;
using POV.Comun.BO;

namespace POV.Prueba.Diagnostico.Dinamica.Service
{
    /// <summary>
    /// Controlador del objeto PruebaDinamica
    /// </summary>
    public class PruebaDinamicaCtrl
    {

        #region Métodos para PruebaDinamica
        /// <summary>
        /// Consulta registros de PruebaDinamica en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="prueba">PruebaDinamica que provée el criterio de selección para realizar la consulta</param>
        /// <param name="lTodas">Null || false: solamente trae las pruebas activas y liberadas ó aquellas que cumplan con la propiedad EstadoLiberacionPrueba; 
        /// true: recupera todas las pruebas sin importar su EstadoLiberacionPrueba</param>
        /// <returns>El DataSet que contiene la información de PruebaDinamica generada por la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, PruebaDinamica prueba, bool? lTodas)
        {
            PruebaDinamicaRetHlp da = new PruebaDinamicaRetHlp();
            DataSet ds = da.Action(dctx, prueba, lTodas);
            return ds;
        }

        /// <summary>
        /// Consulta un registro completo de una PruebaDinamica
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="prueba">PruebaDinamica que provee el criterio de selección para realizar la consulta</param>
        /// <param name="lTodas">Null || false: solamente trae las pruebas activas y liberadas ó aquellas que cumplan con la propiedad EstadoLiberacionPrueba; 
        /// true: recupera todas las pruebas sin importar su EstadoLiberacionPrueba</param>
        /// <returns>Objeto completo de una PruebaDinamica o NULL si no se encuentra</returns>
        public PruebaDinamica RetrieveComplete(IDataContext dctx, PruebaDinamica prueba, bool? lTodas)
        {
            PruebaDinamica pruebaReturn = null;
            PruebaDinamica pruebaAux = new PruebaDinamica();

            DataSet dsPrueba = Retrieve(dctx, prueba, lTodas);

            if (dsPrueba != null && dsPrueba.Tables.Count > 0 && dsPrueba.Tables[0].Rows.Count > 0)
            {
                pruebaAux = this.LastDataRowToPruebaDinamica(dsPrueba);

                ModeloCtrl modeloCtrl = new ModeloCtrl();
                DataSet dsmodelo = modeloCtrl.Retrieve(dctx, new ModeloDinamico() { ModeloID = pruebaAux.Modelo.ModeloID });
                if (dsmodelo.Tables[0].Rows.Count > 0)
                    pruebaAux.Modelo = modeloCtrl.LastDataRowToModelo(dsmodelo);

                AEscalaDinamica escala = this.InstanciarPruebaDinamica(this.GetTipoEscalaDinamica((EMetodoCalificacion)((ModeloDinamico)pruebaAux.Modelo).MetodoCalificacion));
                List<AEscalaDinamica> escalas = this.RetrieveListEscalaDinamica(dctx, pruebaAux, escala);

                int idx = dsPrueba.Tables[0].Rows.Count;
                pruebaReturn = this.DataRowToPruebaDinamica(dsPrueba.Tables[0].Rows[idx - 1], escalas);
                pruebaReturn.Modelo = pruebaAux.Modelo;
            }
            return pruebaReturn;
        }

        /// <summary>
        /// Inserta un registro de PruebaDinamica en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="prueba">PruebaDinamica que provee el criterio de selección para realizar la consulta</param>
        public void Insert(IDataContext dctx, PruebaDinamica prueba)
        {
            PruebaDinamicaInsHlp da = new PruebaDinamicaInsHlp();
            da.Action(dctx, prueba);
        }

        /// <summary>
        /// Elimina un registro de PruebaDinamica en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="prueba">PruebaDinamica que será eliminada</param>
        public void Delete(IDataContext dctx, PruebaDinamica prueba)
        {
            PruebaDinamicaDelHlp da = new PruebaDinamicaDelHlp();
            da.Action(dctx, prueba);
        }

        /// <summary>
        /// Crea un objeto de PruebaDinamica a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de PruebaDinamica</param>
        /// <returns>Un objeto de PruebaDinamica creado a partir de los datos</returns>
        public PruebaDinamica LastDataRowToPruebaDinamica(DataSet ds)
        {
            if (!ds.Tables.Contains("Prueba"))
                throw new Exception("LastDataRowToPruebaDinamica: DataSet no tiene la tabla Prueba");
            int index = ds.Tables["Prueba"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToPruebaDinamica: El DataSet no tiene filas");
            return this.DataRowToPruebaDinamica(ds.Tables["Prueba"].Rows[index - 1]);
        }
        /// <summary>
        /// Crea un objeto de PruebaDinamica a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de PruebaDinamica</param>
        /// <returns>Un objeto de PruebaDinamica creado a partir de los datos</returns>
        public PruebaDinamica DataRowToPruebaDinamica(DataRow row)
        {
            return this.DataRowToPruebaDinamica(row, new List<APuntaje>());
        }
        /// <summary>
        /// Crea un objeto de PruebaDinamica a partir de los datos de un DataRow
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de PruebaDinamica</param>
        /// <param name="listaEscalaDinamica">Lista de RangoDesempeno que se agrega a PruebaDinamica</param>
        /// <returns>Un objeto de PruebaDinamica creado a partir de los datos y su lista de RangoDesempeno correspondiente</returns>
        public PruebaDinamica DataRowToPruebaDinamica(DataRow row, IEnumerable<APuntaje> listaEscalaDinamica)
        {
            PruebaDinamica pruebaDinamica = new PruebaDinamica(listaEscalaDinamica);
            pruebaDinamica.Modelo = new ModeloDinamico();
            if (row.IsNull("PruebaID"))
                pruebaDinamica.PruebaID = null;
            else
                pruebaDinamica.PruebaID = (int)Convert.ChangeType(row["PruebaID"], typeof(int));
            if (row.IsNull("Clave"))
                pruebaDinamica.Clave = null;
            else
                pruebaDinamica.Clave = (string)Convert.ChangeType(row["Clave"], typeof(string));
            if (row.IsNull("Nombre"))
                pruebaDinamica.Nombre = null;
            else
                pruebaDinamica.Nombre = (string)Convert.ChangeType(row["Nombre"], typeof(string));
            if (row.IsNull("Instrucciones"))
                pruebaDinamica.Instrucciones = null;
            else
                pruebaDinamica.Instrucciones = (string)Convert.ChangeType(row["Instrucciones"], typeof(string));
            if (row.IsNull("FechaRegistro"))
                pruebaDinamica.FechaRegistro = null;
            else
                pruebaDinamica.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
            if (row.IsNull("EsDiagnostica"))
                pruebaDinamica.EsDiagnostica = null;
            else
                pruebaDinamica.EsDiagnostica = (bool)Convert.ChangeType(row["EsDiagnostica"], typeof(bool));
            if (row.IsNull("ModeloID"))
                pruebaDinamica.Modelo.ModeloID = null;
            else
                pruebaDinamica.Modelo.ModeloID = (int)Convert.ChangeType(row["ModeloID"], typeof(int));
            if (row.IsNull("EstadoLiberacion"))
                pruebaDinamica.EstadoLiberacionPrueba = null;
            else
                pruebaDinamica.EstadoLiberacionPrueba = (EEstadoLiberacionPrueba)Convert.ChangeType(row["EstadoLiberacion"], typeof(byte));
            if (row.IsNull("EsPremium"))
                pruebaDinamica.EsPremium = null;
            else
                pruebaDinamica.EsPremium = (bool)Convert.ChangeType(row["EsPremium"], typeof(bool));
            if (row.IsNull("TipoPruebaPresentacion"))
                pruebaDinamica.TipoPruebaPresentacion = ETipoPruebaPresentacion.Dinamica;
            else
                pruebaDinamica.TipoPruebaPresentacion = (ETipoPruebaPresentacion)row.Field<byte>("TipoPruebaPresentacion");

            return pruebaDinamica;
        }
        #endregion

        #region Métodos de EscalaDinamica
        /// <summary>
        /// Consulta registros de PruebaDinamicaRetHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="pruebaDinamicaRetHlp">PruebaDinamicaRetHlp que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de PruebaDinamicaRetHlp generada por la consulta</returns>
        public DataSet RetrieveEscalaDinamica(IDataContext dctx, PruebaDinamica prueba, AEscalaDinamica escalaDinamica)
        {
            EscalaDinamicaRetHlp da = new EscalaDinamicaRetHlp();
            DataSet ds = da.Action(dctx, prueba, escalaDinamica);
            return ds;
        }

        /// <summary>
        /// Consulta registros de PruebaDinamicaRetHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="pruebaDinamicaRetHlp">PruebaDinamicaRetHlp que provee el criterio de selección para realizar la consulta</param>
        /// <returns>AEscalaDinamica que contiene la información de PruebaDinamicaRetHlp generada por la consulta</returns>
        public AEscalaDinamica RetrieveEscalaDinamicaComplete(IDataContext dctx, PruebaDinamica prueba, AEscalaDinamica escalaDinamica)
        {
            if (prueba == null || prueba.PruebaID == null) throw new ArgumentNullException("prueba", "PruebaID no puede ser nulo");
            ModeloCtrl modeloCtrl = new ModeloCtrl();
            AEscalaDinamica escala = null;
            DataSet ds = RetrieveEscalaDinamica(dctx, prueba, escalaDinamica);
            if (ds.Tables[0].Rows.Count > 0)
            {
                escala = LastDataRowToEscalaDinamica(ds);
                if (escala.Clasificador != null && escala.Clasificador.ClasificadorID != null)
                {
                    DataSet dsc = modeloCtrl.RetrieveClasificador(dctx, new Clasificador() { ClasificadorID = escala.Clasificador.ClasificadorID }, new ModeloDinamico());
                    if (dsc.Tables[0].Rows.Count > 0)
                    {
                        escala.Clasificador = modeloCtrl.LastDataRowToClasificador(dsc);
                    }
                }
            }
            return escala;
        }
        /// <summary>
        /// Consulta las Escalas Dinamicas Asignadas a la Prueba Dinamica
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="prueba">PruebaDinamica que provee el criterio de consulta</param>
        /// <returns>Lista de escalas dinamicas asignados al PruebaDinamica o nulo</returns>
        public List<AEscalaDinamica> RetrieveListEscalaDinamica(IDataContext dctx, PruebaDinamica prueba, AEscalaDinamica escalaDinamica)
        {
            if (prueba == null || prueba.PruebaID == null) throw new ArgumentNullException("prueba", "PruebaID no puede ser nulo");
            ModeloCtrl modeloCtrl = new ModeloCtrl();
            List<AEscalaDinamica> escalals = new List<AEscalaDinamica>();
            AEscalaDinamica escala = null;
            DataSet ds = null;

            ds = RetrieveEscalaDinamica(dctx, prueba, escalaDinamica);
            if (ds.Tables[0].Rows.Count > 0)
            {
                escalals = new List<AEscalaDinamica>();
                foreach (DataRow cont in ds.Tables[0].Rows)
                {
                    escala = DataRowToEscalaDinamica(cont);
                    DataSet dsc = modeloCtrl.RetrieveClasificador(dctx, new Clasificador() { ClasificadorID = escala.Clasificador.ClasificadorID }, new ModeloDinamico());
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        escala.Clasificador = modeloCtrl.LastDataRowToClasificador(dsc);
                    }
                    escalals.Add(escala);
                }
            }
            return escalals;
        }

        /// <summary>
        /// Inserta una EscalaDinamica asignada a una prueba, en la base de datos
        /// </summary>
        /// <param name="dctx">DataContext que proveerá acceso a la base de datos</param>
        /// <param name="prueba">PruebaDinamica a la que se le asignará la EscalaDinámica</param>
        /// <param name="escalaDinamica">EscalaDinamica que se le va a asignar a la prueba</param>
        public void InsertEscalaDinamica(IDataContext dctx, PruebaDinamica prueba, AEscalaDinamica escala)
        {
            #region Validaciones
            if (prueba == null) throw new Exception("La prueba no puede ser nulo");
            if (prueba.PruebaID == null) throw new Exception("El identificador de la prueba no puede ser nulo");
            if (escala == null) throw new Exception("La escala dinámica no puede ser nulo");
            #endregion

            EscalaDinamicaInsHlp da = new EscalaDinamicaInsHlp();
            da.Action(dctx, prueba, escala);
        }

        /// <summary>
        /// Actualiza una EscalaDinamica en la base de datos
        /// </summary>
        /// <param name="dctx">DataContext que proveerá acceso a la base de datos</param>
        /// <param name="escala">EscalaDinamica que contiene las modificaciones</param>
        /// <param name="previous">EscalaDinamica original de la base de datos</param>
        public void UpdateEscalaDinamica(IDataContext dctx, AEscalaDinamica escala, AEscalaDinamica previous)
        {
            #region Validaciones
            if (escala == null) throw new Exception("La escala dinámica no puede ser nulo");
            if (escala.PuntajeID == null) throw new Exception("El identificador de la escala dinámica no puede ser nulo");
            if (previous == null) throw new Exception("La escala dinámica previa no puede ser nulo");
            #endregion

            EscalaDinamicaUpdHlp da = new EscalaDinamicaUpdHlp();
            da.Action(dctx, escala, previous);
        }

        /// <summary>
        /// Elimina lógicamente una escalaDinámica en la base de datos
        /// </summary>
        /// <param name="dctx">DataContext que proveerá acceso a la base de datos</param>
        /// <param name="escala">EscalaDinámica que se eliminará de manera lógica en la base de datos</param>
        public void DeleteEscalaDinamica(IDataContext dctx, AEscalaDinamica escala)
        {
            #region Validaciones
            if (escala == null) throw new Exception("La escala dinámica no puede ser nulo");
            if (escala.PuntajeID == null) throw new Exception("El identificador de la escala dinámica no puede ser nulo");
            #endregion

            EscalaDinamicaDelHlp da = new EscalaDinamicaDelHlp();
            da.Action(dctx, escala);
        }

        /// <summary>
        /// Crea un objeto de PruebaDinamica a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de PruebaDinamica</param>
        /// <returns>Un objeto de PruebaDinamica creado a partir de los datos</returns>
        public AEscalaDinamica LastDataRowToEscalaDinamica(DataSet ds)
        {
            if (!ds.Tables.Contains("EscalaDinamica"))
                throw new Exception("LastDataRowToEscalaDinamica: DataSet no tiene la tabla EscalaDinamica");
            int index = ds.Tables["EscalaDinamica"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToEscalaDinamica: El DataSet no tiene filas");
            return this.DataRowToEscalaDinamica(ds.Tables["EscalaDinamica"].Rows[index - 1]);
        }

        /// <summary>
        /// Crea un objeto de PruebaDinamica a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de PruebaDinamica</param>
        /// <returns>Un objeto de PruebaDinamica creado a partir de los datos</returns>
        public AEscalaDinamica DataRowToEscalaDinamica(DataRow row)
        {
            ETipoEscalaDinamica tipoEscala;

            if (!row.IsNull("TipoEscalaDinamica"))
            {
                tipoEscala = (ETipoEscalaDinamica)Convert.ChangeType(row["TipoEscalaDinamica"], typeof(byte));
                AEscalaDinamica escalaDinamica = InstanciarPruebaDinamica(tipoEscala);

                escalaDinamica.Clasificador = new Clasificador();
                if (row.IsNull("PuntajeID"))
                    escalaDinamica.PuntajeID = null;
                else
                    escalaDinamica.PuntajeID = (int)Convert.ChangeType(row["PuntajeID"], typeof(int));
                if (row.IsNull("PuntajeMinimo"))
                    escalaDinamica.PuntajeMinimo = null;
                else
                    escalaDinamica.PuntajeMinimo = (decimal)Convert.ChangeType(row["PuntajeMinimo"], typeof(decimal));
                if (row.IsNull("PuntajeMaximo"))
                    escalaDinamica.PuntajeMaximo = null;
                else
                    escalaDinamica.PuntajeMaximo = (decimal)Convert.ChangeType(row["PuntajeMaximo"], typeof(decimal));
                if (row.IsNull("EsPorcentaje"))
                    escalaDinamica.EsPorcentaje = null;
                else
                    escalaDinamica.EsPorcentaje = (bool)Convert.ChangeType(row["EsPorcentaje"], typeof(bool));
                if (row.IsNull("EsPredominante"))
                    escalaDinamica.EsPredominante = null;
                else
                    escalaDinamica.EsPredominante = (bool)Convert.ChangeType(row["EsPredominante"], typeof(bool));
                if (row.IsNull("ClasificadorID"))
                    escalaDinamica.Clasificador.ClasificadorID = null;
                else
                    escalaDinamica.Clasificador.ClasificadorID = (int)Convert.ChangeType(row["ClasificadorID"], typeof(int));
                if (row.IsNull("Nombre"))
                    escalaDinamica.Nombre = null;
                else
                    escalaDinamica.Nombre = (string)Convert.ChangeType(row["Nombre"], typeof(string));
                if (row.IsNull("Descripcion"))
                    escalaDinamica.Descripcion = null;
                else
                    escalaDinamica.Descripcion = (string)Convert.ChangeType(row["Descripcion"], typeof(string));
                if (row.IsNull("Activo"))
                    escalaDinamica.Activo = null;
                else
                    escalaDinamica.Activo = (bool)Convert.ChangeType(row["Activo"], typeof(bool));
                return escalaDinamica;
            }
            return null;
        }

        /// <summary>
        /// Crea un objeto de PruebaDinamica a partir de ETipoEscalaDinamica.
        /// </summary>
        /// <param name="tipoEscala">El ETipoEscalaDinamica que contiene la informacion del tipo de escala</param>
        /// <returns>Un objeto de PruebaDinamica creado a partir un ETipoEscalaDinamica</returns>
        private AEscalaDinamica InstanciarPruebaDinamica(ETipoEscalaDinamica tipoEscala)
        {
            AEscalaDinamica escalaDinamica;
            if (tipoEscala == ETipoEscalaDinamica.CLASIFICACION)
                return escalaDinamica = new EscalaClasificacionDinamica();
            if (tipoEscala == ETipoEscalaDinamica.PORCENTA)
                return escalaDinamica = new EscalaPorcentajeDinamica();
            if (tipoEscala == ETipoEscalaDinamica.PUNTAJE)
                return escalaDinamica = new EscalaPuntajeDinamica();
            if (tipoEscala == ETipoEscalaDinamica.SELECCION)
                return escalaDinamica = new EscalaSeleccionDinamica();
            return null;
        }

        public ETipoEscalaDinamica GetTipoEscalaDinamica(EMetodoCalificacion metodoCalificacion)
        {
            ETipoEscalaDinamica? tipoReturn = null;

            switch (metodoCalificacion)
            {
                case EMetodoCalificacion.CLASIFICACION:
                    tipoReturn = ETipoEscalaDinamica.CLASIFICACION;
                    break;
                case EMetodoCalificacion.PORCENTAJE:
                    tipoReturn = ETipoEscalaDinamica.PORCENTA;
                    break;
                case EMetodoCalificacion.PUNTOS:
                    tipoReturn = ETipoEscalaDinamica.PUNTAJE;
                    break;
                case EMetodoCalificacion.SELECCION:
                    tipoReturn = ETipoEscalaDinamica.SELECCION;
                    break;
            }

            return tipoReturn.Value;
        }

        /// <summary>
        /// Actualiza la lista de Escalas de una prueba dinámica acorde al estado de cada una.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="prueba">PruebaDinamica que ya presenta los cambios realizados al objeto</param>
        /// <param name="previous">PruebaDinamica anterior, con los datos originales</param>
        public void UpdateListEscalas(IDataContext dctx, PruebaDinamica prueba, PruebaDinamica previous)
        {
            #region *** validaciones ***
            if (previous == null) throw new Exception("La prueba anterior no puede ser nulo.");
            if (previous.PruebaID == null) throw new Exception("El identificador de la prueba anterior no puede ser nulo.");
            if (prueba == null) throw new Exception("La prueba no puede ser nulo.");
            if (prueba.ListaPuntajes == null) throw new Exception("Las escalas dinámicas de la prueba no puede ser nulo");
            if (prueba.PuntajeElementos() == 0) throw new Exception("La prueba debe tener al menos una escala dinámica configurada");

            #region *Validación de la política de puntajes*
            APoliticaEscalaDinamica politica = null;
            PruebaDinamica pruebaTemporal = new PruebaDinamica();
            List<AEscalaDinamica> listaEscalasTemporal = new List<AEscalaDinamica>();
            List<EscalaClasificacionDinamica> listaClasificaTemporal = new List<EscalaClasificacionDinamica>();

            if ((prueba.Modelo as ModeloDinamico).MetodoCalificacion == EMetodoCalificacion.CLASIFICACION)
                politica = new PoliticaEscalaClasificacion();
            else if ((prueba.Modelo as ModeloDinamico).MetodoCalificacion == EMetodoCalificacion.PORCENTAJE)
                politica = new PoliticaEscalaPorcentaje();
            else if ((prueba.Modelo as ModeloDinamico).MetodoCalificacion == EMetodoCalificacion.PUNTOS)
                politica = new PoliticaEscalaPuntaje();
            else if ((prueba.Modelo as ModeloDinamico).MetodoCalificacion == EMetodoCalificacion.SELECCION)
                politica = new PoliticaEscalaSeleccion();

            switch ((prueba.Modelo as ModeloDinamico).MetodoCalificacion)
            {
                case EMetodoCalificacion.CLASIFICACION:
                    foreach (EscalaClasificacionDinamica clasificacionDinamica in prueba.ListaPuntajes.OrderBy(x => x.PuntajeMinimo))
                    {
                        if (prueba.PuntajeEstado(clasificacionDinamica) != EObjetoEstado.ELIMINADO)
                        {
                            if (politica.Validar(pruebaTemporal, clasificacionDinamica))
                            {
                                listaClasificaTemporal.Add(clasificacionDinamica);
                                pruebaTemporal = new PruebaDinamica(listaClasificaTemporal);
                            }
                            else
                            {
                                throw new Exception(String.Format("El siguiente rango no cumple con la política de la prueba: Rango {0} - {1} \n Favor de verificar.",
                                clasificacionDinamica.PuntajeMinimo.ToString(), clasificacionDinamica.PuntajeMaximo.ToString()));
                            }
                        }
                    }
                    bool band = prueba.ListaPuntajes.Any(item => item.EsPredominante.Value);
                    if (!band)
                    {
                        throw new Exception(String.Format(
                                "Necesita al menos un rango predominante"));
                    }
                    break;
                default:
                    foreach (AEscalaDinamica escala in prueba.ListaPuntajes.OrderBy(x => x.PuntajeMinimo))
                    {
                        if (prueba.PuntajeEstado(escala) != EObjetoEstado.ELIMINADO)
                        {
                            if (politica.Validar(pruebaTemporal, escala))
                            {
                                listaEscalasTemporal.Add(escala);
                                pruebaTemporal = new PruebaDinamica(listaEscalasTemporal);
                            }
                            else
                                throw new Exception(String.Format(
								"El siguiente rango no cumple con la política de la prueba. Los rangos no deben traslaparse, ni existir conjuntos de puntajes" + 
								" no asignados entre dos rangos.\n\nRango: {0} - {1}",
                                escala.PuntajeMinimo.ToString(), escala.PuntajeMaximo.ToString()));
                        }
                    }
                    break;
            }


            #endregion
            #endregion

            object myFirm = new object();
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            try
            {
                foreach (AEscalaDinamica escala in prueba.ListaPuntajes)
                {
                    if (prueba.PuntajeEstado(escala) == EObjetoEstado.NUEVO)
                    {
                        escala.Activo = true;
                        this.InsertEscalaDinamica(dctx, prueba, escala);
                    }
                    else if (prueba.PuntajeEstado(escala) == EObjetoEstado.EDITADO)
                    {
                        AEscalaDinamica escalaAnterior = null;
                        if ((prueba.Modelo as ModeloDinamico).MetodoCalificacion == EMetodoCalificacion.CLASIFICACION)
                            escalaAnterior = new EscalaClasificacionDinamica { PuntajeID = escala.PuntajeID };
                        else if ((prueba.Modelo as ModeloDinamico).MetodoCalificacion == EMetodoCalificacion.PORCENTAJE)
                            escalaAnterior = new EscalaPorcentajeDinamica { PuntajeID = escala.PuntajeID };
                        else if ((prueba.Modelo as ModeloDinamico).MetodoCalificacion == EMetodoCalificacion.PUNTOS)
                            escalaAnterior = new EscalaPuntajeDinamica { PuntajeID = escala.PuntajeID };
                        else if ((prueba.Modelo as ModeloDinamico).MetodoCalificacion == EMetodoCalificacion.SELECCION)
                            escalaAnterior = new EscalaSeleccionDinamica { PuntajeID = escala.PuntajeID };

                        this.UpdateEscalaDinamica(dctx, escala, escalaAnterior);
                    }
                    else if (prueba.PuntajeEstado(escala) == EObjetoEstado.ELIMINADO)
                    {
                        this.DeleteEscalaDinamica(dctx, escala);
                    }
                }
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
        #endregion

    }
}
