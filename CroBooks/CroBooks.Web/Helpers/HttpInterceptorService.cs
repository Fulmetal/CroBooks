using CroBooks.Shared.Enums;
using CroBooks.Shared.Exceptions;
using EduPortal.Shared.Exceptions;
using MudBlazor;
using System.Net;
using Toolbelt.Blazor;

namespace CroBooks.Web.Helpers;

public class HttpInterceptorService
{
    private readonly HttpClientInterceptor _interceptor;
    private readonly CustomAuthStateProvider customAuthStateProvider;
    private readonly ISnackbar _snackbar;

    public HttpInterceptorService(HttpClientInterceptor interceptor,
        CustomAuthStateProvider customAuthStateProvider,
        ISnackbar snackbar)
    {
        _interceptor = interceptor;
        this.customAuthStateProvider = customAuthStateProvider;
        _snackbar = snackbar;
    }

    public void RegisterEvent()
    {
        _interceptor.AfterSendAsync += InterceptResponse;
    }

    private async Task InterceptResponse(object sender, HttpClientInterceptorEventArgs e)
    {
        if (e.Response == null)
        {
            HandleUnsuccessfulResponse();
            return;
        }

        if (e.Response.IsSuccessStatusCode) return;
        if (e.Response.StatusCode == HttpStatusCode.Unauthorized)
        {
            HandleUnsuccessfulResponse((HttpStatusCode)e.Response.StatusCode);
            return;
        }

        var capturedContent = await e.GetCapturedContentAsync();
        var problemDetails = await capturedContent.ReadFromJsonAsync<ProblemDetails>();
        if (problemDetails == null) return;

        var statusCode = problemDetails.Status;
        if (problemDetails.Type != null && problemDetails.Type == "SafeDisplayException")
        {
            ThrowExceptionWithNotification(ErrorTypes.SafeDisplayException, problemDetails.Detail, (Severity)problemDetails.SafeDisplayExceptionType);
        }
        else
        {
            HandleUnsuccessfulResponse((HttpStatusCode)statusCode);
        }


        //if (problemDetails.Error != null && problemDetails.Error.ErrorType != null)
        //    HandleCustomException((ErrorTypes)problemDetails.Error.ErrorType,
        //        problemDetails.Error.Message); // Custom Exception has been thrown
        //else
        //    HandleUnsuccessfulResponse((HttpStatusCode)statusCode);

    }

    //private void HandleCustomException(ErrorTypes errorType, string message)
    //{
    //    errorType.ThrowException(message);
    //}

    private void HandleUnsuccessfulResponse(HttpStatusCode? statusCode = null)
    {
        switch (statusCode)
        {
            case null:
                ThrowExceptionWithNotification(ErrorTypes.ServerUnreachable,
                    "Server is unreachable, please contact Administrator.", Severity.Error);
                break;
            case HttpStatusCode.NotFound:
                ThrowExceptionWithNotification(ErrorTypes.NotFound, "The requested resource was not found.",
                    Severity.Warning);
                break;
            case HttpStatusCode.Unauthorized:
                customAuthStateProvider.GetAuthenticationStateAsync().Wait();
                ThrowExceptionWithNotification(ErrorTypes.Unauthorized,
                    "User is not authorized to access desired resource.", Severity.Warning);
                break;
            default:
                ThrowExceptionWithNotification(ErrorTypes.ServerError,
                    "Something went wrong, please contact Administrator.", Severity.Error);
                break;
        }
    }

    private void ThrowExceptionWithNotification(ErrorTypes errorType, string message,
        Severity severity)
    {
        _snackbar.Add(message, severity);
        ThrowHttpResponseException(errorType, message);
    }


    private void ThrowHttpResponseException(ErrorTypes errorType, string message)
    {
        throw new HttpResponseException(message, errorType);
    }

    public void DisposeEvent()
    {
        _interceptor.AfterSendAsync -= InterceptResponse;
    }
}