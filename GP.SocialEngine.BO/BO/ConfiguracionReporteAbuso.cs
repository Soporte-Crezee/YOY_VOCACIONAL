using System;

namespace GP.SocialEngine.BO { 
   /// <summary>
   /// Configuracion reporte abuso
   /// </summary>
   public class ConfiguracionReporteAbuso { 
      private int? maximoReportes;
      /// <summary>
      /// Número maximo de reportes de abuso permitidos
      /// </summary>
      public int? MaximoReportes{
          get
          {
              if (maximoReportes == null)
                  return 10;
              return this.maximoReportes;
          }
          set{ this.maximoReportes = value; }
      }
      private DateTime? fechaConsulta;
      /// <summary>
      /// Fecha a consultar los reportes de abuso
      /// </summary>
      public DateTime? FechaConsulta{
         get
         {
             if (TiempoCancelacionMinutos != null || TiempoCancelacionMinutos > 0)
                 this.fechaConsulta = DateTime.Now.AddMinutes((-(int)TiempoCancelacionMinutos));
             else
                 //Fecha Default 1 día
                 this.fechaConsulta = DateTime.Now.AddDays(-1);

             return this.fechaConsulta;
         }
         set
         {
             fechaConsulta = value;
             
         }
      }
     
      private int? tiempoCancelacionMinutos;
      /// <summary>
      /// Tiempo de cancelacion en minutos
      /// </summary>
      public int? TiempoCancelacionMinutos{
         get{ return this.tiempoCancelacionMinutos; }
         set{ this.tiempoCancelacionMinutos = value; }
      }
   } 
}
