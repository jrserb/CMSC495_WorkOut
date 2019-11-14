using System.Collections.Generic;
using System.Threading.Tasks;

namespace WorkoutGen.Data.Services.Equipment
{
    public interface IEquipmentService
    {
        public Task<IEnumerable<Models.Equipment>> GetEquipment();

        public Task<Models.Equipment> GetEquipment(int id);    

        public Task<IEnumerable<Models.Equipment>> GetEquipment(int[] ids);

        public Task<int[]> GetEquipmentIdsFromExercises(int[] ids);

        public Task<int[]> GetAlternateEquipmentIdsFromEquipment(int[] ids);
    }
}
