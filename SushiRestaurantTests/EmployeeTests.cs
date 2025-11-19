using NUnit.Framework;
using SushiRestaurant;

namespace sushi_restaurant_tests
{
    [TestFixture]
    public class EmployeeTests
    {
        [Test]
        public void Should_Assign_Employee_Basic_Information_Correctly()
        {
            var address = new Address();

            var waiter = new Waiter(
                firstName: "Aiko",
                lastName: "Tanaka",
                address: address,
                bankAccount: "JP1234567890",
                phoneNumber: "09012345678",
                baseSalary: 1800m,
                isFullTime: true
            );

            Assert.That(waiter.FirstName, Is.EqualTo("Aiko"));
            Assert.That(waiter.LastName, Is.EqualTo("Tanaka"));
            Assert.That(waiter.PhoneNumber, Is.EqualTo("09012345678"));
            Assert.That(waiter.BankAccount, Is.EqualTo("JP1234567890"));
            Assert.That(waiter.BaseSalary, Is.EqualTo(1800m));
            Assert.That(waiter.IsFullTime, Is.EqualTo(true));
        }

        [Test]
        public void Should_Assign_And_Read_Address_Information()
        {
            var manager = new Manager
            {
                Address = new Address
                {
                    StreetName = "Sakura Ave",
                    StreetNumber = "22B",
                    CityName = "Tokyo",
                    PostalCode = "100-0001"
                }
            };

            Assert.That(manager.Address.StreetName, Is.EqualTo("Sakura Ave"));
            Assert.That(manager.Address.StreetNumber, Is.EqualTo("22B"));
            Assert.That(manager.Address.CityName, Is.EqualTo("Tokyo"));
            Assert.That(manager.Address.PostalCode, Is.EqualTo("100-0001"));
        }

        [Test]
        public void Waiter_Salary_Should_Include_Tips()
        {
            var waiter = new Waiter
            {
                BaseSalary = 1500m,
                Tips = 200m
            };

            var totalSalary = waiter.Salary;

            Assert.That(totalSalary, Is.EqualTo(1700m), "Waiter salary should equal base + tips");
        }

        [Test]
        public void Should_Handle_FullTime_And_PartTime_Status()
        {
            var address = new Address();

            var cook = new Cook(
                firstName: "Kenji",
                lastName: "Sato",
                address: address,
                bankAccount: "ACC123",
                phoneNumber: "0900000000",
                baseSalary: 1800m,
                isFullTime: false,
                bonus: 150m
            );

            Assert.That(cook.IsFullTime, Is.EqualTo(false), "Cook should be marked as part-time");
            Assert.That(cook.Salary, Is.EqualTo(1950m), "Cook salary should equal base + bonus");
        }
    }
}
