
using FirebaseAdmin.Messaging;
using System.Diagnostics;


namespace Application.Notifier.Core.Firebase;

public class FirebaseCoreService : IFirebaseCoreService
{

    public async Task<bool> SendNotificationToTopic(string topic, string title, string body)
    {
        try
        {
            var messageObject = new Message()
            {
                Topic = topic,
                Notification = new Notification()
                {
                    Title = title,
                    Body = body,
                }
            };
            string response = await FirebaseMessaging.DefaultInstance.SendAsync(messageObject);
         
            return true;
        }
        catch (Exception ex)
        {
          
            return false;
        }
    }

    public async Task<bool> SendNotificationToMultiTopics(List<string> topics, string title, string body)
    {
        try
        {
            var messages = topics.Select(x => new Message()
            {
                Topic = x,
                Notification = new Notification()
                {
                    Title = title,
                    Body = body,
                }
            });
            var response = await FirebaseMessaging.DefaultInstance.SendEachAsync(messages);

            return true;
        }
        catch (Exception ex)
        {

            return false;
        }
    }

    public async Task<bool> SendDataToMultiTopic(List<string> topic, Dictionary<string, string> data)
    {
        try
        {
            var messages = topic.Select(x => new Message()
            {
                Topic = x,
                Data = data
            }).ToList();
            BatchResponse response = await FirebaseMessaging.DefaultInstance.SendEachAsync(messages);
           
            Debug.WriteLine("Successfully sent message: " + response);
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception Occurred: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> SendNotificationAndDataToMultiTopics(List<string> topic, string title, string body, Dictionary<string, string> data)
    {
        try
        {
            var messages = topic.Select(x=> new Message()
            {
                Data = data,
                Topic = x,
                Notification = new Notification()
                {
                    Title = title,
                    Body = body,
                }
            });
            var response = await FirebaseMessaging.DefaultInstance.SendEachAsync(messages);
            Debug.WriteLine("Successfully sent message: " + response);
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception Occurred: {ex.Message}");
            return false;
        }
    }

    public async Task SubscribeToTopic(string topic, string? registrationToken)
    {
        if (registrationToken == null)
            return;
        IReadOnlyList<string> registrationTokens = [registrationToken];
   
        await FirebaseMessaging.DefaultInstance.SubscribeToTopicAsync(registrationTokens, topic);
      
    }

    public async Task UnsubscribeToTopic(string topic, List<string> registrationTokens)
    {
    
        await FirebaseMessaging.DefaultInstance.UnsubscribeFromTopicAsync(registrationTokens,topic);

    }


    public async Task<bool> SendNotificationAndDataToTopic(string topic, string title, string body, Dictionary<string, string> data)
    {
        try
        {
            var messages =  new Message()
            {
                Data = data,
                Topic = topic,
                Notification = new Notification()
                {
                    Title = title,
                    Body = body,
                }
            };
            string response = await FirebaseMessaging.DefaultInstance.SendAsync(messages);
            Debug.WriteLine("Successfully sent message: " + response);
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception Occurred: {ex.Message}");
            return false;
        }
    }
}
