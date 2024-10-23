namespace API.UnitTests.Tests;

using API.DTOs;
using API.UnitTests.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

public class AccountControllerTests
{
    private readonly string apiRoute = "api/account";
    private readonly HttpClient client;
    private HttpResponseMessage httpResponse;
    private string requestUri;
    private string requestObject;
    private string requestObjetct;
    private HttpContent httpContent;

    public AccountControllerTests()
    {
        client = TestHelper.Instance.Client;
    }

    [Theory]
    [InlineData("BadRequest", "lisa", "123456")]
    public async Task RegisterShouldBadRequest(string statusCode, string username, string password)
    {
        // Arrange
        requestUri = $"{apiRoute}/register";
        var request = new RegisterRequest
        {
            Username = username,
            Password = password
        };

        requestObject = GetRegisterObject(request);
        httpContent = GetHttpContent(requestObject);

        // Act
        httpResponse = await client.PostAsync(requestUri, httpContent);

        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    [Theory]
    [InlineData("OK", "arturo", "123456")]
    public async Task RegisterShouldOk(string statusCode, string username, string password)
    {
        // Arrange
        requestUri = $"{apiRoute}/register";
        var request = new RegisterRequest
        {
            Username = username,
            Password = password
        };

        requestObject = GetRegisterObject(request);
        httpContent = GetHttpContent(requestObject);

        // Act
        httpResponse = await client.PostAsync(requestUri, httpContent);

        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    [Theory]
    [InlineData("Unauthorized", "lisa", "password")]
    public async Task LoginShouldUnauthorized(string statusCode, string username, string password)
    {
        // Arrange
        requestUri = $"{apiRoute}/login";
        var request = new LoginRequest
        {
            Username = username,
            Password = password
        };

        requestObjetct = GetRegisterObject(request);
        httpContent = GetHttpContent(requestObjetct);

        // Act
        httpResponse = await client.PostAsync(requestUri, httpContent);

        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    [Theory]
    [InlineData("OK", "arenita", "123456")]
    public async Task LoginShouldOK(string statusCode, string username, string password)
    {
        // Arrange
        requestUri = $"{apiRoute}/login";
        var request = new LoginRequest
        {
            Username = username,
            Password = password
        };
        requestObjetct = GetRegisterObject(request);
        httpContent = GetHttpContent(requestObjetct);

        // Act
        httpResponse = await client.PostAsync(requestUri, httpContent);

        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    #region Privated methods

    private static string GetRegisterObject(RegisterRequest registerDto)
    {
        var entityObject = new JObject()
        {
            { nameof(registerDto.Username), registerDto.Username },
            { nameof(registerDto.Password), registerDto.Password }
        };

        return entityObject.ToString();
    }

    private static string GetRegisterObject(LoginRequest loginDto)
    {
        var entityObject = new JObject()
        {
            { nameof(loginDto.Username), loginDto.Username },
            { nameof(loginDto.Password), loginDto.Password }
        };
        return entityObject.ToString();
    }

    private static StringContent GetHttpContent(string objectToEncode) =>
        new(objectToEncode, Encoding.UTF8, "application/json");

    #endregion
}
