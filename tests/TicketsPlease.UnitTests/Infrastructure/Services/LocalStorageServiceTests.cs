namespace TicketsPlease.UnitTests.Infrastructure.Services;

using Microsoft.AspNetCore.Hosting;
using Moq;
using TicketsPlease.Infrastructure.Services;

public class LocalStorageServiceTests
{
  private readonly Mock<IWebHostEnvironment> _mockEnv;
  private readonly string _contentRootPath;

  public LocalStorageServiceTests()
  {
    _mockEnv = new Mock<IWebHostEnvironment>();
    _contentRootPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
    _mockEnv.Setup(m => m.ContentRootPath).Returns(_contentRootPath);

    Directory.CreateDirectory(_contentRootPath);
  }

  [Fact]
  public async Task SaveFileAsync_ShouldCreateDatedSubdirectory()
  {
    // Arrange
    var service = new LocalStorageService(_mockEnv.Object);
    var content = new MemoryStream(new byte[] { 1, 2, 3 });
    var fileName = "test.txt";
    var expectedDatePart = DateTime.UtcNow.ToString("yyyy/MM");

    // Act
    var blobPath = await service.SaveFileAsync(content, fileName);

    // Assert
    Assert.Contains(expectedDatePart, blobPath, StringComparison.Ordinal);
    var fullPath = Path.Combine(_contentRootPath, "App_Data", "Uploads", blobPath);
    Assert.True(File.Exists(fullPath));

    // Cleanup
    if (Directory.Exists(_contentRootPath))
    {
      Directory.Delete(_contentRootPath, true);
    }
  }

  [Fact]
  public async Task GetFileAsync_WhenNotExists_ShouldThrow()
  {
    var service = new LocalStorageService(_mockEnv.Object);
    await Assert.ThrowsAsync<FileNotFoundException>(() => service.GetFileAsync("none.txt"));
  }

  [Fact]
  public async Task DeleteFileAsync_ShouldRemoveFile()
  {
    // Arrange
    var service = new LocalStorageService(_mockEnv.Object);
    var content = new MemoryStream(new byte[] { 1 });
    var path = await service.SaveFileAsync(content, "del.txt");

    // Act
    await service.DeleteFileAsync(path);

    // Assert
    var fullPath = Path.Combine(_contentRootPath, "App_Data", "Uploads", path);
    Assert.False(File.Exists(fullPath));
  }
}
