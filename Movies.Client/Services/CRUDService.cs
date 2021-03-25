using System;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Movies.Client.Models;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Net.Http.Headers;
using System.Text;

namespace Movies.Client.Services
{
    public class CRUDService : IIntegrationService
    {

        private static HttpClient _httpClient = new HttpClient();

        public CRUDService()
        {
            _httpClient.BaseAddress = new Uri($"http://localhost:57863");
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
            _httpClient.DefaultRequestHeaders.Clear();
            //_httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //_httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml", 0.9));
        }

        public async Task Run()
        {
            //await GetResource();
            //await GetResourceUsingHttpRequestMessage();

            await CreateResourceUsingHttpRequestMessage();
            //await PostResource();

            //await MovietoUpdateUsingHttpRequestMessage();
            //await PutResponse();

            //await DeleteAMovieUsingHttpRequestMessage();
            //await DeleteResource();
        }

    //======================================= GET ====================================================
        //GET : Way 1
        public async Task GetResource()
        {
            var response = await _httpClient.GetAsync("api/movies");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine(response.StatusCode);
            var movies = new List<Movie>();
     
            if (response.Content.Headers.ContentType.MediaType == "application/json")
                movies = JsonConvert.DeserializeObject<List<Movie>>(content);
            else if (response.Content.Headers.ContentType.MediaType == "application/xml")
            {
                var serializer = new XmlSerializer(typeof(List<Movie>));
                movies = (List<Movie>)serializer.Deserialize(new StringReader(content));
            }
        }

        //GET : Way 2
        public async Task GetResourceUsingHttpRequestMessage()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/movies");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var movies = JsonConvert.DeserializeObject<List<Movie>>(content);
        }

        //======================================= POST ====================================================
        public async Task CreateResourceUsingHttpRequestMessage()
        {
            var movieToCreate = new MovieForCreation()
            {
                Title = "Reservoir Dogs",
                Description = "After a simple jewelry heist goes terribly wrong, the " +
                 "surviving criminals begin to suspect that one of them is a police informant.",
                DirectorId = Guid.Parse("d28888e9-2ba9-473a-a40f-e38cb54f9b35"),
                ReleaseDate = new DateTimeOffset(new DateTime(1992, 9, 2)),
                Genre = "Crime, Drama"
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "api/movies");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var parsedMovie = JsonConvert.SerializeObject(movieToCreate);
            request.Content = new StringContent(parsedMovie);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            Console.WriteLine(response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var createdMovie = JsonConvert.DeserializeObject<Movie>(content);
        }

        public async Task PostResource()
        {
            var movieToCreate = new MovieForCreation()
            {
                Title = "Reservoir Dogs",
                Description = "After a simple jewelry heist goes terribly wrong, the " +
                             "surviving criminals begin to suspect that one of them is a police informant.",
                DirectorId = Guid.Parse("d28888e9-2ba9-473a-a40f-e38cb54f9b35"),
                ReleaseDate = new DateTimeOffset(new DateTime(1992, 9, 2)),
                Genre = "Crime, Drama"
            };

            var parsedMovie = JsonConvert.SerializeObject(movieToCreate);
            var response = await _httpClient.PostAsync("api/movies", new StringContent(parsedMovie,Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var movieCreated = JsonConvert.DeserializeObject<Movie>(content);
        }

        //======================================= PUT ====================================================
        public async Task MovietoUpdateUsingHttpRequestMessage()
        {
            var movieToUpdate = new MovieForUpdate()
            {
                Title = "Pulp Fiction",
                Description = "The movie with Zed.",
                DirectorId = Guid.Parse("d28888e9-2ba9-473a-a40f-e38cb54f9b35"),
                ReleaseDate = new DateTimeOffset(new DateTime(1992, 9, 2)),
                Genre = "Crime, Drama"
            };

            var request = new HttpRequestMessage(HttpMethod.Put, "api/movies/5b1c2b4d-48c7-402a-80c3-cc796ad49c6b");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var parsedMovie = JsonConvert.SerializeObject(movieToUpdate);
            request.Content = new StringContent(parsedMovie);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var updatedMovie = JsonConvert.DeserializeObject<Movie>(content);
        }

        public async Task PutResponse()
        {
            var movieToUpdate = new MovieForUpdate()
            {
                Title = "Pulp Fiction",
                Description = "The movie with Zed.",
                DirectorId = Guid.Parse("d28888e9-2ba9-473a-a40f-e38cb54f9b35"),
                ReleaseDate = new DateTimeOffset(new DateTime(1992, 9, 2)),
                Genre = "Crime, Drama"
            };

            var parsedMovie = JsonConvert.SerializeObject(movieToUpdate);
            var response = await _httpClient.PutAsync("api/movies/5b1c2b4d-48c7-402a-80c3-cc796ad49c6b",
                                                    new StringContent(parsedMovie, Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var movieUpdated = JsonConvert.DeserializeObject<Movie>(content);
        }

        //======================================= DELETE ====================================================
        public async Task DeleteAMovieUsingHttpRequestMessage()
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, "api/movies/5b1c2b4d-48c7-402a-80c3-cc796ad49c6b");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
        }

        public async Task DeleteResource()
        {
            var response = await _httpClient.DeleteAsync("api/movies/5b1c2b4d-48c7-402a-80c3-cc796ad49c6b");
            response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync();            
        }
    }
}
