namespace API.Tests;

using API.Controllers;
using API.DataEntities;
using API.Tests.Fake;
using Microsoft.EntityFrameworkCore;
using Moq;

public class BuggyControllerTests
{
    private readonly Mock<BuggyController> _buggyControllerMock;
    private readonly Mock<DbContext> _dbContextMock;

    public BuggyControllerTests()
    {
        _buggyControllerMock = new Mock<BuggyController>();
        _dbContextMock = new Mock<DbContext>();
    }


    [Fact]
    public async Task GetSecretShouldOK()
    {
        // Arrange
        _dbContextMock.Setup(x => x.Set<AppUser>())
            .Returns(new FakeDbSet<AppUser>
            {
                new()
                {
                    City = "",
                    Country = "",
                    
                }
            });

        // Act
        var x = 1;

        // Assert
    }
}