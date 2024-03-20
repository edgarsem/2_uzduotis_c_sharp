namespace _2_uzduotis_c_sharp.Models
{
    public class User
    {
        public string Id { get; set; }
        public GitHubUser GitHubProfile { get; set; }
        public GoogleUser GoogleProfile { get; set; }
    }
}
