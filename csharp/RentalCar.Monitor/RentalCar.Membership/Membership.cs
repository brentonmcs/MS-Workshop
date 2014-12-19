namespace RentalCar.Membership
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Text;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    using RentalOffer.Core;

    class Membership
    {
        private readonly string _busName;


        public static void Main(string[] args)
        {
            string host = args[0];
            string busName = args[1];

            new Connection(host, busName).WithOpen(new Membership(busName).ReplyWithMembership);
        }

        private void ReplyWithMembership(Connection connection)
        {
            var sub = connection.Subscribe();
            Console.WriteLine(" [*] Waiting for solutions on the {0} bus... To exit press CTRL+C", _busName);

            while (true)
            {
                var e = sub.Next();
                var message = Encoding.UTF8.GetString(e.Body);

                dynamic packet = JsonConvert.DeserializeObject<ExpandoObject>(message, new ExpandoObjectConverter());

                
                if (string.IsNullOrEmpty(packet.membership))            
                {
                    packet.membership = GetMembership();

                    if (((IDictionary<String, object>)packet).ContainsKey("message_count"))
                    {
                        packet.message_count = packet.message_count + 1;
                    }
                    else
                    {
                        packet.message_count = 1;
                    }
                    connection.Publish(JsonConvert.SerializeObject(packet));
                    Console.WriteLine(" [x] Published {0} on the {1} bus", packet, _busName);
                }                
            }
        }

        private static string GetMembership()
        {
            var rand = new Random().Next() % 3;
            var memberships = new []{ "Gold", "Silver", "Platinum" };
            return memberships[rand];
        }

        public Membership(string busName)
        {
            _busName = busName;
        }


    }
}
