using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Base.DataAccess;
using POV.Web.DTO;
using GP.SocialEngine.BO;
using GP.SocialEngine.Service;
using GP.SocialEngine.DA;
using GP.SocialEngine.Utils;
using POV.Core.RedSocial.Implement;
using POV.Core.RedSocial.Interfaces;
using POV.Blog.BO;
using POV.Blog.Services;


namespace POV.Web.DTO.Services
{
    public class PostFavoritoDTOCtrl
    {
        private IUserSession userSession;
        private PostFavoritoCtrl postFavoritoCtrl;
        private PostFavorito postFavorito;
        private IDataContext dctx;

        public PostFavoritoDTOCtrl()
        {
            dctx = new DataContext(new DataProviderFactory().GetDataProvider("POV"));
            userSession = new UserSession();
            postFavoritoCtrl = new PostFavoritoCtrl(null);
            postFavorito = new PostFavorito();
        }

        //Insert
        public postfavoritodto GuardarFavorito(postfavoritodto dto)        
        {
            try
            {
                List<string> categoryExist = new List<string>();
                postFavorito.AlumnoId = userSession.CurrentAlumno.AlumnoID;                
                postFavorito.BlogId = dto.BlogId;
                postFavorito.PostId = dto.PostId;              

                var result = postFavoritoCtrl.Retrieve(postFavorito, true).ToList();

                if (result.Count > 0)
	            {
                    categoryExist = result[0].Categorias.ToString().Split(',').ToList();

                    foreach (var item in categoryExist)
                    {
                        if (item == dto.Categorias)
                        {
                            dto.Error = "El post se ha guardado con anterioridad como favorito";
                        }
                    }

                    if (string.IsNullOrEmpty(dto.Error))
                    {
                        postFavorito = result[0];
                        postFavorito.Categorias = result[0].Categorias.ToString() + ", " + dto.Categorias;
                        postFavoritoCtrl.Update(postFavorito);
                        dto.Success = "Post agregado "+(categoryExist.Count+1)+" veces como favorito";
                    }
	            }
                else{
                    postFavorito.Categorias = dto.Categorias;
                    postFavoritoCtrl.Insert(postFavorito);
                    var resultInsert = postFavoritoCtrl.Retrieve(postFavorito, false).ToList();

                    if (resultInsert.Count <= 0)
                    {
                        dto.Error = "Error al obtener el post como favorito";
                    }
                    else {
                        dto.PostFavoritoAspiranteId = resultInsert[0].PostFavoritoAspiranteId;                        
                        dto.BlogId = dto.BlogId;
                        dto.PostId = dto.PostId;
                        dto.Error = "";
                        dto.Success = "Post guardado correctamente como favorito";
                    }                   
                }
               
                return dto;
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                dto.Error = "No se pudo insertar el post como favorito";
                return dto;
            }
        }

        //Delete
        public postfavoritodto EliminarFavorito(postfavoritodto dto)
        {
            try
            {
                postFavorito.AlumnoId = userSession.CurrentAlumno.AlumnoID;
                postFavorito.BlogId = dto.BlogId;
                postFavorito.PostId = dto.PostId;

                var result = postFavoritoCtrl.Retrieve(postFavorito, true).ToList();

                if (result.Count <= 0)
                {
                    dto.Error = "El post no se encuentra como favorito";
                }
                else
                {
                    postFavoritoCtrl.Delete(result[0]);
                    
                    dto.PostId = dto.PostId;
                    dto.Error = "";
                    dto.Success = "Post eliminado correctamente como favorito";                    
                }

                return dto;
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                dto.Error = "No se pudo eliminar el post como favorito";
                return dto;
            }
        }
    }
}
