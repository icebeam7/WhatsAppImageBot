using Twilio.TwiML;

namespace ImageBot.Services
{
    public class TwilioService
    {
        public static string GetTwilioMessage(string message)
        {
            var response = new MessagingResponse().Message(message);

            var twiml = response.ToString();
            twiml = twiml.Replace("utf-16", "utf-8");

            return twiml;
        }
    }
}
