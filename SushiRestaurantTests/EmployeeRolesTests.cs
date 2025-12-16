using SushiRestaurant;

namespace sushi_restaurant_tests
{
    [TestFixture]
    public class EmployeeRolesTests
    {
        private Address _defaultAddress;

        [SetUp]
        public void Setup()
        {
            _defaultAddress = new Address("Main", "1", "00-000", "City");
        }

        [Test]
        public void Waiter_Should_Calculate_Salary_As_Base_Plus_Tips()
        {
            // Arrange
            var waiter = new Employee(
                "John", "Doe", _defaultAddress, "IBAN123", "555-123", 
                baseSalary: 1600m,
                role: EmployeeRole.Waiter,
                type: EmploymentType.FullTime, // FullTime works with default args (vacation=0)
                tips: 300m
            );

            // Act
            var totalSalary = waiter.Salary;

            // Assert
            Assert.That(totalSalary, Is.EqualTo(1900m), "Waiter salary should equal base + tips");
            Assert.That(waiter.Role, Is.EqualTo(EmployeeRole.Waiter));
        }

        [Test]
        public void Waiter_Should_Store_SpokenLanguages()
        {
            // Arrange
            var languages = new List<string> { "Japanese", "English", "Spanish" };
            
            var waiter = new Employee(
                "Jane", "Doe", _defaultAddress, "IBAN123", "555-123", 1600m,
                EmployeeRole.Waiter,
                EmploymentType.PartTime,
                // FIX: Added required hours for PartTime
                hoursInContract: 20, 
                spokenLanguages: languages
            );

            // Assert
            Assert.That(waiter.SpokenLanguages.Count, Is.EqualTo(3));
            Assert.That(waiter.SpokenLanguages, Does.Contain("Japanese"));
        }

        [Test]
        public void Cleaner_Should_Store_AssignedArea_And_Shift()
        {
            // Arrange
            var cleaner = new Employee(
                "Clean", "Master", _defaultAddress, "IBAN999", "555-999", 1600m,
                EmployeeRole.Cleaner,
                EmploymentType.FullTime,
                cleaningShift: "Evening",
                assignedArea: "Dining Hall"
            );

            // Assert
            Assert.That(cleaner.AssignedArea, Is.EqualTo("Dining Hall"));
            Assert.That(cleaner.CleaningShift, Is.EqualTo("Evening"));
            Assert.That(cleaner.Role, Is.EqualTo(EmployeeRole.Cleaner));
        }
        
        [Test]
        public void Accessing_Waiter_Properties_On_Cleaner_Throws()
        {
            var cleaner = new Employee(
                "Test", "User", _defaultAddress, "IBAN", "555", 2000m,
                EmployeeRole.Cleaner, EmploymentType.FullTime,
                cleaningShift: "Day", assignedArea: "Kitchen"
            );

            Assert.Throws<InvalidOperationException>(() => { var x = cleaner.Tips; });
            Assert.Throws<InvalidOperationException>(() => cleaner.AddLanguage("English"));
        }

        // [Test]
        // public void Manager_Should_Apply_SeniorityLevel_Multiplier()
        // {
        //     var seniorManager = new Manager
        //     {
        //         BaseSalary = 2000m,
        //         SeniorityLevel = SeniorityLevel.Senior
        //     };

        //     var juniorManager = new Manager
        //     {
        //         BaseSalary = 2000m,
        //         SeniorityLevel = SeniorityLevel.Junior
        //     };

        //     var seniorSalary = seniorManager.Salary;
        //     var juniorSalary = juniorManager.Salary;

        //     Assert.That(seniorSalary, Is.EqualTo(3000m), "Senior manager salary should be base * 1.5");
        //     Assert.That(juniorSalary, Is.EqualTo(2400m), "Junior manager salary should be base * 1.2");
        // }

        // [Test]
        // public void Cook_Should_Add_Bonus_To_BaseSalary()
        // {
        //     var cook = new Cook
        //     {
        //         BaseSalary = 1800m,
        //         Bonus = 200m
        //     };

        //     var totalSalary = cook.Salary;

        //     Assert.That(totalSalary, Is.EqualTo(2000m), "Cook salary should equal base + bonus");
        // }

        // [Test]
        // public void Cook_Should_Store_Specialization()
        // {
        //     var cook = new Cook
        //     {
        //         Specialization = "Sushi"
        //     };

        //     Assert.That(cook.Specialization, Is.EqualTo("Sushi"));
        // }
    }
}
