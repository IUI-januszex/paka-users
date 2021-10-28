namespace PakaUsers.Model
{
    public abstract class Worker : User
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public double Salary { get; set; }
        public int WarehouseId { get; set; }
    }
}