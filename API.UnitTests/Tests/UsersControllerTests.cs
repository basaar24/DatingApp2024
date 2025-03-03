namespace API.UnitTests.Test;

using API.DTOs;
using API.UnitTests.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

public class UsersControllerTests
{
    private readonly string apiRoute = "api/users";
    private readonly HttpClient client;
    private readonly JsonSerializerOptions jsonSerializerOptions;
    private HttpResponseMessage httpResponse;
    private string requestUrl;
    private string requestObject;
    private string memberObject;
    private HttpContent httpContent;

    public UsersControllerTests()
    {
        client = TestHelper.Instance.Client;
        jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    [Theory]
    [InlineData("OK", "arenita", "123456")]
    public async Task GetUsersShouldOK(string statusCode, string username, string password)
    {
        // Arrange
        requestUrl = "api/account/login";
        var request = new LoginRequest
        {
            Username = username,
            Password = password
        };

        requestObject = GetLoginObject(request);
        httpContent = GetHttpContent(requestObject);

        httpResponse = await client.PostAsync(requestUrl, httpContent);
        var reponse = await httpResponse.Content.ReadAsStringAsync();
        var userRequest = JsonSerializer.Deserialize<UserResponse>(reponse, jsonSerializerOptions);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userRequest!.Token);

        requestUrl = $"{apiRoute}";

        // Act
        httpResponse = await client.GetAsync(requestUrl);

        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    [Theory]
    [InlineData("OK", "arenita", "123456")]
    public async Task GetUserByUsernameShouldOK(string statusCode, string username, string password)
    {
        // Arrange
        requestUrl = "api/account/login";
        var request = new LoginRequest
        {
            Username = username,
            Password = password
        };

        requestObject = GetLoginObject(request);
        httpContent = GetHttpContent(requestObject);

        httpResponse = await client.PostAsync(requestUrl, httpContent);
        var reponse = await httpResponse.Content.ReadAsStringAsync();
        var userRequest = JsonSerializer.Deserialize<UserResponse>(reponse, jsonSerializerOptions);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userRequest!.Token);

        requestUrl = $"{apiRoute}/" + username;

        // Act
        httpResponse = await client.GetAsync(requestUrl);

        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    [Theory]
    [InlineData("NoContent", "bob", "123456", "IntroductionU", "LookingForU", "InterestsU", "CityU", "CountryU")]
    public async Task UpdateUserShouldNoContent(string statusCode, string username, string password, string introduction, string lookingFor, string interests, string city, string country)
    {
        // Arrange
        requestUrl = "api/account/login";
        var request = new LoginRequest
        {
            Username = username,
            Password = password
        };

        requestObject = GetLoginObject(request);
        httpContent = GetHttpContent(requestObject);

        httpResponse = await client.PostAsync(requestUrl, httpContent);
        var reponse = await httpResponse.Content.ReadAsStringAsync();
        var userRequest = JsonSerializer.Deserialize<UserResponse>(reponse, jsonSerializerOptions);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userRequest!.Token);

        requestUrl = $"{apiRoute}";
        var response = new MemberResponse
        {
            Introduction = introduction,
            Interests = interests,
            LookingFor = lookingFor,
            City = city,
            Country = country
        };

        memberObject = GetMemberObject(response);
        httpContent = GetHttpContent(memberObject);

        // Act
        httpResponse = await client.PutAsync(requestUrl, httpContent);

        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    #region Privated methods

    private static string GetLoginObject(LoginRequest loginDto)
    {
        var entityObject = new JObject()
        {
            { nameof(loginDto.Username), loginDto.Username },
            { nameof(loginDto.Password), loginDto.Password }
        };

        return entityObject.ToString();
    }

    private static string GetMemberObject(MemberResponse memberDto)
    {
        var entityObject = new JObject()
        {
            { nameof(memberDto.Introduction), memberDto.Introduction },
            { nameof(memberDto.LookingFor), memberDto.LookingFor },
            { nameof(memberDto.Interests), memberDto.Interests },
            { nameof(memberDto.City), memberDto.City },
            { nameof(memberDto.Country), memberDto.Country }
        };

        return entityObject.ToString();
    }

    private static StringContent GetHttpContent(string objectToCode) =>
        new(objectToCode, Encoding.UTF8, "application/json");

    #endregion
}
