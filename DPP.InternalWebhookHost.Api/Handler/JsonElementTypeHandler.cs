using Dapper;
using System.Data;
using System.Text.Json; 

namespace DPP.InternalWebhookHost.Infrastructure.Handler;

public sealed class JsonElementTypeHandler
: SqlMapper.TypeHandler<JsonElement>
{
	public override JsonElement Parse(object value)
	{
		if (value is string json)
		{
			return JsonSerializer.Deserialize<JsonElement>(json);
		} 
		return default;
	}

	public override void SetValue(
		IDbDataParameter parameter,
		JsonElement value)
	{
		parameter.Value = value.GetRawText();
		parameter.DbType = DbType.String;
	}
}
