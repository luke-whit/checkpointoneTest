using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace CheckpointTest
{
    internal class Program
    {
        static Order Parser(string filename)
        {
            Order order = new Order();
            
            string line;
            bool price_found = false;
            bool id_found = false;
            bool msg_found = false;

            StreamReader reader = File.OpenText(filename);
            
            Regex price_reg = new Regex(@"\$[\d.]+");
            Regex msg_reg = new Regex(@"MESSAGE:");
            Regex id_reg = new Regex(@"\d{3}");
            
            // Assuming order ID is first 3 digit number after "MESSAGE:"
            // and that price is first number with a dollar sign
            while ((line = reader.ReadLine()) != null)
            {
                if (!price_found)
                {
                    Match price_match = price_reg.Match(line);
                    if (price_match.Success)
                    {
                        string tmp = price_match.ToString().Substring(1);
                        order.Price = Convert.ToDouble(tmp);
                        //Console.WriteLine(order.Price);
                        price_found = true;
                    }
                }
                if (!id_found)
                {
                    if (msg_found)
                    {
                        Match id_match = id_reg.Match(line);
                        if (id_match.Success)
                        {
                            order.Id = Convert.ToInt32(id_match.ToString());
                            //Console.WriteLine(order.Id);
                            id_found = true;
                        }
                    }
                    else
                    {
                        Match msg_match = msg_reg.Match(line);
                        if (msg_match.Success)
                        {
                            msg_found = true;
                        }
                    }
                }
                if (id_found && price_found)
                {
                    break;
                }
            }

            return order;
        }

        public static Order LoadfromXmlString(string xml)
        {
            using (var StringReader = new StringReader(xml))
            {
                var serializer = new XmlSerializer(typeof(Order));
                return serializer.Deserialize(StringReader) as Order;
            }
        }

        static void Main(string[] args)
        {
            // Get orders from bigcommerce
            string strResponse = String.Empty;
            RestClient client = new RestClient();
            client.endpoint = "https://api.bigcommerce.com/stores/23zz7ce7pf/v2/orders?status_id=7";
            strResponse = client.makeRequest();

            //parse txt files
            const string txt_dir = @"C:\Users\L\Downloads\E-transfers\E-transfers\";
            DirectoryInfo dir = new DirectoryInfo(txt_dir);
            int count = dir.GetFiles().Length;

            Order[] txt_orders = new Order[count];
            for (int i = 0; i < count; i++)
            {
                txt_orders[i] = Parser(txt_dir + dir.GetFiles()[i]);
            }
            //Console.WriteLine(txt_orders[1].Id.ToString());

            //Check for matches
            //Console.WriteLine(strResponse);
            
            //TODO: parse out xml in strresponse and find matches

        }
    }
}
