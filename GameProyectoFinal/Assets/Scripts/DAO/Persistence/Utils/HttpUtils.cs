using Assets.Scripts.DAO.DTOs;
using System;
using System.Net.Http;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class HttpUtils : MonoBehaviour
{
    public static string BaseUrl = "https://localhost:7161/";
    public static ResponseDTO Post(string endpoint, string jsonBody)
    {
        try
        {
            using HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BaseUrl + endpoint);

            // Crear el contenido del cuerpo de la solicitud
            StringContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            // Enviar la solicitud POST con el cuerpo
            Task<HttpResponseMessage> response = client.PostAsync(client.BaseAddress, content);    

            string responseString = response.Result.Content.ReadAsStringAsync().Result;
            ResponseDTO responseDTO = JsonConvert.DeserializeObject<ResponseDTO>(responseString);
            return responseDTO;
        } catch (Exception)
        {
            return new ResponseDTO { IsSuccess = false, Message = "Connection failed to server" };
        }
    }
}
