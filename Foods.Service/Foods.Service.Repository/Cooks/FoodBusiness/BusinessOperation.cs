using System;

namespace Foods.Service.Repository.Cooks.FoodBusiness
{
    public class BusinessOperation
    {
        public bool IsNewBusiness { get; set; }
        public DateTime OpeningDate { get; set; }
        public bool IsSeasonalBusiness { get; set; }

        //If IsSeasonalBusiness is TRUE
        public DateTime SeasonStart { get; set; }
        public DateTime SeasonEnd { get; set; }
    }
}
