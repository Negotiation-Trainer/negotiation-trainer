namespace Models
{
    public class User
    {
        public readonly Inventory Inventory;
        
        public User()
        {
            Inventory = new Inventory();
        }
    }
}