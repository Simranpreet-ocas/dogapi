﻿namespace DogApi.Endpoints.Authentication.Models
{
    public class User
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public string Role { get; set; }
    }
}
