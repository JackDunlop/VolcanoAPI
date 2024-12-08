namespace VolcanoAPI.Data
{
    public class VolcanoData
    {
        public int id { get; set; }
        public string name { get; set; }
        public string country { get; set; }
        public string region { get; set; }
        public string subregion { get; set; }
        public string last_eruption { get; set; }
        public int summit { get; set; }
        public int elevation { get; set; }
        public int population_5km { get; set; }
        public int population_10km { get; set; }
        public int population_30km { get; set; }
        public int population_100km { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
    }
}
