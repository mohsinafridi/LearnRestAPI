using Microsoft.ML.Data;

namespace Movies.Recommender;

public class MovieRating
{
    [LoadColumn(0)]
    public float userId;  // Feature
    [LoadColumn(1)] 
    public float movieId;  // Feature
    [LoadColumn(2)]
    public float Label;   // Label
}


public class MovieRatingPrediction
{
    public float Label;
    public float Score;
}