using System.Collections.Generic;

namespace Doppler.Payments.Weather;

public interface IWeatherForecastService
{
    IEnumerable<WeatherForecast> GetForecasts();
}
