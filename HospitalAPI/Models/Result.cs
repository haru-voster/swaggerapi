using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalAPI.Models
{
    [Table("result_table")] // maps directly to your SQL table
    public class Result
    {
        [Key]
        public int Id { get; set; }

        public string? TestName { get; set; }

        public string? TestValue { get; set; }

        public int LabRequestId { get; set; }
    }
}
