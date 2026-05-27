using System.ComponentModel.DataAnnotations;

namespace WebObrasci1.Models
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Timestamp]
        public uint RowVersion { get; set; }
    }
}
