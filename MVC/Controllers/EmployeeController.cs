using MVC.Models;
using MVC.ModelView;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult Index()
        {
            return View();

        }

        [HttpPost]
        public ActionResult CreateEmployee(EmployeeCreateView obj)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var db = new MVCEntities())
                    {
                        //var employee = new Employee
                        //{
                        //    CreateDate = DateTime.Now,
                        //    ModifiedDate = DateTime.Now,
                        //    IsDeleted = false,
                        //    Fullname = obj.Fullname
                        //};

                        //db.Employees.Add(employee);


                        //var address = new Address
                        //{
                        //    CreateDate = DateTime.Now,
                        //    ModifiedDate = DateTime.Now,
                        //    IsDeleted = false,
                        //    Address1 = obj.Address,
                        //    Phone = obj.Phone,
                        //    EmployeeId = employee.Id

                        //};


                        //db.Addresses.Add(address);
                        //db.SaveChanges();

                        return View(obj);
                    }

                }
                catch (Exception ex)
                {
                    return View(new { Message = ex.Message });
                }
            }
            else
            {
                ModelState.AddModelError("", "Input necessery fields.");
            }

            return View(obj);
        }

        public ActionResult EditEmployee(Employee edit)
        {

            try
            {
                using (var db = new MVCEntities())
                {
                    var obj = db.Employees.Where(x => x.Id == edit.Id).FirstOrDefault();
                    obj.ModifiedDate = DateTime.Now;
                    obj.Fullname = edit.Fullname;

                    db.Employees.Add(obj);
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

        public ActionResult DeleteEmployee(int id)
        {
            try
            {
                using (var db = new MVCEntities())
                {
                    var obj = db.Employees.Where(x => x.Id == id).FirstOrDefault();
                    obj.IsDeleted = true;

                    db.Employees.Add(obj);
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

        public ActionResult TableEmployee()
        {
            try
            {
                using (var db = new MVCEntities())
                {

                    var obj = db.Employees.Where(x => x.IsDeleted == false)
                       .Select(y => new
                       {
                           y.Id,
                           y.ModifiedDate,
                           y.Fullname
                       }).ToList();
                    return Json(obj, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetEmployee(int id)
        {
            try
            {
                using (var db = new MVCEntities())
                {
                    var obj = db.Employees.Where(x => x.Id == id)
                       .Select(y => new
                       {
                           y.Id,
                           y.ModifiedDate,
                           y.Fullname
                       }).ToList();
                    return Json(new { success = true, data = obj }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = ex.Message }, JsonRequestBehavior.AllowGet);

            }

        }

        public ActionResult Employee()
        {
            return View();
        }

        
        //public ActionResult Employee(EmployeeCreateView obj)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            using (var db = new MVCEntities())
        //            {
        //                //var employee = new Employee
        //                //{
        //                //    CreateDate = DateTime.Now,
        //                //    ModifiedDate = DateTime.Now,
        //                //    IsDeleted = false,
        //                //    Fullname = obj.Fullname
        //                //};

        //                //db.Employees.Add(employee);


        //                //var address = new Address
        //                //{
        //                //    CreateDate = DateTime.Now,
        //                //    ModifiedDate = DateTime.Now,
        //                //    IsDeleted = false,
        //                //    Address1 = obj.Address,
        //                //    Phone = obj.Phone,
        //                //    EmployeeId = employee.Id

        //                //};


        //                //db.Addresses.Add(address);
        //                //db.SaveChanges();

        //                return View(obj);
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            return View(new {Message = ex.Message });
        //        }
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("", "Input necessery fields.");
        //    }

        //    return View(obj);
        //}
    }
}