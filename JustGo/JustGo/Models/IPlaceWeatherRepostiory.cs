using System.Collections;

namespace JustGo.Models
{
    public interface IPlaceWeatherRepostiory
    {
        
        public IEnumerable<Place> getPlace(int start,int quantity);
        public IEnumerable<Place> getPlaceFilter(IList placeClass, IList region, IList town, IList Class);
        public Place getPlaceId(string id);
                
        public Weather getWeatherByLocation(string location);
    }
}
