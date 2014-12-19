using System;
using System.Text;
using System.Collections.Generic;

using fastJSON;

namespace RentalOffer.Core {
    using System.Linq;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class  NeedPacket {

        public const string NEED = "car_rental_offer";

        public List<object> solutions = new List<object>();

        public IList<object> Solutions {
            get { return solutions.AsReadOnly(); }            
        }

        public NeedPacket() {}

        public NeedPacket(string json)
        {
            FromJson(json);
        }

        public string UserName { get; set; }

        public string Membership { get; set; }

        public string Segmentation { get; set; }

        public string ToJson() {
            // Clumsy, but this seems easier than dealing with
            // the JSON provider's idiosyncrasies to get snake-cased keys.
            IDictionary<string, object> message = new Dictionary<string, object>();
            message.Add("json_class", this.GetType().FullName);
            message.Add("need", NEED);
            message.Add("user_name", UserName);
            message.Add("user_id", UserId);
            message.Add("membership", Membership);
            message.Add("solutions", solutions);

            return JSON.ToJSON(message);
        }

        public string UserId { get; set; }

        public NeedPacket FromJson(string json)
        {
            var dictionaryObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            if (dictionaryObject.ContainsKey("membership") && dictionaryObject["membership"] != null) 
                Membership = dictionaryObject["membership"].ToString();

            if (dictionaryObject.ContainsKey("user_name") && dictionaryObject["user_name"] != null)
                UserName = dictionaryObject["user_name"].ToString();

            if (dictionaryObject.ContainsKey("user_id") && dictionaryObject["user_id"] != null)
                UserId = dictionaryObject["user_id"].ToString();

            foreach (var o in (JArray)dictionaryObject["solutions"])
            {
                solutions.Add(o);
            }

            return this;
        }

        
        

        public void ProposeSolution(Object solution) {
            solutions.Add(solution);
        }

    }

}

