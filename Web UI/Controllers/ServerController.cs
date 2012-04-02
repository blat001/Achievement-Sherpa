using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AchievementSherpa.Business;

namespace Web_UI.Controllers
{
    public class ServerController : Controller
    {
        private IWorldRepository _worldRepository;

        public ServerController(IWorldRepository worldRepository)
        {
            _worldRepository = worldRepository;
        }
        //
        // GET: /Server/

        public ActionResult Index()
        {
            IList<string> servers = _worldRepository.GetServers();

            return View(servers);
        }

    }
}
