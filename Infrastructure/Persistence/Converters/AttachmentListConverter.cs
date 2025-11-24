using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using System.Text.Json;

namespace Infrastructure.Persistence.Converters
{
    public class AttachmentListConverter : ValueConverter<List<Attachment>, string>
    {
        public AttachmentListConverter()
            : base(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<List<Attachment>>(v, (JsonSerializerOptions)null) ?? new()
            )
        { }
    }
}
