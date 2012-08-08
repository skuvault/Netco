using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;

namespace Netco.Net
{
	/// <summary>
	/// Submits post data to a url.
	/// </summary>
	/// <seealso href="http://geekswithblogs.net/rakker/archive/2006/04/21/76044.aspx"/>
	/// <example>Example on how to POST
	/// <code>
	/// PostSubmitter post=new PostSubmitter();
	/// post.Url="http://seeker.dice.com/jobsearch/servlet/JobSearch";
	/// post.PostItems.Add("op","100");
	/// post.PostItems.Add("rel_code","1102");
	/// post.PostItems.Add("FREE_TEXT","c# jobs");
	/// post.PostItems.Add("SEARCH","");
	/// post.Type=PostSubmitter.PostTypeEnum.Post;
	/// string result=post.Post();</code></example>
	public class PostSubmitter
	{
		#region Config
		private string _url = string.Empty;
		private NameValueCollection _values = new NameValueCollection();
		private PostTypeEnum _type = PostTypeEnum.Post;

		/// <summary>
		/// Gets or sets a value indicating whether to ignore errors, or throw exception.
		/// </summary>
		/// <value><c>true</c> if errors should be ignored; otherwise, <c>false</c>.</value>
		public bool IgnoreErrors { get; set; }

		/// <summary>
		/// Gets or sets the request timeout.
		/// </summary>
		/// <value>The request timeout.</value>
		/// <remarks><see cref="TimeSpan.MaxValue"/> indicates infinite timeout (never timeouts).</remarks>
		public TimeSpan? Timeout { get; set; }

		/// <summary>
		/// Gets or sets the read/write timeout.
		/// </summary>
		/// <value>The read/write timeout.</value>
		/// <remarks><see cref="TimeSpan.MaxValue"/> indicates infinite timeout (never timeouts).</remarks>
		public TimeSpan? ReadWriteTimeout { get; set; }

		/// <summary>
		/// Default constructor.
		/// </summary>
		public PostSubmitter()
		{
			this.Timeout = TimeSpan.FromSeconds( 100 );
			this.ReadWriteTimeout = TimeSpan.FromMinutes( 5 );
		}

		/// <summary>
		/// Constructor that accepts a url as a parameter
		/// </summary>
		/// <param name="url">The url where the post will be submitted to.</param>
		public PostSubmitter( string url )
			: this()
		{
			this._url = url;
		}

		/// <summary>
		/// Constructor allowing the setting of the url and items to post.
		/// </summary>
		/// <param name="url">the url for the post.</param>
		/// <param name="values">The values for the post.</param>
		public PostSubmitter( string url, NameValueCollection values )
			: this( url )
		{
			this._values = values;
		}

		/// <summary>
		/// Gets or sets the url to submit the post to.
		/// </summary>
		public string Url
		{
			get { return this._url; }
			set { this._url = value; }
		}

		/// <summary>
		/// Gets or sets the name value collection of items to post.
		/// </summary>
		public NameValueCollection PostItems
		{
			get { return this._values; }
			set { this._values = value; }
		}

		/// <summary>
		/// Gets or sets the type of action to perform against the url.
		/// </summary>
		public PostTypeEnum Type
		{
			get { return this._type; }
			set { this._type = value; }
		}
		#endregion

		#region Sync Post
		/// <summary>
		/// Posts the supplied data to specified url.
		/// </summary>
		/// <returns>a string containing the result of the post.</returns>
		public string Post()
		{
			var result = this.PostData( this._url, this.GetParameters() );
			return result;
		}

		/// <summary>
		/// Posts the supplied data to specified url.
		/// </summary>
		/// <param name="url">The url to post to.</param>
		/// <returns>a string containing the result of the post.</returns>
		public string Post( string url )
		{
			this._url = url;
			return this.Post();
		}

		/// <summary>
		/// Posts the supplied data to specified url.
		/// </summary>
		/// <param name="url">The url to post to.</param>
		/// <param name="values">The values to post.</param>
		/// <returns>a string containing the result of the post.</returns>
		public string Post( string url, NameValueCollection values )
		{
			this._values = values;
			return this.Post( url );
		}

		/// <summary>
		/// Posts data to a specified url. Note: this assumes that you have already url encoded the post data.
		/// </summary>
		/// <param name="postData">The data to post.</param>
		/// <param name="url">the url to post to.</param>
		/// <returns>Returns the result of the post.</returns>
		private string PostData( string url, string postData )
		{
			try
			{
				HttpWebRequest request;
				if( this._type == PostTypeEnum.Post )
				{
					var uri = new Uri( url );
					request = ( HttpWebRequest )WebRequest.Create( uri );
					request.Method = "POST";
					this.SetTimeout( this.Timeout, x => request.Timeout = x );
					this.SetTimeout( this.ReadWriteTimeout, x => request.ReadWriteTimeout = x );

					request.ContentType = "application/x-www-form-urlencoded";
					request.ContentLength = postData.Length;
					using( var writeStream = request.GetRequestStream() )
					{
						var bytes = Encoding.UTF8.GetBytes( postData );
						writeStream.Write( bytes, 0, bytes.Length );
					}
				}
				else
				{
					// PostTypeEnum.Get
					var uri = new Uri( url + "?" + postData );
					request = ( HttpWebRequest )WebRequest.Create( uri );
					request.Method = "GET";
					this.SetTimeout( this.Timeout, x => request.Timeout = x );
					this.SetTimeout( this.ReadWriteTimeout, x => request.ReadWriteTimeout = x );
				}

				string result;

				using( var response = ( HttpWebResponse )request.GetResponse() )
				using( var responseStream = response.GetResponseStream() )
				using( var readStream = new StreamReader( responseStream, Encoding.UTF8 ) )
					result = readStream.ReadToEnd();

				return result;
			}
			catch
			{
				if( this.IgnoreErrors )
					return null;
				else
					throw;
			}
		}
		#endregion

		#region Async Post
		/// <summary>
		/// Raised when asynchronous post is finished.
		/// </summary>
		public event EventHandler< AsyncPostFinishedEventArgs > AsyncPostFinished;

		/// <summary>
		/// Posts the supplied data to specified url using async methods.
		/// </summary>
		public void BeginPost()
		{
			this.PostDataAsync( this._url, this.GetParameters() );
		}

		/// <summary>
		/// Posts the supplied data to specified url using async methods.
		/// </summary>
		/// <param name="url">The url to post to.</param>
		public void BeginPost( string url )
		{
			this._url = url;
			this.BeginPost();
		}

		/// <summary>
		/// Posts the supplied data to specified url using async methods.
		/// </summary>
		/// <param name="url">The url to post to.</param>
		/// <param name="values">The values to post.</param>
		public void BeginPost( string url, NameValueCollection values )
		{
			this._values = values;
			this.BeginPost( url );
		}

		/// <summary>
		/// Posts data to a specified <paramref name="url"/>.
		/// </summary>
		/// <remarks>Assumes that you have already <paramref name="url"/> encoded the post data</remarks>
		/// <param name="postData">The data to post.</param>
		/// <param name="url">The <c>url</c> to post to.</param>
		private void PostDataAsync( string url, string postData )
		{
			try
			{
				var requestSate = new RequestState { PostData = postData };
				HttpWebRequest request;
				if( this._type == PostTypeEnum.Post )
				{
					var uri = new Uri( url );
					request = ( HttpWebRequest )WebRequest.Create( uri );
					request.Method = "POST";
					request.ContentType = "application/x-www-form-urlencoded";
					request.ContentLength = postData.Length;
					this.SetTimeout( this.Timeout, x => request.Timeout = x );
					this.SetTimeout( this.ReadWriteTimeout, x => request.ReadWriteTimeout = x );

					requestSate.Request = request;
					request.BeginGetRequestStream( this.RequestWriteCallback, requestSate );
				}
				else
				{
					var uri = new Uri( url + "?" + postData );
					request = ( HttpWebRequest )WebRequest.Create( uri );
					request.Method = "GET";
					this.SetTimeout( this.Timeout, x => request.Timeout = x );
					this.SetTimeout( this.ReadWriteTimeout, x => request.ReadWriteTimeout = x );

					requestSate.Request = request;
					this.BeginHandlingResponse( requestSate );
				}
			}
			catch
			{
				if( !this.IgnoreErrors )
					throw;
			}
		}

		private void RequestWriteCallback( IAsyncResult asynchronousResult )
		{
			try
			{
				var requestState = ( RequestState )asynchronousResult.AsyncState;
				// End the operation.
				using( var writeStream = requestState.Request.EndGetRequestStream( asynchronousResult ) )
				{
					var bytes = Encoding.UTF8.GetBytes( requestState.PostData );
					writeStream.Write( bytes, 0, bytes.Length );
				}

				this.BeginHandlingResponse( requestState );
			}
			catch
			{
				if( !this.IgnoreErrors )
					throw;
			}
		}

		#region Get Async Response
		private void BeginHandlingResponse( RequestState requestState )
		{
			try
			{
				// Start the asynchronous request.
				var asyncResult = requestState.Request.BeginGetResponse( this.ResponseCallback, requestState );

				// this line implements the timeout, if there is a timeout, the callback fires and the request becomes aborted
				ThreadPool.RegisterWaitForSingleObject( asyncResult.AsyncWaitHandle, TimeoutCallback, requestState, DefaultTimeout, true );
			}
			catch
			{
				if( !this.IgnoreErrors )
					throw;
			}
		}

		private void ResponseCallback( IAsyncResult asynchronousResult )
		{
			try
			{
				// State of request is asynchronous.
				var requestState = ( RequestState )asynchronousResult.AsyncState;
				var httpWebRequest = requestState.Request;
				requestState.Response = ( HttpWebResponse )httpWebRequest.EndGetResponse( asynchronousResult );

				// Read the response into a Stream object.
				using( var responseStream = requestState.Response.GetResponseStream() )
				using( var readStream = new StreamReader( responseStream, Encoding.UTF8 ) )
				{
					var result = readStream.ReadToEnd();
					var asyncPostFinished = this.AsyncPostFinished;
					if( asyncPostFinished != null )
						asyncPostFinished.Invoke( this, new AsyncPostFinishedEventArgs( result ) );
				}
			}
			catch
			{
				if( !this.IgnoreErrors )
					throw;
			}
		}

		// Abort the request if the timer fires.
		private static void TimeoutCallback( object state, bool timedOut )
		{
			if( timedOut )
			{
				var request = state as HttpWebRequest;
				if( request != null )
					request.Abort();
			}
		}

		private const int DefaultTimeout = 2 * 60 * 1000; // 2 minutes timeout
		#endregion

		#endregion

		#region Misc methods
		/// <summary>
		/// Gets parameters string.
		/// </summary>
		/// <returns>All parameters encoded in a single string.</returns>
		private string GetParameters()
		{
			var parameters = new StringBuilder();
			for( var i = 0; i < this._values.Count; i++ )
			{
				this.EncodeAndAddItem( ref parameters, this._values.GetKey( i ), this._values[ i ] );
			}
			return parameters.ToString();
		}

		/// <summary>
		/// Encodes an item and ads it to the string.
		/// </summary>
		/// <param name="baseRequest">The previously encoded data.</param>
		/// <param name="key">Param key.</param>
		/// <param name="dataItem">The data to encode.</param>
		/// <returns>A string containing the old data and the previously encoded data.</returns>
		private void EncodeAndAddItem( ref StringBuilder baseRequest, string key, string dataItem )
		{
			if( baseRequest == null )
				baseRequest = new StringBuilder();
			if( baseRequest.Length != 0 )
				baseRequest.Append( "&" );
			baseRequest.Append( key );
			baseRequest.Append( "=" );
			baseRequest.Append( HttpUtility.UrlEncode( dataItem ) );
		}

		/// <summary>
		/// Sets the timeout.
		/// </summary>
		/// <param name="timeout">The timeout.</param>
		/// <param name="methodToSetTimout">The method to set timout.</param>
		private void SetTimeout( TimeSpan? timeout, Action< int > methodToSetTimout )
		{
			if( !timeout.HasValue )
				return;

			methodToSetTimout( timeout.Value != TimeSpan.MaxValue ? Convert.ToInt32( timeout.Value.TotalMilliseconds ) : System.Threading.Timeout.Infinite );
		}
		#endregion

		#region Misc Data Types
		/// <summary>
		/// determines what type of post to perform.
		/// </summary>
		public enum PostTypeEnum
		{
			/// <summary>
			/// Does a get against the source.
			/// </summary>
			Get,

			/// <summary>
			/// Does a post against the source.
			/// </summary>
			Post
		}

		private class RequestState
		{
			// This class stores the State of the request.
			public const int BufferSize = 1024;
			public StringBuilder RequestData { get; private set; }
			public byte[] BufferRead { get; private set; }
			public HttpWebRequest Request { get; set; }
			public HttpWebResponse Response { get; set; }
			public Stream StreamResponse { get; set; }
			public string PostData { get; set; }

			public RequestState()
			{
				this.BufferRead = new byte[ BufferSize ];
				this.RequestData = new StringBuilder( string.Empty );
				this.Request = null;
				this.StreamResponse = null;
				this.PostData = string.Empty;
			}
		}

		/// <summary>
		/// Event arguments for <see cref="PostSubmitter.AsyncPostFinished"/>
		/// </summary>
		public class AsyncPostFinishedEventArgs : EventArgs
		{
			internal AsyncPostFinishedEventArgs( string result )
			{
				this.Result = result;
			}

			/// <summary>Gets the result of asynchronous post.</summary>
			public string Result { get; private set; }
		}
		#endregion
	}
}