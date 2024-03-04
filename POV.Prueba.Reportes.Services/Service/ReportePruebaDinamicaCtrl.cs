using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Modelo.Service;
using POV.Prueba.Diagnostico.Dinamica.Service;
using POV.CentroEducativo.BO;
using Framework.Base.DataAccess;
using POV.Prueba.Diagnostico.Dinamica.BO;
using System.Data;
using POV.Prueba.Reportes.BO;
using POV.Prueba.Diagnostico.Dinamica.DA;
using POV.Modelo.BO;
using POV.Prueba.Reportes.BO;
using POV.Prueba.BO;

namespace POV.Prueba.Reportes.Service
{
    public class ReportePruebaDinamicaCtrl
    {
        /// <summary>
        /// Obtiene una lista de resultados de prueba. Cada prueba tiene una lista de grupos con sus resultados por clasificador
        /// </summary>
        /// <param name="dctx">Datacontext para acceder a la conexion</param>
        /// <param name="escuela">escuela que tiene esos resultados de prueba</param>
        /// <param name="prueba">la prueba que se utilizará para consultar los resultados</param>
        /// <param name="grupoCicloEscolar">Filtro para consultar resultados por grupo</param>
        /// <returns>Lista de resultados de prueba. Detalles de la escuela, la prueba, y los resultados de los alumnos y su calificacion en el clasificador</returns>
        public PruebaDinamicaDetail RetrieveResultadPruebaDinamicaEscuela(IDataContext dctx, Escuela escuela,PruebaDinamica prueba,GrupoCicloEscolar grupoCicloEscolar ) {

            PruebaDinamicaCtrl pruebaDinamicaCtrl = new PruebaDinamicaCtrl();
             prueba = pruebaDinamicaCtrl.RetrieveComplete(dctx, prueba, true);

            ModeloDinamico modelo= prueba.Modelo as ModeloDinamico;
            if (modelo == null)
                throw new Exception("ReportePruebaDinamicaCtrl: La prueba no tiene un modelo dinámico ");

            
            ResultadoPruebaDinamicaCtrl srv = new ResultadoPruebaDinamicaCtrl();
            DataSet dsResultadoPruebaDinamica = srv.RetrieveResultadoPruebaDinamicaEscuela(dctx, escuela, prueba, grupoCicloEscolar);
          
             
            PruebaDinamicaDetail pruebaDinamicaDetail = new PruebaDinamicaDetail()
                {
                Nombre = prueba.Nombre,
                Clave= prueba.Clave,
                Metodo = modelo.MetodoCalificacion.ToString(),
                Modelo = prueba.Modelo.Nombre,
                ListaResultadoDinamicaGrupoDetail = new List<BO.ResultadoDinamicaGrupoDetail>()                
            };

            if (dsResultadoPruebaDinamica.Tables[0].Rows.Count <= 0)
                return null;

          
            
            List<ResultadoDinamicaGrupoDetail> listaDinamicaGrupoDetail= new List<ResultadoDinamicaGrupoDetail>();
           
            var tableGroup = dsResultadoPruebaDinamica.Tables[0];
            var grupos = from row in tableGroup.AsEnumerable() group row by new {TipoTurno = row.Field<Byte>("Turno"), Grupo = row.Field<String>("GrupoNombre"),GrupoCicloEscolarID= row.Field<Guid>("GrupoCicloEscolarID"), Ciclo = row.Field<String>("CicloEscolarTitulo"), Escuela = row.Field<String>("NombreEscuela") } into resultadosGrupo select new ResultadoDinamicaGrupoDetail { Grupo = resultadosGrupo.Key.Grupo, Escuela = resultadosGrupo.Key.Escuela, Ciclo = resultadosGrupo.Key.Ciclo, TipoTurno= resultadosGrupo.Key.TipoTurno, GrupoCicloEscolarID = resultadosGrupo.Key.GrupoCicloEscolarID};
            EnumerableRowCollection<DataRow> rowsFromDataSet = dsResultadoPruebaDinamica.Tables[0].AsEnumerable();
            foreach (var grupo in grupos) {

                if (grupo.TipoTurno == (byte)ETurno.MATUTINO)
                    grupo.Turno = ETurno.MATUTINO.ToString();
                else if (grupo.TipoTurno == (byte)ETurno.NOCTURNO)
                    grupo.Turno = ETurno.NOCTURNO.ToString();
                else if (grupo.TipoTurno == (byte)ETurno.DISCONTINUO)
                    grupo.Turno = ETurno.DISCONTINUO.ToString();
                else if (grupo.TipoTurno == (byte)ETurno.VESPERTINO)
                    grupo.Turno = ETurno.VESPERTINO.ToString();
                    
                    
                    EnumerableRowCollection<DataRow> rowsGrupo   = rowsFromDataSet.Where(x => x.Field<Guid>("GrupoCicloEscolarID") == grupo.GrupoCicloEscolarID);
                    ResultadoDinamicaGrupoDetail resultaGrupoDetail = grupo;
                    resultaGrupoDetail.ListaResultadoClasificadorDetail = new List<ResultadoClasificadorDetail>();
                    foreach (DataRow dr in rowsGrupo) {
                        ResultadoClasificadorDetail resultadoClasificadorDetail = new ResultadoClasificadorDetail();
                        if (modelo.MetodoCalificacion == EMetodoCalificacion.CLASIFICACION)
                        {
                            if ((dr.IsNull("NombreClasificadorDC") || dr.IsNull("EscalaDC_Nombre") || dr.IsNull("DetResulC_PuntajeID")))
                                resultadoClasificadorDetail = null;

                            if (!dr.IsNull("DetResulC_PuntajeID"))
                                resultadoClasificadorDetail.PuntajeID = Convert.ToInt32(dr["DetResulC_PuntajeID"]);
                            if (!dr.IsNull("NombreClasificadorDC"))
                                resultadoClasificadorDetail.Clasificador = dr["NombreClasificadorDC"].ToString();
                            if (!dr.IsNull("EscalaDC_Nombre"))
                                resultadoClasificadorDetail.Nombre = dr["EscalaDC_Nombre"].ToString();

                        }
                        else if (modelo.MetodoCalificacion == EMetodoCalificacion.SELECCION)
                        {
                            if (dr.IsNull("NombreClasificadorDS") || dr.IsNull("EscalaDS_Nombre") || dr.IsNull("DetResulS_PuntajeID"))
                            {
                                resultadoClasificadorDetail = null;
                            }
                            if (!dr.IsNull("DetResulS_PuntajeID"))
                                resultadoClasificadorDetail.PuntajeID = Convert.ToInt32(dr["DetResulS_PuntajeID"]);

                            if (!dr.IsNull("NombreClasificadorDS"))
                                resultadoClasificadorDetail.Clasificador = dr["NombreClasificadorDS"].ToString();
                            if (!dr.IsNull("EscalaDS_Nombre"))
                                resultadoClasificadorDetail.Nombre = dr["EscalaDS_Nombre"].ToString();
                        }
                        else if (modelo.MetodoCalificacion == EMetodoCalificacion.PUNTOS || modelo.MetodoCalificacion == EMetodoCalificacion.PORCENTAJE)
                        {
                            if (!dr.IsNull("NombreClasificadorDI"))
                            {
                                resultadoClasificadorDetail.Clasificador = dr["NombreClasificadorDI"].ToString();
                                resultadoClasificadorDetail.Nombre = dr["NombreClasificadorDI"].ToString();

                            }
                            if (!dr.IsNull("PuntajeID"))
                            {
                                resultadoClasificadorDetail.PuntajeID = Convert.ToInt32(dr["PuntajeID"]);
                            }

                            else resultadoClasificadorDetail = null;
                        }
                        if (!dr.IsNull("NumeroAlumnos"))
                        {
                            if (resultadoClasificadorDetail != null)
                                resultadoClasificadorDetail.Numero = Convert.ToInt32(dr["NumeroAlumnos"]);

                        }
                        if (!dr.IsNull("GrupoNombre"))
                        {
                            string grupoCicloEscolarID = dr["GrupoNombre"].ToString();

                            if (resultadoClasificadorDetail != null)
                                 resultaGrupoDetail.ListaResultadoClasificadorDetail.Add(resultadoClasificadorDetail);
                                
                        }
                    
                    }

                  resultaGrupoDetail.ListaResultadoClasificadorDetail = AddEscalasSinResultado(resultaGrupoDetail, prueba);
                  resultaGrupoDetail.ListaResultadoClasificadorDetail =  resultaGrupoDetail.ListaResultadoClasificadorDetail.OrderBy(x => x.Clasificador).ToList<ResultadoClasificadorDetail>();
                  if (resultaGrupoDetail.ListaResultadoClasificadorDetail != null && resultaGrupoDetail.ListaResultadoClasificadorDetail.Count > 0)
                  {                      
                      listaDinamicaGrupoDetail.Add(resultaGrupoDetail);
                  }
               if(listaDinamicaGrupoDetail!=null && listaDinamicaGrupoDetail.Count>0)
                pruebaDinamicaDetail.ListaResultadoDinamicaGrupoDetail = listaDinamicaGrupoDetail;                  
            }

            if (pruebaDinamicaDetail.ListaResultadoDinamicaGrupoDetail == null)
                return null;
            if (pruebaDinamicaDetail.ListaResultadoDinamicaGrupoDetail.Count == 0)
                return null;
           

            return pruebaDinamicaDetail;
        }

        private List<ResultadoClasificadorDetail> AddEscalasSinResultado(ResultadoDinamicaGrupoDetail resultadoDinamicaGrupoDetail,PruebaDinamica prueba)
        {
            List<ResultadoClasificadorDetail> lista = resultadoDinamicaGrupoDetail.ListaResultadoClasificadorDetail;
            foreach (AEscalaDinamica escala in prueba.ListaPuntajes) {

              ResultadoClasificadorDetail existe=    lista.Find(x => x.PuntajeID == escala.PuntajeID);
              if (existe == null) { 
                    lista.Add(new ResultadoClasificadorDetail(){ Numero = 0, Clasificador = escala.Clasificador.Nombre, Nombre = escala.Nombre, PuntajeID = escala.PuntajeID});
              }
                
            }
            return lista;
        }

        /// <summary>
        /// Retrieve de resultados de prueba modelo.
        /// </summary>
        /// <param name="dctx">Proveedor de datos.</param>
        /// <param name="prueba">Prueba proporcionada.</param>
        /// <param name="escuela">Escuela proporcionada.</param>
        /// <param name="gCicloEscolar">Ciclo escolar con el grupo proporcionado.</param>
        /// <returns>Regresa los resultados de la prueba con los datos del alumno y la escuela.</returns>
        public PruebaDinamicaDetail RetrieveListaResultadoPruebaGrupo(IDataContext dctx, PruebaDinamica prueba,
                                                                             Escuela escuela, GrupoCicloEscolar gCicloEscolar)
        {
            #region Variables.
            PruebaDinamicaDetail pruebaDinamicaDetail = new PruebaDinamicaDetail();
            ModeloDinamico modelo = prueba.Modelo as ModeloDinamico;
            pruebaDinamicaDetail.ListaResultadoDinamicaGrupoDetail = new List<ResultadoDinamicaGrupoDetail>();
            ResultadoDinamicaGrupoDetail escueladatos = new ResultadoDinamicaGrupoDetail();
            escueladatos.ListaResultadoAlumnoDetail = new List<ResultadoAlumnoDetail>();
            
            escueladatos.ListaResultadoClasificadorDetail = new List<ResultadoClasificadorDetail>();

            ResultadoPruebaDinamicaCtrl da = new ResultadoPruebaDinamicaCtrl();
            DataSet ds = new DataSet();
            DataSet dsResultadoPruebaDinamica = new DataSet();
            
            #endregion

            #region Validaciones.
            if (prueba == null) throw new Exception("ReportePruebaDinamicaCtrl: La prueba no puede ser vacía.");
            if (modelo == null) throw new Exception("ReportePruebaDinamicaCtrl: La prueba no tiene modelo ");
            if (escuela == null) throw new Exception("ReportePruebaDinamicaCtrl: La escuela no puede ser vacía.");
            if (gCicloEscolar.Grupo == null) throw new Exception("ReportePruebaDinamicaCtrl: El grupo no puede ser vacío.");
            if (gCicloEscolar == null) throw new Exception(" ReportePruebaDinamicaCtrl: El ciclo escolar no puede ser vacío.");
            #endregion

            ds = da.RetrieveResultadoPruebaDinamicaGrupo(dctx, escuela, gCicloEscolar, prueba);
            if (ds.Tables["ResultadoPruebaDinamicaGrupo"].Rows.Count > 0)
            {
                foreach (DataRow item in ds.Tables["ResultadoPruebaDinamicaGrupo"].Rows)
                {
                    ResultadoAlumnoDetail alumnodatos = new ResultadoAlumnoDetail();
                    //Datos de la prueba.
                    pruebaDinamicaDetail.Clave = item["PruebaClave"].ToString();
                    pruebaDinamicaDetail.Nombre = item["PruebaNombre"].ToString();
                    pruebaDinamicaDetail.Modelo = string.IsNullOrEmpty(item["NombreModelo"].ToString()) ? "" : item["NombreModelo"].ToString();
                    pruebaDinamicaDetail.Metodo = string.IsNullOrEmpty(item["MetodoCalificacion"].ToString()) ? "" : item["MetodoCalificacion"].ToString();
                    //Datos de la escuela.
                    escueladatos.Escuela = item["NombreEscuela"].ToString();
                    if (Byte.Parse(item["Turno"].ToString()) == (byte)ETurno.MATUTINO)
                        escueladatos.Turno = ETurno.MATUTINO.ToString();
                    else if (Byte.Parse(item["Turno"].ToString()) == (byte)ETurno.NOCTURNO)
                        escueladatos.Turno = ETurno.NOCTURNO.ToString();
                    else if (Byte.Parse(item["Turno"].ToString()) == (byte)ETurno.DISCONTINUO)
                        escueladatos.Turno = ETurno.DISCONTINUO.ToString();
                    else if (Byte.Parse(item["Turno"].ToString()) == (byte)ETurno.VESPERTINO)
                        escueladatos.Turno = ETurno.VESPERTINO.ToString();
                    escueladatos.Ciclo = item["CicloEscolarTitulo"].ToString();
                    escueladatos.Grupo = item["GrupoNombre"].ToString();
                    //Datos de los alumnos.
                    alumnodatos.NoAlumno = string.IsNullOrEmpty(item["ResultadoPruebaID"].ToString()) ? 0 : Int32.Parse(item["ResultadoPruebaID"].ToString());
                    alumnodatos.Alumno = item["NombreAlumno"].ToString();
                    alumnodatos.Edad = Int32.Parse(item["AlumnoEdad"].ToString());
                    alumnodatos.Sexo = item["AlumnoSexo"].ToString();

                    escueladatos.ListaResultadoAlumnoDetail.Add(alumnodatos);
                    //Agregar a la lista.
                    pruebaDinamicaDetail.ListaResultadoDinamicaGrupoDetail.Add(escueladatos);
                }
            }

            dsResultadoPruebaDinamica = da.RetrieveResultadoPruebaDinamicaEscuela(dctx, escuela, prueba, gCicloEscolar);
            if (dsResultadoPruebaDinamica.Tables["ReportePruebaDinamica"].Rows.Count > 0)
            {
                
                foreach (DataRow item in dsResultadoPruebaDinamica.Tables["ReportePruebaDinamica"].Rows)
                {
                    ResultadoClasificadorDetail clasificadordatos = new ResultadoClasificadorDetail();
                    //Clasificador de la prueba.
                    if (modelo.MetodoCalificacion == EMetodoCalificacion.CLASIFICACION)
                    {
                        clasificadordatos.PuntajeID = !item.IsNull("DetResulC_PuntajeID")
                                                          ? Convert.ToInt32(item["DetResulC_PuntajeID"])
                                                          : 0;
                        if (!item.IsNull("NombreClasificadorDC"))
                            clasificadordatos.Clasificador = item["NombreClasificadorDC"].ToString();
                        if (!item.IsNull("EscalaDC_Nombre"))
                            clasificadordatos.Nombre = item["EscalaDC_Nombre"].ToString();
                        
                    }
                    if (modelo.MetodoCalificacion == EMetodoCalificacion.SELECCION)
                    {
                        clasificadordatos.PuntajeID = !item.IsNull("DetResulS_PuntajeID")
                                                          ? Convert.ToInt32(item["DetResulS_PuntajeID"])
                                                          : 0;
                        if (!item.IsNull("NombreClasificadorDS"))
                            clasificadordatos.Clasificador = item["NombreClasificadorDS"].ToString();
                        if (!item.IsNull("EscalaDS_Nombre"))
                            clasificadordatos.Nombre = item["EscalaDS_Nombre"].ToString();
                    }
                    if (modelo.MetodoCalificacion == EMetodoCalificacion.PUNTOS || modelo.MetodoCalificacion == EMetodoCalificacion.PORCENTAJE)
                    {
                        clasificadordatos.PuntajeID = !item.IsNull("PuntajeID") ? Convert.ToInt32(item["PuntajeID"]) : 0;
                        if (!item.IsNull("NombreClasificadorDI"))
                            clasificadordatos.Clasificador = item["NombreClasificadorDI"].ToString();
                    }
                    if (!item.IsNull("NumeroAlumnos"))
                    {
                        clasificadordatos.Numero = Int32.Parse(item["NumeroAlumnos"].ToString());
                    }

                    escueladatos.ListaResultadoClasificadorDetail.Add(clasificadordatos);
                }

                escueladatos.ListaResultadoClasificadorDetail = AddEscalasSinResultado(escueladatos, prueba);
                escueladatos.ListaResultadoClasificadorDetail =  escueladatos.ListaResultadoClasificadorDetail.OrderBy(item=>item.Clasificador).ToList<ResultadoClasificadorDetail>();
            }
            

            foreach (ResultadoClasificadorDetail detail in escueladatos.ListaResultadoClasificadorDetail)
            {
                detail.ListaEscalas = new List<ResultadoEscalasDetail>();
                PruebaDinamicaCtrl dae = new PruebaDinamicaCtrl();
                ModeloCtrl modeloCtrl = new ModeloCtrl();
                DataSet dse = new DataSet();

                DataSet dsc = modeloCtrl.RetrieveClasificador(dctx, new Clasificador() {Nombre = detail.Clasificador},
                                                              prueba.Modelo as ModeloDinamico);

                if (dsc.Tables["Clasificador"].Rows.Count > 0)
                {
                    foreach (DataRow rowc in dsc.Tables["Clasificador"].Rows)
	                {
                        int clasificadorid = Convert.ToInt32(rowc["ClasificadorID"].ToString());
                        switch (modelo.MetodoCalificacion)
                        {
                            case EMetodoCalificacion.CLASIFICACION:
                                dse = dae.RetrieveEscalaDinamica(dctx, prueba,
                                                      new EscalaClasificacionDinamica(){Clasificador = new Clasificador(){ClasificadorID = clasificadorid}});
                                break;
                            case EMetodoCalificacion.SELECCION:
                                dse = dae.RetrieveEscalaDinamica(dctx, prueba,
                                                      new EscalaSeleccionDinamica() { Clasificador = new Clasificador() { ClasificadorID = clasificadorid } });
                                break;
                            case EMetodoCalificacion.PORCENTAJE:
                                dse = dae.RetrieveEscalaDinamica(dctx, prueba,
                                                      new EscalaPorcentajeDinamica() { Clasificador = new Clasificador() { ClasificadorID = clasificadorid } });
                                break;
                            case EMetodoCalificacion.PUNTOS:
                                dse = dae.RetrieveEscalaDinamica(dctx, prueba,
                                                      new EscalaPuntajeDinamica() { Clasificador = new Clasificador() { ClasificadorID = clasificadorid } });
                                break;
                        }

                        if (dse.Tables["EscalaDinamica"].Rows.Count > 0)
                        {
                            foreach (DataRow row in dse.Tables["EscalaDinamica"].Rows)
                            {
                                ResultadoEscalasDetail escalas = new ResultadoEscalasDetail();
                                escalas.PuntajeID = Int32.Parse(row["PuntajeID"].ToString());
                                escalas.ClasificadorID = Int32.Parse(row["ClasificadorID"].ToString());
                                escalas.PuntajeMinimo = Decimal.Parse(row["PuntajeMinimo"].ToString());
                                escalas.PuntajeMaximo = Decimal.Parse(row["PuntajeMaximo"].ToString());
                                escalas.Nombre = row["Nombre"].ToString();
                                escalas.Descripcion = row["Descripcion"].ToString();

                                detail.ListaEscalas.Add(escalas);
                            }
                        } 
	                }
                }
            }

            return pruebaDinamicaDetail;
        }
    }
}
