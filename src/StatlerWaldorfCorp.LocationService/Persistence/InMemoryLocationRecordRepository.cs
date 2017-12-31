using System;
using System.Collections.Generic;
using StatlerWaldorfCorp.LocationService.Models;
using System.Linq;

namespace StatlerWaldorfCorp.LocationService.Persistence
{
    public class InMemoryLocationRecordRepository : ILocationRecordRepository
    {
        protected static ICollection<LocationRecord> _locations;

        public InMemoryLocationRecordRepository()
        {
            if (_locations == null)
            {
                _locations = new List<LocationRecord>();
            }
        }

        public LocationRecord Add(LocationRecord locationRecord)
        {
            _locations.Add(locationRecord);
            return locationRecord;
        }

        public ICollection<LocationRecord> AllForMember(Guid memberId) => _locations.Where(l => l.MemberId.Equals(memberId)).ToList();

        public LocationRecord Delete(Guid memberId, Guid recordId)
        {
            var locationToDelete = Get(memberId, recordId);
            if (locationToDelete != null)
            {
                _locations.Remove(locationToDelete);
            }
            return locationToDelete;
        }

        public LocationRecord Get(Guid memberId, Guid recordId) => _locations.FirstOrDefault(l => l.MemberId.Equals(memberId) && l.Id.Equals(recordId));

        public LocationRecord GetLatestForMember(Guid memberId) 
            => _locations
                .Where(l => l.MemberId.Equals(memberId))
                .OrderByDescending(l => l.Timestamp)
                .FirstOrDefault();

        public LocationRecord Update(LocationRecord locationRecord)
        {
            var locationToUpdate = Delete(locationRecord.MemberId, locationRecord.Id);
            if (locationToUpdate == null)
            {
                return locationToUpdate;
            }
            locationRecord = Add(locationRecord);
            return locationRecord;
        }
    }
}