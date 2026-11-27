using CountriesRouting.CustomConstraints;

namespace CountriesRouting
{
    public class Program
    {
        
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddRouting(options =>
            {
                options.ConstraintMap.Add("country", typeof(CountryCodeConstraint));
            });
            var app = builder.Build();
            app.UseRouting();

            Dictionary<string, string> countries = Countries.SeedValues();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/countries", async (httpContext) =>
                {
                    foreach (var key in countries.Keys)
                    {
                        await httpContext.Response.WriteAsync($"{key} => {countries[key]}\n");
                    }
                });

                endpoints.MapGet("/countries/{countrycode:country}", async (httpContext) =>
                {
                    var countryCode = httpContext.Request.RouteValues["countrycode"].ToString().ToUpper() ?? "";
                    
                    if (countries.ContainsKey(countryCode))
                    {
                        await httpContext.Response.WriteAsync($"{countries[countryCode]}");
                    }
                    
                    else
                    {
                        if (httpContext.Response.StatusCode == 200)
                        {
                            httpContext.Response.StatusCode = 400;
                            await httpContext.Response.WriteAsync("[No country]");
                        }
                    }
                });
            });
            app.Run(async httpContext =>
            {
                await httpContext.Response.WriteAsync("No response");
            });

            app.Run();
        }
    }
}
