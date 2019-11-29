using System;

namespace WorkoutGen.Models
{
    public partial class UserEquipmentSetEquipment
    {
        public int Id { get; set; }
        public int EquipmentId { get; set; }
        public int UserEquipmentSetId { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? DateDeleted { get; set; }

        public virtual Equipment Equipment { get; set; }
    }
}
