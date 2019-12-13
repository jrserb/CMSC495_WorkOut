/*
    Name: Brett Snyder
    Date: 12/12/2019
    Course: CMSC 495 - Current Trends And Projects in Computer Science
    Desc: This extends session object and abstracts some of the functionality into methods
*/

using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace WorkoutGen.Data.Session
{
    public static class SessionExtentions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) :
                JsonConvert.DeserializeObject<T>(value);
        }

        public static void ClearExerciseSession(this ISession session)
        {
            session.Remove("ExerciseIndex");
            session.Remove("IsUserExercise");
            session.Remove("WorkoutId");
            session.Remove("Sets");
        }
    }
}
