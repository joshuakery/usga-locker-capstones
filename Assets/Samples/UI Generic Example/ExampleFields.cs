using Newtonsoft.Json;
using AirtableUnity.PX.Model;

namespace JoshKery.GenericUI.Example
{
    public class ExampleFields : BaseField
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Year")]
        public int Year { get; set; }

        [JsonProperty("Carousel Background Media Path")]
        public string CarouselBackgroundMediaPath { get; set; }
    }
}


