using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace MVC
{
    public class BorrowModel
    {
       
        public List<Book> books { get; set; }
        public List<Employee> employees { get; set; }

    }
    public class BookModel
    {
        public List<Book> TableBooks { get; set; }
    }
}