namespace Net
{
    public class Lot
    {
        public Lot(string name, string unit, string count, string price)
        {
            Name = name;
            Unit = unit;
            Count = count;
            Price = price;
        }

        public string Name { get; set; }
        public string Unit { get; set; }
        public string Count { get; set; }
        public string Price { get; set; }
    }
}
