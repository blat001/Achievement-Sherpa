using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_UI.Models;
using AchievementSherpa.Business;
using AchievementSherpa.Business;

namespace Web_UI.Controllers
{
    public class RankingController : Controller
    {
        private IWorldRepository _worldRepository;
        public RankingController(IWorldRepository worldRepository)
        {
            _worldRepository = worldRepository;
        }
        //
        // GET: /Ranking/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Guild(string region, string server, string guildName, int? start, int? size)
        {
            int realStart = 0;
            int realSize = 100;

            if (start != null)
            {
                realStart = start.Value;
            }

            if (size != null)
            {
                realSize = size.Value;
            }

            IList<Character> roster = _worldRepository.GetCharactersInGuild(region, server, guildName);

            int count = roster.Count;
            var characters = roster.Skip(realStart).Take(realSize).ToList();
   

            CharacterList listOfCharacters = new CharacterList()
            {
                Characters = characters,
                TotalNumber = count,
                PageSize = realSize
            };
            return View(listOfCharacters);
        }


        public ActionResult ServerRanking(string region, string server, int? start, int? size)
        {
            int realStart = 0;
            int realSize = 100;

            if (start != null)
            {
                realStart = start.Value;
            }

            if (size != null)
            {
                realSize = size.Value;
            }

            int count = _worldRepository.NumberCharactersOnServer(region, server);
            var characters = _worldRepository.ListCharactersOnServerByPoints(region, server, realStart, realSize);


            CharacterList listOfCharacters = new CharacterList()
            {
                Characters = characters,
                TotalNumber = count,
                PageSize = realSize
            };

            return View(listOfCharacters);
        }
    }
}
