using GitHubActionTest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GitHubActionTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        private readonly IConfiguration _configuration;


        public WeatherForecastController(ILogger<WeatherForecastController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;

        }

        [Authorize]
        [HttpGet("AuthGet")]
        public IEnumerable<WeatherForecast> Get2()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }


        [HttpGet("OpenGet")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("GetAuthenticated")]
        public string Test()
        {
            return "yo";
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserCredentials credentials)
        {
            // Tarkista t‰ss‰ credentials-olion arvot, esimerkiksi tietokantahakujen kautta
            if (credentials.Username == "testuser" && credentials.Password == "testpassword")
            {
                // Jos tunnistetiedot ovat oikein, generoi JWT-token ja palauta se
                var tokenService = new TokenService(_configuration); // Oletetaan, ett‰ TokenService on luokka, joka generoi tokenin
                var token = tokenService.GenerateToken();
                return Ok(new { Token = token });
            }
            else
            {
                // Jos tunnistetiedot ovat v‰‰rin, palauta virheilmoitus
                return Unauthorized("K‰ytt‰j‰tunnus tai salasana on v‰‰rin.");
            }
        }
    }
}