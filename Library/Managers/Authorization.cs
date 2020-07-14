using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySqlConnector;
using Dapper.Contrib.Extensions;
using Dapper;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Http;
using Library.Types;
using Library.Managers;

namespace Library.Managers
{
    public class AuthorizationResult
    {
        public int StatusCode;
        public Account Account;
    }   

    public static class Authorization
    {
        public static bool CheckAuthorization(HttpContext HttpContext, MySqlConnection MySQL, HttpResponse httpResponse, out AuthorizationResult authorizationResult)
        {
            authorizationResult = new AuthorizationResult();

            httpResponse = HttpContext.Response;

            HttpRequest request = HttpContext.Request;
            StringValues authHeader = request.Headers["Authorization"];

            if (authHeader.Count == 0)
            {
                Console.WriteLine("No Username or Password entered");
                authorizationResult.StatusCode = 401;
                httpResponse.Headers.Add("WWW-Authenticate", "Basic realm=\"Please Login with your Credentials\"");
                return false;
            }

            string Authenticate = authHeader[0];

            string StartString = "Basic ";

            if (Authenticate.StartsWith(StartString))
            {
                string AuthenticateCoded = Authenticate.Substring(6);

                byte[] AuthenticateDecodedBytes = Convert.FromBase64String(AuthenticateCoded);
                string AuthenticateDecoded = Encoding.Default.GetString(AuthenticateDecodedBytes);

                string[] AuthenticateDecodedSplit = AuthenticateDecoded.Split(":");
                string Username = AuthenticateDecodedSplit[0];
                string Password = AuthenticateDecodedSplit[1];

                string UseritemsQuery = "select id from accounts where USING_BY=2;";
                List<int> UseritemsIds = MySQL.Query<int>(UseritemsQuery).ToList();

                if (UseritemsIds.Count == 0)
                {
                    Console.WriteLine("no users found");
                    return false;
                }
                else
                {
                    foreach (int UseritemId in UseritemsIds)
                    {
                        Account useritem = MySQL.Get<Account>(UseritemId);
                        if (useritem.USERNAME == Username)
                        {
                            authorizationResult.Account = useritem;

                            Console.WriteLine(string.Format("{0} ({1}) is sign in", useritem.USERNAME, useritem.ID));

                            if (authorizationResult.Account.ID == 0)
                            {
                                Console.WriteLine("No User found with this Username");
                                authorizationResult.StatusCode = 401;
                                httpResponse.Headers.Add("WWW-Authenticate", "Basic realm=\"Username/Password is incrorrect\"");
                                return false;
                            }
                            else
                            {
                                if (!Hash.VerifyBCrypt(Password, useritem.PASSWORD))
                                {
                                    Console.WriteLine("Passord is incorrect");
                                    authorizationResult.StatusCode = 401;
                                    httpResponse.Headers.Add("WWW-Authenticate", "Basic realm=\"Username/Password is incrorrect\"");
                                    return false;
                                }
                            }
                        }
                    }
                    if(authorizationResult.Account == null) {
                        Console.WriteLine("No User login");
                        authorizationResult.StatusCode = 401;
                        httpResponse.Headers.Add("WWW-Authenticate", "Basic realm=\"Please Login with your Credentials\"");
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
