# [Dynamic Filtering](../README.md) - A guide to making string filters

In software development, dynamic filtering of data based on varying conditions is a common requirement. The Sam.DynamicPredicate package provides a robust solution to dynamically build LINQ queries from predicate strings. These strings can encompass a variety of conditions, allowing for flexible and powerful data filtering.


## Supported Predicate Conditions

The Sam.DynamicPredicate package supports a wide range of conditions that can be expressed using predicate strings

1.  Equality ('=='):
    - `propertyName == value`

2. Inequality ('!='):
    - `propertyName != value`

3. Greater than ('>'):
    - `propertyName > value`

4. Less than ('<'):
    - `propertyName < value`

5. Greater than or equal to ('>='):
    - `propertyName >= value`

6. Less than or equal to ('<='):
    - `propertyName <= value`

7. Equality ('Equal'):
    - `propertyName Equal value` (equivalent to propertyName == value)

8. Inequality ('NotEqual'):
    - `propertyName NotEqual value` (equivalent to propertyName != value)

9. Contains ('Contains'):
    - `propertyName Contains value`

10. Starts with ('StartsWith'):
    - `propertyName StartsWith value`

11. Ends with ('EndsWith'):
    - `propertyName EndsWith value`


## Usage Example
To utilize these predicate conditions, you can construct a predicate string and compile it into a LINQ expression using the where method. Here’s an example of how you can apply dynamic filtering

```c#
string predicate = "Id == 1 || Id == 2 || (Name StartsWith \"Pro\" && Price >= 10000)";

query.Where(predicate); // type of predicate is string
```

### In this example:

- `predicate` defines a complex condition where `Id` is either 1 or 2, or `Name` starts with `Pro` and `Price` is greater than or equal to 10000.

- `query.Where(predicate)` applies the compiled predicate to the LINQ query `query`, filtering the dataset accordingly.

## Conclusion

The Sam.DynamicPredicate package is an invaluable tool for implementing dynamic filtering capabilities in LINQ queries. By leveraging predicate strings, developers can achieve greater flexibility and maintainability in data filtering operations. This approach not only streamlines development but also enhances application performance and scalability.





