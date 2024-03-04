using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.ConfiguracionActividades.BO;
using POV.ConfiguracionActividades.Services;
using POV.Modelo.Context;
using POV.ConfiguracionesPlataforma.BO;
using POV.ConfiguracionesPlataforma.Services;
using Framework.Base.DataAccess;

namespace POV.ServiciosActividades.Controllers
{
	public class RealizarActividadesController
	{
		#region Atributos

		private Contexto ctx;
		private AsignacionActividadCtrl Controlador;
		private object firma= new object();
		#endregion

		#region Constructores
		/// <summary>
		/// Constructor por defecto
		/// </summary>
		public RealizarActividadesController()
		{
			ctx = new Contexto(firma);
			
		}
		#endregion

		#region Métodos
		/// <summary>
		/// Consulta las actividades asignadas de un alumno
		/// </summary>
		/// <param name="alumno"></param>
		/// <returns></returns>
		public List<AsignacionActividad> ConsultarActividadesAsignadas(Alumno alumno, Escuela escuela, CicloEscolar cicloescolar)
		{
			Controlador = new AsignacionActividadCtrl(ctx);
			List<AsignacionActividad> asignaciones = Controlador.RetrieveAsignacionesAlumno(alumno,escuela,cicloescolar, false);
			return asignaciones;
		}

        /// <summary>
        /// Consulta las actividades anteriores y actuales asignadas de un alumno
        /// </summary>
        /// <param name="alumno"></param>
        /// <returns></returns>
        public List<AsignacionActividad> ConsultarActividadesAsignadas(Alumno alumno)
        {
            Controlador = new AsignacionActividadCtrl(ctx);
            List<AsignacionActividad> asignaciones = Controlador.Retrieve(new AsignacionActividad { Alumno = alumno }, false);
            return asignaciones.Where(item => item.FechaInicio <= DateTime.Now).OrderBy(item => item.FechaInicio).Reverse().ToList();
        }

        public List<BloqueActividad> ConsultarBloques(BloqueActividad filtro)
        {
            BloqueActividadCtrl ctrl = new BloqueActividadCtrl(ctx);
            return ctrl.Retrieve(filtro, false).OrderBy(item => item.FechaInicio).ToList();
        }

        public ConfiguracionGeneral ConsultarConfiguracionGeneral()
        {
            ConfiguracionGeneral config = null;

            ConfiguracionGeneralCtrl ctrl = new ConfiguracionGeneralCtrl(ctx);

            return ctrl.Retrieve(config, false).FirstOrDefault();
        }
        
        public List<PlantillaLudica> ConsultarPlantillasLudicas(PlantillaLudica filtro)
        {
            PlantillaLudicaCtrl ctrl = new PlantillaLudicaCtrl(ctx);
            return ctrl.RetrieveWithRelationship(filtro, false);
        }

		public void IniciarTarea(AsignacionActividad asignacionActividad)
		{
			using (Controlador = new AsignacionActividadCtrl(ctx))
			{
				AsignacionActividad asigActividad = Controlador.Retrieve(asignacionActividad, true).FirstOrDefault();
                TareaRealizada tareaActual = asignacionActividad.TareasRealizadas.FirstOrDefault();
				TareaRealizada tarea =
					asigActividad.TareasRealizadas.Where(
                        tr => tr.TareaRealizadaId == tareaActual.TareaRealizadaId)
						.FirstOrDefault();
				if (tarea.Estatus == EEstatusTarea.No_Iniciado)
				{
					tarea.Estatus = EEstatusTarea.Iniciado;
					tarea.FechaInicio = DateTime.Now;
                    tarea.ResultadoPruebaId = tareaActual.ResultadoPruebaId;
					Controlador.Update(asigActividad);
					ctx.Commit(firma);
				}
			}
			

		}
		
		/// <summary>
		/// Consulta la tarea relacionada de una actividad en especifico
		/// </summary>
		/// <param name="asignacionActividad">asignacion que contiene la tarea realizada a consultar</param>
		/// <returns></returns>
		public TareaRealizada ConsultarTareaActividad(AsignacionActividad asignacionActividad)
		{
			TareaRealizada tareaRealizada = new TareaRealizada();

			using (Controlador = new AsignacionActividadCtrl(ctx))
			{
				AsignacionActividad asigActividad = Controlador.Retrieve(asignacionActividad, true).FirstOrDefault();
				tareaRealizada =
					asigActividad.TareasRealizadas.Where(
						tr => tr.TareaRealizadaId == asignacionActividad.TareasRealizadas.FirstOrDefault().TareaRealizadaId)
						.FirstOrDefault();

			}

			return tareaRealizada;

		}

		public AsignacionActividad ConsultarActividadAsignada(AsignacionActividad asignacion)
		{
			
			using (Controlador= new AsignacionActividadCtrl(ctx))
			{
				return Controlador.Retrieve(asignacion, false).FirstOrDefault();
			}
		}

		public void FinalizarTarea(AsignacionActividad asignacionActividad)
		{
			using (Controlador = new AsignacionActividadCtrl(ctx))
			{
				AsignacionActividad asigActividad = Controlador.Retrieve(asignacionActividad, true).FirstOrDefault();
				TareaRealizada tareaTemp = asignacionActividad.TareasRealizadas.FirstOrDefault();
                
                TareaRealizada tarea =
					asigActividad.TareasRealizadas.Where(
                        tr => tr.TareaRealizadaId == tareaTemp.TareaRealizadaId)
						.FirstOrDefault();
				if (tarea.Estatus == EEstatusTarea.Iniciado)
				{
					tarea.Estatus = EEstatusTarea.Finalizado;
					tarea.FechaFin = DateTime.Now;
                    tarea.ResultadoPruebaId = tareaTemp.ResultadoPruebaId;
					Controlador.Update(asigActividad);
					ctx.Commit(firma);
				}
			}
		}

        public PreferenciaUsuario ConsultarPreferenciaUsuario(PreferenciaUsuario filtro, bool tracking = false)
        {
            PreferenciaUsuarioCtrl ctrl = new PreferenciaUsuarioCtrl(ctx);

            // Se carga la preferencia del usuario
            PreferenciaUsuario preferenciaUsuario = ctrl.Retrieve(filtro, tracking).FirstOrDefault();

            // Se valida que el usuario tiene preferencia
            if (preferenciaUsuario == null)
            {
                
                PlantillaLudicaCtrl ctrlPlantillaLudica = new PlantillaLudicaCtrl(ctx);

                // Plantilla ludica Predeterminada
                var plantillaPredeterminada = ctrlPlantillaLudica.Retrieve(new PlantillaLudica {EsPredeterminado = true}, true).FirstOrDefault();

                if (plantillaPredeterminada != null)
                {
                    // Se agregan a la preferencia la planitlla predeteminada 
                    preferenciaUsuario = new PreferenciaUsuario()
                    {
                        UsuarioId = filtro.UsuarioId,
                        PlantillaLudicaId = plantillaPredeterminada.PlantillaLudicaId,
                        FechaRegistro = DateTime.Now
                    };

                    // Se inserta la preferencia del usuario
                    ctrl.Insert(preferenciaUsuario);
                    ctx.Commit(firma);
                }
            }

            return preferenciaUsuario;
        }

        public bool ActualizarPrefeenciaUsuario(PreferenciaUsuario preferenciaUsuario)
        {
            bool res = false;
            PreferenciaUsuarioCtrl ctrl = new PreferenciaUsuarioCtrl(ctx);
            PreferenciaUsuario pref = ctrl.Retrieve(new PreferenciaUsuario { PreferenciaUsuarioId = preferenciaUsuario.PreferenciaUsuarioId }, true).FirstOrDefault();
            pref.PlantillasLudicas = null;
            pref.PlantillaLudicaId = preferenciaUsuario.PlantillaLudicaId;
            ctrl.Update(pref);
            if (ctx.Commit(firma) != 0) res = true;
            return res;
        }
		#endregion




	}
}
