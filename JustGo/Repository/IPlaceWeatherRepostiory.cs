using JustGo.Models;
using JustGo.ViewModels;

namespace JustGo.Repository
{
    public interface IPlaceWeatherRepostiory
    {

        public ICollection<Place> getPlace(SelectPlaceVM vm);
        public ICollection<Place> getPlaceFilter(SelectPlaceVM vm);
        public Place getPlaceId(string id);

        public ICollection<Weather> getWeatherByLocation(string location);
    }
}
