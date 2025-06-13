using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using FluentAssertions;
using Xunit;

namespace StatePattern.UnitTests;


public class OrderTests
{
    [Fact]
    public void Order_WhenCreated_ShouldBePending()
    {
        // Arrange
        Order order = new Order();

        // Act
        var result = order.Status;

        // Assert
        result.Should().Be(OrderStatus.Placement);

    }

    [Fact]
    public void Confirm_NotPaid_ShouldBePlacement()
    {
        // Arrange
        Order order = new Order();
        order.Confirm();

        // Act
        order.Confirm();

        // Assert
        order.Status.Should().Be(OrderStatus.Canceled);
    }

    [Fact]
    public void Confirm_Paid_ShouldBePicking()
    {
        // Arrange
        Order order = new Order();
        order.Paid();

        // Act
        order.Confirm();

        // Assert
        order.Status.Should().Be(OrderStatus.Picking);
    }

    [Theory]
    [InlineData(OrderStatus.Picking, OrderStatus.Shipping)]        
    [InlineData(OrderStatus.Shipping, OrderStatus.Delivered)]
    [InlineData(OrderStatus.Delivered, OrderStatus.Completed)]
    public void Confirm_WhenSourceStatus_ShouldBeStatus(OrderStatus sourceStatus, OrderStatus expected)
    {
        // Arrange        
        Order order = new Order(sourceStatus);

        // Act
        order.Confirm();

        // Assert
        order.Status.Should().Be(expected);
    }

    [Theory]
    [InlineData(OrderStatus.Placement, OrderStatus.Canceled)]
    [InlineData(OrderStatus.Delivered, OrderStatus.Canceled)]
    public void Cancel_WhenSourceStatus_ShouldBeStatus(OrderStatus sourceStatus, OrderStatus expected)
    {
        // Arrange        
        Order order = new Order(sourceStatus);

        // Act
        order.Cancel();

        // Assert
        order.Status.Should().Be(expected);
    }

    [Fact]
    public void Cancel_WhenSentStatus_ShouldThrowInvalidOperationException()
    {
        // Arrange        
        Order order = new Order(OrderStatus.Shipping);

        // Act
        var act = () => order.Cancel();

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Cancel_WhenCompletedStatus_ShouldThrowInvalidOperationException()
    {
        // Arrange        
        Order order = new Order(OrderStatus.Completed);

        // Act
        var act = () => order.Cancel();

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }



    [Theory]
    [InlineData(OrderStatus.Placement, true)]
    [InlineData(OrderStatus.Shipping, true)]
    [InlineData(OrderStatus.Delivered, true)]
    [InlineData(OrderStatus.Completed, false)]
    public void CanConfirm_WhenStatus_ShouldBe(OrderStatus sourceStatus, bool expected)
    {
        // Arrange        
        Order order = new Order(sourceStatus);

        // Act
        var result = order.CanConfirm;

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void Graph_WhenCalled_ShouldReturnMermaidGraph()
    {
        // Arrange
        var order = new OrderProxy();

        // Act
        var result = order.Graph;

        // Assert
        result.Should().NotBeNullOrEmpty();

    }
}
