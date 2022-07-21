using JustGo.Models;

namespace JustGo.Repository
{
    public interface IPlaceWeatherRepostiory
    {

        public IEnumerable<Place> getPlace(int start, int quantity);
        public IEnumerable<Place> getPlaceFilter(string[] placeClass, string[] region, string[] town, int[] Class);
        public Place getPlaceId(string id);

        public Weather getWeatherByLocation(string location);
    }
}
