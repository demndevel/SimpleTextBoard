namespace Demnoboard;

public static class SendToTelegram
{
    public static void SendPost(int id, string title, string text, string token, string chatId)
    {
        var client = new System.Net.WebClient();

        string result = $"*New post!*\n*Link*: https://board.demns.space/Posts/Post?id={id}\n*Title*: {title}\n*Text*: {text}";
        
        client.DownloadString(string.Format(
            "https://api.telegram.org/bot{0}/sendMessage?chat_id={1}&text={2}&parse_mode=Markdown", 
            token, 
            chatId,
            result));
    }
    
    public static void SendReply(int id, string title, string text, string token, string chatId)
    {
        var client = new System.Net.WebClient();

        string result = $"*Reply to post!*\n*Link*: https://board.demns.space/Posts/Post?id={id}\n*Title*: {title}\n*Text*: {text}";
        
        client.DownloadString(string.Format(
            "https://api.telegram.org/bot{0}/sendMessage?chat_id={1}&text={2}&parse_mode=Markdown", 
            token, 
            chatId,
            result));
    }
}