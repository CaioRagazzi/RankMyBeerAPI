using AutoFixture;
using Moq.AutoMock;

namespace RankMyBeer.Test;

public class BaseTest
{
    private readonly Fixture _fixture = new();
    protected readonly AutoMocker _mocker = new();

    protected T CreateFixture<T>()
    {
        return _fixture.Create<T>();
    }

    protected IEnumerable<T> CreateManyFixture<T>(int count)
    {
        return _fixture.CreateMany<T>();
    }
}