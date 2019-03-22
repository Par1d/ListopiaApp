using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ListopiaApp.Models
{
    public class List
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string OwnerName { get; set; }
    }
}
