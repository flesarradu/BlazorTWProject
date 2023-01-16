using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.ML;
using Microsoft.ML.Data;
using System.Reflection;

namespace BlazorTWProject
{
    public static class AIModel
    {
        public enum Label
        {
            Undefined,
            Bolnav,
            Sanatos
        }
        private static string MLNetModelPath = "AiModelMers.zip";

        public static readonly Lazy<PredictionEngine<ImageInput, ImageOutput>> PredictEngine = new Lazy<PredictionEngine<ImageInput, ImageOutput>>(AIModel.CreatePredictEngine, true);

        public static ImageOutput Predict(ImageInput input)
        {
            var predEngine = PredictEngine.Value;
            return predEngine.Predict(input);
        }

        private static PredictionEngine<ImageInput, ImageOutput> CreatePredictEngine()
        {
            MLContext? mlContext = new MLContext();

            //var thisAssembly = Assembly.GetExecutingAssembly();
            //using var stream = thisAssembly.GetManifestResourceStream(MLNetModelPath) ;
            ITransformer mlModel = mlContext.Model.Load(PathUtilities.GetPathFromBinFolder(MLNetModelPath), out DataViewSchema _);
            return mlContext.Model.CreatePredictionEngine<ImageInput, ImageOutput>(mlModel);
        }
    }
    public class ImageInput
    {

        [LoadColumn(0)]
        [ColumnName(@"Label")]
        public String Label { get; set; }
        public string ImageName { get; set; }
        [LoadColumn(1)]
        [ColumnName(@"ImageSource")]
        public byte[] Image { get; set; }

    }

    public class ImageOutput
    {

        [ColumnName(@"Label")]
        public UInt32 Label { get; set; }

        [ColumnName(@"ImageSource")]
        public byte[] ImageSource { get; set; }

        [ColumnName(@"PredictedLabel")]
        public string PredictedLabel { get; set; }

        [ColumnName(@"Score")]
        public float[] Score { get; set; }
    }
}
