using System.Collections.Generic;

using Newtonsoft.Json;

namespace ImageBot.Models
{
    // LUIS classes
    public class GetCityWeather
    {
        public double score { get; set; }
    }

    public class None
    {
        public double score { get; set; }
    }

    public class Intents
    {
        public GetCityWeather GetCityWeather { get; set; }
        public None None { get; set; }
    }

    public class GeographyV2
    {
        public string value { get; set; }
        public string type { get; set; }
        public string text { get; set; }
        public int startIndex { get; set; }
        public int length { get; set; }
        public int modelTypeId { get; set; }
        public string modelType { get; set; }
        public List<string> recognitionSources { get; set; }
    }

    public class Instance
    {
        public List<GeographyV2> geographyV2 { get; set; }
    }

    public class Entities
    {
        public List<GeographyV2> geographyV2 { get; set; }

        [JsonProperty("$instance")]
        public Instance Instance { get; set; }
    }

    public class Prediction
    {
        public string topIntent { get; set; }
        public Intents intents { get; set; }
        public Entities entities { get; set; }
    }

    public class LuisRoot
    {
        public string query { get; set; }
        public Prediction prediction { get; set; }
    }

}
