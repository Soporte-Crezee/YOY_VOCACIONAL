using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Net.Mail;
using Framework.Base.DataAccess;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Licencias.Service;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.Seguridad.Utils;
using POV.Comun.Service;

namespace POV.Administracion.Service
{
    public class GenerarClavesGrupoCicloEscolarCtrl
    {
        private GrupoCicloEscolarCtrl grupoCicloEscolarCtrl;
        private AlumnoCtrl alumnoCtrl;
        private UsuarioCtrl usuarioCtrl;


        private DataSet dsGrupoCicloEscolar;

        public DataSet DsGrupoCicloEscolar
        {
            get { return dsGrupoCicloEscolar.Tables[0] != null ? dsGrupoCicloEscolar : null; }
            set { dsGrupoCicloEscolar = value; }
        }
        private string linkPortalSocial;
        public string LinkPortalSocial
        {
            set { linkPortalSocial = value; }
        }

        public GenerarClavesGrupoCicloEscolarCtrl()
        {
            grupoCicloEscolarCtrl = new GrupoCicloEscolarCtrl();
            alumnoCtrl = new AlumnoCtrl();
            usuarioCtrl = new UsuarioCtrl();
            dsGrupoCicloEscolar = new DataSet();

        }

        /// <summary>
        /// Genera una DataTable con los datos Del Ciclo Escolar Actual 
        /// </summary>
        /// <param name="dctx">Objeto DataContext para la conexión</param>
        /// <param name="grupoCicloEscolar">GrupoCicloEscolar a consultar</param>
        private DataTable CreateComposeDtGrupoCicloEscolar(IDataContext dctx, GrupoCicloEscolar grupoCicloEscolar)
        {

            #region Generar de resultados
            GrupoCicloEscolar grupoCicloEscolarActual = grupoCicloEscolarCtrl.RetriveComplete(dctx, new GrupoCicloEscolar { GrupoCicloEscolarID = grupoCicloEscolar.GrupoCicloEscolarID, Escuela = grupoCicloEscolar.Escuela });
            DataSet dsCompose = new DataSet();
            DataTable dtResultado = new DataTable("GrupoCicloEscolar");
            dsCompose.Tables.Add(dtResultado);

            dtResultado.Columns.Add(new DataColumn("GrupoCicloEscolarID", typeof(Guid)));
            dtResultado.Columns.Add(new DataColumn("GrupoCicloEscolarClave", typeof(string)));
            dtResultado.Columns.Add(new DataColumn("GrupoID", typeof(Guid)));
            dtResultado.Columns.Add(new DataColumn("GrupoNombre", typeof(string)));
            dtResultado.Columns.Add(new DataColumn("GrupoGrado", typeof(byte)));
            dtResultado.Columns.Add(new DataColumn("Escuela", typeof(string)));
            dtResultado.Columns.Add(new DataColumn("CicloEscolarID", typeof(int)));
            dtResultado.Columns.Add(new DataColumn("CicloEscolarTitulo", typeof(string)));


            DataRow dr = dsCompose.Tables["GrupoCicloEscolar"].NewRow();
            dr.SetField("GrupoCicloEscolarID", grupoCicloEscolarActual.GrupoCicloEscolarID);
            dr.SetField("GrupoCicloEscolarClave", grupoCicloEscolarActual.Clave);
            dr.SetField("GrupoID", grupoCicloEscolarActual.Grupo.GrupoID);
            dr.SetField("GrupoNombre", grupoCicloEscolarActual.Grupo.Nombre);
            dr.SetField("GrupoGrado", grupoCicloEscolarActual.Grupo.Grado);
            dr.SetField("Escuela", grupoCicloEscolarActual.Escuela.NombreEscuela);
            dr.SetField("CicloEscolarID", grupoCicloEscolarActual.CicloEscolar.CicloEscolarID);
            dr.SetField("CicloEscolarTitulo", grupoCicloEscolarActual.CicloEscolar.Titulo);
            dsCompose.Tables["GrupoCicloEscolar"].Rows.Add(dr);
            #endregion
            return (dsCompose.Tables["GrupoCicloEscolar"].Copy());
        }

        public void GenerarClaveGrupoCicloEscolar(IDataContext dctx, GrupoCicloEscolar grupoCicloEscolar)
        {

            if (grupoCicloEscolar == null || grupoCicloEscolar.GrupoCicloEscolarID == null)
                throw new ArgumentException("CicloEscolar es requerido.");
            if (grupoCicloEscolar.Escuela == null)
                throw new ArgumentException("Escuela es requerido");

            object firm = new object();

            try
            {
                //conexión base de datos
                dctx.OpenConnection(firm);
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                throw new Exception("Inconsistencias al conectarse a la base de datos.");
            }
            try
            {
                dctx.BeginTransaction(firm);


                grupoCicloEscolar = grupoCicloEscolarCtrl.RetriveComplete(dctx, grupoCicloEscolar);

                #region Actualizar la clave del grupo

                GrupoCicloEscolar anterior = grupoCicloEscolarCtrl.RetriveComplete(dctx, grupoCicloEscolar);

                grupoCicloEscolar.Clave = new PasswordProvider(5).GetNewPassword();
                grupoCicloEscolarCtrl.Update(dctx, grupoCicloEscolar, anterior);
                #endregion
                #region Generar de resultados
                GrupoCicloEscolar grupoCicloEscolarActual = grupoCicloEscolarCtrl.RetriveComplete(dctx, new GrupoCicloEscolar { GrupoCicloEscolarID = grupoCicloEscolar.GrupoCicloEscolarID, Escuela = grupoCicloEscolar.Escuela });

                DataSet dsCompose = new DataSet();

                DataTable dtResultado = new DataTable("GrupoCicloEscolar");
                dsCompose.Tables.Add(dtResultado);

                dtResultado.Columns.Add(new DataColumn("GrupoCicloEscolarID", typeof(Guid)));
                dtResultado.Columns.Add(new DataColumn("GrupoCicloEscolarClave", typeof(string)));
                dtResultado.Columns.Add(new DataColumn("GrupoID", typeof(Guid)));
                dtResultado.Columns.Add(new DataColumn("GrupoNombre", typeof(string)));
                dtResultado.Columns.Add(new DataColumn("GrupoGrado", typeof(byte)));
                dtResultado.Columns.Add(new DataColumn("Escuela", typeof(string)));

                dtResultado.Columns.Add(new DataColumn("CicloEscolarID", typeof(int)));
                dtResultado.Columns.Add(new DataColumn("CicloEscolarTitulo", typeof(string)));


                DataRow dr = dsCompose.Tables["GrupoCicloEscolar"].NewRow();
                dr.SetField("GrupoCicloEscolarID", grupoCicloEscolarActual.GrupoCicloEscolarID);
                dr.SetField("GrupoCicloEscolarClave", grupoCicloEscolarActual.Clave);
                dr.SetField("GrupoID", grupoCicloEscolarActual.Grupo.GrupoID);
                dr.SetField("GrupoNombre", grupoCicloEscolarActual.Grupo.Nombre);
                dr.SetField("GrupoGrado", grupoCicloEscolarActual.Grupo.Grado);
                dr.SetField("Escuela", grupoCicloEscolarActual.Escuela.NombreEscuela);
                dr.SetField("CicloEscolarID", grupoCicloEscolarActual.CicloEscolar.CicloEscolarID);
                dr.SetField("CicloEscolarTitulo", grupoCicloEscolarActual.CicloEscolar.Titulo);
                dsCompose.Tables["GrupoCicloEscolar"].Rows.Add(dr);
                #endregion
                dctx.CommitTransaction(firm);
                dsGrupoCicloEscolar.Tables.Add(dsCompose.Tables["GrupoCicloEscolar"].Copy());

            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                dctx.RollbackTransaction(firm);
                dsGrupoCicloEscolar = null;
            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(firm);
            }
        }

        public void GenerarClaveAlumnosCicloEscolar(IDataContext dctx, GrupoCicloEscolar grupoCicloEscolar, Dictionary<long, bool> listaAlumnos, bool? regenerarNombreUsuario = null)
        {
            if (grupoCicloEscolar == null || grupoCicloEscolar.GrupoCicloEscolarID == null)
                throw new ArgumentException("CicloEscolar es requerido.");
            if (grupoCicloEscolar.Escuela == null)
                throw new ArgumentException("Escuela es requerido");
            object firma = new object();
            DataSet dsCompose = new DataSet();
            try
            {
                //conexión base de datos
                dctx.OpenConnection(firma);
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                throw new Exception("Inconsistencias al conectarse a la base de datos.");
            }

            try
            {
                dctx.BeginTransaction(firma);
                GrupoCicloEscolar grupoCiclo = grupoCicloEscolarCtrl.RetriveComplete(dctx, grupoCicloEscolar);
                dsGrupoCicloEscolar.Tables.Add(CreateComposeDtGrupoCicloEscolar(dctx, grupoCicloEscolar));
                if (grupoCiclo.AsignacionAlumnos.Any())
                {

                    DataTable dtResultado = new DataTable("AlumnosAsignados");
                    dsCompose.Tables.Add(dtResultado);

                    dsCompose.Tables[0].Columns.Add(new DataColumn("AlumnoID", typeof(long)));
                    dsCompose.Tables[0].Columns.Add(new DataColumn("Curp", typeof(string)));
                    dsCompose.Tables[0].Columns.Add(new DataColumn("Matricula", typeof(string)));
                    dsCompose.Tables[0].Columns.Add(new DataColumn("NombreCompleto", typeof(string)));
                    dsCompose.Tables[0].Columns.Add(new DataColumn("UsuarioID", typeof(int)));
                    dsCompose.Tables[0].Columns.Add(new DataColumn("NombreUsuario", typeof(string)));
                    dsCompose.Tables[0].Columns.Add(new DataColumn("NuevoPassword", typeof(string)));
                    dsCompose.Tables[0].Columns.Add(new DataColumn("GenerarPassword", typeof(bool)));

                    foreach (AsignacionAlumnoGrupo asignacion in grupoCiclo.AsignacionAlumnos)
                    {
                        if (!listaAlumnos.ContainsKey(asignacion.Alumno.AlumnoID.Value))
                            continue;
                        DataSet dsalumno = alumnoCtrl.Retrieve(dctx, new Alumno { AlumnoID = asignacion.Alumno.AlumnoID, Estatus = true });
                        if (dsalumno.Tables["Alumno"].Rows.Count == 1)
                        {
                            Alumno alumnoAsignado = alumnoCtrl.LastDataRowToAlumno(dsalumno);
                            Usuario usuario = (new LicenciaEscuelaCtrl()).RetrieveUsuario(dctx, alumnoAsignado);
                            if (usuario == null) throw new ArgumentNullException("usuario");

                            usuario = usuarioCtrl.RetrieveComplete(dctx, usuario);
                            Usuario usuarioAnterior = (Usuario)usuario.Clone();

                            #region *** generar nuevo usuario
                            if (regenerarNombreUsuario != null && regenerarNombreUsuario.Value)
                                usuario.NombreUsuario = usuarioCtrl.GenerarNombreUsuarioUnico(dctx, alumnoAsignado.Nombre, alumnoAsignado.PrimerApellido, alumnoAsignado.FechaNacimiento.Value);
                            #endregion

                            #region Regenerar Password y Actualizar

                            string claveUsuario = new PasswordProvider(8).GetNewPassword();
                            usuario.Password = EncryptHash.SHA1encrypt(claveUsuario);
                            usuario.PasswordTemp = true;
                            usuarioCtrl.Update(dctx, usuario, usuarioAnterior);
                            usuario = usuarioCtrl.RetrieveComplete(dctx, usuario);
                            #endregion

                            #region envio de correo
                            bool envioCorreo = listaAlumnos[alumnoAsignado.AlumnoID.Value];
                            if (!string.IsNullOrEmpty(usuario.Email) && envioCorreo)
                                enviarCorreo(usuario, claveUsuario);
                            #endregion
                            DataRow dr = dsCompose.Tables[0].NewRow();
                            dr.SetField("AlumnoID", alumnoAsignado.AlumnoID);
                            dr.SetField("Curp", alumnoAsignado.Curp);
                            dr.SetField("Matricula", alumnoAsignado.Matricula);
                            dr.SetField("NombreCompleto", string.Format(" {0} {1} {2}", alumnoAsignado.Nombre, alumnoAsignado.PrimerApellido, alumnoAsignado.SegundoApellido));
                            dr.SetField("UsuarioID", usuario.UsuarioID);
                            dr.SetField("NombreUsuario", usuario.NombreUsuario);
                            dr.SetField("NuevoPassword", claveUsuario);
                            dr.SetField("GenerarPassword", true);
                            dsCompose.Tables["AlumnosAsignados"].Rows.Add(dr);
                        }
                    }
                }

                dctx.CommitTransaction(firma);
                dsGrupoCicloEscolar.Tables.Add(dsCompose.Tables["AlumnosAsignados"].Copy());

            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                dctx.RollbackTransaction(firma);
            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(firma);
            }
        }

        private void enviarCorreo(Usuario usuario, string newPassword)
        {
            CorreoCtrl correoCtrl = new CorreoCtrl();
            #region Variables

            const string imgalt = "NATWARE - Punto de encuentro educativo";
            const string titulo = "NATWARE - Punto de encuentro educativo";
            #endregion
            string cuerpo = string.Format(@"<table width='600'><tr><td>
                                            </td></tr>
                                            <tr><td><h2 style='color:#A5439A'>{2}</h2>
                                            </p>
                                            <p>Estos son los datos para que accedas a tu portal.</p>
                                            <p><b>Usuario:</b> {3}</p>
                                            <p><b>Contraseña:</b> {4}</p><p>Una vez que entres al portal, te recomendamos cambiar tu contraseña.</p>
                                            </td>
                                            </tr>
                                            <tr><td>
                                            <a href='{5}'>NATWARE - Punto de encuentro educativo</a>
                                            </td></tr>
                                          </table>"
                                          , "", imgalt, titulo, usuario.NombreUsuario, newPassword, linkPortalSocial);


            List<string> tos = new List<string>();
            tos.Add(usuario.Email);
            AlternateView texto = null;
            List<string> archivos = new List<string>();
            List<string> copias = new List<string>();
            try
            {
                correoCtrl.sendMessage(tos, "Natware - Recuperación de contraseña", cuerpo, texto, archivos, copias);
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
            }
        }
    }
}
