using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;



namespace Proyecto1_Compiladores_Version1
{
    public partial class Form1 : Form
    {
        SaveFileDialog saveFileDialog1 = new SaveFileDialog();
        FolderBrowserDialog openFile = new FolderBrowserDialog();
        OpenFileDialog openFile1 = new OpenFileDialog();
        string arch;
        string nombre_clase;

        public List<TabPage> pestanias = new List<TabPage>();
        public List<RichTextBox> txtentrada = new List<RichTextBox>();
        public List<int> numero_nodos_Raiz = new List<int>();
        List<Control_nodos> control_nodos = new List<Control_nodos>();
         List<Errores> errores_control = new List<Errores>();
        public List<string> nombre_Proyectos = new List<string>();

        int primer_nodo; //cambia depende del nodo seleccionado
        static int numero_nodo_rai = 0;
        int numero_nodo_sub = 0;
        int numero_pestania = 0;
        string nombre_Archivo;
        string ruta_Archivo;
        string nombre;

        Analizador llamarAnalizador = new Analizador();
        public Form1()
        {
            InitializeComponent();
            this.treeView1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseUp);

            this.contextMenuStrip1.Click += new System.EventHandler(this.contextMenuStrip1_Click);
            this.contextMenuStrip2.Click += new System.EventHandler(this.contextMenuStrip2_Click);
            this.contextMenuStrip3.Click += new System.EventHandler(this.contextMenuStrip3_Click);
        }
        private void contextMenuStrip1_Click(object sender, System.EventArgs e)
        {
            //se tiene que poner para generar el evento
        }

        private void contextMenuStrip2_Click(object sender, System.EventArgs e)
        {
            //se tiene que poner para generar el evento
        }
        private void contextMenuStrip3_Click(object sender, System.EventArgs e)
        {
            //se tiene que poner para generar el evento
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void BuildTree(DirectoryInfo directoryInfo, TreeNodeCollection addInMe)
        {

            TreeNode curNode;
            foreach (DirectoryInfo file in directoryInfo.GetDirectories())
            {
                curNode = addInMe.Add(file.Name);

                nombre_Proyectos.Add(file.Name);
                numero_nodos_Raiz.Add(numero_nodo_rai);
               


                foreach (FileInfo file2 in file.GetFiles())
                {
                    //Este es el que agrega las pestañas
                    curNode.Nodes.Add(file2.FullName, file2.Name, 1, 1);

                    nombre_Archivo = file2.Name;
                    nombre_clase = nombre_Archivo;
                    //poner validacion de clases con el mismo nombre
                    pestanias.Add(new TabPage());
                    txtentrada.Add(new RichTextBox());
                    pestanias[numero_pestania].Text = nombre_clase;
                    txtentrada[numero_pestania].Size = new Size(808, 350);

                    tabControl1.Controls.Add(pestanias[numero_pestania]);
                    pestanias[numero_pestania].Controls.Add(txtentrada[numero_pestania]);

                    txtentrada[numero_pestania].Font = new Font("Arial", 14, FontStyle.Regular);
                    txtentrada[numero_pestania].LoadFile(file2.FullName, RichTextBoxStreamType.PlainText);

                    //txtentrada autosize

                    ruta_Archivo = file2.FullName;
                    this.tabControl1.SelectedIndex = tabControl1.TabPages.Count - 1;
                    Agregar_Lista();
                    numero_pestania = numero_pestania + 1;






                }

                numero_nodo_rai = numero_nodo_rai + 1;

            }




        }

        public void Agregar_Lista()
        {
            Control_nodos nuevo = new Control_nodos();
            nuevo.Nodo_Raiz = numero_nodo_rai;
            nuevo.numero_Tab = numero_pestania;
            nuevo.numero_txt = numero_pestania;
            nuevo.nombre_clase = nombre_clase;
            nuevo.nombre_proyecto = nombre_Proyectos[numero_nodo_rai];
            nuevo.nombre_Arvhivo = nombre_Archivo;
            nuevo.ruta_Archivo = ruta_Archivo;
            control_nodos.Add(nuevo);



        }

        public void Agregar_Lista2()
        {
            Control_nodos nuevo = new Control_nodos();
            nuevo.Nodo_Raiz = primer_nodo;
            nuevo.numero_Tab = numero_pestania;
            nuevo.numero_txt = numero_pestania;
            nuevo.nombre_clase = nombre_clase;
            nuevo.nombre_proyecto = nombre_Proyectos[primer_nodo];
            nuevo.nombre_Arvhivo = nombre_Archivo;
            nuevo.ruta_Archivo = ruta_Archivo;
            control_nodos.Add(nuevo);



        }
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
           /* if (e.Node.Name.EndsWith("java"))
            {
                this.richTextBox1.Clear();
                StreamReader reader = new StreamReader(e.Node.Name);
                this.richTextBox1.Text = reader.ReadToEnd();
                reader.Close();
            }*/
        }

        private void abrirProyectoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Abrir();
        }

        private void Abrir()
        {

            saveFileDialog1.FileName = "";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(openFile.SelectedPath);

                if (directoryInfo.Exists)
                {
                    treeView1.AfterSelect += treeView1_AfterSelect;
                    BuildTree(directoryInfo, treeView1.Nodes);
                }
            }
        }


        private void nuevoProyectoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            nombre = Microsoft.VisualBasic.Interaction.InputBox("Introduzca el nombre del Proyecto", "Proyecto 1 Compiladores");

            if (nombre == "")
            {
                MessageBox.Show("Debe agregarle un nombre al Proyecto para poder crearlo");
                return;
            }

            treeView1.Nodes.Add(nombre, nombre, 0);
            nombre_Proyectos.Add(nombre);
            numero_nodos_Raiz.Add(numero_nodo_rai);
            numero_nodo_rai = numero_nodo_rai + 1;
            nombre = "";
        }

        private void abrirProyectoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Abrir();
        }

        private void treeView1_AfterSelect_1(object sender, TreeViewEventArgs e)
        {
            try
            {
                for (int i = 0; i <= control_nodos.Count - 1; i++)
                {

                    if (treeView1.SelectedNode.Parent.Index == regresarRaiz(i) && treeView1.Nodes[treeView1.SelectedNode.Parent.Index].Nodes[treeView1.SelectedNode.Index].Text == regresar_nombre_clase(i))
                    {
                        tabControl1.SelectedIndex = tabControl1.TabPages.IndexOf(pestanias[regresar_pestania(i)]);
                    }
                }


            }
            catch (Exception x)
            {


            }
        }

        private void analizarToolStripMenuItem_Click(object sender, EventArgs e)
        {
           /* Analizador ej2 = new Analizador();
            if (ej2.esCadenaValida(richTextBox1.Text, new Gramatica()))
            {
                if (Analizador.padre.Root != null)
                {
                    Graficas.ConstruirArbol(Analizador.padre.Root,"AST");
                    ///   textBox2.Text = ej2.hacerOperaciones(Analizador.padre.Root).ToString();                   
                    string path = Directory.GetCurrentDirectory();
                    Graficas.GraficarArbol("AST.txt", path);


                }
            }
            else
            {
                MessageBox.Show("Invalido ");

            }*/

        }

        private TreeNode m_OldSelectNode;
        private void treeView1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Point p = new Point(e.X, e.Y);
            TreeNode node = treeView1.GetNodeAt(p);

            if (e.Button == MouseButtons.Right)
            {
                if (node != null)
                {
                    m_OldSelectNode = treeView1.SelectedNode;
                    treeView1.SelectedNode = node;

                    for (int i = 0; i <= numero_nodos_Raiz.Count - 1; i++)
                    {
                        if (treeView1.Nodes[i].IsSelected == true)
                        {
                            contextMenuStrip2.Show(treeView1, p);
                            primer_nodo = i;  //guarda la posicion del nodo seleccionado
                                              // MessageBox.Show(i.ToString());
                            break;
                        }
                        else
                        {
                            contextMenuStrip3.Show(treeView1, p);
                        }


                    }

                }
                else
                {
                    contextMenuStrip1.Show(treeView1, p);
                }



            }


        }


        public int regresarRaiz(int i)
        {
            int salida = 0;
            Control_nodos Raiz = control_nodos[i];
            salida = Raiz.Nodo_Raiz;
            return salida;

        }

        public int regresar_pestania(int i)
        {
            int salida = 0;
            Control_nodos nodo = control_nodos[i];

            salida = nodo.numero_Tab;
            return salida;

        }



        public int regresar_txt(int i)
        {
            int salida = 0;
            Control_nodos nodo = control_nodos[i];

            salida = nodo.numero_txt;
            return salida;

        }

        public string regresar_nombre_clase(int i)
        {
            string salida = "";
            Control_nodos nodo = control_nodos[i];

            salida = nodo.nombre_clase;

            return salida;

        }

        public string regresar_nombre_Proyecto(int i)
        {
            string salida = "";
            Control_nodos nodo = control_nodos[i];

            salida = nodo.nombre_proyecto;

            return salida;

        }

        private void analizarProyectoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Analizador.Atributos.Clear();
            Analizador.Clases.Clear();
            Analizador.Metodos.Clear();
            Analizador.conexiones.Clear();
            Analizador.Agregacion.Clear();
            Analizador.err.Clear();
            errores_control.Clear();

            for (int i = 0; i <= control_nodos.Count - 1; i++) {

             

                if (regresarRaiz(i) == primer_nodo) {
                    
                    Analizador ej2 = new Analizador();
                    ej2.analizar(txtentrada[regresar_txt(i)].Text);
                    if (ej2.esCadenaValida(txtentrada[regresar_txt(i)].Text, new Gramatica()))
                    {
                        if (Analizador.padre.Root != null)
                        {
                          //  MessageBox.Show(ej2.recorrerArbol(Analizador.padre.Root));                           
                      //    Graficas.ConstruirArbol(Analizador.padre.Root,regresar_nombre_clase(i));                                         
                        //   string path = Directory.GetCurrentDirectory();
                          //  Graficas.GraficarArbol(regresar_nombre_clase(i), path);
                            ej2.recorrerArbol(Analizador.padre.Root);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalido ");

                    }



                }

            }
        }

        
        public void Escribir() {
            StreamWriter escritor = default(StreamWriter);
            //declaro la variable escritor
            SaveFileDialog SFD = new SaveFileDialog();
            string ResultadosSFD = null;
            ResultadosSFD = "grafo1.txt";
            // en visual basic y c# las rutas relativas se encuentran en la carpetda Debug 
            escritor = new StreamWriter(ResultadosSFD);

            var with1 = escritor;
            // escribo el archivo
            with1.WriteLine("digraph Couriers {");
            //
            with1.WriteLine("fontname" + " = " + "\"Bitstream Vera Sans\"");
            with1.WriteLine("fontsize = 8");
            with1.WriteLine("node [");
            with1.WriteLine("fontname" + " = " + "\"Bitstream Vera Sans\"");
            with1.WriteLine("fontsize = 8");
            with1.WriteLine("shape =" + "\"record\"");
            with1.WriteLine("]");
            


             for (int i = 0; i <= Analizador.Clases.Count - 1; i++) {
                 with1.WriteLine(llamarAnalizador.regresar_Nombre_Clase(i) + " [");
                  with1.WriteLine("label =" + "\"" + "{");
                 with1.WriteLine(llamarAnalizador.regresar_Acceso(i) + " " + llamarAnalizador.regresar_Nombre_Clase(i) + "|");
                
                for (int j = 0; j <= Analizador.Atributos.Count - 1; j++) {

                    if (llamarAnalizador.regresar_Nombre_Clase(i) == llamarAnalizador.regresar_Nombre_clases_A(j))
                    {
                        with1.WriteLine(llamarAnalizador.regresar_Acceso_Atrbuto(j));
                        with1.WriteLine(llamarAnalizador.regresar_Nombre_Atributo(j) + " :");
                        with1.WriteLine(llamarAnalizador.regresar_Tipo_Atributo(j) + "\\l");
                    }
                   
                 

                }
                with1.WriteLine("|");
                for (int j = 0; j <= Analizador.Metodos.Count - 1; j++)
                {
                    if (llamarAnalizador.regresar_Nombre_Clase(i) == llamarAnalizador.regresar_Nombre_clases_Metodos(j))
                    {
                        with1.WriteLine(llamarAnalizador.regresar_Acceso_Atrbuto(j));
                        with1.WriteLine(llamarAnalizador.regresar_Nombre_metodos(j) + " :");
                        with1.WriteLine(llamarAnalizador.regresar_tipo_metodos(j) + "\\l");
                    }
                }

                with1.WriteLine("}" + "\"");
                with1.WriteLine("]");
            }

            for (int k = 0; k <= Analizador.conexiones.Count - 1; k++) {
                if (Analizador.Clases.Exists(x => x.Nombre_Clase == llamarAnalizador.regresar_Nombre_claseConexion_Herencia(k)))
                {

                    with1.WriteLine(llamarAnalizador.regresar_Nombre_claseConexion_Herencia(k) + " -> " + llamarAnalizador.regresar_Nombre_clasePrincipal_Herencia(k) + " [dir=back arrowtail = " + "\"empty\"" + "]");
                }
                else {
                    Errores nuevo = new Errores();
                    nuevo.mensaje = "No se encontro la clase " + llamarAnalizador.regresar_Nombre_claseConexion_Herencia(k);
                    nuevo.tipo_error = "Herencia";
                    errores_control.Add(nuevo);
                    continue;
                }
                   
            }

            for (int k = 0; k <= Analizador.Agregacion.Count - 1; k++)

            {
                if (Analizador.Clases.Exists(x => x.Nombre_Clase == llamarAnalizador.regresar_Nombre_claseConexion_Agregacion(k)))
                {

                    with1.WriteLine(llamarAnalizador.regresar_Nombre_clasePrincipal_Agregacion(k) + " -> " + llamarAnalizador.regresar_Nombre_claseConexion_Agregacion(k) + " [dir=back arrowtail = " + "\"ediamond\"" + "]");
                }
                else
                {
                    Errores nuevo = new Errores();
                    nuevo.mensaje = "No se encontro la clase " + llamarAnalizador.regresar_Nombre_claseConexion_Agregacion(k);
                    nuevo.tipo_error = "Agregacion";
                    errores_control.Add(nuevo);
                    continue;
                }
            }


            with1.WriteLine("}");
            with1.Close();
        }

        private void diagramaDeClasesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Escribir();
            string path = Directory.GetCurrentDirectory();
            GenerateGraph("grafo1.txt",path);
            Thread.Sleep(1000);
           // System.Diagnostics.Process.Start("grafo1.png");
             Form2 frm = new Form2();
            frm.Show();

        }

        private static void GenerateGraph(string fileName, string path)
        {
          
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(@"C:\Program Files (x86)\Graphviz2.38\bin\dot.exe");
                startInfo.Arguments = "-Tpng grafo1.txt -o grafo1.png";
                
                Process.Start(startInfo);
             
            }
            catch (Exception x)
            {

            }
        }

        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //este es el de agregar una nueva clase
            if (treeView1.SelectedNode == null)
            {
                MessageBox.Show("No seleciono Proyecto para agregar la clase");
            }
            else
            {
                nombre = Microsoft.VisualBasic.Interaction.InputBox("Introduzca el nombre de la clase", "Proyecto 1 Compiladores");
                nombre_clase = nombre + ".java";
                if (nombre == "")
                {
                    MessageBox.Show("Debe agregarle un nombre a la clase para poder crearlo");
                    return;
                }

                for (int i = 0; i <= control_nodos.Count - 1; i++)
                {
                    if (regresar_nombre_clase(i) == nombre_clase && regresarRaiz(i) == primer_nodo)
                    {
                        MessageBox.Show("Este Nombre de clase ya existe en este proyecto Cambie el nombre");
                        return;
                    }
                }
              
                treeView1.SelectedNode.Nodes.Add(nombre_clase, nombre_clase, 1, 1);
                pestanias.Add(new TabPage());
                txtentrada.Add(new RichTextBox());

                pestanias[numero_pestania].Text = nombre_clase;
                txtentrada[numero_pestania].Size = new Size(808, 350);

                tabControl1.Controls.Add(pestanias[numero_pestania]);
                pestanias[numero_pestania].Controls.Add(txtentrada[numero_pestania]);

                txtentrada[numero_pestania].Font = new Font("Arial", 14, FontStyle.Regular);
                //   txtentrada[numero_pestania].LoadFile(nombre_clase, RichTextBoxStreamType.PlainText);
                txtentrada[numero_pestania].Text = "public class " +nombre + "{" + "\n"+"\n" + "}";
                //txtentrada autosize

                ruta_Archivo = nombre_clase;
                this.tabControl1.SelectedIndex = tabControl1.TabPages.Count - 1;
                Agregar_Lista2();
                numero_pestania = numero_pestania + 1;



            }
        }

        private void eliminarClaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try {
                for (int i = 0; i <= control_nodos.Count - 1; i++) {

                    if (treeView1.SelectedNode.Parent.Index == regresarRaiz(i) && treeView1.Nodes[treeView1.SelectedNode.Parent.Index].Nodes[treeView1.SelectedNode.Index].Text == regresar_nombre_clase(i)) {
                        treeView1.Nodes[treeView1.SelectedNode.Parent.Index].Nodes[treeView1.SelectedNode.Index].Remove();
                        tabControl1.TabPages.Remove(pestanias[regresar_pestania(i)]);
                        control_nodos.RemoveAt(i);
                    }

                }


            } catch (Exception x) {


            }
        }

        private void nuevoProyectoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void agregarClaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFile1.ShowDialog() == DialogResult.OK) {

                nombre_Archivo = Path.GetFileName(openFile1.FileName);
                nombre_clase = nombre_Archivo;

                for(int i = 0; i<= control_nodos.Count-1; i++) {
                    if (regresar_nombre_clase(i) == nombre_Archivo && regresarRaiz(i) == primer_nodo) {
                        MessageBox.Show("Este Nombre de clase ya existe en este proyecto Cambie el nombre");
                        return;
                    }
                }

                treeView1.SelectedNode.Nodes.Add(nombre_clase, nombre_clase, 1, 1);
                pestanias.Add(new TabPage());
                txtentrada.Add(new RichTextBox());

                pestanias[numero_pestania].Text = nombre_clase;
                txtentrada[numero_pestania].Size = new Size(808, 350);

                tabControl1.Controls.Add(pestanias[numero_pestania]);
                pestanias[numero_pestania].Controls.Add(txtentrada[numero_pestania]);

                txtentrada[numero_pestania].Font = new Font("Arial", 14, FontStyle.Regular);
                txtentrada[numero_pestania].LoadFile(openFile1.FileName, RichTextBoxStreamType.PlainText);

                //txtentrada autosize

                ruta_Archivo = openFile1.FileName;
                this.tabControl1.SelectedIndex = tabControl1.TabPages.Count - 1;
                Agregar_Lista2();
                numero_pestania = numero_pestania + 1;
              
            }
        }

        private void reportesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            reporte();
        }

        public void reporte() {



            try
            {
                StreamWriter escritor = default(StreamWriter);
                //declaro la variable escritor

                SaveFileDialog SFD = new SaveFileDialog();
                SFD.Filter = "html|*.html";
                SFD.ShowDialog();
                string ResultadosSFD = null;
                ResultadosSFD = SFD.FileName;
                escritor = new StreamWriter(ResultadosSFD);

                System.Diagnostics.Process.Start(SFD.FileName);
            
                var _with1 = escritor;
                _with1.WriteLine("<html>");
                //crea el encabezado HTML
                _with1.WriteLine("<head>");
                //crea el HEAD de nuestro html
                _with1.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
                _with1.WriteLine("<link rel="+"stylesheet"+" type="+"text/css"+" href="+"CSS.css"+"> ");
                _with1.WriteLine("<style type="+"text/css"+"> {");
                _with1.WriteLine("margin:0px;");
                _with1.WriteLine("padding:0px;");
                _with1.WriteLine("}");
                _with1.WriteLine("div#general{");
                _with1.WriteLine("margin:auto;");
                _with1.WriteLine("margin-top:50px;");
                _with1.WriteLine("width:960px;");
                _with1.WriteLine("height:1100px;");
                _with1.WriteLine("background-color:#9FF781  ;");
               _with1 .WriteLine("text-align: center;");
                _with1.WriteLine("}");
              _with1  .WriteLine("div#texto{");
            _with1    .WriteLine("margin;auto;");
              _with1  .WriteLine("margin-left:50px;");
             _with1   .WriteLine("margin-top:50px;");
            _with1    .WriteLine("margin-right:50px;");
            _with1    .WriteLine("text-align: center;");
                _with1.WriteLine("Background-Color: #86B404  ;");
            _with1    .WriteLine("}");
                _with1.WriteLine("html {");
                _with1.WriteLine("  background-Color: #088A08  ;");
               _with1 .WriteLine("-webkit-background-size: cover;");
                _with1.WriteLine("-moz-background-size: cover;");
                _with1.WriteLine("-o-background-size: cover;");
              _with1  .WriteLine("background-size: cover;");
                _with1.WriteLine("}");
               _with1 .WriteLine("</style>");
               _with1 .WriteLine("<title>" + "REPORTE" + "</title>"); 
               _with1 .WriteLine("</head>");
              _with1  .WriteLine("<body>");
                _with1.WriteLine("<div id="+"texto"+">");
               _with1 .WriteLine("<p> Reportes analisis lexico y sintactico, Compiladores 1 USAC 2017 </p>");
               _with1 .WriteLine("</div> ");
              _with1  .WriteLine("<div id = "+"general"+" >");
                //escribe los META del archivo html
                /*  _with1.WriteLine("<title>" + "REPORTE" + "</title>");
                  //escribe la etiqueta TITLE tomada del nombre con el que se graba el html
                  _with1.WriteLine("</head>");
                  //cierra la etiqueta HEAD
                  _with1.WriteLine("<body>");
                  //escribe la etiqueta BODY
                  _with1.WriteLine("<center>");*/

                _with1.WriteLine("<h1>Tabla de Errores Lexicos y Sintacticos </h1>");
                _with1.WriteLine("<table class=normal align=center border=1 cellspacing=0 cellpadding=2>");
                _with1.WriteLine("<tr>");
                _with1.WriteLine("<td> No </td>");
                _with1.WriteLine("<td> Descripcion </td>");
                _with1.WriteLine("<td> Tipo de error </td>");
                _with1.WriteLine("<td> Linea</td>");
                _with1.WriteLine("<td> Columna</td>");
                _with1.WriteLine("</tr>");
                for (int i = 0; i <= Analizador.err.Count - 1; i++)
                {
                    _with1.WriteLine("<tr>");
                    _with1.WriteLine("<td>" + Convert.ToString(i + 1) + "</td>");
              
                   _with1.WriteLine("<td>" + llamarAnalizador.Error_descripcion(i) + "</td>");
                    _with1.WriteLine("<td>" + llamarAnalizador.Error_tipo(i) + "</td>");
                    _with1.WriteLine("<td>" + llamarAnalizador.Error_linea(i) + "</td>");
                    _with1.WriteLine("<td>" + llamarAnalizador.Error_columna(i) + "</td>");

                }
                _with1.WriteLine("</table>");


                _with1.WriteLine("<h1>Tabla de Errores de Control</h1>");
                _with1.WriteLine("<table class=normal align=center border=1 cellspacing=0 cellpadding=2>");
                _with1.WriteLine("<tr>");
                _with1.WriteLine("<td> No </td>");
                _with1.WriteLine("<td> Descripcion </td>");
                _with1.WriteLine("<td> Tipo de error </td>");

                _with1.WriteLine("</tr>");
                for (int i = 0; i <= errores_control.Count - 1; i++)
                {
                    _with1.WriteLine("<tr>");
                    _with1.WriteLine("<td>" + Convert.ToString(i + 1) + "</td>");
                    _with1.WriteLine("<td>" + Error_descripcion(i) + "</td>");
                    _with1.WriteLine("<td>" + Error_tipo(i) + "</td>");

                }
                _with1.WriteLine("</table>");

                _with1.WriteLine("</table>");


                _with1.WriteLine("</center");
                _with1.WriteLine("</body>");
                //cierra la etiqueta BODY
                _with1.WriteLine("</html>");
                //cierra la etiqueta HTML

                _with1.Close();
                //termina el proceso y crea el archivo

            }
            catch (Exception ex)
            {
            }


        }


        public string Error_descripcion(int i)
        {

            try
            {
                string salida = "";
                Errores nodo = errores_control[i];

                salida = nodo.mensaje;
                return salida;
            }
            catch (Exception e)
            {
                return "";
            }

        }

        public string Error_tipo(int i)
        {

            try
            {
                string salida = "";
                Errores nodo = errores_control[i];

                salida = nodo.tipo_error;
                return salida;
            }
            catch (Exception e)
            {
                return "";
            }

        }

    }


}


