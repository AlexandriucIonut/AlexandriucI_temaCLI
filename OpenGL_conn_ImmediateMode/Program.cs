using System;
using System.IO;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

class InteractiveWindow : GameWindow
{
    private Vector3[] vertices = new Vector3[3];
    private float[] color = { 1.0f, 0.0f, 0.0f, 1.0f }; 
    private float cameraAngleX = 0.0f;  
    private float cameraAngleY = 0.0f;  

    public InteractiveWindow()
        : base(800, 600, GraphicsMode.Default, "Triangle Color and Camera Control")
    {
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        GL.ClearColor(0.39f, 0.58f, 0.93f, 1.0f); 

        LoadTriangleVertices("triangle_vertices.txt");

        GL.Enable(EnableCap.DepthTest);  
    }

    private void LoadTriangleVertices(string filePath)
    {
        string[] lines = File.ReadAllLines(filePath);
        for (int i = 0; i < 3; i++)
        {
            string[] parts = lines[i].Split(' ');
            vertices[i] = new Vector3(float.Parse(parts[0]), float.Parse(parts[1]), float.Parse(parts[2]));
        }
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);

        
        if (Keyboard.GetState().IsKeyDown(Key.R)) color[0] = Math.Min(1.0f, color[0] + 0.01f);  
        if (Keyboard.GetState().IsKeyDown(Key.G)) color[1] = Math.Min(1.0f, color[1] + 0.01f);  
        if (Keyboard.GetState().IsKeyDown(Key.B)) color[2] = Math.Min(1.0f, color[2] + 0.01f);  
        if (Keyboard.GetState().IsKeyDown(Key.A)) color[3] = Math.Max(0.0f, color[3] - 0.01f);  

        
        if (Keyboard.GetState().IsKeyDown(Key.C))
        {
            color[0] = 1.0f; color[1] = 0.0f; color[2] = 0.0f; color[3] = 1.0f; 
        }
    }

    protected override void OnMouseMove(MouseMoveEventArgs e)
    {
        base.OnMouseMove(e);

        
        cameraAngleX += e.XDelta * 0.1f;
        cameraAngleY += e.YDelta * 0.1f;
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        GL.MatrixMode(MatrixMode.Modelview);
        GL.LoadIdentity();
        GL.Rotate(cameraAngleX, 0.0f, 1.0f, 0.0f);  // Rotirea pe axa Y
        GL.Rotate(cameraAngleY, 1.0f, 0.0f, 0.0f);  // Rotirea pe axa X

        GL.Begin(PrimitiveType.Triangles);

        // Primul vertex roșu
        GL.Color3(1.0f, 0.0f, 0.0f);
        GL.Vertex3(vertices[0]);
        Console.WriteLine($"Vertex 1 RGB: 1.0, 0.0, 0.0");

        // Al doilea vertex verde
        GL.Color3(0.0f, 1.0f, 0.0f);
        GL.Vertex3(vertices[1]);
        Console.WriteLine($"Vertex 2 RGB: 0.0, 1.0, 0.0");

        // Al treilea vertex albastru
        GL.Color3(0.0f, 0.0f, 1.0f);
        GL.Vertex3(vertices[2]);
        Console.WriteLine($"Vertex 3 RGB: 0.0, 0.0, 1.0");

        GL.End();

        SwapBuffers();
    }


    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);

        GL.Viewport(0, 0, Width, Height);
        GL.MatrixMode(MatrixMode.Projection);
        GL.LoadIdentity();
        GL.Ortho(-1.0, 1.0, -1.0, 1.0, -10.0, 10.0);  
        GL.MatrixMode(MatrixMode.Modelview);
    }

    protected override void OnKeyDown(KeyboardKeyEventArgs e)
    {
        base.OnKeyDown(e);

        // Închidem aplicația la apăsarea tastei Escape
        if (e.Key == Key.Escape)
            this.Close();
    }

    static void Main(string[] args)
    {
        using (InteractiveWindow window = new InteractiveWindow())
        {
            window.Run(60.0);  
        }
    }
}

/** Raspunsuri lab 3*/
/*2.Ce este anti-aliasing?

Anti-aliasing este o tehnică utilizată în grafică computerizată pentru a reduce efectul de 
„aliased edges”, care apare atunci când liniile sau contururile sunt desenate cu o rezoluție 
scăzută. Efectul de aliasing se manifestă prin marginile zimțate sau pixelate.

Cum funcționează? Anti-aliasing funcționează prin aplicarea unei metode de amestecare 
a culorilor pixelilor de la marginea unui obiect pentru a crea o tranziție mai lină între culori,
făcând marginile să pară mai netede.

3. Efectul rulării comenzilor GL.LineWidth(float) și GL.PointSize(float)
    GL.LineWidth(float): Această comandă controlează lățimea liniilor desenate. 
De exemplu, glLineWidth(2.0f) va face liniile desenate să fie de două ori mai late decât 
lățimea implicită. Această comandă funcționează în interiorul unei zone GL.Begin(),
dar nu este garantat că toate plăcile grafice vor accepta toate lățimile de linie.

    GL.PointSize(float): Această comandă controlează dimensiunea punctelor desenate. 
De exemplu, glPointSize(5.0f) va face ca punctele să fie de 5 ori mai mari decât dimensiunea 
implicită. De asemenea, aceasta funcționează în interiorul unei zone GL.Begin().

4. Efectul directivelor de desenare
    LineLoop: Atunci când se utilizează GL.LineLoop, se creează o linie închisă conectând 
ultimele vertexuri cu primul. Aceasta este utilă pentru a desena poligoane deschise.

    LineStrip: Cu GL.LineStrip, se desenază o serie de linii conectate între ele,
folosind vertexurile în ordinea în care sunt definite. Aceasta creează o formă de linii continue.

    TriangleFan: GL.TriangleFan permite desenarea unui set de triunghiuri care partajează 
un vârf comun. Aceasta este o metodă eficientă de a crea forme convexe.

    TriangleStrip: GL.TriangleStrip este similar cu TriangleFan, dar pentru a forma triunghiuri,
se utilizează un set de vertexuri adiționale. Aceasta permite desenarea unei forme mai complexe 
cu mai puține vertexuri.

6. Importanța culorilor diferite în desenarea obiectelor 3D
Folosirea culorilor diferite (gradiente sau culori selectate pe suprafață) este importantă 
în desenarea obiectelor 3D pentru a crea un efect vizual mai plăcut și a spori profunzimea. 
Culorile ajută la evidențierea detaliilor și formelor, permițând spectatorilor să perceapă 
mai bine volumul și textura obiectului.

7. Ce reprezintă un gradient de culoare?
Un gradient de culoare este o tranziție lină între două sau mai multe culori. În OpenGL,
acesta se poate obține prin interpolarea culorilor între vertexuri, permițându-le să se 
amestece între ele. Poți realiza un gradient aplicând diferite culori fiecărui vertex și 
lăsând OpenGL să gestioneze interpolarea.

8.Ce este transparența în OpenGL?
Transparența este controlată de canalul alpha. Acesta variază între valorile 0 și 1, unde 0 
este complet transparent și 1 este complet opac. Utilizarea canalului de transparență permite crearea 
unui efect de suprapunere vizuală, permițându-se ca obiectele din spate să fie vizibile prin cele din față.

10.Efectul utilizării unei culori diferite pentru fiecare vertex
Când folosești culori diferite pentru fiecare vertex în modul strip sau când desenezi triunghiuri, 
OpenGL va aplica un gradient între acele culori. Aceasta înseamnă că culoarea va fi interpolată lin 
între vertexuri, creând un efect de tranziție de la o culoare la alta pe suprafața obiectului.

 */
