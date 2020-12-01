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

        [Test]
        public void PowerButton_Pressed_50W()
        {
            _door.Close();
            _pButton.Press();

            _display.Received(1).ShowPower(50);
        }

    }
}