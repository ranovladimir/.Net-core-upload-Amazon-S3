using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TouchCloudBackEnd.V1.dto;

namespace TouchCloudBackEnd.V1.Controllers
{

    [Route("v1/api/[controller]")]
    public class AwsController : Controller
    {

        // GET api/values
        [Route("sign")]
        [HttpPost]
        public DtoAwsSignature GetSignatureAndPolicy([FromBody] Dictionary<string, string> settings)
        {
            string directory = "";
            string isPublic = "false";
 
            settings.TryGetValue("isPublic", out isPublic);
            settings.TryGetValue("directory", out directory);

           

            /*
             * Function for produce a signature key and a policy. the form under dialog with that thing.
             * The goal it's to make a upload for the client-side directly to your Amazon S3.
             * 
             */

            string secretKey = "YOUR SECRET ACESS KEY";

            string dateString = "YOUR-DATE-STRING"; 
            var amzCredentialString = string.Format("{0}/{1}/{2}/s3/aws4_request",

                                                    "YOUR-ACCES-KEY-ID",
                                                    dateString,
                                                    "YOUR-REGION-END-POINT");

            const string awsAlgorithm = "AWS4-HMAC-SHA256";
            string aclValue = "private"; //here you can change the acl-public you've initiate in the angular2 Form.
            if (isPublic.Equals("true"))
            {
                aclValue = "public-read";
                Console.WriteLine(aclValue);

            }
            var conditions =
                new List<dynamic[]>
                {
                    //new dynamic[] {"eq", "$acl", "private"},
                    new dynamic[] {"eq", "$acl", aclValue},
                    new dynamic[] {"eq", "$bucket", "YOUR-BUCKET-NAME"},
                    //new dynamic[] {"starts-with", "$key", "pseudo-user/"},
                    new dynamic[] {"starts-with", "$key", ""},
                    new dynamic[] {"starts-with", "$Content-Type", "image/jpeg"},
                    new dynamic[] {"eq", "$x-amz-date", "YOUR-DATE-STRING-IN-ISO-FORMAT => T000000Z"},
                    new dynamic[] {"eq", "$x-amz-algorithm", awsAlgorithm},
                    new dynamic[] {"eq", "$x-amz-credential", amzCredentialString},
                 };


            var expiration = DateTime.UtcNow.AddDays(50).ToString("yyyy-MM-ddTHH:mm:ss.000Z", CultureInfo.InvariantCulture);
            var policyJson = JsonConvert.SerializeObject(new { expiration = expiration, conditions = conditions });
            var policyBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(policyJson));

            var dateKey = new HMACSHA256(Encoding.UTF8.GetBytes("AWS4" + secretKey))
                .ComputeHash(Encoding.UTF8.GetBytes(dateString));
            var dateRegionKey = new HMACSHA256(dateKey)
                .ComputeHash(Encoding.UTF8.GetBytes("YOUR-REGION-END-POINT"));
            var dateRegionServiceKey = new HMACSHA256(dateRegionKey)
                .ComputeHash(Encoding.UTF8.GetBytes("s3"));
            var signingKey = new HMACSHA256(dateRegionServiceKey)
                .ComputeHash(Encoding.UTF8.GetBytes("aws4_request"));
            var signedPolicyBytes = new HMACSHA256(signingKey)
                .ComputeHash(Encoding.UTF8.GetBytes(policyBase64));
            var signature = BitConverter.ToString(signedPolicyBytes).Replace("-", string.Empty).ToLowerInvariant();
            Console.WriteLine(directory);
            Console.WriteLine(isPublic);
            Console.WriteLine(aclValue);

            return new DtoAwsSignature(aclValue,policyBase64, signature);

            //return new string[] {  signature.ToString(), policyBase64.ToString()};

        }
    }
}



