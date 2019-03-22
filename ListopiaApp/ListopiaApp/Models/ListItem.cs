using ListopiaApp.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ListopiaApp.Models
{
    public class ListItem : BaseViewModel
    {
        private bool _on;
        private string _name;

        public int Id { get; set; }

        public string Name
        {
            get { return _name; }
            set { SetValue(ref _name, value); }
        }

        public string CheckedByUserName { get; set; }
        
        public bool IsChecked
        {
            get { return _on; }
            set { SetValue(ref _on, value); }
        }

        public int ListId { get; set; }

        public override bool Equals(object obj)
        {
            var li = obj as ListItem;
            if (li == null)
                return false;

            return Id == li.Id && Name == li.Name && CheckedByUserName == li.CheckedByUserName && IsChecked == li.IsChecked && ListId == li.ListId;
        }
    }
}
