using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnitTest.Configuration;
using WorkshopAspNetCore.Models;
using Xunit;

namespace UnitTest.Controllers
{
    public class PessoasControllerIntegrationTest : BaseIntegrationTest
    {
        private const string BaseUrl = "/api/Pessoas";

        public PessoasControllerIntegrationTest(BaseTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task DeveRetornarListaDePessoasVazia(){
            var response = await Client.GetAsync(BaseUrl);
            response.EnsureSuccessStatusCode();
            
            var responseString  = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<Pessoa>>(responseString);

            Assert.Equal(data.Count, 0);
        }


        [Fact]
        public async Task DeveRetornarListaDePessoasNaoVazia(){
            var pessoa = new Pessoa {
                Nome = "Rafael Barrelo",
                Twitter = "@rafaelbarrelo"
            };

            await TestDataConext.AddAsync(pessoa);
            await TestDataConext.SaveChangesAsync();
            
            var response = await Client.GetAsync(BaseUrl);
            response.EnsureSuccessStatusCode();
            
            var responseString  = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<Pessoa>>(responseString);

            Assert.Equal(data.Count, 1);
            Assert.Contains(data, x => x.Nome == pessoa.Nome);
        }
    }

}