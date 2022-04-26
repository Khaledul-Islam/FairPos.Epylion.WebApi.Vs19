using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FairPos.Epylion.Models
{
    public class UsersWeb
    {
        [Required]
        [JsonPropertyName("username")]
        public string UserId { get; set; }

        //[Required]
        [JsonPropertyName("password")]
        public string Password { get; set; }


        public string Address { get; set; }
        public string Email { get; set; }
        public bool isActive { get; set; }
        public string UserType { get; set; }
        public string EmailUserId { get; set; }
        public string EmailPassword { get; set; }
        public decimal? EmpId { get; set; }
        public string Designation { get; set; }

        [JsonPropertyName("CounterId")]
        [FIK.DAL.FIK_NoCUD]
        public int CounterId { get; set; }

        [FIK.DAL.FIK_NoCUD]
        public List<UsersShop> usersShop { get; set; }

        [FIK.DAL.FIK_NoCUD]
        public int RecordFilter { get; set; }
        [FIK.DAL.FIK_NoCUD]
        public int RecordCount { get; set; }

        [JsonPropertyName("role")]
        [FIK.DAL.FIK_NoCUD]
        public string Role { get; set; }

        [JsonPropertyName("originalUserName")]
        [FIK.DAL.FIK_NoCUD]
        public string OriginalUserName { get; set; }

        [JsonPropertyName("accessToken")]
        [FIK.DAL.FIK_NoCUD]
        public string AccessToken { get; set; }

        [JsonPropertyName("refreshToken")]
        [FIK.DAL.FIK_NoCUD]
        public string RefreshToken { get; set; }

    }

    public class RefreshTokenRequest
    {
        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }
        public string accessToken { get; set; }
    }
}
