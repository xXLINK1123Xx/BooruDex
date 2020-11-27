﻿using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BooruDex2.Models
{
	/// <summary>
	/// Represents a Artist object.
	/// </summary>
	readonly public struct Artist
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"><see cref="Artist"/> id.</param>
		/// <param name="name"><see cref="Artist"/> name.</param>
		/// <param name="urls">List of <see cref="Artist"/> urls.</param>
		public Artist(uint id, string name, IList<string> urls)
		{
			this.ID = id;
			this.Name = name;
			this.Urls = new ReadOnlyCollection<string>(urls);
		}

		/// <summary>
		/// Gets the ID of the artist.
		/// </summary>
		public uint ID { get; }

		/// <summary>
		/// Gets the name of the artist.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// List of <see cref="Artist"/> official url.
		/// </summary>
		public ReadOnlyCollection<string> Urls { get; }

		/// <inheritdoc/>
		public override string ToString()
		{
			return this.Name;
		}
	}
}
