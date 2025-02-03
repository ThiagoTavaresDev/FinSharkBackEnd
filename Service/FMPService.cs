using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinSharkBackEnd.Dtos.Stock;
using FinSharkBackEnd.Interfaces;
using FinSharkProjeto.Mappers;
using FinSharkProjeto.Model;
using Newtonsoft.Json;

namespace FinSharkBackEnd.Service
{
    public class FMPService : IFMPService
    {

        private HttpClient _httpClient;

        private IConfiguration _config;

        public FMPService(HttpClient httpC, IConfiguration configuration)
        {
            _httpClient = httpC;
            _config = configuration;
        }

        public async Task<Stock> FindStockBySymbolAsync(string symbol)
        {
            try{
                var result = await _httpClient.GetAsync($"https://financialmodelingprep.com/api/v3/profile/{symbol}&apikey={_config["FMPKey"]}");

                if (result.IsSuccessStatusCode){
                    var content = await result.Content.ReadAsStringAsync();

                    var tasks = JsonConvert.DeserializeObject<FMPStock[]>(content);
                    var stock = tasks[0];

                    if(stock != null){
                        return stock.ToStockFromFMP();
                    }
                    return null;
                }
                return null;
            }
            catch(Exception ex){
                Console.WriteLine(ex.Message);
                return null;
            }

        }
    }
}