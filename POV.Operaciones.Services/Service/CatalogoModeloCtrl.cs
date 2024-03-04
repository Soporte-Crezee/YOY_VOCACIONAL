using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Framework.Base.DataAccess;
using POV.Modelo.BO;
using POV.Modelo.Service;
using POV.Reactivos.BO;
using POV.Reactivos.Service;

namespace POV.Operaciones.Service
{
    /// <summary>
    /// Controlador del catalogo de modelos
    /// </summary>
    public class CatalogoModeloCtrl
    {
        /// <summary>
        /// Elimina de manera logica un registro de clasificador en la base de datos
        /// </summary>
        /// <param name="dctx">El dataContext que provee la conexion a la base de datos</param>
        /// <param name="clasificador">Clasificador que se desea eliminar</param>
        public void DeleteClasificador(IDataContext dctx, Clasificador clasificador)
        {
            if (clasificador == null) throw new ArgumentNullException("CatalogoModeloCtrl: el clasificador no puede ser nulo");
            if (clasificador.ClasificadorID == null) throw new ArgumentNullException("CatalogoModeloCtrl: el ClasificadorID no puede ser nulo");

            string sError = string.Empty;

            //validamos que no este en uso por un reactivo
            ReactivoCtrl reactivoCtrl = new ReactivoCtrl();

            Reactivo reactivoFiltro = new Reactivo();
            reactivoFiltro.Caracteristicas = new CaracteristicasModeloGenerico { Clasificador = clasificador };
            reactivoFiltro.TipoReactivo = ETipoReactivo.ModeloGenerico;
            reactivoFiltro.Activo = true;


            DataSet dsReactivosEnUso = reactivoCtrl.Retrieve(dctx, reactivoFiltro);

            if (dsReactivosEnUso.Tables[0].Rows.Count > 0)
                sError += ", Reactivo";

            //validamos que no este en uso por una opcion dinamica
            OpcionRespuestaPlantillaCtrl opcionCtrl = new OpcionRespuestaPlantillaCtrl();
            
            OpcionRespuestaModeloGenerico opcionDinamicaFiltro = new OpcionRespuestaModeloGenerico();
            opcionDinamicaFiltro.Activo = true;
            opcionDinamicaFiltro.Clasificador = clasificador;

            DataSet dsOpcionesEnUso = opcionCtrl.Retrieve(dctx, opcionDinamicaFiltro, new RespuestaPlantillaOpcionMultiple(), reactivoFiltro.TipoReactivo.Value);

            if (dsOpcionesEnUso.Tables[0].Rows.Count > 0)
                sError += ", Opción múltiple";

            //validamos que no este en uso por un paquete de juegos
            

            if (sError.Length > 0)
                throw new Exception("No se puede eliminar el clasificador está en uso por: " + sError.Substring(2));

            ModeloCtrl modeloPruebaCtrl = new ModeloCtrl();
            modeloPruebaCtrl.DeleteClasificador(dctx, clasificador);
        }
    }
}
