using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Social.BO;
using POV.CentroEducativo.BO;
using Framework.Base.DataAccess;
using POV.Prueba.Diagnostico.Garner.Service;
using POV.Prueba.Diagnostico.Garner.BO;
using POV.Reactivos.BO;
using POV.Reactivos.Service;
using POV.Modelo.Garner.Service;
using POV.Modelo.Garner.BO;
using System.Data;
using POV.Expediente.BO;
using POV.Expediente.Service;
using POV.Prueba.BO;

namespace POV.Social.Service
{
    /// <summary>
    /// Controlador para el objeto temporal que reune los datos para el reporte de resultados de diagnóstico
    /// por grupo.
    /// </summary>
    public class ReporteResultadoDiagnosticoPorGrupoCtrl
    {
        /// <summary>
        /// construye un objeto particular para mostrar en el reporte de resultados de diagnóstico por grupo.
        /// </summary>
        /// <param name="dctx">parámetro que contiene los datos de conexión con la BD.</param>
        /// <param name="listAlumno"> lista de los alumno de un grupo ciclo escolar.</param>
        /// <returns>regresa una lista de objetos que contienen algunos datos personales del alumno junto con sus 
        /// resultados del examen diagnóstico y su nivel de proclividad.</returns>
        public List<ReporteResultadoDiagnosticoPorGrupo> ConstructObject(IDataContext dctx, List<Alumno> listAlumno,PruebaDiagnostico prueba)
        {
            TipoInteligenciaCtrl tipoInteligenciaCtrl = new TipoInteligenciaCtrl();
            List<ReporteResultadoDiagnosticoPorGrupo> listReporteResultadoDiagnosticoPorGrupo = new List<ReporteResultadoDiagnosticoPorGrupo>();
            RegistroPruebaDiagnosticoCtrl registroPruebaDiagnosticoCtrl = new RegistroPruebaDiagnosticoCtrl();
            int cont = 1;
            foreach (Alumno alumno in listAlumno)
            {
                ReporteResultadoDiagnosticoPorGrupo reporteResultadoDiagnosticoPorGrupo = new ReporteResultadoDiagnosticoPorGrupo();
                reporteResultadoDiagnosticoPorGrupo.Numero = cont;
                reporteResultadoDiagnosticoPorGrupo.NombreAlumno = alumno.Nombre + " " + alumno.PrimerApellido + " " + alumno.SegundoApellido;
                reporteResultadoDiagnosticoPorGrupo.AlumnoID = alumno.AlumnoID;
                reporteResultadoDiagnosticoPorGrupo.ConResultado = true;
                if (alumno.Sexo.Value)
                {
                    reporteResultadoDiagnosticoPorGrupo.SexoAlumno = "Hombre";
                }
                else
                {
                    reporteResultadoDiagnosticoPorGrupo.SexoAlumno = "Mujer";
                }

                DateTime fechaNacimientoAlumno = alumno.FechaNacimiento.Value;
                int edadAlumno = CalcularEdadAlumno(fechaNacimientoAlumno);

                reporteResultadoDiagnosticoPorGrupo.EdadAlumno = edadAlumno;

                RegistroPruebaDiagnostico registroPruebaDiagnostico = new RegistroPruebaDiagnostico();
                registroPruebaDiagnostico.Alumno = alumno;
                registroPruebaDiagnostico.PruebaDiagnostico = prueba;
                 try
                 {
                     ///se intenta objener el registro de prueba diagnóstico de un alumno.
                     registroPruebaDiagnostico = registroPruebaDiagnosticoCtrl.RetrieveComplete(dctx, registroPruebaDiagnostico);
                     string fechaAplicacionDiagnosticoAlumno = null;
                     if (registroPruebaDiagnostico.FechaFin != null)
                     {
                         fechaAplicacionDiagnosticoAlumno = String.Format("{0:dd/MM/yyyy}", registroPruebaDiagnostico.FechaFin);
                     }
                     else
                     {
                         fechaAplicacionDiagnosticoAlumno = "No concluido";
                         reporteResultadoDiagnosticoPorGrupo.ConResultado = false;
                     }
                     reporteResultadoDiagnosticoPorGrupo.FechaAplicacionDiagnosticoAlumno = fechaAplicacionDiagnosticoAlumno;
                     reporteResultadoDiagnosticoPorGrupo.IdResultadoPrueba = ObtenerPuntajePruebaDiagnosticoAlumno(dctx, registroPruebaDiagnostico);

                     String tiempoRespuestaAlumno = TiempoRespuestaPruebaAlumno(dctx, registroPruebaDiagnostico);
                     reporteResultadoDiagnosticoPorGrupo.TiempoRequeridoAlumno = tiempoRespuestaAlumno;

                     List<TipoInteligencia> tiposInteligencia = new List<TipoInteligencia>();
                     DataSet dsTipoInteligencia = tipoInteligenciaCtrl.Retrieve(dctx, new TipoInteligencia());
                     foreach (DataRow row in dsTipoInteligencia.Tables[0].Rows)
                     {
                         TipoInteligencia temp = tipoInteligenciaCtrl.DataRowToTipoInteligencia(row);
                         tiposInteligencia.Add(temp);
                     }
                     reporteResultadoDiagnosticoPorGrupo.InteligenciaPredominante = registroPruebaDiagnostico.ObtenerInteligenciaPredominanteEnBaseAciertos(tiposInteligencia); //.ObtenerInteligenciaPredominante(tiposInteligencia);

                 }
                 catch (Exception ex) {
                     ///si el alumno no cuenta con registros de examen diagnóstico se indicará en sus parámetros.
                     
                     reporteResultadoDiagnosticoPorGrupo.ConResultado = false;
                     reporteResultadoDiagnosticoPorGrupo.FechaAplicacionDiagnosticoAlumno = "No presentó";
                     reporteResultadoDiagnosticoPorGrupo.TiempoRequeridoAlumno = "0 min";
                     reporteResultadoDiagnosticoPorGrupo.InteligenciaPredominante = null;
                     reporteResultadoDiagnosticoPorGrupo.IdResultadoPrueba = null;
                 }
                listReporteResultadoDiagnosticoPorGrupo.Add(reporteResultadoDiagnosticoPorGrupo);
                cont++;
            }
            return listReporteResultadoDiagnosticoPorGrupo;
        }

        /// <summary>
        /// Calcula la edad de un alumno a partir de su fecha de nacimiento.
        /// </summary>
        /// <param name="fechaNacimientoAlumno">Fecha de nacimiento del alumno.</param>
        /// <returns>la edad del alumno.</returns>
        private int CalcularEdadAlumno(DateTime fechaNacimientoAlumno)
        {
            DateTime fechaActual = System.DateTime.Now;
            int diferenciaAnios = fechaActual.Year - fechaNacimientoAlumno.Year;
            int diferenciaMeses = fechaActual.Month - fechaNacimientoAlumno.Month;
            int diferenciaDias = fechaActual.Day - fechaNacimientoAlumno.Day;

            if (diferenciaMeses < 0)
            {
                return diferenciaAnios = diferenciaAnios - 1;
            }
            else if (diferenciaMeses == 0)
            {
                if (diferenciaDias < 0)
                {
                    return diferenciaAnios = diferenciaAnios - 1;
                }
                else
                {
                    return diferenciaAnios;
                }
            }
            else
            {
                return diferenciaAnios;
            }

        }

        /// <summary>
        /// Obtiene la calificación de un alumno para su prueba diagnóstica.
        /// </summary>
        /// <param name="dctx">parámetro con datos para la conexión a la BD.</param>
        /// <param name="pruebaDiagnostico">pruebaDiagnóstica de la cuál se desea obtener el puntaje del alumno.</param>
        /// <returns>la Id del resultado requerida en el segundo reporte.</returns>
        private int? ObtenerPuntajePruebaDiagnosticoAlumno(IDataContext dctx, RegistroPruebaDiagnostico registroPruebaDiagnostico)
        {
            int? idPruebaResultado;
            ResultadoPruebaDiagnostico resultadoPruebaDiagnosticoAlumno = new ResultadoPruebaDiagnostico();
            ResultadoPruebaDiagnosticoCtrl resultadoPruebaDiagnosticoCtrl = new ResultadoPruebaDiagnosticoCtrl();
            resultadoPruebaDiagnosticoAlumno.Prueba = registroPruebaDiagnostico.PruebaDiagnostico;
            resultadoPruebaDiagnosticoAlumno.Alumno = registroPruebaDiagnostico.Alumno;
            DataSet dsResultadoPruebaDiagnosticoAlumno = resultadoPruebaDiagnosticoCtrl.Retrieve(dctx, resultadoPruebaDiagnosticoAlumno);
            try
            {
                resultadoPruebaDiagnosticoAlumno = resultadoPruebaDiagnosticoCtrl.LastDataRowToResultadoPruebaDiagnostico(dsResultadoPruebaDiagnosticoAlumno);
                idPruebaResultado = resultadoPruebaDiagnosticoAlumno.ResultadoPruebaID;
                return idPruebaResultado;

            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                idPruebaResultado = null;
                return idPruebaResultado;
            }

        }

        /// <summary>
        /// Obtiene el tiempo total que el alumno demoró en su prueba diagnóstica.
        /// </summary>
        /// <param name="dctx">parámetro que contiene los datos para la conexión a la BD.</param>
        /// <param name="registroPruebaDiagnostico">Registro de prueba diagnóstico del alumno.</param>
        /// <returns>regresa un string con el tiempo total que tardó el alumno en contestar su prueba diagnóstica.</returns>
        private string TiempoRespuestaPruebaAlumno(IDataContext dctx, RegistroPruebaDiagnostico registroPruebaDiagnostico)
        {
            List<RespuestaPreguntaAlumno> listaRespuestasAlumno = new List<RespuestaPreguntaAlumno>();
            List<RespuestaReactivoAlumno> listRespuestaReactivoAlumno = registroPruebaDiagnostico.RespuestaReactivosAlumno;
            ReactivoCtrl reactivoCtrl = new ReactivoCtrl();
            int tiempoRespuestaAlumno = 0;
            if (listRespuestaReactivoAlumno.Count > 0) 
            {
                ///si el alumno tiene respuestas se recorren para contabilizar su tiempo.
                foreach (RespuestaReactivoAlumno respuestaReactivoAlumno in listRespuestaReactivoAlumno)
                {
                    listaRespuestasAlumno = respuestaReactivoAlumno.RespuestaPreguntasAlumno;
                    
                    if (listaRespuestasAlumno.Count > 0)
                    {
                        //se contabiliza el tiempo invertido en las preguntas
                        foreach (RespuestaPreguntaAlumno respuestaPreguntaAlumno in listaRespuestasAlumno)
                        {
                            if (respuestaPreguntaAlumno.Tiempo!=null)
                                tiempoRespuestaAlumno = tiempoRespuestaAlumno + respuestaPreguntaAlumno.Tiempo.Value;

                        }
                    }
                }
            }
            decimal minutosTotales = Math.Round(Convert.ToDecimal(new TimeSpan(0, 0, tiempoRespuestaAlumno).TotalMinutes), 2);
            return minutosTotales + " min";
        }

    }
}
