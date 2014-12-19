namespace RentalCar.BrandOffer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using RentalOffer.Core;

    class BrandOffer
    {
        private readonly string _busName;


        public static void Main(string[] args)
        {
            string host = args[0];
            string busName = args[1];

            new Connection(host, busName).WithOpen(new BrandOffer(busName).ReplyWithOffers);
        }


        public BrandOffer(string busName)
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
                    
                var dictionaryObject = JsonConvert.DeserializeObject<Dictionary<string,object>>(message);

                Console.WriteLine(" [x] dictionaryObject: {0}", dictionaryObject["solutions"]);
                
                var solutions = (JArray)dictionaryObject["solutions"];

                Console.WriteLine(" [x] solutions: {0}", solutions);
                
                if (!solutions.Any())
                {
                    var packet = new NeedPacket();
                    packet.ProposeSolution("10% off");
                    connection.Publish(packet.ToJson());
                    Console.WriteLine(" [x] Published {0} on the {1} bus", message, _busName);
                }
            }
        }
    }  
}
