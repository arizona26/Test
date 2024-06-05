using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swager.Clients;
using Test.Clients;

namespace Swager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataBaseController : ControllerBase
    {
        private readonly ILogger<DataBaseController> _logger;
        public DataBaseController(ILogger<DataBaseController> logger)
        {
            _logger = logger;
        }

        [HttpPost("/DatabaseController/AddTeam")]
        public async Task<IActionResult> AddTeam(int Id)
        {
            FootballClients clients = new FootballClients();
            var teamSearch = await clients.GetTeamById(Id);

            if (teamSearch.response == null || teamSearch.response.Length == 0)
            {
                return NotFound("Команда не знайдена.");
            }

            DatabaseClient db = new DatabaseClient();
            try
            {
                await db.InsertFavoriteTeam(teamSearch.response[0].team);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok("Команда успішно додана.");
        }

        [HttpPut("/DatabaseController/UpdateTeam")]
        public async Task<IActionResult> UpdateTeam(int Id)
        {
            FootballClients clients = new FootballClients();
            var teamSearch = await clients.GetTeamById(Id);

            if (teamSearch == null || teamSearch.response.Length == 0)
            {
                _logger.LogError($"Команда з ID {Id} не знайдена для оновлення.");
                return NotFound("Команда не знайдена.");
            }

            DatabaseClient db = new DatabaseClient();
            try
            {
                _logger.LogInformation($"Updating team with ID: {Id}");
                await db.UpdateFavoriteTeam(teamSearch.response[0].team);
                _logger.LogInformation($"Команда з ID {Id} успішно оновлена.");
                return Ok("Команда успішно оновлена.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Помилка при оновленні команди з ID {Id}: {ex.Message}");
                return StatusCode(500, "Внутрішня помилка сервера");
            }
        }

        [HttpDelete("/DatabaseController/DeleteTeam")]
        public async Task<IActionResult> DeleteTeam(int teamId)
        {
            FootballClients clients = new FootballClients();
            var teamSearch = await clients.GetTeamById(teamId);

            if (teamSearch == null || teamSearch.response.Length == 0)
            {
                return NotFound("Команда не знайдена.");
            }

            DatabaseClient db = new DatabaseClient();
            await db.DeleteFavoriteTeam(teamId);
            return Ok($"Команда з ID {teamId} успішно видалена.");
        }
        [HttpGet("/DatabaseController/GetFavoriteTeam")]
        public async Task<IActionResult> GetFavoriteTeam()
        {
            DatabaseClient db = new DatabaseClient();
            var team = await db.GetFavoriteTeam();
            if (team == null)
            {
                return NotFound("Улюблена команда не знайдена.");
            }

            var formattedTeam =
               $" Команда: {team.name}" +
               $" ID: {team.id}" +
               $" Країна: {team.country}" +
               $" Засновано: {team.founded}";
            return Ok(formattedTeam);
        }
    }
}
