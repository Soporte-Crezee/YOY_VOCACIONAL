using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Reactivos.BO;
using POV.Reactivos.DAO;
using POV.Modelo.BO;
using POV.Modelo.Service;

namespace POV.Reactivos.Service
{
    /// <summary>
    /// Controlador del objeto Reactivo
    /// </summary>
    public class ReactivoCtrl
    {

        /// <summary>
        /// Consulta registros de ReactivoRetHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="reactivoRetHlp">ReactivoRetHlp que provee el criterio de selecciÃ³n para realizar la consulta</param>
        /// <returns>El DataSet que contiene la informaciÃ³n de ReactivoRetHlp generada por la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, Reactivo reactivo)
        {
            if (reactivo == null) throw new Exception("ReactivoCtrl: El parámetro 'reactivo' no puede ser vacio.");
            if (reactivo.TipoReactivo == null) throw new Exception("ReactivoCtrl: El campo TipoReactivo no puede ser vacio.");
            DataSet ds = null;
            switch (reactivo.TipoReactivo.Value)
            {
                case ETipoReactivo.ModeloGenerico:
                    ReactivoDinamicoRetHlp helperDinamico = new ReactivoDinamicoRetHlp();
                    ds = helperDinamico.Action(dctx, reactivo);
                    break;
            }

            return ds;
        }

        /// <summary>
        /// Valida si existe algun reactivo con los filtros proporcionados.
        /// </summary>
        /// <param name="dctx">Objeto correspondiente al contexto de datos.</param>
        /// <param name="reactivo">Objeto 'Reactivo' utilizado como filtro.</param>
        /// <returns>'true' si se encontro algún reactivo coincidente. De lo contrario 'false'.</returns>
        public bool ExistReactivo(IDataContext dctx, Reactivo reactivo)
        {
            if (reactivo == null) throw new Exception("ReactivoCtrl: El parámetro 'reactivo' no puede ser vacio.");
            if (reactivo.TipoReactivo == null) throw new Exception("ReactivoCtrl: El campo TipoReactivo no puede ser vacio.");
            bool exist = false;
            DataSet dsReactivo = null;
            switch (reactivo.TipoReactivo.Value)
            {
                case ETipoReactivo.ModeloGenerico:
                    dsReactivo = Retrieve(dctx, new Reactivo { Clave = reactivo.Clave, Activo = true, Asignado = false, TipoReactivo = reactivo.TipoReactivo });
                    break;
            }

            if (dsReactivo.Tables["Reactivo"].Rows.Count > 0)
            {
                exist = true;
            }
            return exist;
        }
        /// <summary>
        /// Registra un objeto 'Reactivo' en el contexto de datos. El registro es completo.
        /// </summary>
        /// <param name="dctx">Objeto correspondiente al contexto de datos.</param>
        /// <param name="reactivo">Objeto 'Reactivo' a insertar.</param>
        public void InsertComplete(IDataContext dctx, Reactivo reactivo)
        {
            if (reactivo == null) throw new Exception("ReactivoCtrl: El parámetro 'reactivo' no puede ser vacio.");
            if (reactivo.TipoReactivo == null) throw new Exception("ReactivoCtrl: El campo TipoReactivo no puede ser vacio.");
            object myFirm = new object();
            #region *** validaciones ***
            string sError = string.Empty;
            if (reactivo.Caracteristicas is CaracteristicasModeloGenerico)
            {
                if ((reactivo.Caracteristicas as CaracteristicasModeloGenerico).Modelo == null)
                    (reactivo.Caracteristicas as CaracteristicasModeloGenerico).Modelo = new ModeloDinamico();
                if ((reactivo.Caracteristicas as CaracteristicasModeloGenerico).Modelo.ModeloID == null)
                    sError += ", ModeloID ";
            }
            if (reactivo.Preguntas == null)
                reactivo.Preguntas = new List<Pregunta>();
            if (string.IsNullOrEmpty(reactivo.NombreReactivo))
                sError += ", NombreReactivo";
            if (sError.Length > 0)
                throw new Exception("ReactivoCtrl: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));

            if (reactivo.Preguntas.Count < 1)
                sError += ", Preguntas";
            if (sError.Length > 0)
                throw new Exception("Agregue al menos una pregunta ");


            #endregion
            try
            {
                dctx.OpenConnection(myFirm);
                dctx.BeginTransaction(myFirm);

                Insert(dctx, reactivo);

                PreguntaCtrl preguntaCtrl = new PreguntaCtrl();
                RespuestaPlantillaCtrl respuestaPlantillaCtrl = new RespuestaPlantillaCtrl();

                int orden = 1;
                foreach (Pregunta pregunta in reactivo.Preguntas)
                {
                    pregunta.Orden = orden;
                    pregunta.FechaRegistro = DateTime.Now;
                    preguntaCtrl.InsertComplete(dctx, pregunta, reactivo);

                    orden++;
                }

                if (reactivo.Caracteristicas is CaracteristicasDiagnostico)
                {
                    List<EvaluacionRango> rangos = (reactivo.Caracteristicas as CaracteristicasDiagnostico).ListaEvaluacionRango;

                    EvaluacionRangoCtrl evaluacionRangoCtrl = new EvaluacionRangoCtrl();

                    foreach (EvaluacionRango rango in rangos)
                    {
                        evaluacionRangoCtrl.Insert(dctx, rango, reactivo);
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="reactivo"></param>
        public void UpdateComplete(IDataContext dctx, Reactivo reactivo, Reactivo previous = null)
        {
            if (reactivo == null) throw new Exception("ReactivoCtrl: El parámetro 'reactivo' no puede ser vacio.");
            if (reactivo.TipoReactivo == null) throw new Exception("ReactivoCtrl: El campo TipoReactivo no puede ser vacio.");
            object myFirm = new object();
            #region *** validaciones ***
            string sError = string.Empty;

            if (reactivo.Caracteristicas is CaracteristicasDiagnostico)
            {
                if (reactivo.Caracteristicas == null)
                    reactivo.Caracteristicas = new CaracteristicasDiagnostico();
                if ((reactivo.Caracteristicas as CaracteristicasDiagnostico).ListaEvaluacionRango == null)
                    sError += ", EvaluacionRango";
                if (sError.Length > 0)
                    throw new Exception("ReactivoCtrl: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            }
            if (reactivo.Preguntas == null)
                reactivo.Preguntas = new List<Pregunta>();
            if (string.IsNullOrEmpty(reactivo.NombreReactivo))
                sError += ", NombreReactivo";

            if (sError.Length > 0)
                throw new Exception("ReactivoCtrl: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));

            if (reactivo.Preguntas.Count < 1)
                sError += ", Preguntas";
            if (sError.Length > 0)
                throw new Exception("Agregue al menos una pregunta ");


            #endregion
            try
            {
                dctx.OpenConnection(myFirm);
                dctx.BeginTransaction(myFirm);
                if (previous == null)
                    Update(dctx, reactivo, reactivo);
                else
                    Update(dctx, reactivo, previous);
                PreguntaCtrl preguntaCtrl = new PreguntaCtrl();
                RespuestaPlantillaCtrl respuestaPlantillaCtrl = new RespuestaPlantillaCtrl();

                int orden = 1;
                foreach (Pregunta pregunta in reactivo.Preguntas)
                {
                    pregunta.Orden = orden;

                    if (pregunta.PreguntaID == null)
                    {
                        pregunta.FechaRegistro = DateTime.Now;
                        pregunta.Activo = true;
                        preguntaCtrl.InsertComplete(dctx, pregunta, reactivo);
                    }
                    else
                    {
                        if (previous == null)
                            preguntaCtrl.UpdateComplete(dctx, pregunta, pregunta, reactivo);
                        else
                        {
                            Pregunta preguntaPrevia = previous.Preguntas.FirstOrDefault(item => item.PreguntaID == pregunta.PreguntaID);
                            preguntaCtrl.UpdateComplete(dctx, pregunta, preguntaPrevia, reactivo);
                        }
                    }

                    orden++;
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
        /// <summary>
        /// Regresa un registro completo de Reactivo
        /// </summary>
        public Reactivo RetrieveComplete(IDataContext dctx, Reactivo reactivo)
        {
            if (reactivo == null) throw new Exception("ReactivoCtrl: El parámetro 'reactivo' no puede ser vacio.");
            if (reactivo.TipoReactivo == null) throw new Exception("ReactivoCtrl: El campo TipoReactivo no puede ser vacio.");

            switch (reactivo.TipoReactivo.Value)
            {
                case ETipoReactivo.Diagnostico:
                case ETipoReactivo.InicialDiagnostico:
                    reactivo = this.RetrieveDiagnosticoComplete(dctx, reactivo);
                    break;
                case ETipoReactivo.ModeloGenerico:
                    reactivo = this.RetrieveDinamicoComplete(dctx, reactivo);
                    break;
            }

            return reactivo;
        }
        public Reactivo RetrieveReactivoComplete(IDataContext dctx, Reactivo reactivo)
        {
            if (reactivo == null) throw new Exception("ReactivoCtrl: El parámetro 'reactivo' no puede ser vacio.");
            if (reactivo.TipoReactivo == null) throw new Exception("ReactivoCtrl: El campo TipoReactivo no puede ser vacio.");

            switch (reactivo.TipoReactivo.Value)
            {
                case ETipoReactivo.InicialDiagnostico:
                    reactivo = this.RetrieveReactivoDiagnosticoComplete(dctx, reactivo);
                    break;
                case ETipoReactivo.ModeloGenerico:
                    reactivo = this.RetrieveReactivoDinamicoComplete(dctx, reactivo);
                    break;
            }

            return reactivo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="reactivo"></param>
        /// <returns></returns>
        private Reactivo RetrieveDiagnosticoComplete(IDataContext dctx, Reactivo reactivo)
        {
            if (reactivo == null) throw new Exception("ReactivoCtrl: El parámetro 'reactivo' no puede ser vacio.");
            if (reactivo.TipoReactivo == null) throw new Exception("ReactivoCtrl: El campo TipoReactivo no puede ser vacio.");

            DataSet ds = Retrieve(dctx, new Reactivo { ReactivoID = reactivo.ReactivoID, TipoReactivo = reactivo.TipoReactivo, Caracteristicas = reactivo.Caracteristicas });
            if (ds.Tables["Reactivo"].Rows.Count > 0)
            {
                reactivo = LastDataRowToReactivo(ds, reactivo.TipoReactivo.Value);
                if (reactivo.TipoReactivo == ETipoReactivo.Diagnostico)
                {
                    EvaluacionRangoCtrl evaluacionCtrl = new EvaluacionRangoCtrl();

                    (reactivo.Caracteristicas as CaracteristicasDiagnostico).ListaEvaluacionRango = new List<EvaluacionRango>();
                    foreach (DataRow current in evaluacionCtrl.Retrieve(dctx, new EvaluacionRango(), reactivo).Tables[0].Rows)
                        (reactivo.Caracteristicas as CaracteristicasDiagnostico).ListaEvaluacionRango.Add(evaluacionCtrl.DataRowToEvaluacionRango(current));
                }

                PreguntaCtrl preguntaCtrl = new PreguntaCtrl();

                reactivo.Preguntas = preguntaCtrl.RetrieveListPreguntasReactivo(dctx, reactivo);
            }
            else
            {
                return null;
            }

            return reactivo;
        }
        private Reactivo RetrieveReactivoDiagnosticoComplete(IDataContext dctx, Reactivo reactivo)
        {
            if (reactivo == null) throw new Exception("ReactivoCtrl: El parámetro 'reactivo' no puede ser vacio.");
            if (reactivo.TipoReactivo == null) throw new Exception("ReactivoCtrl: El campo TipoReactivo no puede ser vacio.");

            DataSet ds = Retrieve(dctx, reactivo);
            if (ds.Tables["Reactivo"].Rows.Count > 0)
            {
                reactivo = LastDataRowToReactivo(ds, reactivo.TipoReactivo.Value);
                if (reactivo.TipoReactivo == ETipoReactivo.Diagnostico)
                {
                    EvaluacionRangoCtrl evaluacionCtrl = new EvaluacionRangoCtrl();

                    (reactivo.Caracteristicas as CaracteristicasDiagnostico).ListaEvaluacionRango = new List<EvaluacionRango>();
                    foreach (DataRow current in evaluacionCtrl.Retrieve(dctx, new EvaluacionRango(), reactivo).Tables[0].Rows)
                        (reactivo.Caracteristicas as CaracteristicasDiagnostico).ListaEvaluacionRango.Add(evaluacionCtrl.DataRowToEvaluacionRango(current));
                }

                PreguntaCtrl preguntaCtrl = new PreguntaCtrl();

                reactivo.Preguntas = preguntaCtrl.RetrieveListPreguntasReactivo(dctx, reactivo);
            }
            else
            {
                return null;
            }

            return reactivo;
        }
        public Reactivo RetrieveReactivoInicial(IDataContext dctx, int edad)
        {
            List<Reactivo> reactivos = new List<Reactivo>();
            Reactivo reactivo = new Reactivo();

            reactivo.Activo = true;
            reactivo.Caracteristicas = new CaracteristicasDiagnosticoInicial();
            reactivo.TipoReactivo = ETipoReactivo.InicialDiagnostico;

            DataSet ds = this.Retrieve(dctx, reactivo);
            foreach (DataRow row in ds.Tables[0].Rows)
                reactivos.Add(this.DataRowToReactivo(row, ETipoReactivo.InicialDiagnostico));

            bool edadIgual = reactivos.Any(r => ((CaracteristicasDiagnosticoInicial)r.Caracteristicas).Edad.Value == edad);
            byte minimo = reactivos.Min(r => ((CaracteristicasDiagnosticoInicial)r.Caracteristicas).Edad.Value);
            bool edadMenor = edad < minimo;
            byte maximo = reactivos.Max(r => ((CaracteristicasDiagnosticoInicial)r.Caracteristicas).Edad.Value);
            bool edadMayor = maximo < edad;

            if (edadIgual)
                reactivo = reactivos.FirstOrDefault(r => ((CaracteristicasDiagnosticoInicial)r.Caracteristicas).Edad.Value == edad);
            if (edadMenor)
                reactivo = reactivos.FirstOrDefault(r => ((CaracteristicasDiagnosticoInicial)r.Caracteristicas).Edad.Value == minimo);
            if (edadMayor)
                reactivo = reactivos.FirstOrDefault(r => ((CaracteristicasDiagnosticoInicial)r.Caracteristicas).Edad.Value == maximo);

            reactivo.Caracteristicas = new CaracteristicasDiagnosticoInicial();

            return (reactivo == null ? null : this.RetrieveComplete(dctx, reactivo));
        }

        /// <summary>
        /// Crea un registro de ReactivoInsHlp en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="reactivoInsHlp">ReactivoInsHlp que desea crear</param>
        public void Insert(IDataContext dctx, Reactivo reactivo)
        {
            if (reactivo == null) throw new Exception("ReactivoCtrl: El parámetro 'reactivo' no puede ser vacio.");
            if (reactivo.TipoReactivo == null) throw new Exception("ReactivoCtrl: El campo TipoReactivo no puede ser vacio.");

            switch (reactivo.TipoReactivo.Value)
            {
                case ETipoReactivo.ModeloGenerico:
                    if (reactivo.Asignado == false)
                        if (ExistReactivo(dctx, new Reactivo() { Clave = reactivo.Clave, TipoReactivo = reactivo.TipoReactivo, Asignado = false, Caracteristicas = new CaracteristicasModeloGenerico() }))
                            throw new Exception("No se permite guardar  un reactivo con una clave existente en el sistema.");
                    ReactivoDinamicoInsHlp daDinamico = new ReactivoDinamicoInsHlp();
                    daDinamico.Action(dctx, reactivo);
                    break;
            }
        }
        /// <summary>
        /// Actualiza de manera optimista un registro de ReactivoUpdHlp en la base de datos.
        /// </summary>
        ///   <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="reactivoUpdHlp">ReactivoUpdHlp que tiene los datos nuevos</param>
        /// <param name="anterior">ReactivoUpdHlp que tiene los datos anteriores</param>
        public void Update(IDataContext dctx, Reactivo reactivo, Reactivo previous)
        {
            if (reactivo == null) throw new Exception("ReactivoCtrl: El parámetro 'reactivo' no puede ser vacio.");
            if (reactivo.TipoReactivo == null) throw new Exception("ReactivoCtrl: El campo TipoReactivo no puede ser vacio.");


            if (reactivo.Clave != previous.Clave)
            {
                if (reactivo.TipoReactivo == ETipoReactivo.ModeloGenerico || reactivo.TipoReactivo == ETipoReactivo.Final)
                    if (ExistReactivo(dctx, new Reactivo() { Clave = reactivo.Clave, TipoReactivo = reactivo.TipoReactivo, Asignado = false }))
                    {
                        throw new Exception("No se permite guardar  un reactivo con una clave existente en el sistema.");
                    }

            }
            switch (reactivo.TipoReactivo.Value)
            {
                case ETipoReactivo.ModeloGenerico:
                    ReactivoDinamicoUpdHlp reDinaUpHlp = new ReactivoDinamicoUpdHlp();
                    reDinaUpHlp.Action(dctx, reactivo, previous);
                    break;
            }
        }
        /// <summary>
        /// Elimina un registro de ReactivoDelHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="reactivoDelHlp">ReactivoDelHlp que desea eliminar</param>
        public void Delete(IDataContext dctx, Reactivo reactivo)
        {
            if (reactivo == null) throw new Exception("ReactivoCtrl: El parámetro 'reactivo' no puede ser vacio.");
            if (reactivo.TipoReactivo == null) throw new Exception("ReactivoCtrl: El campo TipoReactivo no puede ser vacio.");

            if (reactivo.TipoReactivo == ETipoReactivo.Final)
                if (reactivo.Asignado == true) throw new Exception("ReactivoCtrl: No se permite eliminar un reactivo asignado a una prueba.");

            switch (reactivo.TipoReactivo.Value)
            {
                case ETipoReactivo.ModeloGenerico:
                    ReactivoDinamicoDelHlp daDinamico = new ReactivoDinamicoDelHlp();
                    daDinamico.Action(dctx, reactivo);
                    break;
            }
        }
        /// <summary>
        /// Elimina un registro completo de reactivo
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="reactivo"></param>
        public void DeleteComplete(IDataContext dctx, Reactivo reactivo)
        {
            if (reactivo == null) throw new Exception("ReactivoCtrl: El parámetro 'reactivo' no puede ser vacio.");
            if (reactivo.TipoReactivo == null) throw new Exception("ReactivoCtrl: El campo TipoReactivo no puede ser vacio.");

            object myFirm = new object();
            try
            {
                dctx.OpenConnection(myFirm);
                dctx.BeginTransaction(myFirm);
                PreguntaCtrl preguntaCtrl = new PreguntaCtrl();
                RespuestaPlantillaCtrl respuestaPlantillaCtrl = new RespuestaPlantillaCtrl();
                reactivo = RetrieveComplete(dctx, reactivo);

                //eliminacion de preguntas
                foreach (Pregunta pregunta in reactivo.Preguntas)
                {
                    RespuestaPlantilla respuestaPlantilla = pregunta.RespuestaPlantilla;
                    respuestaPlantillaCtrl.Delete(dctx, respuestaPlantilla, reactivo.TipoReactivo.Value);

                    preguntaCtrl.Delete(dctx, pregunta, reactivo);
                }

                Delete(dctx, reactivo);
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
        /// Crea un objeto de Reactivo a partir de los datos del Ãºltimo DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la informaciÃ³n de Reactivo</param>
        /// <returns>Un objeto de Reactivo creado a partir de los datos</returns>
        public Reactivo LastDataRowToReactivo(DataSet ds, ETipoReactivo tipoReactivo)
        {
            if (!ds.Tables.Contains("Reactivo"))
                throw new Exception("LastDataRowToReactivo: DataSet no tiene la tabla Reactivo");
            int index = ds.Tables["Reactivo"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToReactivo: El DataSet no tiene filas");
            return this.DataRowToReactivo(ds.Tables["Reactivo"].Rows[index - 1], tipoReactivo);
        }
        /// <summary>
        /// Crea un objeto de Reactivo a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la informaciÃ³n de Reactivo</param>
        /// <returns>Un objeto de Reactivo creado a partir de los datos</returns>
        public Reactivo DataRowToReactivo(DataRow row, ETipoReactivo tipoReactivo)
        {
            Reactivo reactivo = new Reactivo();
            reactivo.TipoReactivo = tipoReactivo;
            switch (tipoReactivo)
            {
                case ETipoReactivo.Diagnostico:
                    reactivo.Caracteristicas = new CaracteristicasDiagnostico();
                    if (row.IsNull("Version"))
                        (reactivo.Caracteristicas as CaracteristicasDiagnostico).Version = null;
                    else
                        (reactivo.Caracteristicas as CaracteristicasDiagnostico).Version = Convert.ToInt32(row["Version"]);
                    if (row.IsNull("ReactivoBaseID"))
                        (reactivo.Caracteristicas as CaracteristicasDiagnostico).ReactivoBaseID = null;
                    else
                        (reactivo.Caracteristicas as CaracteristicasDiagnostico).ReactivoBaseID = Guid.Parse(row["ReactivoBaseID"].ToString());
                    break;
                case ETipoReactivo.ModeloGenerico:
                    reactivo.Caracteristicas = new CaracteristicasModeloGenerico();
                    (reactivo.Caracteristicas as CaracteristicasModeloGenerico).Modelo = new ModeloDinamico();
                    (reactivo.Caracteristicas as CaracteristicasModeloGenerico).Clasificador = new Clasificador();

                    if (row.IsNull("ClasificadorID"))
                        (reactivo.Caracteristicas as CaracteristicasModeloGenerico).Clasificador.ClasificadorID = null;
                    else
                        (reactivo.Caracteristicas as CaracteristicasModeloGenerico).Clasificador.ClasificadorID = Convert.ToInt32(row["ClasificadorID"]);
                    if (row.IsNull("ModeloID"))
                        (reactivo.Caracteristicas as CaracteristicasModeloGenerico).Modelo.ModeloID = null;
                    else
                        (reactivo.Caracteristicas as CaracteristicasModeloGenerico).Modelo.ModeloID = Convert.ToInt32(row["ModeloID"]);
                    break;
            }

            reactivo.Preguntas = new List<Pregunta>();
            if (row.IsNull("ReactivoID"))
                reactivo.ReactivoID = null;
            else
                reactivo.ReactivoID = (Guid)Convert.ChangeType(row["ReactivoID"], typeof(Guid));
            if (row.IsNull("NombreReactivo"))
                reactivo.NombreReactivo = null;
            else
                reactivo.NombreReactivo = (string)Convert.ChangeType(row["NombreReactivo"], typeof(string));
            if (row.Table.Columns.Contains("Valor"))
            {
                if (row.IsNull("Valor"))
                    reactivo.Valor = null;
                else
                {
                    reactivo.Valor = (decimal)Convert.ChangeType(row["Valor"], typeof(decimal));
                }
            }
            if (row.IsNull("FechaRegistro"))
                reactivo.FechaRegistro = null;
            else
                reactivo.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));

            if (row.Table.Columns.Contains("NumeroIntentos"))
            {
                if (row.IsNull("NumeroIntentos"))
                    reactivo.NumeroIntentos = null;
                else
                    reactivo.NumeroIntentos = (Int32)Convert.ChangeType(row["NumeroIntentos"], typeof(Int32));
            }
            if (row.IsNull("PlantillaReactivo"))
                reactivo.PlantillaReactivo = null;
            else
                reactivo.PlantillaReactivo = (string)Convert.ChangeType(row["PlantillaReactivo"], typeof(string));

            if (row.IsNull("Descripcion"))
                reactivo.Descripcion = null;
            else
                reactivo.Descripcion = row["Descripcion"].ToString();


            if (row.Table.Columns.Contains("Asignado"))
            {
                if (row.IsNull("Asignado"))
                    reactivo.Asignado = null;
                else
                    reactivo.Asignado = Convert.ToBoolean(row["Asignado"]);
            }
            if (row.Table.Columns.Contains("Clave"))
            {
                if (row.IsNull("Clave"))
                    reactivo.Clave = null;
                else
                    reactivo.Clave = (string)Convert.ChangeType(row["Clave"], typeof(string));
            }


            if (row.Table.Columns.Contains("PresentacionPlantilla"))
            {
                if (row.IsNull("PresentacionPlantilla"))
                    reactivo.Asignado = null;
                else
                    reactivo.PresentacionPlantilla = (EPresentacionPlantilla)Convert.ChangeType(row["PresentacionPlantilla"], typeof(byte));
            }

            if (row.IsNull("FechaRegistro"))
                reactivo.FechaRegistro = null;
            else
                reactivo.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));

            if (row.IsNull("Activo"))
                reactivo.Activo = null;
            else
                reactivo.Activo = Convert.ToBoolean(row["Activo"]);

            if (row.IsNull("Grupo"))
                reactivo.Grupo = null;
            else
                reactivo.Grupo = Convert.ToInt32(row["Grupo"]);

            return reactivo;
        }
        /// <summary>
        /// Consulta un registro completo de un reactivo dinamico
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="reactivo">Reactivo</param>
        /// <returns>Reactivo completo, null en caso de encontrarlo</returns>
        private Reactivo RetrieveDinamicoComplete(IDataContext dctx, Reactivo reactivo)
        {
            if (reactivo == null) throw new Exception("ReactivoCtrl: El parámetro 'reactivo' no puede ser vacio.");
            if (reactivo.TipoReactivo == null) throw new Exception("ReactivoCtrl: El campo TipoReactivo no puede ser vacio.");
            if (reactivo.ReactivoID == null) throw new Exception("ReactivoCtrl: El campo ReactivoID no puede ser nulo.");
            Reactivo reactivoComplete = null;


            PreguntaCtrl preguntaCtrl = new PreguntaCtrl();
            DataSet ds = Retrieve(dctx, new Reactivo { ReactivoID = reactivo.ReactivoID, TipoReactivo = reactivo.TipoReactivo });
            if (ds.Tables[0].Rows.Count > 0)
            {
                ModeloCtrl modeloCtrl = new ModeloCtrl();

                reactivoComplete = LastDataRowToReactivo(ds, reactivo.TipoReactivo.Value);

                if ((reactivoComplete.Caracteristicas as CaracteristicasModeloGenerico).Modelo != null &&
                    (reactivoComplete.Caracteristicas as CaracteristicasModeloGenerico).Modelo.ModeloID != null)
                    (reactivoComplete.Caracteristicas as CaracteristicasModeloGenerico).Modelo = modeloCtrl.LastDataRowToModelo(modeloCtrl.Retrieve(dctx, (reactivoComplete.Caracteristicas as CaracteristicasModeloGenerico).Modelo)) as ModeloDinamico;


                if ((reactivoComplete.Caracteristicas as CaracteristicasModeloGenerico).Clasificador != null
                    && (reactivoComplete.Caracteristicas as CaracteristicasModeloGenerico).Clasificador.ClasificadorID != null)
                    (reactivoComplete.Caracteristicas as CaracteristicasModeloGenerico).Clasificador = modeloCtrl.LastDataRowToClasificador(modeloCtrl.RetrieveClasificador(dctx, (reactivoComplete.Caracteristicas as CaracteristicasModeloGenerico).Clasificador, new ModeloDinamico())); //modeloCtrl.RetrieveClasificador

                reactivoComplete.Preguntas = preguntaCtrl.RetrieveListPreguntasReactivo(dctx, reactivoComplete);
            }
            return reactivoComplete;
        }
        private Reactivo RetrieveReactivoDinamicoComplete(IDataContext dctx, Reactivo reactivo)
        {
            if (reactivo == null) throw new Exception("ReactivoCtrl: El parámetro 'reactivo' no puede ser vacio.");
            if (reactivo.TipoReactivo == null) throw new Exception("ReactivoCtrl: El campo TipoReactivo no puede ser vacio.");
            Reactivo reactivoComplete = null;


            PreguntaCtrl preguntaCtrl = new PreguntaCtrl();
            DataSet ds = Retrieve(dctx, reactivo);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ModeloCtrl modeloCtrl = new ModeloCtrl();

                reactivoComplete = LastDataRowToReactivo(ds, reactivo.TipoReactivo.Value);

                if ((reactivoComplete.Caracteristicas as CaracteristicasModeloGenerico).Modelo != null &&
                    (reactivoComplete.Caracteristicas as CaracteristicasModeloGenerico).Modelo.ModeloID != null)
                    (reactivoComplete.Caracteristicas as CaracteristicasModeloGenerico).Modelo = modeloCtrl.LastDataRowToModelo(modeloCtrl.Retrieve(dctx, (reactivoComplete.Caracteristicas as CaracteristicasModeloGenerico).Modelo)) as ModeloDinamico;


                if ((reactivoComplete.Caracteristicas as CaracteristicasModeloGenerico).Clasificador != null
                    && (reactivoComplete.Caracteristicas as CaracteristicasModeloGenerico).Clasificador.ClasificadorID != null)
                    (reactivoComplete.Caracteristicas as CaracteristicasModeloGenerico).Clasificador = modeloCtrl.LastDataRowToClasificador(modeloCtrl.RetrieveClasificador(dctx, (reactivoComplete.Caracteristicas as CaracteristicasModeloGenerico).Clasificador, new ModeloDinamico())); //modeloCtrl.RetrieveClasificador

                reactivoComplete.Preguntas = preguntaCtrl.RetrieveListPreguntasReactivo(dctx, reactivoComplete);
            }
            return reactivoComplete;
        }
    }
}
