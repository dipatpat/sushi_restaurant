using NUnit.Framework;
using SushiRestaurant;
using SushiRestaurant.Models;

namespace SushiRestaurantTests
{
    [TestFixture]
    public class EmployeeMentorTests
    {
        private Address _addr;

        [SetUp]
        public void Setup()
        {
            _addr = new Address("Main St", "10", "12345", "City", "5");
        }

        private Waiter CreateWaiter(string name) =>
            new(name, "Test", _addr, "BA", "555", 30, true);

        private Cook CreateCook(string name) =>
            new(name, "Test", _addr, "BA", "555", 30, true);

        [Test]
        public void Employee_Should_Assign_Mentor_Of_Same_Type()
        {
            var alice = CreateWaiter("Alice");
            var bob = CreateWaiter("Bob");

            bob.AssignMentor(alice);

            Assert.That(bob.Mentor, Is.EqualTo(alice));
            Assert.That(alice.Mentees.Contains(bob), Is.True);
        }

        [Test]
        public void Employee_Cannot_Have_Mentor_Of_Different_Type()
        {
            var waiter = CreateWaiter("Alice");
            var cook = CreateCook("Charlie");

            Assert.Throws<InvalidOperationException>(() =>
            {
                waiter.AssignMentor(cook);
            });
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
