using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Web_UI.Models;
using AchievementSherpa.Business;

namespace Web_UI.Controllers
{
    public class HomeController : Controller
    {
        private ICharacterRepository _characterRepository;
        //
        // GET: /Home/

        public HomeController(ICharacterRepository characterRepository)
        {
            _characterRepository = characterRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult All(int? start, int? size)
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



            int count = _characterRepository.FindAll().Count;
            var characters = _characterRepository.FindAll().OrderByDescending(c => c.CurrentPoints).Skip(realStart).Take(realSize).ToList();
            CharacterList listOfCharacters = new CharacterList()
            {
                Characters = characters,
                TotalNumber = count,
                PageSize = realSize
            };

            return View(listOfCharacters);
        }


        public ActionResult Search(string query)
        {

            var characterList = _characterRepository.SearchByName(string.Format(".*{0}.*", query));
            CharacterList listOfCharacters = new CharacterList()
            {
                Characters = characterList,
                TotalNumber = characterList.Count,
                PageSize = 100
            };

            return View(listOfCharacters);
           
        }
    }
}
