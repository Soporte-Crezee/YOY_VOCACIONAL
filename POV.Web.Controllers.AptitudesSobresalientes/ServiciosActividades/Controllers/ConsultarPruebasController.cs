using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Base.DataAccess;
using POV.ConfiguracionActividades.Services;
using POV.Licencias.BO;
using POV.Licencias.Service;
using POV.Modelo.Context;
using POV.Prueba.BO;
using POV.ConfiguracionActividades.BO;

namespace POV.ServiciosActividades.Controllers
{

	public class ConsultarPruebasController : IDisposable
	{
		#region atributos

		private readonly IDataContext dctx;
		private readonly Contexto _context;
		private readonly object _firma;
		#endregion

		#region Constructores
		/// <summary>
		/// Inicializa la conexión a la base de datos
		/// </summary>
		/// <param name="firma">firma que identifica a quien inicia y puede terminar la conexión</param>
		public ConsultarPruebasController(object firma)
		{
			_firma = firma ?? new object();
			dctx = new DataContext((new DataProviderFactory()).GetDataProvider("POV"));
			_context = new Contexto(_firma);
			dctx.OpenConnection(_firma);

		}
		#endregion

		#region Propiedades

		#endregion

		#region Métodos
		public void Dispose()
		{

			dctx.CloseConnection(_firma);
			_context.Dispose();
		}

		/// <summary>
		/// Consulta Pruebas asignadas al contrato
		/// </summary>
		/// <param name="contrato">Contrato que se usara como filtro para consultar pruebas</param>
		/// <param name="cicloContrato">Ciclo del cual se quieren consultar los contratos</param>
		/// <returns> Una lista de pruebas que coinciden con los filtros proporcionados</returns>
		public List<APrueba> ConsultarPruebas(Contrato contrato, CicloContrato cicloContrato, APrueba prueba)
		{
			PruebaContratoCtrl PruebaContratoCtrl = new PruebaContratoCtrl();

			List<PruebaContrato> pruebasContrato = PruebaContratoCtrl.RetrievePruebasAsignadoContrato(dctx, contrato, cicloContrato);
			List<APrueba> pruebas = new List<APrueba>();

			foreach (PruebaContrato pruebaContrato in pruebasContrato)
			{
				APrueba pruebaTMp = prueba;
				prueba.PruebaID = pruebaContrato.Prueba.PruebaID;
				using (PruebaCtrl ctrl = new PruebaCtrl(_context))
				{
					pruebaTMp = ctrl.Retrieve(pruebaTMp, false).FirstOrDefault();
					if(pruebaTMp!=null) pruebas.Add(pruebaTMp);
				}

			}

			return pruebas.Where(p=>p.TipoPrueba==prueba.TipoPrueba).ToList();


		}

        /// <summary>
        /// Consulta Pruebas asignadas al contrato
        /// </summary>
        /// <param name="contrato">Contrato que se usara como filtro para consultar pruebas</param>
        /// <param name="cicloContrato">Ciclo del cual se quieren consultar los contratos</param>
        /// <returns> Una lista de pruebas que coinciden con los filtros proporcionados</returns>
        public List<APrueba> ConsultarPruebas(Contrato contrato, CicloContrato cicloContrato)
        {
            PruebaContratoCtrl PruebaContratoCtrl = new PruebaContratoCtrl();

            List<PruebaContrato> pruebasContrato = PruebaContratoCtrl.RetrievePruebasAsignadoContrato(dctx, contrato, cicloContrato);
            List<APrueba> pruebas = new List<APrueba>();
            PruebaProxy prueba = new PruebaProxy();
            foreach (PruebaContrato pruebaContrato in pruebasContrato)
            {
                APrueba pruebaTMp = prueba;
                prueba.SetTipoPrueba(pruebaContrato.Prueba.TipoPrueba);
                prueba.PruebaID = pruebaContrato.Prueba.PruebaID;
                using (PruebaCtrl ctrl = new PruebaCtrl(_context))
                {
                    pruebaTMp = ctrl.Retrieve(pruebaTMp, false).FirstOrDefault();
                    if (pruebaTMp != null) pruebas.Add(pruebaTMp);
                }

            }

            return pruebas;


        }
		#endregion


	}
}
