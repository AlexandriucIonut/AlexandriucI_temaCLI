using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

class InteractiveWindow : GameWindow
{
    private float _x = 0.0f; // Coordonata X a obiectului
    private float _y = 0.0f; // Coordonata Y a obiectului

    public InteractiveWindow()
        : base(800, 600, GraphicsMode.Default, "OpenTK Interactive Example")
    {
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        GL.ClearColor(0.39f, 0.58f, 0.93f, 1.0f);  // Setăm culoarea de fundal
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);

        // Control prin tastatură: W pentru sus și S pentru jos
        if (Keyboard.GetState().IsKeyDown(Key.W))
            _y += 0.05f;  // Mutăm obiectul în sus
        if (Keyboard.GetState().IsKeyDown(Key.S))
            _y -= 0.05f;  // Mutăm obiectul în jos

        if (Keyboard.GetState().IsKeyDown(Key.A))
            _x -= 0.05f;  // Mutăm obiectul la stânga
        if (Keyboard.GetState().IsKeyDown(Key.D))
            _x += 0.05f;  // Mutăm obiectul la dreapta
    }

    protected override void OnMouseMove(MouseMoveEventArgs e)
    {
        base.OnMouseMove(e);

        // Control prin mouse: actualizare poziție bazată pe mișcarea mouse-ului
        _x = (e.X - Width / 2) / (Width / 2.0f); // Mișcăm pe axa X
        _y = -(e.Y - Height / 2) / (Height / 2.0f); // Mișcăm pe axa Y
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        // Desenăm un pătrat
        GL.Begin(PrimitiveType.Quads);
        GL.Color3(1.0f, 0.0f, 0.0f);  // Culoarea roșie
        GL.Vertex2(_x - 0.05f, _y - 0.05f);
        GL.Vertex2(_x + 0.05f, _y - 0.05f);
        GL.Vertex2(_x + 0.05f, _y + 0.05f);
        GL.Vertex2(_x - 0.05f, _y + 0.05f);
        GL.End();

        SwapBuffers();


        /** Codul pentru laboratorul 3*/
        /*GL.Clear(ClearBufferMask.ColorBufferBit);

        // Setăm lățimea liniilor
        GL.LineWidth(3.0f); // Lățimea liniilor
        GL.Color3(0.0f, 1.0f, 0.0f); // Culoarea verde

        GL.Begin(PrimitiveType.Lines);
        GL.Vertex2(-0.5f, 0.0f);
        GL.Vertex2(0.5f, 0.0f);
        GL.End();

        // Setăm dimensiunea punctului
        GL.PointSize(10.0f); // Dimensiunea punctului
        GL.Color3(1.0f, 0.0f, 0.0f); // Culoarea roșie

        GL.Begin(PrimitiveType.Points);
        GL.Vertex2(0.0f, 0.0f); // Desenăm un punct la originea coordonatelor
        GL.End();*/

    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);

        GL.Viewport(0, 0, Width, Height);
        GL.MatrixMode(MatrixMode.Projection);
        GL.LoadIdentity();
        GL.Ortho(-1.0, 1.0, -1.0, 1.0, -1.0, 1.0);  // Proiecție ortografică
        GL.MatrixMode(MatrixMode.Modelview);
    }

    protected override void OnKeyDown(KeyboardKeyEventArgs e)
    {
        base.OnKeyDown(e);

        // Închide aplicația când se apasă tasta Escape
        if (e.Key == Key.Escape)
            this.Close();
    }

    static void Main(string[] args)
    {
        using (InteractiveWindow window = new InteractiveWindow())
        {
            window.Run(60.0);  // Rulează aplicația la 60 FPS
        }
    }
}
