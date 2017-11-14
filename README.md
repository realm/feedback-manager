# Feedback Manager

This is a very simple demo, showcasing the use of the Realm Global Notifier to observe
for changes made to a Realm file and query the Azure Text Analytics API to obtain a
sentiment score and key phrases for a support ticket.

## Tutorial

This project is accompanying the [announcement blog post](https://blog.realm.io/announcing-realm-dotnet/) that includes a step-by-step
tutorial for building a simplified version of the feedback handler.

## Getting started

The demo contains three major products:
1. Feedback app (iOS and Android) that a customer can use to provide feedback/create support tickets.
2. A Receiver app (iOS and Android) that an employee can use to reply to those tickets
3. A .NET Core Server app which handles changes to the `/feedback` Realm, queries the Text Analytics API and writes a score for the ticket sentiment.

To get started, open with Visual Studio and substitute:
1. `Server.Constants.SentimentApiKey` with the API key for the Text Analytics API.
1. `Server.Constants.FeatureToken` with your Professional or Enterprise feature token for the Realm Object Server.
1. Build the solution.

## Credits

### License

Distributed under the Apache 2.0 license. See [LICENSE](../LICENSE) for more information.

### About

<img src="assets/realm.png" width="184" />

The names and logos for Realm are trademarks of Realm Inc.

We :heart: open source software!

See [our other open source projects](https://realm.github.io), check out [the Realm Academy](https://academy.realm.io), or say hi on twitter ([@realm](https://twitter.com/realm)).
