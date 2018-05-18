using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Exceptions;
using Common.Repository;
using Foods.Service.Repository.Cooks;
using Foods.Service.Repository.Cooks.FoodBusiness;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;
using Postcodes.Service.Business.Helper;

namespace Cooks.Service.Business.Helpers
{
    public class LocationHelper : ILocationHelper
    {

        private readonly IRepository<Cook> _cookRepository;

        public LocationHelper(

            IRepository<Cook> cookRepository
            )
        {
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
            var postcodeclient = new PostcodesClient();
            var location = await postcodeclient.GetPostcode(postcode);

            if (!location.IsValid)
            {
                throw new FoodsValidationException("Postcode", postcode, location.ErrorMessage);
            }

            return new Location
            {
                Type = "Point",
                Coordinates = new List<double>
                {
                    location.Result.Longitude,
                    location.Result.Latitude
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
