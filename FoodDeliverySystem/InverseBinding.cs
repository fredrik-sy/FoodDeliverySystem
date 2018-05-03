using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoodDeliverySystem
{
    class InverseBinding : Binding
    {
        public InverseBinding(string propertyName, object dataSource, string dataMember) : base(propertyName, dataSource, dataMember)
        {
        }

        protected override void OnFormat(ConvertEventArgs cevent)
        {
            cevent.Value = !((bool)cevent.Value);
            base.OnFormat(cevent);
        }

        protected override void OnParse(ConvertEventArgs cevent)
        {
            cevent.Value = !((bool)cevent.Value);
            base.OnParse(cevent);
        }
    }
}
