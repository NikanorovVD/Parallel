using ConsoleApp1;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JoinController : ControllerBase
    {
        private readonly string connectionString;
        public JoinController(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet]
        public string Get([FromQuery]Query query)
        {
            var join = new BucketJoin(connectionString);

            join.CreateTables(query.KeyLength);
            join.GenerateData(query.RecordsCount, query.KeyLength, query.KeyVariations);
            join.CreateSortedTables();

            return join.ComparePerformance(query.RecordsCount, query.VerifyResults);
        }
    }

    public class Query
    {
        [Required]
        [Range(0, 1_000_000)]
        public int RecordsCount { get; set; }

        [Required]
        [Range(1, 100)]
        public int KeyLength { get; set; }

        [Required]
        [Range(2, 26)]
        public int KeyVariations { get; set; }

        public bool VerifyResults { get; set; } = true;
    }
}
