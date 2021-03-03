using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TryArabicPOC.Models
{
    public class Menu
    {
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
        public int ParentId { get; set; }
        public Menu ChildMenu { get; set; }

    }
}