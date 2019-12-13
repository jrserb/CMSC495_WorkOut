/*
    Name: Brett Snyder
    Date: 12/12/2019
    Course: CMSC 495 - Current Trends And Projects in Computer Science
    Desc: Model user workout object
*/

using System;
using System.Collections.Generic;

namespace WorkoutGen.Models
{
    public partial class UserWorkout
    {
        public UserWorkout()
        {
            UserSet = new HashSet<UserSet>();
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? DateDeleted { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<UserSet> UserSet { get; set; }
    }
}
