using System;
using System.Collections.Generic;
using System.Text;

namespace Pospos.Core.Helpers.Jwt
{
    public interface IJsonSerializer
    {
        string Serialize(object obj);
        T Deserialize<T>(string json);
    }
}
