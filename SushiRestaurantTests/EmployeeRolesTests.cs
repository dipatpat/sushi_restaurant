using SushiRestaurant;

namespace sushi_restaurant_tests
{
    [TestFixture]
    public class EmployeeRolesTests
    {
        [Test]
        public void Waiter_Should_Calculate_Salary_As_Base_Plus_Tips()
        {
            var waiter = new Waiter
            {
                BaseSalary = 1600m,
                Tips = 300m
            };

            var totalSalary = waiter.Salary;

            Assert.That(totalSalary, Is.EqualTo(1900m), "Waiter salary should equal base + tips");
        }

        [Test]
        public void Waiter_Should_Store_SpokenLanguages()
        {
            var waiter = new Waiter();

            waiter.AddLanguage("Japanese");
            waiter.AddLanguage("English");
            waiter.AddLanguage("Spanish");

            Assert.That(waiter.SpokenLanguages.Count, Is.EqualTo(3));
            Assert.That(waiter.SpokenLanguages, Does.Contain("Japanese"));
        }

        [Test]
        public void Manager_Should_Apply_SeniorityLevel_Multiplier()
        {
            var seniorManager = new Manager
            {
                BaseSalary = 2000m,
                SeniorityLevel = SeniorityLevel.Senior
            };

            var juniorManager = new Manager
            {
                BaseSalary = 2000m,
                SeniorityLevel = SeniorityLevel.Junior
            };

            var seniorSalary = seniorManager.Salary;
            var juniorSalary = juniorManager.Salary;

            Assert.That(seniorSalary, Is.EqualTo(3000m), "Senior manager salary should be base * 1.5");
            Assert.That(juniorSalary, Is.EqualTo(2400m), "Junior manager salary should be base * 1.2");
        }

        [Test]
        public void Cook_Should_Add_Bonus_To_BaseSalary()
        {
            var cook = new Cook
            {
                BaseSalary = 1800m,
                Bonus = 200m
            };

            var totalSalary = cook.Salary;

            Assert.That(totalSalary, Is.EqualTo(2000m), "Cook salary should equal base + bonus");
        }

        [Test]
        public void Cook_Should_Store_Specialization()
        {
            var cook = new Cook
            {
                Specialization = "Sushi"
            };

            Assert.That(cook.Specialization, Is.EqualTo("Sushi"));
        }

        [Test]
        public void Cleaner_Should_Store_AssignedArea_And_Shift()
        {
            var cleaner = new Cleaner
            {
                AssignedArea = "Dining Hall",
                CleaningShift = "Evening"
            };

            Assert.That(cleaner.AssignedArea, Is.EqualTo("Dining Hall"));
            Assert.That(cleaner.CleaningShift, Is.EqualTo("Evening"));
        }
    }
}
