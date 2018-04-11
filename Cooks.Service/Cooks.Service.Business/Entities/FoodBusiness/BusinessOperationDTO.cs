using System;

namespace Cooks.Service.Business.Entities.FoodBusiness
{
    public class BusinessOperationDto
    {
        public bool IsNewBusiness { get; set; }
        public DateTime? OpeningDate { get; set; }
        public bool IsSeasonalBusiness { get; set; }
        public DateTime? SeasonStart { get; set; }
        public DateTime? SeasonEnd { get; set; }
    }
}
