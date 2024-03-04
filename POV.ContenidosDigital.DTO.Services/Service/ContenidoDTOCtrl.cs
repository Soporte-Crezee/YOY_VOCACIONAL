using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.ContenidosDigital.DTO.BO;
using POV.ContenidosDigital.Busqueda.Service;
using POV.Licencias.BO;
using POV.Core.RedSocial.Interfaces;
using POV.Core.RedSocial.Implement;
using POV.Profesionalizacion.Service;
using POV.Profesionalizacion.BO;
using POV.Profesionalizacion.DTO.BO;

namespace POV.ContenidosDigital.DTO.Service
{
    /// <summary>
    /// Controlador del objeto ContenidoDTO
    /// </summary>
    public class ContenidoDTOCtrl
    {
        private IUserSession userSession;
        private IDataContext dctx;

        public ContenidoDTOCtrl()
        {
            userSession = new UserSession();
            dctx = new DataContext((new DataProviderFactory()).GetDataProvider("POV"));
        }
        /// <summary>
        /// Consulta registros de ContenidoDTO en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="contenidoDTO">ContenidoDTO que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de ContenidoDTO generada por la consulta</returns>
        public List<contenidodto> SearchContenidosDigitales(contenidoinputdto inputdto)
        {
            List<contenidodto> lsContenidoDTO = new List<contenidodto>();

            Contrato contrato = userSession.Contrato;

            if (contrato != null && contrato.ContratoID != null)
            {
                PalabraClaveContenidoCtrl palabraClaveContenidoCtrl = new PalabraClaveContenidoCtrl();
                EjeTematicoCtrl ejeTematicoCtrl = new EjeTematicoCtrl();
                #region Variables
                // Valores por Default
                int pageSize = 10;
                int currentPage = 1;
                String sortColumn = "NombreContenido";
                String sortOrder = "ASC";
                Dictionary<string, string> parameters = null;
                #endregion
                if (inputdto != null)
                {
                    if (inputdto.pagesize != null && inputdto.pagesize > 0)
                        pageSize = inputdto.pagesize.Value;
                    if (inputdto.currentpage != null && inputdto.currentpage > 0)
                        currentPage = inputdto.currentpage.Value;
                    if (inputdto.sort != null && inputdto.sort.Trim() != string.Empty)
                        sortColumn = inputdto.sort.Trim();
                    if (inputdto.order != null && inputdto.order.Trim() != string.Empty)
                        sortOrder = inputdto.order.Trim();
                }

                parameters = new Dictionary<string, string>();
                parameters.Add("ContratoID", contrato.ContratoID.ToString());
                parameters.Add("NombreContenido", inputdto.nombrecontenido);

                DataSet dsContenido;
                dsContenido = palabraClaveContenidoCtrl.RetrieveContenidoPorPalabraClave(this.dctx, pageSize, currentPage, sortColumn, sortOrder, parameters);

                if (dsContenido != null & dsContenido.Tables.Count > 0)
                {
                    contenidodto dto;
                    foreach (DataRow row in dsContenido.Tables[0].Rows)
                    {
                        dto = this.DataRowToContenidoDTO(row);
                        EjeTematico eje = ejeTematicoCtrl.RetrieveComplete(this.dctx,
                                                                           new EjeTematico()
                                                                               {
                                                                                   EjeTematicoID = dto.ejetematicoid
                                                                               });
                        if(eje.AreaProfesionalizacion != null && eje.AreaProfesionalizacion.Activo == true)
                            lsContenidoDTO.Add(dto);
                    }
                }
            }

            return lsContenidoDTO;
        }



        /// <summary>
        /// Crea un objeto de ContenidoDTO a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de ContenidoDTO</param>
        /// <returns>Un objeto de ContenidoDTO creado a partir de los datos</returns>
        public contenidodto LastDataRowToContenidoDTO(DataSet ds)
        {
            if (!ds.Tables.Contains("ContenidoDigital"))
                throw new Exception("LastDataRowToContenidoDTO: DataSet no tiene la tabla ContenidoDigital");
            int index = ds.Tables["ContenidoDigital"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToContenidoDTO: El DataSet no tiene filas");
            return this.DataRowToContenidoDTO(ds.Tables["ContenidoDigital"].Rows[index - 1]);
        }
        /// <summary>
        /// Crea un objeto de ContenidoDTO a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de ContenidoDTO</param>
        /// <returns>Un objeto de ContenidoDTO creado a partir de los datos</returns>
        public contenidodto DataRowToContenidoDTO(DataRow row)
        {
            contenidodto dto = new contenidodto();
            if (row.IsNull("ContenidoID"))
                dto.contenidoid = null;
            else
                dto.contenidoid = (long)Convert.ChangeType(row["ContenidoID"], typeof(long));
            if (row.IsNull("NombreContenido"))
                dto.nombrecontenido = null;
            else
                dto.nombrecontenido = (string)Convert.ChangeType(row["NombreContenido"], typeof(string));
            if (row.IsNull("InstitucionOrigen"))
                dto.institucionorigen = null;
            else
                dto.institucionorigen = (string)Convert.ChangeType(row["InstitucionOrigen"], typeof(string));
            if (row.IsNull("TipoDocumento"))
                dto.tipodocumento = null;
            else
                dto.tipodocumento = (string)Convert.ChangeType(row["TipoDocumento"], typeof(string));
            if (row.IsNull("ImagenDocumento"))
                dto.imagendocumento = null;
            else
                dto.imagendocumento = (string)Convert.ChangeType(row["ImagenDocumento"], typeof(string));
            if (row.IsNull("Etiquetas"))
                dto.etiquetas = null;
            else
                dto.etiquetas = (string)Convert.ChangeType(row["Etiquetas"], typeof(string));
            if (row.IsNull("SituacionAprendizajeID"))
                dto.situacionaprendizajeid = null;
            else
                dto.situacionaprendizajeid = (long)Convert.ChangeType(row["SituacionAprendizajeID"], typeof(long));
            if (row.IsNull("NombreSituacion"))
                dto.nombresituacion = null;
            else
                dto.nombresituacion = (string)Convert.ChangeType(row["NombreSituacion"], typeof(string));
            if (row.IsNull("EjeTematicoID"))
                dto.ejetematicoid = null;
            else
                dto.ejetematicoid = (long)Convert.ChangeType(row["EjeTematicoID"], typeof(long));
            return dto;
        }
		/// <summary>
        /// Consulta registros de ContenidoDTO en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="contenidoDTO">ContenidoDTO que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de ContenidoDTO generada por la consulta</returns>
        public List<contenidodto> ConsultarSituacionContenido(agrupadorinputdto dto)
        {
            List<contenidodto> outputcontenido = new List<contenidodto>();
            AgrupadorContenidoDigitalCtrl agrupadorContenidoCtrl = new AgrupadorContenidoDigitalCtrl();
            AAgrupadorContenidoDigital agrupador;
            if(dto.tipoAgrupador == 1){
               agrupador = new AgrupadorCompuesto();
               agrupador.AgrupadorContenidoDigitalID = dto.agrupadorID;
            }
            else{
                agrupador = new AgrupadorSimple();
                agrupador.AgrupadorContenidoDigitalID = dto.agrupadorID;
            }
            
            dto.sort = "agrupador.AgrupadorContenidoDigitalID";
            dto.order = "ASC";
            DataSet contenidosDs = new DataSet();
            contenidosDs = agrupadorContenidoCtrl.Retrive(dctx, agrupador, dto.pagesize.Value, dto.currentpage.Value, dto.sort, dto.order, userSession.Contrato.ContratoID.Value);
            foreach(DataRow contenidosDr in contenidosDs.Tables[0].Rows){
                contenidodto contenidodto = new contenidodto();
                contenidodto.ejetematicoid = dto.ejeID;
                if (contenidosDr.IsNull("ContenidoDigitalID"))
                    contenidodto.contenidoid = null;
                else
                {
                    contenidodto.contenidoid = (Int64)Convert.ChangeType(contenidosDr["ContenidoDigitalID"], typeof(Int64));
                    contenidodto.nombrecontenido = (String)Convert.ChangeType(contenidosDr["NombreContenido"], typeof(String));
                    contenidodto.etiquetas = (String)Convert.ChangeType(contenidosDr["Tags"], typeof(String));
                    
                }
                if (contenidosDr.IsNull("TipoDocumentoID"))
                    contenidodto.contenidoid = null;
                else
                {
                    contenidodto.tipodocumento = (String)Convert.ChangeType(contenidosDr["NombreTipo"], typeof(String));
                    contenidodto.institucionorigen = (String)Convert.ChangeType(contenidosDr["InstitucionOrigen"], typeof(String));
                    contenidodto.imagendocumento = (String)Convert.ChangeType(contenidosDr["ImagenDocumento"], typeof(String));
                }
                outputcontenido.Add(contenidodto);
            }
            return outputcontenido;
        }
		
    }
}
