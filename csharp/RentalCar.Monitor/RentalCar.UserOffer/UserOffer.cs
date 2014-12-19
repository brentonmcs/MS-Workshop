namespace RentalCar.UserOffer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using RentalOffer.Core;

    class UserOffer
    {
        private readonly string _busName;


        public static void Main(string[] args)
        {
            string host = args[0];
            string busName = args[1];

            new Connection(host, busName).WithOpen(new UserOffer(busName).ReplyWithOffers);
        }


        public UserOffer(string busName)
        {
            _busName = busName;
        }


        public void ReplyWithOffers(Connection connection)
        {
            var sub = connection.Subscribe();
            Console.WriteLine(" [*] Waiting for solutions on the {0} bus... To exit press CTRL+C", _busName);

            while (true)
            {
                var e = sub.Next();
                var message = Encoding.UTF8.GetString(e.Body);
                Console.WriteLine(" [x] Received: {0}", message);

                var dictionaryObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(message);

                var solutions = (JArray)dictionaryObject["solutions"];

                if (solutions != null && solutions.Any())
                {
                    Console.WriteLine("Found need with solutions: {0}", solutions);                  
                }
                else
                {
                    Console.WriteLine("No Solotions Found");
                    
                }

                
            }
        }
    }



}
