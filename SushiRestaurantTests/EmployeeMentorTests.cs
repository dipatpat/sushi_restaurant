using NUnit.Framework;
using SushiRestaurant;
// using SushiRestaurant.Models; // Namespace likely removed or merged

namespace SushiRestaurantTests
{
    [TestFixture]
    public class EmployeeMentorTests
    {
        private Address _addr;

        [SetUp]
        public void Setup()
        {
            // Reset extents before each test to prevent data pollution
            Employee.ClearExtent();
            _addr = new Address("Main St", "10", "123456", "City");
        }

        // Helper: Creates an Employee with Role = Waiter
        private Employee CreateWaiter(string name)
        {
            return new Employee(
                firstName: name, 
                lastName: "Test", 
                address: _addr, 
                bankAccount: "BA", 
                phoneNumber: "555", 
                baseSalary: 3000m, 
                role: EmployeeRole.Waiter, 
                type: EmploymentType.FullTime
            );
        }

        // Helper: Creates an Employee with Role = Cook
        // (Even though Cook logic isn't fully implemented, the Enum exists for validation)
        private Employee CreateCook(string name)
        {
            return new Employee(
                firstName: name, 
                lastName: "Test", 
                address: _addr, 
                bankAccount: "BA", 
                phoneNumber: "555", 
                baseSalary: 3000m, 
                role: EmployeeRole.Cook, 
                type: EmploymentType.FullTime
            );
        }

        [Test]
        public void Employee_Should_Assign_Mentor_Of_Same_Type()
        {
            // Arrange
            var alice = CreateWaiter("Alice");
            var bob = CreateWaiter("Bob");

            // Act
            bob.AssignMentor(alice);

            // Assert
            Assert.That(bob.Mentor, Is.EqualTo(alice));
            Assert.That(alice.Mentees.Contains(bob), Is.True);
        }

        [Test]
        public void Employee_Cannot_Have_Mentor_Of_Different_Type()
        {
            // Arrange
            var waiter = CreateWaiter("Alice");
            var cook = CreateCook("Charlie");

            // Act & Assert
            // This fails because waiter.Role (Waiter) != cook.Role (Cook)
            var ex = Assert.Throws<InvalidOperationException>(() =>
            {
                waiter.AssignMentor(cook);
            });
            
            Assert.That(ex.Message, Does.Contain("same role"));
        }

        [Test]
        public void Assigning_New_Mentor_Should_Remove_From_Old_Mentor()
        {
            var m1 = CreateWaiter("Mentor1");
            var m2 = CreateWaiter("Mentor2");
            var e = CreateWaiter("Employee");

            e.AssignMentor(m1);
            e.AssignMentor(m2); 

            Assert.That(e.Mentor, Is.EqualTo(m2));
            Assert.That(m1.Mentees.Contains(e), Is.False);
            Assert.That(m2.Mentees.Contains(e), Is.True);
        }

        [Test]
        public void RemoveMentor_Should_Update_Both_Sides()
        {
            var mentor = CreateWaiter("Mentor");
            var employee = CreateWaiter("Employee");

            employee.AssignMentor(mentor);
            employee.RemoveMentor();

            Assert.That(employee.Mentor, Is.Null);
            Assert.That(mentor.Mentees.Contains(employee), Is.False);
        }

        [Test]
        public void RemoveMentor_When_None_Assigned_Should_Not_Throw()
        {
            var employee = CreateWaiter("Employee");

            Assert.DoesNotThrow(() => employee.RemoveMentor());
        }

        [Test]
        public void Mentor_Should_Have_Multiple_Mentees()
        {
            var mentor = CreateWaiter("Mentor");
            var e1 = CreateWaiter("E1");
            var e2 = CreateWaiter("E2");

            e1.AssignMentor(mentor);
            e2.AssignMentor(mentor);

            Assert.That(mentor.Mentees.Count, Is.EqualTo(2));
            Assert.That(mentor.Mentees.Contains(e1), Is.True);
            Assert.That(mentor.Mentees.Contains(e2), Is.True);
        }

        [Test]
        public void Assigning_Null_Mentor_Should_Throw()
        {
            var e = CreateWaiter("E");

            Assert.Throws<ArgumentNullException>(() => e.AssignMentor(null!));
        }
    }
}