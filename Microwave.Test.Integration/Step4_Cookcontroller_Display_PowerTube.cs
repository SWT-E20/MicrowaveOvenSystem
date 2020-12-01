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
    class Step4_Cookcontroller_Display_PowerTube
    {
        private CookController _cookController;

        private IPowerTube _powerTube;
        private IDisplay _display;
        private IUserInterface _userInterface;
        private IButton _sbutton;

        private ITimer _timer;
        private IOutput _output;


        [SetUp]
        public void Setup()
        {
            _output = Substitute.For<IOutput>();
            _timer = Substitute.For<ITimer>();

            _sbutton = new Button();

            _display = new Display(_output);
            _powerTube = new PowerTube(_output);

            _cookController = new CookController(_timer, _display, _powerTube, _userInterface);
        }
        [TestCase(1, 5)]
        [TestCase(50, 5)]
        [TestCase(100, 5)]
        [TestCase(350, 5)]
        [TestCase(650, 5)]
        [TestCase(699, 5)]
        [TestCase(700, 5)]
        public void StartCooking_CorrectPower(int power, int time)
        {
            _sbutton.Press();
            _cookController.StartCooking(power, time);
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains($"{power}")));
        }
    }
}
