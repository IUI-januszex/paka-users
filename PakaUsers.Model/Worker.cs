namespace PakaUsers.Model
{
    public abstract class Worker : User
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public decimal Salary { get; set; }
        public int? WarehouseId { get; set; }
        public string WarehouseType { get; set; }
    }
}