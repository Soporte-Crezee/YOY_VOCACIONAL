using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Prueba.Diagnostico.Dinamica.DAO;
using POV.Prueba.Diagnostico.Dinamica.DA;
using POV.Reactivos.BO;
using POV.Reactivos.Service;
using POV.Prueba.BO;

namespace POV.Prueba.Diagnostico.Dinamica.Service
{
    /// <summary>
    /// Controlador del objeto RespuestaPreguntaDinamica
    /// </summary>
    public class RespuestaPreguntaDinamicaCtrl
    {
        /// <summary>
        /// Consulta registros de RespuestaPreguntaDinamica en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="respuestaPreguntaDinamicaRetHlp">RespuestaPreguntaDinamica que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de RespuestaPreguntaDinamicaRetHlp generada por la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, RespuestaReactivoDinamica respuestaReactivo, RespuestaPreguntaDinamica respuestaPregunta)
        {
            RespuestaPreguntaDinamicaRetHlp da = new RespuestaPreguntaDinamicaRetHlp();
            DataSet ds = da.Action(dctx, respuestaReactivo, respuestaPregunta);
            return ds;
        }
        /// <summary>
        /// Consulta un respuesta de preguna completo en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="respuestaReactivo">RespuestaReactivoDinamica</param>
        /// <param name="respuestaPregunta">RespuestaPreguntaDinamica</param>
        /// <returns>RespuestaPreguntaDinamica completo, nulo en caso contrario</returns>
        public RespuestaPreguntaDinamica RetrieveComplete(IDataContext dctx, RespuestaReactivoDinamica respuestaReactivo, RespuestaPreguntaDinamica respuestaPregunta)
        {
            if (respuestaReactivo == null) throw new ArgumentNullException("RespuestaPreguntaDinamicaCtrl: la respuesta reactivo no debe ser nulo");
            if (respuestaPregunta == null) throw new ArgumentNullException("RespuestaPreguntaDinamicaCtrl: la respuesta reactivo no debe ser nulo");
            RespuestaPreguntaDinamica respuestaComplete = new RespuestaPreguntaDinamica();

            DataSet dsRespuestaPregunta = Retrieve(dctx, respuestaReactivo, respuestaPregunta);
            if (dsRespuestaPregunta.Tables[0].Rows.Count > 0)
            {
                PreguntaCtrl preguntaCtrl = new PreguntaCtrl();
                respuestaComplete = LastDataRowToRespuestaPreguntaDinamica(dsRespuestaPregunta);
                respuestaComplete.Pregunta = preguntaCtrl.RetrieveComplete(dctx, respuestaComplete.Pregunta, new Reactivo { TipoReactivo = ETipoReactivo.ModeloGenerico });

                if (respuestaComplete.RespuestaAlumno != null)
                {
                    if (respuestaComplete.RespuestaAlumno is RespuestaDinamicaOpcionMultiple)
                    {
                        (respuestaComplete.RespuestaAlumno as RespuestaDinamicaOpcionMultiple).ListaOpcionesRespuesta = new List<OpcionRespuestaPlantilla>(RetrieveListaOpcionesSeleccionadas(dctx, respuestaComplete));
                    }
                    else if (respuestaComplete.RespuestaAlumno is RespuestaDinamicaAbierta)
                    {
                        RespuestaPlantillaCtrl respuestaPlantillaCtrl = new RespuestaPlantillaCtrl();

                        (respuestaComplete.RespuestaAlumno as RespuestaDinamicaAbierta).RespuestaPlantilla = respuestaPlantillaCtrl.LastDataRowToRespuestaPlantillaAbierta(respuestaPlantillaCtrl.RetrieveRespuestaPlantillaAbierta(dctx, (respuestaComplete.RespuestaAlumno as RespuestaDinamicaAbierta).RespuestaPlantilla, respuestaComplete.Pregunta, ETipoReactivo.ModeloGenerico), ETipoReactivo.ModeloGenerico);
                    }
                }
            }



            return respuestaComplete;
        }

        /// <summary>
        /// Consulta la lista de respuesta pregunta en la base de datos relacionado a un respuesta reactivo
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="respuestaReactivo">RespuestaReactivoDinamica que se usara como filtro</param>
        /// <returns>Lista de respuesta pregunta completo, lista vacia en caso de no encontrar resultado</returns>
        public List<ARespuestaPregunta> RetrieveListaRespuestaPregunta(IDataContext dctx, RespuestaReactivoDinamica respuestaReactivo)
        {
            if (respuestaReactivo == null) throw new ArgumentNullException("RespuestaPreguntaDinamicaCtrl: la respuesta reactivo no debe ser nulo");
            if (respuestaReactivo.RespuestaReactivoID == null) throw new ArgumentNullException("RespuestaPreguntaDinamicaCtrl: RespuestaReactivoID no debe ser nulo");
            List<ARespuestaPregunta> listaRespuestaPregunta = new List<ARespuestaPregunta>();


            DataSet dsRespuestasPregunta = Retrieve(dctx, respuestaReactivo, new RespuestaPreguntaDinamica());

            if (dsRespuestasPregunta.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsRespuestasPregunta.Tables[0].Rows)
                {
                    int respuestaPreguntaID = Convert.ToInt32(dr["RespuestaPreguntaID"].ToString());
                    listaRespuestaPregunta.Add(RetrieveComplete(dctx, respuestaReactivo, new RespuestaPreguntaDinamica { RespuestaPreguntaID = respuestaPreguntaID}));
                }
            }

            return listaRespuestaPregunta.OrderBy(item => item.RespuestaPreguntaID).ToList();
        }
        /// <summary>
        /// Recupera una lista de resultados de opciones
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="respuestaPregunta">RespuestaPreguntaDinamica</param>
        /// <returns>Lista de opciones</returns>
        private List<OpcionRespuestaModeloGenerico> RetrieveListaOpcionesSeleccionadas(IDataContext dctx, RespuestaPreguntaDinamica respuestaPregunta)
        {
            List<OpcionRespuestaModeloGenerico> listaOpciones = new List<OpcionRespuestaModeloGenerico>();

            RespuestaOpcionDinamicaDetalleDARetHlp da = new RespuestaOpcionDinamicaDetalleDARetHlp();
            DataSet dsOpciones = da.Action(dctx, respuestaPregunta);
            if (dsOpciones.Tables[0].Rows.Count > 0)
            {
                OpcionRespuestaPlantillaCtrl opcionCtrl = new OpcionRespuestaPlantillaCtrl();
                //consultamos las opciones seleccionadas
                foreach (DataRow dr in dsOpciones.Tables[0].Rows)
                {
                    int opcionID = Convert.ToInt32(dr["OpcionRespuestaPlantillaID"].ToString());

                    DataSet dsOpcion = opcionCtrl.Retrieve(dctx, new OpcionRespuestaModeloGenerico { OpcionRespuestaPlantillaID = opcionID }, new RespuestaPlantillaOpcionMultiple(), ETipoReactivo.ModeloGenerico);
                    if (dsOpcion.Tables[0].Rows.Count > 0)
                    {
                        listaOpciones.Add(opcionCtrl.LastDataRowToOpcionRespuestaPlantilla(dsOpcion, ETipoReactivo.ModeloGenerico) as OpcionRespuestaModeloGenerico);
                    }
                        
                }

            }

            return listaOpciones;
        }
        /// <summary>
        /// Inserta un registro de RespuestaPreguntaDinamicaCompleto
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="respuestaReactivo">RespuestaReactivoDinamica</param>
        /// <param name="respuestaPregunta">RespuestaPreguntaDinamica completo</param>
        public void InsertComplete(IDataContext dctx, RespuestaReactivoDinamica respuestaReactivo, RespuestaPreguntaDinamica respuestaPregunta)
        {
            if (respuestaReactivo == null) throw new ArgumentNullException("RespuestaPreguntaDinamicaCtrl: respuestaReactivo no puede ser nulo ");
            if (respuestaReactivo.RespuestaReactivoID == null) throw new ArgumentNullException("RespuestaPreguntaDinamicaCtrl: respuestaReactivoID no puede ser nulo");
            if (respuestaPregunta == null) throw new ArgumentNullException("RespuestaPreguntaDinamicaCtrl: respuestaPregunta no puede ser nulo");
            if (respuestaPregunta.RespuestaAlumno == null) throw new ArgumentNullException("RespuestaPreguntaDinamicaCtrl: respuestaAlumno no puede ser nulo");

            RespuestaPreguntaDinamicaInsHlp da = new RespuestaPreguntaDinamicaInsHlp();
            if (respuestaPregunta.RespuestaAlumno is RespuestaDinamicaAbierta)
            {
                da.Action(dctx, respuestaReactivo, respuestaPregunta, respuestaPregunta.RespuestaAlumno as RespuestaDinamicaAbierta);
            }
            else if (respuestaPregunta.RespuestaAlumno is RespuestaDinamicaOpcionMultiple)
            {
                da.Action(dctx, respuestaReactivo, respuestaPregunta, respuestaPregunta.RespuestaAlumno as RespuestaDinamicaOpcionMultiple);

            }



        }
        /// <summary>
        /// Actualiza de manera optimista un registro de RespuestaPreguntaDinamicaUpdHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="respuestaPreguntaDinamicaUpdHlp">RespuestaPreguntaDinamicaUpdHlp que tiene los datos nuevos</param>
        /// <param name="anterior">RespuestaPreguntaDinamicaUpdHlp que tiene los datos anteriores</param>
        public void UpdateComplete(IDataContext dctx, RespuestaPreguntaDinamica respuestaPregunta, RespuestaPreguntaDinamica previous)
        {

            if (respuestaPregunta == null) throw new ArgumentNullException("RespuestaPreguntaDinamicaCtrl: respuestaPregunta no puede ser nulo");
            if (respuestaPregunta.RespuestaAlumno == null) throw new ArgumentNullException("RespuestaPreguntaDinamicaCtrl: respuestaAlumno no puede ser nulo");
            object myFirm = new object();
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            try
            {
                RespuestaPreguntaDinamicaUpdHlp da = new RespuestaPreguntaDinamicaUpdHlp();

                if (respuestaPregunta.RespuestaAlumno is RespuestaDinamicaAbierta)
                {
                    da.Action(dctx, respuestaPregunta, respuestaPregunta.RespuestaAlumno as RespuestaDinamicaAbierta, previous);
                }
                else if (respuestaPregunta.RespuestaAlumno is RespuestaDinamicaOpcionMultiple)
                {
                    da.Action(dctx, respuestaPregunta, respuestaPregunta.RespuestaAlumno as RespuestaDinamicaOpcionMultiple, previous);

                    RespuestaOpcionDinamicaDetalleDAInsHlp opcionRespuestaDa = new RespuestaOpcionDinamicaDetalleDAInsHlp();

                    foreach (OpcionRespuestaPlantilla opcion in (respuestaPregunta.RespuestaAlumno as RespuestaDinamicaOpcionMultiple).ListaOpcionesRespuesta)
                    {
                        opcionRespuestaDa.Action(dctx, respuestaPregunta, opcion);
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
            //RespuestaPreguntaDinamicaUpdHlp da = new RespuestaPreguntaDinamicaUpdHlp();
            //da.Action(dctx, respuestaPregunta, previous);
        }
        /// <summary>
        /// Crea un objeto de RespuestaPreguntaDinamica a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de RespuestaPreguntaDinamica</param>
        /// <returns>Un objeto de RespuestaPreguntaDinamica creado a partir de los datos</returns>
        public RespuestaPreguntaDinamica LastDataRowToRespuestaPreguntaDinamica(DataSet ds)
        {
            if (!ds.Tables.Contains("RespuestaPregunta"))
                throw new Exception("LastDataRowToRespuestaPreguntaDinamica: DataSet no tiene la tabla RespuestaPreguntaDinamica");
            int index = ds.Tables["RespuestaPregunta"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToRespuestaPreguntaDinamica: El DataSet no tiene filas");
            return this.DataRowToRespuestaPreguntaDinamica(ds.Tables["RespuestaPregunta"].Rows[index - 1]);
        }
        /// <summary>
        /// Crea un objeto de RespuestaPreguntaDinamica a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de RespuestaPreguntaDinamica</param>
        /// <returns>Un objeto de RespuestaPreguntaDinamica creado a partir de los datos</returns>
        public RespuestaPreguntaDinamica DataRowToRespuestaPreguntaDinamica(DataRow row)
        {
            RespuestaPreguntaDinamica respuestaPreguntaDinamica = new RespuestaPreguntaDinamica();
            respuestaPreguntaDinamica.Pregunta = new Pregunta();

            if (row.IsNull("RespuestaPreguntaID"))
                respuestaPreguntaDinamica.RespuestaPreguntaID = null;
            else
                respuestaPreguntaDinamica.RespuestaPreguntaID = (int)Convert.ChangeType(row["RespuestaPreguntaID"], typeof(int));
            if (row.IsNull("PreguntaID"))
                respuestaPreguntaDinamica.Pregunta.PreguntaID = null;
            else
                respuestaPreguntaDinamica.Pregunta.PreguntaID = (int)Convert.ChangeType(row["PreguntaID"], typeof(int));
            if (row.IsNull("FechaRegistro"))
                respuestaPreguntaDinamica.FechaRegistro = null;
            else
                respuestaPreguntaDinamica.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));

            if (!row.IsNull("TipoRespuestaPlantilla"))
            {
                ETipoRespuestaPlantilla tipoRespuesta = (ETipoRespuestaPlantilla)(byte)Convert.ChangeType(row["TipoRespuestaPlantilla"], typeof(byte));

                if (tipoRespuesta == ETipoRespuestaPlantilla.OPCION_MULTIPLE)
                {
                    respuestaPreguntaDinamica.RespuestaAlumno = new RespuestaDinamicaOpcionMultiple();
                    respuestaPreguntaDinamica.RespuestaAlumno.TipoRespuestaPlantilla = tipoRespuesta;
                    (respuestaPreguntaDinamica.RespuestaAlumno as RespuestaDinamicaOpcionMultiple).ListaOpcionesRespuesta = new List<OpcionRespuestaPlantilla>();

                }
                else if (tipoRespuesta == ETipoRespuestaPlantilla.ABIERTA)
                {
                    respuestaPreguntaDinamica.RespuestaAlumno = new RespuestaDinamicaAbierta();
                    respuestaPreguntaDinamica.RespuestaAlumno.TipoRespuestaPlantilla = tipoRespuesta;
                    (respuestaPreguntaDinamica.RespuestaAlumno as RespuestaDinamicaAbierta).RespuestaPlantilla = new RespuestaPlantillaGenericoTexto();
                    (respuestaPreguntaDinamica.RespuestaAlumno as RespuestaDinamicaAbierta).RespuestaPlantilla.TipoRespuestaPlantilla = tipoRespuesta;
                    if (row.IsNull("RespuestaPlantillaID"))
                        (respuestaPreguntaDinamica.RespuestaAlumno as RespuestaDinamicaAbierta).RespuestaPlantilla.RespuestaPlantillaID = null;
                    else
                        (respuestaPreguntaDinamica.RespuestaAlumno as RespuestaDinamicaAbierta).RespuestaPlantilla.RespuestaPlantillaID = (int)Convert.ChangeType(row["RespuestaPlantillaID"], typeof(int));

                    if (row.IsNull("TextoRespuesta"))
                        (respuestaPreguntaDinamica.RespuestaAlumno as RespuestaDinamicaAbierta).TextoRespuesta = null;
                    else
                        (respuestaPreguntaDinamica.RespuestaAlumno as RespuestaDinamicaAbierta).TextoRespuesta = (string)Convert.ChangeType(row["TextoRespuesta"], typeof(string));

                }
                else if (tipoRespuesta == ETipoRespuestaPlantilla.ABIERTA_NUMERICO)
                {
                    throw new Exception("Tipo no soportado");
                }
                else
                {
                    throw new Exception("Tipo no soportado");
                }

                if (row.IsNull("Tiempo"))
                    respuestaPreguntaDinamica.RespuestaAlumno.Tiempo = null;
                else
                    respuestaPreguntaDinamica.RespuestaAlumno.Tiempo = (int)Convert.ChangeType(row["Tiempo"], typeof(int));
            }

            if (row.IsNull("EstadoRespuesta"))
                respuestaPreguntaDinamica.EstadoRespuesta = null;
            else
                respuestaPreguntaDinamica.EstadoRespuesta = (EEstadoRespuesta)(byte)Convert.ChangeType(row["EstadoRespuesta"], typeof(byte));
            return respuestaPreguntaDinamica;
        }
    }
}
