using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace Proyecto1_Compiladores_Version1
{
    class Graficas
    {
        public static String graph = "";
        public static void ConstruirArbol(ParseTreeNode raiz ,string Nombre)
        {
            System.IO.StreamWriter f = new System.IO.StreamWriter( Nombre+ ".txt");
            f.Write("digraph lista{ rankdir=TB;node[shape = box, style = filled, color = white]; ");
            graph = "";
            Generar(raiz);
            f.Write(graph);
            f.Write("}");
            f.Close();
        }

        public static void Generar(ParseTreeNode raiz)
        {
            graph = graph + "nodo" + raiz.GetHashCode() + "[label=\"" + raiz.ToString().Replace("\"", "\\\"") + " \", fillcolor=\"LightBlue\", style =\"filled\", shape=\"box\"]; \n";
            if (raiz.ChildNodes.Count > 0)
            {
                ParseTreeNode[] hijos = raiz.ChildNodes.ToArray();
                for (int i = 0; i < raiz.ChildNodes.Count; i++)
                {
                    Generar(hijos[i]);
                    graph = graph + "\"nodo" + raiz.GetHashCode() + "\"-> \"nodo" + hijos[i].GetHashCode() + "\" \n";
                }
            }
        }


        public static void GraficarArbol(string fileName, string path)
        {

            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(@"C:\Program Files (x86)\Graphviz2.38\bin\dot.exe");
                startInfo.Arguments = "-Tpng" + "Alumno.java.txt" + "-o grafo123.png";
                Process.Start(startInfo);
            }
            catch (Exception x)
            {

            }

        }


    }
}
