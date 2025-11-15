

using System.ComponentModel.DataAnnotations;

namespace Domain.Common
{
    public interface IHasRowVersion
    {
        public byte[] RowVersion { get; set; }

    }
}
