using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Text;
using Framework.Base.DataAccess;
using GP.SocialEngine.BO;
using GP.SocialEngine.Service;
using POV.Comun.Service;
using POV.Licencias.BO;
using POV.Licencias.Service;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.Seguridad.Utils;
using System.IO;
using POV.CentroEducativo.Services;

namespace POV.Administracion.Service
{
  public  class CatalogoDocentesCtrl
  {
      private CicloEscolarCtrl cicloEscolarCtrl;
      private EscuelaCtrl escuelaCtrl;
      private DocenteCtrl docenteCtrl;
      private UsuarioCtrl usuarioCtrl;
      private LicenciaEscuelaCtrl licenciaEscuelaCtrl;
      private SocialHubCtrl socialHubCtrl;
      private GrupoCicloEscolarCtrl grupoCicloEscolarCtrl;
      private UsuarioSocialCtrl usuarioSocialCtrl;
      public  CatalogoDocentesCtrl()
      {
          cicloEscolarCtrl= new CicloEscolarCtrl();
          escuelaCtrl = new EscuelaCtrl();
          docenteCtrl = new DocenteCtrl();
          usuarioCtrl = new UsuarioCtrl();
          socialHubCtrl = new SocialHubCtrl();
          licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
          grupoCicloEscolarCtrl = new GrupoCicloEscolarCtrl();
          usuarioSocialCtrl = new UsuarioSocialCtrl();

      }

      private void EnviarCorreo(Usuario usuario, string pws, string clave)
      {
          #region Old Code
          
          #region Variables
          string urlimg = UrlImgNatware;
          const string imgalt = "Sistema Natware";
          const string titulo = "SISTEMA NATWARE";
          string linkportal = UrlPortalSocial;
          #endregion
          CorreoCtrl correoCtrl = new CorreoCtrl();

          string cuerpo = string.Format(@"<table width='600'><tr><td>
                                            <img src='{0}' alt='{1}' /></td></tr>
                                            <tr><td><h2 style='color:#A5439A'>{2}</h2>
                                            </p><p>Estos son los datos para que accedas a tu portal.</p>
                                            <p><b>Usuario:</b> {3}</p>
                                            <p><b>Contraseña:</b> {4}</p><p>Una vez que entres al portal, te recomendamos cambiar tu contraseña.</p>
                                            <p>Clave de activación: {5}</p></td>
                                            </tr>
                                            <tr><td>
                                            <a href='{6}'>Natware - Portal Social</a>
                                            </td></tr>
                                          </table>"
                                          , urlimg, imgalt, titulo, usuario.NombreUsuario, pws, clave, linkportal);
          List<string> tos = new List<string>();
          tos.Add(usuario.Email);

          try
          {
              correoCtrl.sendMessage(tos, "Confirmación cuenta", cuerpo, (AlternateView)null, new List<string>(), new List<string>());
              
          }
          catch (Exception ex)
          {
              POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
          }
          
          #endregion
      }
      public string UrlImgNatware { get; set; }
      public string UrlPortalSocial { get; set; }

      /// <summary>
      /// Agrega un docente a la escuela y ciclo escolar proporcionado, creando de ser necesario el UsuarioSocial
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="escuela">Escuela a consultar</param>
      /// <param name="ciclo">Ciclo escolar a consultar</param>
      /// <param name="docente">Docente a agregar</param>
      public Usuario InsertDocenteEscuela(IDataContext dctx, Escuela escuela, CicloEscolar ciclo, Docente docente, string passwordTemp, long? universidadId = null)
      {
          if(escuela==null || escuela.EscuelaID==null || escuela.EscuelaID<=0)
              throw new Exception("Escuela es requerido");

          if (ciclo == null || ciclo.CicloEscolarID == null || ciclo.CicloEscolarID<=0)
              throw new Exception("CicloEscolar es requerido");

          if(docente==null)
              throw new Exception("Docente es requerido");

          #region Insertar Docente.

          string strNombreUsuario = docente.NombreUsuario;

          object firm = new object();
          try
          {
              dctx.OpenConnection(firm);
              dctx.BeginTransaction(firm);

              #region Verificar licencia de la escuela


              LicenciaEscuela licenciaEscuela = new LicenciaEscuela { CicloEscolar = ciclo, Escuela = escuela, Activo = true };

              DataSet ds = licenciaEscuelaCtrl.Retrieve(dctx, licenciaEscuela);
              if (ds.Tables[0].Rows.Count == 0)
                  throw new Exception("La escuela no cuenta con licencia para el ciclo escolar especificado.");

              licenciaEscuela = licenciaEscuelaCtrl.LastDataRowToLicenciaEscuela(ds);
              Escuela escuelaActual = escuelaCtrl.LastDataRowToEscuela(escuelaCtrl.Retrieve(dctx, licenciaEscuela.Escuela));

              #endregion

              #region Verificar Informacion del Docente

              ds = docenteCtrl.Retrieve(dctx, new Docente { Curp = docente.Curp });
              int index = ds.Tables["Docente"].Rows.Count;

              #region Insertar Docente
              if (index == 1)
              {
                  Docente anterior = docenteCtrl.DataRowToDocente(ds.Tables["Docente"].Rows[index - 1]);
                  docente.Estatus = true;
                  docente.DocenteID = anterior.DocenteID;
                  docente.FechaRegistro = anterior.FechaRegistro;
                  docente.Clave = anterior.Clave;
                  docente.EstatusIdentificacion = anterior.EstatusIdentificacion;
                  docenteCtrl.Update(dctx, docente, anterior);
              }
              else
              {
                  docente.FechaRegistro = DateTime.Now;
                  docente.Clave = new PasswordProvider(5).GetNewPassword();
                  docente.EstatusIdentificacion = false;
                  docente.Estatus = true;
                  docenteCtrl.Insert(dctx, docente);
              }


              #endregion

              #region Registrar Asignación Docente-Escuela

              ds = docenteCtrl.Retrieve(dctx, new Docente { Curp = docente.Curp });
              index = ds.Tables["Docente"].Rows.Count;
              if (index == 1)
              {
                  docente = docenteCtrl.DataRowToDocente(ds.Tables["Docente"].Rows[index - 1]);
                  docente.NombreUsuario = strNombreUsuario;
                  //Registrar docente en la escuela
                  escuelaCtrl.InsertAsignacionDocente(dctx, docente, escuelaActual);
              }
              else
                  throw new Exception("InsertDocenteEscuela:Ocurrió un error al registrar al Docente");

              #endregion

              #region Registro de Usuario

              Usuario usuario = new Usuario();
              usuario = null;

              if (universidadId != null)
              {
                  usuario = licenciaEscuelaCtrl.RetrieveUsuarios(dctx, docente).Where(x => x.UniversidadId == universidadId).FirstOrDefault();
                  if (usuario == null)
                      usuario = new Usuario();
              }
              else
                  usuario = licenciaEscuelaCtrl.RetrieveUsuario(dctx, docente);

              bool envioCorreo = false;
              if (usuario != null & usuario.UsuarioID != null)
              {
                  usuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, usuario));

                  if (usuario.EsActivo == false)
                  {
                      Usuario original = (Usuario)usuario.Clone();
                      usuario.EsActivo = true;
                      usuario.Email = docente.Correo;
                      byte[] pws = EncryptHash.SHA1encrypt(passwordTemp);
                      usuario.Password = pws;

                      if (!string.IsNullOrEmpty(usuario.Email))
                          if (!EmailDisponible(dctx, usuario))
                          {
                              Exception ex = new Exception("El correo electrónico proporcionado no se encuentra disponible") { Source = "Email" };
                              throw ex;
                          }

                      if (!string.IsNullOrEmpty(usuario.TelefonoReferencia))
                          if (!TelefonoDisponible(dctx, usuario))
                          {
                              Exception ex = new Exception("El teléfono proporcionado no se encuentra disponible ") { Source = "Telefono" };
                              throw ex;
                          }

                      usuarioCtrl.Update(dctx, usuario, original);
                  }
              }
              else
              {
                  usuario = new Usuario();
                  usuario.NombreUsuario = docente.NombreUsuario;
                  usuario.Email = docente.Correo;
                  byte[] pws = EncryptHash.SHA1encrypt(passwordTemp);
                  usuario.Password = pws;
                  usuario.EsActivo = true;
                  usuario.FechaCreacion = DateTime.Now;
                  usuario.PasswordTemp = true;
                  usuario.UniversidadId = universidadId;

                  //Consultar Termino Activo
                  TerminoCtrl terminoCtrl = new TerminoCtrl();
                  DataSet dsTermino = (terminoCtrl.Retrieve(dctx, new Termino { Estatus = true }));
                  usuario.Termino = dsTermino.Tables[0].Rows.Count >= 1 ? terminoCtrl.LastDataRowToTermino(dsTermino) : new Termino();
                  usuario.AceptoTerminos = false;

                  if (!string.IsNullOrEmpty(usuario.Email))
                      if (!EmailDisponible(dctx, usuario))
                      {
                          Exception ex = new Exception("El correo electrónico proporcionado no se encuentra disponible") { Source = "Email" };
                          throw ex;
                      }

                  if (!string.IsNullOrEmpty(usuario.TelefonoReferencia))
                      if (!TelefonoDisponible(dctx, usuario))
                      {
                          Exception ex = new Exception("El teléfono proporcionado no se encuentra disponible ") { Source = "Telefono" };
                          throw ex;
                      }

                  usuarioCtrl.Insert(dctx, usuario);
                  usuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, usuario));
                  envioCorreo = true;
              }
              #endregion

              UsuarioPrivilegiosCtrl usuarioPrivilegiosCtrl = new UsuarioPrivilegiosCtrl();

              #region registrar usuario privilegios

              //asignamos el perfil alumno a la lista de privilegios
              Perfil perfil = new Perfil { PerfilID = (int)EPerfil.DOCENTE };

              List<IPrivilegio> privilegios = new List<IPrivilegio>();
              privilegios.Add(perfil);

              usuarioPrivilegiosCtrl.InsertUsuarioEscolarPrivilegios(dctx, usuario, licenciaEscuela.Escuela, licenciaEscuela.CicloEscolar, privilegios);

              #endregion


              LicenciaDocente licenciaDocente = licenciaEscuelaCtrl.RetrieveLicenciaDocente(dctx, licenciaEscuela, docente, usuario);
              UsuarioSocial usuarioSocial = new UsuarioSocial();

              usuarioSocial = licenciaDocente.UsuarioSocial;

              #region Registro de UsuarioSocial

              UsuarioSocial antusuariosocial = null;

              if (usuarioSocial != null)
              {
                  usuarioSocial = usuarioSocialCtrl.LastDataRowToUsuarioSocial(usuarioSocialCtrl.Retrieve(dctx, usuarioSocial));

                  UsuarioSocial originalUsrSocial = (UsuarioSocial)usuarioSocial.Clone();
                  originalUsrSocial.Estatus = true;
                  originalUsrSocial.ScreenName = docente.Nombre + " " + docente.PrimerApellido + ((docente.SegundoApellido == null || docente.SegundoApellido.Trim().Length == 0) ? "" : " " + docente.SegundoApellido);
                  originalUsrSocial.FechaNacimiento = docente.FechaNacimiento;
                  usuarioSocialCtrl.Update(dctx, usuarioSocial, originalUsrSocial);

                  antusuariosocial = usuarioSocialCtrl.LastDataRowToUsuarioSocial(usuarioSocialCtrl.Retrieve(dctx, new UsuarioSocial { UsuarioSocialID = originalUsrSocial.UsuarioSocialID }));

              }
              else
              {
                  usuarioSocial = new UsuarioSocial();
                  usuarioSocial.Email = docente.Correo;
                  usuarioSocial.LoginName = usuario.NombreUsuario;
                  usuarioSocial.ScreenName = docente.Nombre + " " + docente.PrimerApellido + ((docente.SegundoApellido == null || docente.SegundoApellido.Trim().Length == 0) ? "" : " " + docente.SegundoApellido);
                  usuarioSocial.FechaNacimiento = docente.FechaNacimiento;
                  usuario.FechaCreacion = DateTime.Now;
                  usuarioSocial.Estatus = true;

                  usuarioSocialCtrl.Insert(dctx, usuarioSocial);
                  antusuariosocial = usuarioSocialCtrl.LastDataRowToUsuarioSocial(usuarioSocialCtrl.Retrieve(dctx, usuarioSocial));
              }

              //Registrar licencia.
              licenciaEscuelaCtrl.InsertLicenciaDocente(dctx, licenciaEscuela, docente, usuario, antusuariosocial);

              #endregion

              #region Vincular Universidad - Docente

              #endregion
              dctx.CommitTransaction(firm);
              #endregion

              return usuario;
          }
          catch (Exception ex)
          {

              dctx.RollbackTransaction(firm);
              if (dctx.ConnectionState == ConnectionState.Open)
                  dctx.CloseConnection(firm);
              Logger.Service.LoggerHlp.Default.Error(this, ex);

              if (ex.Source == "Usuario" || ex.Source == "Telefono" || ex.Source == "Email")
                  throw new Exception("El " + ex.Source + " ya está en uso");

              throw new Exception("Ocurrió un error al registrar al Docente");
          }
          finally
          {
              if(dctx.ConnectionState == ConnectionState.Open)
                  dctx.CloseConnection(firm);
          }
          #endregion
      }

      /// <summary>
      /// Actualiza la información del docente.
      /// Actualiza la información del UsuarioSocial para la escuela
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="escuela">Escuela a consultar</param>
      /// <param name="ciclo">Ciclo escolar a consultar</param>
      /// <param name="docente">Información del docente a la escuela</param>
      public void UpdateDocenteEscuela(IDataContext dctx, Escuela escuela, CicloEscolar ciclo, Docente docente)
      {
          if (escuela == null || escuela.EscuelaID == null || escuela.EscuelaID <= 0)
              throw new Exception("Escuela es requerido");
          if (docente==null)
              throw new Exception("Docente es requerido");

          if (ciclo == null || ciclo.CicloEscolarID == null || ciclo.CicloEscolarID <= 0)
              throw new Exception("CicloEscolar es requerido");



          object firm = new object();
          try
          {
              dctx.OpenConnection(firm);
              Escuela descuela = escuelaCtrl.RetrieveComplete(dctx, escuela);

          #region Verificar licencia de la escuela

            
              LicenciaEscuela licenciaEscuela = new LicenciaEscuela { CicloEscolar = ciclo, Escuela = escuela, Activo = true };

              DataSet ds = licenciaEscuelaCtrl.Retrieve(dctx, licenciaEscuela);
              if (ds.Tables[0].Rows.Count == 0)
                  throw new Exception("La escuela no cuenta con licencia para el ciclo escolar especificado.");

              licenciaEscuela = licenciaEscuelaCtrl.LastDataRowToLicenciaEscuela(ds);
                 if (licenciaEscuela.ListaLicencia==null || !descuela.AsignacionDocentes.Any())
                     return;

              CicloEscolar cicloEscolar = cicloEscolarCtrl.LastDataRowToCicloEscolar(cicloEscolarCtrl.Retrieve(dctx,licenciaEscuela.CicloEscolar));
              Escuela escuelaActual = escuelaCtrl.LastDataRowToEscuela(escuelaCtrl.Retrieve(dctx, licenciaEscuela.Escuela));
            #endregion
             
          #region Verificar docente

                  ds = docenteCtrl.Retrieve(dctx, new Docente{DocenteID = docente.DocenteID,Estatus = true});
                    if(ds.Tables[0].Rows.Count!=1)
                        throw new Exception("Docente no encontrado");
                  Docente danterior = docenteCtrl.LastDataRowToDocente(ds);

            #endregion

          #region Verificar Usuario y Usuario Social  docente

            Usuario uanterior = licenciaEscuelaCtrl.RetrieveUsuario(dctx, danterior);
            if (uanterior.UsuarioID != null)
                uanterior = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, uanterior));

            LicenciaDocente licenciaDocente = licenciaEscuelaCtrl.RetrieveLicenciaDocente(dctx, licenciaEscuela,danterior,uanterior);
            if(licenciaDocente.LicenciaID==null)
                throw new Exception("Licencia del docente no encontrada");

            UsuarioSocial usocialanterior = usuarioSocialCtrl.RetrieveComplete(dctx, licenciaDocente.UsuarioSocial);
          #endregion

          #region Actualizar Docente
            dctx.BeginTransaction(firm);
           
            docente.Estatus = danterior.Estatus;
            docente.EstatusIdentificacion = danterior.EstatusIdentificacion;
            docente.FechaRegistro = danterior.FechaRegistro;
            docente.Clave = danterior.Clave;

            docenteCtrl.Update(dctx, docente, danterior);

            //Actualizar Usuario (Correo)
            Usuario usuario = (Usuario)uanterior.Clone();
            usuario.Email = docente.Correo;
            usuarioCtrl.Update(dctx,usuario,uanterior);

            //Actualizar UsuarioSocial
            UsuarioSocial usuarioSocial = (UsuarioSocial) usocialanterior.Clone();
            usuarioSocial.FechaNacimiento = docente.FechaNacimiento;
            usuarioSocial.ScreenName = docente.Nombre + " " + docente.PrimerApellido + ((docente.SegundoApellido == null || docente.SegundoApellido.Trim().Length == 0) ? "" : " " + docente.SegundoApellido);
            usuarioSocialCtrl.Update(dctx,usuarioSocial,usocialanterior);
            dctx.CommitTransaction(firm);
          #endregion

          }
          catch (Exception ex)
              {
                  dctx.RollbackTransaction(firm);
                  if (dctx.ConnectionState == ConnectionState.Open)
                      dctx.CloseConnection(firm);

                  Logger.Service.LoggerHlp.Default.Error(this, ex);

                  if (ex.Source == "Usuario" || ex.Source == "Telefono" || ex.Source == "Email")
                      throw new Exception("El " + ex.Source + " ya está en uso");

                  throw new Exception("Ocurrió un error al actualizar el docente");
              }
              finally
              {
                  if(dctx.ConnectionState == ConnectionState.Open)
                      dctx.CloseConnection(firm);
              }
          

      }
      public void DeleteDocenteEscuela(IDataContext dctx, Escuela escuela, CicloEscolar ciclo, Docente docente, List<AreaConocimiento> areasConocimiento, long universidadId)
      {
          if (escuela == null || escuela.EscuelaID == null || escuela.EscuelaID <= 0)
              throw new Exception("Escuela es requerido");

          if (docente==null)
              throw new Exception("Docente es requerido");

          if (ciclo == null || ciclo.CicloEscolarID == null || ciclo.CicloEscolarID <= 0)
              throw new Exception("CicloEscolar es requerido");
          object firm = new object();
          try
          {
              dctx.OpenConnection(firm);
              dctx.BeginTransaction(firm);

              EFDocenteCtrl efDocenteCtrl = new EFDocenteCtrl(null);

              int universidadesDocente = efDocenteCtrl.Retrieve(docente, false).SelectMany(s => s.Universidades).Where(x => x.UniversidadID != universidadId).Count();


              universidadesDocente = universidadesDocente + 0;
              #region Verificar licencia de la escuela


              LicenciaEscuela licenciaEscuela = new LicenciaEscuela { CicloEscolar = ciclo, Escuela = escuela, Activo = true };
              DataSet ds = licenciaEscuelaCtrl.Retrieve(dctx, licenciaEscuela);
              if (ds.Tables[0].Rows.Count == 0)
                  throw new Exception("La escuela no cuenta con licencia para el ciclo escolar especificado.");

              licenciaEscuela = licenciaEscuelaCtrl.LastDataRowToLicenciaEscuela(ds);

              CicloEscolar cicloEscolar = cicloEscolarCtrl.LastDataRowToCicloEscolar(cicloEscolarCtrl.Retrieve(dctx, licenciaEscuela.CicloEscolar));
              Escuela escuelaActual = escuelaCtrl.RetrieveComplete(dctx, licenciaEscuela.Escuela);
              #endregion

              #region Verificar docente
              ds = docenteCtrl.Retrieve(dctx, docente);
              int index = ds.Tables["Docente"].Rows.Count;

              if (index <= 0)
                  throw new Exception("Docente no encontrado");

              docente = docenteCtrl.DataRowToDocente(ds.Tables["Docente"].Rows[index - 1]);
              Usuario usuario = new Usuario();

              if (universidadId != null)
                  usuario = licenciaEscuelaCtrl.RetrieveUsuarios(dctx, docente).Where(x => x.UniversidadId == universidadId).FirstOrDefault();
              else
                  usuario = licenciaEscuelaCtrl.RetrieveUsuario(dctx, docente);

              #endregion

              if (universidadesDocente == 0)
              {
                  #region Eliminacion de las materias asignadas al docente
                  List<Materia> lsmaterias = grupoCicloEscolarCtrl.RetrieveMateriasDocente(dctx, docente, new GrupoCicloEscolar { CicloEscolar = cicloEscolar, Escuela = escuelaActual });
                  foreach (Materia materia in lsmaterias)
                  {
                      var grupoCicloEscolar = grupoCicloEscolarCtrl.LastDataRowToGrupoCicloEscolar(grupoCicloEscolarCtrl.Retrieve(dctx, new GrupoCicloEscolar { CicloEscolar = cicloEscolar, Escuela = escuelaActual }));
                      grupoCicloEscolarCtrl.RemoveAsignacionDocente(dctx, docente, materia, grupoCicloEscolar);
                  }
                  #endregion
                  //}
                  //if (universidadesDocente == 0)
                  //{
                  escuelaCtrl.DeleteAignacionesDocente(dctx, new AsignacionDocenteEscuela { Docente = docente }, escuelaActual);
                  licenciaEscuelaCtrl.DeleteLicenciasDocente(dctx, licenciaEscuela, docente, usuario);
                  //}
              }
              #region Baja de la Red Social en la escuela y ciclo Escolar
              try
              {
                  DeleteDocenteGruposSociales(dctx, licenciaEscuela, new GrupoCicloEscolar { Escuela = escuelaActual, CicloEscolar = cicloEscolar }, docente, areasConocimiento, universidadId);
              }
              catch (Exception ex)
              {
                  Logger.Service.LoggerHlp.Default.Error(this, ex);
                  throw new Exception("Ocurrió un error al eliminar al docente de la red social");
              }

              #endregion

              if (universidadesDocente == 0)              
              {
                  #region Actualizar estatus docente
                  Docente anterior = (Docente)docente.Clone();
                  docente.Estatus = false;
                  docenteCtrl.Update(dctx, docente, anterior);
                  #endregion
              }
              dctx.CommitTransaction(firm);
          }
          catch (Exception ex)
          {
              dctx.RollbackTransaction(firm);
              if (dctx.ConnectionState == ConnectionState.Open)
                  dctx.CloseConnection(firm);

              Logger.Service.LoggerHlp.Default.Error(this, ex);
              if (ex.Source == "Usuario" || ex.Source == "Telefono" || ex.Source == "Email")
                  throw;

              throw new Exception("Ocurrió un error mientras se eliminaba el docente");
          }
          finally
          {
              if(dctx.ConnectionState== ConnectionState.Open)
                  dctx.CloseConnection(firm);
              
          }
      }

      private void DeleteDocenteGruposSociales(IDataContext dctx, LicenciaEscuela licenciaEscuela,GrupoCicloEscolar grupoCicloEscolar,Docente docente, List<AreaConocimiento> areasConocimiento, long universidadID)
      {
          if (licenciaEscuela == null || docente == null || grupoCicloEscolar==null)
              return;
          
          Usuario usuario = licenciaEscuelaCtrl.RetrieveUsuarios(dctx, docente).Where(x=>x.UniversidadId==universidadID).FirstOrDefault();
          usuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, usuario));

          LicenciaDocente licenciaDocente = licenciaEscuelaCtrl.RetrieveLicenciaDocente(dctx, licenciaEscuela, docente, usuario);
          UsuarioSocial usuarioSocial = licenciaDocente.UsuarioSocial;

          if (usuarioSocial.UsuarioSocialID != null)
          {
              //Actualizar el estado del UsuarioSocial del Docente
              DataSet ds = usuarioSocialCtrl.Retrieve(dctx, usuarioSocial);
              if (ds.Tables[0].Rows.Count != 1)
                  throw new Exception("DeleteDocenteGruposSociales:UsuarioSocial no encontrado");

              usuarioSocial = usuarioSocialCtrl.LastDataRowToUsuarioSocial(ds);

              SocialHub socialHub = new SocialHub();
              socialHub.SocialProfile = usuarioSocial;
              socialHub.SocialProfileType = ESocialProfileType.USUARIOSOCIAL;
              ds = socialHubCtrl.RetrieveSocialHubUsuario(dctx, socialHub);

              if (ds.Tables["SocialHub"].Rows.Count <= 0) //El docente no tiene socialHub
                  return;
              socialHub = socialHubCtrl.LastDataRowToSocialHub(ds);
              List<GrupoSocial> lsGrupoSocial = (new GrupoSocialCtrl()).RetrieveGruposSocialSocialHub(dctx, socialHub);


              DataSet dtGrupoCiclo = grupoCicloEscolarCtrl.Retrieve(dctx, grupoCicloEscolar);
              int index = dtGrupoCiclo.Tables["GrupoCicloEscolar"].Rows.Count;
              if (index > 0)
                  foreach (DataRow drgrupoCiclo in dtGrupoCiclo.Tables["GrupoCicloEscolar"].Rows)
                  {
                      GrupoCicloEscolar grupoCiclo = grupoCicloEscolarCtrl.DataRowToGrupoCicloEscolar(drgrupoCiclo);
                      if (lsGrupoSocial.Exists(gr => gr.GrupoSocialID == grupoCiclo.GrupoSocialID))
                          socialHubCtrl.DesasociarSocialHubGrupoSocialUsuarioGrupo(dctx, socialHub,
                                                                                   new GrupoSocial { GrupoSocialID = grupoCiclo.GrupoSocialID }, areasConocimiento, universidadID);
                  }

              UsuarioSocial anterior = (UsuarioSocial)usuarioSocial.Clone();
              usuarioSocial.Estatus = false;
              usuarioSocialCtrl.Update(dctx, usuarioSocial, anterior);

              Usuario usanterior = (Usuario)usuario.Clone();
              usuario.EsActivo = false;
              usuarioCtrl.Update(dctx, usuario, usanterior);

          }
      }

      public bool EmailDisponible(IDataContext dctx, Usuario usuario)
      {
          if (usuario == null || string.IsNullOrEmpty(usuario.Email))
              throw new ArgumentException("Email requerido", "usuario");

          Usuario usr = new Usuario();
          if (usuario.Email != null)
              usr.Email = usuario.Email;
          usr.EsActivo = true;
          if (usuario.UniversidadId != null)
              usr.UniversidadId = usuario.UniversidadId;
          
          DataSet ds = usuarioCtrl.Retrieve(dctx, usr);
          int index = ds.Tables[0].Rows.Count;
          if (index == 1 && usuario.UsuarioID!=null)
          {
              Usuario user = usuarioCtrl.LastDataRowToUsuario(ds);
              return usr.UsuarioID == usuario.UsuarioID;
          }

          if (index <= 0)
              return true;

          return false;
      }

      public bool TelefonoDisponible(IDataContext dctx, Usuario usuario)
      {
          if (usuario == null || string.IsNullOrEmpty(usuario.TelefonoReferencia))
              throw new ArgumentException("Teléfono requerido", "usuario");

          Usuario usr = new Usuario();
          if (usuario.TelefonoReferencia != null)
              usr.Email = usuario.TelefonoReferencia;
          usr.EsActivo = true;
          if (usuario.UniversidadId != null)
              usr.UniversidadId = usuario.UniversidadId;

          DataSet ds = usuarioCtrl.Retrieve(dctx, usr);

          int index = ds.Tables[0].Rows.Count;
          if (index == 1 && usuario.UsuarioID!=null)
          {
              Usuario user = usuarioCtrl.LastDataRowToUsuario(ds);
              return usr.UsuarioID == usuario.UsuarioID;
          }

          if (index <= 0)
              return true;

          return false;
      }

      public Docente GetDocenteByCurp(IDataContext dctx, Docente docente)
      {
          Docente obj = new Docente();
          DataSet ds = docenteCtrl.Retrieve(dctx, new Docente { Curp = docente.Curp });
          int index = ds.Tables["Docente"].Rows.Count;

          if (index == 1)
          {
              obj = docenteCtrl.DataRowToDocente(ds.Tables["Docente"].Rows[index - 1]);
          }

          return obj;
      }
    }
}
