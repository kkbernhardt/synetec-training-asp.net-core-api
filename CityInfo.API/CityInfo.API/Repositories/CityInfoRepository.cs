using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Repositories
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private CityInfoContext context;

        public CityInfoRepository(CityInfoContext context)
        {
            this.context = context;
        }

        public City Get(int cityId, bool includePointOfInterest)
        {
            if (includePointOfInterest)
            {
                return context.Cities.Include(c => c.PointOfInterest)
                    .Where(c => c.Id == cityId).FirstOrDefault();
            }
            return context.Cities.Where(c => c.Id == cityId).FirstOrDefault();
        }

        public IEnumerable<City> GetCities()
        {
            return context.Cities.OrderBy(c => c.Name).ToList();
        }

        public IEnumerable<PointOfInterest> GetPointOfInterestForCity(int cityId)
        {
            return context.PointOfInterests.Where(p => p.CityId == cityId).ToList();
        }

        public PointOfInterest GetPointOfInterestForCity(int cityId, int pointOfInterestId)
        {
            return context.PointOfInterests
                .Where(p => p.Id == cityId && p.Id == pointOfInterestId).FirstOrDefault();
        }
    }
}
