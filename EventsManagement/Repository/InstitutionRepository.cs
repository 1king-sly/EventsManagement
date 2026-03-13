using Azure.Core;
using Dapper;
using EventsManagement.DTOs;
using System.Data;

namespace EventsManagement.Repository
{
    public class InstitutionRepository(IDbConnection dbConnection) : IInstitutionsRepository
    {
        public async Task<InstitutionOutDto?> CreateInstitutionAsync(InstitutionInDto request)
        {
            var query = @"SELECT email FROM institutions WHERE institutions.email = @email";
            var existingAccount = await dbConnection.QueryFirstOrDefaultAsync(query, new { email = request.Email });

            if (existingAccount is not null) return null;

            var createAccountQuery = @"INSERT INTO institutions(name,email,abbreviation) VALUES(@name,@email,@abbreviation);
                                        SELECT * FROM institutions WHERE institutionId = LAST_INSERT_ID();";

            var institution = await dbConnection.QueryFirstAsync<InstitutionOutDto>(createAccountQuery, new {name = request.Name, email = request.Email,abbreviation = request.Abbreviation });

            return institution;

        }
        public async Task<IEnumerable<InstitutionOutDto>> GetAllInstutionsAsync()
        {
            try {
                var query = @"SELECT * FROM institutions;";

                var institutions = await dbConnection.QueryAsync<InstitutionOutDto>(query);
                return institutions;
            
            }catch (Exception)
            {
                return [];
            }
        }

        public async Task<InstitutionOutDto?> GetInstitutionAsync(string requestId)
        {
            return await GetInstitution(requestId);
        }

        public async Task<InstitutionOutDto?> UpdateInstitutionAsync(string institutionId, InstitutionInDto request)
        {
            var institution = await GetInstitution(institutionId);

            if (institution == null) return null;

            var updateQuery = @"UPDATE institutions WHERE institutionId = @InstitutionId SET name = @name, email = @email, abbreviation = @abbreviation;
                                SELECT * FROM institutions WHERE institutionId = @institutionId;";

            var updatedInstitution = await dbConnection.QueryFirstAsync<InstitutionOutDto>(updateQuery, new
            {
                InstitutionId = institutionId,
                name = request.Name,
                email = request.Email,
                abbreviation = request.Abbreviation

            });

            return updatedInstitution;

        }
        public async Task<bool?> DeleteInstitutionAsync(string requestId)
        {
            try {
                var institution = await GetInstitution(requestId);

                if (institution is null) return null;

                var query = @"DELETE FROM institutions WHERE institutionId = @institutionId;";

                await dbConnection.ExecuteAsync(query);

                return true;

            } catch (Exception)
            {
                return false;
            }
            


        }

        private async Task<InstitutionOutDto?> GetInstitution(string institutionId)
        {
            var query = @"SELECT * FROM institutions WHERE institutionId = @institutionId";

            var institution = await dbConnection.QueryFirstOrDefaultAsync<InstitutionOutDto>(query, new { institutionId = institutionId });

            return institution;

        }


    }
}
