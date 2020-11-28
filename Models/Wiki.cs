﻿namespace BooruDex.Models
{
	/// <summary>
	/// Represent Wiki entry. 
	/// </summary>
	public struct Wiki
	{
		/// <summary>
		/// Create a instance of <see cref="Wiki"/>.
		/// </summary>
		/// <param name="id">The ID of the wiki entry.</param>
		/// <param name="title">The name of the wiki</param>
		/// <param name="body">The wiki description.</param>
		public Wiki(uint id, string title, string body)
		{
			this.ID = id;
			this.Title = title;
			this.Body = body;
		}

		/// <summary>
		/// Gets the ID of the wiki entry.
		/// </summary>
		public uint ID { private set; get; }

		/// <summary>
		/// Gets the name of the described tag.
		/// </summary>
		public string Title { private set; get; }

		/// <summary>
		/// Gets the tag description.
		/// </summary>
		public string Body { private set; get; }

		/// <inheritdoc/>
		public override string ToString()
		{
			return this.Title;
		}
	}
}
