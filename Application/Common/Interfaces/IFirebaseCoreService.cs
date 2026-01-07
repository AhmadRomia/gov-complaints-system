using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Notifier.Core.Firebase;

public interface IFirebaseCoreService
{

    public Task<bool> SendNotificationToTopic(string topic, string title, string body);
    Task<bool> SendNotificationToMultiTopics(List<string> topics, string title, string body);

    public Task<bool> SendNotificationAndDataToMultiTopics(List<string> topic, string title, string body, Dictionary<string, string> data);

    public Task<bool> SendNotificationAndDataToTopic(string topic, string title, string body, Dictionary<string, string> data);


    public Task<bool> SendDataToMultiTopic(List<string> topic, Dictionary<string, string> data);

    public Task SubscribeToTopic(string topic, string? FCMTokens);

    Task UnsubscribeToTopic(string topic, List<string> FCMTokens);
}
