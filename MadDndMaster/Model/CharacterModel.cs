using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MadDndMaster.Dnd.Model
{
    public class CharacterModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        [ForeignKey("ClassId")]
        public int ClassId { get; set; }
        public virtual ClassModel Class { get; set; }

        [ForeignKey("RaceId")]
        public int RaceId { get; set; }
        public virtual RaceModel Race { get; set; }
        public int HP { get; set; }
        public int CurrentHP { get; set; }
        public int Attack { get; set; }
        public int Armor { get; set; }
    }
}
