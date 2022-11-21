using System;
using System.ComponentModel.DataAnnotations;

namespace IngenicoOKC.Models
{
    public class UniqueIdData
    {
        [Key]
        public int Id { get; set; }
        public string UniqueId { get; set; }
        public DateTime RecordTime { get; set; }
    }
}
