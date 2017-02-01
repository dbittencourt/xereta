using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace xereta.Models
{
    public class PublicWorker : BaseEntity
    {   
        public string Name {get; set;}
        public string CPF {get; set;}
        public string SIAPE {get; set;}
        public string OriginDepartment {get; set;}
        public string WorkingDepartment {get; set;}
        public string Role {get; set;}  
        [NotMapped]
        public Dictionary<string, float> Salaries {get; set;}
    }
}