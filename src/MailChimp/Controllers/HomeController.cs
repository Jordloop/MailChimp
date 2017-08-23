using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MailChimp.Models;

namespace MailChimp.Controllers
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetMessages()
        {
            var allMessages = CampaignDefaults.GetCampaigns();
            return View(allMessages);
        }
        public IActionResult ShowSignupForm()
        {
            var form = Signup.ShowSignup();
            return View(form);
        }
    }
}
