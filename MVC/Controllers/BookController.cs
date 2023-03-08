using Microsoft.Ajax.Utilities;
using MVC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace MVC.Controllers
{
    public class BookController : Controller
    {
        // GET: Book
        public ActionResult Create()
        {
            using (var showdb = new MVCEntities())
            {

                var books = showdb.Books.Where(x => x.IsDeleted == false).ToList();
                ViewBag.data = books;
            }

            return View();

        }

        [HttpPost]
        public ActionResult Create(Book obj)
        {
            try
            {
                using (var db = new MVCEntities())
                {

                    //insertข้อ มูลแบบ obj เต็ม

                    obj.CreateDate = DateTime.Now;
                    obj.ModifiedDate = DateTime.Now;
                    obj.IsDeleted = false;

                    db.Books.Add(obj);
                    db.SaveChanges();

                    var books = db.Books.Where(x => x.IsDeleted == false).ToList();
                    ViewBag.data = books;

                }
                return View();
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        public ActionResult Edit(int id)
        {

            using (var db = new MVCEntities())
            {
                var obj = db.Books.Where(x => x.Id == id).FirstOrDefault();
                return View(obj);

            }
        }
        [HttpPost]
        //public ActionResult Edit(Book edit)
        //{
        //    using (var db = new MVCEntities())
        //    {
        //        var obj = db.Books.Where(x => x.Id == edit.Id).FirstOrDefault();
        //        obj.ISBN = edit.ISBN;
        //        obj.Title = edit.Title;
        //        obj.ModifiedDate = DateTime.Now;

        //        db.Books.Add(obj);
        //        db.Entry(obj).State = EntityState.Modified;
        //        db.SaveChanges();
        //    }
        //    return RedirectToAction("Create");
        //}
        public ActionResult EditItem(Book edit)
        {

            try
            {
                using (var db = new MVCEntities())
                {
                    var obj = db.Books.Where(x => x.Id == edit.Id).FirstOrDefault();
                    obj.ISBN = edit.ISBN;
                    obj.Title = edit.Title;
                    obj.ModifiedDate = DateTime.Now;

                    db.Books.Add(obj);
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, data = "OK" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult Delete(Book delete)
        {
            using (var db = new MVCEntities())
            {
                //var obj = db.Books.Where(x => x.Id == delete.Id).FirstOrDefault();
                //obj.ModifiedDate = DateTime.Now;
                //obj.IsDeleted = true;

                //db.Books.Add(obj);
                //db.Entry(obj).State = EntityState.Modified;
                //db.SaveChanges();

                //การ ลบ ข้อมูล

                //var obj = db.Books.Where(x => x.Id == 2).FirstOrDefault();
                //db.Books.Remove(obj);
                //db.SaveChanges();

            }
            return RedirectToAction("Create");
        }
        public ActionResult DeleteBook(int id)
        {

            try
            {
                using (var db = new MVCEntities())
                {
                    var obj = db.Books.Where(x => x.Id == id).FirstOrDefault();
                    obj.ModifiedDate = DateTime.Now;
                    obj.IsDeleted = true;

                    db.Books.Add(obj);
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        public ActionResult GetItemList()
        {
            using (var db = new MVCEntities())
            {
                var obj = db.Books.Where(x => x.IsDeleted == false)
                    .Select(y => new
                    {
                        y.Id,
                        y.Title,
                        y.ISBN
                    }).ToList();
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetBook(int id)
        {
            try
            {
                using (var db = new MVCEntities())
                {
                    var obj = db.Books.Where(x => x.Id == id)
                        .Select(y => new
                        {
                            y.Id,
                            y.Title,
                            y.ISBN
                        }).ToList();
                    return Json(new { success = true, data = obj }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = ex.Message }, JsonRequestBehavior.AllowGet);

            }

        }
    }

}
