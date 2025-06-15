using Audora.Application.Auth.DTOs;
using Audora.Application.Common.Abstractions.Interfaces.Services;

namespace Audora.Infrastructure.Services;

using System.Security.Cryptography;
using Microsoft.Extensions.Caching.Memory;

public class AuthResultStore : IAuthResultStore
{
    private readonly IMemoryCache _cache;
    private static readonly TimeSpan Expiration = TimeSpan.FromMinutes(5);

    public AuthResultStore(IMemoryCache cache)
    {
        _cache = cache;
    }

    public void SaveAuthResult(string state, AuthResult result)
    {
        _cache.Set(state, result, Expiration);
    }

    public AuthResult? Get(string state)
    {
        if (_cache.TryGetValue(state, out AuthResult? authResult))
        {
            Remove(state);
            return authResult;
        }

        return null;
    }

    private void Remove(string state)
    {
        _cache.Remove(state);
    }

    public void SaveState(string state)
    {
        _cache.Set<AuthResult?>(state, null, Expiration);
    }

    public bool ContainsState(string state)
    {
        return _cache.TryGetValue(state, out _);
    }

    public string GenerateState(int length = 32)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        string state;

        do
        {
            var bytes = RandomNumberGenerator.GetBytes(length);
            var result = new char[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = chars[bytes[i] % chars.Length];
            }

            state = new string(result);
        }
        while (ContainsState(state));

        SaveState(state);
        return state;
    }
}