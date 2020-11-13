﻿using System;
using System.Linq;
using System.Net.Http;
using System.Security.Authentication;
using System.Threading.Tasks;

using BooruDex.Exceptions;
using BooruDex.Models;

using Litdex.Security.RNG;
using Litdex.Security.RNG.PRNG;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BooruDex.Booru.Template
{
	/// <summary>
	/// Gelbooru beta version 0.2.0.
	/// </summary>
	public abstract class Gelbooru02 : Booru
	{
		#region Constructor & Destructor

		/// <summary>
		/// <see cref="Gelbooru02"/> template for booru client.
		/// </summary>
		/// <param name="domain">URL of booru based sites.</param>
		/// <param name="httpClient">Client for sending and receive http response.</param>
		public Gelbooru02(string domain, HttpClient httpClient = null) : this(domain, httpClient, new JSF32())
		{

		}

		/// <summary>
		/// <see cref="Gelbooru02"/> template for booru client.
		/// </summary>
		/// <param name="domain">URL of booru based sites.</param>
		/// <param name="httpClient">Client for sending and receive http response.</param>
		/// <param name="rng">Random generator for random post.</param>
		public Gelbooru02(string domain, HttpClient httpClient, IRNG rng) : base(domain, httpClient, rng)
		{
			this._PostLimit = 100;
			this._TagsLimit = 0; // no tag limit
			this._PageLimit = 10;
			this.IsSafe = false;
			this._ApiVersion = "";
			this._PasswordSalt = "";
		}

		/// <summary>
		/// Release all resource that this object hold.
		/// </summary>
		~Gelbooru02()
		{

		}

		#endregion Constructor & Destructor

		#region Protected Overrride Method

		protected override string CreateBaseApiCall(string query)
		{
			return $"{ this._BaseUrl.AbsoluteUri }index.php?page=dapi&s={ query }&q=index&json=1";
		}

		#endregion Protected Overrride Method

		#region Protected Virtual Method

		/// <summary>
		/// Read <see cref="Post"/> json search result.
		/// </summary>
		/// <param name="json">Json object.</param>
		/// <returns></returns>
		protected virtual Post ReadPost(object json)
		{
			var item = (JObject)json;
			return new Post(
				item["id"].Value<uint>(),
				new Uri(this._BaseUrl + "index.php?page=post&s=view&id=" + item["id"].Value<int>()),
				new Uri(this._BaseUrl + "/images/3136/" + item["image"].Value<string>()),
				null,
				this.ConvertRating(item["rating"].Value<string>()),
				item["tags"].Value<string>(),
				0,
				item["height"].Value<int>(),
				item["width"].Value<int>(),
				0,
				0,
				""
				);
		}

		#endregion Protected Virtual Method

		#region Public Method

		#region Post

		/// <summary>
		/// Get a list of <see cref="Post"/>.
		/// </summary>
		/// <param name="limit">How many <see cref="Post"/> to retrieve.</param>
		/// <param name="page">The page number.</param>
		/// <param name="tags">The tags to search for.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="AuthenticationException"></exception>
		/// <exception cref="HttpRequestException"></exception>
		/// <exception cref="HttpResponseException"></exception>
		/// <exception cref="SearchNotFoundException"></exception>
		public override async Task<Post[]> PostListAsync(uint limit, string[] tags, uint page = 0)
		{
			if ((this._TagsLimit != 0) &&
				(tags != null) &&
				(tags.Length > this._TagsLimit))
			{
				throw new ArgumentException($"Tag can't more than { this._TagsLimit } tag.");
			}

			if (limit <= 0)
			{
				limit = 1;
			}
			else if (limit > this._PostLimit)
			{
				limit = this._PostLimit;
			}

			string url = "";

			if (tags == null)
			{
				url = this.CreateBaseApiCall("post") +
					$"&limit={ limit }&pid={ page }";
			}
			else
			{
				string tagString = string.Join(" ", tags);
				url = this.CreateBaseApiCall("post") +
					$"&limit={ limit }&pid={ page }&tags={ tagString }";
			}

			var jsonArray = JsonConvert.DeserializeObject<JArray>(
				await this.GetJsonAsync(url));

			try
			{
				return jsonArray.Select(this.ReadPost).ToArray();
			}
			catch
			{
				throw new SearchNotFoundException("No post found.");
			}
		}

		/// <summary>
		/// Search a single random post from booru with the given tags.
		/// </summary>
		/// <param name="tags"><see cref="Tag"/> to search.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="AuthenticationException"></exception>
		/// <exception cref="HttpRequestException"></exception>
		/// <exception cref="HttpResponseException"></exception>
		/// <exception cref="SearchNotFoundException"></exception>
		public override async Task<Post> GetRandomPostAsync(string[] tags = null)
		{
			var post = await this.GetRandomPostAsync(
					this._PostLimit,
					tags);

			if (post.Length == 0)
			{
				throw new SearchNotFoundException("No post found.");
			}

			return post[this._RNG.NextInt(0, (uint)(post.Length - 1))];
		}

		/// <summary>
		/// Search some post from booru with the given tags.
		/// </summary>
		/// <param name="tags"><see cref="Tag"/> to search.</param>
		/// <param name="limit">How many post to retrieve.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="AuthenticationException"></exception>
		/// <exception cref="HttpRequestException"></exception>
		/// <exception cref="HttpResponseException"></exception>
		public override async Task<Post[]> GetRandomPostAsync(uint limit, string[] tags = null)
		{
			return await this.PostListAsync(
				limit,
				tags,
				this._RNG.NextInt(0, this._PageLimit));
		}

		#endregion Post

		#endregion Public Method
	}
}
