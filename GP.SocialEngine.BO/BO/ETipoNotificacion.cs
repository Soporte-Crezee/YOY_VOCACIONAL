using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GP.SocialEngine.BO
{
    public enum ETipoNotificacion : short
    {
        MENSAJE = 1, 
        INVITACION_RECIBIDA = 2, 
        INVITACION_ACEPTADA = 3,
        PUBLICACION = 4, 
        COMENTARIO = 5, 
        RANKING_PUBLICACION = 6, 
        RANKING_COMENTARIO = 7,
        RESPUESTA_MENSAJE=8,
        PUBLICACION_DOCENTE=9,
        PUBLICACION_ELIMINADA=10,
        COMENTARIO_ELIMINADO=11,
        REPORTE_ABUSO=12
    }
}
