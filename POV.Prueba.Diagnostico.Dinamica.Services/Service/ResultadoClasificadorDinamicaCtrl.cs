using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Prueba.Diagnostico.Dinamica.DAO;
using POV.Expediente.BO;
using POV.Expediente.Service;
using POV.Modelo.BO;
using POV.Modelo.Service;

namespace POV.Prueba.Diagnostico.Dinamica.Service
{
    /// <summary>
    /// Controlador del objeto ResultadoClasificadorDinamica
    /// </summary>
    public class ResultadoClasificadorDinamicaCtrl : AResultadoClasificadorCtrl
    {
        /// <summary>
        /// Consulta registros de ResultadoClasificadorDinamicaRetHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="resultadoClasificadorDinamicaRetHlp">ResultadoClasificadorDinamicaRetHlp que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de ResultadoClasificadorDinamicaRetHlp generada por la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, ResultadoClasificadorDinamica resultadoClasificadorDinamica)
        {
            ResultadoClasificadorDinamicaRetHlp da = new ResultadoClasificadorDinamicaRetHlp();
            DataSet ds = da.Action(dctx, resultadoClasificadorDinamica);
            return ds;
        }
        /// <summary>
        /// Crea un registro de ResultadoClasificadorDinamicaInsHlp en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="resultadoClasificadorDinamicaInsHlp">ResultadoClasificadorDinamicaInsHlp que desea crear</param>
        public void Insert(IDataContext dctx, ResultadoClasificadorDinamica resultadoClasificadorDinamica)
        {
            ResultadoClasificadorDinamicaInsHlp da = new ResultadoClasificadorDinamicaInsHlp();
            da.Action(dctx, resultadoClasificadorDinamica);
        }

        /// <summary>
        /// Consulta completa de ResultadoClasificadorDinamica
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="resultadoClasificador">Provee el criterio de selección para realizar la consulta</param>
        /// <returns>ResultadoClasificadorDinamica Completa</returns>
        public override AResultadoClasificador RetrieveComplete(IDataContext dctx, AResultadoClasificador resultadoClasificador)
        {
            if (resultadoClasificador == null)
                throw new ArgumentNullException("resultadoClasificador no puede ser nulo");
            if (!(resultadoClasificador is ResultadoClasificadorDinamica))
                throw new ArgumentException("resultadoClasificador debe ser tipo dinamica");

            AResultadoClasificador resultadoComplete = null;

            DataSet dsResultadoClasificadorDinamica = this.Retrieve(dctx, resultadoClasificador as ResultadoClasificadorDinamica);
            if (dsResultadoClasificadorDinamica.Tables[0].Rows.Count > 0)
            {
                int index = dsResultadoClasificadorDinamica.Tables[0].Rows.Count;
                DataRow drResultadoClasificadorDinamica = dsResultadoClasificadorDinamica.Tables[0].Rows[index - 1];
                resultadoComplete = DataRowToResultadoClasificador(drResultadoClasificadorDinamica);

                //Obtenemos el modelo
                ModeloCtrl modeloCtrl = new ModeloCtrl();
                resultadoComplete.Modelo = modeloCtrl.RetrieveComplete(dctx, resultadoComplete.Modelo);

                //Obtenemos el clasificador
                (resultadoComplete as ResultadoClasificadorDinamica).Clasificador = modeloCtrl.LastDataRowToClasificador(modeloCtrl.RetrieveClasificador(dctx, (resultadoComplete as ResultadoClasificadorDinamica).Clasificador, resultadoComplete.Modelo as ModeloDinamico));

                //Obtenemos el resultado de la prueba
                ResultadoPruebaDinamicaCtrl resultadoPruebaDinamicaCtrl = new ResultadoPruebaDinamicaCtrl();
                resultadoComplete.ResultadoPrueba = resultadoPruebaDinamicaCtrl.RetrieveComplete(dctx, new DetalleCicloEscolar(), resultadoComplete.ResultadoPrueba as ResultadoPruebaDinamica);
            }
            return resultadoComplete;
        }

        /// <summary>
        /// Crea un registro de ResultadoClasificadorDinamica Completo en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="resultadoClasificador">resultadoClasificador que desea crear</param>
        public override void InsertComplete(IDataContext dctx, AResultadoClasificador resultadoClasificador)
        {
            #region *** Validaciones ***
            if (resultadoClasificador == null) throw new ArgumentNullException("ResultadoClasificador no puede ser nulo");
            if (!(resultadoClasificador is ResultadoClasificadorDinamica)) throw new ArgumentException("ResultadoClasificador debe ser tipo dinamica");
            if (resultadoClasificador.ResultadoPrueba == null) throw new ArgumentNullException("Resultado prueba no puede ser nulo");
            if (!(resultadoClasificador.ResultadoPrueba is ResultadoPruebaDinamica)) throw new ArgumentException("Resultado prueba debe ser tipo dinamica");
            if (resultadoClasificador.ResultadoPrueba.ResultadoPruebaID == null) throw new ArgumentNullException("El identificador de resultado prueba no puede ser nulo");
            if (resultadoClasificador.Modelo == null) throw new ArgumentNullException("Modelo no puede ser nulo");
            if (!(resultadoClasificador.Modelo is ModeloDinamico)) throw new ArgumentException("Modelo de ser tipo dinamica");
            if (resultadoClasificador.Modelo.ModeloID == null) throw new ArgumentNullException("El identificador del modelo no puede ser nulo");
            if ((resultadoClasificador as ResultadoClasificadorDinamica).Clasificador == null) throw new ArgumentNullException("Clasificador no puede ser nulo");
            if ((resultadoClasificador as ResultadoClasificadorDinamica).Clasificador.ClasificadorID == null) throw new ArgumentNullException("El identificador del clasificador no puede ser nulo");
            #endregion

            object myFirm = new object();
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);

            try
            {
                //insertamos el resultado clasificador
                this.InsertAResultadoClasificador(dctx, resultadoClasificador);

                //recuperamos en identificador del resultado clasificador padre
                DataSet dsResultadoClasificador = this.RetrieveAResultadoClasificador(dctx, resultadoClasificador);
                resultadoClasificador.ResultadoClasificadorID = dsResultadoClasificador.Tables[0].Rows[dsResultadoClasificador.Tables[0].Rows.Count - 1].Field<long>("ResultadoClasificadorID");

                //insertamos el resultado clasificador dinamica
                this.Insert(dctx, resultadoClasificador as ResultadoClasificadorDinamica);

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

        public override void UpdateComplete(IDataContext dctx, AResultadoClasificador resultadoClasificador, AResultadoClasificador previous)
        {
            throw new NotImplementedException();
        }

        public override void DeleteComplete(IDataContext dctx, AResultadoClasificador resultadoClasificador)
        {
            throw new NotImplementedException();
        }

        public override AResultadoClasificador LastDataRowToResultadoClasificador(DataSet ds)
        {
            if (!ds.Tables.Contains("ResultadoClasificador"))
                throw new Exception("LastDataRowToAResultadoClasificador: DataSet no tiene la tabla ResultadoClasificador");
            int index = ds.Tables[0].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToResultadoClasificador: El DataSet no tiene filas");
            return this.DataRowToResultadoClasificador(ds.Tables[0].Rows[index - 1]);
        }

        public override AResultadoClasificador DataRowToResultadoClasificador(DataRow dr)
        {
            AResultadoClasificador resultadoClasificador = new ResultadoClasificadorDinamica();
            resultadoClasificador.ResultadoPrueba = new ResultadoPruebaDinamica();
            resultadoClasificador.Modelo = new ModeloDinamico();
            (resultadoClasificador as ResultadoClasificadorDinamica).Clasificador = new Clasificador();

            if (dr.IsNull("ResultadoClasificadorID"))
                resultadoClasificador.ResultadoClasificadorID = null;
            else
                resultadoClasificador.ResultadoClasificadorID = (long)Convert.ChangeType(dr["ResultadoClasificadorID"], typeof(long));
            if (dr.IsNull("FechaRegistro"))
                resultadoClasificador.FechaRegistro = null;
            else
                resultadoClasificador.FechaRegistro = (DateTime)Convert.ChangeType(dr["FechaRegistro"], typeof(DateTime));
            if (dr.IsNull("ModeloID"))
                resultadoClasificador.Modelo.ModeloID = null;
            else
                resultadoClasificador.Modelo.ModeloID = (int)Convert.ChangeType(dr["ModeloID"], typeof(int));
            if (dr.IsNull("ResultadoPruebaID"))
                resultadoClasificador.ResultadoPrueba.ResultadoPruebaID = null;
            else
                resultadoClasificador.ResultadoPrueba.ResultadoPruebaID = (int)Convert.ChangeType(dr["ResultadoPruebaID"], typeof(int));
            if (dr.IsNull("ClasificadorID"))
                (resultadoClasificador as ResultadoClasificadorDinamica).Clasificador.ClasificadorID = null;
            else
                (resultadoClasificador as ResultadoClasificadorDinamica).Clasificador.ClasificadorID = (int)Convert.ChangeType(dr["ClasificadorID"], typeof(int));
            return resultadoClasificador;
        }
    }
}
