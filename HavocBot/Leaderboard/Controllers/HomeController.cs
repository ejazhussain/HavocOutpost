using Leaderboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Leaderboard.Controllers
{
    public class HomeController : Controller
    {
        Service service = new Service();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Route("configure")]
        public ActionResult Configure()
        {
            return View();
        }

        [Route("team")]
        public async Task<ActionResult> Team()
        {
            List<LeaderboardItem> results = new List<LeaderboardItem>();
        
            //fet list of Team Leaderboard from API

            var teams = await service.PopulateAsync();

            if(teams?.Count > 0)
            {
                results = teams;
            }
            return View(results);
        }
        [Route("individual")]
        public ActionResult Individual()
        {
            return View();
        }


<<<<<<< HEAD
=======
        public ActionResult TeamLeaderboard(string teamId)
        {
            TeamLeaderboardViewModel teamLeaderboardViewModel = new TeamLeaderboardViewModel()
            {
                TeamId = teamId
            };

            return View(teamLeaderboardViewModel);
        }

        public ActionResult Authenticate()
        {
            return View();
        }
>>>>>>> 6daac4167632245ea4dfbdd4d3e52db37d7a9ded
    }
}