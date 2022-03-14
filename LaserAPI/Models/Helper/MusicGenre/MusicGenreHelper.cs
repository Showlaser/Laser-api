using System;
using System.Collections.Generic;

namespace LaserAPI.Models.Helper.MusicGenre
{
    public static class MusicGenreHelper
    {
        public static Enums.MusicGenre GetMusicGenreFromSpotifyGenre(List<string> spotifyMusicGenres)
        {
            string[] genres = Enum.GetNames(typeof(Enums.MusicGenre));
            int genresLength = genres.Length;
            for (int i = 0; i < genresLength; i++)
            {
                genres[i] = genres[i].ToLower();
            }

            int spotifyMusicGenresLength = spotifyMusicGenres.Count;
            for (int i = 0; i < genresLength; i++)
            {
                string genre = genres[i];
                for (int j = 0; j < spotifyMusicGenresLength; j++)
                {
                    string spotifyMusicGenre = spotifyMusicGenres[j].ToLower();
                    if (spotifyMusicGenre.Contains(genre))
                    {
                        return (Enums.MusicGenre)Enum.Parse(typeof(Enums.MusicGenre), genre);
                    }
                }
            }

            return default;
        }
    }
}
