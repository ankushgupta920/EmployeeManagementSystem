using EmployeeManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeManagementSystem.Controllers
{
    public class LoginController : Controller
    {
        EmpManagementSystemMVCEntities db = new EmpManagementSystemMVCEntities();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(LoginEMP obj)
        {
            if (ModelState.IsValid == true)
            {
                var row = db.LoginEMPs.Where(model => model.username == obj.username && model.password == obj.password).FirstOrDefault();
                if (row == null)
                {
                    ViewBag.ErrorMessage = "Login Failed";
                    return View();
                }
                else
                {
                    Session["username"] = obj.username;
                    return RedirectToAction("Index","Home");
                }
            }
           return View();
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index","Login");
        }
    }
}