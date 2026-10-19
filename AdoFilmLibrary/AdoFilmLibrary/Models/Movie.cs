namespace AdoFilmLibrary.Models;

public class Movie {
    public int Id { get; set; }
    public string Name { get; set; }
    public int Rating { get; set; }
    public int GenreId { get; set; }
    public string GenreName { get; set; }
    public int YearOfPublication { get; set; }
}