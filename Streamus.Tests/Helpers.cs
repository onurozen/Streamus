﻿using Streamus.Domain;
using Streamus.Domain.Managers;
using System;

namespace Streamus.Tests
{
    /// <summary>
    ///     Stores common methods used by tests. Just useful for keeping things DRY between test cases.
    /// </summary>
    public static class Helpers
    {
        private static readonly PlaylistItemManager PlaylistItemManager = new PlaylistItemManager();

        /// <summary>
        ///     Creates a new Video and PlaylistItem, puts item in the database and then returns
        ///     the item. Just a nice utility method to keep things DRY.
        /// </summary>
        public static PlaylistItem CreateItemInPlaylist(Playlist playlist)
        {
            Video videoNotInDatabase = CreateUnsavedVideoWithId();

            //  Create a new PlaylistItem and write it to the database.
            string title = videoNotInDatabase.Title;
            var playlistItem = new PlaylistItem(title, videoNotInDatabase);

            playlist.AddItem(playlistItem);
            PlaylistItemManager.Save(playlistItem);

            return playlistItem;
        }

        /// <summary>
        ///     Creates a new Video with a random Id, or a given Id if specified, saves it to the database and returns it.
        /// </summary>
        public static Video CreateUnsavedVideoWithId(string idOverride = "", string titleOverride = "")
        {
            //  Create a random video ID to ensure the Video doesn't exist in the database currently.
            string randomVideoId = idOverride == string.Empty ? Guid.NewGuid().ToString().Substring(0, 11) : idOverride;
            string title = titleOverride == string.Empty ? string.Format("Video {0}", randomVideoId) : titleOverride;
            var video = new Video(randomVideoId, title, 999, "Author");

            return video;
        }
    }
}
