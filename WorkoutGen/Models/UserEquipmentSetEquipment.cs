using System;
using System.Collections.Generic;

namespace WorkoutGen.Models
{
    public partial class UserEquipmentSetEquipment
    {
        public int Id { get; set; }
        public int EquipmentId { get; set; }
        public int UserEquipmentSetId { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? DateDeleted { get; set; }
    }
}
