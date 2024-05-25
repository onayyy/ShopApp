using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Application.Services
{
    public class CurrencyService
    {
        private readonly HttpClient _httpClient;

        public CurrencyService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<Dictionary<string, decimal>> GetExchangeRatesAsync()
        {
            string url = "https://www.tcmb.gov.tr/kurlar/today.xml";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string xml = await response.Content.ReadAsStringAsync();
                    return ParseExchangeRates(xml);
                }
                else
                {
                    // Handle error
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                return null;
            }
        }

        private Dictionary<string, decimal> ParseExchangeRates(string xml)
        {
            var exchangeRates = new Dictionary<string, decimal>();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            XmlNodeList currencyNodes = xmlDoc.SelectNodes("//Currency");
            foreach (XmlNode node in currencyNodes)
            {
                string code = node.SelectSingleNode("CurrencyName").InnerText;
                decimal rate = Convert.ToDecimal(node.SelectSingleNode("ForexBuying").InnerText);

                exchangeRates[code] = rate;
            }

            return exchangeRates;
        }
    }
}
