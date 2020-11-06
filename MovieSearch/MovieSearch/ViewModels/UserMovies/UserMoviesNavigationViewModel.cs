namespace MovieSearch.ViewModels.UserMovies
{
    public class UserMoviesNavigationViewModel
    {
        public string Index => "Favourites";

        public string ViewedMovies => "ViewedMovies";

        public string UserId { get; set; }

        public string ActivePage { get; set; }
    }
}
