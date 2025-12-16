using NUnit.Framework;
using SushiRestaurant;
using System;

namespace sushi_restaurant_tests
{
    [TestFixture]
    public class ContractTests
    {
        // Helper adjusted to use the Flattened Employee class
        private Employee CreateEmployee(string f = "John", string l = "Doe")
        {
            return new Employee(
                firstName: f, 
                lastName: l,
                address: new Address("Street", "1", "00-001", "City"),
                bankAccount: "BA123", 
                phoneNumber: "123456789",
                baseSalary: 3000m,
                role: EmployeeRole.Waiter,
                type: EmploymentType.FullTime,
                vacationDays: 10,
                isOnSickLeave: false,
                tips: 100m
            );
        }
        
        [Test]
        public void Constructor_Assigns_Employee_And_StartDate_And_Updates_Reverse()
        {
            var emp = CreateEmployee();
            var start = DateTime.Now.AddDays(1);

            var contract = new Contract(emp, start);

            Assert.That(contract.Employee, Is.EqualTo(emp));
            Assert.That(contract.StartDate, Is.EqualTo(start));
            Assert.That(emp.Contracts.Contains(contract), Is.True);
        }

        [Test]
        public void Constructor_Throws_If_Employee_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new Contract(null!, DateTime.Now.AddDays(1)));
        }

        [Test]
        public void Constructor_Throws_If_StartDate_Default()
        {
            var emp = CreateEmployee();

            Assert.Throws<ArgumentException>(() =>
                new Contract(emp, default));
        }
        
        [Test]
        public void Setting_EndDate_Before_StartDate_Should_Throw()
        {
            var emp = CreateEmployee();
            var c = new Contract(emp, DateTime.Now.AddDays(3));

            Assert.Throws<ArgumentException>(() =>
                c.EndDate = DateTime.Now.AddDays(1));
        }

        [Test]
        public void Setting_Valid_EndDate_Should_Work()
        {
            var emp = CreateEmployee();
            var start = DateTime.Now.AddDays(2);

            var c = new Contract(emp, start);
            var end = start.AddDays(2);

            c.EndDate = end;

            Assert.That(c.EndDate, Is.EqualTo(end));
        }

        [Test]
        public void SetEmployee_Changes_Employee_And_Updates_Reverse()
        {
            var emp1 = CreateEmployee("Anna", "Nowak");
            var emp2 = CreateEmployee("John", "Smith");
            
            // Create initial contracts so employees have >1 contract if needed for logic
            var extra = new Contract(emp1, DateTime.Now.AddDays(1));
            var c = new Contract(emp1, DateTime.Now.AddDays(5));

            c.SetEmployee(emp2);

            Assert.That(c.Employee, Is.EqualTo(emp2));
            // emp1 still has 'extra', so contract count logic is satisfied
            Assert.That(emp1.Contracts.Contains(c), Is.False);
            Assert.That(emp2.Contracts.Contains(c), Is.True);
        }

        [Test]
        public void SetEmployee_With_Same_Employee_Does_Nothing()
        {
            var emp = CreateEmployee();
            var c = new Contract(emp, DateTime.Now.AddDays(5));

            c.SetEmployee(emp);

            Assert.That(c.Employee, Is.EqualTo(emp));
            Assert.That(emp.Contracts.Count, Is.EqualTo(1));
        }

        [Test]
        public void SetEmployee_Throws_If_NewEmployee_Is_Null()
        {
            var emp = CreateEmployee();
            var c = new Contract(emp, DateTime.Now.AddDays(5));

            Assert.Throws<ArgumentNullException>(() => c.SetEmployee(null!));
        }
        
        [Test]
        public void Contract_Always_Belongs_To_Exactly_One_Employee()
        {
            var emp = CreateEmployee();
            var c = new Contract(emp, DateTime.Now.AddDays(3));

            Assert.That(c.Employee, Is.Not.Null);
            Assert.That(emp.Contracts.Contains(c), Is.True);
        }

        [Test]
        public void Employee_Cannot_End_Up_With_No_Contracts()
        {
            var emp = CreateEmployee();

            var c1 = new Contract(emp, DateTime.Now.AddDays(3));
            var c2 = new Contract(emp, DateTime.Now.AddDays(10));

            // Logic: Removing c2 is fine because c1 remains
            emp.RemoveContract(c2);

            Assert.That(emp.Contracts.Count, Is.EqualTo(1));

            // Logic: Removing the last contract (c1) should throw
            Assert.Throws<InvalidOperationException>(() =>
                emp.RemoveContract(c1));
        }
        
        [Test]
        public void Contract_Cannot_Be_Assigned_To_Two_Employees_Without_ChangeEmployee()
        {
            var emp1 = CreateEmployee("Anna", "Nowak");
            var emp2 = CreateEmployee("Mark", "Jones");
            
            var c1 = new Contract(emp1, DateTime.Now.AddDays(3));
            var c2 = new Contract(emp1, DateTime.Now.AddDays(5));

            // Move c2 to emp2
            c2.SetEmployee(emp2);

            Assert.That(c2.Employee, Is.EqualTo(emp2));
            Assert.That(emp1.Contracts.Contains(c2), Is.False);
            Assert.That(emp2.Contracts.Contains(c2), Is.True);
            
            // emp1 should still have c1
            Assert.That(emp1.Contracts.Count, Is.EqualTo(1));
        }
    }
}