namespace Foods.Service.Repository.Cooks.FoodBusiness
{
    public class FoodBusinessEstablishment
    {
        //i.e. Trading name
        public string FoodBusinessName { get; set; }
        public bool IsSameAddress { get; set; }
        public Address EstablishmentAddress { get; set; }
    }
}
