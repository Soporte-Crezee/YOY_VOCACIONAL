using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Reactivos.BO;
using POV.Reactivos.DAO;
using POV.Modelo.BO;

namespace POV.Reactivos.Service { 
   /// <summary>
   /// Controlador del objeto OpcionRespuestaPlantilla
   /// </summary>
   public class OpcionRespuestaPlantillaCtrl { 
        /// <summary>
        /// Consulta registros de OpcionRespuestaPlantillaRetHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="opcionRespuestaPlantillaRetHlp">OpcionRespuestaPlantillaRetHlp que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de OpcionRespuestaPlantillaRetHlp generada por la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, OpcionRespuestaPlantilla opcionRespuestaPlantilla, RespuestaPlantilla respuestaPlantilla, ETipoReactivo tipoReactivo){
            if (tipoReactivo == null) throw new Exception("El parámetro 'TipoReactivo' es requerido para realizar esta operación.");
            DataSet ds = null;
            switch (tipoReactivo) 
            {
                case ETipoReactivo.InicialDiagnostico:
                    break;
                case ETipoReactivo.Estandarizado:
                    OpcionRespuestaPlantillaRetHlp helper = new OpcionRespuestaPlantillaRetHlp();
                    ds = helper.Action(dctx, opcionRespuestaPlantilla, respuestaPlantilla);
                    break;
                case ETipoReactivo.ModeloGenerico:
                    if (!(opcionRespuestaPlantilla is OpcionRespuestaModeloGenerico)) throw new Exception("El parámetro opcionRespuestaPlantilla tiene que ser de tipo OpcionRespuestaModeloGenerico.");
                    if (!(respuestaPlantilla is RespuestaPlantillaOpcionMultiple)) throw new Exception("El parámetro respuestaPlantilla tiene que ser de tipo RespuestaPlantillaOpcionMultiple.");

                    OpcionRespuestaPlantillaDinamicoRetHlp helperDinamico = new OpcionRespuestaPlantillaDinamicoRetHlp();
                    ds = helperDinamico.Action(dctx, respuestaPlantilla as RespuestaPlantillaOpcionMultiple, opcionRespuestaPlantilla as OpcionRespuestaModeloGenerico);
                    break;
            }
            return ds;
        }
        /// <summary>
        /// Crea un registro de OpcionRespuestaPlantillaInsHlp en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="opcionRespuestaPlantillaInsHlp">OpcionRespuestaPlantillaInsHlp que desea crear</param>
        public void Insert(IDataContext dctx, OpcionRespuestaPlantilla opcionRespuestaPlantilla, RespuestaPlantilla respuestaPlantilla, ETipoReactivo tipoReactivo)
        {
            if (tipoReactivo == null) throw new Exception("El parámetro 'TipoReactivo' es requerido para realizar esta operación.");
            switch (tipoReactivo) 
            {
                case ETipoReactivo.Diagnostico:
                    break;
                case ETipoReactivo.Estandarizado:
                    OpcionRespuestaPlantillaInsHlp helper = new OpcionRespuestaPlantillaInsHlp();
                    helper.Action(dctx,  opcionRespuestaPlantilla, respuestaPlantilla);
                    break;
                case ETipoReactivo.ModeloGenerico:
                    if (!(opcionRespuestaPlantilla is OpcionRespuestaModeloGenerico)) throw new Exception("El parámetro opcionRespuestaPlantilla tiene que ser de tipo OpcionRespuestaModeloGenerico.");
                    if (!(respuestaPlantilla is RespuestaPlantillaOpcionMultiple)) throw new Exception("El parámetro respuestaPlantilla tiene que ser de tipo RespuestaPlantillaOpcionMultiple.");

                    OpcionRespuestaPlantillaDinamicoInsHlp helperDinamico = new OpcionRespuestaPlantillaDinamicoInsHlp();
                    helperDinamico.Action(dctx, respuestaPlantilla as RespuestaPlantillaOpcionMultiple, opcionRespuestaPlantilla as OpcionRespuestaModeloGenerico);
                    break;
            }
        }
        /// <summary>
        /// Actualiza de manera optimista un registro de OpcionRespuestaPlantillaUpdHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="opcionRespuestaPlantillaUpdHlp">OpcionRespuestaPlantillaUpdHlp que tiene los datos nuevos</param>
        /// <param name="anterior">OpcionRespuestaPlantillaUpdHlp que tiene los datos anteriores</param>
        public void Update(IDataContext dctx, OpcionRespuestaPlantilla opcionRespuestaPlantilla, OpcionRespuestaPlantilla previous, ETipoReactivo tipoReactivo)
        {
            if (tipoReactivo == null) throw new Exception("El parámetro 'TipoReactivo' es requerido para realizar esta operación.");
            switch (tipoReactivo)
            {
                case ETipoReactivo.Diagnostico:
                    break;
                case ETipoReactivo.Estandarizado:
                    OpcionRespuestaPlantillaUpdHlp helper = new OpcionRespuestaPlantillaUpdHlp();
                    helper.Action(dctx,  opcionRespuestaPlantilla, previous);
                    break;
                case ETipoReactivo.ModeloGenerico:
                    OpcionRespuestaPlantillaDinamicoUpdHlp opcionDinamicoDa = new OpcionRespuestaPlantillaDinamicoUpdHlp();
                    opcionDinamicoDa.Action(dctx, opcionRespuestaPlantilla as OpcionRespuestaModeloGenerico, previous as OpcionRespuestaModeloGenerico);

                    break;
            }

        }
        /// <summary>
        /// Elimina un registro de OpcionRespuestaPlantillaDelHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="opcionRespuestaPlantillaDelHlp">OpcionRespuestaPlantillaDelHlp que desea eliminar</param>
        public void Delete(IDataContext dctx, OpcionRespuestaPlantilla opcionRespuestaPlantilla, ETipoReactivo tipoReactivo)
        {
            if (tipoReactivo == null) throw new Exception("El parámetro 'TipoReactivo' es requerido para realizar esta operación.");
            switch (tipoReactivo)
            {
                case ETipoReactivo.Diagnostico:
                    break;
                case ETipoReactivo.Estandarizado:
                    OpcionRespuestaPlantillaDelHlp helper = new OpcionRespuestaPlantillaDelHlp();
                    helper.Action(dctx,  opcionRespuestaPlantilla);
                    break;
                case ETipoReactivo.ModeloGenerico:
                    OpcionRespuestaPlantillaDinamicoDelHlp helperdinamico = new OpcionRespuestaPlantillaDinamicoDelHlp();
                    helperdinamico.Action(dctx, opcionRespuestaPlantilla as OpcionRespuestaModeloGenerico);

                    break;
            }
        }
        /// <summary>
        /// Crea un objeto de OpcionRespuestaPlantilla a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de OpcionRespuestaPlantilla</param>
        /// <returns>Un objeto de OpcionRespuestaPlantilla creado a partir de los datos</returns>
        public OpcionRespuestaPlantilla LastDataRowToOpcionRespuestaPlantilla(DataSet ds, ETipoReactivo tipoReactivo) {
            if (!ds.Tables.Contains("OpcionRespuestaPlantilla"))
            throw new Exception("LastDataRowToOpcionRespuestaPlantilla: DataSet no tiene la tabla OpcionRespuestaPlantilla");
            int index = ds.Tables["OpcionRespuestaPlantilla"].Rows.Count;
            if (index < 1)
            throw new Exception("LastDataRowToOpcionRespuestaPlantilla: El DataSet no tiene filas");
            return this.DataRowToOpcionRespuestaPlantilla(ds.Tables["OpcionRespuestaPlantilla"].Rows[index - 1], tipoReactivo);
        }
        /// <summary>
        /// Crea un objeto de OpcionRespuestaPlantilla a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de OpcionRespuestaPlantilla</param>
        /// <returns>Un objeto de OpcionRespuestaPlantilla creado a partir de los datos</returns>
        public OpcionRespuestaPlantilla DataRowToOpcionRespuestaPlantilla(DataRow row, ETipoReactivo tipoReactivo)
        {
            OpcionRespuestaPlantilla opcionRespuestaPlantilla = null;
            if (tipoReactivo == ETipoReactivo.ModeloGenerico)
                opcionRespuestaPlantilla = new OpcionRespuestaModeloGenerico();
            else
                opcionRespuestaPlantilla = new OpcionRespuestaPlantilla();
            if (row.IsNull("OpcionRespuestaPlantillaID"))
            opcionRespuestaPlantilla.OpcionRespuestaPlantillaID = null;
            else
            opcionRespuestaPlantilla.OpcionRespuestaPlantillaID = (int)Convert.ChangeType(row["OpcionRespuestaPlantillaID"], typeof(int));
            if (row.IsNull("Texto"))
            opcionRespuestaPlantilla.Texto = null;
            else
            opcionRespuestaPlantilla.Texto = (string)Convert.ChangeType(row["Texto"], typeof(string));
            if (row.IsNull("ImagenUrl"))
            opcionRespuestaPlantilla.ImagenUrl = null;
            else
            opcionRespuestaPlantilla.ImagenUrl = (string)Convert.ChangeType(row["ImagenUrl"], typeof(string));
            if (row.IsNull("EsPredeterminado"))
            opcionRespuestaPlantilla.EsPredeterminado = null;
            else
            opcionRespuestaPlantilla.EsPredeterminado = (bool)Convert.ChangeType(row["EsPredeterminado"], typeof(bool));
            if (row.IsNull("EsOpcionCorrecta"))
            opcionRespuestaPlantilla.EsOpcionCorrecta = null;
            else
            opcionRespuestaPlantilla.EsOpcionCorrecta = (bool)Convert.ChangeType(row["EsOpcionCorrecta"], typeof(bool));

            if (row.IsNull("EsInteres"))
                opcionRespuestaPlantilla.EsInteres = null;
            else
                opcionRespuestaPlantilla.EsInteres = (bool)Convert.ChangeType(row["EsInteres"], typeof(bool));

            if (row.Table.Columns.Contains("Activo"))
            {
                if (row.IsNull("Activo"))
                    opcionRespuestaPlantilla.Activo = null;
                else
                    opcionRespuestaPlantilla.Activo = (bool)Convert.ChangeType(row["Activo"], typeof(bool));
            }
            if (row.Table.Columns.Contains("PorcentajeCalificacion"))
            {
                if (row.IsNull("PorcentajeCalificacion"))
                    opcionRespuestaPlantilla.PorcentajeCalificacion = null;
                else
                    opcionRespuestaPlantilla.PorcentajeCalificacion = Convert.ToDecimal(row["PorcentajeCalificacion"]);
            }

            if (opcionRespuestaPlantilla is OpcionRespuestaModeloGenerico)
            {
                if (row.IsNull("ModeloID"))
                    (opcionRespuestaPlantilla as OpcionRespuestaModeloGenerico).Modelo = new ModeloDinamico() { ModeloID = null };
                else
                    (opcionRespuestaPlantilla as OpcionRespuestaModeloGenerico).Modelo = new ModeloDinamico() { ModeloID = Convert.ToInt32(row["ModeloID"]) };
                if (row.IsNull("ClasificadorID"))
                    (opcionRespuestaPlantilla as OpcionRespuestaModeloGenerico).Clasificador = new Clasificador() { ClasificadorID = null };
                else
                    (opcionRespuestaPlantilla as OpcionRespuestaModeloGenerico).Clasificador = new Clasificador() { ClasificadorID = Convert.ToInt32(row["ClasificadorID"]) };
            }
            return opcionRespuestaPlantilla;
        }
   } 
}
