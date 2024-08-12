namespace AcaiCore
{
    public class AcaiSession
    {
        private readonly IFoodItemGateway _foodItemGateway;

        public AcaiSession(IFoodItemGateway foodItemGateway)
        {
            _foodItemGateway = foodItemGateway;
        }

        public IFoodItemGateway GetFoodItemGateway()
        {
            return _foodItemGateway;
        }
    }
}
