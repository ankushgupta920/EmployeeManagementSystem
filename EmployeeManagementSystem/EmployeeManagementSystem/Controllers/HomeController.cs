using EmployeeManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        EmpManagementSystemMVCEntities db = new EmpManagementSystemMVCEntities();
        // GET: Home
        public ActionResult Index(string searchby,string search)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                if (searchby == "Name")
                {
                    var data = db.Employees.Where(model => model.name.StartsWith(search)).ToList();
                    return View(data);
                }
                else if (searchby == "Email")
                {
                    var data = db.Employees.Where(model => model.email.StartsWith(search)).ToList();
                    return View(data);
                }
                else
                {
                    var data = db.Employees.ToList();
                    return View(data);
                }
            }
        }
        public ActionResult Create()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult Create(Employee obj)
        {
            if (ModelState.IsValid == true)
            {
                string fileName = Path.GetFileNameWithoutExtension(obj.ImageFile.FileName);
                string Extension = Path.GetExtension(obj.ImageFile.FileName);
                HttpPostedFileBase obj2 = obj.ImageFile;
                int length = obj2.ContentLength;

                if (Extension.ToLower() == ".jpg" || Extension.ToLower() == ".png" || Extension.ToLower() == ".jpeg")
                {
                    if (length <= 1000000)
                    {
                        fileName = fileName + Extension;
                        obj.image_path = "~/images/" + fileName;
                        // This 2 lines is for saving the file in the images folder:---
                        fileName = Path.Combine(Server.MapPath("~/images/"), fileName);
                        obj.ImageFile.SaveAs(fileName);

                        db.Employees.Add(obj);
                        int i = db.SaveChanges();
                        if (i > 0)
                        {
                            TempData["CreateMsg"] = "<script>alert('Data Inserted Successfully')</script>";
                            ModelState.Clear();
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            TempData["CreateMsg"] = "<script>alert('Data Not Inserted')</script>";
                        }
                    }
                    else
                    {
                        TempData["SizeMsg"] = "<script>alert('Image size should be less than 1MB')</script>";
                    }
                }
                else
                {
                    TempData["ExtensionMsg"] = "<script>alert('Format Not Supported')</script>";
                }
            }
            return View();
        }
        public ActionResult Edit(int id)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                var row = db.Employees.Where(model => model.id == id).FirstOrDefault();
                Session["Image"] = row.image_path;
                return View(row);
            }
        }
        [HttpPost]
        public ActionResult Edit(Employee obj)
        {
            if (ModelState.IsValid == true)
            {
                if (obj.ImageFile != null)
                {

                    string fileName = Path.GetFileNameWithoutExtension(obj.ImageFile.FileName);
                    string Extension = Path.GetExtension(obj.ImageFile.FileName);
                    HttpPostedFileBase obj2 = obj.ImageFile;
                    int length = obj2.ContentLength;

                    if (Extension.ToLower() == ".jpg" || Extension.ToLower() == ".png" || Extension.ToLower() == ".jpeg")
                    {
                        if (length <= 1000000)
                        {
                            fileName = fileName + Extension;
                            obj.image_path = "~/images/" + fileName;
                            // This 2 lines is for saving the file in the images folder:---
                            fileName = Path.Combine(Server.MapPath("~/images/"), fileName);
                            obj.ImageFile.SaveAs(fileName);

                            db.Entry(obj).State = EntityState.Modified;
                            int i = db.SaveChanges();
                            if (i > 0)
                            {
                                string ImagePathh = Request.MapPath(Session["Image"].ToString());
                                if (System.IO.File.Exists(ImagePathh))
                                {
                                    System.IO.File.Delete(ImagePathh);
                                }
                                TempData["UpdateMsg"] = "<script>alert('Data Updated Successfully')</script>";
                                ModelState.Clear();
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                TempData["UpdateMsg"] = "<script>alert('Data Not Updated')</script>";
                            }
                        }
                        else
                        {
                            TempData["SizeMsg"] = "<script>alert('Image size should be less than 1MB')</script>";
                        }
                    }
                    else
                    {
                        TempData["ExtensionMsg"] = "<script>alert('Format Not Supported')</script>";
                    }

                }
                else
                {
                    obj.image_path = Session["Image"].ToString();
                    db.Entry(obj).State = EntityState.Modified;
                    int i = db.SaveChanges();
                    if (i > 0)
                    {
                        TempData["UpdateMsg"] = "<script>alert('Data Updated Successfully')</script>";
                        ModelState.Clear();
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["UpdateMsg"] = "<script>alert('Data Not Updated')</script>";
                    }
                }
            }
            return View();
        }
        public ActionResult Delete(int id)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                if (id > 0)
                {
                    var row = db.Employees.Where(model => model.id == id).FirstOrDefault();
                    if (row != null)
                    {
                        db.Entry(row).State = EntityState.Deleted;
                        int i = db.SaveChanges();
                        if (i > 0)
                        {
                            TempData["DeleteMsg"] = "<script>alert('Data Deleted Successfully')</script>";
                            string ImagePathh = Request.MapPath(row.image_path.ToString());
                            if (System.IO.File.Exists(ImagePathh))
                            {
                                System.IO.File.Delete(ImagePathh);
                            }
                        }
                        else
                        {
                            TempData["DeleteMsg"] = "<script>alert('Data Not Deleted')</script>";
                        }
                    }
                }
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult Details(int id)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                var row = db.Employees.Where(model => model.id == id).FirstOrDefault();
                Session["Image2"] = row.image_path;
                return View(row);
            }
        }
    }
}