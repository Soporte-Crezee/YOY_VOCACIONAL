using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Framework.Base.DataAccess;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Licencias.BO;
using POV.Licencias.Service;
using POV.Seguridad.BO;
using GP.SocialEngine.BO;
using GP.SocialEngine.Service;

namespace POV.Administracion.Service
{
   public class AsignarDocenteGrupoCicloEscolarCtrl
   {
       private GrupoCicloEscolarCtrl grupoCicloEscolarCtrl;
       private DocenteCtrl docenteCtrl;
       private DocenteEscuelaCtrl docenteEscuelaCtrl;
       private SocialHubCtrl socialHubCtrl;
       private UsuarioSocialCtrl usuarioSocialCtrl;
       private GrupoSocialCtrl grupoSocialCtrl;
       private MateriaCtrl materiaCtrl;
       private LicenciaEscuelaCtrl licenciaEscuelaCtrl;

       public  AsignarDocenteGrupoCicloEscolarCtrl()
       {
           grupoCicloEscolarCtrl = new GrupoCicloEscolarCtrl();
           docenteCtrl = new DocenteCtrl();
           docenteEscuelaCtrl = new DocenteEscuelaCtrl();
           socialHubCtrl = new SocialHubCtrl();
           usuarioSocialCtrl = new UsuarioSocialCtrl();
           grupoSocialCtrl = new GrupoSocialCtrl();
           materiaCtrl = new MateriaCtrl();
           licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
         
       }
       public void InsertAsignacionDocenteGrupoCicloEscolar(IDataContext dctx,Docente docente,Materia materia,GrupoCicloEscolar grupoCicloEscolar, List<AreaConocimiento> areasConocimiento, long universidadID)
       {
           if(docente==null || docente.DocenteID==null || docente.DocenteID.Value<=0)
               throw new ArgumentException("Docente no puede ser nulo");

           if (grupoCicloEscolar == null || grupoCicloEscolar.GrupoCicloEscolarID == null)
               throw new ArgumentException("CicloEscolar es requerido.");

           if(materia==null || materia.MateriaID==null || materia.MateriaID.Value<=0)
               throw new ArgumentException("Materia es requerida");

           object firm= new object();

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
               DataSet ds = docenteEscuelaCtrl.Retrieve(dctx, new DocenteEscuela { DocenteID = docente.DocenteID, Estatus = true, EscuelaID = grupoCicloEscolar.Escuela.EscuelaID });
               if (ds.Tables[0].Rows.Count <= 0)
                   throw new Exception("El docente no se encuentra asignado a la escuela proporcionada");

               
               docente = docenteCtrl.RetrieveComplete(dctx,new Docente{DocenteID = docente.DocenteID,Estatus = true});
               grupoCicloEscolarCtrl.InsertAsignacionDocente(dctx,new Docente{DocenteID = docente.DocenteID,Estatus = true}, materia, grupoCicloEscolar);
               InsertDocenteGrupoSocial(dctx,grupoCicloEscolar,docente, areasConocimiento, universidadID);
               dctx.CommitTransaction(firm);
           }
           catch (Exception ex)
           {
               POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
               dctx.RollbackTransaction(firm);   
           }
           finally
           {
               if(dctx.ConnectionState == ConnectionState.Open)
                   dctx.CloseConnection(firm);
           }
       }
       public void DeleteAsignacionDocenteGrupoCicloEscolar(IDataContext dctx, AsignacionMateriaGrupo asignacionMateria, GrupoCicloEscolar grupoCicloEscolar, List<AreaConocimiento> areasConocimiento, long universidadID)
       {

           if (asignacionMateria.Docente == null || asignacionMateria.Docente.DocenteID == null || asignacionMateria.Docente.DocenteID.Value <= 0)
               throw new ArgumentException("Docente no puede ser nulo");

           if (grupoCicloEscolar == null || grupoCicloEscolar.GrupoCicloEscolarID == null)
               throw new ArgumentException("CicloEscolar es requerido.");

           if (asignacionMateria.Materia == null || asignacionMateria.Materia.MateriaID == null || asignacionMateria.Materia.MateriaID.Value <= 0)
               throw new ArgumentException("Materia es requerida");

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
               DataSet ds = docenteEscuelaCtrl.Retrieve(dctx, new DocenteEscuela { DocenteID = asignacionMateria.Docente.DocenteID, Estatus = true, EscuelaID = grupoCicloEscolar.Escuela.EscuelaID });
               if (ds.Tables["DocenteEscuela"].Rows.Count != 1)
                   throw new Exception("El docente no se encuentra asignado a la escuela proporcionada");

               DocenteEscuela docenteEscuela = docenteEscuelaCtrl.LastDataRowToDocenteEscuela(ds);
               ds.Clear();  ds = docenteCtrl.Retrieve(dctx, new Docente {DocenteID = docenteEscuela.DocenteID, Estatus = true});
               if(ds.Tables["Docente"].Rows.Count!=1)
                   throw new Exception("El docente no se encuentra");

               AsignacionMateriaGrupo asignacion = new AsignacionMateriaGrupo();
               asignacion.Docente = docenteCtrl.LastDataRowToDocente(ds);
               ds.Clear(); ds = materiaCtrl.Retrieve(dctx, new Materia { MateriaID = asignacionMateria.Materia.MateriaID});
               if(ds.Tables["Materia"].Rows.Count!=1)
                    throw new Exception("La materia proporcionada no se encuentra");

               asignacion.Materia = materiaCtrl.LastDataRowToMateria(ds);

               List<Materia> matdocente = grupoCicloEscolarCtrl.RetrieveMateriasDocente(dctx,asignacion.Docente, grupoCicloEscolar);
               if (matdocente.Any())
               {
                   if(!matdocente.Exists(mat=> mat.MateriaID==asignacion.Materia.MateriaID))
                       throw new Exception("El docente proporcionada no se encuentra asignado a la materia");

                  
                   grupoCicloEscolarCtrl.RemoveAsignacionDocente(dctx, asignacion.Docente, asignacion.Materia, grupoCicloEscolar);
                   DeleteDocenteGrupoSocial(dctx, grupoCicloEscolar, asignacion.Docente, areasConocimiento, universidadID);
                   dctx.CommitTransaction(firm);
               }  
           }
           catch (Exception ex)
           {
               POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
               dctx.RollbackTransaction(firm);
           }
           finally
           {
               if (dctx.ConnectionState == ConnectionState.Open)
                   dctx.CloseConnection(firm);
           }
           
       }
       private void InsertDocenteGrupoSocial(IDataContext dctx,GrupoCicloEscolar grupoCicloEscolar,Docente docente, List<AreaConocimiento> areasConocimiento, long universidadID)
       {
           if (grupoCicloEscolar == null || grupoCicloEscolar.GrupoCicloEscolarID == null || grupoCicloEscolar.Escuela == null || grupoCicloEscolar.Escuela.EscuelaID == null)
               throw new Exception("InsertDocenteGrupoSocial:GrupoCicloEscolar no puede ser nulo");
           if (docente == null || docente.DocenteID == null || docente.DocenteID <= 0)
               throw new Exception("InsertDocenteGrupoSocial:Docente no puede ser nulo");

           DataSet ds = grupoCicloEscolarCtrl.Retrieve(dctx, grupoCicloEscolar);
           if(ds.Tables[0].Rows.Count<=0)
               throw new Exception("InsertDocenteGrupoSocial:GrupoCicloEscolar no se encontró");

           grupoCicloEscolar = grupoCicloEscolarCtrl.RetriveComplete(dctx, grupoCicloEscolar);ds.Clear();

           ds = (docenteEscuelaCtrl).Retrieve(dctx, new DocenteEscuela { DocenteID = docente.DocenteID, Estatus = true, EscuelaID = grupoCicloEscolar.Escuela.EscuelaID });
           if (ds.Tables[0].Rows.Count <= 0)
               throw new Exception("El docente no se encuentra asignado a la escuela proporcionada");

           DocenteEscuela docenteEscuela = docenteEscuelaCtrl.LastDataRowToDocenteEscuela(ds);
           docente = docenteCtrl.RetrieveComplete(dctx,new Docente{DocenteID = docenteEscuela.DocenteID,Estatus = true});

           #region Verificar licencia de la escuela

           LicenciaEscuela licenciaEscuela = new LicenciaEscuela { CicloEscolar = grupoCicloEscolar.CicloEscolar, Escuela = grupoCicloEscolar.Escuela, Activo = true };

           ds = licenciaEscuelaCtrl.Retrieve(dctx, licenciaEscuela);
           if (ds.Tables[0].Rows.Count == 0)
               throw new Exception("La escuela no cuenta con licencia para el ciclo escolar especificado.");

           licenciaEscuela = licenciaEscuelaCtrl.LastDataRowToLicenciaEscuela(ds);

           #endregion

           UsuarioSocial usuarioSocial = (new LicenciaEscuelaCtrl()).RetrieveUsuarioSocial(dctx,licenciaEscuela,docente);        

           #region Registrar
           if (usuarioSocial.UsuarioSocialID != null)
           {
             
               usuarioSocial = usuarioSocialCtrl.LastDataRowToUsuarioSocial(usuarioSocialCtrl.Retrieve(dctx, usuarioSocial));

               InformacionSocialCtrl infoSocialCtrl = new InformacionSocialCtrl();
               InformacionSocial infoSocial = new InformacionSocial();
               SocialHub socialHub = new SocialHub();
               GrupoSocial grupoSocial = new GrupoSocial {GrupoSocialID = grupoCicloEscolar.GrupoSocialID};

               #region Registrar SocialHub
               socialHub.SocialProfile = usuarioSocial;
               socialHub.SocialProfileType = ESocialProfileType.USUARIOSOCIAL;

               ds = socialHubCtrl.RetrieveSocialHubUsuario(dctx, socialHub);
               if (ds.Tables[0].Rows.Count == 0)
               {
                   #region Registrar Informacion Social
                   infoSocial.FechaRegistro = DateTime.Now;

                   infoSocialCtrl.Insert(dctx, infoSocial);
                   infoSocial = infoSocialCtrl.LastDataRowToInformacionSocial(infoSocialCtrl.Retrieve(dctx, infoSocial));
                   #endregion

                   socialHub.InformacionSocial = infoSocial;
                   socialHub.FechaRegistro = DateTime.Now;
                   socialHub.Alias = "Mi Patio";
                   socialHubCtrl.InsertSocialHubUsuario(dctx, socialHub);
                   ds = socialHubCtrl.RetrieveSocialHubUsuario(dctx, socialHub);
               }
               socialHub = socialHubCtrl.LastDataRowToSocialHub(ds);
               #endregion
               socialHubCtrl.AsociarSocialHubGrupoSocialUsuarioGrupo(dctx, socialHub, grupoSocial, true, areasConocimiento, universidadID);
               
           }

           #endregion
       }
       private void DeleteDocenteGrupoSocial(IDataContext dctx,GrupoCicloEscolar grupoCicloEscolar,Docente docente, List<AreaConocimiento> areasConocimiento, long universidadID)
       {
           if (grupoCicloEscolar == null || grupoCicloEscolar.GrupoCicloEscolarID == null || grupoCicloEscolar.Escuela == null || grupoCicloEscolar.Escuela.EscuelaID == null)
               throw new Exception("DeleteDocenteGrupoSocial:GrupoCicloEscolar no puede ser nulo");
           if (docente == null || docente.DocenteID == null || docente.DocenteID <= 0)
               throw new Exception("DeleteDocenteGrupoSocial:Docente no puede ser nulo");

           DataSet ds = grupoCicloEscolarCtrl.Retrieve(dctx, grupoCicloEscolar);
           if (ds.Tables["GrupoCicloEscolar"].Rows.Count <= 0)
               throw new Exception("DeleteDocenteGrupoSocial:GrupoCicloEscolar no se encontró");

           grupoCicloEscolar = grupoCicloEscolarCtrl.RetriveComplete(dctx, grupoCicloEscolar); ds.Clear();

           ds = docenteEscuelaCtrl.Retrieve(dctx, new DocenteEscuela {DocenteID = docente.DocenteID,Estatus = true});
           if (ds.Tables["DocenteEscuela"].Rows.Count <= 0)
               throw new Exception("DeleteDocenteGrupoSocial:Docente no se encontró en la escuela");

           DocenteEscuela docenteEscuela = docenteEscuelaCtrl.LastDataRowToDocenteEscuela(ds);
           docente = docenteCtrl.RetrieveComplete(dctx, new Docente { DocenteID = docenteEscuela.DocenteID, Estatus = true });

           //obtener la lista de materias asignadas al docente
           List<Materia> lsmaterias = grupoCicloEscolarCtrl.RetrieveMateriasDocente(dctx, docente, grupoCicloEscolar);

           if (lsmaterias.Count >= 1) //El docente tiene más de una materia asignada al grupo ciclo escolar
               return;

           #region Verificar licencia de la escuela

           LicenciaEscuela licenciaEscuela = new LicenciaEscuela { CicloEscolar = grupoCicloEscolar.CicloEscolar, Escuela = grupoCicloEscolar.Escuela, Activo = true };

           ds = licenciaEscuelaCtrl.Retrieve(dctx, licenciaEscuela);
           if (ds.Tables[0].Rows.Count == 0)
               throw new Exception("La escuela no cuenta con licencia para el ciclo escolar especificado.");

           licenciaEscuela = licenciaEscuelaCtrl.LastDataRowToLicenciaEscuela(ds);

           #endregion


           UsuarioSocial usuarioSocial = (new LicenciaEscuelaCtrl()).RetrieveUsuarioSocial(dctx,licenciaEscuela, docente);
           if (usuarioSocial.UsuarioSocialID != null)
           {
               usuarioSocial = usuarioSocialCtrl.LastDataRowToUsuarioSocial(usuarioSocialCtrl.Retrieve(dctx, usuarioSocial));      
               SocialHub socialHub = new SocialHub();
               GrupoSocial grupoSocial = new GrupoSocial { GrupoSocialID = grupoCicloEscolar.GrupoSocialID };
               socialHub.SocialProfile = usuarioSocial;
               socialHub.SocialProfileType = ESocialProfileType.USUARIOSOCIAL;
               ds = socialHubCtrl.RetrieveSocialHubUsuario(dctx, socialHub);

               if (ds.Tables["SocialHub"].Rows.Count <= 0) //El docente no tiene socialHub
                   return;
      
               socialHub = socialHubCtrl.LastDataRowToSocialHub(ds);
               List<GrupoSocial> lsgruposocial = grupoSocialCtrl.RetrieveGruposSocialSocialHub(dctx, socialHub);
               if (!lsgruposocial.Exists(gr => gr.GrupoSocialID == grupoCicloEscolar.GrupoSocialID)) //El docente no esta asignado al grupo social
                   return;
               
               if (lsmaterias.Count <= 0)
                   socialHubCtrl.DesasociarSocialHubGrupoSocialUsuarioGrupo(dctx, socialHub, grupoSocial, areasConocimiento, universidadID);
           }

       }
    }
}
