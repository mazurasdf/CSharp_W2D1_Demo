using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HangMan.Models;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace HangMan.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("game")]
        public IActionResult Game()
        {
            if(HttpContext.Session.GetString("Answer") == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Answer = HttpContext.Session.GetString("Answer");
            ViewBag.Guesses = HttpContext.Session.GetString("Guesses");
            ViewBag.Wrong = HttpContext.Session.GetString("Wrong");

            if(ViewBag.Guesses == ViewBag.Answer)
            {
                ViewBag.ImgSource = Url.Content("~/img/win.png");
            }
            else if(ViewBag.Wrong.Length == 0)
            {
                ViewBag.ImgSource = Url.Content("~/img/0.png");
            }
            else if(ViewBag.Wrong.Length == 1)
            {
                ViewBag.ImgSource = Url.Content("~/img/1.png");
            }
            else if(ViewBag.Wrong.Length == 2)
            {
                ViewBag.ImgSource = Url.Content("~/img/2.png");
            }
            else if(ViewBag.Wrong.Length == 3)
            {
                ViewBag.ImgSource = Url.Content("~/img/3.png");
            }
            else if(ViewBag.Wrong.Length == 4)
            {
                ViewBag.ImgSource = Url.Content("~/img/4.png");
            }
            else if(ViewBag.Wrong.Length == 5)
            {
                ViewBag.ImgSource = Url.Content("~/img/5.png");
            }
            else
            {
                ViewBag.ImgSource = Url.Content("~/img/6.png");
            }
            return View();
        }

        [HttpPost("submit")]
        public IActionResult Submit(string word)
        {
            // Console.WriteLine(word);
            // Console.WriteLine("****************************************************************");
            string blanks = "";
            for(int i = 0; i < word.Length; ++i)
            {
                blanks += "_";
            }
            HttpContext.Session.SetString("Answer", word.ToUpper());
            HttpContext.Session.SetString("Wrong", "");
            HttpContext.Session.SetString("Guesses", blanks);
            return RedirectToAction("Game");
        }

        [HttpPost("guess")]
        public IActionResult Guess(string letter)
        {
            char letterGuess = letter.ToUpper()[0];

            string Answer = HttpContext.Session.GetString("Answer");
            StringBuilder sbGuesses = new StringBuilder(HttpContext.Session.GetString("Guesses"));
            string Wrong = HttpContext.Session.GetString("Wrong");

            bool found = false;
            for(int i = 0; i < Answer.Length; ++i)
            {
                if(Answer[i] == letterGuess)
                {
                    found = true;
                    sbGuesses[i] = letterGuess;
                }
            }

            if(!found)
            {
                Wrong = Wrong + letterGuess;
            }
            string Guesses = sbGuesses.ToString();

            HttpContext.Session.SetString("Wrong", Wrong);
            HttpContext.Session.SetString("Guesses", Guesses);
            // Console.WriteLine(letterGuess);
            // Console.WriteLine("****************************************************************");
            return RedirectToAction("Game");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
