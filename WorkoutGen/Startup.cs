using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using WorkoutGen.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WorkoutGen.Models;
using System;
using Microsoft.AspNetCore.Mvc;
using WorkoutGen.Data.Services.MuscleGroup;
using WorkoutGen.Data.Services.Exercise;
using WorkoutGen.Data.Services.Equipment;
using WorkoutGen.Data.Services.UserWorkout;
using WorkoutGen.Data.Services.UserSet;
using WorkoutGen.Data.Services.UserExercise;
using Microsoft.AspNetCore.Identity;

namespace WorkoutGen
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromMinutes(60);
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });

            services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedAccount = false;
                options.User.RequireUniqueEmail = true;

            }).AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));            

            //services.ConfigureApplicationCookie(options =>
            //{
            //    options.Cookie.HttpOnly = true;
            //    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            //    options.LoginPath = "/Account/Login";
            //    options.SlidingExpiration = true;
            //});

            services.AddScoped<IMuscleGroupService, MuscleGroupService>();
            services.AddScoped<IEquipmentService, EquipmentService>();
            services.AddScoped<IExerciseService, ExerciseService>();
            services.AddScoped<IUserWorkoutService, UserWorkoutService>();
            services.AddScoped<IUserSetService, UserSetService>();
            services.AddScoped<IUserExerciseService, UserExerciseService>();

            services.ConfigureApplicationCookie(options => options.LoginPath = "/Account/Login");

            services.AddRazorPages();

            services.AddMvc().AddRazorPagesOptions(o =>
            {
                o.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
