using AutoMapper;
using MediatR;
using POS.Application.Contracts.Persistence;
using POS.Application.Models.Persistence;
using POS.Domain.Entities;

namespace POS.Application.Features.Clients.Queries
{
    public class PaginatedListQuery : PaginationRequest, IRequest<PaginationResponse<PaginatedListResponse>>
    {
        public string? FullName { get; set; }
        public string? CellphoneNumber { get; set; }
    }

    public class PaginatedListResponse
    {
        public uint Id { get; set; }
        public string FullName { get; set; }
        public string CellphoneNumber { get; set; }
        public string CellphoneNumberSearch { get; set; }
    }

    public class PaginatedListMapper : Profile
    {
        public PaginatedListMapper()
        {
            CreateMap<Client, PaginatedListResponse>();
            CreateMap<PaginationResponse<Client>, PaginationResponse<PaginatedListResponse>>();
        }
    }

    public class PaginatedListHandler : IRequestHandler<PaginatedListQuery, PaginationResponse<PaginatedListResponse>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public PaginatedListHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<PaginationResponse<PaginatedListResponse>> Handle(PaginatedListQuery request, CancellationToken cancellationToken)
        {
            request.CellphoneNumber = request.CellphoneNumber?.Trim() ?? string.Empty;
            var cellphoneStandardized = Util.StandarisizeCellphoneNumber(request.CellphoneNumber);
            request.FullName = request.FullName?.Trim() ?? string.Empty;

            var paginatedList = await unitOfWork.ClientRepository
                .ListPaginatedAsync(request.Page, request.PageSize,
                    x => (string.IsNullOrWhiteSpace(request.FullName) || x.FullName.Contains(request.FullName))
                    && (string.IsNullOrWhiteSpace(request.CellphoneNumber) || x.CellphoneNumber.Contains(request.CellphoneNumber) || x.CellphoneNumberSearch.Contains(cellphoneStandardized)));

            return mapper.Map<PaginationResponse<PaginatedListResponse>>(paginatedList);
        }
    }
}
