# AzureFunction-OctopusToMicrosoftTeams
Code for an Azure Function that:
- accepts a payload sent by Octopus Deploy (via subscriptions) and 
- posts information to a Microsoft Teams channel (0365 web hook).

Inspiration found at
- https://www.troyhunt.com/azure-functions-in-practice/ - great use-case of Azure Function
- https://octopus.com/blog/subscriptions - Octopus to Slack via Zapier

The Azure Function was started from the Generic WebHook C# template.
