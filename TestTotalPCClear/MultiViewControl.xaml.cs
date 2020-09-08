using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace TestTotalPCClear
{
    public sealed partial class MultiViewControl : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int SelectedItemShow
        {
            get { return (int)GetValue(selectedItemShowProperty); }
            set
            {
                SetValue(selectedItemShowProperty, value);
            }
        }
        public static readonly DependencyProperty selectedItemShowProperty =
            DependencyProperty.Register("selectedItemShow", typeof(int), typeof(MultiViewControl), new PropertyMetadata(0, new PropertyChangedCallback(OnSelectedItemChanged)));

        public ObservableCollection<UIElement> Views
        {
            get { return (ObservableCollection<UIElement>)GetValue(ViewsProperty); }
            set
            {
                SetValue(ViewsProperty, value);
                NotifyPropertyChanged(nameof(Views));
            }
        }
        public static readonly DependencyProperty ViewsProperty =
            DependencyProperty.Register("Views", typeof(ObservableCollection<UIElement>), typeof(MultiViewControl), new PropertyMetadata(new ObservableCollection<UIElement>()));


        public MultiViewControl()
        {
            this.InitializeComponent();
            selectedItemChanged();
        }

        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MultiViewControl cc = d as MultiViewControl;
            int content = (int)e.NewValue;
            cc.SelectedItemShow = content;
            cc.selectedItemChanged();
        }

        public void selectedItemChanged()
        {
            mainStackPanel.Children.Clear();

            if (SelectedItemShow > Views.Count || Views.Count == 0) return;

            mainStackPanel.Children.Add(Views[SelectedItemShow]);
        }
    }
}
