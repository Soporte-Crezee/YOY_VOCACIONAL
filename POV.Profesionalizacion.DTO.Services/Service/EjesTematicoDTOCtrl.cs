using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Framework.Base.DataAccess;
using POV.ContenidosDigital.BO;
using POV.Profesionalizacion.BO;
using POV.Profesionalizacion.Service;
using POV.Profesionalizacion.DTO.BO;
using POV.Licencias.BO;
using POV.Licencias.Service;
using POV.Core.RedSocial.Implement;
using POV.Core.RedSocial.Interfaces;

namespace POV.Profesionalizacion.DTO.Service
{
    public class EjesTematicoDTOCtrl
    {        
        private IUserSession userSession;
        private IDataContext dctx;
        
        public EjesTematicoDTOCtrl()
        {
            userSession = new UserSession();
            dctx = new DataContext((new DataProviderFactory()).GetDataProvider("POV"));            
        }
   
        public List<ejetematicodto> SearchEjesTematico(ejetematicoinputdto dto)
        {
            List<ejetematicodto> lsejetematicodto = new List<ejetematicodto>();

            if (!userSession.IsAlumno())
            {
                EjeTematicoCtrl ejeCtrl = new EjeTematicoCtrl();                
                #region Variables                          
                int pageSize;
                int currentPage;
                String filterEjes = "EjeTematicoID";
                String sortOrder = "asc";                
                Dictionary<string, string> parameters = new Dictionary<string,string>();
                #endregion
                
                if (dto.pagesize == null || dto.pagesize <= 0)
                    pageSize = 10;
                else
                    pageSize = (int)dto.pagesize;

                if (dto.currentpage == null || dto.currentpage <= 0)
                    currentPage = 1;
                else
                    currentPage = (int)dto.currentpage;
                                
                if (!String.IsNullOrEmpty(dto.nombreeje))
                {
                    parameters.Add("NombreEjeTematico", "%" + dto.nombreeje + "%");
                }
                if (dto.nivelid != null)
                {
                    parameters.Add("NivelEducativoID", dto.nivelid.ToString());
                }
                if (dto.grado != null)
                {
                    parameters.Add("Grado", dto.grado.ToString());
                }
                if (dto.areaid != null)
                {
                    parameters.Add("AreaProfesionalizacionID", dto.areaid.ToString());
                }
                if (dto.materiaid != null)
                {
                    parameters.Add("MateriaID", dto.materiaid.ToString());
                }

                parameters.Add("AreaProfesionalizacionActivo", "true");

                parameters.Add("ContratoID", userSession.Contrato.ContratoID.ToString());               
                //consulta de los ejes                                
                DataSet ds = ejeCtrl.RetrieveEjesTematicos(dctx, pageSize, currentPage, filterEjes, sortOrder, parameters);
                if (ds.Tables.Contains("EjeTematico") && ds.Tables["EjeTematico"].Rows.Count >= 1)
                {
                    AreaProfesionalizacionCtrl areaCtrl = new AreaProfesionalizacionCtrl();
                    int currentEjeTematicoID = 0;
                    foreach (DataRow dr in ds.Tables["EjeTematico"].Rows)
                    {
                        if (!dr.IsNull("EjeTematicoID") && !dr.IsNull("EstatusEjeTematico") && currentEjeTematicoID.ToString() != dr["EjeTematicoID"].ToString())
                        {                                                       
                            EjeTematico ejeTema = new EjeTematico()
                            {
                                EjeTematicoID = int.Parse(dr["EjeTematicoID"].ToString()),
                                Nombre = dr["NombreEjeTematico"].ToString(),
                                EstatusProfesionalizacion = (EEstatusProfesionalizacion)byte.Parse(dr["EstatusEjeTematico"].ToString()),
                                AreaProfesionalizacion = new AreaProfesionalizacion(){Nombre = dr["NombreArea"].ToString()}                                
                            };
                            //consulta de los materias del eje
                            ejeTema.MateriasProfesionalizacion = ejeCtrl.RetrieveMateriaEjeTematico(dctx, ejeTema, new MateriaProfesionalizacion { Activo = true });
                            ejetematicodto ejedto = EjeToDto(ejeTema);
                            
                            ejetematicoinputdto dtoFilterSituaciones = new ejetematicoinputdto();
                            dtoFilterSituaciones.ejetematicoid = ejedto.ejetematicoid;
                            dtoFilterSituaciones.pagesize = 1000;
                            dtoFilterSituaciones.currentpage = 1;
                            dtoFilterSituaciones.nombretema = dto.nombretema;
                            dtoFilterSituaciones.competencia = dto.competencia;
                            dtoFilterSituaciones.aprendizaje = dto.aprendizaje;
                            ejedto.situacionoutputdto = GetInformacionEjeTematico(dtoFilterSituaciones);
                            if (ejedto.situacionoutputdto != null && ejedto.situacionoutputdto.situaciones.Count > 0){
                                lsejetematicodto.Add(ejedto);
                            }
                            currentEjeTematicoID = int.Parse(dr["EjeTematicoID"].ToString());
                        }
                    }
                }

            }

            return lsejetematicodto;
        }

        private EjeTematico DtoInputToEje(ejetematicoinputdto dto)
        {
            EjeTematico ej = new EjeTematico();
            if (dto.ejetematicoid != null)
            {
                ej.EjeTematicoID = dto.ejetematicoid;            
            }

            if (!string.IsNullOrEmpty(dto.nombreeje))
                ej.Nombre = dto.nombreeje.Trim();

            if (!string.IsNullOrEmpty(dto.nombrearea))
                ej.AreaProfesionalizacion.Nombre = dto.nombrearea.Trim();            

            return ej;
        }

        private ejetematicodto EjeToDto(EjeTematico ejetematico)
        {
            ejetematicodto dto = new ejetematicodto();
            
            if (ejetematico == null)
                return null;

            if (ejetematico.EjeTematicoID != null)
                dto.ejetematicoid = ejetematico.EjeTematicoID;

            if (!string.IsNullOrEmpty(ejetematico.Nombre))
            {
                dto.nombreejetematico = ejetematico.Nombre;
                if (dto.nombreejetematico.Length > 75)
                    dto.nombreejetematico = dto.nombreejetematico.Substring(0, 75) + "...";
            }
                
            if (!string.IsNullOrEmpty(ejetematico.AreaProfesionalizacion.Nombre))
                dto.nombrearea = ejetematico.AreaProfesionalizacion.Nombre;


            if (ejetematico.MateriasProfesionalizacion.Count() > 0)
            {
                foreach (MateriaProfesionalizacion materia in ejetematico.MateriasProfesionalizacion)
                {
                    dto.nombremateria += ", " + materia.Nombre;
                }

                dto.nombremateria = dto.nombremateria.Substring(2);   
            }
                                                
            return dto;
        }
        /// <summary>
        /// Obtiene las situaciones de aprendizaje que pertenecen a un eje temático y que están activas
        /// </summary>
        /// <param name="dto">El dto que proporciona los parametros de consulta</param>
        /// <returns>un dto que contiene la informacion de las situaciones de aprendizaje que cumplen los criterios de consulta</returns>
        public situacionaprendizajeoutputdto GetInformacionEjeTematico(ejetematicoinputdto dto)
        {
            ProfesionalizacionContrato ejesTematicosContrato = new ProfesionalizacionContrato();
            situacionaprendizajeoutputdto result = new situacionaprendizajeoutputdto();
            List<situacionaprendizajeoutputdto> situacionesdto = new List<situacionaprendizajeoutputdto>();

            ContratoCtrl ctrl = new ContratoCtrl();
            EjeTematicoCtrl ejeTematicoCtrl = new EjeTematicoCtrl();
            SituacionAprendizajeCtrl ctrlSituacionAprendizaje = new SituacionAprendizajeCtrl();
            AgrupadorContenidoDigitalCtrl agrupadorCtrl = new AgrupadorContenidoDigitalCtrl();

            Dictionary<string, string> parametros = new Dictionary<string, string>();
            dto.sort = "Nombre";
            dto.order = "ASC";

            if (dto == null)
                throw new Exception("No puede ser nulo el dto");
            if (dto.ejetematicoid == null)
                throw new Exception("No puede ser nulo el ejetematicoid");

            long ejeTematicoID = 0;
            bool resultValidate = long.TryParse(dto.ejetematicoid.ToString(), out ejeTematicoID);

            result.ejetematicoid = ejeTematicoID;
            result.situaciones = new List<situacionaprendizajedto>();
            result.currentpage = dto.currentpage;
            
            if (!resultValidate)
                throw new Exception("El ejetematicoid no es numérico");
            parametros.Add("EjeTematicoID", ejeTematicoID.ToString());

            Byte estatus =Convert.ToByte(EEstatusProfesionalizacion.ACTIVO);
            parametros.Add("EstatusSituacion", estatus.ToString());

            parametros.Add("Nombre", "%" + dto.nombretema + "%");

            if (userSession == null)
            {
                throw new Exception("No hay sesion de usuario");
            }
            if (!userSession.IsAlumno())
            {
                if (userSession.Contrato == null)
                {
                    throw new Exception("No hay contrato en la sesion");
                }
                if (userSession.Contrato.ContratoID == null)
                {
                    throw new Exception("Identificador del Contrato es nulo");
                }
                parametros.Add("ContratoID", userSession.Contrato.ContratoID.ToString());
            }
            else return result;

            ejesTematicosContrato = ctrl.RetrieveProfesionalizacionContrato(dctx, userSession.Contrato);
            var resultado = ejesTematicosContrato.ListaEjesTematicos.Where(r => r.EjeTematicoID == ejeTematicoID).ToList();

            if (resultado == null)
            {
                return result;
            }
            if (resultado.Count == 0)
            {
                return result;
            }

            EjeTematico ejeTematico = ejeTematicoCtrl.RetrieveComplete(dctx, new EjeTematico() { EjeTematicoID = ejeTematicoID });
            if (ejeTematico == null)
                throw new Exception("La consulta de eje temático con identificador no trajo resultados");
            if (ejeTematico.EstatusProfesionalizacion == EEstatusProfesionalizacion.ACTIVO)
            {
                DataSet ds = ctrlSituacionAprendizaje.RetrieveComplete(dctx, (int)dto.pagesize, (int)dto.currentpage, dto.sort, dto.order, parametros);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    AgrupadorCompuesto agrupadorCompuesto = new AgrupadorCompuesto();
                    if (!dr.IsNull("AgrupadorContenidoDigitalID")){
                        agrupadorCompuesto = (AgrupadorCompuesto)agrupadorCtrl.RetrieveComplete(dctx, new AgrupadorCompuesto() { AgrupadorContenidoDigitalID = Convert.ToInt64(dr["AgrupadorContenidoDigitalID"])});
                        List<AAgrupadorContenidoDigital> listAgrupadores = new List<AAgrupadorContenidoDigital>();
                        listAgrupadores = agrupadorCompuesto.AgrupadoresContenido;
                        if(!String.IsNullOrEmpty(dto.competencia))
                        {
                            listAgrupadores = listAgrupadores.Where(r => r.Competencias != null && r.Competencias.ToUpper().Contains(dto.competencia.ToUpper())).ToList();
                        }
                        if(!String.IsNullOrEmpty(dto.aprendizaje))
                        {
                            listAgrupadores = listAgrupadores.Where(r => r.Aprendizajes != null && r.Aprendizajes.ToUpper().Contains(dto.aprendizaje.ToUpper())).ToList();
                        }

                        if (listAgrupadores.Count < agrupadorCompuesto.AgrupadoresContenido.Count)
                        {
                            agrupadorCompuesto.AgrupadoresContenido.Clear();
                            foreach (var aAgrupadorContenidoDigital in listAgrupadores)
                            {
                                agrupadorCompuesto.AgrupadoresContenido.Add(aAgrupadorContenidoDigital);
                                
                            }
                        }
                    }

                    if(agrupadorCompuesto.AgrupadoresContenido.Count > 0){
                        result.situaciones.Add(DataRowToSituacionAprendizajeDTO(dr, agrupadorCompuesto));
                    }
                }
            }
            return result;
        }
        private situacionaprendizajedto DataRowToSituacionAprendizajeDTO(DataRow dr, AgrupadorCompuesto agrupador)
        {
            situacionaprendizajedto dto = new situacionaprendizajedto();
            if (!dr.IsNull("EjeTematicoID"))
                dto.ejetematicoid = Convert.ToInt64(dr["EjeTematicoID"]);

            if (!dr.IsNull("SituacionAprendizajeID"))
                dto.situacionid = Convert.ToInt64(dr["SituacionAprendizajeID"]);

            if (!dr.IsNull("SituacionAprendizaje"))
            {
                dto.nombresituacion = dr["SituacionAprendizaje"].ToString();
                
            }
                
            if (!dr.IsNull("Descripcion"))
            {
                dto.descripcionsituacion = dr["Descripcion"].ToString();
                if (dto.descripcionsituacion.Length > 200)
                    dto.descripcionsituacion = dto.descripcionsituacion.Substring(0, 197) + "...";
            }

            if (agrupador != null)
            {
                dto.agrupadorcontenido = new agrupadoroutputdto();
                dto.agrupadorcontenido.agrupadorID = agrupador.AgrupadorContenidoDigitalID;
                dto.agrupadorcontenido.tipoAgrupador = (int) agrupador.TipoAgrupador;

                dto.agrupadorcontenido.agrupadores = new List<agrupadordto>();
                foreach (AgrupadorSimple agrupadorSimple in agrupador.AgrupadoresContenido)
                {
                    agrupadordto newagrupador = new agrupadordto();
                    newagrupador.ejeid = dto.ejetematicoid;
                    newagrupador.agrupadorid = agrupadorSimple.AgrupadorContenidoDigitalID;
                    newagrupador.nombre = agrupadorSimple.Nombre;
                    newagrupador.competencias = agrupadorSimple.Competencias;
                    newagrupador.aprendizajes = agrupadorSimple.Aprendizajes;
                    newagrupador.contenidosdigitales = new List<contenidodigitaldto>();
                    foreach (ContenidoDigital contenidoDigital in agrupadorSimple.ContenidosDigitales)
                    {
                        newagrupador.contenidosdigitales.Add(new contenidodigitaldto()
                            {
                                contenidodigitalid = contenidoDigital.ContenidoDigitalID,
                                nombrecontenidodigital = contenidoDigital.Nombre
                            }
                        );
                    }
                    dto.agrupadorcontenido.agrupadores.Add(newagrupador);
                }
            }
            return dto;
        }

    }
}
