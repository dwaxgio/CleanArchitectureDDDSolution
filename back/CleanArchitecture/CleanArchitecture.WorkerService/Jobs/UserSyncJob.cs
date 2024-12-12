using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.WorkerService.Jobs
{
    public class UserSyncJob
    {
        private readonly IUserRepository _repository;

        public UserSyncJob(IUserRepository repository) => _repository = repository;

        public async Task SyncUsers()
        {
            using var client = new HttpClient();
            var response = await client.GetFromJsonAsync<ApiResponse>("https://randomuser.me/api/?results=10");
            var users = response.Results.Select(r => new User
            {
                Name = $"{r.Name.First} {r.Name.Last}",
                Country = r.Location.Country,
                City = r.Location.City,
                Email = r.Email,
                PictureUrl = r.Picture.Large
            }).ToList();
            await _repository.UpsertUsersAsync(users);
        }

        public class ApiResponse { 
            public List<Result> Results { get; set; } 
        }
        public class Result { 
            public Name Name { get; set; } 
            public Location Location { get; set; } 
            public string Email { get; set; } 
            public Picture Picture { get; set; } 
        }
        public class Name { 
            public string First { get; set; } 
            public string Last { get; set; } 
        }
        public class Location { 
            public string Country { get; set; } 
            public string City { get; set; } 
        }
        public class Picture { 
            public string Large { get; set; } 
        }
    }
}
