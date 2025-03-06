namespace LaserAPI.Models.Helper
{
    public class GenresSorter
    {
        public string Genre { get; set; }
        public int Occurrences { get; set; }

        public GenresSorter(string genre, int occurrences)
        {
            Genre = genre;
            Occurrences = occurrences;
        }
    }
}
