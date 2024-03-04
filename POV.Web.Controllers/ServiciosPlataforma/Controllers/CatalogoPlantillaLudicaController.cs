using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POV.AmazonAWS.Services;
using POV.Comun.BO;
using POV.Comun.Service;
using POV.ConfiguracionesPlataforma.BO;
using POV.ConfiguracionesPlataforma.Services;
using POV.Modelo.Context;

namespace POV.Web.Controllers.ServiciosPlataforma.Controllers
{
    public class CatalogoPlantillaLudicaController
    {
         private readonly Contexto model;
        private readonly object sign;

        public CatalogoPlantillaLudicaController(Contexto contexto)
        {
            sign = new object();
            model = contexto ?? new Contexto(sign);
        }

        #region Consultar
        public List<PlantillaLudica> ConsultarPlantillasLudicas(PlantillaLudica plantilla)
        {
            PlantillaLudicaCtrl plantillaCtrl = new PlantillaLudicaCtrl(model);
            List<PlantillaLudica> lstPlantilla = new List<PlantillaLudica>();
            lstPlantilla = plantillaCtrl.Retrieve(plantilla, false);
            return lstPlantilla;
        }

        public List<PlantillaLudica> ConsultarPlantillaLudicaCompleta(PlantillaLudica plantilla, bool isTracking)
        {
            PlantillaLudicaCtrl plantillaCtrl = new PlantillaLudicaCtrl(model);
            List<PlantillaLudica> lstPlantilla = new List<PlantillaLudica>();
            lstPlantilla = plantillaCtrl.RetrieveWithRelationship(plantilla, isTracking);
            return lstPlantilla;
        }

        public List<ConfiguracionGeneral> ConsultarConfiguraciónGeneral(ConfiguracionGeneral configuracion)
        {
            ConfiguracionGeneralCtrl configuracionCtrl = new ConfiguracionGeneralCtrl(model);
            List<ConfiguracionGeneral> lstConfiguracion = new List<ConfiguracionGeneral>();
            lstConfiguracion = configuracionCtrl.Retrieve(configuracion, false);
            return lstConfiguracion;
        }

        public List<PreferenciaUsuario> ConsultarPlantillaUsada(PlantillaLudica plantilla)
        {
            PreferenciaUsuarioCtrl preferenciaCtrl = new PreferenciaUsuarioCtrl(model);
            List<PreferenciaUsuario> lstPreferencia = new List<PreferenciaUsuario>();
            PreferenciaUsuario preferencia = new PreferenciaUsuario();
            preferencia.PlantillaLudicaId = plantilla.PlantillaLudicaId;
            preferencia.PlantillasLudicas = new PlantillaLudica();
            preferencia.PlantillasLudicas.PlantillaLudicaId = plantilla.PlantillaLudicaId;
            lstPreferencia = preferenciaCtrl.Retrieve(preferencia, false);
            return lstPreferencia;
        }
        #endregion

        #region Registrar
        public bool RegistrarPlantillaLudica(PlantillaLudica plantilla, List<FileWrapper> files, string servidorContenidos, string username, string password, string metodoServidor)
        {
            PlantillaLudicaCtrl plantillaCtrl = new PlantillaLudicaCtrl(model);

            PlantillaLudica coincidencias = new PlantillaLudica();
            coincidencias.Nombre = plantilla.Nombre;
            bool registrado = false;
            if (plantillaCtrl.Retrieve(coincidencias, false).Count == 0)
            {
                registrado = plantillaCtrl.Insert(plantilla);
                model.Commit(sign);
                model.Dispose();

                if (files.Count > 0)
                {

                    foreach (FileWrapper file in files)
                    {
                        switch (metodoServidor)
                        {
                            case "FTP":
                                if (string.IsNullOrEmpty(username)) throw new ArgumentNullException("CatalogoPlantillaLudicaController: el username del servidor es requerido");
                                if (string.IsNullOrEmpty(password)) throw new ArgumentNullException("CatalogoPlantillaLudicaController: el password del servidor es requerido");

                                FtpFileUploadCtrl ftpUploadCtrl = new FtpFileUploadCtrl(servidorContenidos, username, password);
                                ftpUploadCtrl.UploadFile(file, file.Name);
                                break;
                            case "LOCAL":
                                LocalFileUploadCtrl localUploadCtrl = new LocalFileUploadCtrl(servidorContenidos, username, password);
                                localUploadCtrl.UploadFile(file, file.Name);
                                break;
                            case "AMAZONS3":
                                AmazonS3ClientCtrl clientS3 = new AmazonS3ClientCtrl(username, password);
                                clientS3.UploadFile(servidorContenidos, file);
                                break;
                            default:
                                throw new NotSupportedException("CatalogoPlantillaLudicaController: Metodo no soportado, use : AMAZONS3, FTP o LOCAL como metodo");
                        }
                    }
                }
            }
            else
            {
                throw new Exception("La plantilla ya existe. Por favor verifique");
            }
            return registrado;
        }
        #endregion

        #region Actualizar
        public bool ActualizarPlantillaLudica(PlantillaLudica plantilla, List<int> posicionesEliminadas, List<FileWrapper> files, string servidorContenidos, string username, string password, string metodoServidor)
        {
            PlantillaLudicaCtrl plantillaCtrl = new PlantillaLudicaCtrl(model);

            PlantillaLudica coincidencias = new PlantillaLudica();
            coincidencias.Nombre = plantilla.Nombre;
            bool actualizado = false;
            List<PlantillaLudica> plantillaEncontrada = plantillaCtrl.Retrieve(coincidencias, false);
            if (plantillaEncontrada.Count == 0 || plantillaEncontrada.FirstOrDefault().Nombre == plantilla.Nombre)
            {
                actualizado = plantillaCtrl.Update(plantilla, posicionesEliminadas);
                model.Commit(sign);
                model.Dispose();

                if (files.Count > 0)
                {

                    foreach (FileWrapper file in files)
                    {
                        switch (metodoServidor)
                        {
                            case "FTP":
                                if (string.IsNullOrEmpty(username)) throw new ArgumentNullException("CatalogoPlantillaLudicaController: el username del servidor es requerido");
                                if (string.IsNullOrEmpty(password)) throw new ArgumentNullException("CatalogoPlantillaLudicaController: el password del servidor es requerido");

                                FtpFileUploadCtrl ftpUploadCtrl = new FtpFileUploadCtrl(servidorContenidos, username, password);
                                ftpUploadCtrl.UploadFile(file, file.Name);
                                break;
                            case "LOCAL":
                                LocalFileUploadCtrl localUploadCtrl = new LocalFileUploadCtrl(servidorContenidos, username, password);
                                localUploadCtrl.UploadFile(file, file.Name);
                                break;
                            case "AMAZONS3":
                                AmazonS3ClientCtrl clientS3 = new AmazonS3ClientCtrl(username, password);
                                clientS3.UploadFile(servidorContenidos, file);
                                break;
                            default:
                                throw new NotSupportedException("CatalogoPlantillaLudicaController: Metodo no soportado, use : AMAZONS3, FTP o LOCAL como metodo");
                        }
                    }
                }
            }
            else
            {
                throw new Exception("La plantilla ya existe. Por favor verifique");
            }
            return actualizado;
        }
        #endregion

        #region Eliminar
        public void EliminarPlantillaLudica(PlantillaLudica plantilla)
        {
            try
            {
                PlantillaLudicaCtrl plantillaCtrl = new PlantillaLudicaCtrl(model);
                List<PlantillaLudica> plantillaEliminar = plantillaCtrl.Retrieve(plantilla, true);
                plantillaEliminar.FirstOrDefault().Activo = false;
                plantillaCtrl.Update(plantillaEliminar.FirstOrDefault(), null);
                model.Commit(sign);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public void Dispose()
        {
            model.Disposing(this.sign);
        }
    }
}
