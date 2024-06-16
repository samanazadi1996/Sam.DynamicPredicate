# Dynamic Filtering

In software development, one of the critical needs is filtering data based on various and dynamic conditions. The Sam.DynamicPredicate package provides developers with a powerful tool for this purpose. This article explores the Sam.DynamicPredicate package, how to use it, and the benefits it offers for filtering data using LINQ.

## Introduction to the Sam.DynamicPredicate Package

The Sam.DynamicPredicate package allows you to create LINQ queries dynamically based on predicate strings. By leveraging .NET's Reflection and Expression Trees, this package converts filter conditions into executable LINQ expressions.

### How to Use the Sam.DynamicPredicate Package

To use this package, first add it to your project. You can then use the Compile method to convert predicate strings into LINQ expressions. Here is an example of how to use this package

``` c#
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

        return new
        {
            SqlQuery = sqlQuery,
            Data= await query.ToListAsync()
        };
    }
}
```
#### The value of the sqlQuery variable in this example is as follows

```sql
SELECT [p].[Id], [p].[BarCode], [p].[Name], [p].[Price]
FROM [Products] AS [p]
WHERE ([p].[Name] LIKE N'Pro%' AND [p].[Price] >= 150000.0E0) OR [p].[Id] = CAST(1 AS bigint) OR [p].[Price] = 105000.0E0
```

Note that the where method, which has a string input, is located in the Sam.DynamicPredicate
namespace.
## [A guide to making string filters](./Documents/MakingFiltersGuide.md)
## Benefits of Using the Sam.DynamicPredicate Package

1. #### Dynamic and Adjustable Filters
    - One of the biggest advantages of this package is the ability to define dynamic filters based on changing needs. You can input various conditions as strings and easily apply the filters.

2. #### Reduced Code Complexity
   - Using this package, your code becomes cleaner and more manageable. There is no need to write complex queries for every condition, and you can use a uniform structure for all conditions.

3. #### Improved Readability and Maintenance
   - Utilizing predicate strings instead of combining multiple LINQ expressions enhances code readability. This approach simplifies maintenance and makes it easy to modify filter conditions.

4. #### High Flexibility
    - The Sam.DynamicPredicate package supports various types of conditions, including simple comparisons, text comparisons, and more complex conditions using logical operators.

5. ### High Performance
   - With this package, the generated queries are optimized and directly translated into equivalent SQL statements, ensuring high performance during execution.

## Conclusion

The Sam.DynamicPredicate package is a powerful and practical tool for dynamic filtering in LINQ. By providing capabilities to convert predicate strings into LINQ expressions, this package helps developers create queries easily and with high flexibility. Using this package, you can have cleaner, more readable, and maintainable code, improving the performance of your applications.

Using the Sam.DynamicPredicate package allows you to quickly and dynamically respond to new filter conditions without rewriting code, thereby increasing your productivity.
