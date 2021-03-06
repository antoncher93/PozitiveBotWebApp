using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Positive.SqlDbContext.Repos;
using Pozitive.Entities;
using Pozitive.Entities.Repos;
using Unity.AspNet.Mvc;

namespace Positive.SqlDbContext
{
    public static class PozitiveContextExtensions
    {
        public static IServiceCollection AddPozitiveSqlServer(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<PozitiveSqlContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<UserRepos>();
            services.AddScoped<DocumentRepos>();
            return services;
        }


    }
}
