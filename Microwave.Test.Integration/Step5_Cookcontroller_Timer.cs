using System;
using System.Collections.Generic;
using System.Text;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    [TestFixture]
    class Step5_Cookcontroller_Timer
    {
        private Door _door;
        private Button _pButton, _tButton, _scButton;
        private UserInterface _userInterface;
        private Light _light;
        private IDisplay _display;
        private CookController _cookController;
        private Timer _timer;
        private PowerTube _powerTube;
        private IOutput _output;

        [SetUp]
        public void Setup()
        {
            _output = Substitute.For<IOutput>();
            _display = Substitute.For<IDisplay>();

            _door = new Door();
            _pButton = new Button();
            _tButton = new Button();
            _scButton = new Button();
            _timer = new Timer();
            _powerTube = new PowerTube(_output);
            _light = new Light(_output);
            _cookController = new CookController(_timer, _display, _powerTube);
            _userInterface = new UserInterface(_pButton, _tButton, _scButton, _door, _display, _light, _cookController);

            _cookController.UI = _userInterface;
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        public void StartCooking_TimerRemainingEqualsTime(int time)
        {
            _cookController.StartCooking(50, time);
            Assert.AreEqual(_timer.TimeRemaining, time);
        }

        [TestCase(5, 4, 0, 1)]
        [TestCase(10, 5, 0, 5)]
        [TestCase(60, 10, 0, 50)]
        [TestCase(120, 5, 1, 55)]
        [TestCase(180, 10, 2, 50)]
        public void StartCooking(int time, int timeElapsed, int minPassed, int secPassed)
        {
            _cookController.StartCooking(50, time);

            System.Threading.Thread.Sleep(timeElapsed*1000 + 500); // plus 500 just to make sure event is thrown

            _display.Received(1).ShowTime(minPassed, secPassed);
        }
    }
}
