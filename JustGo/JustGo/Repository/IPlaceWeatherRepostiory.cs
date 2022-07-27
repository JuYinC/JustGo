using JustGo.Models;
using JustGo.ViewModels;

namespace JustGo.Repository
{
    public interface IPlaceWeatherRepostiory
    {

        public ICollection<Place> getPlace(int start, int quantity);
        public ICollection<Place> getPlaceFilter(SelectPlaceVM vm);
        public Place getPlaceId(string id);

        public Weather getWeatherByLocation(string location);
    }
}
