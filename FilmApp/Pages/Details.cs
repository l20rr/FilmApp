using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FilmApp.Pages
{
    public partial class Details
    {

        private const string image_path = "https://image.tmdb.org/t/p/w500";
        private Movie movie;
        private VideoData videoData;

        [Parameter] public string id { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await FetchDataDetails(id);
        }

        private async Task FetchDataDetails(string movieId)
        {
            var response = await HttpClient.GetFromJsonAsync<MovieInfo>($"https://api.themoviedb.org/3/movie/{movieId}?api_key=31962af5bef643f17aee7a648f172713&language=pt-PT&page=1");

            string apiUrl = $"https://api.themoviedb.org/3/movie/{movieId}/videos?api_key=31962af5bef643f17aee7a648f172713&language=en-US";
            var httpClient = new HttpClient();
            var responseVideos = await httpClient.GetAsync(apiUrl);
            if (responseVideos.IsSuccessStatusCode)
            {
                var content = await responseVideos.Content.ReadAsStringAsync();
                videoData = JsonSerializer.Deserialize<VideoData>(content);
            }

            var movieData = new Movie
            {
                Id = movieId,
                Title = response.title,
                Sinopse = response.overview,
                Image = $"{image_path}{response.poster_path}",
                Vote_average = response.vote_average,
                ReleaseDate = response.release_date,
                Genres = response.genres
            };

            movie = movieData;
        }

        public class VideoData
        {
            public List<Video> results { get; set; }
        }

        public class Video
        {
            public string key { get; set; }
            public string name { get; set; }
        }

        private class Movie
        {
            public string Id { get; set; }
            public string Title { get; set; }
            public string Sinopse { get; set; }
            public string Image { get; set; }
            public string ReleaseDate { get; set; }
            public decimal Vote_average { get; set; }
            public List<Genre> Genres { get; set; }
        }

        private class MovieInfo
        {
            public string title { get; set; }
            public string poster_path { get; set; }
            public string overview { get; set; }
            public string release_date { get; set; }
            public decimal vote_average { get; set; }
            public List<Genre> genres { get; set; }
        }

        public class Genre
        {
            public int id { get; set; }
            public string name { get; set; }
        }

        // Método para carregar os vídeos
        private async Task LoadDataAsync()
        {
            string apiUrl = $"https://api.themoviedb.org/3/movie/{id}/videos?api_key=31962af5bef643f17aee7a648f172713&language=en-US";

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                videoData = JsonSerializer.Deserialize<VideoData>(content);
            }
        }
    }
}
