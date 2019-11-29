using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkoutGen.Data.Services.MuscleGroup
{
    public class MuscleGroupService : IMuscleGroupService
    {
        private readonly ApplicationDbContext _context;

        public MuscleGroupService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Models.MuscleGroup> GetMuscleGroup(int id)
        {
            return await _context.MuscleGroup
                        .Where(x => x.Id == id)
                        .SingleAsync();
        }

        public async Task<IEnumerable<Models.MuscleGroup>> GetMuscleGroups()
        {
            return await _context.MuscleGroup
                        .OrderBy(x => x.Name)
                        .ToListAsync();
        }

        public async Task<IEnumerable<Models.MuscleGroup>> GetMuscleGroups(int[] ids)
        {
            return await _context.MuscleGroup
                        .Where(x => ids.Contains(x.Id))
                        .ToListAsync();
        }
    }
}
