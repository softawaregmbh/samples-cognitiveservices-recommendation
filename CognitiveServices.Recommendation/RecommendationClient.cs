using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CognitiveServices.Recommendation
{
    public class RecommendationClient
    {
        private string modelId;
        private string subscriptionKey;

        public RecommendationClient(string subscriptionKey, string modelId)
        {
            this.modelId = modelId;
            this.subscriptionKey = subscriptionKey;
        }

        public async Task<IEnumerable<Recommendation>> GetItemToItemRecommendation(IEnumerable<string> itemIds, int numberOfResults = 5)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", $"{subscriptionKey}");

            var uri = $"https://westus.api.cognitive.microsoft.com/recommendations/v4.0/models/{modelId}/recommend/item?itemIds={string.Join(",", itemIds)}&numberOfResults={numberOfResults}&minimalScore=0";

            var response = await client.GetAsync(uri);

            var json = JObject.Parse(await response.Content.ReadAsStringAsync());
            var items = json["recommendedItems"].ToString();

            var result = JsonConvert.DeserializeObject<Recommendation[]>(items);
            return result;
        }

        public async Task UploadUsageEvent(string userId, string itemId, RecommendationEventType eventType, DateTime timeStamp)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", $"{subscriptionKey}");

            var uri = $"https://westus.api.cognitive.microsoft.com/recommendations/v4.0/models/{modelId}/usage/events";

            // Build -1 = current default build
            int buildId = -1;

            var body = new
            {
                userId = userId,
                buildId = buildId,
                events = new[]
                {
                    new
                    {
                        eventType = Enum.GetName(typeof(RecommendationEventType), eventType),
                        itemId = itemId,
                        timestamp = timeStamp.ToString("yyyy/MM/ddTHH:mm:ss", CultureInfo.InvariantCulture)
                    }
                }
            };

            HttpResponseMessage response;

            var json = JsonConvert.SerializeObject(body);

            using (var content = new StringContent(json))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = await client.PostAsync(uri, content);
            }
        }
    }
}
