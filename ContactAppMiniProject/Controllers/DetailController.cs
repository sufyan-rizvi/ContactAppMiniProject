using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContactAppMiniProject.Data;
using ContactAppMiniProject.Models;

namespace ContactAppMiniProject.Controllers
{
    [Authorize(Roles = "Staff")]
    public class DetailController : Controller
    {
        // GET: Detail
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(Guid id)
        {
            Session["ContactId"] = id;
            return View();
        }

        public ActionResult GetData(int page, int rows, string sidx, string sord, bool _search,
           string searchField, string searchString, string searchOper)
        {
            using (var s = Helper.CreateSession())
            {
                var id = (Guid)Session["ContactId"];
                var detailList = s.Query<ContactDetail>().Where(d => d.Contact.Id == id).ToList();
                if (_search && searchField == "Name" && searchOper == "eq")
                {
                    detailList = detailList.Where(q => q.Email == searchString).ToList();
                }


                //Get total count of records 
                int totalCount = detailList.Count();

                //Calculate total Pages
                int totalPages = (int)Math.Ceiling((double)totalCount / rows);

                switch (sidx)
                {
                    case "PhoneNumber":
                        detailList = sord == "asc" ? detailList.OrderBy(p => p.PhoneNumber).ToList()
                            : detailList.OrderByDescending(p => p.PhoneNumber).ToList();
                        break;

                    case "Email":
                        detailList = sord == "asc" ? detailList.OrderBy(p => p.Email).ToList()
                            : detailList.OrderByDescending(p => p.Email).ToList();
                        break;

                    default:
                        break;

                }



                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalCount,
                    

                    rows = detailList.Select(p => new
                    {
                        
                        cell = new string[]
                        {
                        p.Id.ToString(),                        
                        p.Email,
                        p.PhoneNumber
                        }
                    }).Skip((page - 1) * rows).Take(rows).ToArray()
                };

                //rows = (from product in products
                //        orderby sidx + " " + sord
                //        select new
                //        {
                //            cell = new string[]
                //            {
                //                product.Id.ToString(),
                //                product.Name,
                //                product.Description,
                //                product.Price.ToString()
                //            }
                //        }).Skip((page - 1) * rows).Take(rows).ToArray()

                //        var result = products.OrderBy(sidx + " " + sord)
                //.Select(product => new
                //{
                //    cell = new string[] {
                //        product.Id.ToString(),
                //        product.Name,
                //        product.Description,
                //        product.Price.ToString()
                //    }
                //})
                //.Skip((page - 1) * rows)
                //.Take(rows)
                //.ToArray();
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Add(ContactDetail detail)
        {
            using (var s = Helper.CreateSession())
            {
                using (var txn = s.BeginTransaction())
                {
                    var id = (Guid)Session["ContactId"];
                    detail.Contact = s.Query<Contact>().FirstOrDefault(d => d.Id == id);
                    s.Save(detail);
                    txn.Commit();
                }
            }
            return Json(new { success = true, message = "Product added successfully" });
        }

        public ActionResult Delete(Guid id)
        {
            using (var s = Helper.CreateSession())
            {
                using (var txn = s.BeginTransaction())
                {
                    var detail = s.Query<ContactDetail>().FirstOrDefault(d => d.Id == id);
                    s.Delete(detail);
                    txn.Commit();
                }
            }
            return Json(new { success = true, message = "Product Deleted successfully" });
        }

        public ActionResult Edit(ContactDetail detail)
        {
            using (var s = Helper.CreateSession())
            {
                using (var txn = s.BeginTransaction())
                {
                    var existingProduct = s.Query<ContactDetail>().FirstOrDefault(p => p.Id == detail.Id);
                    if (existingProduct != null)
                    {
                        existingProduct.PhoneNumber = detail.PhoneNumber;
                        existingProduct.Email = detail.Email;
                        s.Update(existingProduct);
                        txn.Commit();
                    }
                }
            }
            return Json(new { success = true, message = "Product edited successfully." });




        }

    }

} 
    
