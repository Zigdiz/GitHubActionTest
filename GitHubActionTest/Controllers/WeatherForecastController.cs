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

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
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

        [HttpGet("QP")]
        public IEnumerable<WeatherForecast> Get([FromQuery] string id, [FromQuery] string name)
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }


        [HttpPost("login")]
        public IActionResult Login([FromBody] UserCredentials credentials)
        {
            // Tarkista t‰ss‰ credentials-olion arvot, esimerkiksi tietokantahakujen kautta
            if (credentials.Username == "testuser" && credentials.Password == "testpassword")
            {
                // Jos tunnistetiedot ovat oikein, generoi JWT-token ja palauta se
                var tokenService = new TokenService(); // Oletetaan, ett‰ TokenService on luokka, joka generoi tokenin
                var token = tokenService.GenerateToken(credentials.Username, false);
                return Ok(new { Token = token });
            }
            else if(credentials.Username == "admin" && credentials.Password == "adminpassword")
            {
                var tokenService = new TokenService();
                var token = tokenService.GenerateToken(credentials.Username, true);
                return Ok(new { Token = token });
            }
            else
            {
                // Jos tunnistetiedot ovat v‰‰rin, palauta virheilmoitus
                return Unauthorized("K‰ytt‰j‰tunnus tai salasana on v‰‰rin.");
            }
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("GetSecret")]
        public IActionResult GetSecretData()
        {
            return Ok("T‰m‰ on suojattua tietoa vain Admin-roolia k‰ytt‰vilt‰.");
        }

        [Authorize(Policy = "RequireUserRole")]
        [HttpGet("GetSecretUser")]
        public IActionResult GetSecretDataUser()
        {
            return Ok("T‰m‰ on suojattua tietoa vain Admin-roolia k‰ytt‰vilt‰.");
        }

    }
}