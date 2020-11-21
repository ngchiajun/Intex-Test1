using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Intex_Test1.Models;
using System.Data.Entity.Validation;

namespace Intex_Test1.Controllers
{
    [RoutePrefix("api/meme")]
    public class MemeController : ApiController
    {
        // private MemeContext db = new MemeContext();
        private IntextDBEntities2 db = new IntextDBEntities2();

        public IHttpActionResult Get()
        {
            var data = db.Images.ToList();
            foreach (var item in data)
            {
                item.requestCount += 1;
            }
            db.SaveChanges();
            return Ok(data); 
          
        }
        
        [HttpGet]
        [Route("id/{id}")]
        public IHttpActionResult Get(int id)
        {
            var data = db.Images.Where(s => s.id == id).ToList();
            
            data[0].requestCount += 1;
     
            db.SaveChanges();
            return Ok(data);

        }

        [HttpGet]
        [Route("page/{page}")]
        public IHttpActionResult GetPage(int page)
        {
            var data = db.Images.Where(s => s.page == page).ToList();
            foreach (var item in data)
            {
                item.requestCount += 1;
            }
            db.SaveChanges();
            return Ok(data);

        }

        [HttpGet]
        [Route("popular")]
        public IHttpActionResult GetPopular()
        {
            var data = db.Images.OrderByDescending(s => s.requestCount).FirstOrDefault();
            data.requestCount += 1;

            db.SaveChanges();
            return Ok(data);

        }

        [HttpPost]
        [Route("create")]
        public IHttpActionResult Create(Image newImage)
        {
            try
            {
                //var data = db.Images.OrderByDescending(s => s.id).FirstOrDefault();
            var data = db.Images.ToList();
            var count = data.Count();
            int newdata = (int)Math.Ceiling((count + 1) / 9.0);
            newImage.requestCount = 0;
            newImage.page = newdata;
               
                   
            db.Images.Add(newImage);
            db.SaveChanges();
            return Ok("babi");

            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }




        [HttpGet]
        [Route("all")]
        public string[] all()
        {
            return new string[]
            {
                "c",
                "d"
            };
        }
    }
}
