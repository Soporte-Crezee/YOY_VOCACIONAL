
var favoritoApi = new FavoritosApi();

function insertarFavoritos(data) {
    var dataString = $.toJSON(data);

    favoritoApi.GuardarFavorito({
        success: function (result) { 
        },
        error: function (result) { 
            
        }
    }, dataString);
}

function eliminarFavoritos(data) {
    var dataString = $.toJSON(data);

    favoritoApi.EliminarFavorito({
        success: function (result) {
        },
        error: function (result) {
            
        }
    }, dataString);
}