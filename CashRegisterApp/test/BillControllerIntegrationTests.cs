using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ApplicationLayer.ViewModels;
using CashRegister.IntegrationTests;
using Domain;
using Xunit;

namespace CashRegister.IntegrationTests
{
    public class BillControllerIntegrationTests
    {
        //GetBills
        //if bill table is not empty
        [Fact]
        public async Task GetBills_IfBillTableIsNotEmpty_ReturnBills()
        {
            HttpClient client = new TestingWebAppFactory<Program>().CreateClient();
            var response = await client.GetAsync("/api/Bill/GetAllBills");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("200000000007540220", responseString);
        }
        //if bill table is empty
        [Fact]
        public async Task GetBills_IfBillTableIsEmpty_ReturnNotFountObjectResult()
        {
            HttpClient client = new TestingWebAppFactory<Program>().CreateClient();
            await client.DeleteAsync("/api/Bill/delete/200000000007540220");
            var response = await client.GetAsync("/api/Bill/GetAllBills");
            //response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("There are no bills in database!", responseString);
        }

        //update bill
        //if bill exist
        [Fact]
        public async Task UpdateBill_IfThatBillExist_ReturnTrue()
        {
            var billProductvm = new BillProductViewModel()
            {
                Bill_number = "200000000007540220",
                Product_id = 1,
                Product_quantity = 3,
                Products_cost = 60
            };
            var billProductVMS = new List<BillProductViewModel>() { billProductvm };
            var billViewModel = new BillViewModel()
            {
                Bill_number = "200000000007540220",
                Total_cost = 150,
                Credit_card = "4003600000000014",
                Bill_Products = billProductVMS
            };
            var billViewModelSerialized = JsonSerializer.Serialize(billViewModel);
            var content = new StringContent(billViewModelSerialized, Encoding.UTF8, "application/json");
            HttpClient client = new TestingWebAppFactory<Program>().CreateClient();
            var response = await client.PutAsync("/api/Bill/UpdateBill", content);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal("true", responseString);

        }
        //if bill not exist
        [Fact]
        public async Task UpdateBill_IfThatBillExist_ReturnNotFountObjectResult()
        {
            var billProductvm = new BillProductViewModel()
            {
                Bill_number = "200000000007540220",
                Product_id = 1,
                Product_quantity = 3,
                Products_cost = 60
            };
            var billProductVMS = new List<BillProductViewModel>() { billProductvm };
            var billViewModel = new BillViewModel()
            {
                Bill_number = "105008123123123173",
                Total_cost = 150,
                Credit_card = "4003600000000014",
                Bill_Products = billProductVMS
            };
            var billViewModelSerialized = JsonSerializer.Serialize(billViewModel);
            var content = new StringContent(billViewModelSerialized, Encoding.UTF8, "application/json");
            HttpClient client = new TestingWebAppFactory<Program>().CreateClient();
            var response = await client.PutAsync("/api/Bill/UpdateBill", content);
            //response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Bill with that Bill_number does not exist!", responseString);

        }
        //Create bill
        //if bill does not exist
        [Fact]
        public async Task CreateBill_IfThatBillDoesNottExist_ReturnTrue()
        {
            var billProductvm = new BillProductViewModel()
            {
                Bill_number = "200000000007540220",
                Product_id = 1,
                Product_quantity = 3,
                Products_cost = 60
            };
            var billProductVMS = new List<BillProductViewModel>() { billProductvm };
            var billViewModel = new BillViewModel()
            {
                Bill_number = "105008123123123173",
                Total_cost = 150,
                Credit_card = "4003600000000014",
                Bill_Products = billProductVMS
            };
            var productvmSerialized = JsonSerializer.Serialize(billViewModel);
            var content = new StringContent(productvmSerialized, Encoding.UTF8, "application/json");
            HttpClient client = new TestingWebAppFactory<Program>().CreateClient();
            var response = await client.PostAsync("/api/Bill/CreateNewBill", content);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal("true", responseString);

        }
        //if bill already exist
        [Fact]
        public async Task CreateBill_IfThatBillAlreadyExist_ReturnBadRequestObjectResult()
        {
            var billProductvm = new BillProductViewModel()
            {
                Bill_number = "200000000007540220",
                Product_id = 1,
                Product_quantity = 3,
                Products_cost = 60
            };
            var billProductVMS = new List<BillProductViewModel>() { billProductvm };
            var billViewModel = new BillViewModel()
            {
                Bill_number = "200000000007540220",
                Total_cost = 150,
                Credit_card = "4003600000000014",
                Bill_Products = billProductVMS
            };
            var productvmSerialized = JsonSerializer.Serialize(billViewModel);
            var content = new StringContent(productvmSerialized, Encoding.UTF8, "application/json");
            HttpClient client = new TestingWebAppFactory<Program>().CreateClient();
            var response = await client.PostAsync("/api/Bill/CreateNewBill", content);
            //response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Bill with that Bill_number already exist!", responseString);

        }
        //delete bill
        //if bill does not exist
        [Fact]
        public async Task DeleteBill_IfThatBillDoesNotExist_ReturnNotFoundObjectResult()
        {
            HttpClient client = new TestingWebAppFactory<Program>().CreateClient();
            var response = await client.DeleteAsync("/api/Bill/delete/5");
            //response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Bill with that Bill_number does not exist!", responseString);
        }
        //if bill exist
        [Fact]
        public async Task DeleteBill_IfThatBillExist_ReturnTrue()
        {
            HttpClient client = new TestingWebAppFactory<Program>().CreateClient();
            var response = await client.DeleteAsync("/api/Bill/delete/200000000007540220");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal("true", responseString);
        }
        //GetByllById test
        [Fact]
        public async Task GetBillById_IfBillExist_ReturnsBillViewModel()
        {
            HttpClient client = new TestingWebAppFactory<Program>().CreateClient();
            var response = await client.GetAsync("/api/Bill/GetBillById200000000007540220");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var billProductvm = new BillProductViewModel()
            {
                Bill_number = "200000000007540220",
                Product_id = 1,
                Product_quantity = 3,
                Products_cost = 60
            };
            var billProductVMS = new List<BillProductViewModel>() { billProductvm };
            var billViewModel = new BillViewModel()
            {
                Bill_number = "200000000007540220",
                Total_cost = 150,
                Credit_card = "4003600000000014",
                Bill_Products = billProductVMS
            };
            var productvmSerialized = JsonSerializer.Serialize(billViewModel);
            Assert.Equal(productvmSerialized.ToLower(), responseString.ToLower());
        }
        [Fact]
        public async Task GetBillById_IfBillDoesNotExist_ReturnsNotFoundObjectResult()
        {
            HttpClient client = new TestingWebAppFactory<Program>().CreateClient();
            var response = await client.GetAsync("/api/Bill/GetBillById2");
            //response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Bill with that Bill_number does not exist!", responseString);
        }
        //addcredit card tests
        [Fact]
        public async Task AddCreditCard_ValidBillNumberAndValidCreditCard_returnsBillViewModel()
        {
            HttpClient client = new TestingWebAppFactory<Program>().CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/Bill/AddCreditCardToBill/200000000007540220,4532400716827721");
            var response = await client.SendAsync(request);
            //response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("4532400716827721", responseString);
        }
    }
}
