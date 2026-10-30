using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class GeneradorLaberintoUnico : EditorWindow
{
    public Texture2D imagenLaberinto;
    public float alturaPared = 4f;
    public float anchoPared = 3f; // Hazlo grande para que sea espacioso

    [MenuItem("Tools/Generador Laberinto (Un Objeto)")]
    public static void ShowWindow()
    {
        GetWindow<GeneradorLaberintoUnico>("Laberinto Rápido");
    }

    void OnGUI()
    {
        GUILayout.Label("Configuración Anti-Overflow", EditorStyles.boldLabel);
        imagenLaberinto = (Texture2D)EditorGUILayout.ObjectField("Imagen (Read/Write ON)", imagenLaberinto, typeof(Texture2D), false);
        alturaPared = EditorGUILayout.FloatField("Altura de Pared", alturaPared);
        anchoPared = EditorGUILayout.FloatField("Tamaño de Pasillos", anchoPared);

        if (GUILayout.Button("Generar Laberinto en 1 Solo Objeto"))
        {
            if (imagenLaberinto == null)
            {
                EditorUtility.DisplayDialog("Error", "Asigna la imagen del laberinto.", "OK");
                return;
            }
            GenerarMallaUnica();
        }
    }

    void GenerarMallaUnica()
    {
        // Creamos un cubo temporal en memoria para copiar su geometría
        GameObject cuboTemp = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Mesh mallaCubo = cuboTemp.GetComponent<MeshFilter>().sharedMesh;

        List<CombineInstance> listaCombinacion = new List<CombineInstance>();

        int ancho = imagenLaberinto.width;
        int alto = imagenLaberinto.height;

        // Leemos la imagen y armamos las matrices en memoria (NO crea GameObjects)
        for (int x = 0; x < ancho; x++)
        {
            for (int y = 0; y < alto; y++)
            {
                Color pixel = imagenLaberinto.GetPixel(x, y);

                // Si el píxel es oscuro (negro), es pared
                if (pixel.r < 0.5f)
                {
                    CombineInstance ci = new CombineInstance();
                    ci.mesh = mallaCubo;

                    Vector3 posicion = new Vector3(x * anchoPared, alturaPared / 2f, y * anchoPared);
                    Vector3 escala = new Vector3(anchoPared, alturaPared, anchoPared);

                    ci.transform = Matrix4x4.TRS(posicion, Quaternion.identity, escala);
                    listaCombinacion.Add(ci);
                }
            }
        }

        DestroyImmediate(cuboTemp); // Limpiamos la memoria

        // Fusionamos todo en una sola malla de alto rendimiento
        Mesh mallaFinal = new Mesh();
        mallaFinal.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mallaFinal.CombineMeshes(listaCombinacion.ToArray(), true, true);
        mallaFinal.RecalculateNormals();
        mallaFinal.RecalculateBounds();

        // Creamos el único GameObject que existirá en la escena
        GameObject laberintoFinal = new GameObject("Laberinto_Optimizado");
        MeshFilter mf = laberintoFinal.AddComponent<MeshFilter>();
        mf.sharedMesh = mallaFinal;

        MeshRenderer mr = laberintoFinal.AddComponent<MeshRenderer>();
        mr.sharedMaterial = new Material(Shader.Find("Standard"));

        // Un solo colisionador perfecto para que el OVRPlayerController choque
        MeshCollider mc = laberintoFinal.AddComponent<MeshCollider>();
        mc.sharedMesh = mallaFinal;

        Selection.activeGameObject = laberintoFinal;
        EditorUtility.DisplayDialog("Éxito", "Laberinto creado. 1 solo objeto, 0 lag.", "OK");
    }
}