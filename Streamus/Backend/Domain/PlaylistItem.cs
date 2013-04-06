﻿using FluentValidation;
using Streamus.Backend.Domain.Validators;
using System;
using System.Runtime.Serialization;

namespace Streamus.Backend.Domain
{
    [DataContract]
    public class PlaylistItem
    {
        [DataMember(Name = "playlistId")]
        public Guid PlaylistId { get; set; }

        [DataMember(Name = "id")]
        public Guid Id { get; set; }

        [DataMember(Name = "nextItemId")]
        public Guid NextItemId { get; set; }

        [DataMember(Name = "previousItemId")]
        public Guid PreviousItemId { get; set; }

        //  Store Title on PlaylistItem as well as on Video because user might want to rename PlaylistItem.
        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "video")]
        public Video Video { get; set; }

        public PlaylistItem()
        {
            //  Id shall be generated by the client. This is OK because it is composite key with 
            //  PlaylistId which is generated by the server. 
            Id = Guid.Empty;
            PlaylistId = Guid.Empty;
            NextItemId = Guid.Empty;
            PreviousItemId = Guid.Empty;
            Title = string.Empty;
        }

        public PlaylistItem(Guid playlistId, Guid id, string title, Video video)
            : this()
        {
            PlaylistId = playlistId;
            Id = id;
            Title = title;
            Video = video;
        }

        public void ValidateAndThrow()
        {
            var validator = new PlaylistItemValidator();
            validator.ValidateAndThrow(this);
        }

        private int? _oldHashCode;
        public override int GetHashCode()
        {
            // Once we have a hash code we'll never change it
            if (_oldHashCode.HasValue)
                return _oldHashCode.Value;

            bool thisIsTransient = Equals(Id, Guid.Empty);

            // When this instance is transient, we use the base GetHashCode()
            // and remember it, so an instance can NEVER change its hash code.
            if (thisIsTransient)
            {
                _oldHashCode = base.GetHashCode();
                return _oldHashCode.Value;
            }
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            PlaylistItem other = obj as PlaylistItem;
            if (other == null)
                return false;

            // handle the case of comparing two NEW objects
            bool otherIsTransient = Equals(other.Id, Guid.Empty);
            bool thisIsTransient = Equals(Id, Guid.Empty);
            if (otherIsTransient && thisIsTransient)
                return ReferenceEquals(other, this);

            return other.Id.Equals(Id);
        }
    }
}