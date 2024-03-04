using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Base.DataAccess;
using POV.Core.RedSocial.Implement;
using POV.Core.RedSocial.Interfaces;
using POV.Profesionalizacion.BO;
using POV.Profesionalizacion.DTO.BO;
using System.Data;
using POV.Profesionalizacion.Service;

namespace POV.Profesionalizacion.DTO.Service
{
    public class SituacionAprendizajeDTOCtrl
    {
        private IUserSession userSession;
        private IDataContext dctx;
        private SituacionAprendizajeCtrl situacionCtrl;

        public SituacionAprendizajeDTOCtrl()
        {
            userSession = new UserSession();
            situacionCtrl = new SituacionAprendizajeCtrl();
            dctx = new DataContext((new DataProviderFactory()).GetDataProvider("POV"));
        }

        #region Consultar Situaciones de Aprendizaje Acciones.
        /// <summary>
        /// Obtiene la lista de situaciones encontradas de acuerdo a los filtros de búsqueda proporcionados por el usuario.
        /// </summary>
        /// <param name="dto">Información introducida por el usuario.</param>
        /// <returns>Regresa una lista de las asistencias encontradas.</returns>
        public List<situacionaprendizajedto> SearchSituacionesAprendizaje(situacionaprendizajeinputdto dto)
        {
            List<situacionaprendizajedto> lsituacionaprendizaje = new List<situacionaprendizajedto>();
            try
            {
                int currentpage,pagesize;
                Dictionary<string, string> parameters = null;
                if (dto.currentpage == null)
                {
                    currentpage = 1;
                }
                else
                {
                    currentpage = (int)dto.currentpage;
                }

                dto.sort = "Nombre";
                dto.order = "DESC";
                pagesize = 10;
                parameters = dtoInputToParemeters(dto);

                DataSet lsitaprend = situacionCtrl.RetrieveComplete(dctx, pagesize, currentpage, dto.sort, dto.order, parameters);
                if (lsitaprend.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow situacionDr in lsitaprend.Tables[0].Rows)
                    {
                        situacionaprendizajedto situaciondto = new situacionaprendizajedto();
                        situaciondto.situacionid = Convert.ToInt32(situacionDr["SituacionAprendizajeID"]);
                        situaciondto.ejetematicoid = Convert.ToInt32(situacionDr["EjeTematicoID"]);
                        situaciondto.nombresituacion = situacionDr["SituacionAprendizaje"].ToString();
                        situaciondto.nombreeje = situacionDr["Nombre"].ToString();
                        situaciondto.descripcionsituacion = situacionDr["Descripcion"].ToString();
                        situaciondto.success = true;

                        lsituacionaprendizaje.Add(situaciondto);
                    }
                }
                return lsituacionaprendizaje;
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
        }
        #endregion
        /// <summary>
        /// Convierte la información introducida por el usuario a los parámetros de búsqueda.
        /// </summary>
        /// <param name="dto">Datos introducidos por el usuario.</param>
        /// <returns>Regresa un diccionario de parámetros.</returns>
        private Dictionary<string, string> dtoInputToParemeters(situacionaprendizajeinputdto dto)
        {
            Dictionary<string, string> parametros = new Dictionary<string, string>();
            parametros.Add("EstatusSituacion", "1");
            parametros.Add("EstatusEjeTematico", "1");
            if (dto.ejetematicoid > 0)
                parametros.Add("EjeTematicoID", dto.ejetematicoid.ToString());
            if (dto.nombresituacion != null)
                parametros.Add("Nombre", string.Format("%{0}%",dto.nombresituacion));

            parametros.Add("ContratoID", userSession.Contrato.ContratoID.ToString());               

            return parametros;
        }
    }
}
