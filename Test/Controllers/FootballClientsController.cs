using Microsoft.AspNetCore.Mvc;
using Swager;
using System.Text;
using Test.Clients;
using Test.Model;

namespace Test.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class FootballClientsController : ControllerBase
    {
        private readonly ILogger<FootballClientsController> _logger;

        public FootballClientsController(ILogger<FootballClientsController> logger)
        {
            _logger = logger;
        }
        [HttpGet("/APIController/GetTeamById")]
        public async Task<IActionResult> GetTeamById(int teamId)
        {
            FootballClients clients = new FootballClients();
            var teamSearch = await clients.GetTeamById(teamId);

            if (teamSearch == null || teamSearch.response == null || teamSearch.response.Length == 0)
            {
                _logger.LogError($"Команда з ID {teamId} не знайдена.");
                return NotFound("Команда не знайдена.");
            }

            var team = teamSearch.response[0].team;
            var result = $"Команда: {team.name}\nID: {team.id}\nКраїна: {team.country}\nЗасновано: {team.founded}\nЛоготип: {team.logo ?? "Немає"}";
            return Ok(result);
        }

        [HttpGet("/APIController/GetNextMatch")]
        public async Task<IActionResult> GetSeasonInfo(int teamid, int number)
        {
            FootballClients clients1 = new FootballClients();
            var matchInfo = await clients1.GetNextMatchById(teamid, number);
            var fixtures = matchInfo.api.fixtures;
            var result = "Інформація про матчі:\n";
            foreach (var fixture in fixtures)
            {
                result += $"Дата: {fixture.event_date}\nКоманда 1: {fixture.homeTeam.team_name}\nКоманда 2: {fixture.awayTeam.team_name}\nРезультат: {fixture.score.fulltime}\nСтатус: {fixture.status}\n\n";
            }
            return Ok(result);
        }
        [HttpGet("/APIController/GetTeamByName")]
        public async Task<IActionResult> GetName(string name)
        {
            FootballClients clients = new FootballClients();
            try
            {

                FootballClients clients1 = new FootballClients();
                var searchResult = await clients1.GetName(name);

                if (searchResult != null && searchResult.api != null && searchResult.api.teams != null && searchResult.api.teams.Length > 0)
                {
                    var sb = new StringBuilder();
                    foreach (var team in searchResult.api.teams)
                    {
                        sb.AppendLine($"Команда: {team.name}\nID Команди: {team.team_id}\nКод: {team.code}\nКраїна: {team.country}\nЗасновано: {team.founded}\nЛоготип: {team.logo}\nСтадіон: {team.venue_name}\nАдреса стадіону: {team.venue_address}\nМісто стадіону: {team.venue_city}\nВмістимість стадіону: {team.venue_capacity}\n");
                    }
                    return Ok(sb.ToString());
                }
                else
                {
                    return NotFound("Команду не знайдено.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while fetching the team: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

        }
    }
}
