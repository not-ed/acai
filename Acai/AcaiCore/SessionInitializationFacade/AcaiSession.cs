namespace AcaiCore
{
    public class AcaiSession
    {
        private readonly IFoodItemGateway _foodItemGateway;
        private readonly IFoodItemShortcutGateway _foodItemShortcutGateway;
        private readonly IFoodJournalNoteGateway _foodJournalNoteGateway;

        public AcaiSession(IFoodItemGateway foodItemGateway, IFoodItemShortcutGateway foodItemShortcutGateway, IFoodJournalNoteGateway foodJournalNoteGateway)
        {
            _foodItemGateway = foodItemGateway;
            _foodItemShortcutGateway = foodItemShortcutGateway;
            _foodJournalNoteGateway = foodJournalNoteGateway;
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
    }
}
