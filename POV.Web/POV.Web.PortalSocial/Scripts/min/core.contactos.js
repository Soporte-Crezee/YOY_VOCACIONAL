﻿function initPage(){$("#postit").css("display","none");$.get("../Scripts/tmpl/contacto.tmpl.htm",function(a){$("body").append(a);LoadContactos()})}function LoadContactos(){var a={dto:{pagesize:10}};var b=$.toJSON(a);contactosApi.GetContactos({success:function(a){printContactos(a)}},b)}function printContactos(a){var b=$.toJSON(a);if(a.d.length<=0){var c="<span>No se encontraron resultados.</span>"}else{$("#contactosTemp").tmpl(a.d).appendTo("#ContactosStream")}}$(document).ready(initPage);var contactosApi=new ContactosApi;var group