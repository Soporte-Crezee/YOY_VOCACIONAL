using Framework.Base.DataAccess;
using POV.Reactivos.BO;
using POV.Reactivos.Service;
using System.Collections.Generic;
using System.Data;


namespace POV.ServiciosActividades.Controllers
{

    public class ConsultarReactivosController
    {

        /// <summary>
        /// Consulta los reactivos que cumplen con el Reactivo Filtro
        /// </summary>
        /// <param name="filtro">Reactivo Buscado</param>
        /// <returns></returns>
        public List<Reactivo> ConsultarReactivo(Reactivo filtro)
        {

            IDataContext dctx = new DataContext((new DataProviderFactory()).GetDataProvider("POV"));

            ReactivoCtrl reactivoCtrl = new ReactivoCtrl();

            DataSet dsReactivos = reactivoCtrl.Retrieve(dctx, filtro);
            List<Reactivo> listReactivos = new List<Reactivo>();
            if (dsReactivos.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drReactivo in dsReactivos.Tables[0].Rows)
                {
                    Reactivo boReactivo = reactivoCtrl.DataRowToReactivo(drReactivo, ETipoReactivo.Estandarizado);
                    boReactivo = reactivoCtrl.RetrieveComplete(dctx, boReactivo);
                    listReactivos.Add(boReactivo);
                }
            }
            return listReactivos;
        }
    }
}
