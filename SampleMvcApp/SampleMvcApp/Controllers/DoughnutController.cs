using Sample.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SampleMvcApp.Controllers
{
    public class DoughnutController : Controller
    {
        private SampleContext db = new SampleContext();

        // GET: Doughnut
        public ActionResult Index()
        {
            // Get list of all doughnuts from database
            List<Doughnut> doughnuts = db.Doughnuts.ToList<Doughnut>();
            return View(doughnuts);
        }

        // GET: Doughnut/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Doughnut/CreatePost
        [HttpPost]
        public ActionResult CreatePost()
        {
            // Get values that were posted by doughnut creation form
            string doughnutName = Request.Form["doughnutName"];
            string flavorName = Request.Form["flavorName"];

            // Ensure neither values are null
            if(doughnutName == "" || flavorName == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Flavor newFlavor;

            // Check to see if database already contains flavor
            newFlavor = (from Flavor flav in db.Flavors
                         where flav.Name.ToLower() == flavorName.ToLower()
                         select flav).FirstOrDefault<Flavor>();

            // If flavor was not found in database, create new flavor entity
            if(newFlavor == null)
            {
                newFlavor = new Flavor()
                {
                    Name = flavorName
                };
                db.Flavors.Add(newFlavor);
            }

            //Create new doughnut entity
            Doughnut newDoughnut = new Doughnut()
            {
                Name = doughnutName,
                Flavor = newFlavor
            };
            db.Doughnuts.Add(newDoughnut);
            db.SaveChanges();
            

            // Return to list of doughnuts
            return RedirectToAction("Index", "Doughnut");
        }

        // GET: Doughnut/Edit
        public ActionResult Edit(int? id)
        {

            // Ensure the id is not null
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Find doughnut in database by id
            Doughnut doughnut = db.Doughnuts.Find(id);
            if (doughnut == null)
            {
                return HttpNotFound();
            }

            // Return view with doughnut model if doughnut is found
            return View(doughnut);
        }

        // POST: Doughnut/EditPost
        [HttpPost]
        public ActionResult EditPost()
        {
            // Get values that were posted by doughnut creation form
            int? doughnutId = int.Parse(Request.Form["doughnutId"]);
            string doughnutName = Request.Form["doughnutName"];
            string flavorName = Request.Form["flavorName"];

            // Ensure no values are null
            if (doughnutId == null || doughnutName == "" || flavorName == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Find doughnut in database by id
            Doughnut doughnut = db.Doughnuts.Find(doughnutId);
            if (doughnut == null)
            {
                return HttpNotFound();
            }
            
            Flavor flavor = doughnut.Flavor;

            // If flavor name has changed...
            if(flavor.Name != flavorName)
            {
                // Remove doughnut entity from Flavor.Doughnuts and then delete flavor if no doughnuts are attached
                flavor.Doughnuts.Remove(doughnut);
                if(flavor.Doughnuts.Count() == 0)
                {
                    db.Flavors.Remove(flavor);
                    db.Entry(flavor).State = EntityState.Deleted;
                }

                // Check to see if database already contains new flavor
                flavor = (from Flavor flav in db.Flavors
                             where flav.Name.ToLower() == flavorName.ToLower()
                             select flav).FirstOrDefault<Flavor>();

                // If flavor was not found in database, create new flavor entity
                if (flavor == null)
                {
                    flavor = new Flavor()
                    {
                        Name = flavorName
                    };
                    db.Flavors.Add(flavor);
                }
            }

            
            if(doughnut.Name != doughnutName)
            {
                // If doughnut name has changed...

                // Remove doughnut from database
                db.Doughnuts.Remove(doughnut);
                db.Entry(doughnut).State = EntityState.Deleted;

                //Create new doughnut entity
                Doughnut newDoughnut = new Doughnut()
                {
                    Name = doughnutName,
                    Flavor = flavor
                };
                db.Doughnuts.Add(newDoughnut);
            } else
            {
                // If doughnut name has not changed

                // Set doughnut.Flavor to flovor in case flavor was modified
                doughnut.Flavor = flavor;
                db.Entry(doughnut).State = EntityState.Modified;
            }
            db.SaveChanges();

            // Return to list of doughnuts
            return RedirectToAction("Index", "Doughnut");
        }

        // GET: Doughnut/Delete
        public ActionResult Delete(int? id)
        {
            // Ensure the id is not null
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Find doughnut in database by id
            Doughnut doughnut = db.Doughnuts.Find(id);
            if (doughnut == null)
            {
                return HttpNotFound();
            }

            // Remove doughnut from flavor and flavor from database if no doughnuts are attached to it
            Flavor flavor = doughnut.Flavor;
            flavor.Doughnuts.Remove(doughnut);
            if(flavor.Doughnuts.Count() == 0)
            {
                db.Flavors.Remove(flavor);
                db.Entry(flavor).State = EntityState.Deleted;
            }

            // Remove doughnut from database
            db.Doughnuts.Remove(doughnut);
            db.Entry(doughnut).State = EntityState.Deleted;
            db.SaveChanges();

            return RedirectToAction("Index", "Doughnut");
        }
    }
}