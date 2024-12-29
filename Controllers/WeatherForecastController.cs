using aspDotNetCore.Services;
using Microsoft.AspNetCore.Mvc;

namespace aspDotNetCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherForecastService _weatherForcastService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastService weatherForcastService)
        {
            this._logger = logger;
            this._weatherForcastService = weatherForcastService;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return this._weatherForcastService.GetForecasts();
        }
    }
}
