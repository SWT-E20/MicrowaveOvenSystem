using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using NUnit.Framework;
using Microwave.Classes.Interfaces;
using NSubstitute;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class Step1_UI_Door
    {
        private Door _door;
        private Button _pButton, _tButton, _scButton;
        private UserInterface _userInterface;
        private ILight _light;
        private IDisplay _display;
        private ICookController _cookController;


        [SetUp]
        public void Setup()
        {
            _door = new Door();
            _pButton = new Button();
            _tButton = new Button();
            _scButton = new Button();
            _light = Substitute.For<ILight>();
            _display = Substitute.For<IDisplay>();
            _cookController = Substitute.For<ICookController>();
            _userInterface = new UserInterface(_pButton, _tButton, _scButton, _door, _display, _light, _cookController);

        }

        [Test]
        public void Signal_From_Door_Open()
        {
            _door.Close();
            _door.Open();
            _light.Received(1).TurnOn();
        }

        [Test]
        public void Signal_From_Door_Closed()
        {
            _door.Open();
            _door.Close();
            _light.Received(1).TurnOff();
        }

        [TestCase(50, 1, 1)]
        [TestCase(100, 2, 1)]
        [TestCase(150, 3, 1)]
        [TestCase(200, 4, 1)]
        [TestCase(50, 15, 2)]
        public void PowerButton_Pressed_specifiedWatt(int power, int loop, int rec)
        {
            for(int i = 0; i < loop; i++)
            {
                _pButton.Press();
            }

            _display.Received(rec).ShowPower(power);
        }

        [TestCase(1, 1)]
        [TestCase(2, 2)]
        [TestCase(3, 3)]
        [TestCase(4, 4)]
        [TestCase(15, 15)]
        public void TimeButton_Pressed_specifiedMinute(int minutes, int loop)
        {
            _pButton.Press();

            for (int i = 0; i < loop; i++)
            {
                _tButton.Press();
            }

            _display.Received(1).ShowTime(minutes, 0);
        }

        [Test]
        public void CancelButton_Pressed_cookingStopped()
        {
            _pButton.Press();

            _scButton.Press();

            _display.Received(1).Clear();
            _light.Received(1).TurnOff();
            _cookController.Received(0).Stop();
        }


    }
}