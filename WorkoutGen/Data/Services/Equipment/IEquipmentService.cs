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

        public Task<IEnumerable<Models.Equipment>> GetEquipmentFromUserExercise(string userId, int id);

        public Task<int[]> GetEquipmentIdsFromExercise(int id);

        public Task<int[]> GetEquipmentIdsFromUserExercise(string userId, int id);

        public Task<int[]> GetEquipmentIdsFromExercises(int[] ids);

        public Task<int[]> GetEquipmentIdsFromUserExercises(string userId, int[] ids);

        public Task<int[]> GetExerciseEquipmentIdsFromExercise(int id);

        public Task<int[]> GetExerciseEquipmentIdsFromExercises(int[] ids);

        public Task<int[]> GetUserExerciseEquipmentIdsFromExercise(int id);

        public Task<int[]> GetUserExerciseEquipmentIdsFromExercises(int[] ids);

        public Task<int[]> GetAlternateEquipmentIdsFromExerciseEquipment(int id);

        public Task<int[]> GetAlternateEquipmentIdsFromExerciseEquipment(int[] ids);

        public Task<IEnumerable<Models.Equipment>> GetAlternateEquipmentFromExerciseEquipment(int exerciseId);
    }
}
