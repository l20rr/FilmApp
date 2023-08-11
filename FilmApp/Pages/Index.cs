using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FilmApp.Pages
{
    public partial class Index
    {

        private List<MovieInfo> movies = new List<MovieInfo>();
        private const string image_path = "https://image.tmdb.org/t/p/w500";

        protected override async Task OnInitializedAsync()
        {
            await FetchData();
        }

        private async Task FetchData()
        {
            string QUERY_URL = $"https://api.themoviedb.org/3/movie/popular?api_key=31962af5bef643f17aee7a648f172713&language=en-US&page=1";

            var response = await HttpClient.GetStreamAsync(QUERY_URL);
            var movieApiResponse = await JsonSerializer.DeserializeAsync<MovieApiResponse>(response);

            movies = movieApiResponse.results;
        }

        public class MovieApiResponse
        {
            public List<MovieInfo> results { get; set; }
        }

        public class MovieInfo
        {
            public int id { get; set; }
            public string title { get; set; }
            public string poster_path { get; set; }
        }
    }
}
