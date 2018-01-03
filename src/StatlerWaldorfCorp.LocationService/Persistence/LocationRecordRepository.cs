using System;
using System.Linq;
using System.Collections.Generic;
using StatlerWaldorfCorp.LocationService.Models;
using Microsoft.EntityFrameworkCore;

namespace StatlerWaldorfCorp.LocationService.Persistence
{
    public class LocationRecordRepository: ILocationRecordRepository
    {
        private readonly LocationDbContext _dbContext;

        public LocationRecordRepository(LocationDbContext locationDbContext)
        {
            _dbContext = locationDbContext;
        }

        public LocationRecord Add(LocationRecord locationRecord)
        {
            _dbContext.LocationRecords.Add(locationRecord);
            _dbContext.SaveChanges();
            return locationRecord;
        }

        public ICollection<LocationRecord> AllForMember(Guid memberId)
        {
            return _dbContext.LocationRecords
                .Where(m => m.MemberId.Equals(memberId))
                .OrderBy(m => m.Timestamp)
                .ToList();
        }

        public LocationRecord Delete(Guid memberId, Guid recordId)
        {
            var locationRecord = Get(memberId, recordId);
            _dbContext.Remove(locationRecord);
            _dbContext.SaveChanges();
            return locationRecord;
        }

        public LocationRecord Get(Guid memberId, Guid recordId)
        {
            return _dbContext.LocationRecords
                .Single(m => m.MemberId.Equals(memberId) && m.Id.Equals(recordId));
        }

        public LocationRecord GetLatestForMember(Guid memberId)
        {
            var locationRecord = _dbContext.LocationRecords
                .Where(m => m.MemberId.Equals(memberId))
                .OrderBy(m => m.Timestamp)
                .Last();
            
            return locationRecord;
        }

        public LocationRecord Update(LocationRecord locationRecord)
        {
            _dbContext.Entry(locationRecord).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return locationRecord;
        }
    }
}