using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Exceptions;
using Common.Repository;
using Foods.Service.Intercom.Postcode;
using Foods.Service.Repository.Cooks;
using Foods.Service.Repository.Cooks.FoodBusiness;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Cooks.Service.Business.Helpers
{
    public class LocationHelper : ILocationHelper
    {
        private readonly IPostcodeIntercom _postcodeIntercom;
        private readonly IRepository<Cook> _cookRepository;

        public LocationHelper(
            IPostcodeIntercom postcodeIntercom,
            IRepository<Cook> cookRepository
            )
        {
            _postcodeIntercom = postcodeIntercom;
            _cookRepository = cookRepository;
        }

        public async Task SetPostcodeLocation(Address address, string cookId)
        {
            if (!await PostHasChanged(cookId, address))
            {
                return;
            }

            var location = await GetPostcodeLocation(address.PostCode);

            address.Location = location;
        }

        public async Task<Location> GetPostcodeLocation(string postcode)
        {
            var location = await _postcodeIntercom.GetLocation(postcode);

            if (!location.IsValid)
            {
                throw new FoodsValidationException("Postcode", postcode, location.ErrorMessage);
            }

            return new Location
            {
                Type = "Point",
                Coordinates = new List<double>
                {
                    location.Longitude,
                    location.Latitude
                }
            };
        }

        public async Task<IEnumerable<Cook>> GetNearbyCooks(double longitude, double latitude, double searchRadiusInMiles)
        {
            var radiusInMetres = ConvertMilesToMetres(searchRadiusInMiles);
            var filter = Builders<Cook>.Filter.NearSphere(x => x.Address.Location, GeoJson.Point(new GeoJson2DGeographicCoordinates(longitude, latitude)), radiusInMetres);
            return await _cookRepository.GetAll(filter, limitResults: 50);
        }

        private static double ConvertMilesToMetres(double miles)
        {
            const double kmPerMile = 1.609344;
            const double metresPerKm = 1000;

            var km = miles * kmPerMile;
            var metres = km * metresPerKm;

            return metres;
        }

        private async Task<bool> PostHasChanged(string cookId, Address address)
        {
            if (string.IsNullOrEmpty(cookId))
            {
                return false;
            }

            var cook = await _cookRepository.Get(cookId);

            if (cook.Address?.PostCode?.ToLower() != address.PostCode.ToLower())
            {
                return true;
            }

            address.Location = cook.Address?.Location;

            return false;
        }
    }
}
