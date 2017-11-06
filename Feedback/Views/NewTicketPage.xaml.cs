using System;
using ViewModels;

namespace Pages
{
    public partial class NewTicketPage : PageBase
    {
        private readonly Lazy<NewTicketViewModel> _typedViewModel = new Lazy<NewTicketViewModel>();

        public override ViewModelBase ViewModel => _typedViewModel.Value;

        public NewTicketPage()
        {
            InitializeComponent();
        }
    }
}
