namespace AcaiCore
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Load existing file [1] or create new [2]?");
            int loadSelection = 0;
            while (loadSelection != 1 && loadSelection != 2)
            {
                try
                {
                    loadSelection = int.Parse(Console.ReadLine() ?? string.Empty);
                }
                catch
                {
                    // ignored
                }
            }
            Console.WriteLine("Specify journal path :");
            string journalFilePath = Console.ReadLine();

            ISessionInitializationFacade sessionInitializationFacade = new SessionInitializationFacade(JournalTableSchemas.All, new SqliteConnectionFactory(journalFilePath));

            bool succeeded = false;
            while (succeeded == false)
            {
                if (loadSelection == 1)
                {
                    succeeded = sessionInitializationFacade.InitializeSessionFromExistingJournalFileAtPath(journalFilePath);
                }
                else
                {
                    succeeded = sessionInitializationFacade.InitializeSessionFromNewJournalFileAtPath(journalFilePath);
                }

                if (succeeded == false)
                {
                    switch (sessionInitializationFacade.GetInitializationFailureReason())
                    {
                        case SessionInitializationFailureReason.NONE:
                            Console.WriteLine("Failed to Initialize Session: None");
                            break;
                        case SessionInitializationFailureReason.JOURNAL_FILE_ALREADY_EXISTS:
                            Console.WriteLine("Failed to Initialize Session: Journal File already exists");
                            break;
                        case SessionInitializationFailureReason.JOURNAL_FILE_DOES_NOT_EXIST:
                            Console.WriteLine("Failed to Initialize Session: Journal File does not exist");
                            break;
                        case SessionInitializationFailureReason.JOURNAL_FILE_IS_MISSING_TABLES:
                            Console.WriteLine("Failed to Initialize Session: Missing Table in Journal Files");
                            break;
                    }
                }
            }

            Console.WriteLine("Session Initialized Successfully.");

            Console.WriteLine("Heres your journal for today:");
            var session = sessionInitializationFacade.GetSession();

            string userInput = "";
            while (userInput.ToLower() != "quit")
            {
                var todaysItems = session.GetFoodItemGateway().GetFoodItemsForDate(DateTime.Now);
                foreach (var item in todaysItems)
                {
                    Console.WriteLine($" - {item.GetName()} | {item.GetCalories()} cals | {item.GetCreationDate()}");
                }
                Console.WriteLine($"TOTAL: {todaysItems.Sum(x => x.GetCalories())} cals");
                Console.WriteLine("\n");

                Console.WriteLine("Type new item name to add or type \"quit\" to exit:");
                userInput = Console.ReadLine();
                if (userInput.ToLower() == "quit")
                {
                    continue;
                }
                Console.WriteLine("Write number of calories:");
                float calories = float.Parse(Console.ReadLine());
                session.GetFoodItemGateway().CreateNewFoodItem(userInput, calories, DateTime.Now);
            }
        }
    }
}
