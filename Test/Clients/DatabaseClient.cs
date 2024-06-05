using Npgsql;
using Test.Model;

namespace Swager.Clients
{
    public class DatabaseClient
    {
        NpgsqlConnection con = new NpgsqlConnection(Test.Constants.Connect);

        public async Task InsertFavoriteTeam(Team1 team)
        {
            if (await FavoriteTeamExists())
            {
                throw new InvalidOperationException("Улюблена команда вже існує.");
            }

            var sql = "insert into public.\"FavoriteTeam\"(\"name\", \"id\", \"founded\", \"country\")" +
                      " values (@name, @id, @founded, @country)";
            using (NpgsqlCommand comm = new NpgsqlCommand(sql, con))
            {
                comm.Parameters.AddWithValue("name", team.name);
                comm.Parameters.AddWithValue("id", team.id);
                comm.Parameters.AddWithValue("founded", team.founded);
                comm.Parameters.AddWithValue("country", team.country);

                await con.OpenAsync();
                await comm.ExecuteNonQueryAsync();
                await con.CloseAsync();
            }
        }

        public async Task DeleteFavoriteTeam(int id)
        {
            var sql = "delete from public.\"FavoriteTeam\" where \"id\" = @id";
            using (NpgsqlCommand comm = new NpgsqlCommand(sql, con))
            {
                comm.Parameters.AddWithValue("id", id);

                await con.OpenAsync();
                await comm.ExecuteNonQueryAsync();
                await con.CloseAsync();
            }
        }

        public async Task UpdateFavoriteTeam(Team1 team)
        {
            var sql = "update public.\"FavoriteTeam\" set \"name\" = @name, \"founded\" = @founded, \"country\" = @country where \"id\" = @id";
            using (NpgsqlCommand comm = new NpgsqlCommand(sql, con))
            {
                comm.Parameters.AddWithValue("name", team.name);
                comm.Parameters.AddWithValue("id", team.id);
                comm.Parameters.AddWithValue("founded", team.founded);
                comm.Parameters.AddWithValue("country", team.country);

                await con.OpenAsync();
                try
                {
                    int affectedRows = await comm.ExecuteNonQueryAsync();
                    if (affectedRows == 0)
                    {
                        throw new InvalidOperationException("Команда не знайдена або не було оновлено жодного запису.");
                    }
                }
                catch (Exception ex)
                {

                    throw new Exception($"Помилка при оновленні команди: {ex.Message}");
                }
                finally
                {
                    await con.CloseAsync();
                }
            }
        }

        public async Task<bool> FavoriteTeamExists()
        {
            var sql = "select count(*) from public.\"FavoriteTeam\"";
            using (NpgsqlCommand comm = new NpgsqlCommand(sql, con))
            {
                await con.OpenAsync();
                var result = (long)await comm.ExecuteScalarAsync();
                await con.CloseAsync();
                return result > 0;
            }
        }
        public async Task<Team1> GetFavoriteTeam()
        {
            var sql = "select \"name\", \"id\", \"founded\", \"country\" from public.\"FavoriteTeam\" limit 1";
            using (NpgsqlCommand comm = new NpgsqlCommand(sql, con))
            {
                await con.OpenAsync();
                using (var reader = await comm.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        var team = new Team1
                        {
                            name = reader.GetString(0),
                            id = reader.GetInt32(1),
                            founded = reader.GetInt32(2),
                            country = reader.GetString(3)
                        };
                        await con.CloseAsync();
                        return team;
                    }
                    else
                    {
                        await con.CloseAsync();
                        return null;
                    }
                }
            }
        }
    }
}
