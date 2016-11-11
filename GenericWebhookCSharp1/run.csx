#r "Newtonsoft.Json"

using System;
using System.Configuration;
using System.Net;
using Newtonsoft.Json;

public static async Task<object> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info($"Webhook was triggered!");

    string jsonContent = await req.Content.ReadAsStringAsync();
    log.Info(jsonContent);

    dynamic data = JsonConvert.DeserializeObject(jsonContent);

    if (data.Payload?.Event?.Message == null) {
        return req.CreateResponse(HttpStatusCode.BadRequest, new {
            error = "Payload.Event.Message missing from body."
        });
    }

    var message = data.Payload.Event.Message;
    var occurred = data.Payload.Event.Occurred;

    log.Info("* " + message);
    log.Info("* " + occurred);

    // make call to Teams WebHook Url, which is stored in the app settings
    // https://outlook.office365.com/webhook/cb67..4b/IncomingWebhook/93...
    var appKey = "TeamsWebHookUrl";
    var webHookUrl = ConfigurationManager.AppSettings[appKey];
    log.Info($"App Setting. {appKey}: {webHookUrl}");

    // Payload content at https://dev.outlook.com/Connectors/GetStarted
    var body = new { text = $"{message} {occurred}" };
    using (var client = new HttpClient())
    {
        await client.PostAsJsonAsync(webHookUrl, body);
        log.Info("Sent the JSON payload to Teams WebHook!");
    }

    return req.CreateResponse(HttpStatusCode.OK);
}

/*
// basic one from octopus docs http://docs.octopusdeploy.com/display/OD/Subscriptions
{
  "Timestamp": "0001-01-01T00:00:00+00:00",
  "EventType": "SubscriptionPayload",
  "Payload": {
    "ServerUri": "http://my-octopus.com",
    "ServerAuditUri": "http://my-octopus.com",
    "BatchProcessingDate": "0001-01-01T00:00:00+00:00",
    "Subscription": {},
    "Event": {}
  }
}

// actual one sent webhook
// part of subscription was deleted because it's irrelevant here
{
   "Timestamp":"2016-11-10T22:30:36.2847383Z",
   "EventType":"SubscriptionPayload",
   "Payload":{
      "ServerUri":"http://tempuri.org/",
      "ServerAuditUri":"http://temuri.org//#/configuration/audit?projects=Projects...",
      "BatchProcessingDate":"2016-11-10T22:29:44.8158084+00:00",
      "Subscription":{
      },
      "Event":{
         "Id":"Events-16201",
         "RelatedDocumentIds":[
            "Deployments-6644",
            "Projects-81",
            "Releases-3601",
            "Environments-1"
         ],
         "Category":"DeploymentSucceeded",
         "UserId":"users-system",
         "Username":"system",
         "IdentityEstablishedWith":"Unknown",
         "Occurred":"2016-11-10T22:29:29.7889623+00:00",
         "Message":"Deploy to Production succeeded  for NATCH Sandbox release 1.0.7834.7 to Production",
         "MessageHtml":"<a href='/r/Deployments-6644'>Deploy to Production</a> succeeded  for <a href='/r/Projects-81'>NATCH Sandbox</a> release <a href='/r/Releases-3601'>1.0.7834.7</a> to <a href='/r/Environments-1'>Production</a>",
         "MessageReferences":[
            {
               "ReferencedDocumentId":"Deployments-6644",
               "StartIndex":0,
               "Length":20
            },
            {
               "ReferencedDocumentId":"Projects-81",
               "StartIndex":36,
               "Length":13
            },
            {
               "ReferencedDocumentId":"Releases-3601",
               "StartIndex":58,
               "Length":10
            },
            {
               "ReferencedDocumentId":"Environments-1",
               "StartIndex":72,
               "Length":10
            }
         ],
         "Comments":null,
         "Details":null,
         "Links":{
            "Self":{

            }
         }
      }
   }
}
*/
