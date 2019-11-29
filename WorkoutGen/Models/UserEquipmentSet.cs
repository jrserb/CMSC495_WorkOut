using System;

namespace WorkoutGen.Models
{
    public partial class UserEquipmentSet
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public bool Enabled { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? DateDeleted { get; set; }

        public virtual AspNetUsers User { get; set; }
    }
}
