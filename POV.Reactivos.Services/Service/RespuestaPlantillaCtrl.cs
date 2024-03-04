using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Reactivos.BO;
using POV.Reactivos.DAO;

namespace POV.Reactivos.Service
{
    /// <summary>
    /// Controlador del objeto RespuestaPlantilla
    /// </summary>
    public class RespuestaPlantillaCtrl
    {
        /// <summary>
        /// Consulta registros de RespuestaPlantillaRetHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="respuestaPlantillaRetHlp">RespuestaPlantillaRetHlp que provee el criterio de selecciÃ³n para realizar la consulta</param>
        /// <returns>El DataSet que contiene la informaciÃ³n de RespuestaPlantillaRetHlp generada por la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, RespuestaPlantilla respuestaPlantilla, Pregunta pregunta, ETipoReactivo tipoReactivo)
        {
            DataSet ds = null;
            switch (tipoReactivo)
            {
                case ETipoReactivo.InicialDiagnostico:
                case ETipoReactivo.Diagnostico:
                    RespuestaPlantillaDiagnosticoRetHlp daDiagnostico = new RespuestaPlantillaDiagnosticoRetHlp();
                    ds = daDiagnostico.Action(dctx, (respuestaPlantilla as RespuestaPlantillaOpcionMultiple), pregunta);
                    break;
                case ETipoReactivo.Estandarizado:
                    RespuestaPlantillaRetHlp da = new RespuestaPlantillaRetHlp();
                    ds = da.Action(dctx, respuestaPlantilla, pregunta);
                    break;
                case ETipoReactivo.ModeloGenerico:
                    RespuestaPlantillaDinamicoRetHlp daDinamico = new RespuestaPlantillaDinamicoRetHlp();
                    ds = daDinamico.Action(dctx, pregunta, respuestaPlantilla);
                    break;
            }
            return ds;
        }
        /// <summary>
        /// Consulta registros de RespuestaPlantillaOpcionMultipleRetHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="respuestaPlantillaOpcionMultipleRetHlp">RespuestaPlantillaOpcionMultipleRetHlp que provee el criterio de selecciÃ³n para realizar la consulta</param>
        /// <returns>El DataSet que contiene la informaciÃ³n de RespuestaPlantillaOpcionMultipleRetHlp generada por la consulta</returns>
        public DataSet RetrieveRespuestaPlantillaOpcionMultiple(IDataContext dctx, RespuestaPlantillaOpcionMultiple respuestaPlantilla, Pregunta pregunta, ETipoReactivo tipoReactivo)
        {
            DataSet ds = null;
            switch (tipoReactivo)
            {
                case ETipoReactivo.Estandarizado:
                    RespuestaPlantillaOpcionMultipleRetHlp da = new RespuestaPlantillaOpcionMultipleRetHlp();
                    ds = da.Action(dctx, respuestaPlantilla, pregunta);
                    break;
                case ETipoReactivo.Diagnostico:
                case ETipoReactivo.InicialDiagnostico:
                    RespuestaPlantillaDiagnosticoRetHlp daDiagnostico = new RespuestaPlantillaDiagnosticoRetHlp();
                    ds = daDiagnostico.Action(dctx, respuestaPlantilla, pregunta);
                    break;
                case ETipoReactivo.ModeloGenerico:
                    RespuestaPlantillaOpcionMultipleDinamicoRetHlp daDinamico = new RespuestaPlantillaOpcionMultipleDinamicoRetHlp();
                    ds = daDinamico.Action(dctx, pregunta, respuestaPlantilla);
                    break;
            }

            return ds;
        }
        /// <summary>
        /// Consultar registro de RespuestaPlantillaOpcionMultiple (Consulta completa)
        /// </summary>
        /// <param name="dctx">Contexto de datos</param>
        /// <param name="respuestaPlantilla">Objeto RespuestaPlantillaOpcionMultiple utilizado para el filtrado.</param>
        /// <param name="pregunta">Objeto Pregunta utilizado para el filtrado</param>
        /// <param name="tipoReactivo">Tipo de reactivo al cual correspondera la consulta.</param>
        /// <returns></returns>
        public RespuestaPlantillaOpcionMultiple RetrieveCompleteRespuestaPlantillaOpcionMultiple(IDataContext dctx, RespuestaPlantillaOpcionMultiple respuestaPlantilla, Pregunta pregunta, ETipoReactivo tipoReactivo)
        {
            RespuestaPlantillaOpcionMultiple rpom = LastDataRowToRespuestaPlantillaOpcionMultiple(RetrieveRespuestaPlantillaOpcionMultiple(dctx, respuestaPlantilla, pregunta, tipoReactivo));

            OpcionRespuestaPlantillaCtrl opcionCtrl = new OpcionRespuestaPlantillaCtrl();
            OpcionRespuestaPlantilla opcion = new OpcionRespuestaPlantilla();

            if (tipoReactivo == ETipoReactivo.ModeloGenerico)
                opcion = new OpcionRespuestaModeloGenerico();
            opcion.Activo = true;
            DataSet opcionesDS = opcionCtrl.Retrieve(dctx, opcion, rpom, tipoReactivo);

            foreach (DataRow dr in opcionesDS.Tables[0].Rows)
            {
                rpom.ListaOpcionRespuestaPlantilla.Add(opcionCtrl.DataRowToOpcionRespuestaPlantilla(dr, tipoReactivo));
            }
            return rpom;
        }
        /// <summary>
        /// Consulta registros de RetrieveRespuestaPlantillaAbierta en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveer acceso a la base de datos</param>
        /// <param name="tipoReactivo">Tipo reactivo a consultar</param>
        /// <param name="respuestaPlantillaAbierta">RetrieveRespuestaPlantillaAbiertaRetHlp que provee el criterio de selecciÃ³n para realizar la consulta </param>
        /// <param name="pregunta">Pregunta que provee el criterio de selección</param>
        /// <returns>El DataSet que contiene la informaciÃ³n de RespuestaPlantillaAbiertaRetHlp generada por la consulta</returns>
        public DataSet RetrieveRespuestaPlantillaAbierta(IDataContext dctx, RespuestaPlantillaAbierta respuestaPlantillaAbierta, Pregunta pregunta, ETipoReactivo tipoReactivo)
        {
            DataSet ds = null;
            switch (tipoReactivo)
            {
                case ETipoReactivo.ModeloGenerico:
                    RespuestaPlantillaAbiertaRetHlp da = new RespuestaPlantillaAbiertaRetHlp();
                    ds = da.Action(dctx, respuestaPlantillaAbierta, pregunta);
                    break;
            }
            return ds;
        }
        /// <summary>
        /// Guarda un registro de RespuestaPlantilla
        /// </summary>
        public void Insert(IDataContext dctx, RespuestaPlantilla respuestaPlantilla, Pregunta pregunta, ETipoReactivo tipoReactivo)
        {

            if (respuestaPlantilla.TipoRespuestaPlantilla != null)
            {
                RespuestaPlantillaInsHlp da = new RespuestaPlantillaInsHlp();
                RespuestaPlantillaDiagnosticoInsHlp daDiag = new RespuestaPlantillaDiagnosticoInsHlp();
                RespuestaPlantillaDinamicoInsHlp daDinamica = new RespuestaPlantillaDinamicoInsHlp();
                DataSet ds = null;
                switch (respuestaPlantilla.TipoRespuestaPlantilla)
                {
                    case ETipoRespuestaPlantilla.OPCION_MULTIPLE:
                        RespuestaPlantillaOpcionMultiple respuestaPlantillaOpcionMultiple = (RespuestaPlantillaOpcionMultiple)respuestaPlantilla;
                        respuestaPlantillaOpcionMultiple.FechaRegistro = DateTime.Now;
                        
                        switch (tipoReactivo)
                        {
                            case ETipoReactivo.Estandarizado:
                                respuestaPlantillaOpcionMultiple.ModoSeleccion = EModoSeleccion.UNICA;
                                da.Action(dctx, respuestaPlantillaOpcionMultiple, pregunta);
                                ds = this.Retrieve(dctx, respuestaPlantillaOpcionMultiple, pregunta, tipoReactivo);
                                int index = ds.Tables["RespuestaPlantilla"].Rows.Count;
                                respuestaPlantillaOpcionMultiple.RespuestaPlantillaID = Convert.ToInt32(ds.Tables["RespuestaPlantilla"].Rows[index - 1]["RespuestaPlantillaID"]);
                                RespuestaPlantillaOpcionMultipleInsHlp rpomDa = new RespuestaPlantillaOpcionMultipleInsHlp();
                                rpomDa.Action(dctx, respuestaPlantillaOpcionMultiple);
                                break;
                            case ETipoReactivo.InicialDiagnostico:
                            case ETipoReactivo.Diagnostico:
                                respuestaPlantillaOpcionMultiple.ModoSeleccion = EModoSeleccion.UNICA;
                                daDiag.Action(dctx, respuestaPlantillaOpcionMultiple, pregunta);
                                ds = this.Retrieve(dctx, respuestaPlantillaOpcionMultiple, pregunta, tipoReactivo);
                                int indexDiag = ds.Tables[0].Rows.Count;
                                respuestaPlantillaOpcionMultiple.RespuestaPlantillaID = Convert.ToInt32(ds.Tables[0].Rows[indexDiag - 1]["RespuestaPlantillaID"]);
                                break;
                            case ETipoReactivo.ModeloGenerico:
                                daDinamica.Action(dctx, pregunta, respuestaPlantillaOpcionMultiple);
                                ds = this.Retrieve(dctx, respuestaPlantillaOpcionMultiple, pregunta, tipoReactivo);
                                int indexDinamica = ds.Tables[0].Rows.Count;
                                respuestaPlantillaOpcionMultiple.RespuestaPlantillaID = Convert.ToInt32(ds.Tables[0].Rows[indexDinamica - 1]["RespuestaPlantillaID"]);
                                RespuestaPlantillaOpcionMultipleDinamicoInsHlp rpomDinamico = new RespuestaPlantillaOpcionMultipleDinamicoInsHlp();
                                rpomDinamico.Action(dctx, respuestaPlantillaOpcionMultiple);
                                
                                break;
                        }

                        break;
                    case ETipoRespuestaPlantilla.ABIERTA:
                        switch (tipoReactivo)
                        {
                            case ETipoReactivo.Estandarizado:
                                //RespuestaPlantillaTexto respuestaPlantillaTexto = (RespuestaPlantillaTexto)respuestaPlantilla;
                                //da.Action(dctx, respuestaPlantillaTexto, pregunta);
                                //DataSet dsrpa = this.Retrieve(dctx, respuestaPlantillaTexto, pregunta, tipoReactivo);
                                //int indexrpa = dsrpa.Tables["RespuestaPlantilla"].Rows.Count;
                                //respuestaPlantillaTexto.RespuestaPlantillaID = Convert.ToInt32(dsrpa.Tables["RespuestaPlantilla"].Rows[indexrpa - 1]["RespuestaPlantillaID"]);
                                //RespuestaPlantillaAbiertaInsHlp rpaDa = new RespuestaPlantillaAbiertaInsHlp();
                                //rpaDa.Action(dctx, respuestaPlantillaTexto);

                                break;
                            case ETipoReactivo.Diagnostico:
                                break;
                            case ETipoReactivo.InicialDiagnostico:
                                break;
                            case ETipoReactivo.ModeloGenerico:
                                RespuestaPlantillaTexto respuestaPlantillaTexto = (RespuestaPlantillaTexto)respuestaPlantilla;
                                da.Action(dctx, respuestaPlantillaTexto, pregunta);
                                DataSet dsrpa = this.Retrieve(dctx, respuestaPlantillaTexto, pregunta, tipoReactivo);
                                int indexrpa = dsrpa.Tables["RespuestaPlantilla"].Rows.Count;
                                respuestaPlantillaTexto.RespuestaPlantillaID = Convert.ToInt32(dsrpa.Tables["RespuestaPlantilla"].Rows[indexrpa - 1]["RespuestaPlantillaID"]);
                                RespuestaPlantillaAbiertaInsHlp rpaDa = new RespuestaPlantillaAbiertaInsHlp();
                                rpaDa.Action(dctx, respuestaPlantillaTexto);

                                break;
                            default:
                                throw new ArgumentOutOfRangeException("tipoReactivo");
                        }
                        break;
                    case ETipoRespuestaPlantilla.ABIERTA_NUMERICO:
                        switch (tipoReactivo)
                        {
                            case ETipoReactivo.Estandarizado:
                                break;
                            case ETipoReactivo.Diagnostico:
                                break;
                            case ETipoReactivo.InicialDiagnostico:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException("tipoReactivo");
                        }
                        break;
                    default:
                        throw new Exception("InsertRespuestaPlantilla: El tipo de respuesta plantilla no esta definido.");
                }
            }
            else
            {
                throw new Exception("InsertRespuestaPlantilla: El tipo de respuesta plantilla no puede ser nulo");
            }
        }
        
        public void Update(IDataContext dctx, RespuestaPlantilla respuestaPlantilla, RespuestaPlantilla previous, ETipoReactivo tipoReactivo)
        {
            RespuestaPlantillaUpdHlp da = new RespuestaPlantillaUpdHlp();
            RespuestaPlantillaDiagnosticoUpdHlp daDiag = new RespuestaPlantillaDiagnosticoUpdHlp();
            if (respuestaPlantilla.TipoRespuestaPlantilla != null && previous.TipoRespuestaPlantilla != null)
            {
                switch (previous.TipoRespuestaPlantilla)
                {
                    case ETipoRespuestaPlantilla.OPCION_MULTIPLE:
                        RespuestaPlantillaOpcionMultiple respuestaPlantillaOpcionMultiple = (RespuestaPlantillaOpcionMultiple)respuestaPlantilla;
                        RespuestaPlantillaOpcionMultiple previousRespuestaPlantillaOpcionMultiple = (RespuestaPlantillaOpcionMultiple)previous;
                        switch (tipoReactivo)
                        {
                            case ETipoReactivo.Estandarizado:
                                da.Action(dctx, respuestaPlantillaOpcionMultiple, previousRespuestaPlantillaOpcionMultiple);
                                RespuestaPlantillaOpcionMultipleUpdHlp rpomDa = new RespuestaPlantillaOpcionMultipleUpdHlp();
                                rpomDa.Action(dctx, respuestaPlantillaOpcionMultiple, previousRespuestaPlantillaOpcionMultiple);
                                break;
                            case ETipoReactivo.InicialDiagnostico:
                            case ETipoReactivo.Diagnostico:
                                daDiag.Action(dctx, previousRespuestaPlantillaOpcionMultiple, previousRespuestaPlantillaOpcionMultiple);
                                break;
                            case ETipoReactivo.ModeloGenerico:
                                RespuestaPlantillaDinamicoUpdHlp resDinamicoDa = new RespuestaPlantillaDinamicoUpdHlp();
                                RespuestaPlantillaOpcionMultipleDinamicoUpdHlp resDinaOpcDa = new RespuestaPlantillaOpcionMultipleDinamicoUpdHlp();

                                resDinamicoDa.Action(dctx, respuestaPlantillaOpcionMultiple, previousRespuestaPlantillaOpcionMultiple);

                                resDinaOpcDa.Action(dctx, respuestaPlantillaOpcionMultiple, previousRespuestaPlantillaOpcionMultiple);


                                break;
                        }
                        break;
                    case ETipoRespuestaPlantilla.ABIERTA:
                        switch (tipoReactivo)
                        {
                            case ETipoReactivo.Estandarizado:
                                //RespuestaPlantillaTexto respuestaPlantillaTexto = (RespuestaPlantillaTexto)respuestaPlantilla;
                                //RespuestaPlantillaTexto previousRespuestaPlantillaTexto = (RespuestaPlantillaTexto)previous;
                                //da.Action(dctx, respuestaPlantillaTexto, previousRespuestaPlantillaTexto);
                                //RespuestaPlantillaAbiertaUpdHlp rpaDa = new RespuestaPlantillaAbiertaUpdHlp();
                                //rpaDa.Action(dctx, respuestaPlantillaTexto, previousRespuestaPlantillaTexto);

                                break;
                            case ETipoReactivo.Diagnostico:
                                break;
                            case ETipoReactivo.InicialDiagnostico:
                                break;
                            case ETipoReactivo.ModeloGenerico:
                                RespuestaPlantillaTexto respuestaPlantillaTexto = (RespuestaPlantillaTexto)respuestaPlantilla;
                                RespuestaPlantillaTexto previousRespuestaPlantillaTexto = (RespuestaPlantillaTexto)previous;
                                da.Action(dctx, respuestaPlantillaTexto, previousRespuestaPlantillaTexto);
                                RespuestaPlantillaAbiertaUpdHlp rpaDa = new RespuestaPlantillaAbiertaUpdHlp();
                                rpaDa.Action(dctx, respuestaPlantillaTexto, previousRespuestaPlantillaTexto);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException("tipoReactivo");
                        }
                        break;
                    case ETipoRespuestaPlantilla.ABIERTA_NUMERICO:
                        switch (tipoReactivo)
                        {
                            case ETipoReactivo.Estandarizado:
                                break;
                            case ETipoReactivo.Diagnostico:
                                break;
                            case ETipoReactivo.InicialDiagnostico:
                                break;
                            case ETipoReactivo.ModeloGenerico:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException("tipoReactivo");
                        }
                        break;
                    default:
                        throw new Exception("UpdateRespuestaPlantilla: El tipo de respuesta plantilla no esta definido.");
                }
            }
            else
            {
                throw new Exception("UpdateRespuestaPlantilla: El tipo de respuesta plantilla no puede ser nulo");
            }
        }
        /// <summary>
        /// RespuestaPlantillaDelHlp
        /// </summary>
        public void Delete(IDataContext dctx, RespuestaPlantilla respuestaPlantilla, ETipoReactivo tipoReactivo)
        {
            RespuestaPlantillaDelHlp da = new RespuestaPlantillaDelHlp();
            RespuestaPlantillaDiagnosticoDelHlp daDiag = new RespuestaPlantillaDiagnosticoDelHlp();
            RespuestaPlantillaOpcionMultiple respuestaPlantillaOpcionMultiple = (RespuestaPlantillaOpcionMultiple)respuestaPlantilla;
            if (respuestaPlantilla.TipoRespuestaPlantilla != null)
            {
                switch (respuestaPlantilla.TipoRespuestaPlantilla)
                {
                    case ETipoRespuestaPlantilla.OPCION_MULTIPLE:
                        switch (tipoReactivo)
                        {
                            case ETipoReactivo.Estandarizado:
                                DeleteOpcionByRespuestaPlantillaHlp delOpcionDa = new DeleteOpcionByRespuestaPlantillaHlp();
                                delOpcionDa.Action(dctx, respuestaPlantillaOpcionMultiple);
                                RespuestaPlantillaOpcionMultipleDelHlp rpomDa = new RespuestaPlantillaOpcionMultipleDelHlp();
                                rpomDa.Action(dctx, respuestaPlantillaOpcionMultiple);
                                da.Action(dctx, respuestaPlantillaOpcionMultiple);
                                break;
                            case ETipoReactivo.InicialDiagnostico:
                            case ETipoReactivo.Diagnostico:
                                daDiag.Action(dctx, respuestaPlantillaOpcionMultiple);
                                break;
                        }
                        break;
                    case ETipoRespuestaPlantilla.ABIERTA:
                        switch (tipoReactivo)
                        {
                            case ETipoReactivo.Estandarizado:
                                RespuestaPlantillaAbierta respuestaPlantillaAbierta = (RespuestaPlantillaAbierta)respuestaPlantilla;
                                RespuestaPlantillaAbiertaDelHlp rpaDa = new RespuestaPlantillaAbiertaDelHlp();
                                rpaDa.Action(dctx, respuestaPlantillaAbierta);
                                da.Action(dctx, respuestaPlantillaAbierta);
                                break;
                            case ETipoReactivo.Diagnostico:
                                break;
                            case ETipoReactivo.InicialDiagnostico:
                                break;
                            case ETipoReactivo.ModeloGenerico:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException("tipoReactivo");
                        }

                        break;
                    case ETipoRespuestaPlantilla.ABIERTA_NUMERICO:
                        switch (tipoReactivo)
                        {
                            case ETipoReactivo.Estandarizado:
                                break;
                            case ETipoReactivo.Diagnostico:
                                break;
                            case ETipoReactivo.InicialDiagnostico:
                                break;
                            case ETipoReactivo.ModeloGenerico:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException("tipoReactivo");
                        }

                        break;
                    default:
                        throw new Exception("DeleteRespuestaPlantilla: El tipo de respuesta plantilla no esta definido.");
                }
            }
            else
            {
                throw new Exception("DeleteRespuestaPlantilla: El tipo de respuesta plantilla no puede ser nulo");
            }
        }
        /// <summary>
        /// Crea un objeto de RespuestaPlantillaOpcionMultiple a partir de los datos del Ãºltimo DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de RespuestaPlantillaOpcionMultiple</param>
        /// <returns>Un objeto de RespuestaPlantillaOpcionMultiple creado a partir de los datos</returns>
        public RespuestaPlantillaOpcionMultiple LastDataRowToRespuestaPlantillaOpcionMultiple(DataSet ds)
        {

            int index = ds.Tables[0].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToRespuestaPlantillaOpcionMultiple: El DataSet no tiene filas");
            return this.DataRowToRespuestaPlantillaOpcionMultiple(ds.Tables[0].Rows[index - 1]);
        }
        /// <summary>
        /// Crea un objeto de RespuestaPlantillaOpcionMultiple a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de RespuestaPlantillaOpcionMultiple</param>
        /// <returns>Un objeto de RespuestaPlantillaOpcionMultiple creado a partir de los datos</returns>
        public RespuestaPlantillaOpcionMultiple DataRowToRespuestaPlantillaOpcionMultiple(DataRow row)
        {
            RespuestaPlantillaOpcionMultiple respuestaPlantillaOpcionMultiple = new RespuestaPlantillaOpcionMultiple();
            respuestaPlantillaOpcionMultiple.ListaOpcionRespuestaPlantilla = new List<OpcionRespuestaPlantilla>();
            if (row.IsNull("RespuestaPlantillaID"))
                respuestaPlantillaOpcionMultiple.RespuestaPlantillaID = null;
            else
                respuestaPlantillaOpcionMultiple.RespuestaPlantillaID = (int)Convert.ChangeType(row["RespuestaPlantillaID"], typeof(int));
            if (row.IsNull("Estatus"))
                respuestaPlantillaOpcionMultiple.Estatus = null;
            else
                respuestaPlantillaOpcionMultiple.Estatus = (bool)Convert.ChangeType(row["Estatus"], typeof(bool));
            if (row.IsNull("FechaRegistro"))
                respuestaPlantillaOpcionMultiple.FechaRegistro = null;
            else
                respuestaPlantillaOpcionMultiple.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
            if (row.Table.Columns.Contains("TipoRespuestaPlantilla"))
            {
                if (row.IsNull("TipoRespuestaPlantilla"))
                    respuestaPlantillaOpcionMultiple.ToShortTipoRespuestaPlantilla = null;
                else
                    respuestaPlantillaOpcionMultiple.ToShortTipoRespuestaPlantilla = (short)Convert.ChangeType(row["TipoRespuestaPlantilla"], typeof(short));
            }
            if (row.Table.Columns.Contains("TipoPuntaje"))
            {
                if (row.IsNull("TipoPuntaje"))
                    respuestaPlantillaOpcionMultiple.TipoPuntaje = null;
                else
                    respuestaPlantillaOpcionMultiple.TipoPuntaje = (ETipoPuntaje?)(byte)Convert.ChangeType(row["TipoPuntaje"], typeof(byte));
            }
            if (row.IsNull("NumeroSeleccionablesMaximo"))
                respuestaPlantillaOpcionMultiple.NumeroSeleccionablesMaximo = null;
            else
                respuestaPlantillaOpcionMultiple.NumeroSeleccionablesMaximo = (int)Convert.ChangeType(row["NumeroSeleccionablesMaximo"], typeof(int));
            if (row.IsNull("NumeroSeleccionablesMinimo"))
                respuestaPlantillaOpcionMultiple.NumeroSeleccionablesMinimo = null;
            else
                respuestaPlantillaOpcionMultiple.NumeroSeleccionablesMinimo = (int)Convert.ChangeType(row["NumeroSeleccionablesMinimo"], typeof(int));
            if (row.IsNull("ModoSeleccion"))
                respuestaPlantillaOpcionMultiple.ToShortModoSeleccion = null;
            else
                respuestaPlantillaOpcionMultiple.ToShortModoSeleccion = (short)Convert.ChangeType(row["ModoSeleccion"], typeof(short));
            if (row.Table.Columns.Contains("PresentacionOpcion"))
            {
                if (row.IsNull("PresentacionOpcion"))
                    respuestaPlantillaOpcionMultiple.PresentacionOpcion = null;
                else
                    respuestaPlantillaOpcionMultiple.PresentacionOpcion = (EPresentacionOpcion)(byte)Convert.ChangeType(row["PresentacionOpcion"], typeof(Byte));
            }
            return respuestaPlantillaOpcionMultiple;
        }

        /// <summary>
        /// Crea un objeto de RespuestaPlantillaAbierta a partir de los datos del ultimo DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de RespuestaPlantillaAbierta</param>
        /// <param name="tipoReactivo">Tipo de reactivo</param>
        /// <returns>Un objeto de RespuestaPlantillaAbierta creado a partir de los datos</returns>
        public RespuestaPlantillaAbierta LastDataRowToRespuestaPlantillaAbierta(DataSet ds, ETipoReactivo? tipoReactivo = null)
        {
            if (!ds.Tables.Contains("RespuestaPlantillaAbierta"))
                throw new Exception("LastDataRowToRespuestaPlantillaAbierta: DataSet no tiene la tabla RespuestaPlantillaAbierta");
            int index = ds.Tables["RespuestaPlantillaAbierta"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToRespuestaPlantillaAbierta: El DataSet no tiene filas");
            return this.DataRowToRespuestaPlantillaAbierta(ds.Tables["RespuestaPlantillaAbierta"].Rows[index - 1]);
        }

        /// <summary>
        /// Crea un objeto de RespuestaPlantillaAbierta a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de RespuestaPlantillaAbierta</param>
        /// <param name="tipoReactivo">Tipo de reactivo</param>
        /// <returns>Un objeto de RespuestaPlantillaAbierta creado a partir de los datos</returns>
        public RespuestaPlantillaAbierta DataRowToRespuestaPlantillaAbierta(DataRow row, ETipoReactivo? tipoReactivo = null)
        {
            RespuestaPlantillaAbierta respuestaPlantillaAbierta = null;

            if (row.Table.Columns.Contains("TipoRespuestaPlantilla"))
            {
                if (row.IsNull("TipoRespuestaPlantilla"))
                    return null;
                else
                {
                    ETipoRespuestaPlantilla? tipoRespuestaPlantilla = (ETipoRespuestaPlantilla?)(short)Convert.ChangeType(row["TipoRespuestaPlantilla"], typeof(short));
                    switch (tipoRespuestaPlantilla)
                    {
                        case ETipoRespuestaPlantilla.ABIERTA:
                            respuestaPlantillaAbierta = DataRowToRespuestaPlantillaTexto(row);
                            break;
                        case ETipoRespuestaPlantilla.ABIERTA_NUMERICO:
                            respuestaPlantillaAbierta = DataRowToRespuestaPlantillaNumerico(row);
                            break;
                    }
                }
            }
            return respuestaPlantillaAbierta;
        }

        /// <summary>
        /// Consulta un registro RespuestaPlantilla
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="respuestaPlantilla">RespuestaPlantilla que provee el criterio de selección</param>
        /// <param name="pregunta">Pregunta que provee el criterio de selección</param>
        /// <param name="tipoReactivo">TipoReactivo que provee el criterio de selección</param>
        /// <returns></returns>
        public RespuestaPlantilla RetrieveComplete(IDataContext dctx, RespuestaPlantilla respuestaPlantilla, Pregunta pregunta, ETipoReactivo tipoReactivo)
        {
            DataSet ds = Retrieve(dctx, respuestaPlantilla, pregunta, tipoReactivo);
            RespuestaPlantilla resPlantilla = null;
            int index = ds.Tables[0].Rows.Count;
            if (index > 0)
            {
                int respuestaPlantillaID = Convert.ToInt32(ds.Tables["RespuestaPlantilla"].Rows[index - 1]["RespuestaPlantillaID"]);
                short tipoRespuestaPlantilla = (short)Convert.ChangeType(ds.Tables["RespuestaPlantilla"].Rows[index - 1]["TipoRespuestaPlantilla"], typeof(short));

                switch ((ETipoRespuestaPlantilla)tipoRespuestaPlantilla)
                {
                    case ETipoRespuestaPlantilla.ABIERTA:
                        resPlantilla = RetrieveCompleteRespuestaPlantillaAbierta(dctx, new RespuestaPlantillaTexto { RespuestaPlantillaID = respuestaPlantillaID, TipoRespuestaPlantilla = (ETipoRespuestaPlantilla?) tipoRespuestaPlantilla }, pregunta, tipoReactivo);
                        break;
                    case ETipoRespuestaPlantilla.OPCION_MULTIPLE:
                        resPlantilla = RetrieveCompleteRespuestaPlantillaOpcionMultiple(dctx, new RespuestaPlantillaOpcionMultiple { RespuestaPlantillaID = respuestaPlantillaID, TipoRespuestaPlantilla = (ETipoRespuestaPlantilla?)tipoRespuestaPlantilla }, pregunta, tipoReactivo);
                        break;
                    case ETipoRespuestaPlantilla.ABIERTA_NUMERICO:
                        resPlantilla = RetrieveCompleteRespuestaPlantillaAbierta(dctx, new RespuestaPlantillaNumerico { RespuestaPlantillaID = respuestaPlantillaID, TipoRespuestaPlantilla = (ETipoRespuestaPlantilla?)tipoRespuestaPlantilla }, pregunta, tipoReactivo);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return resPlantilla;
        }

        private RespuestaPlantillaAbierta RetrieveCompleteRespuestaPlantillaAbierta(IDataContext dctx, RespuestaPlantillaAbierta respuestaPlantilla,  Pregunta pregunta, ETipoReactivo tipoReactivo)
        {
            DataSet ds = RetrieveRespuestaPlantillaAbierta(dctx, respuestaPlantilla, pregunta, tipoReactivo);
            RespuestaPlantillaAbierta respuestaPlantillaAbierta = null;
            int index = ds.Tables[0].Rows.Count;
            if (index > 0)
            {
                respuestaPlantillaAbierta = LastDataRowToRespuestaPlantillaAbierta(ds, tipoReactivo);
            }

            return respuestaPlantillaAbierta;
        }


        /// <summary>
        /// Crea un objeto de RespuestaPlantillaTexto a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la informaciÃ³n de RespuestaPlantillaAbierta</param>
        /// <returns>Un objeto de RespuestaPlantillaTexto creado a partir de los datos</returns>
        private RespuestaPlantillaTexto DataRowToRespuestaPlantillaTexto(DataRow row)
        {
            RespuestaPlantillaTexto respuestaPlantillaAbierta = new RespuestaPlantillaTexto();
            if (row.IsNull("RespuestaPlantillaID"))
                respuestaPlantillaAbierta.RespuestaPlantillaID = null;
            else
                respuestaPlantillaAbierta.RespuestaPlantillaID = (int)Convert.ChangeType(row["RespuestaPlantillaID"], typeof(int));
            if (row.IsNull("Estatus"))
                respuestaPlantillaAbierta.Estatus = null;
            else
                respuestaPlantillaAbierta.Estatus = (bool)Convert.ChangeType(row["Estatus"], typeof(bool));
            if (row.IsNull("FechaRegistro"))
                respuestaPlantillaAbierta.FechaRegistro = null;
            else
                respuestaPlantillaAbierta.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
            if (row.IsNull("TipoRespuestaPlantilla"))
                respuestaPlantillaAbierta.TipoRespuestaPlantilla = null;
            else
                respuestaPlantillaAbierta.TipoRespuestaPlantilla = (ETipoRespuestaPlantilla?)(short)Convert.ChangeType(row["TipoRespuestaPlantilla"], typeof(short));
            if (row.IsNull("TipoPuntaje"))
                respuestaPlantillaAbierta.TipoPuntaje = null;
            else
                respuestaPlantillaAbierta.TipoPuntaje = (ETipoPuntaje?)(byte)Convert.ChangeType(row["TipoPuntaje"], typeof(byte));
            if (row.IsNull("Ponderacion"))
                respuestaPlantillaAbierta.Ponderacion = null;
            else
                respuestaPlantillaAbierta.Ponderacion = (decimal)Convert.ChangeType(row["Ponderacion"], typeof(decimal));
            if (row.IsNull("ValorRespuesta"))
                respuestaPlantillaAbierta.ValorRespuesta = null;
            else
                respuestaPlantillaAbierta.ValorRespuesta = (string)Convert.ChangeType(row["ValorRespuesta"], typeof(string));
            if (row.IsNull("MaximoCaracteres"))
                respuestaPlantillaAbierta.MaximoCaracteres = null;
            else
                respuestaPlantillaAbierta.MaximoCaracteres = (int)Convert.ChangeType(row["MaximoCaracteres"], typeof(int));
            if (row.IsNull("MinimoCaracteres"))
                respuestaPlantillaAbierta.MinimoCaracteres = null;
            else
                respuestaPlantillaAbierta.MinimoCaracteres = (int)Convert.ChangeType(row["MinimoCaracteres"], typeof(int));
            if (row.IsNull("EsRespuestaCorta"))
                respuestaPlantillaAbierta.EsRespuestaCorta = null;
            else
                respuestaPlantillaAbierta.EsRespuestaCorta = (bool)Convert.ChangeType(row["EsRespuestaCorta"], typeof(bool));
            if (row.IsNull("ModeloID"))
                respuestaPlantillaAbierta.Modelo = null;
            else
                respuestaPlantillaAbierta.Modelo = new Modelo.BO.ModeloDinamico(){ ModeloID = (Int32)Convert.ChangeType(row["ModeloID"], typeof(Int32))};
            if (row.IsNull("ClasificadorID"))
                respuestaPlantillaAbierta.Clasificador = null;
            else
                respuestaPlantillaAbierta.Clasificador = new Modelo.BO.Clasificador() { ClasificadorID = (Int32)Convert.ChangeType(row["ClasificadorID"], typeof(Int32)) };
            if (row.IsNull("EsSensibleMayusculaMinuscula"))
                respuestaPlantillaAbierta.EsSensibleMayusculaMinuscula = null;
            else
                respuestaPlantillaAbierta.EsSensibleMayusculaMinuscula = (bool)Convert.ChangeType(row["EsSensibleMayusculaMinuscula"], typeof(bool));


            return respuestaPlantillaAbierta;
        }

        private RespuestaPlantillaNumerico DataRowToRespuestaPlantillaNumerico(DataRow row)
        {
            RespuestaPlantillaNumerico respuestaPlantillaNumerico = new RespuestaPlantillaNumerico();
            if (row.IsNull("RespuestaPlantillaID"))
                respuestaPlantillaNumerico.RespuestaPlantillaID = null;
            else
                respuestaPlantillaNumerico.RespuestaPlantillaID = (int)Convert.ChangeType(row["RespuestaPlantillaID"], typeof(int));
            if (row.IsNull("Estatus"))
                respuestaPlantillaNumerico.Estatus = null;
            else
                respuestaPlantillaNumerico.Estatus = (bool)Convert.ChangeType(row["Estatus"], typeof(bool));
            if (row.IsNull("FechaRegistro"))
                respuestaPlantillaNumerico.FechaRegistro = null;
            else
                respuestaPlantillaNumerico.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
            if (row.IsNull("TipoRespuestaPlantilla"))
                respuestaPlantillaNumerico.ToShortTipoRespuestaPlantilla = null;
            else
                respuestaPlantillaNumerico.ToShortTipoRespuestaPlantilla = (short)Convert.ChangeType(row["TipoRespuestaPlantilla"], typeof(short));
            if (row.IsNull("Ponderacion"))
                respuestaPlantillaNumerico.Ponderacion = null;
            else
                respuestaPlantillaNumerico.Ponderacion = (decimal)Convert.ChangeType(row["Ponderacion"], typeof(decimal));
            if (row.IsNull("ValorRespuesta"))
                respuestaPlantillaNumerico.ValorRespuesta = null;
            else
                respuestaPlantillaNumerico.ValorRespuesta = (string)Convert.ChangeType(row["ValorRespuesta"], typeof(string));
            if (row.IsNull("MargenError"))
                respuestaPlantillaNumerico.MargenError = null;
            else
                respuestaPlantillaNumerico.MargenError = (decimal)Convert.ChangeType(row["MargenError"], typeof(decimal));
            if (row.IsNull("NumeroDecimales"))
                respuestaPlantillaNumerico.NumeroDecimales = null;
            else
                respuestaPlantillaNumerico.NumeroDecimales = (int)Convert.ChangeType(row["NumeroDecimales"], typeof(int));
            if (row.IsNull("TipoMargen"))
                respuestaPlantillaNumerico.TipoMargen = null;
            else
                respuestaPlantillaNumerico.TipoMargen = (ETipoMargen)Convert.ChangeType(row["TipoMargen"], typeof(byte));
            if (row.IsNull("TipoPuntaje"))
                respuestaPlantillaNumerico.TipoPuntaje = null;
            else
                respuestaPlantillaNumerico.TipoPuntaje = (ETipoPuntaje)Convert.ChangeType(row["TipoPuntaje"], typeof(byte));

            return respuestaPlantillaNumerico;
        }



    }
}
