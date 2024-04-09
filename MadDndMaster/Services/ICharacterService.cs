using MadDndMaster.Dnd.Model;

namespace MadDndMaster.Services
{
    public interface ICharacterService
    {
        Task<IEnumerable<ClassModel>> GetClasses();
        Task<IEnumerable<RaceModel>> GetRaces();
        Task<IEnumerable<CharacterModel>> GetCharacters();
        Task<CharacterModel> AddCharacter(CharacterDTO character);
        Task<CharacterModel> AddRandomCharacter();
        Task<int> AttackCharacter(int characterId, int damage);
        Task DeleteAllCharacters();
    }
}
