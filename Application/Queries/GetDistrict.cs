/*using Domain.Repositories;
using MediatR;
using ApplicationException = Application.Exceptions.ApplicationException;

// Namespace for your application queries
namespace Application.Queries
{
    public class GetDistrict
    {
        // Query to fetch a single district by DistrictId
        public record GetDistrictByIdQuery(Guid DistrictId) : IRequest<GetDistrictResponse>;

        // DTO for District response
        public record GetDistrictResponse
        {
            public Guid? Id { get; init; }
            public string Name { get; init; } = default!;
            public string? Code { get; init; }
            public string? Description { get; init; }
            public Guid RegionId { get; init; }
            public string RegionName { get; init; } = default!;
        }

        // Handler for GetDistrictByIdQuery
        public class GetDistrictByIdHandler : IRequestHandler<GetDistrictByIdQuery, GetDistrictResponse>
        {
            private readonly IDistrictRepository _districtRepository;

            public GetDistrictByIdHandler(IDistrictRepository districtRepository)
            {
                _districtRepository = districtRepository;
            }

            public async Task<GetDistrictResponse> Handle(GetDistrictByIdQuery request, CancellationToken cancellationToken)
            {
                // Fetch the district by DistrictId
                var district = await _districtRepository.GetByIdAsync(request.DistrictId);

                if (district == null)
                {
                    throw new ApplicationException($"District not found.", Exceptions.ExceptionCodes.DistrictNotFound.ToString(), 404);
                }

                // Map the District entity to GetDistrictResponse DTO
                var response = new GetDistrictResponse
                {
                    Id = district.Id,
                    Name = district.Name,
                    Code = district.Code,
                    Description = district.Description,
                    RegionId = district.Region.Id,
                    RegionName = district.Region.Name
                };

                return response;
            }
        }
    }
}
*/