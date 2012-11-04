using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_UI.Models;
using AchievementSherpa.Business;
using AchievementSherpa.Business.Services;

namespace Web_UI.Controllers
{
    public class PlayerController : Controller
    {
        int featsOfStrength = 81;
        private IAchievementService _achievementService;
        private ICharacterService _characterService;
        //
        // GET: /PLayer/

        public PlayerController(IAchievementService achievementService, ICharacterService characterService)
        {
            _achievementService = achievementService;
            _characterService = characterService;

        }
        public ActionResult Index(string server, string player, string region)
        {


            Character character = _characterService.FindCharacter(region, server, player);

            if (character == null)
            {
                return Redirect("~/");
            }

            IList<Achievement> recommended = _achievementService.GetRecommendedAchievements(character);

            Ranking rank = _characterService.RankCharacter(character);

            int position = rank.GuildRank;
            int serverPostion = rank.ServerRank;
            int worldWidePostion = rank.WorldRank;


            IList<PlayerAchievement> recentAchievements = new List<PlayerAchievement>();
            var recentlyEarnedAchievements = character.Achievements.OrderByDescending(a => a.WhenAchieved).Take(5);

            foreach (AchievedAchievement earnedAchievement in recentlyEarnedAchievements)
            {
                /*Achievement achievement = _achievementService.FindAchivementByBlizzardId(earnedAchievement.AchievementId);

                if ( achievement != null )
                {

                recentAchievements.Add(new PlayerAchievement() { WhenAchieved = earnedAchievement.WhenAchieved,
                                                                 Achievement = achievement
                });
                }*/
            }

            return View(new Player() { WowPlayer = character, RecommendedAchievements = recommended, GuildPostion = position, ServerPosition = serverPostion, WorldWidePositon = worldWidePostion, RecentAchievements = recentAchievements });
        }

      


        public ActionResult Parse(string name, string server, string region)
        {
            _characterService.ForceCharacterParse(server, name, region);
            return RedirectToAction("Index", new { region = region, server = server, player = name });
        }

    }
}
