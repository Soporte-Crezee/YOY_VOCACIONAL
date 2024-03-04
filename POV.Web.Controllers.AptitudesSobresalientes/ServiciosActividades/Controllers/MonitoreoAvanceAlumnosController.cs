using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using Framework.Base.DataAccess;
using POV.ConfiguracionActividades.BO;
using POV.ConfiguracionActividades.Services;
using POV.Modelo.Context;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;

namespace POV.Web.Controllers.ServiciosActividades.Controllers
{
	public class MonitoreoAvanceAlumnosController
	{
		private readonly Contexto _contexto;
		private readonly object _firma;
        #region Conexion Version Anterior
        IDataContext dataContext = new DataContext((new DataProviderFactory()).GetDataProvider("POV"));
        #endregion


		public MonitoreoAvanceAlumnosController(Contexto contexto)
		{
			_firma = new object();
			_contexto = contexto ?? new Contexto(_firma);
		}
		public List<AsignacionActividad> ConsultarAsignacionesActividad(AsignacionActividad asignacion)
		{
			List<AsignacionActividad> asignaciones;
			using (AsignacionActividadCtrl asignacionCtrl = new AsignacionActividadCtrl(_contexto))
			{
				asignaciones = asignacionCtrl.RetrieveWithRelationship(asignacion, false);
			}
			return asignaciones;
        }

        #region 
        /// <summary>
        /// Consultar los Grupos de la escuela con la que se inicia la sesión
        /// </summary>
        /// <param name="escuela">Escuela con la que se inicio sesión</param>
        /// <returns>Onject el cual tiene dos listas.</returns>
        public List<Grupo> ConsultarGruposEscuela(Escuela escuela, CicloEscolar cicloEscolar)
        {
            GrupoCicloEscolarCtrl grupoCicloEscolarCtrl = new GrupoCicloEscolarCtrl();
            GrupoCtrl grupoCtrl = new GrupoCtrl();
            List<Grupo> grups = new List<Grupo>();
            DataSet dsGrupos = grupoCicloEscolarCtrl.Retrieve(dataContext,
                 new GrupoCicloEscolar
                 {
                     Escuela = escuela,
                     CicloEscolar = cicloEscolar
                 });
            List<GrupoCicloEscolar> gruposCicloEscolars = new List<GrupoCicloEscolar>();
            foreach (DataRow dataRow in dsGrupos.Tables[0].Rows)
            {
                gruposCicloEscolars.Add(grupoCicloEscolarCtrl.DataRowToGrupoCicloEscolar(dataRow));
            }

            List<Grupo> grupos = gruposCicloEscolars.Select(g => g.Grupo).ToList();
            foreach (Grupo grupo in grupos)
            {
                grups.Add(grupoCtrl.LastDataRowToGrupo(grupoCtrl.Retrieve(dataContext, grupo, escuela)));
            }

            return grups;
        }
        #endregion
    }
}
