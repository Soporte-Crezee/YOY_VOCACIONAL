using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Base.DataAccess;
using POV.ContenidosDigital.DTO.BO;
using POV.Core.RedSocial.Implement;
using POV.Core.RedSocial.Interfaces;
using POV.Profesionalizacion.BO;
using POV.Profesionalizacion.DTO.BO;
using POV.Profesionalizacion.Service;
using System.Data;

namespace POV.Profesionalizacion.DTO.Service
{
    public class AsistenciaDTOCtrl
    {
        private IUserSession userSession;
        private IDataContext dctx;
        private AsistenciaCtrl asistenciaCtrl;

        public AsistenciaDTOCtrl() 
        {
            userSession = new UserSession();
            asistenciaCtrl = new AsistenciaCtrl();
            dctx = new DataContext((new DataProviderFactory()).GetDataProvider("POV"));
        }

        #region Consultar Asistencias.
        /// <summary>
        /// Obtiene los datos de las asistencias dadas de alta en la Base de Datos.
        /// </summary>
        /// <param name="dto">Objeto que contiene los datos capturados por el usuario</param>
        /// <returns>Regresa una lista de asistencias.</returns>
        public List<asistenciadto> SearchAsistencia(asistenciainputdto dto)
        {
            List<asistenciadto> lsAsistencia = new List<asistenciadto>();
            if (!userSession.IsAlumno())
            {
                int currentpage, pagesize;
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

                DataSet lsasistencia = asistenciaCtrl.RetrieveComplete(dctx, pagesize, currentpage, dto.sort, dto.order,
                                                                       parameters);
                if (lsasistencia.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drasistencia in lsasistencia.Tables[0].Rows)
                    {
                        asistenciadto asistenciadto = new asistenciadto();
                        asistenciadto.asistenciaid = Convert.ToInt32(drasistencia["AsistenciaID"]);
                        asistenciadto.nombre = drasistencia["Nombre"].ToString();
                        asistenciadto.tema = drasistencia["Tema"].ToString();
                        asistenciadto.success = true;

                        lsAsistencia.Add(asistenciadto);
                    }
                }
                
            }
            return lsAsistencia;
        }
        /// <summary>
        /// Valida y convierte los datos ingresados por el usuario en los parámetros de búsqueda.
        /// </summary>
        /// <param name="dto">Objeto que contiene los datos capturados por el usuario</param>
        /// <returns>Regresa un diccionario de string con los parámetros de búsqueda.</returns>
        private Dictionary<string, string> dtoInputToParemeters(asistenciainputdto dto)
        {
            Dictionary<string, string> parametros = new Dictionary<string, string>();

            if (dto.temaid > 0)
                parametros.Add("TemaID", dto.temaid.ToString());
            if(dto.tipodocumentoid > 0)
                parametros.Add("TipoDocumentoID", dto.tipodocumentoid.ToString());
            if (dto.nombre != null)
                parametros.Add("Nombre", string.Format("%{0}%", dto.nombre));

            return parametros;
        }
        #endregion
        #region Consultar Detalle de Asistencias.
        /// <summary>
        /// Obtiene la información detalle de cada asistencia dada de alta en la base de datos.
        /// </summary>
        /// <param name="dto">Objeto que contiene los datos capturados por el usuario</param>
        /// <returns>Regresa una lista de contenidos digitales asociados a la asistencia</returns>
        public contenidodigitaldto GetInformaciónAsistencia(asistenciainputdto dto)
        {
            AsistenciaCtrl asistenciactrl = new AsistenciaCtrl();
            contenidodigitaldto result = new contenidodigitaldto();
            
            Dictionary<string,string> parameters = new Dictionary<string, string>();
            int pageSize = 0, currentPage = 0;
            result.currentpage = Convert.ToInt32(dto.currentpage);
            result.asistenciaid = dto.asistenciaid;
            dto.sort = "AgrupadorContenidoDigitalID";
            dto.order = "ASC";

            if (!userSession.IsAlumno())
            {
                currentPage = Convert.ToInt32(dto.currentpage);
                pageSize = Convert.ToInt32(dto.pagesize);
                if(dto.asistenciaid != null)
                    parameters.Add("AgrupadorPadreID", dto.asistenciaid.ToString());

                DataSet ds = asistenciaCtrl.RetrieveDetalle(dctx, pageSize, currentPage, dto.sort, dto.order, parameters);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow drAsistenciaID = ds.Tables[0].Rows[0];
                    if (!drAsistenciaID.IsNull("AgrupadorPadreID"))
                        result.asistenciaid = Convert.ToInt32(drAsistenciaID["AgrupadorPadreID"]);

                    result.contenidos = new List<contenidodigitaldto>();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        result.contenidos.Add(DataRowToAsistenciaDetalleDTO(dr));
                    }
                    result.currentpage = Convert.ToInt32(dto.currentpage) + 1;
                }
            }

            return result;
        }
        /// <summary>
        /// Convierte un datarow en objeto de lista de contenidos digitales.
        /// </summary>
        /// <param name="dr">DataRow que contiene la información a convertir.</param>
        /// <returns>Regresa un lista de contenidos digitales asociados a la asistencia.</returns>
        private contenidodigitaldto DataRowToAsistenciaDetalleDTO(DataRow dr)
        {
            contenidodigitaldto dto = new contenidodigitaldto();

            if (!dr.IsNull("AgrupadorPadreID"))
                dto.asistenciaid = Convert.ToInt32(dr["AgrupadorPadreID"]);
            if (!dr.IsNull("Nombre"))
                dto.nombrecontenidodigital = dr["Nombre"].ToString();
            if (!dr.IsNull("ContenidoDigitalID"))
                dto.contenidodigitalid = Convert.ToInt16(dr["ContenidoDigitalID"]);
            if (!dr.IsNull("TipoDocumento"))
                dto.tipodocumento = dr["TipoDocumento"].ToString();
            return dto;
        }
        #endregion
    }
}
