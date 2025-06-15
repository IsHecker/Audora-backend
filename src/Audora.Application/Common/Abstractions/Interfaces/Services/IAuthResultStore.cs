using Audora.Application.Auth.DTOs;

namespace Audora.Application.Common.Abstractions.Interfaces.Services;

public interface IAuthResultStore
{
    void SaveAuthResult(string state, AuthResult result);
    AuthResult? Get(string state);

    string GenerateState(int length = 32);
    bool ContainsState(string state);
}