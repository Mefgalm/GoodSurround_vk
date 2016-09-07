using System;

namespace GoodSurround.ApiModels
{
    public class AuthUser
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Photo50 { get; set; }
        public Guid Token { get; set; }
    }
}
