using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Realms;
using Realms.Server;
using Realms.Sync;

namespace Server
{
    public class Program
    {
        public static void Main(string[] args) => MainAsync().Wait();

        private static async Task MainAsync()
        {
            SyncConfiguration.SetFeatureToken(Constants.FeatureToken);

            var credentials = Credentials.UsernamePassword(Constants.RosUsername, Constants.RosPassword, createUser: false);
            var admin = await User.LoginAsync(credentials, new Uri($"http://{Constants.RosUrl}"));

            // Hack to create synchronized Realm the first time it's used
            var syncConfig = new SyncConfiguration(admin, new Uri($"realm://{Constants.RosUrl}/{Constants.FeedbackRealm}"));
            using (var realm = Realm.GetInstance(syncConfig))
            {
                if (!realm.All<Foo>().Any())
                {
                    realm.Write(() => realm.Add(new Foo()));
                    await realm.GetSession().WaitForUploadAsync();

                    await admin.ApplyPermissionsAsync(PermissionCondition.Default, syncConfig.ServerUri.ToString(), AccessLevel.Write);
                }
            }

            var config = new NotifierConfiguration(admin)
            {
                Handlers = { new FeedbackHandler() },
                WorkingDirectory = Path.Combine(Directory.GetCurrentDirectory(), Constants.NotifierDirectory)
            };

            using (var notifier = await Notifier.StartAsync(config))
            {
                do
                {
                    Console.WriteLine("Type in 'exit' to quit the app.");
                }
                while (Console.ReadLine() != "exit");
            }
        }

        public class Foo : RealmObject
        {
            public int Value { get; set; }
        }
    }
}