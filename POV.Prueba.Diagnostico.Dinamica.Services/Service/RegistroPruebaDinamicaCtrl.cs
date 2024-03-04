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
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Prueba.BO;

namespace POV.Prueba.Diagnostico.Dinamica.Service
{
    /// <summary>
    /// Controlador del objeto RegistroPruebaDinamica
    /// </summary>
    public class RegistroPruebaDinamicaCtrl
    {
        /// <summary>
        /// Consulta registros de RegistroPruebaDinamicaRetHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="registroPruebaDinamicaRetHlp">RegistroPruebaDinamicaRetHlp que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de RegistroPruebaDinamicaRetHlp generada por la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, ResultadoPruebaDinamica resultadoPrueba, RegistroPruebaDinamica registroPrueba)
        {
            RegistroPruebaDinamicaRetHlp da = new RegistroPruebaDinamicaRetHlp();
            DataSet ds = da.Action(dctx, resultadoPrueba, registroPrueba);
            return ds;
        }
        /// <summary>
        /// Consulta un registro de prueba dinamica a partir de los filtros proporcionados
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos (Requerido)</param>
        /// <param name="resultadoPrueba">Resultado Prueba que se usará como filtro</param>
        /// <param name="registroPrueba">Registro de prueba que se usará como filtro</param>
        /// <returns></returns>
        public RegistroPruebaDinamica RetrieveComplete(IDataContext dctx, ResultadoPruebaDinamica resultadoPrueba, RegistroPruebaDinamica registroPrueba)
        {
            if (resultadoPrueba == null) throw new ArgumentNullException("RespuestaReactivoDinamicaCtrl: ResultadoPruebaDinamica no puede ser nulo");
            if (registroPrueba == null) throw new ArgumentNullException("RespuestaReactivoDinamicaCtrl: RegistroPruebaDinamica no puede ser nulo");

            DataSet dsRegistroPrueba = Retrieve(dctx, resultadoPrueba, registroPrueba);
            RegistroPruebaDinamica registroComplete = null;

            if (dsRegistroPrueba.Tables[0].Rows.Count > 0)
            {
                AlumnoCtrl alumnoCtrl = new AlumnoCtrl();
                RespuestaReactivoDinamicaCtrl respuestaReactivoCtrl = new RespuestaReactivoDinamicaCtrl();

                registroComplete  = LastDataRowToRegistroPruebaDinamica(dsRegistroPrueba);
                registroComplete.Alumno = alumnoCtrl.LastDataRowToAlumno(alumnoCtrl.Retrieve(dctx, registroComplete.Alumno));
                registroComplete.ListaRespuestaReactivos = respuestaReactivoCtrl.RetrieveListRespuestaReactivo(dctx, new RegistroPruebaDinamica { RegistroPruebaID = registroComplete.RegistroPruebaID });
            }

            return registroComplete;
        }
        /// <summary>
        /// Crea un registro de RegistroPruebaDinamicaInsHlp en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="registroPruebaDinamicaInsHlp">RegistroPruebaDinamicaInsHlp que desea crear</param>
        public void Insert(IDataContext dctx, ResultadoPruebaDinamica resultadoPrueba, RegistroPruebaDinamica registroPrueba)
        {
            RegistroPruebaDinamicaInsHlp da = new RegistroPruebaDinamicaInsHlp();
            da.Action(dctx, resultadoPrueba, registroPrueba);
        }
        /// <summary>
        /// Inserta un registro de prueba dinamica en la base de datos del sistema
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="resultadoPrueba">Resultado prueba dinamico relacionado (Requerido)</param>
        /// <param name="registroPrueba">registro de prueba que se desea insertar (Requerido)</param>
        public void InsertComplete(IDataContext dctx, ResultadoPruebaDinamica resultadoPrueba, RegistroPruebaDinamica registroPrueba)
        {
            if (resultadoPrueba == null) throw new ArgumentNullException("RegistroPruebaDinamicaCtrl: ResultadoPruebaDinamica no puede ser nulo ");
            if (resultadoPrueba.ResultadoPruebaID == null) throw new ArgumentNullException("RegistroPruebaDinamicaCtrl: ResultadoPruebaID no puede ser nulo ");
            if (registroPrueba == null) throw new ArgumentNullException("RegistroPruebaDinamicaCtrl: RegistroPruebaDinamica no puede ser nulo ");
            if (registroPrueba.ListaRespuestaReactivos == null) throw new ArgumentNullException("RegistroPruebaDinamicaCtrl: ListaRespuestaReactivos no puede ser nulo ");
            if (registroPrueba.ListaRespuestaReactivos.Count == 0) throw new ArgumentNullException("RegistroPruebaDinamicaCtrl: ListaRespuestaReactivos no puede ser vacio ");
            


            #region *** begin transaction ***
            object myFirm = new object();
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            try
            {
            #endregion

                Insert(dctx, resultadoPrueba, registroPrueba);
                registroPrueba.RegistroPruebaID =  LastDataRowToRegistroPruebaDinamica(Retrieve(dctx, resultadoPrueba, registroPrueba)).RegistroPruebaID;

                RespuestaReactivoDinamicaCtrl respuestaReactivoCtrl = new RespuestaReactivoDinamicaCtrl();
                foreach (ARespuestaReactivo respuestaReactivo in registroPrueba.ListaRespuestaReactivos)
                {
                    respuestaReactivoCtrl.InsertComplete(dctx, registroPrueba, respuestaReactivo as RespuestaReactivoDinamica);
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
        /// Actualiza de manera optimista un registro de RegistroPruebaDinamicaUpdHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="registroPruebaDinamicaUpdHlp">RegistroPruebaDinamicaUpdHlp que tiene los datos nuevos</param>
        /// <param name="anterior">RegistroPruebaDinamicaUpdHlp que tiene los datos anteriores</param>
        public void Update(IDataContext dctx, RegistroPruebaDinamica registroPrueba, RegistroPruebaDinamica previous)
        {
            RegistroPruebaDinamicaUpdHlp da = new RegistroPruebaDinamicaUpdHlp();
            da.Action(dctx, registroPrueba, previous);
        }
        /// <summary>
        /// Actualiza un registro completo de registro de prueba en la base de datos del sistema
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="registroPrueba">Registro de prueba que se desea actualizar</param>
        /// <param name="previous">Registro previo</param>


        /// <summary>
        /// Crea un objeto de RegistroPruebaDinamica a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de RegistroPruebaDinamica</param>
        /// <returns>Un objeto de RegistroPruebaDinamica creado a partir de los datos</returns>
        public SumarioGeneralSacks LastDataRowToSumarioGeneralSacks(DataSet ds)
        {
            if (!ds.Tables.Contains("SumarioGeneralSacks"))
                throw new Exception("LastDataRowToSumarioGeneralSacks: DataSet no tiene la tabla SumarioGeneralSacks");
            int index = ds.Tables["SumarioGeneralSacks"].Rows.Count;
            if (index < 1)
                return null;
            return this.DataRowToSumarioGeneralSacks(ds.Tables["SumarioGeneralSacks"].Rows[index - 1]);
        }
        /// <summary>
        /// Crea un objeto de SumarioGeneralSacks a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de SumarioGeneralSacks</param>
        /// <returns>Un objeto de SumarioGeneralSacks creado a partir de los datos</returns>
        public SumarioGeneralSacks DataRowToSumarioGeneralSacks (DataRow row)
        {
            SumarioGeneralSacks obj = new SumarioGeneralSacks();
            obj.Prueba = new PruebaDinamica();
            obj.Alumno = new Alumno();
            if (row.IsNull("SumarioGeneralSACKSID"))
                obj.SumarioGeneralSACKSID = null;
            else
                obj.SumarioGeneralSACKSID = (int)Convert.ChangeType(row["SumarioGeneralSACKSID"], typeof(int));

            if (row.IsNull("PruebaID"))
                obj.Prueba.PruebaID = null;
            else
                obj.Prueba.PruebaID = (int)Convert.ChangeType(row["PruebaID"], typeof(int));

            if (row.IsNull("AlumnoID"))
                obj.Alumno.AlumnoID = null;
            else
                obj.Alumno.AlumnoID = (long)Convert.ChangeType(row["AlumnoID"], typeof(long));

            if (row.IsNull("SumarioMadurez"))
                obj.SumarioMadurez = null;
            else
                obj.SumarioMadurez = (string)Convert.ChangeType(row["SumarioMadurez"], typeof(string));

            if (row.IsNull("SumarioNivelRealidad"))
                obj.SumarioNivelRealida = null;
            else
                obj.SumarioNivelRealida = (string)Convert.ChangeType(row["SumarioNivelRealidad"], typeof(string));

            if (row.IsNull("SumarioConflictosExpresados"))
                obj.SumarioConflictoExpresados = null;
            else
                obj.SumarioConflictoExpresados = (string)Convert.ChangeType(row["SumarioConflictosExpresados"], typeof(string));

            if (row.IsNull("FechaRegistro"))
                obj.FechaRegistro = null;
            else
                obj.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
            return obj;
        }

        /// <summary>
        /// Crea un objeto de RegistroPruebaDinamica a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de RegistroPruebaDinamica</param>
        /// <returns>Un objeto de RegistroPruebaDinamica creado a partir de los datos</returns>
        public SumarioGeneralFrasesVocacionales LastDataRowToSumarioGeneralFrasesVocacionales(DataSet ds)
        {
            if (!ds.Tables.Contains("SumarioGeneralFrasesVocacionales"))
                throw new Exception("LastDataRowToSumarioGeneralFrasesVocacionales: DataSet no tiene la tabla SumarioGeneralFrasesVocacionales");
            int index = ds.Tables["SumarioGeneralFrasesVocacionales"].Rows.Count;
            if (index < 1)
                return null;
            return this.DataRowToSumarioGeneralFrasesVocacionales(ds.Tables["SumarioGeneralFrasesVocacionales"].Rows[index - 1]);
        }
        /// <summary>
        /// Crea un objeto de SumarioGeneralFrasesVocacionales a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de SumarioGeneralSacks</param>
        /// <returns>Un objeto de SumarioGeneralFrasesVocacionales creado a partir de los datos</returns>
        public SumarioGeneralFrasesVocacionales DataRowToSumarioGeneralFrasesVocacionales(DataRow row)
        {
            SumarioGeneralFrasesVocacionales obj = new SumarioGeneralFrasesVocacionales();
            obj.Prueba = new PruebaDinamica();
            obj.Alumno = new Alumno();
            if (row.IsNull("SumarioGeneralFrasesID"))
                obj.SumarioGeneralFrasesID = null;
            else
                obj.SumarioGeneralFrasesID = (int)Convert.ChangeType(row["SumarioGeneralFrasesID"], typeof(int));

            if (row.IsNull("PruebaID"))
                obj.Prueba.PruebaID = null;
            else
                obj.Prueba.PruebaID = (int)Convert.ChangeType(row["PruebaID"], typeof(int));

            if (row.IsNull("AlumnoID"))
                obj.Alumno.AlumnoID = null;
            else
                obj.Alumno.AlumnoID = (long)Convert.ChangeType(row["AlumnoID"], typeof(long));

            if (row.IsNull("SumarioOrganizacionPersonalidad"))
                obj.SumarioOrganizacionPersonalidad = null;
            else
                obj.SumarioOrganizacionPersonalidad = (string)Convert.ChangeType(row["SumarioOrganizacionPersonalidad"], typeof(string));

            if (row.IsNull("SumarioPerspectivaOpciones"))
                obj.SumarioPerspectivaOpciones = null;
            else
                obj.SumarioPerspectivaOpciones = (string)Convert.ChangeType(row["SumarioPerspectivaOpciones"], typeof(string));

            if (row.IsNull("SumarioFuentesConflicto"))
                obj.SumarioFuentesConflicto = null;
            else
                obj.SumarioFuentesConflicto = (string)Convert.ChangeType(row["SumarioFuentesConflicto"], typeof(string));

            if (row.IsNull("FechaRegistro"))
                obj.FechaRegistro = null;
            else
                obj.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
            return obj;
        }

        /// <summary>
        /// Crea un registro de RegistroPruebaDinamicaInsHlp en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="registroPruebaDinamicaInsHlp">RegistroPruebaDinamicaInsHlp que desea crear</param>
        public void Insert(IDataContext dctx, SumarioGeneralSacks sumarioGeneralSacks)
        {
            SumarioGeneralSACKInsHlp da = new SumarioGeneralSACKInsHlp();
            da.Action(dctx, sumarioGeneralSacks);
        }
        /// <summary>
        /// Consulta registros de RegistroPruebaDinamicaRetHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="registroPruebaDinamicaRetHlp">SumarioGeneralSacksRetHlp que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de SumarioGeneralSacks generada por la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, SumarioGeneralSacks sumarioGeneralSacks)
        {
            SumarioGeneralSACKSRetHlp da = new SumarioGeneralSACKSRetHlp();
            DataSet ds = da.Action(dctx, sumarioGeneralSacks);
            return ds;
        }

        /// <summary>
        /// Crea un registro de RegistroPruebaDinamicaInsHlp en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="registroPruebaDinamicaInsHlp">RegistroPruebaDinamicaInsHlp que desea crear</param>
        public void Insert(IDataContext dctx, SumarioGeneralFrasesVocacionales sumarioGeneralFrasesVocacionales)
        {
            SumarioGeneralFrasesVocacionalesInsHlp da = new SumarioGeneralFrasesVocacionalesInsHlp();
            da.Action(dctx, sumarioGeneralFrasesVocacionales);
        }
        /// <summary>
        /// Consulta registros de RegistroPruebaDinamicaRetHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="registroPruebaDinamicaRetHlp">SumarioGeneralFrasesVocacionalesRetHlp que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de SumarioGeneralFrasesVocacionales generada por la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, SumarioGeneralFrasesVocacionales sumarioGeneralFrasesVocacionales)
        {
            SumarioGeneralFrasesVocacionalesRetHlp da = new SumarioGeneralFrasesVocacionalesRetHlp();
            DataSet ds = da.Action(dctx, sumarioGeneralFrasesVocacionales);
            return ds;
        }

        /// <summary>
        /// Actualiza un registro completo de registro de prueba en la base de datos del sistema
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="registroPrueba">Registro de prueba que se desea actualizar</param>
        /// <param name="previous">Registro previo</param>
        

        /// <summary>
        /// Crea un objeto de RegistroPruebaDinamica a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de RegistroPruebaDinamica</param>
        /// <returns>Un objeto de RegistroPruebaDinamica creado a partir de los datos</returns>
        public RegistroPruebaDinamica LastDataRowToRegistroPruebaDinamica(DataSet ds)
        {
            if (!ds.Tables.Contains("RegistroPrueba"))
                throw new Exception("LastDataRowToRegistroPruebaDinamica: DataSet no tiene la tabla RegistroPruebaDinamica");
            int index = ds.Tables["RegistroPrueba"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToRegistroPruebaDinamica: El DataSet no tiene filas");
            return this.DataRowToRegistroPruebaDinamica(ds.Tables["RegistroPrueba"].Rows[index - 1]);
        }
        /// <summary>
        /// Crea un objeto de RegistroPruebaDinamica a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de RegistroPruebaDinamica</param>
        /// <returns>Un objeto de RegistroPruebaDinamica creado a partir de los datos</returns>
        public RegistroPruebaDinamica DataRowToRegistroPruebaDinamica(DataRow row)
        {
            RegistroPruebaDinamica registroPruebaDinamica = new RegistroPruebaDinamica();
            registroPruebaDinamica.Alumno = new Alumno();
            if (row.IsNull("RegistroPruebaID"))
                registroPruebaDinamica.RegistroPruebaID = null;
            else
                registroPruebaDinamica.RegistroPruebaID = (int)Convert.ChangeType(row["RegistroPruebaID"], typeof(int));
            if (row.IsNull("AlumnoID"))
                registroPruebaDinamica.Alumno.AlumnoID = null;
            else
                registroPruebaDinamica.Alumno.AlumnoID = (long)Convert.ChangeType(row["AlumnoID"], typeof(long));
            if (row.IsNull("EstadoPrueba"))
                registroPruebaDinamica.EstadoPrueba = null;
            else
                registroPruebaDinamica.EstadoPrueba = (EEstadoPrueba)(byte)Convert.ChangeType(row["EstadoPrueba"], typeof(byte));
            if (row.IsNull("FechaInicio"))
                registroPruebaDinamica.FechaInicio = null;
            else
                registroPruebaDinamica.FechaInicio = (DateTime)Convert.ChangeType(row["FechaInicio"], typeof(DateTime));
            if (row.IsNull("FechaFin"))
                registroPruebaDinamica.FechaFin = null;
            else
                registroPruebaDinamica.FechaFin = (DateTime)Convert.ChangeType(row["FechaFin"], typeof(DateTime));
            if (row.IsNull("FechaRegistro"))
                registroPruebaDinamica.FechaRegistro = null;
            else
                registroPruebaDinamica.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
            return registroPruebaDinamica;
        }
    }
}
