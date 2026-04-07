using Avalonia.Controls;
using Avalonia.Interactivity;
using AvaloniaApplication.ViewModels;
using System.Diagnostics;

namespace AvaloniaApplication.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void OnSliderValueChanged(object sender, Avalonia.Controls.Primitives.RangeBaseValueChangedEventArgs ev)
        {
            var viewModel = DataContext as MainWindowViewModel;
            viewModel.OnSliderValueChanged();
        }


        public void OnTextBoxValueChanged(object sender, Avalonia.Controls.TextChangedEventArgs e)
        {
            var viewModel = DataContext as MainWindowViewModel;
            viewModel.OnTextBoxValueChanged();
        }
    }
}