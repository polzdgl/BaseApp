using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseApp.Data.LogTable.Models
{
    [Table("Log", Schema = "App")]
    public class Log
    {
        [Key]
        public int Id { get; set; }

        public DateTime? TimeStamp { get; set; }

        [MaxLength(200)]
        public string? Level { get; set; }

        [MaxLength]
        [Unicode]
        public string? Message { get; set; }

        [MaxLength]
        [Unicode]
        public string? Exception { get; set; }

        [MaxLength]
        [Unicode]
        public string? Properties { get; set; }

        [MaxLength(2000)]
        [Unicode]
        public string? RequestHeader { get; set; }

        public Guid? CorrelationId { get; set; }

        [MaxLength(200)]
        public string? MachineName { get; set; }

        [MaxLength(200)]
        public string? Application { get; set; }

        public int? ThreadId { get; set; }

        public int? ProcessId { get; set; }

        [MaxLength(200)]
        public string? ProcessName { get; set; }

        [MaxLength(50)]
        public string? ClientIp { get; set; }

        [MaxLength(200)]
        [Unicode]
        public string? UserName { get; set; }
    }
}
