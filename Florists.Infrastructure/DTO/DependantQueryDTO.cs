namespace Florists.Infrastructure.DTO
{
  public record DependantQueryDTO(
    string SqlMaster,
    object ParametersMaster,
    string SqlSlave,
    object ParametersSlave);
}
