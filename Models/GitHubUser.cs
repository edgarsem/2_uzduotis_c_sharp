namespace _2_uzduotis_c_sharp.Models
{
    public class GitHubUser
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Picture { get; set; }
        public string Type { get; set; }
        public int PublicRepos { get; set; }
        public string Created { get; set; }
    }
}
/*
"{
\"login\":\"edgarsem\",
\"id\":90824295,
\"node_id\":\"MDQ6VXNlcjkwODI0Mjk1\",
\"avatar_url\":\"https://avatars.githubusercontent.com/u/90824295?v=4\",
\"gravatar_id\":\"\",\"url\":\"https://api.github.com/users/edgarsem\",
\"html_url\":\"https://github.com/edgarsem\",
\"followers_url\":\"https://api.github.com/users/edgarsem/followers\",
\"following_url\":\"https://api.github.com/users/edgarsem/following{/other_user}\",
\"gists_url\":\"https://api.github.com/users/edgarsem/gists{/gist_id}\",
\"starred_url\":\"https://api.github.com/users/edgarsem/starred{/owner}{/repo}\",
\"subscriptions_url\":\"https://api.github.com/users/edgarsem/subscriptions\",
\"organizations_url\":\"https://api.github.com/users/edgarsem/orgs\",
\"repos_url\":\"https://api.github.com/users/edgarsem/repos\",
\"events_url\":\"https://api.github.com/users/edgarsem/events{/privacy}\",
\"received_events_url\":\"https://api.github.com/users/edgarsem/received_events\",
\"type\":\"User\",
\"site_admin\":false,
\"name\":null,
\"company\":null,
\"blog\":\"\",
\"location\":null,
\"email\":null,
\"hireable\":null,
\"bio\":null,
\"twitter_username\":null,
\"public_repos\":18,
\"public_gists\":0,
\"followers\":0,
\"following\":0,
\"created_at\":\"2021-09-16T06:41:11Z\",
\"updated_at\":\"2024-03-02T18:59:13Z\"}"*/