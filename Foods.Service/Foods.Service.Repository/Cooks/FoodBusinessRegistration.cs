using System.Collections.Generic;
using Common.Repository;
using Foods.Service.Repository.Cooks.FoodBusiness;

namespace Foods.Service.Repository.Cooks
{
    public class FoodBusinessRegistration : BaseEntity
    {
        public string CookId { get; set; }
        //Page 1
        public ApplicantDetails ApplicantDetails { get; set; }
        public ApplicantBusiness ApplicantBusiness { get; set; }

        //TODO TBC!
        public Address CorrespondenceAddress { get; set; } //Not sure if needed as only required for 'Individual'
        public string ApplicationType { get; set; } //Not sure if needed as may auto-set to 'Individual'

        //Page 2
        public FoodBusinessEstablishment FoodBusinessEstablishment { get; set; }

        //Page 3
        public FoodBusinessOperator FoodBusinessOperator { get; set; }

        //Page 4
        //E.g Packer, Imported, Catering, Hotel, Distribution, etc
        public List<string> TypeOfFoodBusiness { get; set; }

        //Page 5
        public BusinessOperation BusinessOperation { get; set; }

        //Page 6
        //E.g. 0-10, 11-50, etc
        public string NumberOfPeopleInFoodBusiness { get; set; }

        //Page 7 - Declaration
        public Declaration Declaration { get; set; }

        //Determines whether the food business is completed and can no longer be altered
        public bool IsCompleted { get; set; }
    }
}
