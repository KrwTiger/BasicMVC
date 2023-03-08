using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC.ModelView
{
    public class EmployeeCreateView
    {
        
        [Required(ErrorMessage = "* Please enter valid fullname."), 
         RegularExpression("white|list", ErrorMessage = "* Please enter only character.")]
        public string Fullname { get; set; }
        [Required(ErrorMessage = "* Please enter valid Address."),StringLength(200, ErrorMessage = "* You enter more than 10 character")]
        public string Address { get; set; }

        [Required(ErrorMessage = "* Please enter phone numbers"), 
         MaxLength(10, ErrorMessage = "* You enter more than 10 phone numbers."), 
         MinLength(10, ErrorMessage = "* Please enter 10 phone numbers."),
         RegularExpression("([0-9]+)", ErrorMessage = "* Please enter valid numbers.")]
        public string Phone { get; set; }

    }
}