using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace PCCleaner.Helpers.CustomXamlElements
{
    public class CustomListBox : ListBox
    {
        public Brush EvenListBoxItemBackground
        {
            get { return (Brush)GetValue(EvenListBoxItemBackgroundProperty); }
            set { SetValue(EvenListBoxItemBackgroundProperty, value); }
        }

        public static readonly DependencyProperty EvenListBoxItemBackgroundProperty =
            DependencyProperty.Register("EvenListBoxItemBackground", typeof(Brush), typeof(CustomListBox), new PropertyMetadata(null));

        public Brush OddListBoxItemBackground
        {
            get { return (Brush)GetValue(OddListBoxItemBackgroundProperty); }
            set { SetValue(OddListBoxItemBackgroundProperty, value); }
        }

        public static readonly DependencyProperty OddListBoxItemBackgroundProperty =
            DependencyProperty.Register("OddListBoxItemBackground", typeof(Brush), typeof(CustomListBox), new PropertyMetadata(null));


        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            var listBoxItem = element as ListBoxItem;
            if (listBoxItem != null)
            {
                var index = IndexFromContainer(element);

                if ((index + 1) % 2 != 1)
                {
                    listBoxItem.Background = EvenListBoxItemBackground;
                }
                if ((index + 1) % 2 == 1)
                {
                    listBoxItem.Background = OddListBoxItemBackground;
                }
            }
        }
    }
}
