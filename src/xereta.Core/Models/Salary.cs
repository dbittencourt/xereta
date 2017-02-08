using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace xereta.Core.Models
{
    public class Salary : BaseEntity
    {
        public override string Id {get; set;}
        public int Year {get; set;}
        public int Month {get; set;}
        public float Income {get; set;}
    }
}