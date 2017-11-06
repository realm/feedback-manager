using System;
using System.Linq;
using System.Threading.Tasks;
using Realms;
using Realms.Sync;
using Services;
using Xamarin.Forms;

namespace ViewModels
{
    public class LoginViewModel : ViewModelBase, IPromptable<User>
    {
        #region Promptable

        public Action<User> Success { get; set; }

        public Action Cancel { get; set; }

        public Action<Exception> Error { get; set; }

        #endregion

        private readonly Realm _realm;
        private string _password;

        public LoginDetails Details { get; }

        public string Password
        {
            get
            {
                return _password;
            }

            set
            {
                Set(ref _password, value);
            }
        }

        public Command LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(Login, () => !IsBusy);

            var cacheConfig = new RealmConfiguration("logincache.realm")
            {
                ObjectClasses = new[] { typeof(LoginDetails) }
            };

            _realm = Realm.GetInstance(cacheConfig);
            var loginDetails = _realm.All<LoginDetails>().FirstOrDefault();
            if (loginDetails == null)
            {
                loginDetails = new LoginDetails
                {
                    ServerUrl = Constants.RosUrl
                };

                _realm.Write(() => _realm.Add(loginDetails));
            }

            Details = loginDetails;
        }

        private void Login()
        {
            LoginCore(() => Task.FromResult(Credentials.UsernamePassword(Details.Username, Password, false)));
        }

        private void LoginCore(Func<Task<Credentials>> getCredentialsFunc)
        {
            PerformTask(async () =>
            {
                _realm.Write(() =>
                {
                    Details.ServerUrl = Details.ServerUrl.Replace("http://", string.Empty)
                                                         .Replace("https://", string.Empty)
                                                         .Replace("realm://", string.Empty)
                                                         .Replace("realms://", string.Empty);
                });

                Constants.RosUrl = Details.ServerUrl;

                var credentials = await getCredentialsFunc();
                var user = await User.LoginAsync(credentials, Constants.AuthServerUri);

                Success(user);
            }, onError: async ex =>
            {
                await Task.Delay(500);
                DialogService.Alert("Unable to login", ex.Message);
                HandleException(ex);
            }, progressMessage: "Logging in...");
        }

        [Explicit]
        public class LoginDetails : RealmObject
        {
            public string ServerUrl { get; set; }

            public string Username { get; set; }
        }
    }
}
