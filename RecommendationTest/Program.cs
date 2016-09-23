using CognitiveServices.Recommendation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationTest
{
    class Program
    {
        static void Main(string[] args)
        {
            TestRecommendations().Wait();

            Console.WriteLine("Finished");
        }

        private static async Task TestRecommendations()
        {
            // Create your Cognitive Services Subscription:
            // https://www.microsoft.com/cognitive-services/en-US/subscriptions
            string subscriptionKey = "{INSERT_YOUR_SUBSCRIPTION_KEY}";

            // Create your Recommendations Model:
            // http://recommendations-portal.azurewebsites.net
            // https://westus.dev.cognitive.microsoft.com/docs/services/Recommendations.V4.0/operations/56f30d77eda5650db055a3d6
            string modelKey = "{INSERT_YOUR_MODEL_KEY}";

            var client = new RecommendationClient(subscriptionKey, modelKey);

            // Upload Usage Data
            await client.UploadUsageEvent("test", "398", RecommendationEventType.AddShopCart, DateTime.UtcNow);

            // Get recommendation
            var recommendations = await client.GetItemToItemRecommendation(new[] { "398" });
        }
    }
}
