using System;
using System.Threading.Tasks;
using ViewModels;

namespace Services
{
    public interface INavigationService
    {
        Task Navigate<T>(Action<T> setup = null) where T : ViewModelBase;

        Task GoBack();

        void SetMainPage<T>() where T : ViewModelBase;

        Task<TResult> Prompt<TViewModel, TResult>(Action<TViewModel> setup = null) where TViewModel : ViewModelBase, IPromptable<TResult>;
    }
}