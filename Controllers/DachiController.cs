using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Dojodachi
{
    public class DachiController : Controller
    {
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            Initialize();
            return View();
        }
        private void Initialize(){
            HttpContext.Session.Clear();
            HttpContext.Session.SetInt32("happiness",20);
            HttpContext.Session.SetInt32("fullness",20);
            HttpContext.Session.SetInt32("meals",3);
            HttpContext.Session.SetInt32("energy",50);
        }
        private string CheckStatus(){
            string status;
            if((int)HttpContext.Session.GetInt32("fullness") > 100 && (int)HttpContext.Session.GetInt32("happiness") > 100 && (int)HttpContext.Session.GetInt32("energy") > 100)
            {
                status = "Win";
            }
            else if ((int)HttpContext.Session.GetInt32("fullness") <= 0 || (int)HttpContext.Session.GetInt32("happiness") <= 0)
            {
                status = "Dead";
            }
            else
            {
                status = "Doin' fine";
            }
            return status;
        }
        [HttpGet]
        [Route("start")]
        public JsonResult Start()
        {
            Dictionary<string, int> data = new Dictionary<string, int>();
            data["meals"] = (int)HttpContext.Session.GetInt32("meals");
            data["fullness"] = (int)HttpContext.Session.GetInt32("fullness");
            data["happiness"] = (int)HttpContext.Session.GetInt32("happiness");
            data["energy"] = (int)HttpContext.Session.GetInt32("energy");
            return Json(data);
        }
        [HttpGet]
        [Route("feed")]
        public JsonResult Feed()
        {
            int? meals = HttpContext.Session.GetInt32("meals");
            Random rand = new Random();
            Dictionary<string, object> data = new Dictionary<string, object>();
            if (meals>0)
            {
                meals --;
                HttpContext.Session.SetInt32("meals",(int)meals);
                int dislike = rand.Next(1,5);
                if (dislike != 1)
                {
                    int? fullness = HttpContext.Session.GetInt32("fullness");
                    int added = rand.Next(5,11);
                    fullness += added;
                    HttpContext.Session.SetInt32("fullness",(int)fullness);
                    data["response"] = "love";
                    data["message"] = $"You fed your DojoDachi! They liked it! Fullness +{added}, Meals -1";
                }
                else
                {
                    data["response"] = "hate";
                    data["message"] = $"You fed your DojoDachi! They hated it! Meals -1";
                }
            }
            else
            {
                data["message"] = "Your Dojodachi does not have enough food for this! You both need to work for it.";
            }
            data["meals"] = HttpContext.Session.GetInt32("meals");
            data["fullness"] = HttpContext.Session.GetInt32("fullness");
            data["status"] = CheckStatus();
            return Json(data);
        }
        [HttpGet]
        [Route("play")]
        public JsonResult Play()
        {
            Random rand = new Random();
            Dictionary<string, object> data = new Dictionary<string, object>();
            int? energy = HttpContext.Session.GetInt32("energy");
            if(energy >= 5)
            {
                energy -= 5;
                HttpContext.Session.SetInt32("energy",(int)energy);
                int dislike = rand.Next(1,5);
                if (dislike != 1)
                {
                    int? happiness = HttpContext.Session.GetInt32("happiness");
                    int added = rand.Next(5,11);
                    happiness += added;
                    HttpContext.Session.SetInt32("happiness",(int)happiness);
                    data["response"] = "love";
                    data["message"] = $"You played with your Dojodachi! They loved it! Happiness +{added}, Energy -5";
                }
                else
                {
                    data["response"] = "hate";
                    data["message"] = $"You played with your Dojodachi! They hated it! Energy -5";
                }
            }
            else
            {
                data["message"] = "Your Dojodachi is too tired for this! Get some rest first.";
            }
            data["happiness"] = HttpContext.Session.GetInt32("happiness");
            data["energy"] = HttpContext.Session.GetInt32("energy");
            data["status"] = CheckStatus();
            return Json(data);
        }
        [HttpGet]
        [Route("work")]
        public JsonResult Work()
        {
            Random rand = new Random();
            Dictionary<string, object> data = new Dictionary<string, object>();
            int? energy = HttpContext.Session.GetInt32("energy");
            if(energy >= 5)
            {
                energy -= 5;
                HttpContext.Session.SetInt32("energy",(int)energy);
                int? meals = HttpContext.Session.GetInt32("meals");
                int added = rand.Next(1,4);
                meals += added;
                HttpContext.Session.SetInt32("meals",(int)meals);
                data["message"] = $"You and your Dojodachi put in a hard day of work! Meals +{added}, Energy -5";
            }
            else
            {
                data["message"] = "Your Dojodachi is too tired for this! Get some rest first.";
            }
            data["meals"] = HttpContext.Session.GetInt32("meals");
            data["energy"] = HttpContext.Session.GetInt32("energy");
            return Json(data);
        }
        [HttpGet]
        [Route("sleep")]
        public JsonResult Sleep()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            int? energy = HttpContext.Session.GetInt32("energy");
            int? fullness = HttpContext.Session.GetInt32("fullness");
            int? happiness = HttpContext.Session.GetInt32("happiness");
            energy+= 15;
            fullness -= 5;
            happiness -= 5;
            HttpContext.Session.SetInt32("energy",(int)energy);
            HttpContext.Session.SetInt32("fullness",(int)fullness);
            HttpContext.Session.SetInt32("happiness",(int)happiness);
            data["fullness"] = HttpContext.Session.GetInt32("fullness");
            data["happiness"] = HttpContext.Session.GetInt32("happiness");
            data["energy"] = HttpContext.Session.GetInt32("energy");
            data["status"] = CheckStatus();
            data["message"] = "Your Dojodachi slept well! Energy +15, Happiness -5, Fullness -5";
            return Json(data);
        }
    }
}