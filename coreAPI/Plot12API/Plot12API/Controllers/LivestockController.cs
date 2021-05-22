using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using Plot12API.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Plot12API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LivestockController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public LivestockController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                      select ID, Cattle, Pigs, Goats, Chicken, Date, BookedBy";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("PlotAppCon");
            using(SqlConnection myCon=new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    SqlDataReader myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }
        [HttpPost]

        public JsonResult Post(Livestock liv)
        {
            string query = @"
                      insert into dbo.Livestock values
                       ('" + liv.Cattle + "','" + liv.Pigs + "','" + liv.Goats + "','" + liv.Chicken + "','" + liv.Date + "','" + liv.BookedBy + "')";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("PlotAppCon");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    SqlDataReader myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Information Sucessfully added");
        }
        [Route("Save File")]
        [HttpPost]

        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + filename;

                using(var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(filename);
            }
            catch(Exception)
            {

                return new JsonResult("Photo.jpg");
            }
        }
        
    }
}
