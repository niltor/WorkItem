using System.Threading.Tasks;
using System.Net.Http;
using Android.Util;
using Newtonsoft.Json;
using VSTS.Models;
using System.Text;
using System.Linq;
using VSTS.Models.WorkItemIdResult;
using VSTS.Models.WorkItemResult;

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
        /// <param name="projectName"></param>
        /// <param name="type"></param>
        public async Task<WorkItemResult> GetWorkItemsAsync(string projectName, string type)
        {

            string queryString = "SELECT [System.TeamProject] FROM WorkItems WHERE [System.TeamProject] = @project";
            var queryModel = new WIQLQuery()
            {
                Query = queryString
            };
            var query = JsonConvert.SerializeObject(queryModel); //request body  json string

            Log.Debug("vsts", "query:" + query);

            var url = "/DefaultCollection/_apis/projects?api-version=3.0";

            if (!string.IsNullOrEmpty(projectName))
            {
                url = $"/DefaultCollection/{projectName}/_apis/wit/wiql?api-version=3.0";
            }
            if (!string.IsNullOrEmpty(type))
            {
                queryString += $" AND  [System.WorkItemType] = '{type}' ";
            }
            //���� ��ȡworkitem ids
            var responseString = await PostAsync(url, new StringContent(query, Encoding.UTF8, "application/json"));
            var workItemIdResult = JsonConvert.DeserializeObject<WorkiItemIdResult>(responseString);

            var ids = workItemIdResult?.WorkItems.Select(s => s.Id).ToArray();

            var idsQuery = string.Join(",", ids);
            //��ȡworkitem
            var requestUrl = $"DefaultCollection/_apis/wit/workitems?ids={idsQuery}&api-version=1.0";
            var result = await GetAsync(requestUrl);
            return JsonConvert.DeserializeObject<WorkItemResult>(result);
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

                Log.Debug("vsts", "{1} ��result: {0}", result, Daemon + url);
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