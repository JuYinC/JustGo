using JustGo.Models;
using JustGo.ViewModels;

namespace JustGo.Repository
{
    public interface IPlaceWeatherRepostiory
    {

        public IEnumerable<Place> getPlace(int start, int quantity);
        public IEnumerable<Place> getPlaceFilter(SelectPlaceVM vm);
        public Place getPlaceId(string id);

        public Weather getWeatherByLocation(string location);
    }
}
