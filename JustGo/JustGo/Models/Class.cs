using System.Collections;

namespace JustGo.Models
{
    public class Class : IPlaceWeatherRepostiory
    {
        public IEnumerable<Place> getPlace(int start, int quantity)
        {
            return new List<Place>();
        }

        public IEnumerable<Place> getPlaceFilter(IList placeClass, IList region, IList town, IList Class)
        {
            return new List<Place>();
        }

        public Place getPlaceId(string id)
        {
            return new Place();
        }

        public Weather getWeatherByLocation(string location)
        {
            return new Weather();
        }
    }
}
