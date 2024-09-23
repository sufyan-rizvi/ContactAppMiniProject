using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Security;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using ContactAppMiniProject.DTOs;
using ContactAppMiniProject.Data;
using ContactAppMiniProject.Models;
using NHibernate.Linq;

namespace ContactAppMiniProject.Controllers
{
    [Authorize(Roles = "Staff")]
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetContacts()
        {
            var user = (User)Session["User"];
            using (var s = Helper.CreateSession())
            {
                //var contacts = s.Query<Contact>().FetchMany(c=>c.Details).Fetch(c=>c.User).Where(c => c.User.Id == user.Id).Select(c => new Contact
                //{
                //    Id = c.Id,
                //    FirstName = c.FirstName,
                //    LastName = c.LastName,
                //    IsActive = c.IsActive
                //}).ToList();

                var data = s.Query<Contact>().Where(c => c.User.Id == user.Id).ToList();
                var contacts = Mapper.Map<List<ContactDTO>>(data);


                return Json(contacts, JsonRequestBehavior.AllowGet);

            }
        }
        public ActionResult AddContact()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddContact(Contact contact)
        {
            if (!ModelState.IsValid)
                return Json(false);
            using (var s = Helper.CreateSession())
            {
                using (var txn = s.BeginTransaction())
                {
                    var userId = ((User)Session["User"]).Id;
                    contact.User = s.Query<User>().FirstOrDefault(u => u.Id == userId);
                    contact.IsActive = true;

                    s.Save(contact);
                    txn.Commit();
                    return Json(true);
                }
            }
        }
        [HttpPost]
        public JsonResult DeleteContact(Guid id)
        {
            using (var s = Helper.CreateSession())
            {
                using (var txn = s.BeginTransaction())
                {
                    var contact = s.Query<Contact>().FirstOrDefault(u => u.Id == id);
                    contact.IsActive = !contact.IsActive;
                    s.Update(contact);
                    txn.Commit();
                    return Json("Great success");
                }
            }
        }

        public ActionResult GetContact(Guid id)
        {
            using(var s =  Helper.CreateSession())
            {
                var contact = s.Query<Contact>().FirstOrDefault(c => c.Id == id);
                var dto = Mapper.Map<ContactDTO>(contact);
                return Json(dto, JsonRequestBehavior.AllowGet);
            }            
        }

        [HttpPost]
        public JsonResult EditContact(Contact contact)
        {
            using(var s = Helper.CreateSession())
            {
                using(var txn = s.BeginTransaction())
                {
                    var existingContact = s.Query<Contact>().FirstOrDefault(c=>c.Id== contact.Id);
                    existingContact.FirstName = contact.FirstName;
                    existingContact.LastName = contact.LastName;
                    s.Update(existingContact);
                    txn.Commit();
                    return Json("Great success");
                }
            }
        }
    }
}