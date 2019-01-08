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

            if (!cityInfoRepository.CityExist(cityId))
            {
                return NotFound();
            }

            var finalPointOfInterest = Mapper.Map<Entities.PointOfInterest>(pointOfInterest);
            cityInfoRepository.AddPointOfInterestForCity(cityId, finalPointOfInterest);
            if (!cityInfoRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            var createdPointOfInterestToReturn = Mapper.Map<Models.PointOfInterestDto>(finalPointOfInterest);
            return Ok(createdPointOfInterestToReturn);
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

            if (!cityInfoRepository.CityExist(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity = cityInfoRepository.GetPointOfInterestForCity(cityId, id);
            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }
            Mapper.Map(pointOfInterest, pointOfInterestEntity);
            if (!cityInfoRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
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
            if (!cityInfoRepository.CityExist(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity = cityInfoRepository.GetPointOfInterestForCity(cityId, id);
            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = Mapper.Map<PointOfInterestForUpdateDto>(pointOfInterestEntity);

            patchDoc.ApplyTo(pointOfInterestToPatch, ModelState);
            
            TryValidateModel(pointOfInterestToPatch);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);
            if (!cityInfoRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
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
            //check if the city exist
            if (!cityInfoRepository.CityExist(cityId))
            {
                return NotFound();
            }
            //if yes we look for it
            var pointOfInterestEntity = cityInfoRepository.GetPointOfInterestForCity(cityId, id);
            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }
            //if exist we remove it and save changes
            cityInfoRepository.DeletePointOfInterest(pointOfInterestEntity);
            if (!cityInfoRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            mailService.Send("Point of interest deleted.", $"Point of interest {pointOfInterestEntity.Name}" +
                $"with id {pointOfInterestEntity.Id} was deleted.");
            return NoContent();
        }
    }
}
