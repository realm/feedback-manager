using System;
using ViewModels;

namespace Pages
{
    public partial class TicketsPage : PageBase
    {
        private readonly Lazy<TicketsViewModel> _typedViewModel = new Lazy<TicketsViewModel>();

        public override ViewModelBase ViewModel => _typedViewModel.Value;

        public TicketsPage()
        {
            InitializeComponent();
        }
    }
}
