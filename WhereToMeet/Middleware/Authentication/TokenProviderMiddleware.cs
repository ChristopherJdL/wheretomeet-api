using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WhereToMeet.Database;

namespace WhereToMeet.Middleware.Authentication
{
    public class TokenProviderMiddleware
    {
        //TODO: Documentation in markdown
        private readonly RequestDelegate _next;
        private readonly TokenProviderOptions _options;
        public const string UserIdRegisteredClaimName = "userid";
        public TokenProviderMiddleware(
            RequestDelegate next,
            IOptions<TokenProviderOptions> options)
        {
            _next = next;
            _options = options.Value;
        }

        public Task Invoke(HttpContext context)
        {
            // If the request path doesn't match, skip
            if (!context.Request.Path.Equals(_options.Path, StringComparison.Ordinal))
            {
                return _next(context);
            }

            // Request must be POST with Content-Type: application/x-www-form-urlencoded
            if (!context.Request.Method.Equals("POST")
               || !context.Request.HasFormContentType)
            {
                context.Response.StatusCode = 400;
                return context.Response.WriteAsync("Bad request.");
            }

            return GenerateToken(context);
        }
        public User IdentifyUser(string username, string password)
        {
            using (ProgramDbContext dbContext = new ProgramDbContext())
            {
                var foundUsers = from user in dbContext.Users
                                 where user.Username == username && user.Password == password
                                 select user;
                if (!foundUsers.Any())
                    return null;
                return foundUsers.First();
            }
        }
        private Tuple<string, string> ObtainCredentials(HttpContext context)
        {
            string username = context.Request.Query["username"];
            string password = context.Request.Query["password"];
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                username = context.Request.Form["username"];
                password = context.Request.Form["password"];
            }
            return new Tuple<string, string>(username, password);
        }
        private async Task GenerateToken(HttpContext context)
        {
            var credentialsTuple = ObtainCredentials(context);

            var foundUser = IdentifyUser(credentialsTuple.Item1, credentialsTuple.Item2);
            if (foundUser == null)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid username or password.");
                return;
            }

            var now = DateTime.UtcNow;

            // Specifically add the jti (random nonce), iat (issued timestamp), and sub (subject/user) claims.
            // You can add other claims here, if you want:
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, credentialsTuple.Item1),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, TokenProviderMiddleware.ToUnixEpochDate(now).ToString(), ClaimValueTypes.Integer64),
                new Claim(TokenProviderMiddleware.UserIdRegisteredClaimName, foundUser.Id.ToString(), ClaimValueTypes.Integer32)
            };

            // Create the JWT and write it to a string
            var jwt = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(_options.Expiration),
                signingCredentials: _options.SigningCredentials);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new //LogInOutputTransporter
            {
                appToken = encodedJwt
            };

            // Serialize and return the response
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }

        internal static readonly DateTimeOffset UnixEpoch = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);
        internal static long ToUnixEpochDate(DateTime date)
           => (long)Math.Round((date.ToUniversalTime() - UnixEpoch).TotalSeconds);
    }
}
