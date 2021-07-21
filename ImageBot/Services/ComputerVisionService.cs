using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace ImageBot.Services
{
    public class ComputerVisionService
    {
        private static string subscriptionKey = "";
        private static string endpoint = "";

        public static async Task<string> AnalyzeImage(string url)
        {
            var client = Authenticate(endpoint, subscriptionKey);
            var analysis = await AnalyzeImageUrl(client, url);
            return analysis;
        }

        private static ComputerVisionClient Authenticate(string endpoint, string key)
        {
            return new ComputerVisionClient(
                new ApiKeyServiceClientCredentials(key))
            {
                Endpoint = endpoint
            };
        }

        private static async Task<string> AnalyzeImageUrl(ComputerVisionClient client, string imageUrl)
        {
            var features = new List<VisualFeatureTypes?>()
            {
                VisualFeatureTypes.Categories, VisualFeatureTypes.Description,
                VisualFeatureTypes.Faces, VisualFeatureTypes.ImageType,
                VisualFeatureTypes.Tags, VisualFeatureTypes.Adult,
                VisualFeatureTypes.Color, VisualFeatureTypes.Brands,
                VisualFeatureTypes.Objects
            };

            var results = await client.AnalyzeImageAsync(imageUrl, visualFeatures: features);

            var analysis = new StringBuilder();

            analysis.AppendLine("** Summary **");
            foreach (var caption in results.Description.Captions)
                analysis.AppendLine($"{caption.Text} with confidence {caption.Confidence}");

            analysis.AppendLine("** Categories **");
            foreach (var category in results.Categories)
                analysis.AppendLine($"{category.Name} with confidence {category.Score}");

            analysis.AppendLine("** Tags **");
            foreach (var tag in results.Tags)
                analysis.AppendLine($"{tag.Name} {tag.Confidence}");

            analysis.AppendLine("** Objects **");
            foreach (var obj in results.Objects)
                analysis.AppendLine($"{obj.ObjectProperty} with confidence {obj.Confidence}");

            analysis.AppendLine("** Brands **");
            foreach (var brand in results.Brands)
                analysis.AppendLine($"Logo of {brand.Name} with confidence {brand.Confidence}");

            analysis.AppendLine("** Faces **");
            foreach (var face in results.Faces)
                analysis.AppendLine($"A {face.Gender} of age {face.Age}");

            analysis.AppendLine("** Celebrities **");
            foreach (var category in results.Categories)
                if (category.Detail?.Celebrities != null)
                    foreach (var celeb in category.Detail.Celebrities)
                        analysis.AppendLine($"{celeb.Name} with confidence {celeb.Confidence}");

            analysis.AppendLine("** Landmarks **");
            foreach (var category in results.Categories)
                if (category.Detail?.Landmarks != null)
                    foreach (var landmark in category.Detail.Landmarks)
                        analysis.AppendLine($"{landmark.Name} with confidence {landmark.Confidence}");

            return analysis.ToString();
        }

    }
}
