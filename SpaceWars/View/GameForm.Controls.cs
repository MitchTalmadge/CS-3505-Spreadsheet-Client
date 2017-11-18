using System.Windows.Forms;

namespace SpaceWars
{
    /// <inheritdoc />
    /// <summary>
    /// Deals with the controls portion of the game form.
    /// </summary>
    /// <authors>Jiahui Chen, Mitch Talmadge</authors>
    public partial class GameForm
    {
        /// <summary>
        /// Keeps track of the currently fired controls.
        /// Index 0: Forward Thrust
        /// Index 1: Right Turn
        /// Index 2: Left Turn
        /// Index 3: Fire Projectile
        /// </summary>
        private readonly bool[] _controls = new bool[4];

        /// <summary>
        /// The speed at which controls are sent.
        /// </summary>
        private const int ControlFps = 200;

        /// <summary>
        /// The timer that sends control commands to the space wars client.
        /// </summary>
        private Timer _controlTimer;

        /// <summary>
        /// Initializes the control handling mechanisms. (Thrust, fire, etc.)
        /// </summary>
        private void InitializeControls()
        {
            // Register key listeners
            KeyDown += OnKeyDown;
            KeyUp += OnKeyUp;

            // Create a timer that sends controls on an interval.
            _controlTimer = new Timer
            {
                Interval = 1000 / ControlFps
            };
            _controlTimer.Tick += (sender, args) => _spaceWarsClient.SendCommand(_controls);

            // Stop timer when this component disposes.
            Disposed += (sender, args) => _controlTimer.Stop();

            // Start the control timer.
            _controlTimer.Start();
        }

        /// <summary>
        /// Called when a key is pushed down.
        /// Records when controls are enabled.
        /// </summary>
        private void OnKeyDown(object sender, KeyEventArgs keyEventArgs)
        {
            switch (keyEventArgs.KeyCode)
            {
                case Keys.W:
                    _controls[0] = true;
                    break;
                case Keys.D:
                    _controls[1] = true;
                    break;
                case Keys.A:
                    _controls[2] = true;
                    break;
                case Keys.Space:
                    _controls[3] = true;
                    break;
            }
        }

        /// <summary>
        /// Called when a key is released.
        /// Records when controls are disabled.
        /// </summary>
        private void OnKeyUp(object sender, KeyEventArgs keyEventArgs)
        {
            switch (keyEventArgs.KeyCode)
            {
                case Keys.W:
                    _controls[0] = false;
                    break;
                case Keys.D:
                    _controls[1] = false;
                    break;
                case Keys.A:
                    _controls[2] = false;
                    break;
                case Keys.Space:
                    _controls[3] = false;
                    break;
            }
        }
    }
}