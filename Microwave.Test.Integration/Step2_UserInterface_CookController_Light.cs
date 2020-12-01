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

        private IUserInterface _userInterface;
        private ILight _light;
        private ICookController _cookController;
        private IDoor _door;
        private IButton _pButton, _tButton, _scButton;

        //Mocks
        private IDisplay _display;
        private ITimer _timer;
        private IPowerTube _powerTube;
        private IOutput _output;

        [SetUp]
        public void Setup()
        {
            //Set up mocks
            _display = Substitute.For<IDisplay>();
            _timer = Substitute.For<ITimer>();
            _powerTube = Substitute.For<IPowerTube>();
            _output = Substitute.For<IOutput>();
            _door = Substitute.For<IDoor>();

            _pButton = new Button();
            _tButton = new Button();
            _scButton = new Button();


            _light = new Light(_output);
            _cookController = new CookController(_timer, _display, _powerTube);
            _userInterface = new UserInterface(_pButton, _tButton, _scButton, _door, _display, _light, _cookController);
        }

        /* STATES: { READY, SETPOWER, SETTIME, COOKING, DOOROPEN } */


        [Test]
        public void OnDoorClosed_LightTurnsOn()
        {
            _door.Close();
            _userInterface.OnDoorClosed(new object(), new EventArgs());
            _light.Received(1).TurnOff();
        }

        [Test]
        public void OnDoorOpen_LightsTurnsOff()
        {
            _door.Open();
            _userInterface.OnDoorOpened(new object(), new EventArgs());
            _light.Received(1).TurnOn();
        }



    }
}
