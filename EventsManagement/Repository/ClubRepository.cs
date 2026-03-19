using Dapper;
using EventsManagement.DTOs;
using System.Data;

namespace EventsManagement.Repository
{
    enum QueryType {institution,school };
    public class ClubRepository(IDbConnection dbConnection) : IClubRepository
    {
        public async Task<ClubOutDto?> CreateClubAsync(ClubInDto request)
        {
            try {

                var existingClub = await dbConnection.QueryFirstOrDefaultAsync<ClubOutDto>("SELECT * FROM clubs WHERE schoolId = @schoolId AND name = @name", new { schoolId = request.SchoolId, name = request.Name });

                    if (existingClub is not null) return null;
                var query = @"INSERT INTO clubs(schoolId,name,abbreviation,userId)
                                VALUES(@schoolId,@name,@abbreviation,@userId);
                            SELECT * FROM clubs WHERE clubId = LAST_INSERT_ID();";

                var club = await dbConnection.QueryFirstAsync<ClubOutDto>(query,new {schoolId = request.SchoolId,name =request.Name,abbreviation = request.Abbreviation,userId = request.UserId});

                return club;
            
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<bool?> DeleteClubAsync(string clubId)
        {
            try { 
                var club = await GetClubAsync(clubId);

                if (club is null) return null;

                var query = @"DELETE FROM clubs WHERE clubId = @id";

                await dbConnection.ExecuteAsync(query,new {id = clubId});

                return true;
            
            } catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<ClubOutDto>?> GetAllClubsAsync()
        {
            try {
                var clubs = await GetAllOrInstitutionOrSchoolClubs();

                return clubs;
            }
            catch(Exception) {
                return null;
            }
        }

        public async Task<IEnumerable<ClubOutDto>?> GetAllClubsByInstitutionAsync(string institutionId)
        {
            try
            {
                var clubs = await GetAllOrInstitutionOrSchoolClubs(valueId:institutionId,queryType:QueryType.institution);

                return clubs;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IEnumerable<ClubOutDto>?> GetAllClubsBySchoolAsync(string schoolId)
        {
            try
            {
                var clubs = await GetAllOrInstitutionOrSchoolClubs(valueId: schoolId, queryType: QueryType.school);

                return clubs;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ClubOutDto?> GetClubAsync(string clubId)
        {
            try {
                var query = @"SELECT * FROM clubs WHERE clubId = @id";

                var club  = await dbConnection.QueryFirstOrDefaultAsync<ClubOutDto>(query,new {id = clubId});

                return club;
            
            } catch (Exception)
            {
                return null;
            }
        }

        public async Task<ClubOutDto?> UpdateClubAsync(string clubId, ClubInDto request)
        {
            try
            {
                var club = await GetClubAsync(clubId);

                if (club is null) return null;


                Console.WriteLine(club.Name);
                var query = @"UPDATE clubs
                            SET name = @name,abbreviation = @abbreviation,schoolId = @schoolId 
                            WHERE clubId = @clubId;
                            SELECT * FROM clubs WHERE clubId = @clubId;";

                var updatedClub = await dbConnection.QueryFirstAsync<ClubOutDto>(query, new {name = request.Name,abbreviation = request.Abbreviation,schoolId = request.SchoolId,clubId});

                return updatedClub;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private async Task<IEnumerable<ClubOutDto>?> GetAllOrInstitutionOrSchoolClubs(string? valueId = null, QueryType? queryType = null) {
            try
            {
                var query = @"SELECT * FROM clubs";

                

                if(valueId is not null && queryType is not null)
                {
                    switch (queryType)
                    {
                        case QueryType.institution:
                            query += " WHERE schoolId IN(SELECT schoolId FROM institutions WHERE institutionId = @id);";
                            
                            break;
                            case QueryType.school:
                            query += " WHERE schoolId = @id;";
                            break;
                            default:
                            break;
                    }
                }

                Console.WriteLine(query);
                var clubs = await dbConnection.QueryAsync<ClubOutDto>(query,valueId is not null ?new {id = valueId}:null);

                return clubs;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
