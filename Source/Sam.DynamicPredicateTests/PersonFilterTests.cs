using Sam.DynamicPredicate;

namespace Sam.DynamicPredicateTests
{
    public class PersonFilterTests
    {
        // Sample data for testing
        private readonly IEnumerable<Person> people = new List<Person>
        {
            new Person { Id=1, Name = "John", Age = 25 ,Code="1"},
            new Person { Id=2, Name = "Jane", Age = 10 ,Code="1"},
            new Person { Id=3, Name = "Jake", Age = 20 ,Code="1"},
            new Person { Id=4, Name = "Jill", Age = 30 },
            new Person { Id=5, Name = null, Age = 40 , Code = "1"}
        };

        [Fact]
        public void Test_FilterByComplexCondition()
        {
            // Arrange
            var predicate = "Name == \"Jake\" && Id >= 2 && (Age == 10 || Age == 10000 || Age == 20)";

            // Act
            var filteredPeople = people.Where(predicate).ToList();

            // Assert
            Assert.Single(filteredPeople);
            Assert.Equal("Jake", filteredPeople[0].Name);
            Assert.Equal(20, filteredPeople[0].Age);
        }

        [Fact]
        public void Test_FilterByNonExistingName()
        {
            // Arrange
            var predicate = "Name == \"NonExisting\"";

            // Act
            var filteredPeople = people.Where(predicate).ToList();

            // Assert
            Assert.Empty(filteredPeople);
        }

        [Fact]
        public void Test_FilterByAgeRange()
        {
            // Arrange
            var predicate = "Age >= 20 && Age <= 30";

            // Act
            var filteredPeople = people.Where(predicate).ToList();

            // Assert
            Assert.Equal(3, filteredPeople.Count);
            Assert.Contains(filteredPeople, p => p.Name == "John");
            Assert.Contains(filteredPeople, p => p.Name == "Jake");
            Assert.Contains(filteredPeople, p => p.Name == "Jill");
        }

        [Fact]
        public void Test_FilterByCodeIsNull()
        {
            // Arrange
            var predicate = "Code Is null";

            // Act
            var filteredPeople = people.Where(predicate).ToList();

            // Assert
            Assert.Single(filteredPeople);
            Assert.Contains(filteredPeople, p => p.Name == "Jill");
        }

        [Fact]
        public void Test_FilterByNameIsNull()
        {
            // Arrange
            var predicate = "Name Is null";

            // Act
            var filteredPeople = people.Where(predicate).ToList();

            // Assert
            Assert.Single(filteredPeople);
            Assert.Null(filteredPeople[0].Name);
            Assert.Equal(40, filteredPeople[0].Age);
        }

        [Fact]
        public void Test_FilterByMultipleConditions()
        {
            // Arrange
            var predicate = "Age >= 20 && (Name == \"John\" || Name == \"Jake\")";

            // Act
            var filteredPeople = people.Where(predicate).ToList();

            // Assert
            Assert.Equal(2, filteredPeople.Count);
            Assert.Contains(filteredPeople, p => p.Name == "John");
            Assert.Contains(filteredPeople, p => p.Name == "Jake");
        }

        [Fact]
        public void Test_FilterByGreaterThan()
        {
            // Arrange
            var predicate = "Age > 25";

            // Act
            var filteredPeople = people.Where(predicate).ToList();

            // Assert
            Assert.Equal(2, filteredPeople.Count);
            Assert.Contains(filteredPeople, p => p.Name == "Jill");
            Assert.Contains(filteredPeople, p => p.Age == 40);
        }

        [Fact]
        public void Test_FilterByLessThan()
        {
            // Arrange
            var predicate = "Age < 20";

            // Act
            var filteredPeople = people.Where(predicate).ToList();

            // Assert
            Assert.Single(filteredPeople);
            Assert.Equal("Jane", filteredPeople[0].Name);
        }
    }

    public class Person
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public int Age { get; set; }
    }
}
