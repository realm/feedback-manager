using System;
using System.Threading.Tasks;
using Realms;
using Realms.Sync;

namespace ViewModels
{
    public abstract class MainViewModelBase : ViewModelBase
    {
        protected virtual bool IsPartial => false;
        protected Realm _realm;

        protected override async void InitializeCore()
        {
            base.InitializeCore();

            User user = null;

            try
            {
                user = User.Current;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            if (user == null)
            {
                try
                {
                    user = await NavigationService.Prompt<LoginViewModel, User>();
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
            else
            {
                var uri = user.ServerUri;
                Constants.RosUrl = $"{uri.Host}:{uri.Port}";
            }

            try
            {
                var config = new SyncConfiguration(user, Constants.SyncServerUri)
                {
                    IsPartial = IsPartial
                };

                _realm = Realm.GetInstance(config);

                await CompleteInitializationAsync(user);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        protected abstract Task CompleteInitializationAsync(User user);
    }
}
