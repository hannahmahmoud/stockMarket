using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class Stock

    {
        public int id {get; set;}
        public String companyName {get; set;} = string.Empty;
        public String symbol {get; set;} = string.Empty;
        
        [Column(TypeName ="decimal(18,2)")]
        public  decimal purchase {get; set;}

        [Column(TypeName ="decimal (18,2)")]
         public  decimal lastDiv {get; set;}

         public String  industry {get; set;}= string.Empty;

         public  long matketCap {get; set;}
         public List<Comments> Comments { get; set; } = new List<Comments>();




        
    }
}