using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using  POV.ConfiguracionActividades.BO;
using POV.ConfiguracionActividades.Services;

namespace POV.ServiciosActividades.Controllers
{
    /// <summary>
    /// Controlador de UI para la configuracion de bloques
    /// </summary>
    public class ConfigurarBloquesActividadController : IDisposable
    {
        /// <summary>
        /// Controlador de los bloques
        /// </summary>
        private BloqueActividadCtrl _ctrl;

        /// <summary>
        /// Constructor por defecto de la clase
        /// </summary>
        public ConfigurarBloquesActividadController()
        {
            _ctrl = new BloqueActividadCtrl(null);
        }

        /// <summary>
        /// Consulta la lista de bloques de acuerdo a los criterios proporcionados como filtro
        /// </summary>
        /// <param name="filtro">Bloque que se usara como filtro</param>
        /// <param name="tracking">booleano que determina si los resultados permaneceran en el cache</param>
        /// <returns>Lista de bloques</returns>
        public List<BloqueActividad> RetrieveListBloques(BloqueActividad filtro, bool tracking = false)
        {
            List<BloqueActividad> list = _ctrl.Retrieve(filtro, tracking);

            return list.OrderBy(item => item.Nombre).ToList();
        }

        /// <summary>
        /// Consulta la lista de bloques con sus relaciones de acuerdo a los criterios proporcionados como filtro
        /// </summary>
        /// <param name="filtro">Bloque que se usara como filtro</param>
        /// <param name="tracking">booleano que determina si los resultados permaneceran en el cache</param>
        /// <returns>Lista de bloques</returns>
        public List<BloqueActividad> RetrieveListBloquesWithRelationship(BloqueActividad filtro, bool tracking = false)
        {
            List<BloqueActividad> list = _ctrl.RetrieveWithRelationship(filtro, tracking);

            return list.OrderBy(item => new { item.Nombre, item.FechaInicio }).ToList();
        }

        /// <summary>
        /// Inserta un bloque
        /// </summary>
        /// <param name="bloque">bloque que se desea insertar</param>
        /// <returns>Cadena que indica si ocurre un error, vacia si esta todo correcto</returns>
        public string InsertBloque(BloqueActividad bloque)
        {
            string sError = string.Empty;

            //validacion de traslape

            sError = ValidateData(bloque);

            if (string.IsNullOrEmpty(sError))
                _ctrl.Insert(bloque);

            return sError;
        }

        /// <summary>
        /// Actualiza un bloque
        /// </summary>
        /// <param name="bloque">bloque que se desea actualizar</param>
        /// <returns>Cadena que indica si ocurre un error, vacia si esta todo correcto</returns>
        public string UpdateBloque(BloqueActividad bloque)
        {
            string sError = string.Empty;

            //validacion de traslape

            sError = ValidateData(bloque);

            if (string.IsNullOrEmpty(sError))
                _ctrl.Update(bloque);

            return sError;
        }

        private string ValidateData(BloqueActividad bloque)
        {
            string sError = string.Empty;

            List<BloqueActividad> bloques = RetrieveListBloques(new BloqueActividad { CicloEscolarId = bloque.CicloEscolarId, EscuelaId = bloque.EscuelaId, Activo = true });

            foreach (BloqueActividad b in bloques)
            {
                if (bloque.BloqueActividadId != null && bloque.BloqueActividadId == b.BloqueActividadId)
                    continue;

                if (b.Nombre.ToUpper().CompareTo(bloque.Nombre.ToUpper()) == 0)
                    sError += ", El nombre del bloque ya existe ";
                if (b.FechaInicio.Value.CompareTo(bloque.FechaInicio) <= 0
                    && b.FechaFin.Value.CompareTo(bloque.FechaInicio) >= 0)
                    sError += ", La fecha de inicio se traslapa con el bloque: " + b.Nombre;

                if (b.FechaInicio.Value.CompareTo(bloque.FechaFin) <= 0 
                    && b.FechaFin.Value.CompareTo(bloque.FechaFin) >= 0)
                    sError += ", La fecha de fin se traslapa con el bloque: " + b.Nombre;

                if (!string.IsNullOrEmpty(sError))
                {
                    sError = sError.Substring(2);
                    break;
                }

                if (bloque.FechaInicio.Value.CompareTo(b.FechaInicio) <= 0
                    && bloque.FechaFin.Value.CompareTo(b.FechaInicio) >= 0)
                    sError += ", El bloque " + bloque.Nombre + " tiene rangos que incluyen a la fecha de inicio del bloque: " + b.Nombre;

                if (bloque.FechaInicio.Value.CompareTo(b.FechaFin) <= 0
                    && bloque.FechaFin.Value.CompareTo(b.FechaFin) >= 0)
                    sError += ", El bloque " + bloque.Nombre + " tiene rangos que incluyen a la fecha de fin del bloque: " + b.Nombre;

                if (!string.IsNullOrEmpty(sError))
                {
                    sError = sError.Substring(2);
                    break;
                }
            }

            return sError;
        }

        /// <summary>
        /// Elimina un bloque de manera logica de la base de datos del sistema
        /// </summary>
        /// <param name="bloque"></param>
        /// <returns></returns>
        public string DeleteBloque(BloqueActividad bloque)
        {
            string sError = string.Empty;
            ActividadCtrl actividadCtrl = new ActividadCtrl(null);

            var actividadesBloque = actividadCtrl.Retrieve(new Actividad { BloqueActividadId = bloque.BloqueActividadId}, false);

            if (actividadesBloque.Where(a => a.Activo.Value).Count() > 0)
                return "No se puede eliminar el bloque, está en uso por una o varias actividades activas";

            if (actividadesBloque.Where(a => !a.Activo.Value).Count() > 0)
            {
                bloque.Activo = false;
                _ctrl.Update(bloque);
            }
            else
            {
                _ctrl.Delete(bloque);
            }
            return sError;

        }
        
        public void Dispose()
        {
            _ctrl.Dispose();
        }
    }
}
