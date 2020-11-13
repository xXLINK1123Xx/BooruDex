﻿namespace BooruDex.Models
{
    /// <summary>
    /// Represent a Tag Related object.
    /// </summary>
    public struct TagRelated
    {
        /// <summary>
        /// Create a instance of <see cref="TagRelated"/>
        /// </summary>
        /// <param name="name">The name of the tag.</param>
        /// <param name="count">The number of occurences of the tag.</param>
        public TagRelated(string name, uint count)
        {
            this.Name = name;
            this.Count = count;
        }

        /// <summary>
        /// Gets the name of the tag.
        /// </summary>
        public string Name { private set; get; }

        /// <summary>
        /// Gets the number of occurences of the tag.
        /// </summary>
        public uint Count { private set; get; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
