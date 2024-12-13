namespace AcaiCore
{
    public class AcaiSession
    {
        private readonly IFoodItemGateway _foodItemGateway;
        private readonly IFoodItemShortcutGateway _foodItemShortcutGateway;

        public AcaiSession(IFoodItemGateway foodItemGateway, IFoodItemShortcutGateway foodItemShortcutGateway)
        {
            _foodItemGateway = foodItemGateway;
            _foodItemShortcutGateway = foodItemShortcutGateway;
        }

        public IFoodItemGateway GetFoodItemGateway()
        {
            return _foodItemGateway;
        }

        public IFoodItemShortcutGateway GetFoodItemShortcutGateway()
        {
            return _foodItemShortcutGateway;
        }
    }
}
