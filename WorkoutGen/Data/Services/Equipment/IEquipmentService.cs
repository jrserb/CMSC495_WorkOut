using System.Collections.Generic;
using System.Threading.Tasks;

namespace WorkoutGen.Data.Services.Equipment
{
    public interface IEquipmentService
    {
        public Task<Models.Equipment> GetEquipment(int id);


        public Task<IEnumerable<Models.Equipment>> GetEquipment();


        public Task<IEnumerable<Models.Equipment>> GetEquipment(int[] ids);


        public Task<IEnumerable<Models.Equipment>> GetEquipmentFromExercise(int id);


        public Task<IEnumerable<Models.Equipment>> GetAlternateEquipmentFromExerciseEquipment(int exerciseId);


        public Task<int[]> GetEquipmentIdsFromExercise(int id);


        public Task<int[]> GetEquipmentIdsFromExercises(int[] ids);


        public Task<int[]> GetExerciseEquipmentIdsFromExercise(int id);


        public Task<int[]> GetExerciseEquipmentIdsFromExercises(int[] ids);


        public Task<int[]> GetExerciseEquipmentIdsFromExerciseAndEquipment(int exerciseId, int equipmentId);


        public Task<int[]> GetAlternateEquipmentIdsFromExerciseEquipment(int id);


        public Task<int[]> GetAlternateEquipmentIdsFromExerciseEquipment(int[] ids);




        // USER METHODS

        public Task<IEnumerable<Models.Equipment>> GetEquipmentFromUserExercise(string userId, int id);


        public Task<int[]> GetEquipmentIdsFromUserExercise(string userId, int id);


        public Task<int[]> GetEquipmentIdsFromUserExercises(string userId, int[] ids);


        public Task<int[]> GetUserExerciseEquipmentIdsFromExercise(int id);


        public Task<int[]> GetUserExerciseEquipmentIdsFromExercises(int[] ids);


        public Task<int[]> GetUserExerciseEquipmentIdsFromExerciseAndEquipment(int exerciseId, int equipmentId);


        public Task<Models.UserEquipmentSet> GetUserEquipmentSet(int id);


        public Task<IEnumerable<Models.UserEquipmentSet>> GetUserEquipmentSets(string userId);


        public Task<IEnumerable<Models.Equipment>> GetEquipmentFromUserEquipmentSet(int equipmentSetId);


        public Task<int> AddUserEquipmentSet(Models.UserEquipmentSet userEquipmentSet, int[] equipmentIds);


        public Task UpdateUserEquipmentSet(Models.UserEquipmentSet userEquipmentSet, int[] equipmentIds);


        public Task DeleteUserEquipmentSet(Models.UserEquipmentSet userEquipmentSet, int[] equipmentIds);


        public Task<bool> UserEquipmentSetExists(int id);
    }
}
