using ErrorOr;
using Florists.Application.Interfaces.Persistence;
using Florists.Application.Interfaces.Services;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Common.Messages;
using Florists.Core.DTO.Common;
using Florists.Core.Entities;
using MediatR;

namespace Florists.Application.Features.Flowers.Commands.CreateFlower
{
  public class CreateFlowerCommandHandler : IRequestHandler<CreateFlowerCommand, ErrorOr<MessageResultDTO>>
  {
    private readonly IFlowerRepository _flowerRepository;
    private readonly IDateTimeService _dateTimeService;

    public CreateFlowerCommandHandler(
      IFlowerRepository flowerRepository,
      IDateTimeService dateTimeService)
    {
      _flowerRepository = flowerRepository;
      _dateTimeService = dateTimeService;
    }

    public async Task<ErrorOr<MessageResultDTO>> Handle(
      CreateFlowerCommand command,
      CancellationToken cancellationToken)
    {
      var dbFlower = await _flowerRepository.GetOneByNameAsync(command.FlowerName);

      if (dbFlower is not null)
      {
        return CustomErrors.Flowers.FlowerAlreadyExists;
      }

      var flower = new Flower
      {
        FlowerId = Guid.NewGuid(),
        FlowerName = command.FlowerName,
        AvailableQuantity = command.AvailableQuantity,
        UnitPrice = command.UnitPrice,
        CreatedAt = _dateTimeService.UtcNow,
      };

      var success = await _flowerRepository.CreateAsync(flower);

      if (!success)
      {
        return CustomErrors.Flowers.UnableToCreateFlower;
      }

      return new MessageResultDTO(true, Messages.Flowers.CreateSuccess);
    }
  }
}
