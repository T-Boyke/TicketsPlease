namespace TicketsPlease.UnitTests.Application.Services;

using Moq;
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Application.Services;
using TicketsPlease.Domain.Entities;

public class MessageServiceTests
{
  private readonly Mock<IMessageRepository> _mockMessageRepo;
  private readonly Mock<IFileAssetRepository> _mockFileRepo;
  private readonly Mock<IFileStorageService> _mockStorage;
  private readonly Mock<INotificationService> _mockNotificationService;
  private readonly MessageService _service;

  public MessageServiceTests()
  {
    _mockMessageRepo = new Mock<IMessageRepository>();
    _mockFileRepo = new Mock<IFileAssetRepository>();
    _mockStorage = new Mock<IFileStorageService>();
    _mockNotificationService = new Mock<INotificationService>();

    _service = new MessageService(
        _mockMessageRepo.Object,
        _mockStorage.Object,
        _mockFileRepo.Object,
        _mockNotificationService.Object);
  }

  [Fact]
  public async Task GetConversationAsync_ShouldReturnMappedDtosWithAttachments()
  {
    // Arrange
    var userId = Guid.NewGuid();
    var otherUserId = Guid.NewGuid();
    var messageId = Guid.NewGuid();

    var msg = new Message
    {
      Id = messageId,
      SenderUserId = userId,
      ReceiverUserId = otherUserId,
      BodyMarkdown = "Hello",
      SentAt = DateTime.UtcNow
    };
    msg.Attachments.Add(new FileAsset { FileName = "test.jpg", BlobPath = "path/to/test.jpg", SizeBytes = 1024 });

    var messages = new List<Message> { msg };

    _mockMessageRepo.Setup(r => r.GetConversationAsync(userId, otherUserId, default))
        .ReturnsAsync(messages);

    // Act
    var result = await _service.GetConversationAsync(userId, otherUserId);

    // Assert
    Assert.Single(result);
    var dto = result[0];
    Assert.Equal("Hello", dto.BodyMarkdown);
    Assert.Single(dto.Attachments);
    Assert.Equal("test.jpg", dto.Attachments.First().FileName);
  }
}
