using System.Diagnostics.CodeAnalysis;
using Bollekurs.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Bollekurs;
public sealed class CaseManager
{
    private const string SessionKey = "_SessionKey";
    
    private Case[]? _cases;
    private int[]? _counter;
    private readonly IMemoryCache _cache;
    private readonly IOptionsMonitor<ApplicationOptions> _optionsMonitor;

    public CaseManager(IMemoryCache cache, IOptionsMonitor<ApplicationOptions> optionsMonitor)
    {
        _cache = cache;
        _optionsMonitor = optionsMonitor;
    }


    internal async Task<Case> GetRandomCase(ISession session, bool ny = false)
    {
        if (_cases == null || _counter == null)
            throw new InvalidOperationException("No cases added");

        string? sessionKey = null;
        if (!ny)
        {
            await session.LoadAsync();
            sessionKey = session.GetString(SessionKey);
        }
        if (string.IsNullOrEmpty(sessionKey))
        {
            sessionKey = Guid.NewGuid().ToString();
            session.SetString(SessionKey, sessionKey);
        }
        var caseIndex = _cache.GetOrCreate(sessionKey, e =>
        {
            e.SetSlidingExpiration(_optionsMonitor.CurrentValue.CaseTimeout);
            var index = GetRandomIndex();
            e.RegisterPostEvictionCallback((key, value, reason, state) => _counter[(int) state]--, index);
            return index;
        });

        return _cases[caseIndex];
    }

    internal Case GetCase(int id)
    {
        if (_cases == null)
            throw new InvalidOperationException("No cases added");
        if(id < 0 || id > _cases.Length-1)
            throw new ArgumentOutOfRangeException("Id must be between 0 and {_cases.Length-1}",nameof(id));
        
        return _cases[id];
    }


    private int GetRandomIndex()
    {
        if (_counter == null)
            throw new InvalidOperationException("No cases added");
        var min = _counter.Min();
        var minIndexes = _counter
        .Select((count, index) => (count, index))
        .Where(i => i.count == min)
        .Select(i => i.index)
        .ToList();
        var index = minIndexes[Random.Shared.Next(minIndexes.Count)];
        _counter[index]++;
        return index;
    }

    [MemberNotNull(nameof(_cases), nameof(_counter))]
    internal void AddCases(IEnumerable<Case> cases)
    {
        if (_cases != null)
            throw new InvalidOperationException("Cases already added");
        
        _cases = cases.ToArray();
        _counter = new int[_cases.Length];
    }
}