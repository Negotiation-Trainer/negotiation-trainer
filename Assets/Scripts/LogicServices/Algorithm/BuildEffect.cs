using Models;

namespace LogicServices.Algorithm
{
    public class BuildEffect
    {
        public bool Calculate(Trade trade, User target, User originator)
        {
            var points = target.PointTable[(trade.RequestedItem, originator)];
            return points switch
            {
                5 => true,
                0 => true,
                _ => false
            };
        }
    }
}