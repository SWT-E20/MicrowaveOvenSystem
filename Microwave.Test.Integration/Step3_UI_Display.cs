using System;
using System.Collections.Generic;
using System.Text;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using NUnit.Framework;
using Microwave.Classes.Interfaces;
using NSubstitute;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class Step3_UI_Display
    {

        private IDoor _door;
        private IButton _pButton, _tButton, _scButton;
        private IUserInterface _userInterface;
        private ILight _light;
        private IDisplay _display;
        private ICookController _cookController;

        //Mocks
        private ITimer _timer;
        private IPowerTube _powerTube;
        private Output _output;

        [SetUp]
        public void Setup()
        {
            _door = new Door();
            _pButton = new Button();
            _tButton = new Button();
            _scButton = new Button();
            //_display =new Display(_output);
           _output = Substitute.For<Output>();
            _display = new Display(_output);
            _light = Substitute.For<ILight>();
            _timer = Substitute.For<ITimer>();

            _userInterface = new UserInterface(_pButton, _tButton, _scButton, _door, _display, _light, _cookController);
            _cookController = new CookController(_timer,_display,_powerTube);
        }
        //    [Test]
        //    public void ShowTime_state()
        //    {
        //        _door.Close();
        //        _tButton.Press();
        //        _display.Received().ShowTime(1,22);
        //    }
        //    [Test]
        //    public void ShowPower_state()
        //    {
        //        _door.Close();
        //        _pButton.Press();
        //        _display.Received().ShowPower(50);
        //    }
        //    [Test]
        //    public void Clear_state()
        //    {
        //        _pButton.Press();
        //        _display.Received().ShowPower(50);
        //    }
        [TestCase(1, 1, 0)]
        [TestCase(2, 2, 0)]
        public void display_ShowTime_state(int press, int min, int sec)
        {
            _pButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            for (int i = 0; i < press; i++)
            {
                _tButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            }

            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains($"Display shows: {min:D2}:{sec:D2}")));
        }
        [TestCase("Display shows: 50 W")]
        [TestCase("Display shows: 100 W")]
        [TestCase("Display shows: 150 W")]
        public void display_ShowPower_state(string a)
        {
            _pButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            _tButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            _scButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains(a)));

        }
        [TestCase("Display is cleared")]
        public void display_Clear_state_door_opended(string a)
        {
            _pButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            _door.Opened += Raise.EventWith(this, EventArgs.Empty);

            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains(a)));
        }
    }
}
