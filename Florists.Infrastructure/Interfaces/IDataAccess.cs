using Florists.Infrastructure.DTO;

namespace Florists.Infrastructure.Interfaces
{
  public interface IDataAccess
  {
    Task<List<T>> LoadData<T, U>(
      string sql,
      U parameters);
    Task<int> SaveData<T>(
      string sql,
      T parameters);

    Task<int> SaveDataTransaction(List<DependantQueryDTO> dependantQueries);
  }
}
