using POV.Administracion.BO;
using POV.Blog.BO;
using POV.CentroEducativo.BO;
using POV.ConfiguracionActividades.BO;
using POV.ConfiguracionesPlataforma.BO;
using POV.Expediente.BO;
using POV.Modelo.Mapping.Administracion;
using POV.Modelo.Mapping.Blog;
using POV.Modelo.Mapping.CentroEducativo;
using POV.Modelo.Mapping.ConfiguracionActividades;
using POV.Modelo.Mapping.ConfiguracionPlataforma;
using POV.Modelo.Mapping.ContenidosDigital;
using POV.Modelo.Mapping.Expediente;
using POV.Modelo.Mapping.Licencias;
using POV.Modelo.Mapping.ModeloDiagnostico;
using POV.Modelo.Mapping.Operaciones;
using POV.Modelo.Mapping.Profesionalizacion;
using POV.Modelo.Mapping.Pruebas;
using POV.Modelo.Mapping.Reactivos;
using POV.Modelo.Mapping.Seguridad;
using POV.Operaciones.BO;
using POV.Prueba.BO;
using POV.Reactivos.BO;
using POV.Seguridad.BO;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;

namespace POV.Modelo.Context
{
    public class Contexto : DbContext
    {
        private object firma = null;

        public Contexto(object firma)
            : base("POV")
        {
            if (firma == null)
                throw new ArgumentNullException("firma");

            this.firma = firma;

            Database.SetInitializer<Contexto>(null);
            Configuration.LazyLoadingEnabled = true;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove(new ManyToManyCascadeDeleteConvention());
            modelBuilder.Conventions.Remove(new OneToManyCascadeDeleteConvention());

            #region ModeloDiagnostico
            modelBuilder.Configurations.Add(new AModeloMapping());
            modelBuilder.Configurations.Add(new ModeloDinamicoMapping());
            modelBuilder.Configurations.Add(new ClasificadorMapping());
            #endregion

            #region CentroEducativo
            modelBuilder.Configurations.Add(new AlumnoMapping());
            modelBuilder.Configurations.Add(new DocenteMapping());
            modelBuilder.Configurations.Add(new EscuelaMapping());
            modelBuilder.Configurations.Add(new GrupoCicloEscolarMapping());
            modelBuilder.Configurations.Add(new GrupoMapping());
            modelBuilder.Configurations.Add(new CicloEscolarMapping());
            modelBuilder.Configurations.Add(new CarreraMapping());
            modelBuilder.Configurations.Add(new UniversidadMapping());
            modelBuilder.Configurations.Add(new TutorMapping());
            modelBuilder.Configurations.Add(new EventoUniversidadMapping());
            modelBuilder.Configurations.Add(new NotaCompraMapping());
            modelBuilder.Configurations.Add(new TutorAlumnoMapping());
            modelBuilder.Configurations.Add(new SesionOrientacionMapping());
            modelBuilder.Configurations.Add(new EncuestaSatisfaccionMapping());
            #endregion

            #region Expediente
            modelBuilder.Configurations.Add(new UniversidadCarreraAspiranteMapping());
            modelBuilder.Configurations.Add(new UsuarioExpedienteMapping());
            #endregion

            #region ConfiguracionActividad
            modelBuilder.Ignore<PruebaFilter>();
            modelBuilder.Ignore<PruebaProxy>();
            modelBuilder.Ignore<ReactivoDocente>();
            modelBuilder.Configurations.Add(new ActividadMapping());
            modelBuilder.Configurations.Add(new ActividadDocenteMapping());
            modelBuilder.Configurations.Add(new AsignacionActividadMapping());
            modelBuilder.Configurations.Add(new AsignacionActividadGrupoMapping());
            modelBuilder.Configurations.Add(new TareaEjeTematicoMapping());
            modelBuilder.Configurations.Add(new TareaMapping());
            modelBuilder.Configurations.Add(new TareaPruebaMapping());
            modelBuilder.Configurations.Add(new TareaContenidoDigitalMapping());
            modelBuilder.Configurations.Add(new TareaReactivoMapping());
            modelBuilder.Configurations.Add(new TareaRealizadaMapping());
            modelBuilder.Configurations.Add(new URLContenidoMapping());
            modelBuilder.Configurations.Add(new ContenidoDigitalMapping());
            modelBuilder.Configurations.Add(new EjeTematicoMapping());
            modelBuilder.Configurations.Add(new ReactivoMapping());
            modelBuilder.Configurations.Add(new PruebaMapping());
            modelBuilder.Configurations.Add(new BloqueActividadMapping());
            modelBuilder.Configurations.Add(new ClasificadorResultadoMapping());
            modelBuilder.Configurations.Add(new ClasificadorResultadoDinamicaMapping());
            #endregion

            #region Configuracion Plataforma
            modelBuilder.Configurations.Add(new PlantillaLudicaMapping());
            modelBuilder.Configurations.Add(new PosicionActividadMapping());
            modelBuilder.Configurations.Add(new PreferenciaUsuarioMapping());
            modelBuilder.Configurations.Add(new ConfiguracionGeneralMapping());
            #endregion

            #region Licencias
            modelBuilder.Configurations.Add(new ContratoMapping());
            #endregion

            #region Blog
            modelBuilder.Configurations.Add(new PostFavoritoMapping());
            #endregion

            #region Administracion
            modelBuilder.Configurations.Add(new PaquetePremiumMapping());
            modelBuilder.Configurations.Add(new CompraPremiumMapping());
            modelBuilder.Configurations.Add(new CostoProductoMapping());
            modelBuilder.Configurations.Add(new CompraProductoMapping());
            modelBuilder.Configurations.Add(new ProductoCosteoMapping());
            modelBuilder.Configurations.Add(new CompraCreditoMapping());
            #endregion

            #region Seguridad
            modelBuilder.Configurations.Add(new UsuarioMapping());
            modelBuilder.Configurations.Add(new ConfigCalendarMapping());
            modelBuilder.Configurations.Add(new EventCalendarMapping());
            #endregion

            #region Operaciones
            modelBuilder.Configurations.Add(new ResultadoExportacionOrientadorMapping());
            #endregion
        }

        #region CentroEducativo
        public DbSet<Alumno> Alumno { get; set; }
        public DbSet<Docente> Docente { get; set; }
        public DbSet<Universidad> Universidad { get; set; }
        public DbSet<Carrera> Carrera { get; set; }
        public DbSet<Tutor> Tutor { get; set; }
        public DbSet<EventoUniversidad> EventoUniversidad { get; set; }
        public DbSet<NotaCompra> NotaCompra { get; set; }
        public DbSet<TutorAlumno> TutorAlumno { get; set; }
        public DbSet<SesionOrientacion> SesionOrientacion { get; set; }
        public DbSet<EncuestaSatisfaccion> EncuestaSatisfaccion { get; set; }
        #endregion

        #region Expediente
        public DbSet<UniversidadCarreraAspirante> UniversidadCarreraAspirante { get; set; }
        public DbSet<UsuarioExpediente> UsuarioExepdiente { get; set; }
        #endregion

        #region ConfiguracionActividad
        public DbSet<AsignacionActividad> AsignacionesActividades
        {
            get;
            set;
        }
        public DbSet<AsignacionActividadGrupo> AsignacionesActividadesGrupos
        {
            get;
            set;
        }
        public DbSet<Actividad> Actividades
        {
            get;
            set;
        }
        public DbSet<ActividadDocente> ActividadesDocentes
        {
            get;
            set;
        }
        public DbSet<ContenidosDigital.BO.ContenidoDigital> ContenidosDigitales
        {
            get;
            set;
        }
        public DbSet<APrueba> Pruebas { get; set; }

        public DbSet<BloqueActividad> BloquesActividad { get; set; }
        public DbSet<TareaRealizada> TareasRealizadas { get; set; }
        #endregion

        #region ConfiguracionPlataforma
        public DbSet<PlantillaLudica> PlantillasLudicas
        {
            get;
            set;
        }

        public DbSet<PreferenciaUsuario> PreferenciaUsuarios { get; set; }

        public DbSet<PosicionActividad> PosicionesActividades
        {
            get;
            set;
        }

        public DbSet<ConfiguracionGeneral> ConfiguracionesGenerales
        {
            get;
            set;
        }
        #endregion

        #region Blog
        public DbSet<PostFavorito> PostFavoritos { get; set; }
        #endregion

        #region Administracion
        public DbSet<PaquetePremium> PaquetePremium { get; set; }
        public DbSet<CompraPremium> CompraPremium { get; set; }
        public DbSet<CostoProducto> CostoProducto { get; set; }
        public DbSet<CompraProducto> CompraProducto { get; set; }
        public DbSet<ProductoCosteo> Producto { get; set; }
        public DbSet<CompraCredito> CompraCredito { get; set; }
        #endregion

        #region Seguridad
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<ConfigCalendar> ConfigCalendar { get; set; }
        public DbSet<EventCalendar> EventCalendar { get; set; }
        #endregion

        #region Operaciones
        public DbSet<ResultadoExportacionOrientador> ResultadoExportacionOrientador { get; set; }
        #endregion

        public int Commit(object firma)
        {
            try
            {
                return (this.firma == firma) ? base.SaveChanges() : 0;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        public void Disposing(object firma)
        {
            if (this.firma == firma)
                Dispose();
        }
    }
}
