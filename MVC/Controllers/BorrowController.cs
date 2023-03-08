using MVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Util;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace MVC.Controllers
{

    public class GetData
    {
        public int TransactionId { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public int BookId { get; set; }
        public int EmployeeId { get; set; }

    }
    public class BorrowController : Controller
    {

        // GET: Borrow
        public ActionResult BorrowIndex()
        {
            using (var db = new MVCEntities())
            {
                return View();
            }

        }

        public ActionResult Book()
        {
            using (var db = new MVCEntities())
            {
                var obj = new BorrowModel();
                var _book = db.Books.Where(x => x.IsDeleted == false).ToList();
                
                obj.books = _book.Select(x => new Book
                {
                    Id = x.Id,
                    Title = x.Title,
                    ISBN = x.ISBN

                }).ToList();

                return Json(new { success = true, data = obj.books }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult TableBook()
        {
            using (var db = new MVCEntities())
            {
                var obj = new BookModel();
                var _book = db.Books.Where(x => x.IsDeleted == false).ToList();

                obj.TableBooks = _book.Select(x => new Book
                {
                    Id = x.Id,
                    Title = x.Title,
                    ISBN = x.ISBN

                }).ToList();

                return Json(obj.TableBooks, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Employee()
        {

            using (var db = new MVCEntities())
            {

                var obj = new BorrowModel();
                var _employee = db.Employees.Where(x => x.IsDeleted == false).ToList();

                obj.employees = _employee.Select(x => new Employee
                {
                    Id = x.Id,
                    Fullname = x.Fullname

                }).ToList();

                return Json(new { success = true, data = obj.employees }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult BorrowBook(string json)
        {
            var _obj = new GetData();

            var serializer = new JavaScriptSerializer();
            dynamic jsondata = serializer.Deserialize(json, typeof(object));

            //Get your variables here from AJAX call
            var checkBook = jsondata["checkBook"];
            var Employee = jsondata["Employee"];
            var i = 0;

            try
            {

                using (var db = new MVCEntities())
                {
                    var obj = new Transaction();

                    foreach (var item in checkBook)
                    {

                        obj.CreateDate = DateTime.Now;
                        obj.ModifiedDate = DateTime.Now;
                        obj.IsDeleted = false;
                        obj.BookId = checkBook[i++];
                        obj.EmployeeId = Employee;

                        db.Transactions.Add(obj);
                        db.SaveChanges();

                        var book = db.Books.Where(x => x.Id == obj.BookId).FirstOrDefault();
                        book.IsDeleted = true;

                        db.Books.Add(book);
                        db.Entry(book).State = EntityState.Modified;
                        db.SaveChanges();

                        _obj = new GetData
                        {

                            TransactionId = obj.Id,
                            ModifiedDate = obj.ModifiedDate,
                            IsDeleted = obj.IsDeleted,
                            BookId = obj.BookId,
                            EmployeeId = obj.EmployeeId

                        };

                        _obj.Equals(obj);

                    }

                }
                return Json(new { success = true, data = _obj });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        public ActionResult TableBorrow()
        {
            try
            {
                using (var db = new MVCEntities())

                {
                    var obj = db.Transactions.Where(x => !x.IsDeleted)
                        .Select(y => new
                        {
                            y.Id,
                            y.BookId,
                            y.ModifiedDate,
                            y.Employee.Fullname,
                            y.Book.Title,

                        }).ToList();

                    return Json(obj, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetBorrow(int id)
        {
            try
            {
                using (var db = new MVCEntities())
                {

                    var obj = db.Transactions.Where(x => x.Id == id && !x.IsDeleted)
                        .Select(y => new
                        {
                            y.Id,
                            y.EmployeeId,
                            y.BookId

                        }).ToList();

                    return Json(new { success = true, data = obj }, JsonRequestBehavior.AllowGet);

                }

            }

            catch (Exception ex)

            {
                return Json(new { success = false, data = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult UpdateBorrow(Transaction update)
        {
            try
            {
                using (var db = new MVCEntities())
                {


                    var obj = db.Transactions.Where(x => x.Id == update.Id).FirstOrDefault();

                    //update Old BookId => IsDeleted = 0
                    var OldBook = db.Books.Where(b => b.Id == obj.BookId).FirstOrDefault();
                    OldBook.IsDeleted = false;

                    db.Books.Add(OldBook);
                    db.Entry(OldBook).State = EntityState.Modified;

                    //update borrow book

                    obj.ModifiedDate = DateTime.Now;
                    obj.EmployeeId = update.EmployeeId;
                    obj.BookId = update.BookId;

                    db.Transactions.Add(obj);
                    db.Entry(obj).State = EntityState.Modified;

                    //update New BookId => IsDeleted =1
                    var book = db.Books.Where(b => b.Id == update.BookId).FirstOrDefault();
                    book.IsDeleted = true;

                    db.Books.Add(book);
                    db.Entry(book).State = EntityState.Modified;
                    db.SaveChanges();

                    return Json(new { success = true });
                }

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult DeleteBorrow(int id)
        {
            try
            {
                using (var db = new MVCEntities())
                {

                    var obj = db.Transactions.Where(x => x.Id == id).FirstOrDefault();
                    obj.IsDeleted = true;

                    db.Transactions.Add(obj);
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();

                    var book = db.Books.Where(b => b.Id == obj.BookId).FirstOrDefault();
                    book.IsDeleted = false;

                    db.Books.Add(book);
                    db.Entry(book).State = EntityState.Modified;
                    db.SaveChanges();

                    return Json(new { Result = "OK" });
                }

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}