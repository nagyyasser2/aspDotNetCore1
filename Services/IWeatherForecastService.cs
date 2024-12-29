namespace aspDotNetCore.Services
{
    public interface IWeatherForecastService
    {
        public IEnumerable<WeatherForecast> GetForecasts();
    }
}
