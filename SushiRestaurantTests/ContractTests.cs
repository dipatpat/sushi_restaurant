using NUnit.Framework;
using SushiRestaurant;
using System;

namespace sushi_restaurant_tests
{
    [TestFixture]
    public class ContractTests
    {
        [Test]
        public void Should_Assign_Employee_To_Contract()
        {
            var emp = new FullTimeWaiter(
                "John", "Doe",
                new Address("Street", "1", "00-001", "City"),
                "BA123", "123456789",
                3000m, vacationDays: 10, isOnSickLeave: false,
                tips: 200m
            );

            var contract = new Contract(DateTime.Now.AddDays(1));
            
            contract.SetEmployee(emp);
            
            Assert.That(contract.Employee, Is.EqualTo(emp));
            Assert.That(emp.Contracts.Contains(contract), Is.True);
        }

        [Test]
        public void Should_Not_Allow_Assigning_Second_Employee_To_Same_Contract()
        {
            var emp1 = new FullTimeCleaner(
                "Alice", "Smith",
                new Address("Street", "2", "00-002", "City"),
                "BA999", "555666777",
                2500m, cleaningShift: "Morning",
                assignedArea: "Hall",
                vacationDays: 5, isOnSickLeave: false
            );

            var emp2 = new FullTimeCleaner(
                "Bob", "Brown",
                new Address("Street", "3", "00-003", "City"),
                "BA888", "111222333",
                2600m, cleaningShift: "Night",
                assignedArea: "Kitchen",
                vacationDays: 8, isOnSickLeave: false
            );

            var contract = new Contract(DateTime.Now.AddDays(2));
            contract.SetEmployee(emp1);
            
            Assert.Throws<InvalidOperationException>(() => contract.SetEmployee(emp2));
        }

        [Test]
        public void Should_Not_Allow_EndDate_Before_StartDate()
        {
            var contract = new Contract(DateTime.Now.AddDays(5));

            Assert.Throws<ArgumentException>(() =>
            {
                contract.EndDate = contract.StartDate.AddDays(-1);
            });
        }

        [Test]
        public void Employee_RemoveContract_Should_Remove_Relationship()
        {
            var emp = new FullTimeCook(
                "Mina", "Tanaka",
                new Address("Main", "10", "11-111", "Tokyo"),
                "BA111", "987654321",
                5000m, bonus: 500m,
                specialization: "Sushi",
                vacationDays: 12, isOnSickLeave: false
            );

            var c1 = new Contract(DateTime.Now.AddDays(10));
            var c2 = new Contract(DateTime.Now.AddDays(20));

            c1.SetEmployee(emp);
            c2.SetEmployee(emp);
            
            emp.RemoveContract(c2);
            
            Assert.That(emp.Contracts.Contains(c2), Is.False);
            Assert.That(c2.Employee, Is.Null);
        }
    }
}
