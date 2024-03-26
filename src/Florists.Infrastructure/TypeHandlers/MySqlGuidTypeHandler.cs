using Dapper;
using System.Data;

namespace Florists.Infrastructure.TypeHandlers
{
  public class MySqlGuidTypeHandler : SqlMapper.TypeHandler<Guid>
  {
    public override Guid Parse(object value)
    {
      return new Guid((string)value);
    }

    public override void SetValue(IDbDataParameter parameter, Guid guid)
    {
      parameter.Value = guid.ToString();
    }
  }
}
