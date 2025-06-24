using Movies.Application.Models;
using Newtonsoft.Json;

namespace Movies.Recommender;

public class Program
{
    public async static void Main(string[] args)
    {
        var movieResponse = await GetMoviesAsync();

        

    }



    public async static  Task<List<Movie>> GetMoviesAsync()
    {
        using HttpClient  httpClient = new HttpClient();
        var response = await httpClient.GetAsync("/api/movies");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<List<Movie>>(json);
    }

}
