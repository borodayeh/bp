using BP.Domain.Entities;

namespace BP.Domain.Interfaces;

public interface IProfileService
{
    Task<IReadOnlyList<Profile>> GetProfilesAsync(CancellationToken cancellationToken = default);
    Task ApplyProfileAsync(string profileId, CancellationToken cancellationToken = default);
}
