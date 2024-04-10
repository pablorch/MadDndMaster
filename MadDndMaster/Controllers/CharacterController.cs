using MadDndMaster.Dnd.Model;
using MadDndMaster.Resources;
using MadDndMaster.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace MadDndMaster.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CharacterController : Controller
    {
        private readonly ICharacterService _characterService;

        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        [SwaggerOperation(Summary = "Get all classes")]
        [SwaggerResponse(200, "OK", typeof(IEnumerable<ClassModel>))]
        [SwaggerResponse(400, "Error", typeof(string))]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassModel>>> GetClasses()
        {
            try
            {
                var classes = await _characterService.GetClasses();
                return Ok(classes);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, Properties.ERROR + ex.Message);
            }
        }

        [SwaggerOperation(Summary = "Get all races")]
        [SwaggerResponse(200, "OK", typeof(IEnumerable<RaceModel>))]
        [SwaggerResponse(400, "Error", typeof(string))]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RaceModel>>> GetRaces()
        {
            try
            {
                var races = await _characterService.GetRaces();
                return Ok(races);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, Properties.ERROR + ex.Message);
            }
        }

        [SwaggerOperation(Summary = "Get all characters")]
        [SwaggerResponse(200, "OK", typeof(IEnumerable<CharacterModel>))]
        [SwaggerResponse(400, "Error", typeof(string))]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CharacterModel>>> GetCharacters()
        {
            try
            {
                var characters = await _characterService.GetCharacters();
                return Ok(characters);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, Properties.ERROR + ex.Message);
            }
        }

        [SwaggerOperation(Summary = "Create character")]
        [SwaggerResponse(200, "OK", typeof(CharacterModel))]
        [SwaggerResponse(400, "Error", typeof(string))]
        [HttpPost]
        public async Task<ActionResult<CharacterModel>> CreateCharacter(CharacterDTO character)
        {
            try
            {
                return Ok(await _characterService.AddCharacter(character));
                //return CreatedAtAction(nameof(GetCharacters), new { id = character.Id }, character);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, Properties.ERROR + ex.Message);
            }
        }
    }
}
