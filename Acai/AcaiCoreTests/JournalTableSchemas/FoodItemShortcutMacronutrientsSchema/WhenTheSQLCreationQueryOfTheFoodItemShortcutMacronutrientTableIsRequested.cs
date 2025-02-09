using NUnit.Framework;

namespace AcaiCoreTests.JournalTableSchemas.FoodItemShortcutMacronutrientsSchema
{
    [TestFixture]
    public class WhenTheSQLCreationQueryOfTheFoodItemShortcutMacronutrientTableIsRequested
    {
        string _result;
        
        [OneTimeSetUp]
        public void Setup()
        {
            var subject = new AcaiCore.FoodItemShortcutMacronutrientsSchema();
            _result = subject.GetSQLTableCreationQuery();
        }

        [Test]
        public void ThenTheCorrectQueryStringIsReturned()
        {
            var expectedCreationQuery = "ALTER TABLE food_item_shortcuts ADD COLUMN protein REAL;" + 
                                        "ALTER TABLE food_item_shortcuts ADD COLUMN carbohydrates REAL;" +
                                        "ALTER TABLE food_item_shortcuts ADD COLUMN fat REAL;" +
                                        "ALTER TABLE food_item_shortcuts ADD COLUMN fibre REAL;" +
                                        "ALTER TABLE food_item_shortcuts ADD COLUMN water REAL;";
            
            Assert.That(_result, Is.EqualTo(expectedCreationQuery));
        }
    }
}
