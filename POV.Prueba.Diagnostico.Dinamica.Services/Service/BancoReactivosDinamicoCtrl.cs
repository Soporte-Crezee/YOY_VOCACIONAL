using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Linq;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Prueba.BO;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Prueba.Diagnostico.Dinamica.DAO;
using POV.Reactivos.Service;
using POV.Reactivos.BO;

namespace POV.Prueba.Diagnostico.Dinamica.Service
{
    /// <summary>
    /// Controlador del objeto BancoReactivosDinamico
    /// </summary>
    public class BancoReactivosDinamicoCtrl
    {
        /// <summary>
        /// Consulta registros de BancoReactivosDinamicoRetHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="bancoReactivosDinamicoRetHlp">BancoReactivosDinamicoRetHlp que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de BancoReactivosDinamicoRetHlp generada por la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, BancoReactivosDinamico bancoReactivosDinamico)
        {
            BancoReactivosDinamicoRetHlp da = new BancoReactivosDinamicoRetHlp();
            DataSet ds = da.Action(dctx, bancoReactivosDinamico);
            return ds;
        }

        /// <summary>
        /// Consulta un registro completo de BancoRactivosDinamico
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="bancoReactivosDinamico">Provee el criterio de selección para la realizar la consulta</param>
        /// <returns>BancoRactivosDinamico Completo</returns>
        public BancoReactivosDinamico RetrieveComplete(IDataContext dctx, BancoReactivosDinamico bancoReactivosDinamico)
        {
            if (bancoReactivosDinamico == null)
                throw new Exception("bancoReactivosDinamico no puede ser nulo");

            BancoReactivosDinamico bancoReactivosDinamicoComplete = null;

            DataSet dsBancoReactivosDinamico = Retrieve(dctx, bancoReactivosDinamico);

            if (dsBancoReactivosDinamico.Tables[0].Rows.Count > 0)
            {
                int index = dsBancoReactivosDinamico.Tables[0].Rows.Count;
                DataRow drBancoReactivosDinamico = dsBancoReactivosDinamico.Tables[0].Rows[index - 1];
                bancoReactivosDinamicoComplete = DataRowToBancoReactivosDinamico(drBancoReactivosDinamico);
                bancoReactivosDinamicoComplete.Prueba.TipoPruebaPresentacion = bancoReactivosDinamico.Prueba.TipoPruebaPresentacion;
                //Obtenemos la prueba
                PruebaDinamicaCtrl pruebaDinamicaCtrl = new PruebaDinamicaCtrl();
                bancoReactivosDinamicoComplete.Prueba = pruebaDinamicaCtrl.RetrieveComplete(dctx, bancoReactivosDinamicoComplete.Prueba as PruebaDinamica, true);

                //Obtenemos la lista ReactivosBancoDinamico
                bancoReactivosDinamicoComplete.ListaReactivosBanco = RetrieveListaReactivosBancoDinamico(dctx, bancoReactivosDinamicoComplete);
            }
            return bancoReactivosDinamicoComplete;
        }

        /// <summary>
        /// Actualiza un BancoReactivos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="bancoReactivos">El BancoReactivos para actualizar</param>
        /// <param name="previous">El BancoReactivos previo</param>
        public void Update(IDataContext dctx, BancoReactivosDinamico bancoReactivos, BancoReactivosDinamico previous)
        {
            BancoReactivosDinamicoUpdHlp da = new BancoReactivosDinamicoUpdHlp();
            da.Action(dctx, bancoReactivos, previous);
        }


        /// <summary>
        /// Actualiza un BancoReactivos y sus ReactivosBanco
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="bancoReactivos">El BancoReactivos para actualizar</param>
        /// <param name="previous">El BancoReactivos previo</param>
        public void UpdateComplete(IDataContext dctx, BancoReactivosDinamico bancoReactivos, BancoReactivosDinamico previous)
        {
            object firm = new object();
            try
            {
                dctx.OpenConnection(firm);
                dctx.BeginTransaction(firm);
                Update(dctx, bancoReactivos, previous);

                UpdateReactivosBancoReactivos(dctx,bancoReactivos);
                dctx.CommitTransaction(firm);
            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(firm);
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(firm);
                throw;
            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(firm);
            }
        }

        /// <summary>
        /// Registra un BancoReactivos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos </param>
        /// <param name="bancoReactivos">El BancoReactivos que se desea registrar</param>
        public void Insert(IDataContext dctx, BancoReactivosDinamico bancoReactivos)
        {
            BancoReactivosDinamicoInsHlp da = new BancoReactivosDinamicoInsHlp();
            da.Action(dctx, bancoReactivos);
        }

       
        /// <summary>
        /// Crea un objeto de BancoReactivosDinamico a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de BancoReactivosDinamico</param>
        /// <returns>Un objeto de BancoReactivosDinamico creado a partir de los datos</returns>
        public BancoReactivosDinamico LastDataRowToBancoReactivosDinamico(DataSet ds)
        {
            if (!ds.Tables.Contains("BancoReactivosDinamico"))
                throw new Exception("LastDataRowToBancoReactivosDinamico: DataSet no tiene la tabla BancoReactivosDinamico");
            int index = ds.Tables["BancoReactivosDinamico"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToBancoReactivosDinamico: El DataSet no tiene filas");
            return this.DataRowToBancoReactivosDinamico(ds.Tables["BancoReactivosDinamico"].Rows[index - 1]);
        }
        /// <summary>
        /// Crea un objeto de BancoReactivosDinamico a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de BancoReactivosDinamico</param>
        /// <returns>Un objeto de BancoReactivosDinamico creado a partir de los datos</returns>
        public BancoReactivosDinamico DataRowToBancoReactivosDinamico(DataRow row)
        {
            BancoReactivosDinamico bancoReactivosDinamico = new BancoReactivosDinamico();
            bancoReactivosDinamico.Prueba = new PruebaDinamica();
            bancoReactivosDinamico.ListaReactivosBanco = new List<ReactivoBanco>();
            if (row.IsNull("BancoReactivoID"))
                bancoReactivosDinamico.BancoReactivoID = null;
            else
                bancoReactivosDinamico.BancoReactivoID = (int)Convert.ChangeType(row["BancoReactivoID"], typeof(int));
            if (row.IsNull("PruebaID"))
                bancoReactivosDinamico.Prueba.PruebaID = null;
            else
                bancoReactivosDinamico.Prueba.PruebaID = (int)Convert.ChangeType(row["PruebaID"], typeof(int));
            if (row.IsNull("NumeroReactivos"))
                bancoReactivosDinamico.NumeroReactivos = null;
            else
                bancoReactivosDinamico.NumeroReactivos = (int)Convert.ChangeType(row["NumeroReactivos"], typeof(int));
            if (row.IsNull("FechaRegistro"))
                bancoReactivosDinamico.FechaRegistro = null;
            else
                bancoReactivosDinamico.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
            if (row.IsNull("Activo"))
                bancoReactivosDinamico.Activo = null;
            else
                bancoReactivosDinamico.Activo = (Boolean)Convert.ChangeType(row["Activo"], typeof(Boolean));
            if (row.IsNull("EsSeleccionOrdenada"))
                bancoReactivosDinamico.EsSeleccionOrdenada = null;
            else
                bancoReactivosDinamico.EsSeleccionOrdenada = (Boolean)Convert.ChangeType(row["EsSeleccionOrdenada"], typeof(Boolean));
            if (row.IsNull("TipoSeleccionBanco"))
                bancoReactivosDinamico.TipoSeleccionBanco = null;
            else
                bancoReactivosDinamico.TipoSeleccionBanco = (ETipoSeleccionBanco)(byte)Convert.ChangeType(row["TipoSeleccionBanco"], typeof(byte));
            if (row.IsNull("ReactivosPorPagina"))
                bancoReactivosDinamico.ReactivosPorPagina = null;
            else
                bancoReactivosDinamico.ReactivosPorPagina = (Int32)Convert.ChangeType(row["ReactivosPorPagina"], typeof(Int32));
            if (row.IsNull("EsPorGrupo"))
                bancoReactivosDinamico.EsPorGrupo = null;
            else
                bancoReactivosDinamico.EsPorGrupo = (Boolean)Convert.ChangeType(row["EsPorGrupo"], typeof(Boolean));
            return bancoReactivosDinamico;
        }

        #region *** ReactivosBancoDinamico ***
        /// <summary>
        /// Consulta una lista de ReactivosBancoDinamico
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="bancoReactivosDinamico">Provee el criterio de selección para realizar la consulta</param>
        /// <returns>listaReactivosBancoDinamico</returns>
        private List<ReactivoBanco> RetrieveListaReactivosBancoDinamico(IDataContext dctx, BancoReactivosDinamico bancoReactivosDinamico)
        {
            List<ReactivoBanco> listaReactivosBancoDinamico = new List<ReactivoBanco>();
            ReactivoCtrl reactivoCtrl = new ReactivoCtrl();
            if (bancoReactivosDinamico != null)
            {
                if (bancoReactivosDinamico.BancoReactivoID != null)
                {
                    DataSet dsReactivosBancoDinamico = RetrieveReactivosBancoDinamico(dctx, bancoReactivosDinamico, new ReactivoBanco { Activo = true });
                    if (dsReactivosBancoDinamico.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drReactivosBancoDinamico in dsReactivosBancoDinamico.Tables[0].Rows)
                        {
                            ReactivoBanco reactivo = DataRowToReactivosBancoDinamico(drReactivosBancoDinamico);
                            reactivo.Reactivo.TipoReactivo = ETipoReactivo.ModeloGenerico;
                            reactivo.Reactivo = reactivoCtrl.RetrieveComplete(dctx, reactivo.Reactivo);
                            listaReactivosBancoDinamico.Add(reactivo);
                        }
                    }
                }
            }
            return listaReactivosBancoDinamico;
        }

        /// <summary>
        /// Consulta registros de ReactivosBancoDinamicoRetHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="reactivosBancoDinamicoRetHlp">ReactivosBancoDinamicoRetHlp que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de ReactivosBancoDinamicoRetHlp generada por la consulta</returns>
        private DataSet RetrieveReactivosBancoDinamico(IDataContext dctx, BancoReactivosDinamico bancoReactivosDinamico, ReactivoBanco reactivoBanco)
        {
            ReactivosBancoDinamicoRetHlp da = new ReactivosBancoDinamicoRetHlp();
            DataSet ds = da.Action(dctx, bancoReactivosDinamico, reactivoBanco);
            return ds;
        }
        /// <summary>
        /// Crea un objeto de ReactivosBancoDinamico a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de ReactivosBancoDinamico</param>
        /// <returns>Un objeto de ReactivosBancoDinamico creado a partir de los datos</returns>
        private ReactivoBanco LastDataRowToReactivosBancoDinamico(DataSet ds)
        {
            if (!ds.Tables.Contains("ReactivosBancoDinamico"))
                throw new Exception("LastDataRowToReactivosBancoDinamico: DataSet no tiene la tabla ReactivosBancoReactivosDinamico");
            int index = ds.Tables["ReactivosBancoReactivosDinamico"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToReactivosBancoDinamico: El DataSet no tiene filas");
            return this.DataRowToReactivosBancoDinamico(ds.Tables["ReactivosBancoReactivosDinamico"].Rows[index - 1]);
        }
        /// <summary>
        /// Crea un objeto de ReactivosBancoDinamico a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de ReactivosBancoDinamico</param>
        /// <returns>Un objeto de ReactivosBancoDinamico creado a partir de los datos</returns>
        private ReactivoBanco DataRowToReactivosBancoDinamico(DataRow row)
        {
            ReactivoBanco reactivoBanco = new ReactivoBanco();
            reactivoBanco.Reactivo = new Reactivo { TipoReactivo = ETipoReactivo.ModeloGenerico };
            reactivoBanco.ReactivoOriginal = new Reactivo { TipoReactivo = ETipoReactivo.ModeloGenerico };
            if (row.IsNull("ReactivoBancoID"))
                reactivoBanco.ReactivoBancoID = null;
            else
                reactivoBanco.ReactivoBancoID = (long)Convert.ChangeType(row["ReactivoBancoID"], typeof(long));
            if (row.IsNull("ReactivoOriginalID"))
                reactivoBanco.ReactivoOriginal = null;
            else
                reactivoBanco.ReactivoOriginal.ReactivoID = (Guid)Convert.ChangeType(row["ReactivoOriginalID"], typeof(Guid));
            if (row.IsNull("ReactivoID"))
                reactivoBanco.Reactivo.ReactivoID = null;
            else
                reactivoBanco.Reactivo.ReactivoID = (Guid)Convert.ChangeType(row["ReactivoID"], typeof(Guid));
            if (row.IsNull("Activo"))
                reactivoBanco.Activo = null;
            else
                reactivoBanco.Activo = (Boolean)Convert.ChangeType(row["Activo"], typeof(Boolean));
            if (row.IsNull("Orden"))
                reactivoBanco.Orden = null;
            else
                reactivoBanco.Orden = (int)Convert.ChangeType(row["Orden"], typeof(int));
            if (row.IsNull("EstaSeleccionado"))
                reactivoBanco.EstaSeleccionado = null;
            else
                reactivoBanco.EstaSeleccionado = (Boolean)Convert.ChangeType(row["EstaSeleccionado"], typeof(Boolean));
            return reactivoBanco;
        }
		
		/// <summary>
		/// Actualiza la lista de ReactivoBanco en el banco de reactivos
		/// </summary>
		/// <param name="dctx"></param>
		/// <param name="bancoReactivos"></param>
		public void UpdateReactivosBancoReactivos(IDataContext dctx, BancoReactivosDinamico bancoReactivos)
		{
			PruebaDinamicaCtrl pruebaCtrl = new PruebaDinamicaCtrl();
			BancoReactivosDinamicoCtrl bancoReactivoCtrl = new BancoReactivosDinamicoCtrl();
			ReactivoCtrl reactivoCtrl = new ReactivoCtrl();

			#region ** Validaciones **
			if (bancoReactivos == null) throw new ArgumentNullException("ReactivoBancoReactivosDinamicoCtrl: bancoReactivos no puede ser nulo ");
			if (bancoReactivos.ListaReactivosBanco == null) throw new ArgumentNullException("ReactivoBancoReactivosDinamicoCtrl: ListaReactivosBanco no puede ser nulo ");
			if (bancoReactivos.ListaReactivosBanco.Count == 0) throw new ArgumentNullException("ReactivoBancoReactivosDinamicoCtrl: ListaReactivosBanco no puede estar vacío ");
			if (bancoReactivos.Prueba == null) throw new ArgumentNullException("ReactivoBancoReactivosDinamicoCtrl: Prueba no puede ser nulo ");
			if (bancoReactivos.Prueba.PruebaID == null) throw new ArgumentNullException("ReactivoBancoReactivosDinamicoCtrl: PruebaID no puede ser nulo ");
			#endregion

			#region *** begin transaction ***
			object myFirm = new object();
			dctx.OpenConnection(myFirm);
			dctx.BeginTransaction(myFirm);

			try
			{
			#endregion

				PruebaDinamica prueba = pruebaCtrl.RetrieveComplete(dctx, bancoReactivos.Prueba as PruebaDinamica, true);

				foreach (ReactivoBanco reactivoBanco in bancoReactivos.ListaReactivosBanco.OrderBy(x => x.Activo))
				{
					if (reactivoBanco.ReactivoBancoID == null || reactivoBanco.ReactivoBancoID == 0) //NUEVO
					{
						if (reactivoBanco.ReactivoOriginal == null || reactivoBanco.ReactivoOriginal.ReactivoID == null)
							throw new ArgumentNullException("ReactivoBancoReactivosDinamicoCtrl: ReactivoOriginalID no puede ser nulo");

						//Obtengo el objeto bancoReactivo completo de la base datos
						BancoReactivosDinamico bancoReactivosActual = new BancoReactivosDinamico { BancoReactivoID = bancoReactivos.BancoReactivoID };
                        bancoReactivosActual.Prueba = prueba;
						bancoReactivosActual = bancoReactivoCtrl.RetrieveComplete(dctx, bancoReactivosActual);

						if (bancoReactivosActual.ListaReactivosBanco.Where(x => x.ReactivoOriginal.ReactivoID == reactivoBanco.ReactivoOriginal.ReactivoID && x.Activo == true).Count() > 0
						&& bancoReactivos.ListaReactivosBanco.Where(y => y != reactivoBanco &&  y.ReactivoOriginal.ReactivoID == reactivoBanco.ReactivoOriginal.ReactivoID && y.Activo == true).Count() > 0)
							throw new Exception("ReactivoBancoReactivosDinamicoCtrl: El banco de reactivos ya tiene asignado uno o más reactivos que desea dar de alta. Verifique.");

						Reactivo reactivoCopia = reactivoCtrl.RetrieveComplete(dctx, new Reactivo { ReactivoID = reactivoBanco.ReactivoOriginal.ReactivoID, TipoReactivo = ETipoReactivo.ModeloGenerico });
						if (reactivoCopia == null) throw new Exception("ReactivoBancoReactivosDinamicoCtrl: Sólo se puede asignar reactivos de tipo genérico a una prueba dinámica. Verifique");

						reactivoCopia.ReactivoID = Guid.NewGuid();
						reactivoCopia.Asignado = true;

                        #region limpiar identificadores del reactivo
                        foreach (Pregunta pregunta in reactivoCopia.Preguntas)
                        {
                            pregunta.PreguntaID = null;
                            pregunta.RespuestaPlantilla.RespuestaPlantillaID = null;

                            if (pregunta.RespuestaPlantilla.TipoRespuestaPlantilla == ETipoRespuestaPlantilla.OPCION_MULTIPLE)
                            {
                                foreach (OpcionRespuestaPlantilla opcion in (pregunta.RespuestaPlantilla as RespuestaPlantillaOpcionMultiple).ListaOpcionRespuestaPlantilla)
                                {
                                    opcion.OpcionRespuestaPlantillaID = null;
                                }
                            }
                        }
                        #endregion

                        reactivoCtrl.InsertComplete(dctx, reactivoCopia);

						//Configurar el objeto ReactivoBanco a Insetar
						reactivoBanco.Reactivo = reactivoCopia;
						reactivoBanco.Activo = true;

						ReactivoBancoReactivosDinamicoInsHlp da = new ReactivoBancoReactivosDinamicoInsHlp();
						da.Action(dctx, bancoReactivos, reactivoBanco);
					}
					else //reactivoBanco Editar
					{
						ReactivoBancoReactivosDinamicoUpdHlp da = new ReactivoBancoReactivosDinamicoUpdHlp();
						da.Action(dctx, reactivoBanco);
					}
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
        #endregion
    }
}
