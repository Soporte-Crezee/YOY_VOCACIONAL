﻿var NotificacionesApi=function(){};NotificacionesApi.prototype.GetTotalNotificaciones=function(a){var b=$.extend({success:function(){},error:function(){}},a);$.apiCall({url:encodeURI("../wcf/NotificacionService.svc/GetTotalNotificaciones"),type:"POST",data:null,contentType:"application/json; charset=utf-8",dataType:"json",processData:false,success:function(a){b.success(a)},error:function(a){b.error(a)}})};NotificacionesApi.prototype.GetNotificaciones=function(a,b){var c=$.extend({success:function(){},error:function(){}},a);GetNotificaciones=$.apiCall({url:encodeURI("../wcf/NotificacionService.svc/GetNotificaciones"),type:"POST",data:b,contentType:"application/json; charset=utf-8",dataType:"json",processData:false,success:function(a){c.success(a)},error:function(a){c.error(a)}})};NotificacionesApi.prototype.DeleteNotificacion=function(a,b){var c=$.extend({success:function(){},error:function(){}},a);$.apiCall({url:encodeURI("../wcf/NotificacionService.svc/DeleteNotificacion"),type:"POST",data:b,contentType:"application/json; charset=utf-8",dataType:"json",processData:false,success:function(a){c.success(a)},error:function(a){c.error(a)}})};NotificacionesApi.prototype.ConfirmNotificacion=function(a,b){var c=$.extend({success:function(){},error:function(){}},a);$.apiCall({url:encodeURI("../wcf/NotificacionService.svc/ConfirmNotificacion"),type:"POST",data:b,contentType:"application/json; charset=utf-8",dataType:"json",processData:false,success:function(a){c.success(a)},error:function(a){c.error(a)}})}