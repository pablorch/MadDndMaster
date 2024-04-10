using MadDndMaster.Data;
using MadDndMaster.Dnd.Model;
using MadDndMaster.Resources;
using MadDndMaster.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace MadDndMaster.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class MasterController : Controller
    {
        private readonly ICharacterService _characterService;

        public MasterController(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        [SwaggerOperation(Summary = "Create a random character")]
        [SwaggerResponse(200, "OK", typeof(CharacterModel))]
        [SwaggerResponse(400, "Error", typeof(string))]
        [HttpGet]
        public async Task<ActionResult<CharacterModel>> CreateRandomCharacter()
        {
            try
            {
                var character = await _characterService.AddRandomCharacter();
                return Ok(character);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, Properties.ERROR + ex.Message);
            }
        }

        [SwaggerOperation(Summary = "Annihilate all characters")]
        [SwaggerResponse(200, "OK", typeof(string))]
        [SwaggerResponse(400, "Error", typeof(string))]
        [HttpGet, Authorize]
        public async Task<ActionResult> MeteorShower()
        {
            try
            {
                await _characterService.DeleteAllCharacters();
                return Ok(Properties.MESSAGE_METEOR_SHOWER_OK);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, Properties.ERROR + ex.Message);
            }
        }

        [SwaggerOperation(Summary = "Attack a character")]
        [SwaggerResponse(200, "OK", typeof(string))]
        [SwaggerResponse(400, "Error", typeof(string))]
        [HttpPost]
        public async Task<ActionResult> AttackCharacter(int characterId, int damage)
        {
            try
            {
                int remainingHp = await _characterService.AttackCharacter(characterId, damage);
                if (remainingHp > 0)
                {
                    return Ok("Remaining HP: " + remainingHp);
                }
                else
                {
                    return Ok("The character died");
                }
                //return CreatedAtAction(nameof(GetCharacters), new { id = character.Id }, character);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, Properties.ERROR + ex.Message);
            }
        }
    }
}
