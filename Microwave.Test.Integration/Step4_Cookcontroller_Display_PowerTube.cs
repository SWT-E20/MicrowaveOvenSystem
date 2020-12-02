using System;
using System.Collections.Generic;
using System.Text;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal.Execution;
//using Timer = Microwave.Classes.Boundary.Timer; //<-- added

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
        [TestCase(1, 5)]
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
    }
}
