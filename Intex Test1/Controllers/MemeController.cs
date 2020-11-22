using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Intex_Test1.Models;
using System.Data.Entity.Validation;
using Intex_Test1.Dtos;

namespace Intex_Test1.Controllers
{
    [RoutePrefix("api/meme")]
    public class MemeController : ApiController
    {
        // private MemeContext db = new MemeContext();
        private IntextDBEntities3 db = new IntextDBEntities3();

        public IHttpActionResult Get()
        {
            try
            {
                var data = db.Images.ToList();
                foreach (var item in data)
                {
                    item.requestCount += 1;
                }
                db.SaveChanges();

                return Ok(data);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.ToString()); // try publish again
            }
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
        public IHttpActionResult Create(DataDto data)
        {
            try
            {
                foreach (ImageDto imageDto in data.Data)
                {
                    Image newImage = new Image();

                    newImage.name = imageDto.Name;
                    newImage.url = imageDto.Url;
                    newImage.page = GetPageNo();
                    newImage.requestCount = 0;
                        
                    db.Images.Add(newImage);
                    db.SaveChanges();
                }

                return Ok("Created");
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

        // Extract the calculation of page
        private int GetPageNo()
        {
            // Config number of images per page in DB
            int noOfImgPerPage = Int32.Parse(db.Configurations.Where(c => c.id == "NOI01").Select(c => c.configVal).ToList()[0]);
            int count = db.Images.ToList().Count();
            int pageNo = (int)Math.Ceiling((count + 1) / (double)noOfImgPerPage);

            return pageNo;
        }
    }
}
