using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epylion.Models
{
    public class WebMenu
    {
        public string ID { get; set; } = string.Empty;
        public string URL { get; set; }
        public string URL_DIV { get; set; }
        public string Text { get; set; }
        public string ParentID { get; set; }
        public string MenuOwner { get; set; }
        public int SortOrder { get; set; }
        public bool Checked { get; set; }
        public bool HasUserTypePermission { get; set; }  // this property set on user permission page , role type selection , api call

        public string CLASS { get; set; }
        public string SPAN_CLASS { get; set; }
        public string DefaultActionName { get; set; }

        public bool HasThirdLayer { get; set; }
    }
}
