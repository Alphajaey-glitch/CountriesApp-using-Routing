using System.Text.RegularExpressions;

namespace CountriesRouting.CustomConstraints
{
    public class CountryCodeConstraint : IRouteConstraint
    {
        public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (!values.ContainsKey(routeKey))
            {
                return false;
            }
            Regex regex = new Regex("^[A-Za-z]{2,3}$");

            string countryCode = values[routeKey]?.ToString() ?? "";
            if (regex.IsMatch(countryCode))
            {
                return true;            
            }
            return false;
        }
    }
}
