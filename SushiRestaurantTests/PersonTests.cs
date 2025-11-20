using System;
using NUnit.Framework;
using SushiRestaurant;

namespace sushi_restaurant_tests
{
    [TestFixture]
    public class PersonTests
    {
        [Test]
        public void Should_Assign_First_And_Last_Name_Correctly()
        {
            var person = new Person
            {
                FirstName = "Hiroshi",
                LastName = "Tanaka"
            };

            Assert.That(person.FirstName, Is.EqualTo("Hiroshi"));
            Assert.That(person.LastName, Is.EqualTo("Tanaka"));
        }

        [Test]
        public void Should_Not_Allow_Empty_LastName()
        {
            var person = new Person
            {
                FirstName = "Mark"
            };

            var ex = Assert.Throws<ArgumentException>(() => person.LastName = string.Empty);

            Assert.That(ex!.ParamName, Is.EqualTo("LastName"));
            Assert.That(ex.Message, Does.Contain("Last name is required."));
        }
    }
}