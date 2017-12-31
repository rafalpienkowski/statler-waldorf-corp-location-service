using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StatlerWaldorfCorp.LocationService.Models;

namespace StatlerWaldorfCorp.LocationService.Controllers
{
    [Route("api/locations")]
    public class LocationRecordController : Controller
    {
        private readonly ILocationRecordRepository _locationRecordRepository;

        public LocationRecordController(ILocationRecordRepository locationRecordRepository)
        {
            _locationRecordRepository = locationRecordRepository;
        }

        [HttpGet("{memberId}/latest")]
        public IActionResult GetLatestForMember(Guid memberId) => Ok(_locationRecordRepository.GetLatestForMember(memberId));

        [HttpGet("{memberId}")]
        public IActionResult GetLocationsForMember(Guid memberId) => Ok(_locationRecordRepository.AllForMember(memberId));

        [HttpPost]
        [Route("{memberId}")]
        public IActionResult AddLocation(Guid memberId, [FromBody]LocationRecord locationRecord)
        {
            var created = _locationRecordRepository.Add(locationRecord);
            return Created($"/api/locations/{memberId}/latest", created);
        }

    }
}
