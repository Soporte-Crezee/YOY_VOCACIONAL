using System;
using System.Collections.Generic;
using System.Text;
using POV.Reactivos.BO;
using GP.SocialEngine.BO;

namespace POV.ReactivosUsuario.BO { 
   /// <summary>
   /// RespuestaPreguntaUsuario
   /// </summary>
   public class RespuestaReactivoUsuario { 
      private Guid? respuestaReactivoUsuarioID;
      /// <summary>
      /// identificador de la respuesta usuario
      /// </summary>
      public Guid? RespuestaReactivoUsuarioID{
         get{ return this.respuestaReactivoUsuarioID; }
         set{ this.respuestaReactivoUsuarioID = value; }
      }
      private UsuarioSocial usuarioSocial;
      /// <summary>
      /// UsuarioSocial de la respuesta
      /// </summary>
      public UsuarioSocial UsuarioSocial{
         get{ return this.usuarioSocial; }
         set{ this.usuarioSocial = value; }
      }
      private Reactivo reactivo;
      /// <summary>
      /// Reactivo
      /// </summary>
      public Reactivo Reactivo{
         get{ return this.reactivo; }
         set{ this.reactivo = value; }
      }
      private decimal? primeraCalificacion;
      /// <summary>
      /// Calificacion de la respuesta usuario
      /// </summary>
      public decimal? PrimeraCalificacion{
         get{ return this.primeraCalificacion; }
         set{ this.primeraCalificacion = value; }
      }
      private decimal? ultimaCalificacion;
      /// <summary>
      /// Calificacion de la respuesta usuario
      /// </summary>
      public decimal? UltimaCalificacion{
         get{ return this.ultimaCalificacion; }
         set{ this.ultimaCalificacion = value; }
      }
      private int? numeroIntentos;
      /// <summary>
      /// NumeroIntento de la respuesta usuario
      /// </summary>
      public int? NumeroIntentos{
         get{ return this.numeroIntentos; }
         set{ this.numeroIntentos = value; }
      }
      private DateTime? fechaRegistro;
      /// <summary>
      /// FechaRegistro
      /// </summary>
      public DateTime? FechaRegistro{
         get{ return this.fechaRegistro; }
         set{ this.fechaRegistro = value; }
      }
      private DateTime? ultimaActualizacion;
      /// <summary>
      /// FechaRegistro
      /// </summary>
      public DateTime? UltimaActualizacion{
         get{ return this.ultimaActualizacion; }
         set{ this.ultimaActualizacion = value; }
      }

      private EEstadoReactivoUsuario? estadoReactivoUsuario;

      public EEstadoReactivoUsuario? EstadoReactivoUsuario
      {
          get { return estadoReactivoUsuario; }
          set { estadoReactivoUsuario = value; }
      }
      private List<RespuestaPreguntaUsuario> listaRespuestaPreguntaUsuario;
      /// <summary>
      /// ListaRespuestaPreguntaUsuario
      /// </summary>
      public List<RespuestaPreguntaUsuario> ListaRespuestaPreguntaUsuario{
         get{ return this.listaRespuestaPreguntaUsuario; }
         set{ this.listaRespuestaPreguntaUsuario = value; }
      }
   } 
}
