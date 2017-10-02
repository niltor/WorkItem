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
using System.Threading.Tasks;
using System.Net.Http;
using Android.Util;
using Newtonsoft.Json;
using VSTS.Models;

namespace VSTS.Services
{
    class VSTSService
    {
        private string _accessToken;
        static string Daemon = "https://zpty.visualstudio.com";
        public VSTSService(string accessToken)
        {
            _accessToken = accessToken;
        }


        /// <summary>
        /// ��ȡ�ҵ���Ŀ�б�
        /// </summary>
        public async Task<Projects> GetMyProjectAsync()
        {
            var url = "/DefaultCollection/_apis/projects?api-version=3.0";
            var result = await GetAsync(url);
            return JsonConvert.DeserializeObject<Projects>(result);
        }

        /// <summary>
        /// ��ȡ������Ŀ
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="type"></param>
        public async Task GetWorkItemsAsync(string projectId,string type)
        {
            var url = "/DefaultCollection/_apis/projects?api-version=3.0";
            var result = await GetAsync(url);
        }


        /// <summary>
        /// Get��������
        /// </summary>
        /// <param name="url">����url����������</param>
        /// <returns>�����ַ���</returns>
        public async Task<string> GetAsync(string url)
        {
            using (var hc = new HttpClient())
            {
                if (!url.StartsWith("/")) url = "/" + url;
                hc.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", string.Format("Bearer {0}", _accessToken));
                var result = await hc.GetStringAsync(Daemon + url);
                Log.Debug("vsts", "result: {0}", result);
                return result;
            }
        }

        /// <summary>
        /// Post��������
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<string> PostAsync(string url, HttpContent content)
        {
            using (var hc = new HttpClient())
            {
                if (!url.StartsWith("/")) url = "/" + url;
                hc.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", string.Format("Bearer {0}", _accessToken));
                var response = await hc.PostAsync(Daemon + url, content);
                var result = await response.Content.ReadAsStringAsync();

                Log.Debug("vsts", "result: {0}", result);
                return result;
            }
        }

        public async Task<string> PutAsync(string url, HttpContent content)
        {
            using (var hc = new HttpClient())
            {
                if (!url.StartsWith("/")) url = "/" + url;
                hc.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", string.Format("Bearer {0}", _accessToken));
                var response = await hc.PutAsync(Daemon + url, content);
                var result = await response.Content.ReadAsStringAsync();

                Log.Debug("vsts", "result: {0}", result);
                return result;
            }
        }
    }
}