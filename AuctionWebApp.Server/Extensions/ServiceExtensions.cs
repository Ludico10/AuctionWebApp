namespace AuctionWebApp.Server.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddCors(this IServiceCollection services,IConfiguration configuration)
        {
            //var originsConfig = configuration.GetSection("AllowedOrigins").Get<string>();
            //var origins = originsConfig.Split(',');

            services.AddCors(x => x.AddPolicy("SusloPolicy", opt =>
            {
                opt.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
                //opt.AllowCredentials();
                //opt.WithOrigins(origins);
            }));
        }
    }
}
