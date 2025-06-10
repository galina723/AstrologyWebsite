using Newtonsoft.Json;

namespace AstrologyWebsite.Models
{
    public class ZodiacCompatibilityResult
    {
        public string Sign1 { get; set; }
        public string Sign2 { get; set; }
        public string OverallCompatibility { get; set; }
        public string PositiveAspects { get; set; }
        public string NegativeAspects { get; set; }
        public string Elements { get; set; }
        public string IdealDate { get; set; }

        public string ScoreGeneral { get; set; }
        public string ScoreCommunication { get; set; }
        public string ScoreSex { get; set; }

        public static ZodiacCompatibilityResult FromApiJson(string json)
        {
            var root = JsonConvert.DeserializeObject<RootObject>(json);
            var prediction = root?.Data?.Prediction;
            return prediction == null ? null : new ZodiacCompatibilityResult
            {
                Sign1 = prediction.Sign_1,
                Sign2 = prediction.Sign_2,
                OverallCompatibility = prediction.Overall_compatibility,
                PositiveAspects = prediction.Positive_aspects,
                NegativeAspects = prediction.Negative_aspects,
                Elements = prediction.Elements,
                IdealDate = prediction.Ideal_date,
                ScoreGeneral = prediction.Score.General,
                ScoreCommunication = prediction.Score.Communication,
                ScoreSex = prediction.Score.Sex
            };
        }

        private class RootObject
        {
            public int Success { get; set; }
            public DataContainer Data { get; set; }
        }

        private class DataContainer
        {
            public Prediction Prediction { get; set; }
        }

        private class Prediction
        {
            public string Sign_1 { get; set; }
            public string Sign_2 { get; set; }
            public string Overall_compatibility { get; set; }
            public string Positive_aspects { get; set; }
            public string Negative_aspects { get; set; }
            public string Elements { get; set; }
            public string Ideal_date { get; set; }
            public Score Score { get; set; }
        }

        private class Score
        {
            public string General { get; set; }
            public string Communication { get; set; }
            public string Sex { get; set; }
        }
    }


}
