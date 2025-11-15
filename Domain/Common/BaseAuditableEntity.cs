
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Domain.Common
{
    public abstract class BaseAuditableEntity : BaseEntity
    {
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }

        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    }
}
