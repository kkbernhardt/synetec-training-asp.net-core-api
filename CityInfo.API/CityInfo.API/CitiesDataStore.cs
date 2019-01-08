using CityInfo.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API
{
    public class CitiesDataStore
    {
        public static CitiesDataStore Current { get; } = new CitiesDataStore();
        public List<CityDto> Cities { get; set; }
        public CitiesDataStore()
        {
            Cities = new List<CityDto>()
            {
                new CityDto()
                {
                    Id = 1,
                    Name = "New York City",
                    Description = "The one with that big park.",
                    PointOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 1,
                            Name = "Cathedral of our Lady",
                            Description = "A gothic style cathedral."
                        },
                        new PointOfInterestDto()
                        {
                            Id = 2,
                            Name = "Cathedral of our Lady",
                            Description = "A gothic style cathedral."
                        }
                    }
                },

                new CityDto()
                {
                    Id = 2,
                    Name = "Antwerp",
                    Description = "The one with the cathedral that was never finished.",
                    PointOfInterest = new List<PointOfInterestDto>()
                    {
                         new PointOfInterestDto()
                        {
                            Id = 1,
                            Name = "Antwerpen Central Station",
                            Description = "The finest example of railway architecture."
                        },
                         new PointOfInterestDto()
                        {
                            Id = 2,
                            Name = "Antwerpen Central Station",
                            Description = "The finest example of railway architecture."
                        },
                         new PointOfInterestDto()
                        {
                            Id = 3,
                            Name = "Antwerpen Central Station",
                            Description = "The finest example of railway architecture."
                        }
                    }
                },
                new CityDto()
                {
                    Id = 2,
                    Name = "Budapest",
                    Description = "The one with the beautiful river.",
                    PointOfInterest = new List<PointOfInterestDto>()
                    {
                         new PointOfInterestDto()
                        {
                            Id = 1,
                            Name = "Keleti palyaudvar",
                            Description = "The finest example of railway architecture."
                        },
                         new PointOfInterestDto()
                        {
                            Id = 2,
                            Name = "Varosliget",
                            Description = "The beautiest park."
                        },
                         new PointOfInterestDto()
                        {
                            Id = 3,
                            Name = "Parliament",
                            Description = "The finest example of parliament."
                        }
                    }
                }
            };
        }

    }
}
