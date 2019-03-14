using System;
using System.Collections;
using UnityEngine;

public class Responder : MonoBehaviour
{
    public static string DomaintURL { get; private set; }

    public string Responce { get; private set; }
    public bool IsSuccess { get; private set; }

    static Responder()
    {
        DomaintURL = "https://creatrixteam.ru/taskmanager/";
        //"https://creatrixteam.ru/madeinsstu/";
        //"http://u0523807.cp.regruhosting.ru/creatrixteam.ru/madeinsstu/";
        //"https://madeinsstu.000webhostapp.com/";
    }

    /// <summary>
    /// Отправка данных на сервер
    /// </summary>
    /// <param name="phpFile">Метод обработки</param> 
    /// <param name="url">Параметры запроса</param>
    /// <returns>Ответ с сервера</returns>
    public string Request(string phpFile, string url)
    {
        var extend = "";
        for (var i = 0; i < 5; i++)
        {
            var request = new WWW(DomaintURL + phpFile + ".php?" + url);
            Debug.Log(extend + "Request URL:" + Environment.NewLine + request.url);
            StartCoroutine(OnRespond(request));
            while (!request.isDone) { }

            if (request.error != null)
            {
                Debug.LogError("Server does not respond with error: " + Environment.NewLine + request.error);
                IsSuccess = false;
                extend = "Try again... ";
            }
            else
            {
                Debug.Log("Respond line:" + Environment.NewLine + request.text);
                Responce = request.text;
                IsSuccess = true;
                break;
            }

            request.Dispose();
        }

        return Responce;
    }

    public void Send(string phpFile, WWWForm data)
    {
        var extend = "";
        for (var i = 0; i < 5; i++)
        {
            var request = new WWW(DomaintURL + phpFile + ".php", data);
            Debug.Log(extend + "Target URL:" + Environment.NewLine + request.url);
            StartCoroutine(OnRespond(request));
            while (!request.isDone) { }

            if (request.error != null)
            {
                Debug.LogError("Server does not respond with error: " + Environment.NewLine + request.error);
                IsSuccess = false;
                extend = "Try again... ";
            }
            else
            {
                Debug.Log("Respond line:" + Environment.NewLine + request.text);
                Responce = request.text;
                IsSuccess = true;
                break;
            }

            request.Dispose();
        }
    }

    private IEnumerator OnRespond(WWW request)
    {
        yield return request;
    }

    private IEnumerator WaitForSeconds(int seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
}

