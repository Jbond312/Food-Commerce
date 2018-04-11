using System.Collections.Generic;
using Cooks.Service.Business.Entities.FoodBusiness;

namespace Cooks.Service.Business.Entities
{
    public class FoodBusinessRegistrationDto
    {
        public string Id { get; set; }
        public string CookId { get; set; }
        public ApplicantDetailsDto ApplicantDetails { get; set; }
        public ApplicantBusinessDto ApplicantBusiness { get; set; }

        //TODO TBC!
        public AddressDto CorrespondenceAddress { get; set; } //Not sure if needed as only required for 'Individual'
        public string ApplicationType { get; set; } //Not sure if needed as may auto-set to 'Individual'

        public FoodBusinessEstablishmentDto FoodBusinessEstablishment { get; set; }
        public FoodBusinessOperatorDto FoodBusinessOperator { get; set; }
        public List<string> TypeOfFoodBusiness { get; set; }
        public BusinessOperationDto BusinessOperation { get; set; }
        public string NumberOfPeopleInFoodBusiness { get; set; }
        public DeclarationDto Declaration { get; set; }
        public bool IsCompleted { get; set; }
    }
}
