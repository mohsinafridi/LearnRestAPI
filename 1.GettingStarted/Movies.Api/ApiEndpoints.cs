namespace Movies.Api;

public static class ApiEndpoints
{
    private const string ApiBase = "api";


    public static class Movies
    {
        private const string Base = $"{ApiBase}/movies";   // "api/movies"
        
        
        public const string Create = $"{Base}";
        public const string Get = $"{Base}/{{idOrSlug}}"; // "api/movies/{id}"
        public const string GetAll = $"{Base}"; // "api/movies"
        public const string Update = $"{Base}/{{id:guid}}"; // "api/movies/{id}"
        public const string Delete = $"{Base}/{{id:guid}}"; // "api/movies/{id}"

        public const string Rate = $"{Base}/{{id:guid}}/ratings"; // "api/movies/{id}/ratings"
        public const string DeleteRating = $"{Base}/{{id:guid}}/ratings"; // "api/movies/{id}/ratings"


    }

    public static class Ratings
    {
        public const string Base = $"{ApiBase}/ratings";

        public const string GetUserRatings = $"{Base}/me"; // "api/ratings/user"
    }   
}
