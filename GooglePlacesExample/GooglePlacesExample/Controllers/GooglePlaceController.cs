using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GooglePlacesExample.Controllers;
using GooglePlacesExample.Models;


namespace GooglePlacesExample.Controllers
{
    public class GooglePlaceController : Controller
    {
        private JObject jobject;
        private readonly string APIKEY = "AIzaSyBrSztRX-AnW3upjsYjKwWl71E_5YfFa_E ";

        // GET: GooglePlace
        [HttpGet]
        public ActionResult search()
        {
            return View();
        }
        [HttpPost]
        public ActionResult search(decimal latitude = 0, decimal longitude = 0, int radius = 0, string type = "", string name = "")
        {

            string searchUrl = @"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=" + latitude + "," + longitude +

               @"&radius=" + radius + @"&type=" + type + @"&name=" + name + @"&key=AIzaSyBrSztRX-AnW3upjsYjKwWl71E_5YfFa_E";

            // JObject jobject = MakeRequest(searchUrl);
           // searchUrl = @"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=-33.8670522,151.1957362&radius=500&type=restaurant&name=cruise&key=AIzaSyDilaKqynhjELXyHU0vZJU3L8nNUw2T9S0";
          // string searchUrl = @"http://forecast.weather.gov/MapClick.php?lat=38.4247341&lon=-86.9624086&FcstType=json";


            JObject jobject = MakeRequest(searchUrl);
            var results = jobject["results"].ToList();

            List<PassThroughPart> parts = new List<PassThroughPart>();

            foreach (var r in results)
            {

                PassThroughPart p = new PassThroughPart();
                p.lat = (decimal)r["geometry"]["location"]["lat"];
                p.lng = (decimal)r["geometry"]["location"]["lng"];


                p.name = (string)r["name"];
                p.icon = (string)r["icon"];
                if (r["rating"] != null)
                {
                    p.rating = (decimal)r["rating"];
                }
                p.types = r["types"].ToString().Split(',').ToList();

                parts.Add(p);
            }
            ViewBag.Latitude = latitude;
            ViewBag.Longitude = longitude;
            ViewBag.Results = parts;

            return View("SearchResults");
        }
        public JObject MakeRequest(string requestUrl)
        {
            JObject o = new JObject();
            try 
     {                                                  

                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        o = JObject.Parse("{error:\"Server error\"}");
                        // throw new Exception(String.Format("Server error (HTTP {0}: {1}).", response.StatusCode, response.StatusDescription));
                    }
                    System.IO.Stream stream = response.GetResponseStream();
                    dynamic dynObj = JsonConvert.DeserializeObject(stream.ToString());


                    o = JObject.Parse(stream.ToString());
                    return o;
                }
            }
            catch (Exception e)
            {
                // catch exception and log it
                o = JObject.Parse("{error:\"" + e.Message + "\"}");

                return o;
            }

        }
    }
}