using AutoMapper;
using FluentValidation;
using MediatR;
using POS.Application.Contracts.Persistence;
using POS.Domain.Entities;

namespace POS.Application.Features.Clients.Commands
{
    public class UpdateCommand : IRequest<UpdateResponse>
    {
        public uint Id { get; set; }
        public string FullName { get; set; }
        public string CellphoneNumber { get; set; }
    }

    public class UpdateResponse
    {
        public uint ClientId { get; set; }
    }

    public class UpdateValidator : AbstractValidator<UpdateCommand>
    {
        public UpdateValidator()
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

    public class UpdateMapper : Profile
    {
        public UpdateMapper()
        {
            CreateMap<UpdateCommand, Client>()
                .ForMember(d => d.CellphoneNumberSearch, s => s.MapFrom(x => Util.StandarisizeCellphoneNumber(x.CellphoneNumber)));
        }
    }

    public class UpdateHandler : IRequestHandler<UpdateCommand, UpdateResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public UpdateHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<UpdateResponse> Handle(UpdateCommand request, CancellationToken cancellationToken)
        {
            var repository = unitOfWork.Repository<Client>();
            var entity = await repository.GetByIdAsync(request.Id);
            
            mapper.Map(request, entity);

            unitOfWork.Repository<Client>()
                .Update(entity!);

            await unitOfWork.Complete();

            return new UpdateResponse { ClientId = entity!.Id };
        }
    }
}
