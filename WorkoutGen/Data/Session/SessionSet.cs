/*
    Name: Brett Snyder
    Date: 12/12/2019
    Course: CMSC 495 - Current Trends And Projects in Computer Science
    Desc: This is a class used in session for users who are not logged in
*/

namespace WorkoutGen.Data.Session
{
    public class SessionSet
    {
        public int? exerciseId { get; set; }
        public int? userExerciseId { get; set; }
        public string set { get; set; }
    }
}
