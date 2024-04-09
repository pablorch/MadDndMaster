using System.ComponentModel.DataAnnotations.Schema;

namespace MadDndMaster.Dnd.Model
{
    public class CharacterDTO
    {
        public string Name { get; set; }
        public int ClassId { get; set; }
        public int RaceId { get; set; }
        public int HP { get; set; }
        public int CurrentHP { get; set; }
        public int Attack { get; set; }
        public int Armor { get; set; }
    }
}
