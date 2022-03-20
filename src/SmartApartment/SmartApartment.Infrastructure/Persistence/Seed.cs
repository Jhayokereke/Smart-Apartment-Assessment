using Microsoft.Extensions.Configuration;
using SmartApartment.Application.Contracts;
using SmartApartment.Domain;
using SmartApartment.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartApartment.Infrastructure.Persistence
{
    public static class Seed
    {
        public static async void SeedData(IManagementRepository managementRepo, IPropertyRepository propertyRepo, IConfiguration config)
        {
            var index = Constants.SmartApartment;
            if (!await managementRepo.IndexExists(index))
            {
                var manPath = config["Seed:Managements.Path"];
                var mangements = File.Exists(manPath) ? JsonSerializer.Deserialize<List<ManagementObject>>(File.ReadAllText(manPath)) : new List<ManagementObject>();
                if(mangements.Any())
                {
                    await managementRepo.CreateIndex(index);
                    await managementRepo.BulkAddAsync(index, mangements);
                }

                var propPath = config["Seed:Properties.Path"];
                var properties = File.Exists(propPath) ? JsonSerializer.Deserialize<List<PropertyObject>>(File.ReadAllText(propPath)) : new List<PropertyObject>();
                if (properties.Any())
                {
                    await propertyRepo.CreateIndex(index);
                    await propertyRepo.BulkAddAsync(index, properties);
                }
            }
        }
    }
}
