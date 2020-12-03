using System;
using System.Collections.Generic;
using System.Text;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal.Execution;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class Step4_Cookcontroller_Display_PowerTube
    {
        private CookController _cookController;

        private IPowerTube _powerTube;
        private IDisplay _display;
        private IUserInterface _userInterface;

        private IDoor _door;
        private IButton _pButton, _tButton, _scButton;
        private ILight _light;

        private ITimer _timer;
        private IOutput _output;


        [SetUp]
        public void Setup()
        {

            _door = new Door();
            _pButton = new Button();
            _tButton = new Button();
            _scButton = new Button();
            _output = Substitute.For<IOutput>();
            _timer = Substitute.For<ITimer>();
            _light = Substitute.For<ILight>();

            _display = new Display(_output);
            _powerTube = new PowerTube(_output);

            _userInterface = new UserInterface(_pButton, _tButton, _scButton, _door, _display, _light, _cookController);
            _cookController = new CookController(_timer, _display, _powerTube, _userInterface);
        }
        [TestCase(50, 5)]
        [TestCase(100, 5)]
        [TestCase(350, 5)]
        [TestCase(650, 5)]
        [TestCase(699, 5)]
        [TestCase(700, 5)]
        public void StartCooking_CorrectPower_CorrectOutputReceivedFromPowerTube(int power, int time)
        {
            _cookController.StartCooking(power, time);
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains($"{power}")));
        }

        [Test]
        public void StartCooking_TimerExpired_CorrectOutputReceivedFromPowerTube()
        {
            _cookController.StartCooking(600, 5); //start cooking
            _timer.Expired += Raise.EventWith(this, EventArgs.Empty);
            _output.Received(1).OutputLine($"PowerTube turned off");
        }

        [TestCase(0, 5)]
        [TestCase(-1, 5)]
        [TestCase(701, 5)]
        [TestCase(49, 5)]
        [TestCase(750, 5)]
        public void StartCooking_WrongPower_ThrowArgumentOutOfRangeException(int power, int time)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _cookController.StartCooking(power, time));
        }


        [Test]
        public void StartCooking_AlreadyCooking_ThrowApplicationException()
        {
            _cookController.StartCooking(50, 120); //arrange for the cooking startet
            Assert.Throws<ApplicationException>(() => _cookController.StartCooking(50, 120));
        }

        [Test]
        public void StopCooking_WhileCookingStarted_CorrectOutputReceivedFromPowerTube()
        {
            _cookController.StartCooking(50, 120);
            _cookController.Stop();

            _output.Received().OutputLine(Arg.Is<string>(str => str == $"PowerTube turned off"));
        }

        [Test]
        public void StopCooking_NoCookingStarted_NoOutputReceivedFromPowerTube()
        {
            _cookController.Stop();
            _output.DidNotReceiveWithAnyArgs();
        }
    }
}
