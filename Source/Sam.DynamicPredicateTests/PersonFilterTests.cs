using Sam.DynamicPredicate;

namespace Sam.DynamicPredicateTests
{
    public class PersonFilterTests
    {
        // Sample data for testing
        private readonly IQueryable<Person> people = new List<Person>
        {
            new Person { Id=1, Name = "John", Age = 25 },
            new Person { Id=2, Name = "Jane", Age = 10 },
            new Person { Id=3, Name = "Jake", Age = 20 },
            new Person { Id=4, Name = "Jill", Age = 30 }
        }.AsQueryable();

        [Fact]
        public void Test_FilterByComplexCondition()
        {
            // Arrange
            var predicate = PredicateBuilder.Compile<Person>("Name == \"Jake\" && Id >= 2 && (Age == 10 || Age == 20)");

            // Act
            var filteredPeople = people.Where(predicate).ToList();

            // Assert
            Assert.Single(filteredPeople);
            Assert.Equal("Jake", filteredPeople[0].Name);
            Assert.Equal(20, filteredPeople[0].Age);
        }

        [Fact]
        public void Test_FilterByNameStartsWith()
        {
            // Arrange
            var predicate = PredicateBuilder.Compile<Person>("Name StartsWith \"Ja\"");

            // Act
            var filteredPeople = people.Where(predicate).ToList();

            // Assert
            Assert.Equal(2, filteredPeople.Count);
            Assert.Equal("Jane", filteredPeople[0].Name);
            Assert.Equal("Jake", filteredPeople[1].Name);
        }


        [Fact]
        public void Test_FilterByNonExistingName()
        {
            // Arrange
            var predicate = PredicateBuilder.Compile<Person>("Name == \"NonExisting\"");

            // Act
            var filteredPeople = people.Where(predicate).ToList();

            // Assert
            Assert.Empty(filteredPeople);
        }

        [Fact]
        public void Test_FilterByAgeRange()
        {
            // Arrange
            var predicate = PredicateBuilder.Compile<Person>("Age >= 20 && Age <= 30");

            // Act
            var filteredPeople = people.Where(predicate).ToList();

            // Assert
            Assert.Equal(3, filteredPeople.Count);
            Assert.Contains(filteredPeople, p => p.Name == "John");
            Assert.Contains(filteredPeople, p => p.Name == "Jake");
            Assert.Contains(filteredPeople, p => p.Name == "Jill");
        }
    }

    public class Person
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Age { get; set; }
    }
}
