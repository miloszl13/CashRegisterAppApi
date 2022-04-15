using ApplicationLayer.ViewModels;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace CashRegister.IntegrationTests
{
    public class ProductControllerIntegrationTests : IClassFixture<TestingWebAppFactory<Program>>

    {
        //GetProducts
        //if product table is not empty
        [Fact]
        public async Task GetProducts_IfProductTableIsNotEmpty_ReturnProducts()
        {
            HttpClient client = new TestingWebAppFactory<Program>().CreateClient();
            var response = await client.GetAsync("/api/Product");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("pen", responseString);
            Assert.Contains("coffee", responseString);
        }
        [Fact]
        public async Task GetProducts_IfProductTableIsEmpty_ReturnNotFoundObjectResult()
        {
            HttpClient client = new TestingWebAppFactory<Program>().CreateClient();
            await client.DeleteAsync("/api/Product/DeleteProduct/1");
            await client.DeleteAsync("/api/Product/DeleteProduct/2");
            var response = await client.GetAsync("/api/Product");
            //response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("There are 0 products in database!", responseString);
        }
        //delete product
        //if product does not exist
        [Fact]
        public async Task DeleteProduct_IfThatProductDoesNotExist_ReturnNotFoundObjectResult()
        {
            HttpClient client = new TestingWebAppFactory<Program>().CreateClient();
            var response = await client.DeleteAsync("/api/Product/DeleteProduct/5");
            //response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Product with that id doesnt exist!", responseString);
        }
        //if product exist
        [Fact]
        public async Task DeleteProduct_IfThatProductExist_ReturnTrue()
        {
            HttpClient client = new TestingWebAppFactory<Program>().CreateClient();
            var response = await client.DeleteAsync("/api/Product/DeleteProduct/1");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal("true", responseString);
        }
        //Create product
        //if product does not exist
        [Fact]
        public async Task CreateProduct_IfThatProducDoesNottExist_ReturnTrue()
        {
            var productvm = new ProductViewModel()
            {
                Product_id = 3,
                Name = "glass",
                Cost = 45
            };
            var productvmSerialized= JsonSerializer.Serialize(productvm);
            var content = new StringContent(productvmSerialized, Encoding.UTF8, "application/json");
            HttpClient client = new TestingWebAppFactory<Program>().CreateClient();
            var response = await client.PostAsync("/api/Product/CreateProducts",content);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal("true", responseString);

        }
        //if product already exist
        [Fact]
        public async Task CreateProduct_IfThatProducAlreadyExist_ReturnBadRequestObjectResult()
        {
            var productvm = new ProductViewModel()
            {
                Product_id = 1,
                Name = "pen",
                Cost = 20
            };
            var productvmSerialized = JsonSerializer.Serialize(productvm);
            var content = new StringContent(productvmSerialized, Encoding.UTF8, "application/json");
            HttpClient client = new TestingWebAppFactory<Program>().CreateClient();
            var response = await client.PostAsync("/api/Product/CreateProducts", content);
            //response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Product with that id already exist!", responseString);

        }
        //update product
        //if product does not exist
        [Fact]
        public async Task EditProduct_IfThatProducDoesNottExist_ReturnNotFoundObjectResult()
        {
            var productvm = new ProductViewModel()
            {
                Product_id = 5,
                Name = "glass",
                Cost = 45
            };
            var productvmSerialized = JsonSerializer.Serialize(productvm);
            var content = new StringContent(productvmSerialized, Encoding.UTF8, "application/json");
            HttpClient client = new TestingWebAppFactory<Program>().CreateClient();
            var response = await client.PutAsync("/api/Product/UpdateProduct", content);
            //response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Product with that id doesnt exist!", responseString);

        }
        //if product exist
        [Fact]
        public async Task EditProduct_IfThatProducExist_ReturnTrue()
        {
            var productvm = new ProductViewModel()
            {
                Product_id = 1,
                Name = "pen",
                Cost = 30
            };
            var productvmSerialized = JsonSerializer.Serialize(productvm);
            var content = new StringContent(productvmSerialized, Encoding.UTF8, "application/json");
            HttpClient client = new TestingWebAppFactory<Program>().CreateClient();
            var response = await client.PutAsync("/api/Product/UpdateProduct", content);
            //response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal("true", responseString);

        }


    }
}