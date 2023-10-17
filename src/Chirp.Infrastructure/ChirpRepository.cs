using Chirp.Core;

namespace Chirp.Infrastructure;

public class ChirpRepository : IChirpRepository
{
    public void Create(Guid id, CheepDTO entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<CheepDTO> Read()
    {
        throw new NotImplementedException();
    }

    public CheepDTO Read(Guid id)
    {
        throw new NotImplementedException();
    }

    public void Update(CheepDTO entity)
    {
        throw new NotImplementedException();
    }
}