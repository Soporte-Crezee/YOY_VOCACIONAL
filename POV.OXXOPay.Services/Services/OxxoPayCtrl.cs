using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.CentroEducativo.Services;
using POV.Licencias.BO;
using POV.Licencias.Service;
using POV.Modelo.Context;
using POV.OXXOPay.Services.AppCode;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using System;
using System.Collections.Generic;


namespace POV.OXXOPay.Service
{
    public class OxxoPayCtrl
    {
        public OxxoPayCtrl()
        {
        }

        public string ActivateUser(String strIDReferenciaOXXO)
        {
            string result = "";
            try
            {
                EFAlumnoCtrl efAlumnoCtrl = new EFAlumnoCtrl(null);

                List<Alumno> alumnos = efAlumnoCtrl.Retrieve(new Alumno() { IDReferenciaOXXO = strIDReferenciaOXXO }, true);
                foreach (Alumno alumno in alumnos)
                {
                    alumno.EstatusPago = EEstadoPago.PAGADO;
                    efAlumnoCtrl.Update(alumno);
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }

        public string RetrieveUser(String email)
        {
            string result = "";
            try
            {
                UsuarioCtrl usuarioCtrl = new UsuarioCtrl();
                Usuario usuario = usuarioCtrl.RetrieveComplete(ConnectionHelper.Default.Connection, new Usuario { Email = email });
                ALicencia licencia = null;
                if (usuario != null)
                {
                    LicenciaEscuelaCtrl licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
                    List<LicenciaEscuela> licenciasEscuela = licenciaEscuelaCtrl.RetrieveLicencia(ConnectionHelper.Default.Connection, usuario);
                    foreach (LicenciaEscuela licenciaEscuela in licenciasEscuela)
                    {
                        // buscamos la licencia
                        licencia = licenciaEscuela.ListaLicencia.Find(item => (bool)item.Activo && item.Tipo != ETipoLicencia.DIRECTOR);
                    }
                }
                LicenciaEscuelaCtrl licenciaCtrl = new LicenciaEscuelaCtrl();
                AlumnoCtrl alumnoCtrl = new AlumnoCtrl();

                Alumno alumno;
                Tutor tutor;
                if (licencia != null)
                {
                    if (licencia.Tipo == ETipoLicencia.TUTOR)
                    {
                        TutorCtrl tutorCtrl=new TutorCtrl(null);
                        tutor = licenciaCtrl.RetrieveUsuarioTutor(ConnectionHelper.Default.Connection, usuario);
                        tutor = tutorCtrl.Retrieve(new Tutor { TutorID = tutor.TutorID, NotificacionPago = tutor.NotificacionPago }, false)[0];

                        if (tutor != null)
                            result = "Notificar";
                    }
                    else if (licencia.Tipo == ETipoLicencia.ALUMNO) 
                    {
                        alumno = licenciaCtrl.RetrieveUsuarioAlumno(ConnectionHelper.Default.Connection, usuario);
                        alumno = alumnoCtrl.LastDataRowToAlumno(alumnoCtrl.Retrieve(ConnectionHelper.Default.Connection, new Alumno { AlumnoID = alumno.AlumnoID, NotificacionPago = false }));
                        if (alumno != null)
                            result = "Notificar";
                    }
                }

                
                

                //EFAlumnoCtrl efAlumnoCtrl = new EFAlumnoCtrl(null);

                //List<Alumno> alumnos = efAlumnoCtrl.Retrieve(new Alumno() { IDReferenciaOXXO = strIDReferenciaOXXO }, true);
                //foreach (Alumno alumno in alumnos)
                //{
                //    alumno.EstatusPago = EEstadoPago.PAGADO;
                //    efAlumnoCtrl.Update(alumno);
                //}
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }
    }
}
