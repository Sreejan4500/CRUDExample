using ServiceContracts;
using Services;

namespace CRUDExample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();

            // Add Services to IoC container
            builder.Services.AddSingleton<ICountryService, CountryService>(serviceProvider => new CountryService(initializeCountries: true));
            builder.Services.AddSingleton<IPersonService, PersonService>(serviceProvider => new PersonService(initializePersons: true));

            var app = builder.Build();

            if(builder.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.MapControllers();

            app.Run();
        }
    }
}
