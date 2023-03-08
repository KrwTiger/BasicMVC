using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult TableBorrow()
        {
            try
            {
                using (var db = new MVCEntities())

                {

                    var obj = db.Transactions
                        .Join(db.Books, Transactions => Transactions.BookId, Books => Books.Id, (Transactions, Books) => new
                        {
                            Transactions,
                            Books
                        }).Join(db.Employees, Transactions => Transactions.Transactions.EmployeeId, Employees => Employees.Id, (Transactions, Employees) => new
                        {
                            Transactions,
                            Employees
                        })
                        .Where(x => x.Transactions.Transactions.IsDeleted == false)
                        .Select(y => new
                        {
                            y.Transactions.Transactions.Id,
                            y.Transactions.Transactions.BookId,
                            y.Transactions.Transactions.ModifiedDate,
                            y.Transactions.Transactions.Employee.Fullname,
                            y.Transactions.Transactions.Book.Title

                        }).OrderByDescending(z => z.ModifiedDate).ToList();

                    return Json(obj, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult TableBook()
        {
            try
            {
                using (var db = new MVCEntities())

                {

                    var obj = db.Books.Where(x => x.IsDeleted == false).Select(
                        s => new
                        {
                            s.Id,
                            s.Title,
                            s.ModifiedDate
                        }).OrderBy(z => z.ModifiedDate).ToList();

                    return Json(obj, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}