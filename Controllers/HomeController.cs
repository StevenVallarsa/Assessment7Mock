using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Assessment7Mock.Models;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Assessment7Mock.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Search(int Id)
        {
            // GET DONUT COUNT
            string donutCount = "https://grandcircusco.github.io/demo-apis/donuts.json";

            HttpWebRequest request = WebRequest.CreateHttp(donutCount);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader rd = new StreamReader(response.GetResponseStream());
            string APIText = rd.ReadToEnd();

            JToken jsonDonutCount = JToken.Parse(APIText);

            int count = (int)jsonDonutCount["count"];

            if (Id < 1 || Id > count)
            {
                ViewBag.Message = "That donut doesn't exist. Try again.";
                return View("Index");
            }

            // GET DONUT
            string donutURL = $"https://grandcircusco.github.io/demo-apis/donuts/{Id}.json";

            request = WebRequest.CreateHttp(donutURL);
            response = (HttpWebResponse)request.GetResponse();
            rd = new StreamReader(response.GetResponseStream());
            APIText = rd.ReadToEnd();

            JToken jsonDonut = JToken.Parse(APIText);

            Doughnut d = new Doughnut();

            d.Name = jsonDonut["name"].ToString();
            d.Calories = (int)jsonDonut["calories"];

            if(jsonDonut["photo"] != null)
            {
                d.PhotoURL = jsonDonut["photo"].ToString();
            }

            List<JToken> a = jsonDonut["extras"].ToList();

            List<string> listDonuts = new List<string>();

            if (a != null)
            {
                foreach(JToken jt in a)
                {
                    listDonuts.Add(jt.ToString());
                }
                d.Extras = listDonuts.ToArray();
            }


            ViewBag.Donut = d;

            return View(d);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
