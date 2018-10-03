using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Net.Http;
using System.Net;
using System.IO;
using System.Collections.Specialized;
using Newtonsoft.Json;

namespace MySportsBook
{
    public class ServiceHelper
    {
        private static readonly HttpClient client = new HttpClient();
        public static string urlAddress = "http://18.191.204.210:8080/";
        public Login GetLogin(string username, string password)
        {
            try
            {
                var request = (HttpWebRequest) WebRequest.Create(urlAddress + "gettoken");

                var postData = "username=" + username;
                postData += "&password=" + password;
                postData += "&grant_type=password";
                var data = Encoding.ASCII.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                return JsonConvert.DeserializeObject<Login>(responseString);

            }
            catch (Exception e)
            {
                return null;
            }
        }


        public List<Venue> GetVenue(string token)
        {
            List<Venue> venueList=new List<Venue>();
            string url = urlAddress + "api/venue";
            try
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                using (var client = new HttpClient())
                {
                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    }
                    var response = client.GetAsync(url).Result;
                    var result = response.Content.ReadAsStringAsync().Result;
                    if (result != null)
                    {
                        venueList = JsonConvert.DeserializeObject<List<Venue>>(result);
                    }
                }

                return venueList;
            }
            catch (Exception e)
            {
                return null;
            }
            
        }

        public List<Sport> GetSports(string token, string venueId)
        {
            List<Sport> sportList = new List<Sport>();
            string url = urlAddress + "api/sport/" + venueId;
            try
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                using (var client = new HttpClient())
                {
                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    }
                    var response = client.GetAsync(url).Result;
                    var result = response.Content.ReadAsStringAsync().Result;
                    if (result != null)
                    {
                        sportList = JsonConvert.DeserializeObject<List<Sport>>(result);
                    }
                }

                return sportList;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        
        public List<Court> GetCourt(string token, string venueId,string sportId)
        {
            List<Court> courtList = new List<Court>();
            string url = urlAddress + "api/court/" + venueId + "/" + sportId;
            try
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                using (var client = new HttpClient())
                {
                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    }
                    var response = client.GetAsync(url).Result;
                    var result = response.Content.ReadAsStringAsync().Result;
                    if (result != null)
                    {
                        courtList = JsonConvert.DeserializeObject<List<Court>>(result);
                    }
                }

                return courtList;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<BatchCountModel> GetBatch(string token, string venueId, string sportId,string courtId)
        {
            List<BatchCountModel> batchList = new List<BatchCountModel>();
            string url = urlAddress + "api/batch/" + venueId + "/" + sportId + "/" + courtId;
            try
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                using (var client = new HttpClient())
                {
                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    }
                    var response = client.GetAsync(url).Result;
                    var result = response.Content.ReadAsStringAsync().Result;
                    if (result != null)
                    {
                        batchList = JsonConvert.DeserializeObject<List<BatchCountModel>>(result);
                    }
                }

                return batchList;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<Player> GetPlayer(string token, string venueId, string sportId, string courtId,string batchId)
        {
            List<Player> playerList = new List<Player>();
            string url = urlAddress + "api/player/" + venueId + "/" + sportId + "/" + courtId + "/" + batchId;
                        
            try
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                using (var client = new HttpClient())
                {
                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    }
                    var response = client.GetAsync(url).Result;
                    var result = response.Content.ReadAsStringAsync().Result;
                    if (result != null)
                    {
                        playerList = JsonConvert.DeserializeObject<List<Player>>(result);
                    }
                }

                return playerList;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<Player> GetPlayerForAttendance(string token, string venueId, string sportId, string courtId, string batchId, string playerId, string date)
        {
            List<Player> playerList = new List<Player>();
            string url = urlAddress + "api/attendance/" + venueId + "/" + sportId + "/" + courtId + "/" + batchId + "/" +
                         playerId + "/" + date;
            try
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                using (var client = new HttpClient())
                {
                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    }
                    var response = client.GetAsync(url).Result;
                    var result = response.Content.ReadAsStringAsync().Result;
                    if (result != null)
                    {
                        playerList = JsonConvert.DeserializeObject<List<Player>>(result);
                    }
                }

                return playerList;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public bool AttendanceSubmit(string token,List<Attendance> attendanceList)
        {
            bool responseResult = false;
            var json = JsonConvert.SerializeObject(attendanceList);
            var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            string url = urlAddress + "api/attendance";
            try
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                using (var client = new HttpClient())
                {
                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    }
                    var response = client.PostAsync(url, content).Result;
                    var result = response.Content.ReadAsStringAsync().Result;
                    if (result != null)
                    {
                        if (response.IsSuccessStatusCode)
                            responseResult= true;
                        else
                        {
                            responseResult= false;
                        }
                    }
                    else
                    {
                        responseResult = false;
                    }
                }

                return responseResult;

            }
            catch (Exception e)
            {
                responseResult= false;
                return responseResult;
            }
        }

        public List<Games> GetGames(string token, string venueId, string sportId)
        {
            List<Games> gameList = new List<Games>();
            string url = urlAddress + "api/sport/" + venueId;
            try
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                using (var client = new HttpClient())
                {
                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    }
                    var response = client.GetAsync(url).Result;
                    var result = response.Content.ReadAsStringAsync().Result;
                    if (result != null)
                    {
                        gameList = JsonConvert.DeserializeObject<List<Games>>(result);
                    }
                }

                return gameList;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}