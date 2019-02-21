using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GridFS;
using MongoDB.Driver.Linq;
using System.Data.SqlClient;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {

            Console.WriteLine("Configuration Setting: 172.16.1.24:27017");
            Console.WriteLine("====================================================");
            string connectionString = "mongodb://localhost:27017";

            Console.WriteLine("Creating Client..........");
            MongoClient client = null;
            try
            {
                client = new MongoClient(connectionString);
                Console.WriteLine("Client Created Successfuly........");
                Console.WriteLine("Client: " + client.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Filed to Create Client.......");
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Initianting Mongo Db Server.......");
            MongoServer server = null;
            try
            {
                Console.WriteLine("Getting Servicer object......");
                server = client.GetServer();

                Console.WriteLine("Server object created Successfully....");
                Console.WriteLine("Server :" + server.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Filed to getting Server Details");
                Console.WriteLine(ex.Message);
            }


            Console.WriteLine("Initianting Mongo Databaser.........");
            MongoDatabase database = null;
            try
            {
                Console.WriteLine("Getting reference of database.......");

                database = server.GetDatabase("admin");
                Console.WriteLine("Database Name : " + database.Name);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to Get reference of Database");
                Console.WriteLine("Error :" + ex.Message);
            }

            Console.WriteLine("Getting Collections from database Database.......");

            MongoCollection symbolcollection = null;
            try
            {
                if (database.GetCollection("Symbols").Exists())
                {
                    symbolcollection = database.GetCollection("Symbols");
                    Console.WriteLine(symbolcollection.Count().ToString());
                }
                else
                {
                    database.CreateCollection("Symbols");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to Get collection from Database");
                Console.WriteLine("Error :" + ex.Message);
            }
            
            Symbol symbol;

            Console.WriteLine("Inserting document to collection............");
            try
            {
                ObjectId id = new ObjectId();
                for (int i = 0; i < 10; i++)
                {
                    symbol = new Symbol();
                    symbol.Name = "Option" + i;
                    symbol.Value = i;

                    symbolcollection.Insert(symbol);
                    id = symbol._id;
                }
                
                Console.WriteLine(symbolcollection.Count().ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to insert into collection of Database " + database.Name);
                Console.WriteLine("Error :" + ex.Message);
            }

            try
            {
                Console.WriteLine("Preparing Query Document............");
                
                List<Symbol> query = symbolcollection.AsQueryable<Symbol>().ToList();
                Console.WriteLine(query);
                List<SelectListItem> li = new List<SelectListItem>();
              
                for (int i = 0; i < query.Count(); i++)
                {
                    li.Add(new SelectListItem { Text = query[i].Name, Value = query[i].Value.ToString() });
                }
                ViewData["country"] = li;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to query from collection");
                Console.WriteLine("Exception :" + ex.Message);
            }
            Console.WriteLine("");
            Console.WriteLine("====================================================");
          
            return View();
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
