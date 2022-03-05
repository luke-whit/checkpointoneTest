namespace CheckpointTest
{
    class Order
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public string Date { get; set; }
        public string Status { get; set; }


        public Order()
        {
            Price = 0;
            Id = 0;
            Date = string.Empty;
            Status = string.Empty;
        }
    }
}
