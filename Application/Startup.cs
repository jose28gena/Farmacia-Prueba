using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using System.Text;
using WebApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Farmacia.Database.EFCore;
using Microsoft.EntityFrameworkCore;
using WebApi.Services;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }
        public Startup(IConfiguration configuration)
        {

            Configuration = configuration;
        }
        public void ConfigurationServices(IServiceCollection services)
        {// Add services to the container.


            var strConnection = Configuration.GetConnectionString("default");
            services.AddDbContext<FarmaciaContext>(options =>
            options.UseSqlServer(strConnection)
        );


            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IMedicamentoService, MedicamentoService>();
            services.AddScoped<IFormasFarmaceuticasService, FormasFarmaceuticasService>();

            services.AddAutoMapper(typeof(Startup));
            services.AddCors(o => o.AddPolicy("AllowAnyOrigin",
                 builder =>
                 {
                     builder.AllowAnyOrigin()
                                 .AllowAnyMethod()
                                 .AllowAnyHeader();
                 }));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Farmacia Api", Version = "v1" });
            });

            // Configure JWT authentication
            var llave = Configuration["SecretKey"];
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                 Encoding.UTF8.GetBytes(Configuration["SecretKey"])),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
          
        }

    }
}
