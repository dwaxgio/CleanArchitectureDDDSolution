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
    }
}
