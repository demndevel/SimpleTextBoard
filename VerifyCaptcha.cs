using Newtonsoft.Json;

namespace Demnoboard;

public class VerifyCaptcha
{
    public static bool Verify(string recaptchaResponse, string serverKey)
    {
        if (string.IsNullOrEmpty(recaptchaResponse)) return false;

        var client = new System.Net.WebClient();

        if (string.IsNullOrEmpty(serverKey)) return false;

        var googleReply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", serverKey, recaptchaResponse));

        dynamic responseJson = JsonConvert.DeserializeObject(googleReply);

        if (responseJson.success is null)
            return false;
        return responseJson.success;
    }
}