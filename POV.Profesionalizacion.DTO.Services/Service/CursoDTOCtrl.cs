using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Core.RedSocial.Interfaces;
using Framework.Base.DataAccess;
using POV.Core.RedSocial.Implement;
using POV.Profesionalizacion.DTO.BO;
using POV.Profesionalizacion.Service;
using System.Data;
using POV.ContenidosDigital.DTO.BO;
using POV.Profesionalizacion.BO;

namespace POV.Profesionalizacion.DTO.Service
{
    public class CursoDTOCtrl
    {
        private IUserSession userSession;
        private IDataContext dctx;
      
        public CursoDTOCtrl()
        {
            userSession = new UserSession();
            dctx = new DataContext((new DataProviderFactory()).GetDataProvider("POV"));            
        }
        /// <summary>
        /// Obtiene los cursos que cumplen los criterios  de búsqueda 
        /// </summary>
        /// <param name="dto">El dto que proporciona los parametros de consulta</param>
        /// <returns>un dto que contiene la informacion de las situaciones de aprendizaje que cumplen los criterios de consulta</returns>    
        public List<cursodto> GetCursos(cursoinputdto dto)
        {
            
            #region Inicializar
            CursoCtrl ctrl = new CursoCtrl();
            String SORT_COLUMN = "AgrupadorContenidoDigitalID";
            String ORDER_COLUMN = "ASC";
            Dictionary<string, string> parametros = new Dictionary<string, string>();
            int pageSize = 0, currenPage = 0;
            List<cursodto> cursos = new List<cursodto>();
          
            #endregion
            
            if (!userSession.IsAlumno()) {
                currenPage = dto.currentpage;
                pageSize = dto.pagesize;
                if (dto.cursonombre != null)
                    if(dto.cursonombre.Length>0)
                    parametros.Add("CursoNombre","%"+ dto.cursonombre +"%");
                if (dto.cursotemaid != null)
                {
                    if(dto.cursotemaid>0)
                    parametros.Add("CursoTemaID", dto.cursotemaid.ToString());
                }
                if (dto.cursopresencial != null)
                    if(dto.cursopresencial>=0)
                    parametros.Add("CursoPresencial",( dto.cursopresencial.ToString()));


                dto.cursoestatus = (byte)EEstatusProfesionalizacion.ACTIVO;
                parametros.Add("CursoEstatus", (dto.cursoestatus.ToString()));
             
                
                DataSet ds =  ctrl.Retrieve(dctx, pageSize, currenPage,SORT_COLUMN, ORDER_COLUMN, parametros);
             foreach (DataRow dr in ds.Tables[0].Rows) {
                 cursos.Add(DataRowToCursoDTO(dr));
             }
           }
            return cursos;
        }
        /// <summary>
        /// Obtiene la informacion de un curso y sus contenidos digitales uaciones de aprendizaje que pertenecen a un eje temático y que están activas
        /// </summary>
        /// <param name="dto">El dto que proporciona los parametros de consulta</param>
        /// <returns>un dto que contiene la informacion de las situaciones de aprendizaje que cumplen los criterios de consulta</returns>
        public cursodetalledto GetInformacionCurso(cursoinputdto dto){
            #region Inicializar
            CursoCtrl ctrl = new CursoCtrl();
            cursodetalledto result = new cursodetalledto();

            String SORT_COLUMN = "AgrupadorContenidoDigitalID";
            String ORDER_COLUMN = "ASC";
            Dictionary<string, string> parametros = new Dictionary<string, string>();
            int pageSize = 0, currenPage = 0;
            result.currentpage = dto.currentpage;
            result.cursoid = dto.cursoid;
            
            if (!userSession.IsAlumno())
            {
                currenPage = dto.currentpage;
                pageSize = dto.pagesize;
                if (dto.cursoid != null)
                    parametros.Add("AgrupadorPadreID", dto.cursoid.ToString());
                

                DataSet ds = ctrl.RetrieveDetails(dctx, pageSize, currenPage, SORT_COLUMN, ORDER_COLUMN, parametros);

                
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow drCursoID = ds.Tables[0].Rows[0];
                    if (!drCursoID.IsNull("AgrupadorPadreID"))
                        result.cursoid = Convert.ToInt32(drCursoID["AgrupadorPadreID"]);

                    result.contenidos = new List<contenidodto>();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        result.contenidos.Add(DataRowToCursoDetalleDTO(dr));
                    }
                    result.currentpage = dto.currentpage + 1;
                }
               
                
               
            }
            return result;

            #endregion
        }
        private cursodto DataRowToCursoDTO(DataRow dr)
        {
            cursodto dto = new cursodto();
            if (!dr.IsNull("AgrupadorContenidoDigitalID"))
                dto.cursoid = Convert.ToInt32(dr["AgrupadorContenidoDigitalID"]);
            if (!dr.IsNull("Nombre"))
                dto.cursonombre = dr["Nombre"].ToString();
            if (!dr.IsNull("Presencial"))
                dto.cursopresencial = Convert.ToInt16(dr["Presencial"]);
            if (!dr.IsNull("TemaCurso"))
                dto.cursotema = dr["TemaCurso"].ToString();
            if (!dr.IsNull("Informacion"))
                dto.cursoinformacion = dr["Informacion"].ToString();

            return dto;
        }
        private contenidodto DataRowToCursoDetalleDTO(DataRow dr)
        {
            contenidodto dto = new contenidodto();

            if (!dr.IsNull("AgrupadorPadreID"))
                dto.cursoid = Convert.ToInt32(dr["AgrupadorPadreID"]);            
            if (!dr.IsNull("Nombre"))
                dto.nombrecontenido = dr["Nombre"].ToString();
            if (!dr.IsNull("ContenidoDigitalID"))
                dto.contenidoid = Convert.ToInt16(dr["ContenidoDigitalID"]);
           if (!dr.IsNull("Extension"))
                dto.tipodocumento = dr["Extension"].ToString();           
            return dto;
        }

    }
}
