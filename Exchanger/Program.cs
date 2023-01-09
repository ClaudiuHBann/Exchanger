﻿using Microsoft.EntityFrameworkCore;

using Exchanger.Data;
using Exchanger.Services;

namespace Exchanger
{
    public class Program
    {
        public static void Main(string[] args)
        {
            InitializeAsync(args);
            while (true) Thread.Sleep(int.MaxValue);
        }

        static async void InitializeAsync(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<ExchangerContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("ExchangerContext") ?? throw new InvalidOperationException("Connection string 'ExchangerContext' not found.")));

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);
            });

            await Seeder.Initialize(builder.Services.BuildServiceProvider());

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
                );

            app.UseSession();

            await app.RunAsync();
        }
    }
}

/*
    TODO:
         - seed with random photos (people & items)
         - test everything
         - clean up the code

         - make the home search button more rounder with smaller border or none
         - home user unknown doesn't work
         - offer offer button more rounder
         - offer image list no margin bottom
         - home use theme for the pagination
         - center home pagination
         - 
         - 
         - 
         - 
         - 
         - 
         - viewdata constants with an initializator
*/