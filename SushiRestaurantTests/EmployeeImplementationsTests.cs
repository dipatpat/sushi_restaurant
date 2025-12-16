using NUnit.Framework;
using SushiRestaurant;

namespace sushi_restaurant_tests
{
    [TestFixture]
    public class EmployeeImplementationsTests
    {
        private Address _defaultAddress;

        [SetUp]
        public void Setup() => _defaultAddress = new Address("St", "1", "123456", "City");

        [Test]
        public void FullTime_Employee_Should_Store_VacationDays()
        {
            // Arrange: Create FullTime Waiter
            var employee = new Employee(
                "Alice", "Full", _defaultAddress, "IBAN", "555", 1500m,
                role: EmployeeRole.Waiter,
                type: EmploymentType.FullTime,
                vacationDays: 10,
                isOnSickLeave: false
            );

            // Assert
            Assert.That(employee.Type, Is.EqualTo(EmploymentType.FullTime));
            Assert.That(employee.VacationDays, Is.EqualTo(10));
            Assert.That(employee.IsOnSickLeave, Is.False);
        }

        [Test]
        public void PartTime_Employee_Should_Store_HoursInContract()
        {
            // Arrange: Create PartTime Cleaner
            var employee = new Employee(
                "Bob", "Part", _defaultAddress, "IBAN", "555", 1000m,
                role: EmployeeRole.Cleaner,
                type: EmploymentType.PartTime,
                hoursInContract: 25.5
            );

            // Assert
            Assert.That(employee.Type, Is.EqualTo(EmploymentType.PartTime));
            Assert.That(employee.HoursInContract, Is.EqualTo(25.5));
        }

        [Test]
        public void Accessing_FullTime_Properties_On_PartTime_Throws()
        {
            var employee = new Employee(
                "Test", "User", _defaultAddress, "IBAN", "555", 1000m,
                EmployeeRole.Waiter, EmploymentType.PartTime, hoursInContract: 20
            );

            Assert.Throws<InvalidOperationException>(() => { var x = employee.VacationDays; });
        }
    }
}