using MadDndMaster.Data;
using MadDndMaster.Dnd.Model;
using MadDndMaster.Resources;
using Microsoft.EntityFrameworkCore;

namespace MadDndMaster.Services
{
    public class CharacterService : ICharacterService
    {
        private readonly DndContext _context;
        private Random randomGen;

        public CharacterService(DndContext context)
        {
            randomGen = new Random();
            context.Characters.Include(c => c.Class).Include(c => c.Race);
            _context = context;
        }

        public async Task<CharacterModel> AddCharacter(CharacterDTO character)
        {
            CharacterModel newChar = new CharacterModel {
                Name = character.Name,
                ClassId = character.ClassId,
                RaceId = character.RaceId,
                Attack = character.Attack,
                Armor = character.Armor,
                HP = character.HP,
                CurrentHP = character.CurrentHP,
            };
            await AddCharacter(newChar);
            return newChar;
        }

        public async Task<CharacterModel> AddRandomCharacter()
        {
            var i = await _context.Classes.CountAsync();
            var randomClass = await _context.Classes.FindAsync(randomGen.Next(1, 1 + await _context.Classes.CountAsync()));
            var randomRace = await _context.Races.FindAsync(randomGen.Next(1, 1 + await _context.Races.CountAsync()));
            CharacterModel newRandomChar = new CharacterModel
            {
                Name = "str",
                Class = randomClass,
                ClassId = randomClass.Id,
                Race = randomRace,
                RaceId = randomRace.Id,
                Attack = randomClass.Attack + randomGen.Next(randomRace.AttackModifier),
                Armor = randomClass.Armor + randomGen.Next(randomRace.ArmorModifier),
                HP = randomClass.MaxHP,
            };
            newRandomChar.CurrentHP = newRandomChar.HP;
            await AddCharacter(newRandomChar);
            return newRandomChar;
        }

        public async Task<int> AttackCharacter(int characterId, int damage)
        {
            if(CharacterExists(characterId))
            {
                var character = await _context.Characters.FindAsync(characterId);
                int remainingHP = character.CurrentHP - damage;
                if (remainingHP <= 0)
                {
                    await DeleteCharacter(character);
                }
                else
                {
                    character.CurrentHP = remainingHP;
                    await _context.SaveChangesAsync();
                }
                return remainingHP;
            }
            else
            {
                throw new Exception(Properties.ERROR_CHARACTER_NOT_EXISTS);
            }
        }

        public async Task DeleteAllCharacters()
        {
            await _context.Database.ExecuteSqlRawAsync(Properties.DATABASE_TRUNCATE + Properties.DATABASE_TABLE_CHARACTERS);
        }

        public async Task<IEnumerable<CharacterModel>> GetCharacters()
        {
            return await _context.Characters.ToListAsync(); ;
        }

        public async Task<IEnumerable<ClassModel>> GetClasses()
        {
            return await _context.Classes.ToListAsync();
        }

        public async Task<IEnumerable<RaceModel>> GetRaces()
        {
            return await _context.Races.ToListAsync();
        }

        private async Task<CharacterModel> AddCharacter(CharacterModel character)
        {
            if (IsCharacterValid(character))
            {
                await _context.Characters.AddAsync(character);
                await _context.SaveChangesAsync();
                return character;
            }
            else
            {
                throw new Exception(Properties.ERROR_CHARACTER_NOT_VALID);
            }
        }

        private bool IsCharacterValid(CharacterModel character)
        {
            // TODO: there should be more checks like race and class check also
            if (character.HP < character.CurrentHP)
            {
                return false;
            }
            return true;
        }

        private bool CharacterExists(int characterId)
        {
            return _context.Characters.Any(c => c.Id == characterId);
        }

        private async Task DeleteCharacter(CharacterModel character)
        {
            _context.Characters.Remove(character);
            await _context.SaveChangesAsync();
        }
    }
}
