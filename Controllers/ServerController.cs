using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CsvHelper;
using CsvHelper.Configuration;
using System.IO;
using System.Linq;
using System.Globalization;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CsvHelper;
using CsvHelper.Configuration;
using System.IO;
using System.Linq;

namespace Galytix.WebApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ServerController : ControllerBase
    {
        private readonly string csvFilePath = "./Data/gwtByCountry.csv"; 

        [AllowAnonymous]
        [HttpGet]
        [Route("ping")]
        public async Task<IActionResult> Ping()
        {
            return Ok("pong");
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("postexample")]
        public async Task<IActionResult> PostExample([FromBody] YourRequestModel requestModel)
        {
            var response = new
            {
                Message = "POST request received successfully",
                ReceivedData = new
                {
                    Country = requestModel.Country,
                    LineOfBusiness = requestModel.Lob
                },
                AllCountriesData = GetAllCountriesData()
            };

            return Ok(response);
        }

        private dynamic GetAllCountriesData()
        {
            using (var reader = new StreamReader(csvFilePath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                var records = csv.GetRecords<dynamic>().ToList();

                var countriesData = records
                    .GroupBy(record => record.Country)
                    .ToDictionary(
                        group => group.Key,
                        group => group.ToList()
                    );

                return countriesData;
            }
        }
    }

    public class YourRequestModel
    {
        public string Country { get; set; }
        public string[] Lob { get; set; }
    }
}