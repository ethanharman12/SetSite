using SetSite.Data;
using SetSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SetSite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Rules()
        {
            return View();
        }

        public ActionResult Solitaire()
        {
            return View();
        }

        public ActionResult Bots()
        {
            return View();
        }

        [Authorize]
        public ActionResult Multiplayer(int id)
        {
            ViewBag.gameId = id;
            List<PlayerViewModel> friends = new List<PlayerViewModel>();

            //GameContext context = new GameContext();

            //foreach (var player in context.Users)
            //{
            //    friends.Add(new PlayerViewModel() { Id = player.Id, Name = player.DisplayName });
            //}

            ViewBag.friends = friends;
            return View();
        }

        [Authorize]
        public ActionResult MultiplayerLobby()
        {
            return View();
        }
    }
}