using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Script.Serialization;

namespace PostBin
{
    public class Post
    {
        /// <summary>
        /// Create a collection of Post objects ordered by ReceivedOn.
        /// </summary>
        /// <param name="models">This is the result of a database lookup of the bins table by the Massive ORM</param>
        /// <returns>A collection of Posts objects ordered by ReceivedOn</returns>
        public static IOrderedEnumerable<Post> CreateFromDatabase(IEnumerable<dynamic> models)
        {
            var list = new List<Post>();

            foreach (var model in models)
            {
                var post = Deserialize(model.Data);
                post.ReceivedOn = (DateTime)model.ReceivedOn;
                list.Add(post);
            }
            return list.OrderByDescending(x => x.ReceivedOn);
        }

        /// <summary>
        /// The date the post was received
        /// </summary>
        public DateTime ReceivedOn { get; set; }

        /// <summary>
        /// Http Headers
        /// </summary>
        public List<PostHeader> Headers { get; set; }

        /// <summary>
        /// Message body
        /// </summary>
        public string Data { get; set; }

        public Post()
        {
            Headers = new List<PostHeader>();
        }

        /// <summary>
        /// Creates a post object from an http call
        /// </summary>
        /// <param name="request">The http request to use</param>
        public Post(HttpRequestBase request)
            
        {
            Headers = new List<PostHeader>();

            foreach (var headerKey in request.Headers.AllKeys)
            {
                Headers.Add(new PostHeader { Name = headerKey, Value = request.Headers[headerKey] });
            }

            using (StreamReader reader = new StreamReader(request.InputStream))
                Data = reader.ReadToEnd();
        }

        /// <summary>
        /// Serialize the current post to JSON
        /// </summary>
        /// <returns>JSON representing the current object</returns>
        public string Serialize()
        {
            var ser = new JavaScriptSerializer();
            return ser.Serialize(this);
        }

        /// <summary>
        /// Creates a post object from a JSON serialized string
        /// </summary>
        /// <param name="data">JSON string representing a Post object</param>
        /// <returns></returns>
        public static Post Deserialize(string data)
        {
            var ser = new JavaScriptSerializer();
            return ser.Deserialize(data, typeof(Post)) as Post;
        }

    }
}