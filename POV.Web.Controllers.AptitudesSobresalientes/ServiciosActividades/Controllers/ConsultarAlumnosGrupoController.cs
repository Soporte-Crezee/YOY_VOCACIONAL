using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Framework.Base.DataAccess;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.ConfiguracionActividades.BO;
using POV.Modelo.Context;
using POV.ConfiguracionActividades.Services;
using POV.Seguridad.BO;

namespace POV.Web.ServiciosActividades.Controllers
{
    public class ConsultarAlumnosGrupoController
    {
        #region Conexion Version Anterior
        IDataContext dctx = new DataContext((new DataProviderFactory()).GetDataProvider("POV"));
        #endregion

        private readonly Contexto model;
        private readonly object sign;

        public DataTable ConsultarAlumnos(CicloEscolar cicloEscolar, Escuela escuela,Docente docente, Alumno alumno, Grupo grupo, AreaConocimiento areaConocimiento, Usuario usuario = null )
        {

            GrupoCicloEscolarCtrl ctrl = new GrupoCicloEscolarCtrl();

            List<Grupo> gpFiltros = new List<Grupo>();
            List<GrupoCicloEscolar> gruposDocente = ConsultarGruposEscuelaDocente(escuela, cicloEscolar, docente);
            if (grupo != null && (!string.IsNullOrEmpty(grupo.Nombre) || grupo.Grado != null))
            {
                if (!string.IsNullOrEmpty(grupo.Nombre))
                    gpFiltros.Add(grupo);
                else 
                {
                    gpFiltros.AddRange(gruposDocente.Where(item => item.Grupo.Grado == grupo.Grado).Select( gp => gp.Grupo).ToList());
                }
            }
            else
            {
                gpFiltros.AddRange(gruposDocente.Select(gpc => gpc.Grupo).ToList());
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("AlumnoID");
            dt.Columns.Add("GrupoCicloEscolarID");
            dt.Columns.Add("EscuelaID");
            dt.Columns.Add("Grado");
            dt.Columns.Add("NombreGrupo");
            dt.Columns.Add("NombreCompletoAlumno");
            dt.Columns.Add("FechaNacimiento");
            dt.Columns.Add("AreaConocimientoID");
            foreach (Grupo gp in gpFiltros)
            {

                GrupoCicloEscolar grupoCicloFiltro = new GrupoCicloEscolar
                {
                    Escuela = escuela,
                    CicloEscolar = cicloEscolar,
                    Grupo = new Grupo { Grado = gp.Grado, Nombre = gp.Nombre }
                };

                DataSet ds = ctrl.RetrieveAsignacionAlumnosGrupo(dctx, grupoCicloFiltro, alumno, areaConocimiento, docente, usuario);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    bool activo = (bool)Convert.ChangeType(dr["AlumnoID"], typeof(bool));
                    DataRow drAlumno = dt.NewRow();

                    drAlumno["AlumnoID"] = (long)Convert.ChangeType(dr["AlumnoID"], typeof(long));
                    drAlumno["GrupoCicloEscolarID"] = (Guid)Convert.ChangeType(dr["GrupoCicloEscolarID"], typeof(Guid));
                    drAlumno["EscuelaID"] = (int)Convert.ChangeType(dr["EscuelaID"], typeof(int));
                    drAlumno["Grado"] = (byte)Convert.ChangeType(dr["Grado"], typeof(byte));
                    drAlumno["NombreGrupo"] = (string)Convert.ChangeType(dr["NombreGrupo"], typeof(string));
                    drAlumno["NombreCompletoAlumno"] = (string)Convert.ChangeType(dr["NombreCompletoAlumno"], typeof(string));
                    drAlumno["FechaNacimiento"] = (DateTime)Convert.ChangeType(dr["FechaNacimiento"], typeof(DateTime));
                    drAlumno["AreaConocimientoID"] = (int)Convert.ChangeType(dr["AreaConocimientoID"], typeof(int));

                    dt.Rows.Add(drAlumno);
                }
            }

            
            return dt;
        }

        /// <summary>
        /// Consulta los grupos del docente
        /// </summary>
        /// <param name="escuela"></param>
        /// <param name="cicloEscolar"></param>
        /// <param name="docente"></param>
        /// <returns></returns>
        public List<GrupoCicloEscolar> ConsultarGruposEscuelaDocente(Escuela escuela, CicloEscolar cicloEscolar, Docente docente)
        {
            GrupoCicloEscolarCtrl ctrl = new GrupoCicloEscolarCtrl();
            GrupoCtrl gpCtrl = new GrupoCtrl();
            DataSet ds = ctrl.Retrieve(dctx, new GrupoCicloEscolar { Escuela = escuela, CicloEscolar = cicloEscolar });
            List<GrupoCicloEscolar> grupos = new List<GrupoCicloEscolar>();
            if (ds.Tables[0].Rows.Count > 0)
            {
                //por cada grupo ciclo escolar se cargan los grupos
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GrupoCicloEscolar grupoCicloEscolar = ctrl.DataRowToGrupoCicloEscolar(dr);

                    List<Materia> materias = ctrl.RetrieveMateriasDocente(dctx, docente, grupoCicloEscolar);

                    //si el docente tiene materias asignadas en el grupo se agrega
                    if (materias.Count > 0)
                    {
                        DataSet dsGrupo = gpCtrl.Retrieve(dctx, new Grupo { GrupoID = grupoCicloEscolar.Grupo.GrupoID }, escuela);
                        if (dsGrupo.Tables[0].Rows.Count > 0)
                        {
                            grupoCicloEscolar.Grupo = gpCtrl.LastDataRowToGrupo(dsGrupo); 
                            grupos.Add(grupoCicloEscolar);
                        }
                    }
                }
            }

            return grupos;
        }

        /// <summary>
        /// Consulta las actividades de un alumno que un docente le haya asignado
        /// </summary>
        /// <param name="alumno"></param>
        /// <param name="docente"></param>
        /// <returns></returns>
        public List<AsignacionActividad> ConsultarActividadesAlumno(Alumno alumno, Docente docente)
        {
            #region validaciones
            if (alumno == null) throw new ArgumentNullException("Alumno", "El alumno no puede ser nulo");
            if (alumno.AlumnoID == null) throw new ArgumentNullException("Alumno.AlumnoID", "El identificador del alumno no puede ser nulo");
            if (docente == null) throw new ArgumentNullException("Docente", "El docente no puede ser nulo");
            if (docente.DocenteID == null) throw new ArgumentNullException("Docente.DocenteID", "El identificador del docente no puede ser nulo");
            #endregion

            object sign = new object();
            Contexto ctx = new Contexto(sign);
            AsignacionActividadCtrl ctrl = new AsignacionActividadCtrl(model);
            
            List<AsignacionActividad> actividads = ctrl.RetrieveWithRelationship(new AsignacionActividad()
            {
                AlumnoId = alumno.AlumnoID
            }, false);

            actividads = actividads.Where(item => item.Actividad.Activo.Value && item.Actividad is ActividadDocente && (item.Actividad as ActividadDocente).Docente.DocenteID == docente.DocenteID).ToList();

            return actividads;
        }
    }
}
