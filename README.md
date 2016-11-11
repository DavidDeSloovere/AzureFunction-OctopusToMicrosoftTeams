# AzureFunction-OctopusToMicrosoftTeams
Code for an Azure Function that:
- accepts a payload sent by Octopus Deploy (via subscriptions) and 
- posts information to a Microsoft Teams channel (0365 web hook).

Inspiration found at
- https://www.troyhunt.com/azure-functions-in-practice/ - great use-case of Azure Function
- https://octopus.com/blog/subscriptions - Octopus to Slack via Zapier

The Azure Function was started from the Generic WebHook C# template.
Initially I coded the function directly in the Azure Portal, the feedback cycle is fast, the online editor is good enough for small things.
When the code was complete, I set up continuous delivery. So every commit to the master will automatically update the code in Azure.
