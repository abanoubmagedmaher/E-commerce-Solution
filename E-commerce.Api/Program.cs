using Core.Identity;
using Core.Interfaces;
using Infrastrucure.Data;
using Infrastrucure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            #region Identity DbContext
            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });
            builder.Services.AddIdentityCore<AppUser>(opt =>
            {

            })
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddSignInManager<SignInManager<AppUser>>()
                ;
            builder.Services.AddAuthentication();
            builder.Services.AddAuthorization(); 
            #endregion


            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddCors(opt =>
            {
                opt.AddPolicy("CorePolicy", policy =>
                {
                    policy.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
                });
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseMiddleware<MiddleWare.ExceptionMiddleWare>();
            //app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors("CorePolicy");
            app.UseAuthorization();


            app.MapControllers();

            #region Handel The AutoMigration
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<StoreContext>();
            var identity= services.GetRequiredService<AppIdentityDbContext>();  
            var userManager= services.GetRequiredService<UserManager<AppUser>>();
            var logger = services.GetRequiredService<ILogger<Program>>();
            try
            {
                await  context.Database.MigrateAsync();
                await identity.Database.MigrateAsync();
                await AppIdentityDbContextSeed.SeedUserAsync(userManager);
                await StoreContextSeed.SeedAsync(context);
            }
            catch (Exception ex)
            {

                logger.LogError(ex, "An error occurred during migration");
            }
            #endregion
            await app.RunAsync();
        }
    }
}
