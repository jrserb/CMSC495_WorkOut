using System.Collections.Generic;
using System.Threading.Tasks;

namespace WorkoutGen.Data.Services.Equipment
{
    public interface IEquipmentService
    {
        public Task<IEnumerable<Models.Equipment>> GetEquipment();

        public Task<Models.Equipment> GetEquipment(int id);    

        public Task<IEnumerable<Models.Equipment>> GetEquipment(int[] ids);

        public Task<IEnumerable<Models.Equipment>> GetEquipmentFromExercise(int id);

        public Task<IEnumerable<Models.Equipment>> GetEquipmentFromUserExercise(int id);

        public Task<int[]> GetEquipmentIdsFromExercise(int id);

        public Task<int[]> GetEquipmentIdsFromUserExercise(int id);

        public Task<int[]> GetEquipmentIdsFromExercises(int[] ids);

        public Task<int[]> GetAlternateEquipmentIdsFromExerciseEquipment(int id);

        public Task<int[]> GetAlternateEquipmentIdsFromEquipment(int[] ids);

        public Task<IEnumerable<Models.Equipment>> GetAlternateEquipmentFromEquipment(int[] ids);
    }
}
