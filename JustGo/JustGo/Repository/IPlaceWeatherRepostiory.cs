using JustGo.Models;
using JustGo.ViewModels;

namespace JustGo.Repository
{
    public interface IPlaceWeatherRepostiory
    {

        public IQueryable<fn_selePlaceDistanceResult> getPlace(double centerLat, double centerLng);
        public ICollection<Place> getPlaceFilter(SelectPlaceVM vm);
        public Place getPlaceId(string id);

        public Weather getWeatherByLocation(string location);
    }
}
