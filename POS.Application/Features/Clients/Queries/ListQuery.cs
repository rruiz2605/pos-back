﻿using AutoMapper;
using MediatR;
using POS.Application.Contracts.Persistence;
using POS.Domain.Entities;

namespace POS.Application.Features.Clients.Queries
{
    public class ListQuery : IRequest<IEnumerable<ListResponse>>
    {
        public string? FullName { get; set; }
        public string? CellphoneNumber { get; set; }
    }

    public class ListResponse
    {
        public uint Id { get; set; }
        public string FullName { get; set; }
        public string CellphoneNumber { get; set; }
        public string CellphoneNumberSearch { get; set; }
    }

    public class ListMapper : Profile
    {
        public ListMapper()
        {
            CreateMap<Client, ListResponse>();
        }
    }

    public class ListHandler : IRequestHandler<ListQuery, IEnumerable<ListResponse>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ListHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ListResponse>> Handle(ListQuery request, CancellationToken cancellationToken)
        {
            var list = await unitOfWork.Repository<Client>()
                .ListAsync();

            return mapper.Map<IEnumerable<ListResponse>>(list);
        }
    }
}
