using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class CitiesController : Controller
    {
        private ICityInfoRepository cityInfoRepository;

        public CitiesController(ICityInfoRepository cityInfoRepository)
        {
            this.cityInfoRepository = cityInfoRepository;
        }

        [HttpGet]
        public IActionResult GetCities()
        {
            var cityEntities = cityInfoRepository.GetCities();
            var results = Mapper.Map<IEnumerable<CityWithoutPointOfInterestDto>>(cityEntities);
            return Ok(results);
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id, bool includePointOfInterest = false)
        {
            var city = cityInfoRepository.GetCity(id, includePointOfInterest);
            if (city == null)
            {
                return NotFound();
            }

            //when the query string in in the url: http://localhost/api/cities/1?incudePointOfInterest=true
            if (includePointOfInterest)
            {
                var cityResult = Mapper.Map<CityDto>(city);
                return Ok(cityResult);
            }

            var cityWithOutPointsOfInterestResult = Mapper.Map<CityWithoutPointOfInterestDto>(city);
            return Ok(cityWithOutPointsOfInterestResult);
        }
    }
}
