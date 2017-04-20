﻿using System;
using System.Runtime.Serialization;
using THNETII.Common;

namespace THNETII.Security.JOSE
{
    [DataContract]
    public class JsonWebKey
    {
        private readonly DuplexConversionTuple<string, JsonWebKeyType> kty =
            new DuplexConversionTuple<string, JsonWebKeyType>(ConvertStringToKty, ConvertKtyToString);

        [DataMember(Name = "kty")]
        public virtual string KeyTypeString
        {
            get => kty.RawValue;
            set => kty.RawValue = value;
        }

        [IgnoreDataMember]
        public virtual JsonWebKeyType KeyType
        {
            get => kty.ConvertedValue;
            set => kty.ConvertedValue = value;
        }

        private static JsonWebKeyType ConvertStringToKty(string rawKty)
        {
            if (Enum.TryParse(rawKty, out JsonWebKeyType kty))
                return kty;
            else
                return JsonWebKeyType.Unknown;
        }

        private static string ConvertKtyToString(JsonWebKeyType kty)
        {
            switch (kty)
            {
                case JsonWebKeyType.Ec: return "EC";
                case JsonWebKeyType.Rsa: return "RSA";
                default: return null;
            }
        }
    }
}
