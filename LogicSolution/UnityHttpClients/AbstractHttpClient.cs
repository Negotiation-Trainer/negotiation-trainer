using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

namespace UnityHttpClients
{
    public class AbstractHttpClient
    {

        public string TestGetMethod()
        {
            return Get("https://catfact.ninja/fact", new Dictionary<string, string>());
        }
        
        private string Get(string url, Dictionary<string, string> headers)
        {
            UnityWebRequest request = UnityWebRequest.Get(url);
            foreach (KeyValuePair<string, string> header in headers)
            {
                request.SetRequestHeader(header.Key, header.Value);
            }
            
            var operation = request.SendWebRequest();

            // Wait for the request to complete
            while (!operation.isDone) { }

            string resultText;
            switch (request.result)
            {
                case UnityWebRequest.Result.Success:
                    resultText = request.downloadHandler.text;
                    break;
                case UnityWebRequest.Result.ConnectionError:
                    resultText = $"Connection Error: {request.error}";
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    resultText = $"Protocol Error: {request.error}";
                    break;
                default:
                    resultText = "Unknown error";
                    break;
            }

            return resultText;
        }
        
        private string Post(string url, Dictionary<string, string> headers, Object body)
        {
            UnityWebRequest request = new UnityWebRequest(url, "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(body));
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            foreach (KeyValuePair<string, string> header in headers)
            {
                request.SetRequestHeader(header.Key, header.Value);
            }

            var operation = request.SendWebRequest();

            // Wait for the request to complete
            while (!operation.isDone) { }

            string resultText;
            switch (request.result)
            {
                case UnityWebRequest.Result.Success:
                    resultText = request.downloadHandler.text;
                    break;
                case UnityWebRequest.Result.ConnectionError:
                    resultText = $"Connection Error: {request.error}";
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    resultText = $"Protocol Error: {request.error}";
                    break;
                default:
                    resultText = "Unknown error";
                    break;
            }

            return resultText;
        }
    }
}