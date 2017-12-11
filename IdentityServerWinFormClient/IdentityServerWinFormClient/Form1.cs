using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Windows.Forms;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;

namespace IdentityServerWinFormClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ServicePointManager.ServerCertificateValidationCallback += (sender2, cert, chain, sslPolicyErrors) => true;
        }

        private async void button2_Click_1(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();

            var disco = await DiscoveryClient.GetAsync("http://localhost/identityserver");

            if (disco.IsError)
            {
                MessageBox.Show("" + disco.Error);
                return;
            }

            var tokenClient = new TokenClient(disco.TokenEndpoint, "adminClient", "adminSecret");
            //var tokenClient = new TokenClient(disco.TokenEndpoint, "userClient", "userSecret");

            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("MySecuredApi");
            if (tokenResponse.IsError)
            {
                MessageBox.Show("" + tokenResponse.Error);
                return;
            }
            textBox1.AppendText(tokenResponse.AccessToken);
            //return;

            var client = new HttpClient();
            client.SetBearerToken(textBox1.Text);//tokenResponse.AccessToken

            //var response = await client.GetAsync("http://localhost/IdentityServerApi/JustAdmin");
            var response = await client.GetAsync("http://localhost/IdentityServerApi/JustUser");


            if (!response.IsSuccessStatusCode)
            {
                textBox2.AppendText(response.StatusCode + Environment.NewLine);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                textBox2.AppendText(content + Environment.NewLine);
            }
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            var client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost/identityserver/")
            };

            client.DefaultRequestHeaders.Authorization = new BasicAuthenticationHeaderValue("adminClient", "adminSecret");
            //client.DefaultRequestHeaders.Authorization = new BasicAuthenticationHeaderValue("userClient", "userSecret");
            var form = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "scope", "MySecuredApi" }
            });

            HttpResponseMessage result = client.PostAsync("connect/token", form).Result;

            if (result.IsSuccessStatusCode)
            {
                var r = await result.Content.ReadAsStringAsync();
                textBox1.AppendText(r);
            }
            else
                MessageBox.Show("" + result.Content.ReadAsStringAsync().Result);


            JObject o = JObject.Parse(textBox1.Text);

            string access_token = (string)o["access_token"];

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", @"Bearer " + access_token);

            //var response = await client.GetAsync("http://localhost/IdentityServerApi/JustAdmin");
            var response = await client.GetAsync("http://localhost/IdentityServerApi/JustUser");

            if (!response.IsSuccessStatusCode)
            {
                textBox2.AppendText(response.StatusCode + Environment.NewLine);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();

                //JArray json = JArray.Parse(content);
                //string formatted = json.ToString();

                textBox2.AppendText(content + Environment.NewLine);
            }
        }
    }
}
