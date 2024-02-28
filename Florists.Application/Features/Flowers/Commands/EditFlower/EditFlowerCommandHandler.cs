using ErrorOr;
using Florists.Application.Interfaces.Persistence;
using Florists.Application.Interfaces.Services;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Common.Messages;
using Florists.Core.DTO.Common;
using MediatR;

namespace Florists.Application.Features.Flowers.Commands.EditFlower
{
  public class EditFlowerCommandHandler : IRequestHandler<EditFlowerCommand, ErrorOr<MessageResultDTO>>
  {
    private readonly IFlowerRepository _flowerRepository;
    private readonly IDateTimeService _dateTimeService;

    public EditFlowerCommandHandler(
      IFlowerRepository flowerRepository,
      IDateTimeService dateTimeService)
    {
      _flowerRepository = flowerRepository;
      _dateTimeService = dateTimeService;
    }

    public async Task<ErrorOr<MessageResultDTO>> Handle(
      EditFlowerCommand command,
      CancellationToken cancellationToken)
    {
      var flower = await _flowerRepository.GetOneByIdAsync(command.FlowerId);

      if (flower is null)
      {
        return CustomErrors.Flowers.FlowerNotFound;
      }

      var sameNameFlower = await _flowerRepository.GetOneByNameAsync(command.FlowerName);

      if (sameNameFlower is not null && sameNameFlower.FlowerId != command.FlowerId)
      {
        return CustomErrors.Flowers.FlowerAlreadyExists;
      }

      flower.FlowerName = command.FlowerName;
      flower.UnitPrice = command.UnitPrice;
      flower.AvailableQuantity = command.AvailableQuantity;
      flower.UpdatedAt = _dateTimeService.UtcNow;

      var success = await _flowerRepository.UpdateAsync(flower);

      if (!success)
      {
        return CustomErrors.Flowers.UnableToEditFlower;
      }

      return new MessageResultDTO(true, Messages.Flowers.EditSuccess);
    }
  }
}
