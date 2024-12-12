using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CleanArchitecture.WorkerService.Jobs
{
    public class UserSyncJob
    {
        private readonly IUserRepository _repository;

        public UserSyncJob(IUserRepository repository) => _repository = repository;

        public async Task SyncUsers()
        {
            try
            {
                Console.WriteLine("Starting SyncUsers job...");

                using var client = new HttpClient();
                var response = await client.GetFromJsonAsync<ApiResponse>("https://randomuser.me/api/?results=10");

                if (response != null && response.Results != null)
                {
                    var users = response.Results.Select(r => new User
                    {
                        Name = $"{r.Name.First} {r.Name.Last}",
                        Country = r.Location.Country,
                        City = r.Location.City,
                        Email = r.Email,
                        PictureUrl = r.Picture.Large
                    }).ToList();

                    Console.WriteLine($"Fetched {users.Count} users from API.");
                    await _repository.UpsertUsersAsync(users);
                    Console.WriteLine("Users successfully saved to the database.");
                }
                else
                {
                    Console.WriteLine("No data fetched from the API.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SyncUsers job: {ex.Message}");
            }
        }

        // Define the classes needed to deserialize the JSON response from the API
        public class ApiResponse
        {
            public List<Result> Results { get; set; }
        }

        public class Result
        {
            public Name Name { get; set; }
            public Location Location { get; set; }
            public string Email { get; set; }
            public Picture Picture { get; set; }
        }

        public class Name
        {
            public string First { get; set; }
            public string Last { get; set; }
        }

        public class Location
        {
            public string Country { get; set; }
            public string City { get; set; }
        }

        public class Picture
        {
            public string Large { get; set; }
        }
    }
}
