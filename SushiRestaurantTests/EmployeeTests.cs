using NUnit.Framework;
using SushiRestaurant;

namespace sushi_restaurant_tests
{
    [TestFixture]
    public class EmployeeTests
    {
        private Address _defaultAddress;

        [SetUp]
        public void Setup()
        {
            _defaultAddress = new Address("Test", "1", "123456", "City");
            Employee.ClearExtent();
        }

        [Test]
        public void Should_Assign_Base_Information_Correctly()
        {
            var emp = new Employee(
                "Aiko", "Tanaka", _defaultAddress, "JP123", "090123", 1800m,
                EmployeeRole.Waiter, EmploymentType.FullTime
            );

            Assert.That(emp.FirstName, Is.EqualTo("Aiko"));
            Assert.That(emp.LastName, Is.EqualTo("Tanaka"));
            Assert.That(emp.BaseSalary, Is.EqualTo(1800m));
        }

        [Test]
        public void Should_Handle_Dynamic_Role_Switch_From_Waiter_To_Cleaner()
        {
            // 1. Create as Waiter
            var emp = new Employee(
                "Ghost", "User", _defaultAddress, "ACC", "000", 2500m,
                EmployeeRole.Waiter, EmploymentType.FullTime, tips: 100m
            );
            
            Assert.That(emp.Role, Is.EqualTo(EmployeeRole.Waiter));
            Assert.That(emp.Tips, Is.EqualTo(100m));

            // 2. Switch to Cleaner dynamically
            emp.ChangeRoleToCleaner("Night Shift", "Lobby");

            // 3. Assert Discriminator Change
            Assert.That(emp.Role, Is.EqualTo(EmployeeRole.Cleaner));
            
            // 4. Assert Data Integrity (New data present, Old data inaccessible)
            Assert.That(emp.CleaningShift, Is.EqualTo("Night Shift"));
            Assert.Throws<InvalidOperationException>(() => { var t = emp.Tips; });
        }

        [Test]
        public void Should_Handle_Dynamic_Type_Switch_From_PartTime_To_FullTime()
        {
            // 1. Create as PartTime
            var emp = new Employee(
                "Student", "Worker", _defaultAddress, "ACC", "000", 2000m,
                EmployeeRole.Waiter, EmploymentType.PartTime, hoursInContract: 20
            );

            Assert.That(emp.Type, Is.EqualTo(EmploymentType.PartTime));

            // 2. Switch to FullTime
            emp.ChangeTypeToFullTime(vacationDays: 20);

            // 3. Assert
            Assert.That(emp.Type, Is.EqualTo(EmploymentType.FullTime));
            Assert.That(emp.VacationDays, Is.EqualTo(20));
            Assert.Throws<InvalidOperationException>(() => { var h = emp.HoursInContract; });
        }
        
        [Test]
        public void AssignMentor_Should_Throw_If_Roles_Different()
        {
            var waiter = new Employee("W", "W", _defaultAddress, "1", "1", 3000m, EmployeeRole.Waiter, EmploymentType.FullTime);
            var cleaner = new Employee("C", "C", _defaultAddress, "1", "1", 3000m, EmployeeRole.Cleaner, EmploymentType.FullTime);

            var ex = Assert.Throws<InvalidOperationException>(() => waiter.AssignMentor(cleaner));
            Assert.That(ex.Message, Does.Contain("same role"));
        }
    }
}
