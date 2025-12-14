namespace AcaiCore
{
    public class AcaiSession
    {
        private readonly IFoodItemGateway _foodItemGateway;
        private readonly IFoodItemShortcutGateway _foodItemShortcutGateway;
        private readonly IFoodJournalNoteGateway _foodJournalNoteGateway;
        private readonly IWeightJournalGateway _weightJournalGateway;
        
        public AcaiSession(IFoodItemGateway foodItemGateway, IFoodItemShortcutGateway foodItemShortcutGateway, IFoodJournalNoteGateway foodJournalNoteGateway, IWeightJournalGateway weightJournalGateway)
        {
            _foodItemGateway = foodItemGateway;
            _foodItemShortcutGateway = foodItemShortcutGateway;
            _foodJournalNoteGateway = foodJournalNoteGateway;
            _weightJournalGateway = weightJournalGateway;
        }

        public IFoodItemGateway GetFoodItemGateway()
        {
            return _foodItemGateway;
        }

        public IFoodItemShortcutGateway GetFoodItemShortcutGateway()
        {
            return _foodItemShortcutGateway;
        }

        public IFoodJournalNoteGateway GetFoodJournalNoteGateway()
        {
            return _foodJournalNoteGateway;
        }

        public IWeightJournalGateway GetWeightJournalGateway()
        {
            return _weightJournalGateway;
        }
    }
}
