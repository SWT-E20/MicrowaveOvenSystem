using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class Step6_Light_Display_Powertube_Output
    {
        private Door _door;
        private Button _pButton, _tButton, _scButton;
        private UserInterface _userInterface;
        private Light _light;
        private Display _display;
        private CookController _cookController;
        private Timer _timer;
        private PowerTube _powerTube;
        private Output _output;

        [SetUp]
        public void Setup()
        {
            _door = new Door();
            _pButton = new Button();
            _tButton = new Button();
            _scButton = new Button();
            _output = new Output();
            _timer = new Timer();
            _powerTube = new PowerTube(_output);
            _light = new Light(_output);
            _display = new Display(_output);
            _cookController = new CookController(_timer, _display, _powerTube);
            _userInterface = new UserInterface(_pButton, _tButton, _scButton, _door, _display, _light, _cookController);

            _cookController.UI = _userInterface;

        }

        [Test]
        public void LightTurnOn_LightIsTurnedOn_OutputsCorrectMessage()
        {
            var sw = new StringWriter();
            Console.SetOut(sw);

            _light.TurnOn();

            string output = sw.ToString();

            Assert.That(output, Is.EqualTo("Light is turned on\r\n"));
        }

        [Test]
        public void LightTurnOff_LightIsTurnedOff_OutputsCorrectMessage()
        {
            var sw = new StringWriter();

            _light.TurnOn();

            Console.SetOut(sw);

            _light.TurnOff();

            string output = sw.ToString();

            Assert.That(output, Is.EqualTo("Light is turned off\r\n"));
        }

        [TestCase(0, 0)]
        [TestCase(0, 1)]
        [TestCase(0, 59)]
        [TestCase(1, 0)]
        [TestCase(59, 59)]
        public void DisplayShowTime_DisplayShowsTime_OutputsCorrectMessage(int min, int sec)
        {
            var sw = new StringWriter();
            Console.SetOut(sw);

            _display.ShowTime(min, sec);

            string output = sw.ToString();

            Assert.That(output, Is.EqualTo($"Display shows: {min:D2}:{sec:D2}\r\n"));
        }

        [TestCase(0)]
        [TestCase(50)]
        [TestCase(100)]
        [TestCase(650)]
        [TestCase(700)]
        public void DisplayShowPower_DisplayShowsPower_OutputsCorrectMessage(int power)
        {
            var sw = new StringWriter();
            Console.SetOut(sw);

            _display.ShowPower(power);

            string output = sw.ToString();

            Assert.That(output, Is.EqualTo($"Display shows: {power} W\r\n"));
        }

        [Test]
        public void DisplayClear_DisplayIsCleared_OutputsCorrectMessage()
        {
            var sw = new StringWriter();
            Console.SetOut(sw);

            _display.Clear();

            string output = sw.ToString();

            Assert.That(output, Is.EqualTo("Display cleared\r\n"));
        }

        [Test]
        public void PowerTubeTurnOff_PowerTubeIsTurnedOff_OutputsCorrectMessage()
        {
            var sw = new StringWriter();

            _powerTube.TurnOn(50);

            Console.SetOut(sw);

            _powerTube.TurnOff();

            string output = sw.ToString();

            Assert.That(output, Is.EqualTo("PowerTube turned off\r\n"));
        }

        [TestCase(150)]
        [TestCase(50)]
        [TestCase(100)]
        public void PowerTubeTurnOn_PowerTubeIsTurnedOn_OutputsCorrectMessage(int power)
        {
            var sw = new StringWriter();
            Console.SetOut(sw);

            _powerTube.TurnOn(power);

            string output = sw.ToString();

            Assert.That(output, Is.EqualTo($"PowerTube works with {power}\r\n"));
        }
    }
}
