using AutoMapper;
using FluentValidation;
using MediatR;
using POS.Application.Contracts.Persistence;
using POS.Domain.Entities;

namespace POS.Application.Features.Clients.Commands
{
    public class CreateCommand : IRequest<CreateResponse>
    {
        public string? FullName { get; set; }
        public string? CellphoneNumber { get; set; }
    }

    public class CreateResponse
    {
        public uint ClientId { get; set; }
    }

    public class CreateValidator : AbstractValidator<CreateCommand>
    {
        public CreateValidator()
        {
            RuleFor(p => p.FullName)
                .NotEmpty().WithMessage("Nombre es requerido.")
                .NotNull()
                .MaximumLength(250).WithMessage("El nombre no debe exceder los 250 caracteres");

            RuleFor(p => p.CellphoneNumber)
                .NotEmpty().WithMessage("Número de celular es requerido")
                .NotNull()
                .MaximumLength(20).WithMessage("El número de celular debe tener máximo 25 caracteres");
        }
    }

    public class CreateMapper : Profile
    {
        public CreateMapper()
        {
            CreateMap<CreateCommand, Client>()
                .ForMember(dest => dest.CellphoneNumberSearch, opt => opt.MapFrom(src => Util.StandarisizeCellphoneNumber(src.CellphoneNumber)))
                ;
        }
    }

    public class CreateHandler : IRequestHandler<CreateCommand, CreateResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public CreateHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<CreateResponse> Handle(CreateCommand request, CancellationToken cancellationToken)
        {
            var entity = mapper.Map<Client>(request);

            unitOfWork.ClientRepository.Add(entity);

            await unitOfWork.Complete();

            return new CreateResponse { ClientId = entity.Id };
        }
    }
}
