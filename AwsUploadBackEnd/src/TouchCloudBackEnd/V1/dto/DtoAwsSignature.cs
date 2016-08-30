using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TouchCloudBackEnd.V1.dto
{
    public class DtoAwsSignature
    {

        public string Acl{ get; set; }

        public string Policy { get; set; }
        public string Signature { get; set; }


        public DtoAwsSignature() { }

        public DtoAwsSignature( string acl,string policy, string signature)
        {
            Acl = acl;
            Policy = policy;
            Signature = signature;
        }

    }
}
