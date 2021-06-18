using CatalogService.Api.Core.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogService.Api.Infrastructure.Context
{
    public class CatalogContextSeed
    {
        public async Task SeedAsync(CatalogContext context, IWebHostEnvironment env, ILogger<CatalogContextSeed> logger)
        {
            var policy = Policy.Handle<SqlException>().
                WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                    onRetry: (exception, timeSpan, retry, ctx) =>
                    {
                        logger.LogWarning(exception, "[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}", nameof(logger), exception.GetType().Name, exception.Message, retry, 3);
                    }
                );


            var setupDirPath = Path.Combine(env.ContentRootPath, "Infrastructure", "Setup", "SeedFiles");
            var picturePath =  "Pics";

            await policy.ExecuteAsync(() => ProcessSeeding(context, setupDirPath, picturePath, logger));
        }


        private async Task ProcessSeeding(CatalogContext context, string setupDirPath, string picturePath, ILogger logger)
        {
            if (!context.CatalogBrands.Any())
            {
                await context.CatalogBrands.AddRangeAsync(GetCatalogBrandsFromFile(setupDirPath));

                await context.SaveChangesAsync();
            }

            if (!context.CatalogTypes.Any())
            {
                await context.CatalogTypes.AddRangeAsync(GetCatalogTypesFromFile(setupDirPath));

                await context.SaveChangesAsync();
            }

            if (!context.CatalogItems.Any())
            {
                await context.CatalogItems.AddRangeAsync(GetCatalogItemsFromFile(setupDirPath, context));

                await context.SaveChangesAsync();

                GetCatalogItemPictures(setupDirPath, picturePath);
            }
        }

        private IEnumerable<CatalogBrand> GetCatalogBrandsFromFile(string contentPath)
        {
            IEnumerable<CatalogBrand> GetPreconfiguredCatalogBrands()
            {
                return new List<CatalogBrand>()
                {
                    new CatalogBrand() { Brand = "Azure"},
                    new CatalogBrand() { Brand = ".NET" },
                    new CatalogBrand() { Brand = "Visual Studio" },
                    new CatalogBrand() { Brand = "SQL Server" },
                    new CatalogBrand() { Brand = "Other" }
                };
            }

            string fileName = Path.Combine(contentPath, "BrandsTextFile.txt");

            if (!File.Exists(fileName))
            {
                return GetPreconfiguredCatalogBrands();
            }

            var fileContent = File.ReadAllLines(fileName);

            var list = fileContent.Select(i => new CatalogBrand()
            {
                Brand = i.Trim('"').Trim()
            }).Where(i => i != null);

            return list ?? GetPreconfiguredCatalogBrands();
        }

        private IEnumerable<CatalogType> GetCatalogTypesFromFile(string contentPath)
        {
            IEnumerable<CatalogType> GetPreconfiguredCatalogTypes()
            {
                return new List<CatalogType>()
                {
                    new CatalogType() { Type = "Mug"},
                    new CatalogType() { Type = "T-Shirt" },
                    new CatalogType() { Type = "Sheet" },
                    new CatalogType() { Type = "USB Memory Stick" }
                };
            }

            string fileName = Path.Combine(contentPath, "CatalogTypes.txt");

            if (!File.Exists(fileName))
            {
                return GetPreconfiguredCatalogTypes();
            }

            var fileContent = File.ReadAllLines(fileName);

            var list = fileContent.Select(i => new CatalogType()
            {
                Type = i.Trim('"').Trim()
            }).Where(i => i != null);

            return list ?? GetPreconfiguredCatalogTypes();
        }

        private IEnumerable<CatalogItem> GetCatalogItemsFromFile(string contentPath, CatalogContext context)
        {
            IEnumerable<CatalogItem> GetPreconfiguredItems()
            {
                return new List<CatalogItem>()
                {
                    new CatalogItem { CatalogTypeId = 2, CatalogBrandId = 2, AvailableStock = 100, Description = ".NET Bot Black Hoodie", Name = ".NET Bot Black Hoodie", Price = 19.5M, PictureFileName = "1.png" },
                    new CatalogItem { CatalogTypeId = 1, CatalogBrandId = 2, AvailableStock = 100, Description = ".NET Black & White Mug", Name = ".NET Black & White Mug", Price= 8.50M, PictureFileName = "2.png" },
                    new CatalogItem { CatalogTypeId = 2, CatalogBrandId = 5, AvailableStock = 100, Description = "Prism White T-Shirt", Name = "Prism White T-Shirt", Price = 12, PictureFileName = "3.png" },
                    new CatalogItem { CatalogTypeId = 2, CatalogBrandId = 2, AvailableStock = 100, Description = ".NET Foundation T-shirt", Name = ".NET Foundation T-shirt", Price = 12, PictureFileName = "4.png" },
                    new CatalogItem { CatalogTypeId = 3, CatalogBrandId = 5, AvailableStock = 100, Description = "Roslyn Red Sheet", Name = "Roslyn Red Sheet", Price = 8.5M, PictureFileName = "5.png" },
                    new CatalogItem { CatalogTypeId = 2, CatalogBrandId = 2, AvailableStock = 100, Description = ".NET Blue Hoodie", Name = ".NET Blue Hoodie", Price = 12, PictureFileName = "6.png" },
                    new CatalogItem { CatalogTypeId = 2, CatalogBrandId = 5, AvailableStock = 100, Description = "Roslyn Red T-Shirt", Name = "Roslyn Red T-Shirt", Price = 12, PictureFileName = "7.png" },
                    new CatalogItem { CatalogTypeId = 2, CatalogBrandId = 5, AvailableStock = 100, Description = "Kudu Purple Hoodie", Name = "Kudu Purple Hoodie", Price = 8.5M, PictureFileName = "8.png" },
                    new CatalogItem { CatalogTypeId = 1, CatalogBrandId = 5, AvailableStock = 100, Description = "Cup<T> White Mug", Name = "Cup<T> White Mug", Price = 12, PictureFileName = "9.png" },
                    new CatalogItem { CatalogTypeId = 3, CatalogBrandId = 2, AvailableStock = 100, Description = ".NET Foundation Sheet", Name = ".NET Foundation Sheet", Price = 12, PictureFileName = "10.png" },
                    new CatalogItem { CatalogTypeId = 3, CatalogBrandId = 2, AvailableStock = 100, Description = "Cup<T> Sheet", Name = "Cup<T> Sheet", Price = 8.5M, PictureFileName = "11.png" },
                    new CatalogItem { CatalogTypeId = 2, CatalogBrandId = 5, AvailableStock = 100, Description = "Prism White TShirt", Name = "Prism White TShirt", Price = 12, PictureFileName = "12.png" },
                };
            }

            string fileName = Path.Combine(contentPath, "CatalogItems.txt");

            if (!File.Exists(fileName))
            {
                return GetPreconfiguredItems();
            }

            var catalogTypeIdLookup = context.CatalogTypes.ToDictionary(ct => ct.Type, ct => ct.Id);
            var catalogBrandIdLookup = context.CatalogBrands.ToDictionary(ct => ct.Brand, ct => ct.Id);


            var fileContent = File.ReadAllLines(fileName)
                        .Skip(1) // skip header row
                        .Select(i => i.Split(','))
                        .Select(i => new CatalogItem()
                        {
                            CatalogTypeId = catalogTypeIdLookup[i[0].Trim()],
                            CatalogBrandId = catalogBrandIdLookup[i[1].Trim()],
                            Description = i[2].Trim('"').Trim(),
                            Name = i[3].Trim('"').Trim(),
                            Price = Decimal.Parse(i[4].Trim('"').Trim(), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture),
                            PictureFileName = i[5].Trim('"').Trim(),
                            AvailableStock = string.IsNullOrEmpty(i[6].Trim()) ? 0 : int.Parse(i[6].Trim()),
                            OnReorder = Convert.ToBoolean(i[7].Trim())
                        });

            return fileContent;
        }

        private void GetCatalogItemPictures(string contentPath, string picturePath)
        {
            picturePath ??= "pics";

            if (picturePath != null)
            {
                DirectoryInfo directory = new DirectoryInfo(picturePath);
                foreach (FileInfo file in directory.GetFiles())
                {
                    file.Delete();
                }

                string zipFileCatalogItemPictures = Path.Combine(contentPath, "CatalogItems.zip");
                ZipFile.ExtractToDirectory(zipFileCatalogItemPictures, picturePath);
            }
        }
    }
}
