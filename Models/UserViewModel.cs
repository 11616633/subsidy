using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SubsidyReconciliation.Models
{
    public class UserViewModel
    {
        public string applicationcode { get; set; }

        //[Required(ErrorMessage = "User Name is Required")]
        //public string username { get; set; }

        //[Required(ErrorMessage ="Password is Required")]

        //public string password { get; set; }
        [Required]
        public string username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }
}
