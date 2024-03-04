using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Linq;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Reactivos.BO;
using POV.Reactivos.DAO;

namespace POV.Reactivos.Service
{
    /// <summary>
    /// Controlador del objeto Pregunta
    /// </summary>
    public class PreguntaCtrl
    {
        /// <summary>
        /// Consulta registros de PreguntaRetHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="preguntaRetHlp">PreguntaRetHlp que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de PreguntaRetHlp generada por la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, Pregunta pregunta, Reactivo reactivo)
        {
            if (reactivo == null) throw new Exception("PreguntaCtrl: El parámetro 'reactivo' no puede ser vacio.");
            if (reactivo.TipoReactivo == null) throw new Exception("PreguntaCtrl: El campo 'reactivo.TipoReactivo' no puede ser vacio.");
            DataSet ds = null;
            switch (reactivo.TipoReactivo.Value)
            {
                case ETipoReactivo.Diagnostico:
                case ETipoReactivo.InicialDiagnostico:
                    PreguntaDiagnosticoRetHlp helperDiagnostico = new PreguntaDiagnosticoRetHlp();
                    ds = helperDiagnostico.Action(dctx, pregunta, reactivo);
                    break;
                case ETipoReactivo.Estandarizado:
                    PreguntaRetHlp helper = new PreguntaRetHlp();
                    ds = helper.Action(dctx, pregunta, reactivo);
                    break;
                case ETipoReactivo.ModeloGenerico:
                    PreguntaDinamicoRetHlp helperDinamico = new PreguntaDinamicoRetHlp();
                    ds = helperDinamico.Action(dctx, reactivo, pregunta);
                    break;

            }

            return ds;
        }

        public Pregunta RetrieveComplete(IDataContext dctx, Pregunta pregunta, Reactivo reactivo)
        {
            if (reactivo == null) throw new Exception("PreguntaCtrl: El parámetro 'reactivo' no puede ser vacio.");
            if (reactivo.TipoReactivo == null) throw new Exception("PreguntaCtrl: El campo 'reactivo.TipoReactivo' no puede ser vacio.");

            pregunta = LastDataRowToPregunta(Retrieve(dctx, pregunta, reactivo));

            RespuestaPlantillaCtrl respuestaPlantillaCtrl = new RespuestaPlantillaCtrl();

            DataSet ds = respuestaPlantillaCtrl.Retrieve(dctx, new RespuestaPlantillaOpcionMultiple(), pregunta, reactivo.TipoReactivo.Value);
            int index = ds.Tables[0].Rows.Count;

            ETipoRespuestaPlantilla tipo = (ETipoRespuestaPlantilla)Convert.ToInt16(ds.Tables[0].Rows[index - 1]["TipoRespuestaPlantilla"]);
            int idRespuestaPlantilla = Convert.ToInt32(ds.Tables[0].Rows[index - 1]["RespuestaPlantillaID"]);
            switch (tipo)
            {
                case ETipoRespuestaPlantilla.ABIERTA:
                    pregunta.RespuestaPlantilla = respuestaPlantillaCtrl.LastDataRowToRespuestaPlantillaAbierta(respuestaPlantillaCtrl.RetrieveRespuestaPlantillaAbierta(dctx, new RespuestaPlantillaTexto { RespuestaPlantillaID = idRespuestaPlantilla }, pregunta, reactivo.TipoReactivo.Value));
                    break;
                case ETipoRespuestaPlantilla.OPCION_MULTIPLE:
                    pregunta.RespuestaPlantilla = respuestaPlantillaCtrl.RetrieveCompleteRespuestaPlantillaOpcionMultiple(dctx, new RespuestaPlantillaOpcionMultiple { RespuestaPlantillaID = idRespuestaPlantilla }, pregunta, reactivo.TipoReactivo.Value);
                    break;
                case ETipoRespuestaPlantilla.ABIERTA_NUMERICO:
                    pregunta.RespuestaPlantilla = respuestaPlantillaCtrl.LastDataRowToRespuestaPlantillaAbierta(respuestaPlantillaCtrl.RetrieveRespuestaPlantillaAbierta(dctx, new RespuestaPlantillaNumerico { RespuestaPlantillaID = idRespuestaPlantilla }, pregunta, reactivo.TipoReactivo.Value));
                    break;
            }

            return pregunta;
        }

        public List<Pregunta> RetrieveListPreguntasReactivo(IDataContext dctx, Reactivo reactivo)
        {
            List<Pregunta> preguntas = new List<Pregunta>();
            DataSet preguntaDS = Retrieve(dctx, new Pregunta { Activo = true }, reactivo);
            foreach (DataRow preguntaRow in preguntaDS.Tables["Pregunta"].Rows)
            {
                Pregunta pregunta = DataRowToPregunta(preguntaRow);

                RespuestaPlantillaCtrl respuestaPlantillaCtrl = new RespuestaPlantillaCtrl();

                DataSet ds = respuestaPlantillaCtrl.Retrieve(dctx, new RespuestaPlantillaOpcionMultiple(), pregunta, reactivo.TipoReactivo.Value);
                int index = ds.Tables[0].Rows.Count;

                ETipoRespuestaPlantilla tipo = (ETipoRespuestaPlantilla)Convert.ToInt16(ds.Tables[0].Rows[index - 1]["TipoRespuestaPlantilla"]);
                int idRespuestaPlantilla = Convert.ToInt32(ds.Tables[0].Rows[index - 1]["RespuestaPlantillaID"]);
                switch (tipo)
                {
                    case ETipoRespuestaPlantilla.ABIERTA:
                        pregunta.RespuestaPlantilla = respuestaPlantillaCtrl.LastDataRowToRespuestaPlantillaAbierta(respuestaPlantillaCtrl.RetrieveRespuestaPlantillaAbierta(dctx, new RespuestaPlantillaTexto { RespuestaPlantillaID = idRespuestaPlantilla }, pregunta, reactivo.TipoReactivo.Value));
                        break;
                    case ETipoRespuestaPlantilla.OPCION_MULTIPLE:
                        pregunta.RespuestaPlantilla = respuestaPlantillaCtrl.RetrieveCompleteRespuestaPlantillaOpcionMultiple(dctx, new RespuestaPlantillaOpcionMultiple { RespuestaPlantillaID = idRespuestaPlantilla }, pregunta, reactivo.TipoReactivo.Value);
                        break;
                    case ETipoRespuestaPlantilla.ABIERTA_NUMERICO:
                        pregunta.RespuestaPlantilla = respuestaPlantillaCtrl.LastDataRowToRespuestaPlantillaAbierta(respuestaPlantillaCtrl.RetrieveRespuestaPlantillaAbierta(dctx, new RespuestaPlantillaNumerico { RespuestaPlantillaID = idRespuestaPlantilla }, pregunta, reactivo.TipoReactivo.Value));

                        break;
                }

                preguntas.Add(pregunta);
            }

            return preguntas;
        }
        /// <summary>
        /// Crea un registro de PreguntaInsHlp en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="preguntaInsHlp">PreguntaInsHlp que desea crear</param>
        public void Insert(IDataContext dctx, Pregunta pregunta, Reactivo reactivo)
        {
            if (reactivo == null) throw new Exception("PreguntaCtrl: El parámetro 'reactivo' no puede ser vacio.");
            if (reactivo.TipoReactivo == null) throw new Exception("PreguntaCtrl: El campo 'reactivo.TipoReactivo' no puede ser vacio.");
            switch (reactivo.TipoReactivo.Value)
            {
                case ETipoReactivo.Estandarizado:
                    PreguntaInsHlp da = new PreguntaInsHlp();
                    da.Action(dctx, pregunta, reactivo);
                    break;
                case ETipoReactivo.InicialDiagnostico:
                case ETipoReactivo.Diagnostico:
                    PreguntaDiagnosticoInsHlp daDiag = new PreguntaDiagnosticoInsHlp();
                    daDiag.Action(dctx, pregunta, reactivo);
                    break;
                case ETipoReactivo.ModeloGenerico:
                    PreguntaDinamicoInsHlp daDinamico = new PreguntaDinamicoInsHlp();
                    daDinamico.Action(dctx, reactivo, pregunta);
                    break;
            }
        }

        public void InsertComplete(IDataContext dctx, Pregunta pregunta, Reactivo reactivo)
        {
            if (reactivo == null) throw new Exception("PreguntaCtrl: El parámetro 'reactivo' no puede ser vacio.");
            if (reactivo.TipoReactivo == null) throw new Exception("PreguntaCtrl: El campo 'reactivo.TipoReactivo' no puede ser vacio.");
            if (pregunta == null) throw new Exception("PreguntaCtrl: El parámetro 'pregunta' no puede ser vacio.");

            string sError = string.Empty; 
            if (string.IsNullOrEmpty(pregunta.TextoPregunta))
                sError += ", Texto de la Pregunta";

            if (sError.Length > 0)
                throw new Exception("PreguntaCtrl: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (pregunta.RespuestaPlantilla == null)
                throw new Exception("PreguntaCtrl: Seleccione una plantilla de respuesta ");

            RespuestaPlantillaCtrl respuestaPlantillaCtrl = new RespuestaPlantillaCtrl();
            object myFirm = new object();

            try
            {
                dctx.OpenConnection(myFirm);
                dctx.BeginTransaction(myFirm);

                Insert(dctx, pregunta, reactivo);

                Pregunta preguntaInsert = LastDataRowToPregunta(Retrieve(dctx, pregunta, reactivo));

                pregunta.PreguntaID = preguntaInsert.PreguntaID;
                pregunta.RespuestaPlantilla.Estatus = true;
                pregunta.RespuestaPlantilla.FechaRegistro = DateTime.Now;

                respuestaPlantillaCtrl.Insert(dctx, pregunta.RespuestaPlantilla, pregunta, reactivo.TipoReactivo.Value);

                switch (pregunta.RespuestaPlantilla.TipoRespuestaPlantilla)
                {
                    case ETipoRespuestaPlantilla.OPCION_MULTIPLE:
                        if (pregunta.RespuestaPlantilla is RespuestaPlantillaOpcionMultiple)
                        {
                            OpcionRespuestaPlantillaCtrl opcionRespuestaPlatillaCtrl = new OpcionRespuestaPlantillaCtrl();
                            if (pregunta.RespuestaPlantilla.RespuestaPlantillaID == null)
                            {
                                pregunta.RespuestaPlantilla.RespuestaPlantillaID =
                                    respuestaPlantillaCtrl.LastDataRowToRespuestaPlantillaOpcionMultiple(
                                    respuestaPlantillaCtrl.RetrieveRespuestaPlantillaOpcionMultiple(
                                    dctx, (RespuestaPlantillaOpcionMultiple)pregunta.RespuestaPlantilla, pregunta, reactivo.TipoReactivo.Value)).RespuestaPlantillaID;
                            }
                            RespuestaPlantillaOpcionMultiple plantilla = (RespuestaPlantillaOpcionMultiple)pregunta.RespuestaPlantilla;
                            if (plantilla.ListaOpcionRespuestaPlantilla != null && plantilla.ListaOpcionRespuestaPlantilla.Count > 0)
                            {
                                foreach (OpcionRespuestaPlantilla opcion in plantilla.ListaOpcionRespuestaPlantilla)
                                {
                                    opcionRespuestaPlatillaCtrl.Insert(dctx, opcion, plantilla, reactivo.TipoReactivo.Value);
                                }
                            }
                        }
                        else
                        {
                            throw new Exception("PreguntaCtrl: Error de datos, la Respuesta no es del tipo correcto; se esperaba OPCION_MULTIPLE");
                        }
                        break;
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

        public void UpdateComplete(IDataContext dctx, Pregunta pregunta, Pregunta previous, Reactivo reactivo)
        {
            if (reactivo == null) throw new Exception("PreguntaCtrl: El parámetro 'reactivo' no puede ser vacio.");
            if (reactivo.TipoReactivo == null) throw new Exception("PreguntaCtrl: El campo 'reactivo.TipoReactivo' no puede ser vacio.");

            RespuestaPlantillaCtrl respuestaPlantillaCtrl = new RespuestaPlantillaCtrl();
            object myFirm = new object();
            string sError = string.Empty;
            if (string.IsNullOrEmpty(pregunta.TextoPregunta))
                sError += ", Texto de la Pregunta";
            if (sError.Length > 0)
                throw new Exception("ReactivoCtrl: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (pregunta.RespuestaPlantilla == null)
                throw new Exception("Seleccione una plantilla de respuesta ");
            try
            {
                dctx.OpenConnection(myFirm);
                dctx.BeginTransaction(myFirm);

                Update(dctx, pregunta, previous, reactivo);

                if (pregunta.RespuestaPlantilla is RespuestaPlantillaAbierta)
                {
                    respuestaPlantillaCtrl.Update(dctx,pregunta.RespuestaPlantilla,previous.RespuestaPlantilla,reactivo.TipoReactivo.Value);
                }

                if (pregunta.RespuestaPlantilla is RespuestaPlantillaOpcionMultiple)
                {
                    respuestaPlantillaCtrl.Update(dctx, pregunta.RespuestaPlantilla, previous.RespuestaPlantilla, reactivo.TipoReactivo.Value);
                    OpcionRespuestaPlantillaCtrl opcionRespuestaPlatillaCtrl = new OpcionRespuestaPlantillaCtrl();
                    //pregunta.RespuestaPlantilla.RespuestaPlantillaID = respuestaPlantillaCtrl.LastDataRowToRespuestaPlantillaOpcionMultiple(respuestaPlantillaCtrl.RetrieveRespuestaPlantillaOpcionMultiple(dctx, (RespuestaPlantillaOpcionMultiple)pregunta.RespuestaPlantilla, pregunta)).RespuestaPlantillaID;
                    RespuestaPlantillaOpcionMultiple plantilla = (RespuestaPlantillaOpcionMultiple)pregunta.RespuestaPlantilla;
                    foreach (OpcionRespuestaPlantilla opcion in plantilla.ListaOpcionRespuestaPlantilla)
                    {
                        if (opcion.OpcionRespuestaPlantillaID == null) {
                            opcionRespuestaPlatillaCtrl.Insert(dctx, opcion, plantilla, reactivo.TipoReactivo.Value);
                        }
                        else{
                            OpcionRespuestaPlantilla opcionAnterior = (previous.RespuestaPlantilla as RespuestaPlantillaOpcionMultiple).ListaOpcionRespuestaPlantilla.FirstOrDefault(item => item.OpcionRespuestaPlantillaID == opcion.OpcionRespuestaPlantillaID);

                            if (opcionAnterior != null)
                                opcionRespuestaPlatillaCtrl.Update(dctx, opcion, opcionAnterior, reactivo.TipoReactivo.Value);
                            else
                                opcionRespuestaPlatillaCtrl.Update(dctx, opcion, opcion, reactivo.TipoReactivo.Value);
                        }
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
        /// Actualiza de manera optimista un registro de PreguntaUpdHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="preguntaUpdHlp">PreguntaUpdHlp que tiene los datos nuevos</param>
        /// <param name="anterior">PreguntaUpdHlp que tiene los datos anteriores</param>
        public void Update(IDataContext dctx, Pregunta pregunta, Pregunta previous, Reactivo reactivo)
        {
            if (reactivo == null) throw new Exception("PreguntaCtrl: El parámetro 'reactivo' no puede ser vacio.");
            if (reactivo.TipoReactivo == null) throw new Exception("PreguntaCtrl: El campo 'reactivo.TipoReactivo' no puede ser vacio.");

            switch (reactivo.TipoReactivo.Value)
            {
                case ETipoReactivo.Diagnostico:
                case ETipoReactivo.InicialDiagnostico:
                    PreguntaDiagnosticoUpdHlp helperDiagnostico = new PreguntaDiagnosticoUpdHlp();
                    helperDiagnostico.Action(dctx, pregunta, previous);
                    break;
                case ETipoReactivo.Estandarizado:
                    PreguntaUpdHlp helper = new PreguntaUpdHlp();
                    helper.Action(dctx, pregunta, previous);
                    break;
                case ETipoReactivo.ModeloGenerico:
                    PreguntaDinamicoUpdHlp preDinamicoUpdHlp = new PreguntaDinamicoUpdHlp();
                    preDinamicoUpdHlp.Action(dctx, pregunta, previous);
                    break;
            }
        }
        /// <summary>
        /// Elimina un registro de PreguntaDelHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="preguntaDelHlp">PreguntaDelHlp que desea eliminar</param>
        public void Delete(IDataContext dctx, Pregunta pregunta, Reactivo reactivo)
        {

            if (reactivo == null) throw new Exception("PreguntaCtrl: El parámetro 'reactivo' no puede ser vacio.");
            if (reactivo.TipoReactivo == null) throw new Exception("PreguntaCtrl: El campo 'reactivo.TipoReactivo' no puede ser vacio.");

            switch (reactivo.TipoReactivo.Value)
            {
                case ETipoReactivo.Diagnostico:
                case ETipoReactivo.InicialDiagnostico:
                    PreguntaDiagnosticoDelHlp helperDiagnostico = new PreguntaDiagnosticoDelHlp();
                    helperDiagnostico.Action(dctx, pregunta);
                    break;
                case ETipoReactivo.Estandarizado:
                    PreguntaDelHlp helper = new PreguntaDelHlp();
                    helper.Action(dctx, pregunta);
                    break;
            }
        }

        /// <summary>
        /// Crea un objeto de Pregunta a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de Pregunta</param>
        /// <returns>Un objeto de Pregunta creado a partir de los datos</returns>
        public Pregunta LastDataRowToPregunta(DataSet ds)
        {
            if (!ds.Tables.Contains("Pregunta"))
                throw new Exception("LastDataRowToPregunta: DataSet no tiene la tabla Pregunta");
            int index = ds.Tables["Pregunta"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToPregunta: El DataSet no tiene filas");
            return this.DataRowToPregunta(ds.Tables["Pregunta"].Rows[index - 1]);
        }
        /// <summary>
        /// Crea un objeto de Pregunta a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la informaciÃ³n de Pregunta</param>
        /// <returns>Un objeto de Pregunta creado a partir de los datos</returns>
        public Pregunta DataRowToPregunta(DataRow row)
        {
            Pregunta pregunta = new Pregunta();
            if (row.IsNull("PreguntaID"))
                pregunta.PreguntaID = null;
            else
                pregunta.PreguntaID = (int)Convert.ChangeType(row["PreguntaID"], typeof(int));
            if (row.IsNull("Orden"))
                pregunta.Orden = null;
            else
                pregunta.Orden = (int)Convert.ChangeType(row["Orden"], typeof(int));
            if (row.Table.Columns.Contains("Descripcion"))
            {
                if (row.IsNull("Descripcion"))
                    pregunta.Descripcion = null;
                else
                    pregunta.Descripcion = (string)Convert.ChangeType(row["Descripcion"], typeof(string));
            }
            if (row.IsNull("TextoPregunta"))
                pregunta.TextoPregunta = null;
            else
                pregunta.TextoPregunta = (string)Convert.ChangeType(row["TextoPregunta"], typeof(string));
            if (row.IsNull("Valor"))
                pregunta.Valor = null;
            else
                pregunta.Valor = (decimal)Convert.ChangeType(row["Valor"], typeof(decimal));
            if (row.IsNull("PlantillaPregunta"))
                pregunta.PlantillaPregunta = null;
            else
                pregunta.PlantillaPregunta = (string)Convert.ChangeType(row["PlantillaPregunta"], typeof(string));
            if (row.IsNull("FechaRegistro"))
                pregunta.FechaRegistro = null;
            else
                pregunta.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
            if (row.Table.Columns.Contains("PuedeOmitir"))
            {
                if (row.IsNull("PuedeOmitir"))
                    pregunta.PuedeOmitir = null;
                else
                    pregunta.PuedeOmitir = Convert.ToBoolean(row["PuedeOmitir"]);
            }
            if (row.Table.Columns.Contains("SoloImagen"))
            {
                if (row.IsNull("SoloImagen"))
                    pregunta.SoloImagen = null;
                else
                    pregunta.SoloImagen = Convert.ToBoolean(row["SoloImagen"]);
            }
            if (row.IsNull("Activo"))
                pregunta.Activo = null;
            else
                pregunta.Activo = Convert.ToBoolean(row["Activo"]);
            if (row.Table.Columns.Contains("PresentacionPlantilla"))
            {
                if (row.IsNull("PresentacionPlantilla"))
                    pregunta.PresentacionPlantilla = null;
                else
                    pregunta.PresentacionPlantilla = (EPresentacionPlantilla)Convert.ChangeType(row["PresentacionPlantilla"], typeof(Byte));
            }
            return pregunta;
        }
    }
}
