namespace CountriesRouting
{
    public static class Countries
    {
        public static Dictionary<string, string> SeedValues()
        {
            Dictionary<string, string> countries = new();

            countries.Add("USA", "United States of America");
            countries.Add("CND", "Canada");
            countries.Add("UK", "United Kingdom");
            countries.Add("IND", "India");
            countries.Add("JPN", "Japan");

            return countries;
        }
        
    }
}
