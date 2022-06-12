namespace ServerApp.Models
{
    public class UserQueryParams
    {
        public int UserId { get; set; }

        public bool Followers { get; set; } // Kullanıcının takipçileri

        public bool Followings { get; set; } // Kullanıcının takip ettikleri

        public GenderEnum? Gender { get; set; }

        public int minAge { get; set; } = 18;

        public int maxAge { get; set; } = 100;

        public string? Country { get; set; }

        public string? City { get; set; }

        public OrderByEnum? OrderBy { get; set; }
    }
}