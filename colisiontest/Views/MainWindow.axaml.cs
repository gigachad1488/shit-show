using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using System.Diagnostics;
using System.Numerics;
using System.Threading.Tasks;

namespace colisiontest.Views;

public partial class MainWindow : Window
{
    private const int frameTime = 10;

    private float time = 0;

    private float deltaTime
    {
        get
        {
            return frameTime * 0.02f;
        }
    }

    private Vector2 speed = new Vector2(8, 8);
    private Vector2 currentSpeed = Vector2.Zero;
    
    public MainWindow()
    {
        InitializeComponent();
        Update();
    }

    public async void Update()
    {
        while (true)
        {
            Velocity();

            if (r2.Bounds.Intersects(r1.Bounds))
            {
                r1.Fill = new SolidColorBrush(Colors.Black);
            }
            time += frameTime;

            await Task.Delay(frameTime);
        }
    }

    public void Velocity()
    {
        Vector2 vel;
        SpeedHandler();
        vel = Vector2.Normalize(currentSpeed) * speed * deltaTime;

        if (vel.Length() > 0)
        {
            Canvas.SetLeft(r1, Canvas.GetLeft(r1) + vel.X);
            Canvas.SetBottom(r1, Canvas.GetBottom(r1) + vel.Y);
        }
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (e.Key == Key.D)
        {
            currentSpeed += new Vector2(speed.X, 0);
        }
        if (e.Key == Key.A)
        {
            currentSpeed -= new Vector2(speed.X, 0);
        }
        if (e.Key == Key.W)
        {
            currentSpeed += new Vector2(0, speed.Y);
        }
        if (e.Key == Key.S)
        {
            currentSpeed -= new Vector2(0, speed.Y);
        }
    }

    private void SpeedHandler()
    {
        currentSpeed = Vector2.Lerp(currentSpeed, Vector2.Zero, deltaTime);

        Debug.WriteLine("CURSPED = " + Vector2.Normalize(currentSpeed).ToString());

        if (currentSpeed.Length() <= 0.5f)
        {
            currentSpeed = Vector2.Zero;
        }
    }

    private float Evaluate(float max, float current)
    {
        return current / max;
    }

    public static float QuadraticBezier(float a, float b, float t)
    {
        if (t <= 0)
            return a;
        else if (t >= 1)
            return b;

        float st = t * t;
        return a + 2 * t - 2 * a * t + b * st - 2 * st + a * st;
    }

    public static Vector2 QuadraticBezier(Vector2 a, Vector2 b, float t)
    {
        if (t <= 0)
            return a;
        else if (t >= 1)
            return b;

        return new Vector2(QuadraticBezier(a.X, b.X, t), QuadraticBezier(a.Y, b.Y, t));
    }

    private float EaseOutQuad(float x)
    {
        float z = 1 - x;
        return 1 - z * z;
    }
}
