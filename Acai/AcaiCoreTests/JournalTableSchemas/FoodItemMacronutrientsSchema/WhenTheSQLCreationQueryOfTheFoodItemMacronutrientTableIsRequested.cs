using NUnit.Framework;

namespace AcaiCoreTests.JournalTableSchemas.FoodItemMacronutrientsSchema
{
    [TestFixture]
    public class WhenTheSQLCreationQueryOfTheFoodItemMacronutrientTableIsRequested
    {
        string _result;
        
        [OneTimeSetUp]
        public void Setup()
        {
            var subject = new AcaiCore.FoodItemMacronutrientsSchema();
            _result = subject.GetSQLTableCreationQuery();
        }

        [Test]
        public void ThenTheCorrectQueryStringIsReturned()
        {
            var expectedCreationQuery = "ALTER TABLE food_items ADD COLUMN protein REAL;" + 
                                        "ALTER TABLE food_items ADD COLUMN carbohydrates REAL;" +
                                        "ALTER TABLE food_items ADD COLUMN fat REAL;" +
                                        "ALTER TABLE food_items ADD COLUMN fibre REAL;" +
                                        "ALTER TABLE food_items ADD COLUMN water REAL;";
            
            Assert.That(_result, Is.EqualTo(expectedCreationQuery));
        }
    }
}
