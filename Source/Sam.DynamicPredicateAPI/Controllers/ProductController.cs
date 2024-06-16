using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sam.DynamicPredicate;
using Sam.DynamicPredicateAPI.Contexts;
using Sam.DynamicPredicateAPI.Models;

namespace Sam.DynamicPredicateAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ProductController(ApplicationDbContext applicationDbContext, ILogger<Product> logger) : ControllerBase
{
    [HttpGet]
    public async Task<object> Get()
    {
        var query = applicationDbContext.Products.AsQueryable();

            query = query.Where("Id == 1 || Id == 2 || ( Name StartsWith Pro && Price >= 10000 )");

        var sqlQuery = query.ToQueryString();
        logger.LogInformation("query -> " + sqlQuery);

        // sqlQuery is
        //SELECT [p].[Id], [p].[BarCode], [p].[Name], [p].[Price]\r\nFROM [Products] AS [p]\r\nWHERE [p].[Id] IN (CAST(1 AS bigint), CAST(2 AS bigint)) OR ([p].[Name] LIKE N'Pro%' AND [p].[Price] >= 10000.0E0)

        return new
        {
            SqlQuery = sqlQuery,
            Data= await query.ToListAsync()
        };
    }

    [HttpPost]
    public async Task<long> CreateProduct(CreateProduct model)
    {
        var product = new Product(model.Name, model.Price, model.BarCode);

        await applicationDbContext.Products.AddAsync(product);
        await applicationDbContext.SaveChangesAsync();

        return product.Id;
    }


    //[HttpGet]
    //public async Task<object> Get(string? predicate)
    //{
    //    var query = applicationDbContext.Products.AsQueryable();

    //    if (!string.IsNullOrEmpty(predicate))
    //    {
    //        var func = PredicateBuilder.Compile<Product>(predicate);
    //        query = query.Where(func).AsQueryable();
    //    }

    //    var sqlQuery = query.ToQueryString();
    //    logger.LogInformation("query -> " + sqlQuery);

    //    return new
    //    {
    //        SqlQuery = sqlQuery,
    //        Data = await query.ToListAsync()
    //    };
    //}

}