﻿var ReporteAbusoApi=function(){};ReporteAbusoApi.prototype.GetReportesAbuso=function(a,b){var c=$.extend({success:function(){},error:function(){}},a);$.apiCall({url:encodeURI("../wcf/ReporteAbusoService.svc/GetReportesAbuso"),type:"POST",data:b,contentType:"application/json; charset=utf-8",dataType:"json",processData:false,success:function(a){c.success(a)},error:function(a){c.error(a)}})};ReporteAbusoApi.prototype.ConfirmReporteAbuso=function(a,b){var c=$.extend({success:function(){},error:function(){}},a);$.apiCall({url:encodeURI("../wcf/ReporteAbusoService.svc/ConfirmReporteAbuso"),type:"POST",data:b,contentType:"application/json; charset=utf-8",dataType:"json",processData:false,success:function(a){c.success(a)},error:function(a){c.error(a)}})};ReporteAbusoApi.prototype.GetPublicacionReporteAbuso=function(a,b){var c=$.extend({success:function(){},error:function(){}},a);$.apiCall({url:encodeURI("../wcf/ReporteAbusoService.svc/GetCompleteReporteAbuso"),type:"POST",data:b,contentType:"application/json; charset=utf-8",dataType:"json",processData:false,success:function(a){c.success(a)},error:function(a){c.error(a)}})};ReporteAbusoApi.prototype.DeleteReporteAbuso=function(a,b){var c=$.extend({success:function(){},error:function(){}},a);$.apiCall({url:encodeURI("../wcf/ReporteAbusoService.svc/DeleteReporteAbuso"),type:"POST",data:b,contentType:"application/json; charset=utf-8",dataType:"json",processData:false,success:function(a){c.success(a)},error:function(a){c.error(a)}})};ReporteAbusoApi.prototype.AddReporteAbuso=function(a,b){var c=$.extend({success:function(){},error:function(){}},a);$.apiCall({url:encodeURI("../wcf/ReporteAbusoService.svc/InsertReporteAbuso"),type:"POST",data:b,contentType:"application/json; charset=utf-8",dataType:"json",processData:false,success:function(a){c.success(a)},error:function(a){c.error(a)}})};ReporteAbusoApi.prototype.validateReportarAbuso=function(a){var b=$.extend({success:function(){},error:function(){}},a);$.apiCall({url:encodeURI("../wcf/ReporteAbusoService.svc/ValidateInsertReporteAbuso"),type:"POST",data:null,contentType:"application/json; charset=utf-8",dataType:"json",processData:false,success:function(a){b.success(a)},error:function(a){b.error(a)}})}