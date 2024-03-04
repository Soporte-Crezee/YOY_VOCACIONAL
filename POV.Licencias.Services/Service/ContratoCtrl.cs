using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Framework.Base.DataAccess;
using GP.SocialEngine.BO;
using POV.CentroEducativo.BO;
using POV.Comun.BO;
using POV.Licencias.BO;
using POV.Licencias.DA;
using POV.Licencias.DAO;
using POV.Localizacion.BO;
using POV.Localizacion.Service;
using POV.Logger.Service;
using POV.Seguridad.BO;
using POV.Prueba.BO;
using POV.Profesionalizacion.BO;
using POV.Profesionalizacion.Service;
namespace POV.Licencias.Service
{
    public class ContratoCtrl
    {
        /// <summary>
        /// Consulta registros de Contratos en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="contrato">Contrato que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de Contratos generada por la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, Contrato contrato)
        {
            return new ContratoRetHlp().Action(dctx, contrato);
        }

        /// <summary>
        /// Consulta un registro completo de contrato de la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="contrato">Contrato que provee el criterio de selección para realizar la consulta</param>
        /// <returns>Contrato completo</returns>
        public Contrato RetrieveComplete(IDataContext dctx, Contrato contrato)
        {
            if (contrato == null) throw new Exception("El contrato no puede ser nulo.");

            Contrato contratoComplete = null;

            DataSet dsContrato = Retrieve(dctx, contrato);
             if (dsContrato.Tables[0].Rows.Count > 0)
            {
                int index = dsContrato.Tables[0].Rows.Count;
                DataRow drContrato = dsContrato.Tables[0].Rows[index - 1];
                contratoComplete = DataRowToContrato(drContrato);

                //Obtener lista de ciclos contrato activo completa
                CicloContratoCtrl cicloContratoCtrl = new CicloContratoCtrl();
                List<CicloContrato> ciclosContrato = cicloContratoCtrl.RetrieveListCicloContratoComplete(dctx, contratoComplete);

                contratoComplete = DataRowToContrato(drContrato, ciclosContrato);
            }

            return contratoComplete;
        }
        /// <summary>
        /// Consulta una lista de prueba contrato
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="contrato">contrato que se usara como filtro</param>
        /// <param name="cicloContrato">Filtro para consultar solo las pruebas de ese contrato con ese ciclo escolar
        /// <returns></returns>
        public List<PruebaContrato> RetrievePruebasAsignadoContrato(IDataContext dctx, Contrato contrato, CicloContrato cicloContrato)
        {
            PruebaContratoCtrl ctrl = new PruebaContratoCtrl();
            return  ctrl.RetrievePruebasAsignadoContrato(dctx, contrato, cicloContrato);
        }
        /// <summary>
        /// Verifica si el contrato tiene licencias disponibles para todas las licencias escuela asociadas.
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="contrato">Contrato</param>
        /// <param name="nDisponibles"> total de licencias disponibles, -1 en caso de ser ilimitadas</param>
        /// <returns></returns>
        public bool TieneLicenciasDisponibles(IDataContext dctx, Contrato contrato, out int nDisponibles)
        {
            LicenciaEscuelaCtrl licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();

            contrato = LastDataRowToContrato(Retrieve(dctx, contrato));

            if (contrato.LicenciasLimitadas.Value)
            {//Recuperacion de las licencias del contrato
                List<LicenciaEscuela> licencias = licenciaEscuelaCtrl.RetriveLicenciaEscuela(dctx, new LicenciaEscuela { Contrato = contrato });
                //contabilizar las licencias asignadas
                int asignadas = licencias.Sum(item => item.LicenciasAsignadas());
                //Se sustraen las asignadas al numero de licencias del contrato para obtener las diponibles
                nDisponibles = contrato.NumeroLicencias.Value - asignadas;

                return nDisponibles > 0;
            }
            else
            {
                nDisponibles = -1;
                return true;
            }
        }

        public int RetrieveNumeroLicenciasAsignadas(IDataContext dctx, Contrato contrato)
        {
            if (contrato == null) throw new Exception("El contrato no puede ser nulo");
            contrato = LastDataRowToContrato(Retrieve(dctx, contrato));
            LicenciaEscuelaCtrl licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
            List<LicenciaEscuela> licencias = licenciaEscuelaCtrl.RetriveLicenciaEscuela(dctx, new LicenciaEscuela { Contrato = contrato });

            int asignadas = licencias.Sum(item => item.LicenciasAsignadas());

            return asignadas;
        }
        /// <summary>
        /// Crear un registro de Contrato en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="contrato">Contrato que desea crear</param>
        public void Insert(IDataContext dctx, Contrato contrato)
        {
            if (contrato == null)
                throw new ArgumentNullException("contrato", "Contrato es requerido.");

            if (contrato.Ubicacion.Pais.PaisID == null || contrato.Ubicacion.Estado.EstadoID == null)
                throw new ArgumentException("Pais y Estado es requerido.");

            if (contrato.FechaContrato > contrato.InicioContrato)
                throw new ArgumentException("Fecha de Contrato debe ser menor a Fecha de Inicio.");

            if (contrato.InicioContrato > contrato.FinContrato)
                throw new ArgumentException("Fecha de Inicio debe ser menor a Fecha de Finalización.");

            if (contrato.LicenciasLimitadas.Value && !(contrato.NumeroLicencias.Value > 0))
                throw new ArgumentException("Número de Licencias es requerido.");

            #region Inicializar conexion a BD
            object con = new object();
            try
            {
                dctx.OpenConnection(con);
            }
            catch (Exception)
            {
                throw new Exception("Inconsistencias al conectarse a la base de datos.");
            }
            #endregion

            try
            {
                dctx.BeginTransaction(con);

                if (contrato.Ubicacion.UbicacionID == null)
                {
                    Ubicacion ubiEstado = new Ubicacion();
                    ubiEstado.Pais = new Pais();
                    ubiEstado.Estado = new Estado();
                    ubiEstado.Pais.PaisID = contrato.Ubicacion.Pais.PaisID;
                    ubiEstado.Estado.EstadoID = contrato.Ubicacion.Estado.EstadoID;

                    UbicacionCtrl ubicaCtrl = new UbicacionCtrl();
                    DataSet resultado = ubicaCtrl.RetrieveExacto(dctx, ubiEstado);

                    if (resultado.Tables[0].Rows.Count == 0)
                    {
                        ubiEstado.FechaRegistro = DateTime.Now;
                        ubicaCtrl.Insert(dctx, ubiEstado);
                        resultado = ubicaCtrl.RetrieveExacto(dctx, ubiEstado);
                    }

                    contrato.Ubicacion.UbicacionID = ubicaCtrl.LastDataRowToUbicacion(resultado).UbicacionID;
                }

                new ContratoInsHlp().Action(dctx, contrato);


                dctx.CommitTransaction(con);
            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(con);
                LoggerHlp.Default.Error(this, ex);

                contrato.Ubicacion.UbicacionID = null;
                throw;
            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(con);
            }
        }

        /// <summary>
        /// Inserta un registro completo de un contrato en la base de datos del sistema
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="contrato"></param>
        public void InsertComplete(IDataContext dctx, Contrato contrato)
        {
            if (contrato == null)
                throw new ArgumentNullException("contrato", "Contrato es requerido.");

            if (contrato.Ubicacion.Pais.PaisID == null || contrato.Ubicacion.Estado.EstadoID == null)
                throw new ArgumentException("Pais y Estado es requerido.");

            if (contrato.FechaContrato > contrato.InicioContrato)
                throw new ArgumentException("Fecha de Contrato debe ser menor a Fecha de Inicio.");

            if (contrato.InicioContrato > contrato.FinContrato)
                throw new ArgumentException("Fecha de Inicio debe ser menor a Fecha de Finalización.");

            if (contrato.LicenciasLimitadas.Value && !(contrato.NumeroLicencias.Value > 0))
                throw new ArgumentException("Número de Licencias es requerido.");


            #region Inicializar conexion a BD
            object con = new object();
            try
            {
                dctx.OpenConnection(con);
            }
            catch (Exception)
            {
                throw new Exception("Inconsistencias al conectarse a la base de datos.");
            }
            #endregion

            try
            {
                dctx.BeginTransaction(con);

                Insert(dctx, contrato);

                dctx.CommitTransaction(con);
            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(con);
                LoggerHlp.Default.Error(this, ex);

                contrato.Ubicacion.UbicacionID = null;
                throw;
            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(con);
            }
        }

        /// <summary>
        /// Actualiza de manera optimista un registro de Contrato en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="contrato">Contrato que tiene los datos nuevos</param>
        /// <param name="previous">Contrato que tiene los datos anteriores</param>
        public void Update(IDataContext dctx, Contrato contrato, Contrato previous)
        {
            if (contrato == null)
                throw new ArgumentNullException("contrato", "Contrato es requerido.");

            if (contrato.Ubicacion.Pais.PaisID == null || contrato.Ubicacion.Estado.EstadoID == null)
                throw new ArgumentException("Pais y Estado es requerido.");

            if (contrato.FechaContrato > contrato.InicioContrato)
                throw new ArgumentException("Fecha de Contrato debe ser menor a Fecha de Inicio.");

            if (contrato.InicioContrato > contrato.FinContrato)
                throw new ArgumentException("Fecha de Inicio debe ser menor a Fecha de Finalización.");

            if (contrato.LicenciasLimitadas.Value && !(contrato.NumeroLicencias.Value > 0))
                throw new ArgumentException("Número de Licencias es requerido.");

            #region Inicializar conexion a BD
            object con = new object();
            try
            {
                dctx.OpenConnection(con);
            }
            catch (Exception)
            {
                throw new Exception("Inconsistencias al conectarse a la base de datos.");
            }
            #endregion

            try
            {
                dctx.BeginTransaction(con);

                //TODO: Validar los contratos en cuanto a las licencias asignaadas a LicenciaEscuela con el dato de Numero de Licencias si esto es requerido

                if (contrato.Ubicacion.UbicacionID == null)
                {
                    Ubicacion ubiEstado = new Ubicacion();
                    ubiEstado.Pais = new Pais();
                    ubiEstado.Estado = new Estado();
                    ubiEstado.Pais.PaisID = contrato.Ubicacion.Pais.PaisID;
                    ubiEstado.Estado.EstadoID = contrato.Ubicacion.Estado.EstadoID;

                    UbicacionCtrl ubicaCtrl = new UbicacionCtrl();
                    DataSet resultado = ubicaCtrl.RetrieveExacto(dctx, ubiEstado);

                    if (resultado.Tables[0].Rows.Count == 0)
                    {
                        ubiEstado.FechaRegistro = DateTime.Now;
                        ubicaCtrl.Insert(dctx, ubiEstado);
                        resultado = ubicaCtrl.RetrieveExacto(dctx, ubiEstado);
                    }

                    contrato.Ubicacion.UbicacionID = ubicaCtrl.LastDataRowToUbicacion(resultado).UbicacionID;
                }

                new ContratoUpdHlp().Action(dctx, contrato, previous);
                dctx.CommitTransaction(con);
            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(con);
                LoggerHlp.Default.Error(this, ex);

                contrato.Ubicacion.UbicacionID = null;
                throw;
            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(con);
            }
        }

        /// <summary>
        /// Crea un objeto de Contrato a partir de los datos del último DataRow de la primera DataTable del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de Contrato</param>
        /// <returns>Un objeto Contrato creado a partir del DataSet</returns>
        public Contrato LastDataRowToContrato(DataSet ds)
        {
            if (!ds.Tables.Contains("Contrato"))
                throw new Exception("DataRowToContrato: DataSet no tiene la tabla Contrato");
            int index = ds.Tables["Contrato"].Rows.Count;
            if (index < 1)
                throw new Exception("DataRowToContrato: El DataSet no tiene filas");
            return this.DataRowToContrato(ds.Tables["Contrato"].Rows[index - 1]);
        }

        /// <summary>
        /// Crea un objeto Contrato a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de Contrato</param>
        /// <param name="ciclosContrato">Opcional. Lista de ciclos contrato del contrato</param>
        /// <returns>Un objeto Contrato creado a partir de los datos del DataRow</returns>
        public Contrato DataRowToContrato(DataRow row, List<CicloContrato> ciclosContrato = null)
        {
            if (ciclosContrato == null)
                ciclosContrato = new List<CicloContrato>();

            Contrato contrato = new Contrato(ciclosContrato);

            contrato.Cliente = new Cliente();
            contrato.Ubicacion = new Ubicacion();
            contrato.UsuarioRegistro = new Usuario();
            if (row.IsNull("ContratoID"))
                contrato.ContratoID = null;
            else
                contrato.ContratoID = (Int64)Convert.ChangeType(row["ContratoID"], typeof(Int64));
            if (row.IsNull("Clave"))
                contrato.Clave = null;
            else
                contrato.Clave = (String)Convert.ChangeType(row["Clave"], typeof(String));
            if (row.IsNull("FechaContrato"))
                contrato.FechaContrato = null;
            else
                contrato.FechaContrato = (DateTime)Convert.ChangeType(row["FechaContrato"], typeof(DateTime));
            if (row.IsNull("InicioContrato"))
                contrato.InicioContrato = null;
            else
                contrato.InicioContrato = (DateTime)Convert.ChangeType(row["InicioContrato"], typeof(DateTime));
            if (row.IsNull("FinContrato"))
                contrato.FinContrato = null;
            else
                contrato.FinContrato = (DateTime)Convert.ChangeType(row["FinContrato"], typeof(DateTime));
            if (row.IsNull("LicenciasLimitadas"))
                contrato.LicenciasLimitadas = null;
            else
                contrato.LicenciasLimitadas = (Boolean)Convert.ChangeType(row["LicenciasLimitadas"], typeof(Boolean));
            if (row.IsNull("Estatus"))
                contrato.Estatus = null;
            else
                contrato.Estatus = (Boolean)Convert.ChangeType(row["Estatus"], typeof(Boolean));
            if (row.IsNull("NumeroLicencias"))
                contrato.NumeroLicencias = null;
            else
                contrato.NumeroLicencias = (Int32)Convert.ChangeType(row["NumeroLicencias"], typeof(Int32));
            if (row.IsNull("ClienteNombre"))
                contrato.Cliente.Nombre = null;
            else
                contrato.Cliente.Nombre = (String)Convert.ChangeType(row["ClienteNombre"], typeof(String));
            if (row.IsNull("ClienteDomicilio"))
                contrato.Cliente.Domicilio = null;
            else
                contrato.Cliente.Domicilio = (String)Convert.ChangeType(row["ClienteDomicilio"], typeof(String));
            if (row.IsNull("ClienteRepresentante"))
                contrato.Cliente.Representante = null;
            else
                contrato.Cliente.Representante = (String)Convert.ChangeType(row["ClienteRepresentante"], typeof(String));
            if (row.IsNull("ClienteTelefono"))
                contrato.Cliente.Telefono = null;
            else
                contrato.Cliente.Telefono = (String)Convert.ChangeType(row["ClienteTelefono"], typeof(String));
            if (row.IsNull("UbicacionID"))
                contrato.Ubicacion.UbicacionID = null;
            else
                contrato.Ubicacion.UbicacionID = (Int64)Convert.ChangeType(row["UbicacionID"], typeof(Int64));
            if (row.IsNull("UsuarioID"))
                contrato.UsuarioRegistro.UsuarioID = null;
            else
                contrato.UsuarioRegistro.UsuarioID = (Int32)Convert.ChangeType(row["UsuarioID"], typeof(Int32));
            if (row.IsNull("FechaRegistro"))
                contrato.FechaRegistro = null;
            else
                contrato.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
            return contrato;
        }

        #region *** Profesionalizacion Contrato ***

        /// <summary>
        /// Consulta registros de ProfesionalizacionContratoDaRetHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="contrato">Provee el criterio para seleccion para la realizacion de la consulta</param>
        /// <returns>El DataSet que contiene la información de ProfesionalizacionContratoDARetHlp generada por la consulta</returns>
        private DataSet RetrieveProfContrato(IDataContext dctx, Contrato contrato, EjeTematico ejeTematico)
        {
            ProfesionalizacionContratoDARetHlp da = new ProfesionalizacionContratoDARetHlp();
            DataSet ds = da.Action(dctx, contrato, ejeTematico);
            return ds;
        }

        /// <summary>
        /// Consulta la lista de Ejes Tematicos del contrato 
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="contrato">Provee el criterio para seleccion para la realizacion de la consulta</param>
        /// <returns>La lista de ejes tematicos que estan asignados al contrato</returns>
        public ProfesionalizacionContrato RetrieveProfesionalizacionContrato(IDataContext dctx, Contrato contrato)
        {
            if (contrato == null)
                throw new NoNullAllowedException("El contrato no puede ser nulo");
            if (contrato.ContratoID == null)
                throw new NoNullAllowedException("El contratoID no puede ser nulo");

            ProfesionalizacionContrato profesionalizacionContrato = new ProfesionalizacionContrato();
            profesionalizacionContrato.ListaEjesTematicos = new List<EjeTematico>();

            EjeTematicoCtrl ejeTematicoCtrl = new EjeTematicoCtrl();
            long ejeTematico;

            DataSet ds = this.RetrieveProfContrato(dctx, contrato, new EjeTematico());
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ejeTematico = (long)Convert.ChangeType(dr["EjeTematicoID"], typeof(long));
                    EjeTematico eje = null;
                    DataSet dsEjesTematicos = ejeTematicoCtrl.Retrieve(dctx, new EjeTematico { EjeTematicoID = ejeTematico, EstatusProfesionalizacion = EEstatusProfesionalizacion.ACTIVO });
                    if (dsEjesTematicos.Tables[0].Rows.Count > 0)
                    {
                        eje = ejeTematicoCtrl.LastDataRowToEjeTematico(ejeTematicoCtrl.Retrieve(dctx, new EjeTematico { EjeTematicoID = ejeTematico, EstatusProfesionalizacion = EEstatusProfesionalizacion.ACTIVO }));
                        profesionalizacionContrato.ListaEjesTematicos.Add(eje);
                    }

                }
            }

            return profesionalizacionContrato;
        }

        /// <summary>
        /// Consulta los cotratos de un ejetematico
        /// </summary>
        /// <param name="dctx">provee el acceso a la base de datos</param>
        /// <param name="ejeTematico">Provee el criterio de consulta</param>
        /// <returns>contratos</returns>
        public List<Contrato> RetrieveContratoByEje(IDataContext dctx, EjeTematico ejeTematico)
        {
            List<Contrato> contratos = new List<Contrato>();
            long ContratoID;

            if (ejeTematico != null)
            {
                if (ejeTematico.EjeTematicoID != null)
                {
                    DataSet dsContratos = RetrieveProfContrato(dctx, new Contrato(), ejeTematico);
                    if (dsContratos.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drContratos in dsContratos.Tables[0].Rows)
                        {
                            ContratoID = (long)Convert.ChangeType(drContratos["ContratoID"], typeof(long));
                            Contrato contrato = LastDataRowToContrato(Retrieve(dctx, new Contrato { ContratoID = ContratoID, Estatus = true }));
                            contratos.Add(contrato);

                        }
                    }
                }
            }
            return contratos;

        }

        /// <summary>
        /// Elimina un registro de ProfesionalizacionContratoDADelHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="contrato">Provee el criterio de la elimnacion</param>
        /// <param name="ejeTematico">El eje tematico que sera eliminado</param>
        public void DeleteProfesionalizacionContrato(IDataContext dctx, Contrato contrato, EjeTematico ejeTematico)
        {
            ProfesionalizacionContratoDADelHlp da = new ProfesionalizacionContratoDADelHlp();
            da.Action(dctx, contrato, ejeTematico);
        }

        /// <summary>
        /// Inserta un registro de ProfesionalizacionContratoInselHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="contrato">Provee el criterio de registro</param>
        /// <param name="ejeTematico">El eje tematico que sera registrado</param>
        public void InsertProfesionalizacionContrato(IDataContext dctx, Contrato contrato, EjeTematico ejeTematico)
        {
            if (ValidateEjeTematicoContratoForInsertOrUpdate(dctx, contrato, ejeTematico))
            {
                ProfesionalizacionContratoInsHlp da = new ProfesionalizacionContratoInsHlp();
                da.Action(dctx, contrato, ejeTematico);
            }
            else
            {
                throw new DuplicateNameException("El Eje Tematico ya está registrado en el contrato, por favor verifique");

            }
        }

        /// <summary>
        /// Actualiza un registro de ProfesionalizacionContratoInselHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="contrato">Provee el criterio de actualizacion</param>
        /// <param name="ejeTematico">El eje tematico que sera actualizado</param>
        /// <param name="contratoAnterior">Provee el criterio de actualizacion</param>
        /// <param name="ejeTematicoAnterior">El eje tematico que sera actualizado</param>
        public void UpdateProfesionalizacionContrato(IDataContext dctx, Contrato contrato, EjeTematico ejeTematico, Contrato contratoAnterior, EjeTematico ejeTematicoAnterior)
        {
            if (ValidateEjeTematicoContratoForInsertOrUpdate(dctx, contrato, ejeTematico))
            {
                ProfesionalizacionContratoUpdHlp da = new ProfesionalizacionContratoUpdHlp();
                da.Action(dctx, contrato, ejeTematico, contratoAnterior, ejeTematicoAnterior);
            }
            else
            {
                throw new DuplicateNameException("El Eje Tematico ya está registrado en el contrato, por favor verifique");
            }
        }
        /// <summary>
        /// Inserta una lista de registros de ejes al contrato en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="contrato">Provee el criterio de registro</param>
        /// <param name="profesionalizacionContrato">lista de ejes tematicos que seran registrado</param>
        public void InsetListaEjesContrato(IDataContext dctx,Contrato contrato, ProfesionalizacionContrato profesionalizacionContrato)
        {
            foreach (EjeTematico ejeTematico in profesionalizacionContrato.ListaEjesTematicos)
            {
                InsertProfesionalizacionContrato(dctx,contrato,ejeTematico);
            }
        }
        /// <summary>
        /// Elimina una lista de registro de ejes asignados al contrato en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="contrato">Provee el criterio de la elimnacion</param>
        /// <param name="profesionalizacionContrato">Lista de ejes tematicos que seran eliminado</param>
        public void DeleteListaEjesContrato(IDataContext dctx, Contrato contrato, ProfesionalizacionContrato profesionalizacionContrato)
        {
            foreach (EjeTematico ejeTematico in profesionalizacionContrato.ListaEjesTematicos)
            {
                DeleteProfesionalizacionContrato(dctx, contrato, ejeTematico);
            }
        }
        #region Validaciones
        /// <summary>
        /// Valida que no exista otro registro igual en el contrato.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="contrato">Provee el criterio de registro</param>
        /// <param name="ejeTematico">El eje tematico a insertar en el contrato</param>
        private bool ValidateEjeTematicoContratoForInsertOrUpdate(IDataContext dctx, Contrato contrato, EjeTematico ejeTematico)
        {
            DataSet ds = RetrieveProfContrato(dctx, new Contrato { ContratoID = contrato.ContratoID }, new EjeTematico { EjeTematicoID = ejeTematico.EjeTematicoID });
            return ds.Tables["ProfesionalizacionContrato"].Rows.Count <= 0;
        }
        #endregion

        #endregion
    }
}
