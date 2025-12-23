using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Services.Contracts;

public interface IEmailTokenService
{
    string Generate(string email);
    bool Validate(string token, string email);
}
