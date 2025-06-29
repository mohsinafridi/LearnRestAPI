﻿using Microsoft.ML;
using Microsoft.ML.Trainers;
// using MovieRecommendation;
namespace Movies.Recommender;


public class Program111
{

    public static void Main(string[] args)
    {
        MLContext mlContext = new MLContext();

        // Load Data
        (IDataView trainingDataView, IDataView testDataView) = LoadData(mlContext);

        // Build & train model
        
        ITransformer model = BuildAndTrainModel(mlContext, trainingDataView);

        EvaluateModel(mlContext, testDataView, model);


        UseModelForSinglePrediction(mlContext, model);



        SaveModel(mlContext, trainingDataView.Schema, model);
       
    }


    public static (IDataView training, IDataView test) LoadData(MLContext mlContext)
    {
        // Load training & test datasets using datapaths    
        var trainingDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "recommendation-ratings-train.csv");
        var testDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "recommendation-ratings-test.csv");

        IDataView trainingDataView = mlContext.Data.LoadFromTextFile<MovieRating>(trainingDataPath, hasHeader: true, separatorChar: ',');
        IDataView testDataView = mlContext.Data.LoadFromTextFile<MovieRating>(testDataPath, hasHeader: true, separatorChar: ',');

        return (trainingDataView, testDataView);
       
    }

    public static ITransformer BuildAndTrainModel(MLContext mlContext, IDataView trainingDataView)
    {
        // Add data transformations
     
        IEstimator<ITransformer> estimator = mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "userIdEncoded", inputColumnName: "userId")
            .Append(mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "movieIdEncoded", inputColumnName: "movieId"));
       

       
        var options = new MatrixFactorizationTrainer.Options
        {
            MatrixColumnIndexColumnName = "userIdEncoded",
            MatrixRowIndexColumnName = "movieIdEncoded",
            LabelColumnName = "Label",
            NumberOfIterations = 20,
            ApproximationRank = 100
        };

        var trainerEstimator = estimator.Append(mlContext.Recommendation().Trainers.MatrixFactorization(options));
      

        
        Console.WriteLine("=============== Training the model ===============");
        ITransformer model = trainerEstimator.Fit(trainingDataView);

        return model;
    
    }

    public static void EvaluateModel(MLContext mlContext, IDataView testDataView, ITransformer model)
    {
        // Evaluate model on test data & print evaluation metrics
      
        Console.WriteLine("=============== Evaluating the model ===============");
        var prediction = model.Transform(testDataView);
     

    
        var metrics = mlContext.Regression.Evaluate(prediction, labelColumnName: "Label", scoreColumnName: "Score");


  
        Console.WriteLine("Root Mean Squared Error : " + metrics.RootMeanSquaredError.ToString());
        Console.WriteLine("RSquared: " + metrics.RSquared.ToString());
       
    }

    public static void UseModelForSinglePrediction(MLContext mlContext, ITransformer model)
    {
        
        Console.WriteLine("=============== Making a prediction ===============");
        var predictionEngine = mlContext.Model.CreatePredictionEngine<MovieRating, MovieRatingPrediction>(model);
        

        // Create test input & make single prediction
        
        var testInput = new MovieRating { userId = 6, movieId = 10 };

        var movieRatingPrediction = predictionEngine.Predict(testInput);
        

        
        if (Math.Round(movieRatingPrediction.Score, 1) > 3.5)
        {
            Console.WriteLine("Movie " + testInput.movieId + " is recommended for user " + testInput.userId);
        }
        else
        {
            Console.WriteLine("Movie " + testInput.movieId + " is not recommended for user " + testInput.userId);
        }
        
    }

    public static void SaveModel(MLContext mlContext, DataViewSchema trainingDataViewSchema, ITransformer model)
    {
        // Save the trained model to .zip file
 
        var modelPath = Path.Combine(Environment.CurrentDirectory, "Data", "MovieRecommenderModel.zip");

        Console.WriteLine("=============== Saving the model to a file ===============");
        mlContext.Model.Save(model, trainingDataViewSchema, modelPath);
       
    }


}
