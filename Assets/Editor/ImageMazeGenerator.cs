using UnityEngine;
using UnityEditor;

public class ImageMazeGenerator : EditorWindow
{
    private Texture2D mazeImage;
    private float wallSize = 1f;
    private float wallHeight = 3f;
    private int skipPixels = 2; // Ajuste para no saturar memoria

    [MenuItem("Tools/Laberinto Exacto desde Imagen")]
    public static void ShowWindow()
    {
        GetWindow<ImageMazeGenerator>("Laberinto Exacto");
    }

    private void OnGUI()
    {
        GUILayout.Label("Generación Determinista", EditorStyles.boldLabel);
        mazeImage = (Texture2D)EditorGUILayout.ObjectField("Imagen Mapa", mazeImage, typeof(Texture2D), false);
        wallSize = EditorGUILayout.FloatField("Tamaño del Muro", wallSize);
        wallHeight = EditorGUILayout.FloatField("Altura del Muro", wallHeight);
        skipPixels = EditorGUILayout.IntSlider("Salto de Píxeles (Optimización)", skipPixels, 1, 10);

        if (GUILayout.Button("Generar Laberinto") && mazeImage != null)
        {
            GenerateMaze();
        }
    }

    private void GenerateMaze()
    {
        GameObject parent = new GameObject("Laberinto_Circular");

        for (int x = 0; x < mazeImage.width; x += skipPixels)
        {
            for (int y = 0; y < mazeImage.height; y += skipPixels)
            {
                Color pixelColor = mazeImage.GetPixel(x, y);

                // Si el píxel es oscuro (negro), es un muro
                if (pixelColor.grayscale < 0.5f)
                {
                    GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    // Mapear X e Y de la imagen a X y Z en 3D
                    wall.transform.position = new Vector3(x * wallSize, wallHeight / 2f, y * wallSize);
                    wall.transform.localScale = new Vector3(wallSize, wallHeight, wallSize);
                    wall.transform.parent = parent.transform;
                }
            }
        }

        // Crear suelo base
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        floor.transform.position = new Vector3((mazeImage.width * wallSize) / 2f, 0, (mazeImage.height * wallSize) / 2f);
        floor.transform.localScale = new Vector3(mazeImage.width * 0.1f * wallSize, 1, mazeImage.height * 0.1f * wallSize);
        floor.transform.parent = parent.transform;

        Debug.Log("Laberinto generado con éxito basado en la imagen.");
    }
}