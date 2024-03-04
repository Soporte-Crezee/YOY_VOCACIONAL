using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Base.DataAccess;
using GP.SocialEngine.BO;
using GP.SocialEngine.DA;
using POV.Seguridad.BO;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.CentroEducativo.DA;
using GP.SocialEngine.Service;
using POV.Core.RedSocial.Implement;
using POV.Core.RedSocial.Interfaces;

namespace POV.Web.DTO.Services
{
    public class RankingDTOCtrl
    {
        private IDataContext dctx;

        private IUserSession userSession;

        public RankingDTOCtrl()
        {
            dctx = new DataContext((new DataProviderFactory()).GetDataProvider("POV"));
            userSession = new UserSession();
        }

        public rankingdto GetPeople(rankingdto dto)
        {
            try
            {
                List<contactodto> peoples = new List<contactodto>();
                dto.peoples = peoples;
                if (!string.IsNullOrEmpty(dto.rankingid))
                {
                    RankingCtrl rankingCtrl = new RankingCtrl();

                    Guid rankingID = Guid.Parse(dto.rankingid);

                    Ranking ranking = new Ranking { RankingID = rankingID };
                    int vote = dto.vote.Value;

                    ranking.ListaPuntuaciones = rankingCtrl.RetrieveUsuariosSocialRanking(dctx, ranking, (EPuntuacionRanking)dto.vote);
                    dto = RankingToDTO(ranking);
                    dto.vote = vote;
                }

                return dto;
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
        }


        public rankingdto RankingToDTO(Ranking ranking)
        {
            rankingdto dto = new rankingdto();
            dto.rankingid = ranking.RankingID.ToString();

            List<UsuarioSocial> usuarios = new List<UsuarioSocial>();

            foreach (UsuarioSocialRanking usuarioSocialRanking in ranking.ListaPuntuaciones)
            {
                usuarios.Add(usuarioSocialRanking.UsuarioSocial);
            }

            ContactoDTOCtrl contactoDTOCtrl = new ContactoDTOCtrl();
            dto.peoples = contactoDTOCtrl.ListUsuarioSocialToListContactoDTO(usuarios);

            return dto;
        }
    }
}
