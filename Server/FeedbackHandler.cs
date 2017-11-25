using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Text.Core;
using Microsoft.ProjectOxford.Text.KeyPhrase;
using Microsoft.ProjectOxford.Text.Sentiment;
using Realms;
using Realms.Server;

namespace Server
{
    public class FeedbackHandler : RegexNotificationHandler
    {
        private readonly SentimentClient _sentimentClient;
        private readonly KeyPhraseClient _keyPhraseClient;

        public FeedbackHandler() : base($"^/{Constants.FeedbackRealm}$")
        {
            _sentimentClient = new SentimentClient(Constants.SentimentApiKey)
            {
                Url = Constants.SentimentUrl
            };

            _keyPhraseClient = new KeyPhraseClient(Constants.SentimentApiKey)
            {
                Url = Constants.KeyPhraseUrl
            };
        }

        public override async Task HandleChangeAsync(IChangeDetails details)
        {
            if (details.Changes.TryGetValue("Ticket", out var changeSetDetails) &&
                changeSetDetails.Insertions.Length > 0)
            {
                // A new thread has been started - run sentiment analysis.

                try
                {
                    var tickets = changeSetDetails.Insertions
                                                  .Select(i => i.CurrentObject)
                                                  .Select(o => (string)(o.Title + Environment.NewLine + o.Description))
                                                  .ToArray();

                    if (tickets.Length == 0)
                    {
                        return;
                    }

                    Console.WriteLine($"Requesting sentiment score for {tickets.Length} objects...");

                    var sentimentRequest = tickets.Select((text, index) => new SentimentDocument
                    {
                        Id = index.ToString(),
                        Text = text,
                        Language = "en"
                    })
                                                  .Cast<IDocument>()
                                                  .ToList();

                    var sentimentResponse = await _sentimentClient.GetSentimentAsync(new SentimentRequest
                    {
                        Documents = sentimentRequest
                    });

                    foreach (var error in sentimentResponse.Errors)
                    {
                        Console.WriteLine("Error from sentiment API: " + error.Message);
                    }

                    Console.WriteLine($"Requesting key phrases for {tickets.Length} objects...");

                    var keyPhraseRequest = tickets.Select((text, index) => new KeyPhraseDocument
                    {
                        Id = index.ToString(),
                        Text = text,
                        Language = "en"
                    })
                                                  .Cast<IDocument>()
                                                  .ToList();

                    var keyPhraseResponse = await _keyPhraseClient.GetKeyPhrasesAsync(new KeyPhraseRequest
                    {
                        Documents = keyPhraseRequest
                    });

                    foreach (var error in keyPhraseResponse.Errors)
                    {
                        Console.WriteLine("Error from KeyPhrase API: " + error.Message);
                    }

                    var keyPhraseDictionary = keyPhraseResponse.Documents.ToDictionary(d => d.Id, d => d.KeyPhrases);

                    var toUpdate = sentimentResponse.Documents
                                           .Select(doc =>
                                           {
                                               var obj = changeSetDetails.Insertions[int.Parse(doc.Id)].CurrentObject;

                                               if (!keyPhraseDictionary.TryGetValue(doc.Id, out var keyPhrases) ||
                                                   keyPhrases == null)
                                               {
                                                   keyPhrases = new List<string> { "Unknown" };
                                               }

                                               Console.WriteLine("------------------");
                                               Console.WriteLine($"Analyzed: {obj.Title}");
                                               Console.WriteLine($"Score: {doc.Score}");
                                               Console.WriteLine($"KeyPhrases: {string.Join(", ", keyPhrases)}");
                                               Console.WriteLine("------------------");

                                               return new
                                               {
                                                   Score = doc.Score,
                                                   Reference = ThreadSafeReference.Create(obj),
                                                   KeyPhrases = keyPhrases
                                               };
                                           })
                                           .ToArray();

                    using (var realm = details.GetRealmForWriting())
                    {
                        var resolved = toUpdate.Select(t => new
                        {
                            Score = t.Score,
                            Object = realm.ResolveReference(t.Reference),
                            KeyPhrases = t.KeyPhrases
                        })
                                               .ToArray();

                        realm.Write(() =>
                        {
                            foreach (var item in resolved)
                            {
                                item.Object.Score = item.Score;
                                item.Object.Tags = string.Join(" ", item.KeyPhrases);
                            }
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }
    }
}
