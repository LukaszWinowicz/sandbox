using ChainOfResponsibilityPattern.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ChainOfResponsibilityPattern.UnitTests.UnitTests
{
    [TestClass]
    public class ProcessMessageTests
    {
        [TestMethod]
        public void ProcessMessage_SenderFromWhiteListWithOrder_ShouldByProcess()
        {
            // Arrange
            MessageHandlerFactory factory = new MessageHandlerFactory();
            var firstHandler = factory.Create("priority");
            
            MessageProcessor messageProcessor = new MessageProcessor(firstHandler);
            Message message = new Message { From = "john@domain.com", Title = "Order #1", Body = "Lorem ipsum 953-120-45-91" };

            // Act
            var result = messageProcessor.Process(message);

            // Assert
            Assert.AreEqual("953-120-45-91", result);

        }

        [TestMethod]
        public void ProcessMessage_TitleContainsOrder_ShouldByProcess()
        {
            // Arrange
            MessageHandlerFactory factory = new MessageHandlerFactory();
            var firstHandler = factory.Create();

            string[] whiteList = new string[] { "john@domain.com", "bob@domain.com" };
            MessageProcessor messageProcessor = new MessageProcessor(firstHandler);
            Message message = new Message { From = "john@domain.com", Title = "Order #1", Body = "Lorem ipsum 953-120-45-91" };

            // Act
            var result = messageProcessor.Process(message);

            // Assert
            Assert.AreEqual("953-120-45-91", result);

        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ProcessMessage_TitleNotContainsOrder_ShouldByProcess()
        {
            // Arrange
            MessageHandlerFactory factory = new MessageHandlerFactory();
            var firstHandler = factory.Create();

            string[] whiteList = new string[] { "john@domain.com", "bob@domain.com" };
            MessageProcessor messageProcessor = new MessageProcessor(firstHandler);
            Message message = new Message { From = "john@domain.com", Title = "a", Body = "Lorem ipsum 953-120-45-91" };

            // Act
            var result = messageProcessor.Process(message);

            // Assert

        }

        [TestMethod]
        public void ProcessMessage_BodyContainsValidTaxNumber_ShouldReturnsTaxNumber()
        {
            // Arrange
            MessageHandlerFactory factory = new MessageHandlerFactory();
            var firstHandler = factory.Create();

            string[] whiteList = new string[] { "john@domain.com", "bob@domain.com" };
            MessageProcessor messageProcessor = new MessageProcessor(firstHandler);
            Message message = new Message { From = "john@domain.com", Title = "Order #1", Body = "Lorem ipsum 953-120-45-91" };

            // Act
            var result = messageProcessor.Process(message);

            // Assert
            Assert.AreEqual("953-120-45-91", result);

        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ProcessMessage_BodyContainsInvalidTaxNumber_ShouldThrowsFormatException()
        {
            // Arrange
            MessageHandlerFactory factory = new MessageHandlerFactory();
            var firstHandler = factory.Create();

            string[] whiteList = new string[] { "john@domain.com", "bob@domain.com" };
            MessageProcessor messageProcessor = new MessageProcessor(firstHandler);
            Message message = new Message { From = "john@domain.com", Title = "Order #1", Body = "Lorem ipsum 000-000-00-000" };

            // Act
            var result = messageProcessor.Process(message);

            // Assert
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ProcessMessage_SenderFromNotWhiteListWithOrder_ShouldNotByProcess()
        {
            // Arrange
            MessageHandlerFactory factory = new MessageHandlerFactory();
            var firstHandler = factory.Create();

            string[] whiteList = new string[] { "john@domain.com", "bob@domain.com" };
            MessageProcessor messageProcessor = new MessageProcessor(firstHandler);
            Message message = new Message { From = "a@b.pl", Title = "Order #1" };

            // Act
            messageProcessor.Process(message);           

            // Assert

        }
    }
}
