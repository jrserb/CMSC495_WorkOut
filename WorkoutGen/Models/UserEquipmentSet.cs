/*
    Name: Brett Snyder
    Date: 12/12/2019
    Course: CMSC 495 - Current Trends And Projects in Computer Science
    Desc: Model equipment set object
*/

using System;
using System.ComponentModel.DataAnnotations;

namespace WorkoutGen.Models
{
    public partial class UserEquipmentSet
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public bool Enabled { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? DateDeleted { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
