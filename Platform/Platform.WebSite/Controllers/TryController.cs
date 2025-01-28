using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Platform.WebSite.Controllers
{
    public class TryController : BaseMVCController
    {
        // GET: Try
        public ActionResult Index()
        {
            this.InitAction();

            return View();
        }

        // GET: Try
        public ActionResult Index2()
        {
            this.InitAction();

            return View();
        }


        // GET: Try/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Try/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Try/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Try/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Try/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Try/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Try/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
