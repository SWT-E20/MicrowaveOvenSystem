using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class Step2_UserInterface_CookController_Light
    {
        private Door _door;
        private Button _pButton, _tButton, _scButton;
        private UserInterface _userInterface;
        private Light _light;
        private IDisplay _display;
        private CookController _cookController;
        private ITimer _timer;
        private IPowerTube _powerTube;
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
            _powerTube = Substitute.For<IPowerTube>();
            _light = new Light(_output);
            _display = Substitute.For<IDisplay>();
            _cookController = new CookController(_timer, _display, _powerTube);
            _userInterface = new UserInterface(_pButton, _tButton, _scButton, _door, _display, _light, _cookController);

            _cookController.UI = _userInterface;

        }

        [Test]
        public void DoorOpen_LightTurnsOn_OutputsCorrectMessage()
        {
            _door.Open();

            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains($"Light is turned on")));
        }

        [Test]
        public void DoorOpen_InSetPowerState_CorrectActionsTaken()
        {
            _pButton.Press();
            _display.Received(1).ShowPower(50);

            _door.Open();

            _display.Received(1).Clear();
            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains($"Light is turned on")));
        }

        [Test]
        public void DoorOpen_InSetTimeState_CorrectActionsTaken()
        {
            _pButton.Press();
            _display.Received(1).ShowPower(50);

            _tButton.Press();
            _display.Received(1).ShowTime(1, 0);

            _door.Open();

            _display.Received(1).Clear();
            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains($"Light is turned on")));
        }

        [Test]
        public void DoorOpen_InCookingState_CorrectActionsTaken()
        {
            _pButton.Press();
            _display.Received(1).ShowPower(50);

            _tButton.Press();
            _display.Received(1).ShowTime(1, 0);

            _scButton.Press();
            _powerTube.Received(1).TurnOn(50);
            _timer.Received(1).Start(60);

            _door.Open();
            _powerTube.Received(1).TurnOff();
            _timer.Received(1).Stop();

            _display.Received(1).Clear();
            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains($"Light is turned on")));
        }
    }
}
