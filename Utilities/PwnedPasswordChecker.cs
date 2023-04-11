using System;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace INTEX.Utilities
{
    public class PwnedPasswordChecker
    {
        private const string PwnedPasswordUrl = "https://api.pwnedpasswords.com/range/";

        public async Task<int> CheckPasswordAsync(string password)
        {
            using (var sha1 = SHA1.Create())
            {
                // Compute the SHA-1 hash of the password
                var hashedPasswordBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(password));
                var hashedPassword = BitConverter.ToString(hashedPasswordBytes).Replace("-", "").ToLower();

                // Send the first five characters of the hash to the Pwned Passwords API to get a list of matching hashes
                var prefix = hashedPassword.Substring(0, 5);
                var suffix = hashedPassword.Substring(5);
                var url = PwnedPasswordUrl + prefix;

                using (var httpClient = new HttpClient())
                using (var response = await httpClient.GetAsync(url))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var hashes = responseContent.Split("\r\n");

                        // Check if the hash of the password appears in the list of matching hashes
                        var matchingHash = $"{prefix}{hashes.FirstOrDefault(x => x.StartsWith(suffix.ToUpper()))}";
                        var count = int.Parse(hashes.FirstOrDefault(x => x.StartsWith(suffix.ToUpper()))?.Split(":")[1] ?? "0");
                        return count;
                    }

                    // The Pwned Passwords API returned an error
                    throw new HttpRequestException($"Failed to check password against Pwned Passwords API. Status code: {response.StatusCode}. Reason: {response.ReasonPhrase}");
                }
            }
        }
    }
}

