using CityInfo.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API
{
    public static class CityInfoContextExtension
    {
        public static void EnsureSeedDataForContext(this CityInfoContext context)
        {
            if (context.Cities.Any())
            {
                return;
            }

            var cities = new List<City>()
            {
                new City()
                {
                    Name = "New York City",
                    Description = "The one with that big park.",
                    PointOfInterest = new List<PointOfInterest>()
                    {
                        new PointOfInterest()
                        {
                            Name = "Central Park",
                            Description = "The most visited urban park in the United States."
                        },
                        new PointOfInterest()
                        {
                            Name = "Empire State Building",
                            Description = "A 102-story skyscraper located in Midtown Manhattan."
                        }
                    }
                },
                new City()
                {
                    Name = "London",
                    Description = "The one with smart travel.",
                    PointOfInterest = new List<PointOfInterest>()
                    {
                        new PointOfInterest()
                        {
                            Name = "Hyde park",
                            Description = "The most visited urban park in England."
                        },
                        new PointOfInterest()
                        {
                            Name = "Shard",
                            Description = "A skyscraper located in central London."
                        }
                    }
                },
                new City()
                {
                    Name = "Budapest",
                    Description = "The one with hot waters.",
                    PointOfInterest = new List<PointOfInterest>()
                    {
                        new PointOfInterest()
                        {
                            Name = "Varosliget",
                            Description = "The most visited urban park."
                        },
                        new PointOfInterest()
                        {
                            Name = "Parliament",
                            Description = "A beautiful parliament in the central."
                        },
                        new PointOfInterest()
                        {
                            Name = "Danube",
                            Description = "A beautiful river through the central."
                        }
                    }
                },
                new City()
                {
                    Name = "Paris",
                    Description = "The one with the big tower.",
                    PointOfInterest = new List<PointOfInterest>()
                    {
                        new PointOfInterest()
                        {
                            Name = "Eiffel tower",
                            Description = "The most visited by couples."
                        },
                        new PointOfInterest()
                        {
                            Name = "The Louvre",
                            Description = "The world largest museum."
                        }
                    }
                }
            };

            context.Cities.AddRange(cities);
            context.SaveChanges();
        }
    }
}
