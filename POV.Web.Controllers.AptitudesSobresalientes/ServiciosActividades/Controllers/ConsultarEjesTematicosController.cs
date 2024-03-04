using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using Framework.Base.DataAccess;
using Framework.Base.Entity;
using POV.ConfiguracionActividades.BO;
using POV.ContenidosDigital.Service;
using POV.Licencias.BO;
using POV.Licencias.Service;
using POV.Profesionalizacion.BO;
using POV.Profesionalizacion.Service;

namespace POV.ServiciosActividades.Controllers
{
    public class ConsultarEjesTematicosController
    {
        #region Conexion
        IDataContext dctx = new DataContext((new DataProviderFactory()).GetDataProvider("POV"));
        #endregion

        private EjeTematicoRef _ejeTematicoRefFiltro;

        /// <summary>
        /// Consulta las areas de profesionalizacion que se tienen en el sistema
        /// </summary>
        /// <param name="areaProfesionalizacion">criterios de la consulta</param>
        /// <returns>Conjunto de areas de profesionalizacion existentes</returns>
        public DataSet ConsultarAreaProfesionalizacion(AreaProfesionalizacion areaProfesionalizacion)
        {
            var areaProfesionalizacionCtrl = new AreaProfesionalizacionCtrl();

            var ds = areaProfesionalizacionCtrl.Retrieve(dctx, areaProfesionalizacion);

            return ds;
        }

        /// <summary>
        /// Método que consulta las materias a partir de una area de profesionalización
        /// </summary>
        /// <param name="areaProfesionalizacion">area de profesionalización que filtra las materias</param>
        /// <returns>Lista de materias que cumplen con el filtro proporcionado</returns>
        public List<MateriaProfesionalizacion> ConsultarMateriasPorArea(AreaProfesionalizacion areaProfesionalizacion)
        {
            var areaProfesionalizacionCtrl = new AreaProfesionalizacionCtrl();
            var areaComplete = areaProfesionalizacionCtrl.RetrieveComplete(dctx, areaProfesionalizacion);

            return areaComplete.MateriasProfesionalizacion.Where(x => x.Activo == true).ToList();

        }

        /// <summary>
        /// Consulta los ejes temáticos a partir de ciertos filtros porporcionados por el usuario
        /// </summary>
        /// <param name="contrato">Contrato al que pertenece el usuario que filtra</param>
        /// <param name="ejeFiltrosRef">datos de filtro de búsqueda de los ejes temáticos</param>
        /// <returns>Lista de ejes temáticos que cumplen con el criterio de búsqueda</returns>
        public List<EjeTematicoRef> ConsultarEjesTematicos(Contrato contrato, EjeTematicoRef ejeFiltrosRef)
        {
            var contratoCtrl = new ContratoCtrl();
            var listEjeReferencia = new List<EjeTematicoRef>();
            _ejeTematicoRefFiltro = ejeFiltrosRef;

            #region Obtenemos toda la información de los Ejes tematicos
            //Consultamos los ejes disponibles en el contrato
           var profesionalizacionContrato = contratoCtrl.RetrieveProfesionalizacionContrato(dctx, contrato);

            foreach (var ejeTematico in profesionalizacionContrato.ListaEjesTematicos)
            {
                var ejeTematicoCtrl = new EjeTematicoCtrl();
                var situacionAprendizajeCtrl = new SituacionAprendizajeCtrl();
                
                var ejeSemiComplete = ejeTematicoCtrl.RetrieveComplete(dctx, ejeTematico);
                
                if(ejeSemiComplete.AreaProfesionalizacion != null && ejeSemiComplete.AreaProfesionalizacion.Activo == true)
                {
                    ejeSemiComplete.SituacionesAprendizaje =situacionAprendizajeCtrl.RetrieveList(dctx, ejeTematico,
                        new SituacionAprendizaje());
                
                    foreach (var situacionAprendizaje in ejeSemiComplete.SituacionesAprendizaje)
                    {
                        var contenidoDitigalCtrl = new ContenidoDigitalCtrl();
                        var contenidoDigAgrupadorCtrl = new ContenidoDigitalAgrupadorCtrl();
                        var agrupadorCtrl = new AgrupadorContenidoDigitalCtrl();
                 

                        var dsContenidoDigitalAgrupador = contenidoDigAgrupadorCtrl.Retrieve(dctx,
                            new ContenidoDigitalAgrupador
                            {
                                SituacionAprendizaje =
                                    new SituacionAprendizaje
                                    {
                                        SituacionAprendizajeID = situacionAprendizaje.SituacionAprendizajeID
                                    },
                                EjeTematico = 
                                    new EjeTematico
                                    {
                                        EjeTematicoID = ejeSemiComplete.EjeTematicoID
                                    }
                            });

                   
                   
                        foreach (DataRow dataRow in dsContenidoDigitalAgrupador.Tables[0].Rows)
                        {
                            var contenidoDigitalAgrupador =
                                contenidoDigAgrupadorCtrl.DataRowToContenidoDigitalAgrupador(dataRow);
                            contenidoDigitalAgrupador = contenidoDigAgrupadorCtrl.RetrieveCompete(dctx,
                                contenidoDigitalAgrupador);

                       if (contenidoDigitalAgrupador == null)
                           continue;

                       contenidoDigitalAgrupador.ContenidoDigital = contenidoDitigalCtrl.RetrieveComplete(dctx,
                           contenidoDigitalAgrupador.ContenidoDigital);

                            var agrupador = agrupadorCtrl.RetrieveSimple(dctx,
                                contenidoDigitalAgrupador.AgrupadorContenidoDigital);
                            if(agrupador != null){
                                var ejeTematicoRef = new EjeTematicoRef
                                {
                                    EjeTematicoId = ejeTematico.EjeTematicoID,
                                    NombreEjeTematico = ejeTematico.Nombre,
                                    AreaProfesionalizacionId = ejeSemiComplete.AreaProfesionalizacion.AreaProfesionalizacionID,
                                    NombreArea = ejeSemiComplete.AreaProfesionalizacion.Nombre,
                                    SituacionAprendizajeId = situacionAprendizaje.SituacionAprendizajeID,
                                    Clasificador = agrupador.Nombre,
                                    NombreSituacion = situacionAprendizaje.Nombre,
                                    ContenidoDigitalId = contenidoDigitalAgrupador.ContenidoDigital.ContenidoDigitalID,
                                    NombreContenido = contenidoDigitalAgrupador.ContenidoDigital.Nombre,
                                    ClaveContenido = contenidoDigitalAgrupador.ContenidoDigital.Clave,
                                    TipoDocumentoId = contenidoDigitalAgrupador.ContenidoDigital.TipoDocumento.TipoDocumentoID,
                                    NombreTipoDocumento = contenidoDigitalAgrupador.ContenidoDigital.TipoDocumento.Nombre,
                                    ExtencionTipoDocumento = contenidoDigitalAgrupador.ContenidoDigital.TipoDocumento.Extension,
                                    MateriasList = new List<MateriaProfesionalizacion>(),
                                    Grado = ejeSemiComplete.AreaProfesionalizacion.Grado,
                                    Competencia = agrupador.Competencias,
                                    Aprendizaje = agrupador.Aprendizajes
                                };

                                foreach (var materia in ejeSemiComplete.MateriasProfesionalizacion)
                                {
                                    ejeTematicoRef.MateriasList.Add(materia);
                                }

                                listEjeReferencia.Add(ejeTematicoRef);
                            }
                        }
                    }
                }
            }
            #endregion

            var expre = QueryAction();

            List<EjeTematicoRef> filtrados = listEjeReferencia.Where(expre.Compile()).ToList();
            var resultEjesTematicos = new List<EjeTematicoRef>();

            foreach (var ejeTematicoRef in filtrados)
            {
                foreach (var materiaProfesionalizacion in ejeTematicoRef.MateriasList)
                {
                    if (ejeTematicoRef.NombreMaterias == null)
                    {
                        ejeTematicoRef.NombreMaterias = materiaProfesionalizacion.Nombre;
                    }
                    else
                    {
                        ejeTematicoRef.NombreMaterias = ejeTematicoRef.NombreMaterias + ", " +
                                                        materiaProfesionalizacion.Nombre;
                    }

                }
                resultEjesTematicos.Add(ejeTematicoRef);
                
            }

            return resultEjesTematicos;
        }

        /// <summary>
        /// Método para filtrar los ejes tematicos según los criterios de búsqueda
        /// </summary>
        /// <returns>Ejes tematicos que cumplen con los criterios de búsqueda</returns>
        Expression<Func<EjeTematicoRef, bool>> QueryAction()
        {
            Expression<Func<EjeTematicoRef, bool>> exp = x => true;

            if (!String.IsNullOrEmpty(_ejeTematicoRefFiltro.NombreEjeTematico))
                exp = exp.And(x => x.NombreEjeTematico != null && x.NombreEjeTematico.Contains(_ejeTematicoRefFiltro.NombreEjeTematico));

            if (!String.IsNullOrEmpty(_ejeTematicoRefFiltro.NombreSituacion))
                exp = exp.And(x => x.NombreSituacion != null && x.NombreSituacion.Contains(_ejeTematicoRefFiltro.NombreSituacion));

            if (!String.IsNullOrEmpty(_ejeTematicoRefFiltro.Competencia))
                exp = exp.And(x => x.Competencia != null && x.Competencia.Contains(_ejeTematicoRefFiltro.Competencia));

            if (!String.IsNullOrEmpty(_ejeTematicoRefFiltro.Aprendizaje))
                exp = exp.And(x => x.Aprendizaje != null && x.Aprendizaje.Contains(_ejeTematicoRefFiltro.Aprendizaje));

            if (_ejeTematicoRefFiltro.Grado != null)
                exp = exp.And(x => x.Grado == _ejeTematicoRefFiltro.Grado);
            
            if (_ejeTematicoRefFiltro.AreaProfesionalizacionId != null)
                exp = exp.And(x => x.AreaProfesionalizacionId == _ejeTematicoRefFiltro.AreaProfesionalizacionId);

            if (_ejeTematicoRefFiltro.MateriasList != null)
            {
                foreach (var materiaProfesionalizacion in _ejeTematicoRefFiltro.MateriasList)
                {
                    if (materiaProfesionalizacion.MateriaID != null)
                    {
                        exp =
                            exp.And(
                                x => x.MateriasList.Any(y => y.MateriaID == materiaProfesionalizacion.MateriaID));
                    }
                }
            }
                

            return exp;
        } 
    }
}
