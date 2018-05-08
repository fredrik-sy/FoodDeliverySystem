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

        /// <summary>
        /// Raises the <see cref="Binding.Format"/> event with inversed value.
        /// </summary>
        protected override void OnFormat(ConvertEventArgs cevent)
        {
            cevent.Value = !((bool)cevent.Value);
            base.OnFormat(cevent);
        }

        /// <summary>
        /// Raises the <see cref="Binding.Parse"/> event with inversed value.
        /// </summary>
        protected override void OnParse(ConvertEventArgs cevent)
        {
            cevent.Value = !((bool)cevent.Value);
            base.OnParse(cevent);
        }
    }
}
