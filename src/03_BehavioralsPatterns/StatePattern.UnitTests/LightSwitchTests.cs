using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace StatePattern.UnitTests
{

    [TestClass]
    public class LightSwitchTests
    {
        [TestMethod]
        public void Init_WhenCalled_ShouldStateIsOff()
        {
            // Arrange

            // Act
            LightSwitch lightSwitch = new LightSwitch();

            // Assert
            Assert.IsInstanceOfType<Off>(lightSwitch.State);

        }

        [TestMethod]
        public void Push_Once_ShouldStateIsOn()
        {
            // Arrange
            LightSwitch lightSwitch = new LightSwitch();

            // Act
            lightSwitch.Push();

            // Assert
            Assert.IsInstanceOfType<On>(lightSwitch.State);
        }

        [TestMethod]
        public void PushDown_Twice_ShouldStateIsOff()
        {

            // Arrange
            LightSwitch lightSwitch = new LightSwitch();

            // Act
            lightSwitch.Push();
            lightSwitch.Push();

            // Assert
            Assert.IsInstanceOfType<Off>(lightSwitch.State);
        }

    }
}
