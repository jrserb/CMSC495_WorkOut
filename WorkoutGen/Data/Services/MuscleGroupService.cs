using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkoutGen.Models;

namespace WorkoutGen.Data.Services
{
    public class MuscleGroupService
    {
        private readonly WorkoutGenContext _context;

        public MuscleGroupService(WorkoutGenContext context)
        {
            _context = context;
        }

        public async Task<List<MuscleGroup>> GetMuscleGroups()
        { 
            return await _context.MuscleGroup.ToListAsync();
        }
    }
}
