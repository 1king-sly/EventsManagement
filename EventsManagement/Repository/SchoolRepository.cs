using Dapper;
using EventsManagement.DTOs;
using System.Data;

namespace EventsManagement.Repository
{
    public class SchoolRepository(IDbConnection dbConnection) : ISchoolRepository
    {
        public async Task<SchoolOutDto> CreateSchoolAsync(SchoolCreateDto request)
        {
            var query = @"INSERT INTO schools(institutionId,name,abbreviation)
                        VALUES(@institutionId,@name,@abbreviation);
                        SELECT * FROM schools WHERE schoolId = LAST_INSERT_ID();";

            var school = await dbConnection.QueryFirstAsync<SchoolOutDto>(query,request);

            return school;
        }

        public async Task<bool?> DeleteSchoolAsync(string schoolId)
        {
            var school = await GetSchoolByIdAsync(schoolId);

            if (school is null) return null;

            var query = "DELETE FROM schools WHERE schoolId = @schoolId";

            var execution = await dbConnection.ExecuteAsync(query,schoolId);

            return execution != 0;
        }

        public async Task<IEnumerable<SchoolOutDto>> GetAllSchoolsAsync()
        {
            var query = @"
                        SELECT * FROM schools";

            var schools  = await dbConnection.QueryAsync<SchoolOutDto>(query);
            return schools;
        }

        public async Task<IEnumerable<SchoolOutDto>> GetAllSchoolsByInstitutionAsync(string institutionId)
        {
            var query = @"SELECT * FROM schools WHERE institutionId = @institutionId";
            var schools = await dbConnection.QueryAsync<SchoolOutDto>(query,new {institutionId});
            return schools;
        }

        public async Task<SchoolOutDto?> GetSchoolByIdAsync(string schoolId)
        {
            var query = @"SELECT * FROM schools WHERE schoolId = @schoolId";

            var school = await dbConnection.QueryFirstOrDefaultAsync<SchoolOutDto>(query,new {schoolId});

            return school;
            
        }

        public Task<IEnumerable<UserOutDto>> GetSchoolLeadersAsync(string institutionId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UserOutDto>> GetSchoolMembersAsync(string institutionId)
        {
            var query = @"SELECT * FROM users WHERE schoolId = @id;";

            var users = await dbConnection.QueryAsync<UserOutDto>(query, new { id = institutionId });

            return users;
        }

        public async Task<SchoolOutDto?> UpdateSchoolAsync(SchoolUpdateDto request)
        {
            var school = await GetSchoolByIdAsync(request.schoolId);

            if (school is null) return null;

            var query = @"UPDATE schools WHERE schoolId = @schoolId
                        SET name = @name, abbreviation = @abbreviation;
                        SELECT * FROM schools WHERE schoolId = @schoolId;";

            var newSchool = await dbConnection.QueryFirstAsync<SchoolOutDto>(query,request);

            return newSchool;
        }
    }
}
