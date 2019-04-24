using ListopiaApp.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ListopiaApp.REST
{
    public static class Service
    {
        static App app = App.Current as App;
        private static HttpClient _client = new HttpClient();
        private static string _apiUrl = "https://listopia.live/api/v1/";

        static Service()
        {
            //RefreshAuth();
        }

        static async public Task<bool> Login(Models.Login login)
        {
            var result = await _client.PostAsync("https://listopia.live/Token", new FormUrlEncodedContent(ObjectToKeyValuePairs(login)));
            var authinfo = JsonConvert.DeserializeObject<Models.AuthInfo>(await result.Content.ReadAsStringAsync());
            app.AuthInfo = authinfo;
            return result.IsSuccessStatusCode;
        }

        static async public Task<ObservableCollection<List>> GetLists()
        {
            var result = await _client.GetAsync(_apiUrl + "Lists");
            return JsonConvert.DeserializeObject<ObservableCollection<List>>(await result.Content.ReadAsStringAsync());
        }

        static async public Task<ObservableCollection<ListItem>> GetListItems(int listId)
        {
            var result = await _client.GetAsync(_apiUrl + $"Lists/{listId}/ListItems");
            return JsonConvert.DeserializeObject<ObservableCollection<ListItem>>(await result.Content.ReadAsStringAsync());
        }

        static async public Task<List> AddList(List list)
        {
            var result = await _client.PostAsync(_apiUrl + "Lists", new FormUrlEncodedContent(ObjectToKeyValuePairs(list)));
            return JsonConvert.DeserializeObject<List>(await result.Content.ReadAsStringAsync());
        }

        static async public Task<bool> DeleteList(List list)
        {
            var result = await _client.DeleteAsync(_apiUrl + $"Lists/{list.Id}");
            return result.IsSuccessStatusCode;
        }

        static async public Task<bool> EditList(List list)
        {
            var result = await _client.PutAsync(_apiUrl + $"Lists/{list.Id}", new FormUrlEncodedContent(ObjectToKeyValuePairs(list)));
            Debug.WriteLine(await result.Content.ReadAsStringAsync());
            return result.IsSuccessStatusCode;
        }

        static async public Task<bool> CheckItem(ListItem item)
        {
            var result = await _client.PutAsync(_apiUrl + $"ListItems/{item.Id}/Check", null);
            return result.IsSuccessStatusCode;
        }

        static async public Task<bool> UncheckItem(ListItem item)
        {
            var result = await _client.PutAsync(_apiUrl + $"ListItems/{item.Id}/Uncheck", null);
            return result.IsSuccessStatusCode;
        }

        static async  public Task<ListItem> AddListItem(ListItem listItem)
        {
            var result = await _client.PostAsync(_apiUrl + "ListItems", new FormUrlEncodedContent(ObjectToKeyValuePairs(listItem)));
            return JsonConvert.DeserializeObject<ListItem>(await result.Content.ReadAsStringAsync());
        }

        static async public Task<bool> DeleteListItem(ListItem listItem)
        {
            var result = await _client.DeleteAsync(_apiUrl + $"ListItems/{listItem.Id}");
            return result.IsSuccessStatusCode;
        }

        static async public Task<bool> EditListItem(ListItem listItem)
        {
            var result = await _client.PutAsync(_apiUrl + $"ListItems/{listItem.Id}", new FormUrlEncodedContent(ObjectToKeyValuePairs(listItem)));
            return result.IsSuccessStatusCode;
        }

        static async public Task<ShareInvite> AddShareInvite(ShareInvite shareInvite)
        {
            var result = await _client.PostAsync(_apiUrl + "Invites", new FormUrlEncodedContent(ObjectToKeyValuePairs(shareInvite)));
            return JsonConvert.DeserializeObject<ShareInvite>(await result.Content.ReadAsStringAsync());
        }

        static async public Task<ObservableCollection<ShareInvite>> GetListShareInvites(List list)
        {
            var result = await _client.GetAsync(_apiUrl + $"Lists/{list.Id}/Invites");

            if (result.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            return JsonConvert.DeserializeObject<ObservableCollection<ShareInvite>>(await result.Content.ReadAsStringAsync());
        }

        static async public Task<ObservableCollection<ShareInvite>> GetShareInvites()
        {
            var result = await _client.GetAsync(_apiUrl + "Invites");
            return JsonConvert.DeserializeObject<ObservableCollection<ShareInvite>>(await result.Content.ReadAsStringAsync());
        }

        static async public Task<bool> AcceptInvite(ShareInvite invite)
        {
            var result = await _client.PutAsync(_apiUrl + $"Invites/{invite.Id}/Accept", null);
            return result.IsSuccessStatusCode;
        }

        static async public Task<bool> DeclineInvite(ShareInvite invite)
        {
            var result = await _client.PutAsync(_apiUrl + $"Invites/{invite.Id}/Decline", null);
            return result.IsSuccessStatusCode;
        }

        static public List<KeyValuePair<string, string>> ObjectToKeyValuePairs(object obj)
        {
            var list = new List<KeyValuePair<string, string>>();

            foreach(var prop in obj.GetType().GetProperties())
            {
                if (prop.GetValue(obj) != null)
                    list.Add(new KeyValuePair<string, string>(prop.Name, prop.GetValue(obj).ToString()));
            }

            return list;
        }

        public static void RefreshAuth()
        {
            if (_client == null) _client = new HttpClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", app.AuthInfo.access_token);
        }

    }
}
