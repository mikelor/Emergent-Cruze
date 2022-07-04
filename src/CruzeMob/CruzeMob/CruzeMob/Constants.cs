using Xamarin.Forms;

namespace CruzeMob
{
    public static class Constants
    {
        // URL of REST service
        public static string RestUrl = Device.RuntimePlatform == Device.Android ? "https://cruze-test-api-westus2.azurewebsites.net{0}" : "https://cruze-test-api-westus2.azurewebsites.net{0}";
    }
}