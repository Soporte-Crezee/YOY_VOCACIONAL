using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POV.ConfiguracionesPlataforma.BO;
using POV.ConfiguracionesPlataforma.Services;
using POV.Modelo.Context;

namespace POV.Web.Controllers.ServiciosPlataforma.Controllers
{
    public class ConfiguracionGeneralController
    {
        private readonly Contexto model;
        private readonly object sign;

        public ConfiguracionGeneralController(Contexto contexto)
        {
            sign = new object();
            model = contexto ?? new Contexto(sign);
        }

        public List<ConfiguracionGeneral> ConsultarConfiguracionesGenerales(ConfiguracionGeneral configuracion)
        {
            ConfiguracionGeneralCtrl configuracionCtrl = new ConfiguracionGeneralCtrl(model);
            List<ConfiguracionGeneral> lstConfiguracion = new List<ConfiguracionGeneral>();
            lstConfiguracion = configuracionCtrl.Retrieve(configuracion, true);
            return lstConfiguracion;
        }

        public Boolean InsertarConfiguracionesGenerales(ConfiguracionGeneral configuracion)
        {
            ConfiguracionGeneralCtrl configuracionCtrl = new ConfiguracionGeneralCtrl(model);

            bool registrado = false;
            registrado = configuracionCtrl.Insert(configuracion);
            model.Commit(sign);
            model.Dispose();

            return registrado;
        }

        public Boolean ActualizarConfiguracionesgenerales(ConfiguracionGeneral configuracion)
        {
            ConfiguracionGeneralCtrl configuracionCtrl = new ConfiguracionGeneralCtrl(model);

            bool actualizado = false;

            actualizado = configuracionCtrl.Update(configuracion);
            model.Commit(sign);
            model.Dispose();

            return actualizado;
        }

    }
}
