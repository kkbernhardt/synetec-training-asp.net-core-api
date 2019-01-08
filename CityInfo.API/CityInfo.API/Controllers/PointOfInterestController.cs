using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Repositories;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class PointOfInterestController : Controller
    {
        private IMailService mailService;
        private ICityInfoRepository cityInfoRepository;
        public PointOfInterestController(IMailService mailService, ICityInfoRepository cityInfoRepository)
        {
            this.mailService = mailService;
            this.cityInfoRepository = cityInfoRepository;
        }

        [HttpGet("{cityId}/pointsofinterest")]
        public IActionResult GetPointOfInterest(int cityId)
        {
            if (!cityInfoRepository.CityExist(cityId))
            {
                return NotFound();
            }
            var pointsOfInterestForCity = cityInfoRepository.GetPointOfInterestForCity(cityId);
            var pointsOfInterestForCityResult = Mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterestForCity);
            return Ok(pointsOfInterestForCityResult);
        }

        [HttpGet("{cityId}/pointsofinterest/{id}", Name = "GetPointOfInterest")]
        public IActionResult GetPointOfInterest(int cityId, int id)
        {
            if (!cityInfoRepository.CityExist(cityId))
            {
                return NotFound();
            }
            var pointOfInterest = cityInfoRepository.GetPointOfInterestForCity(cityId, id);
            if (pointOfInterest == null)
            {
                return NotFound();
            }
            var pointOfInterestResult = Mapper.Map<PointOfInterestDto>(pointOfInterest);
            return Ok(pointOfInterestResult);
        }

        [HttpPost("{cityId}/pointsofinterest")]
        public IActionResult CreatePointOfInterest(int cityId,
            [FromBody] PointOfInterestForCreationDto pointOfInterest)
        {
            if (pointOfInterest == null)
            {
                return BadRequest();
            }

            if (pointOfInterest.Description == pointOfInterest.Name)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the name.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }
            var maxPointOfInterestId = CitiesDataStore.Current.Cities.
                SelectMany(c => c.PointOfInterest).Max(p => p.Id);
            var finalPointOfInterest = new PointOfInterestDto()
            {
                Id = maxPointOfInterestId,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };
            city.PointOfInterest.Add(finalPointOfInterest);
            return CreatedAtRoute("GetPointOfInterest", new
            { cityId = cityId, id = finalPointOfInterest.Id }, finalPointOfInterest); ;
        }

        [HttpPut("{cityId}/pointsofinterest/{id}")]
        public IActionResult UpdatePointOfInterest(int cityId, int id, [FromBody] PointOfInterestForUpdateDto pointOfInterest)
        {
            if (pointOfInterest == null)
            {
                return BadRequest();
            }

            if (pointOfInterest.Description == pointOfInterest.Name)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the name.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestFromStore = city.PointOfInterest.FirstOrDefault(p => p.Id == id);
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            pointOfInterestFromStore.Name = pointOfInterest.Name;
            pointOfInterestFromStore.Description = pointOfInterest.Description;
            return NoContent();
        }

        [HttpPatch("{cityId}/pointsofinterest/{id}")]
        public IActionResult PartiallyUpdatePointOfInterest(int cityId, int id,
                [FromBody] JsonPatchDocument<PointOfInterestForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestFromStore = city.PointOfInterest.FirstOrDefault(p => p.Id == id);
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch =
                new PointOfInterestForUpdateDto()
                {
                    Name = pointOfInterestFromStore.Name,
                    Description = pointOfInterestFromStore.Description
                };

            patchDoc.ApplyTo(pointOfInterestToPatch, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (pointOfInterestToPatch.Description == pointOfInterestToPatch.Name)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the name.");
            }
            pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
            pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;
            return NoContent();
            /*Patch request should look like this:
            [
                {
                "op": "replace",
                "path": "/name",
                "value": "updated - central park"
                }
            ]*/
        }

        [HttpDelete("{cityId}/pointsofinterest/{id}")]
        public IActionResult  DeletePointOfInterest(int cityId, int id)
        {
            //check if they exist
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestFromStore = city.PointOfInterest.FirstOrDefault(p => p.Id == id);
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }
            //if exist we remove
            city.PointOfInterest.Remove(pointOfInterestFromStore);

            mailService.Send("Point of interest deleted.", $"Point of interest {pointOfInterestFromStore.Name}" +
                $"with id {pointOfInterestFromStore.Id} was deleted.");
            return NoContent();
        }
    }
}
