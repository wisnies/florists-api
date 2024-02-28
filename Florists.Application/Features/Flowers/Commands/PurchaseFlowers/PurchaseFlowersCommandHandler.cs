using ErrorOr;
using Florists.Application.Interfaces.Persistence;
using Florists.Application.Interfaces.Services;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Common.Messages;
using Florists.Core.DTO.Common;
using Florists.Core.Entities;
using Florists.Core.Enums;
using MediatR;

namespace Florists.Application.Features.Flowers.Commands.PurchaseFlowers
{
  public class PurchaseFlowersCommandHandler : IRequestHandler<PurchaseFlowersCommand, ErrorOr<MessageResultDTO>>
  {
    private readonly IFlowerRepository _flowerRepository;
    private readonly IFlowerTransactionRepository _flowerTransactionRepository;
    private readonly IUserRepository _userRepository;
    private readonly IDateTimeService _dateTimeService;

    public PurchaseFlowersCommandHandler(
      IFlowerRepository flowerRepository,
      IDateTimeService dateTimeService,
      IUserRepository userRepository,
      IFlowerTransactionRepository flowerTransactionRepository)
    {
      _flowerRepository = flowerRepository;
      _dateTimeService = dateTimeService;
      _userRepository = userRepository;
      _flowerTransactionRepository = flowerTransactionRepository;
    }

    public async Task<ErrorOr<MessageResultDTO>> Handle(
      PurchaseFlowersCommand command,
      CancellationToken cancellationToken)
    {
      var user = await _userRepository.GetOneByEmailAsync(command.Email);

      if (user is null)
      {
        return CustomErrors.Auth.InvalidCredentials;
      }

      var transactions = new List<FlowerTransaction>();

      foreach (var dto in command.FlowersToPurchase)
      {
        var flower = await _flowerRepository.GetOneByIdAsync(dto.FlowerId);

        if (flower is null)
        {
          return CustomErrors.Flowers.FlowerNotFound;
        }

        flower.UpdatedAt = _dateTimeService.UtcNow;

        var transaction = new FlowerTransaction
        {
          FlowerTransactionId = Guid.NewGuid(),
          FlowerId = flower.FlowerId,
          UserId = user.UserId,
          PurchaseOrderNumber = command.PurchaseOrderNumber,
          QuantityBefore = flower.AvailableQuantity,
          QuantityAfter = flower.AvailableQuantity + dto.QuantityToPurchase,
          TransactionValue = flower.UnitPrice * dto.QuantityToPurchase,
          TransactionType = FlowerTransactionTypeOptions.PurchaseFlower,
          CreatedAt = _dateTimeService.UtcNow,
          Flower = flower
        };

        transactions.Add(transaction);
      }

      var success = await _flowerTransactionRepository.PurchaseAsync(transactions);

      if (!success)
      {
        return CustomErrors.Flowers.UnableToPurchaseFlowers;
      }

      return new MessageResultDTO(
        true,
        Messages.Flowers.PurchaseSuccess);
    }
  }
}
