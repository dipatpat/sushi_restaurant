using NUnit.Framework;
using SushiRestaurant;

namespace sushi_restaurant_tests
{
    [TestFixture]
    public class EmployeeImplementationsTests
    {
        [Test]
        public void FullTimeWaiter_Should_Store_VacationDays_And_SickLeave()
        {
            var waiter = new FullTimeWaiter
            {
                VacationDays = 10,
                IsOnSickLeave = false,
                BaseSalary = 1500m,
                Tips = 200m
            };

            var totalSalary = waiter.Salary;

            Assert.That(waiter.VacationDays, Is.EqualTo(10));
            Assert.That(waiter.IsOnSickLeave, Is.False);
            Assert.That(totalSalary, Is.EqualTo(1700m), "FullTimeWaiter salary should equal base + tips");
        }

        [Test]
        public void FullTimeCook_Should_Store_VacationDays_And_SickLeave()
        {
            var cook = new FullTimeCook
            {
                VacationDays = 12,
                IsOnSickLeave = false,
                BaseSalary = 2000m,
                Bonus = 200m
            };

            var totalSalary = cook.Salary;

            Assert.That(cook.VacationDays, Is.EqualTo(12));
            Assert.That(cook.IsOnSickLeave, Is.False);
            Assert.That(totalSalary, Is.EqualTo(2200m), "FullTimeCook salary should equal base + bonus");
        }

        [Test]
        public void FullTimeCleaner_Should_Store_VacationDays_And_SickLeave()
        {
            var cleaner = new FullTimeCleaner
            {
                VacationDays = 15,
                IsOnSickLeave = false,
                BaseSalary = 1200m
            };

            var totalSalary = cleaner.Salary;

            Assert.That(cleaner.VacationDays, Is.EqualTo(15));
            Assert.That(cleaner.IsOnSickLeave, Is.False);
            Assert.That(totalSalary, Is.EqualTo(1200m), "FullTimeCleaner salary should equal base salary");
        }

        [Test]
        public void PartTimeWaiter_Should_Store_HoursInContract()
        {
            var waiter = new PartTimeWaiter
            {
                HoursInContract = 25.5,
                BaseSalary = 1000m,
                Tips = 150m
            };

            var totalSalary = waiter.Salary;

            Assert.That(waiter.HoursInContract, Is.EqualTo(25.5));
            Assert.That(totalSalary, Is.EqualTo(1150m), "PartTimeWaiter salary should equal base + tips");
        }

        [Test]
        public void PartTimeCleaner_Should_Store_HoursInContract()
        {
            var cleaner = new PartTimeCleaner
            {
                HoursInContract = 15,
                BaseSalary = 1000m
            };

            var totalSalary = cleaner.Salary;

            Assert.That(cleaner.HoursInContract, Is.EqualTo(15));
            Assert.That(totalSalary, Is.EqualTo(1000m), "PartTimeCleaner salary should equal base salary");
        }
    }
}
