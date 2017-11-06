using System;
using ViewModels;

namespace Pages
{
    public partial class TicketDetailsPage : PageBase
    {
        private readonly Lazy<TicketDetailsViewModel> _typedViewModel = new Lazy<TicketDetailsViewModel>();

        public override ViewModelBase ViewModel => _typedViewModel.Value;

        public TicketDetailsPage()
        {
            InitializeComponent();
        }
    }
}
