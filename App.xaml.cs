
namespace bobesponja2._0
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var win = new Window(new AppShell())

            {
                Width = 500,   // ajuste aqui
                Height = 800,   // ajuste aqui
                X = 150,        // opcional: posição na tela
                Y = 100
            };
            return win;

        }
    }
}