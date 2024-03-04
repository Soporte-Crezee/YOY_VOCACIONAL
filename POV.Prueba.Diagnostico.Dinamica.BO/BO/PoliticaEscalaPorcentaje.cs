using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Prueba.BO;

namespace POV.Prueba.Diagnostico.Dinamica.BO
{
    public class PoliticaEscalaPorcentaje : APoliticaEscalaDinamica
    {
		public override bool Validar(PruebaDinamica pruebaDinamica, AEscalaDinamica nuevo)
		{
			bool lFlag = false;
			if (pruebaDinamica == null || pruebaDinamica.ListaPuntajes == null)
				throw new Exception("PruebaDinamica:No puede ser nulo");
			if (nuevo == null)
				throw new Exception("AEscalaDinamica:No puede ser nulo");

			if (nuevo.PuntajeMinimo >= nuevo.PuntajeMaximo)
				throw new Exception(String.Format(
					"Porcentaje inicial no puede ser mayor o igual a Porcentaje final:\n\n Rango: {0} - {1}",
					nuevo.PuntajeMinimo.ToString(), nuevo.PuntajeMaximo.ToString()));

			if (nuevo.PuntajeMinimo > 100 || nuevo.PuntajeMaximo > 100)
				throw new Exception(String.Format(
					"Los porcentajes del rango no pueden ser mayor a 100, por favor verifique:\n\n Rango: {0} - {1}",
					nuevo.PuntajeMinimo.ToString(), nuevo.PuntajeMaximo.ToString()));

			if (!pruebaDinamica.ListaPuntajes.Any())
			{
				//Si no hay ninguna escala aún en la lista, la primera a insertar debe tener como puntaje mínimo 0.
				if (nuevo.PuntajeMinimo == 0)
					return true;
				else
					throw new Exception("El primer rango de la prueba, debe tener como porcentaje inicial cero");
			}

			if (nuevo.EsPredominante.Value)
			{
				if (pruebaDinamica.ListaPuntajes.Where(x => x.EsPredominante == true).Count() > 0)
					throw new Exception("Sólo puede existir un rango predominante, por favor verifique");
			}

			foreach (APuntaje ap in pruebaDinamica.ListaPuntajes.OrderBy(x => (x as AEscalaDinamica).PuntajeMinimo))
			{
				AEscalaDinamica escala = ap as EscalaPorcentajeDinamica;

				if (nuevo.Clasificador.ClasificadorID == escala.Clasificador.ClasificadorID)
				{
					throw new Exception(String.Format(
					"No puede haber dos rangos con el mismo Clasificador:\n\n Rango: {0} - {1}",
					nuevo.PuntajeMinimo.ToString(), nuevo.PuntajeMaximo.ToString()));
				}
				else if (nuevo.Nombre.ToUpper() == escala.Nombre.ToUpper())
				{
					throw new Exception(String.Format(
					"No puede haber dos rangos con el mismo Nombre:\n\n Rango: {0} - {1}",
					nuevo.PuntajeMinimo.ToString(), nuevo.PuntajeMaximo.ToString()));
				}
				else if (nuevo.PuntajeMinimo == escala.PuntajeMinimo)
				{
					throw new Exception(String.Format(
					"No puede haber dos rangos con el mismo Puntaje inicial:\n\n Rango: {0} - {1}",
					nuevo.PuntajeMinimo.ToString(), nuevo.PuntajeMaximo.ToString()));
				}
				else if (nuevo.PuntajeMaximo == escala.PuntajeMaximo)
				{
					throw new Exception(String.Format(
					"No puede haber dos rangos con el mismo Puntaje final:\n\n Rango: {0} - {1}",
					nuevo.PuntajeMinimo.ToString(), nuevo.PuntajeMaximo.ToString()));
				}
				else
				{
					if (nuevo.PuntajeMinimo == escala.PuntajeMaximo)
						lFlag = true;
				}
			}

			// Si en este punto NO es un rango válido pero no ocurrió excepción, hacer una segunda vuelta comparando ahora
			// nuevo.puntajeMaximo con escala.puntajeMinimo.
			if (!lFlag)
			{
				foreach (APuntaje ap in pruebaDinamica.ListaPuntajes.OrderByDescending(x => (x as AEscalaDinamica).PuntajeMinimo))
				{
					AEscalaDinamica escala = ap as EscalaPorcentajeDinamica;
					if (nuevo.PuntajeMaximo == escala.PuntajeMinimo)
						lFlag = true;
				}
			}

			return lFlag;
		}
    }
}
