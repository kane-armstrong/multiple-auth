using Api.Tests.Infrastructure;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Api.Tests.Clients;
using Xunit;

namespace Api.Tests.AuthenticationTests
{
    public class MultipleAuthenticationSchemeTests : IDisposable
    {
        private readonly TestApiFactory _apiFactory;

        public MultipleAuthenticationSchemeTests()
        {
            _apiFactory = new TestApiFactory();
        }

        [Fact]
        public async Task Calls_to_authenticated_endpoints_work_when_given_a_valid_identity_server_token()
        {
            var tokenClient = new IdentityServerTokenClient();
            var token = await tokenClient.GetToken();
            var testClient = _apiFactory.CreateClient();
            using var request = new HttpRequestMessage(HttpMethod.Get, "time");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            using var response = await testClient.SendAsync(request);
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task Calls_to_authenticated_endpoints_work_when_given_a_valid_firebase_token()
        {
            var tokenClient = new FirebaseTokenClient();
            var token = await tokenClient.GetToken();
            var testClient = _apiFactory.CreateClient();
            using var request = new HttpRequestMessage(HttpMethod.Get, "time");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            using var response = await testClient.SendAsync(request);
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task Calls_to_authenticated_endpoints_401_when_no_token_is_provided()
        {
            using var response = await _apiFactory.CreateClient().GetAsync("/time");
            response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task Calls_to_unauthenticated_endpoints_work_when_no_token_is_provided()
        {
            using var response = await _apiFactory.CreateClient().GetAsync("/time/utc");
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task Calls_to_unauthenticated_endpoints_work_when_given_a_valid_identity_server_token()
        {
            var tokenClient = new IdentityServerTokenClient();
            var token = await tokenClient.GetToken();
            var testClient = _apiFactory.CreateClient();
            using var request = new HttpRequestMessage(HttpMethod.Get, "time/utc");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            using var response = await testClient.SendAsync(request);
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task Calls_to_unauthenticated_endpoints_work_when_given_a_valid_firebase_token()
        {
            var tokenClient = new FirebaseTokenClient();
            var token = await tokenClient.GetToken();
            var testClient = _apiFactory.CreateClient();
            using var request = new HttpRequestMessage(HttpMethod.Get, "time/utc");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            using var response = await testClient.SendAsync(request);
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        public void Dispose()
        {
            _apiFactory?.Dispose();
        }
    }
}