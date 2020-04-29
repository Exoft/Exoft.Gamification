namespace Exoft.Gamification.Api.Data.Core.Entities
{
    public class OrderCategory : Entity
    {
        public Order Order { get; set; }

        public Category Category { get; set; }
    }
}
