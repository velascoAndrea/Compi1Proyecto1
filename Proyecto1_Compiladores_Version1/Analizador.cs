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
    class Analizador
    {
        public static ParseTree padre;
       public static List<Diagrama_de_Clases> Clases = new List<Diagrama_de_Clases>();
        public static List<Diagrama_de_Clases> Atributos = new List<Diagrama_de_Clases>();
        public static List<Diagrama_de_Clases> Metodos = new List<Diagrama_de_Clases>();
        public static List<Diagrama_de_Clases> conexiones = new List<Diagrama_de_Clases>();
        public static List<Diagrama_de_Clases> Agregacion = new List<Diagrama_de_Clases>();
        public static List<Errores> err = new List<Errores>();
        public Stack<string> continues_ciclos = new Stack<string>();
        public Stack<string> breaks_ciclos_S = new Stack<string>();
        public Stack<string> returns = new Stack<string>();

        public bool esCadenaValida(string cadenaEntrada, Grammar gramatica)
        {
            LanguageData lenguaje = new LanguageData(gramatica);
            Parser p = new Parser(lenguaje);
            ParseTree arbol = p.Parse(cadenaEntrada);
            padre = arbol;
         return arbol.Root != null;
           
        }

        public ParseTreeNode analizar(string cadenaEntrada) {
            Gramatica gramatica = new Gramatica();
            LanguageData lenguaje = new LanguageData(gramatica);
            Parser p = new Parser(lenguaje);
            ParseTree arbol = p.Parse(cadenaEntrada);
            ParseTreeNode raiz = arbol.Root;

            if (raiz == null)
            {
                for (int i = 0; i < arbol.ParserMessages.Count(); i++) {
                    Errores nuevo = new Errores();
                    nuevo.mensaje = arbol.ParserMessages.ElementAt(i).Message.ToString();
                    nuevo.linea = arbol.ParserMessages.ElementAt(i).Location.Line.ToString();
                    nuevo.columna = arbol.ParserMessages.ElementAt(i).Location.Column.ToString();
                    
                    //  MessageBox.Show(arbol.ParserMessages.ElementAt(i).Level.ToString());
                    if (arbol.ParserMessages.ElementAt(i).Message.ToString().StartsWith("Invalid"))
                    {
                        //MessageBox.Show("error lexico");
                        nuevo.tipo_error = "Error lexico";
                    }
                    else {

                        nuevo.tipo_error = "Error Sintactico";
                      //  MessageBox.Show("Se encontro " + arbol.ParserMessages.ElementAt(i).ParserState.ToString());
                    }

                    err.Add(nuevo);

                }
              
                return null;
            }
            else {
                for (int i = 0; i < arbol.ParserMessages.Count(); i++)
                {
                    Errores nuevo = new Errores();
                    nuevo.mensaje = arbol.ParserMessages.ElementAt(i).Message.ToString();
                    nuevo.linea = arbol.ParserMessages.ElementAt(i).Location.Line.ToString();
                    nuevo.columna = arbol.ParserMessages.ElementAt(i).Location.Column.ToString();

                    if (arbol.ParserMessages.ElementAt(i).Message.ToString().StartsWith("Invalid"))
                    {
                        //MessageBox.Show("error lexico");
                        nuevo.tipo_error = "Error lexico";
                    }
                    else
                    {

                        nuevo.tipo_error = "Error Sintactico";
                       // MessageBox.Show("Se encontro " + arbol.Tokens[0]);
                    }
                    err.Add(nuevo);
                //    MessageBox.Show(arbol.ParserMessages.ElementAt(i).Message.TrimEnd().ToString());
                    
                }

                return raiz;
            }

        }



        static string acc = "";
        static string tipo = "";
        static string tipo2 = "";
        static string classs = "";
        static string identi = "";
        static string param = "";
        static string linea = "";
        static string columna = "";


        public string recorrerArbol(ParseTreeNode raiz)
        {
           
           
            string Inicio = raiz.ToString();
            ParseTreeNode[] hijos = null;
            if (raiz.ChildNodes.Count > 0)
            {
                hijos = raiz.ChildNodes.ToArray();
            }

            switch (Inicio)
            {
                case "S":
                    {
                        string resultado = recorrerArbol(hijos[0]);
                        return resultado;
                    }
                case "clase":
                    {
                        string acceso = recorrerArbol(hijos[0]); //acceso
                        string clase = hijos[1].ToString().Replace(" (Keyword)", ""); //clase
                        string ID_clase = hijos[2].ToString().Replace(" (Identifier)", "");
                        classs = ID_clase;
                        string Herencia = recorrerArbol(hijos[3]);
                        string llav_izq = recorrerArbol(hijos[4]);
                        string Instrucciones_clase = recorrerArbol(hijos[5]);
                        string llav_der = recorrerArbol(hijos[6]);
                    
                        Diagrama_de_Clases nuevo = new Diagrama_de_Clases();
                        nuevo.Acceso = acceso;
                        nuevo.Nombre_Clase = ID_clase;                       
                        Clases.Add(nuevo);
                        //MessageBox.Show(acceso + " " + ID_clase);
                        
                        //MessageBox.Show(classs);
                        return Instrucciones_clase;
                    }
                case "Acceso":
                    {
                        if (raiz.ChildNodes.Count == 1)
                        {
                            string acceso = hijos[0].ToString().Replace(" (Keyword)", "");
                           // acc = acceso;
                            if (acceso == "public") {
                                acc = "+";
                            }
                            else if (acceso == "private")
                            {
                                acc = "-";
                            }
                            else if (acceso == "protected")
                            {
                                acc = "#";
                            }

                            return acceso;
                        }
                        acc = "+"; // aqui tene que ir + - # en estemetodo para el diagrama de clases
                        return "public";
                    }

                case "Herencia":
                    {
                        if (raiz.ChildNodes.Count == 2)
                        {
                            string id_herencia = hijos[1].ToString().Replace(" (Identifier)", "");

                            Diagrama_de_Clases nuevo = new Diagrama_de_Clases();
                            nuevo.Nombre_Clase = classs;
                            nuevo.Nombre_Clase_Conexion = id_herencia;
                            conexiones.Add(nuevo);

                            return id_herencia;

                            //guardar las conexiones
                        }
                        return "";
                    }

                case "Instrucciones_Clase":
                    {
                        /*Instrucciones_Clase + Instrucciones_Clase2 // primer camino
                        |Instrucciones_Clase2; // recursivo
                         */
                        if (raiz.ChildNodes.Count == 2)
                        {
                         //   MessageBox.Show("aqui va por Instrucciones de clase");
                            string operador = recorrerArbol(hijos[0]);
                            string operador2 = recorrerArbol(hijos[1]);
                            string recursivo = operador + operador2;
                            return recursivo;
                        }
                       else if (raiz.ChildNodes.Count == 1)
                        {
                           // MessageBox.Show("aqui va por Instrucciones de clase");
                            string operador = recorrerArbol(hijos[0]);
                            return operador;
                        }


                        return "";

                    }
                case "Instrucciones_Clase2":
                    {
                        if (raiz.ChildNodes.Count == 2)
                        {
                            //Acceso + Instrucciones_Clase3 
                       //     MessageBox.Show("aqui va por Instrucciones de clase2");
                            string acceso = recorrerArbol(hijos[0]); //acceso
                            string Instrucciones_clase3 = recorrerArbol(hijos[1]);
                            return Instrucciones_clase3;

                        }
                        else if (raiz.ChildNodes.Count == 3) {

                            //Sobreescribir + Acceso +Sobre_Metodo_Funcion
                            string Sobreescribe = recorrerArbol(hijos[0]); //acceso
                            string Acceso = recorrerArbol(hijos[1]);

                            string Sobre_Metodo_Funcion = recorrerArbol(hijos[2]);
                            return Sobre_Metodo_Funcion;
                         
                           
                        }
                            return "";
                    }
                case "Instrucciones_Clase3":
                    {
                        if (raiz.ChildNodes.Count == 2)
                        {
                            //Tipo + Instrucciones_Clase4 
                          //  MessageBox.Show("aqui va por Instrucciones de clase3");
                            string tipo23 = recorrerArbol(hijos[0]); //tipo
                           
                            string Instrucciones_clase4 = recorrerArbol(hijos[1]);
                            return Instrucciones_clase4;

                        }
                        else if (raiz.ChildNodes.Count == 8)
                        {
                            //Void + ID +parentesisIzquierdo +Parametros+ parentesisDerecho + llaveIzquierda  +L_Sentencias+ llaveDerecha
                            string ID = hijos[1].ToString().Replace(" (Identifier)", "");
                            string parametros = recorrerArbol(hijos[3]);
                            Diagrama_de_Clases nuevo = new Diagrama_de_Clases();
                            nuevo.Tipo = "void";
                            nuevo.Nombre_Metodos = ID + "(" + param+ ")";
                            nuevo.Nombre_Clase = classs;
                            nuevo.Acceso = acc;
                            Metodos.Add(nuevo);
                         //   MessageBox.Show(ID + "(" + param + ") : void");
                            string L_Sentencias = recorrerArbol(hijos[6]);
                            return L_Sentencias; // para el void
                        }
                        else if (raiz.ChildNodes.Count == 1)
                        {
                            //constructor_Atrib; 
                            string constructor_Atrib = recorrerArbol(hijos[0]);
                            return constructor_Atrib;
                           // para el constructor con atributos
                        }

                        return "";

                    }
                case "Instrucciones_Clase4":
                    {

                        if (raiz.ChildNodes.Count == 2)
                        {
                            //ID Instrucciones clase5
                         //   MessageBox.Show("aqui va por Instrucciones de clase4");
                            string ID = hijos[0].ToString().Replace(" (Identifier)", ""); //ID ya sea para declarar variable o el id de la funcion
                            identi = ID;
                          
                         //  MessageBox.Show(classs);
                            //     MessageBox.Show(acc + " " + tipo + " " + ID);
                            string Instrucciones_clase5 = recorrerArbol(hijos[1]);                      
                            return Instrucciones_clase5;
                           
                        }
                        return "";
                    }
                case "Instrucciones_Clase5":
                    {

                        if (raiz.ChildNodes.Count == 3)
                        {
                            //Extension  + Asigna + PuntoyComa 
                            string Extension = recorrerArbol(hijos[0]);
                            string Asigna = recorrerArbol(hijos[1]);
                            string punto_coma = recorrerArbol(hijos[2]);
                            Diagrama_de_Clases nuevo = new Diagrama_de_Clases();
                            nuevo.Acceso = acc;
                            nuevo.Tipo = tipo;
                            nuevo.Nombre_Atributo = identi;
                            nuevo.Nombre_Clase = classs;
                            Atributos.Add(nuevo);

                            return Extension;



                        }
                        else if (raiz.ChildNodes.Count == 6)
                        {
                            //parentesisIzquierdo + Parametros + parentesisDerecho + llaveIzquierda + L_Sentencias + llaveDerecha
                            // MessageBox.Show("va pasando por la funcion");


                            string parametros = recorrerArbol(hijos[1]);
                            Diagrama_de_Clases nuevo = new Diagrama_de_Clases();
                            nuevo.Tipo = tipo;
                            nuevo.Nombre_Metodos = identi + "(" + param + ")";
                            nuevo.Nombre_Clase = classs;
                            nuevo.Acceso = acc;
                            Metodos.Add(nuevo);
                            returns.Push("return");
                            string L_Sentencias = recorrerArbol(hijos[4]);
                            return L_Sentencias + returns.Pop();

                           

                        }
                        return "";
                    }
                case "Extension":
                    {
                        if (raiz.ChildNodes.Count == 2)
                        {
                            //Extension + Exte2
                            //donde se agregan los otros ID

                           // MessageBox.Show("aqui va por Extension");
                            string operador = recorrerArbol(hijos[0]);
                            string operador2 = recorrerArbol(hijos[1]);
                            string recursivo = operador + operador2;
                            return recursivo;


                            

                        }
                        if (raiz.ChildNodes.Count == 1)
                        {
                            //Exte2
                         //   MessageBox.Show("aqui va por Exte_2");
                            string operador = recorrerArbol(hijos[0]);
                            return operador;

                    

                        }
                        return "";
                    }
                case "Exte2":
                    {
                        if (raiz.ChildNodes.Count == 2)
                        {
                            //coma + ID
                            string coma = hijos[0].ToString().Replace(" (Keyword)", "");
                            string ID = hijos[1].ToString().Replace(" (Identifier)", "");
                            Diagrama_de_Clases nuevo = new Diagrama_de_Clases();
                            nuevo.Acceso = acc;
                            nuevo.Tipo = tipo;
                            nuevo.Nombre_Atributo = ID;
                            nuevo.Nombre_Clase = classs;
                            Atributos.Add(nuevo);
                            //  MessageBox.Show(acc + " " + tipo + " " + ID);
                         //   MessageBox.Show(clases_ID);
                            return ID;
                        }
                        return "";
                    }


                case "Tipo":
                    {

                        if (raiz.ChildNodes.Count == 1)
                        {
                            string tip = hijos[0].ToString().Replace(" (Keyword)", "");
                            tipo = tip;
                            return tip;
                        }
                    
                        return "";//tipos
                    }
                case "Tipo2":
                    {

                        if (raiz.ChildNodes.Count == 1)
                        {
                            string tip = hijos[0].ToString().Replace(" (Keyword)", "");
                           
                            string tip2 = hijos[0].ToString();

                            if (tip2 == tip)
                            {
                                tipo2 = tip2 = hijos[0].ToString().Replace(" (Identifier)", "");
                                return tip2;
                            }
                            else {
                                tipo2 = tip;
                                return tip;
                            }
                                                    
                            
                        }

                        return "";//tipos
                    }
                case "constructor_Atrib":
                    {
                        //ID + constructor_Atrib2
                        string ID = hijos[0].ToString().Replace(" (Identifier)", "");
                        tipo = ID; // si toma el camino de funcion o declaracion de un objeto
                        identi = ID; // si toma el camino del constructor
                        string constructor_Atrib2 = recorrerArbol(hijos[1]);
                        return constructor_Atrib2;
                        
                    }
                case "constructor_Atrib2":
                    {

                        if (raiz.ChildNodes.Count == 2)
                        {
                            //ID + constructor_Atrib3
                            string ID = hijos[0].ToString().Replace(" (Identifier)", "");
                            identi = ID;

                            string constructor_Atrib3 = recorrerArbol(hijos[1]);
                            return constructor_Atrib3;

                        }
                        else if (raiz.ChildNodes.Count == 6)
                        {
                            // parentesisIzquierdo + Parametros + parentesisDerecho + llaveIzquierda + L_Sentencias + llaveDerecha; 
                            string parametros = recorrerArbol(hijos[1]);
                            Diagrama_de_Clases nuevo = new Diagrama_de_Clases();
                            nuevo.Tipo = "";// aqui va el contructor
                            nuevo.Nombre_Metodos = identi + "(" + param + ")";
                            nuevo.Nombre_Clase = classs;
                            nuevo.Acceso = acc;
                            Metodos.Add(nuevo);
                            string L_Sentencias = recorrerArbol(hijos[4]);
                            return L_Sentencias;


                        }



                        return "";
                    }

                case "constructor_Atrib3":
                    {
                        if (raiz.ChildNodes.Count == 3)
                        {
                            //Extension + Asigna + PuntoyComa
                            Diagrama_de_Clases nuevo = new Diagrama_de_Clases();
                            nuevo.Acceso = acc;
                            nuevo.Tipo = tipo;
                            nuevo.Nombre_Atributo = identi;
                            nuevo.Nombre_Clase = classs;
                            Atributos.Add(nuevo);

                            if (Agregacion.Count == 0)
                            {

                                Diagrama_de_Clases nuevo2 = new Diagrama_de_Clases();//VALIDAR QUE SOLO SE PUEDA AGREGAR UNA CONEXION CON UNA CLASE
                                nuevo2.Nombre_Clase = classs;
                                nuevo2.Nombre_Clase_Conexion = tipo;
                                Agregacion.Add(nuevo2);

                            }
                            else 
                            {

                                if (Agregacion.Exists(x => x.Nombre_Clase == classs && x.Nombre_Clase_Conexion == tipo)) 
                                {
                                    //MessageBox.Show(classs + " " + tipo + " " + identi);
                                    //no se agrega

                                }
                                else {
                                    Diagrama_de_Clases nuevo2 = new Diagrama_de_Clases();//VALIDAR QUE SOLO SE PUEDA AGREGAR UNA CONEXION CON UNA CLASE
                                    nuevo2.Nombre_Clase = classs;
                                    nuevo2.Nombre_Clase_Conexion = tipo;
                                    Agregacion.Add(nuevo2);
                                }
                    
                            }
                            string Extension = recorrerArbol(hijos[0]);
                            return Extension;



                        }
                        else if (raiz.ChildNodes.Count == 6)
                        {
                            //parentesisIzquierdo + Parametros + parentesisDerecho + llaveIzquierda + L_Sentencias + llaveDerecha
                            string parametros = recorrerArbol(hijos[1]);
                            Diagrama_de_Clases nuevo = new Diagrama_de_Clases();
                            nuevo.Tipo = tipo;
                            nuevo.Nombre_Metodos = identi + "(" + param + ")";
                            nuevo.Nombre_Clase = classs;
                            nuevo.Acceso = acc;
                            Metodos.Add(nuevo);
                            returns.Push("return");
                            string L_Sentencias = recorrerArbol(hijos[4]);
                            return L_Sentencias + returns.Pop();
                        }

                        return "";
                    }
                case "Parametos":
                    {
                        /*                        
                          Tipo2 + ID + Parametros2
                             | Empty;

                         */
                        if (raiz.ChildNodes.Count == 3)
                        {
                            string Tipo2 = recorrerArbol(hijos[0]);
                            string ID = hijos[1].ToString().Replace(" (Identifier)", "");
                            param = tipo2 + " " + ID;
                            string parametros2 = recorrerArbol(hijos[2]);
                            return  parametros2;
                        }
                        param = "";
                            return "";
                    }
                case "Parametos2":
                    {
                        if (raiz.ChildNodes.Count == 2)
                        {
                            //Parametros2 + Parametros3
                     
                            string operador = recorrerArbol(hijos[0]);
                            string operador2 = recorrerArbol(hijos[1]);
                            string recursivo = operador + operador2;
                            return recursivo;




                        }
                        if (raiz.ChildNodes.Count == 1)
                        {
                            //Parametros3;

                            string operador = recorrerArbol(hijos[0]);
                            return operador;



                        }
                        return "";
                    }
                case "Parametos3":
                    {
                        if (raiz.ChildNodes.Count == 3)
                        {
                            //coma + Tipo2 + ID
                            string coma = hijos[0].ToString().Replace(" (Key symbol)", "");
                            string Tipo2 = recorrerArbol(hijos[1]);
                            tipo2 = Tipo2;
                            string ID = hijos[2].ToString().Replace(" (Identifier)", "");
                            param = param + coma + " " + tipo2 +" "+ ID;
                            return coma + tipo2 + ID;
                        }
                        return "";
                    }
                case "Sobre_Metodo_Funcion":
                    {
                        if (raiz.ChildNodes.Count == 8)
                        {

                            /*
                              Tipo + ID + parentesisIzquierdo + Parametros + parentesisDerecho + llaveIzquierda + L_Sentencias + llaveDerecha
                                       | Void + ID + parentesisIzquierdo + Parametros + parentesisDerecho + llaveIzquierda + L_Sentencias + llaveDerecha
                                       | ID + ID + parentesisIzquierdo + Parametros + parentesisDerecho + llaveIzquierda + L_Sentencias + llaveDerecha;
                             
                             */
                            if (hijos[0].ToString().Replace(" (Keyword)", "") != "void")
                            {

                                string tipo23 = recorrerArbol(hijos[0]);
                                string ID = hijos[1].ToString().Replace(" (Identifier)", "");
                                string parametros = recorrerArbol(hijos[3]);
                                Diagrama_de_Clases nuevo = new Diagrama_de_Clases();
                                nuevo.Tipo = tipo;
                                nuevo.Nombre_Metodos = ID + "(" + param + ")";
                                nuevo.Nombre_Clase = classs;
                                nuevo.Acceso = acc;
                                Metodos.Add(nuevo);
                                returns.Push("return");
                                string L_Sentencias = recorrerArbol(hijos[6]);
                                return L_Sentencias + returns.Pop();
                            }
                            else {
                                string tipo23 = recorrerArbol(hijos[0]);
                                string ID = hijos[1].ToString().Replace(" (Identifier)", "");
                                string parametros = recorrerArbol(hijos[3]);
                                Diagrama_de_Clases nuevo = new Diagrama_de_Clases();
                                nuevo.Tipo = "void";
                                nuevo.Nombre_Metodos = ID + "(" + param + ")";
                                nuevo.Nombre_Clase = classs;
                                nuevo.Acceso = acc;
                                Metodos.Add(nuevo);
                                string L_Sentencias = recorrerArbol(hijos[6]);
                                return L_Sentencias;

                            }
                        }
                        return "";
                    }

                case "L_Sentencias":
                    {
                        if (raiz.ChildNodes.Count == 2)
                        {
                            //L_Sentencias + Sentencias

                            string operador = recorrerArbol(hijos[0]);
                            string operador2 = recorrerArbol(hijos[1]);
                            string recursivo = operador + operador2;
                            return recursivo;




                        }
                        if (raiz.ChildNodes.Count == 1)
                        {
                            //Sentencias
                            string operador = recorrerArbol(hijos[0]);
                            return operador;



                        }

                        return "";

                    }

                case "Sentecias":
                    {


                        if (raiz.ChildNodes.Count == 7)
                        {
                            // para el while

                            string whiles = hijos[0].ToString();
                            continues_ciclos.Push("while");
                            breaks_ciclos_S.Push("break");
                            string L_sentencias = recorrerArbol(hijos[5]);
                           // MessageBox.Show(whiles);
                            return L_sentencias + continues_ciclos.Pop() + breaks_ciclos_S.Pop();



                        }
                        else if (raiz.ChildNodes.Count == 8)
                        {
                            //para el if
                            string L_sentencias = recorrerArbol(hijos[5]);
                            string Elses = recorrerArbol(hijos[7]); // arreglar lo de los elses
                            return L_sentencias + Elses;
                        }
                        else if (raiz.ChildNodes.Count == 9)
                        {
                            // para do while
                            continues_ciclos.Push("Dowhile");
                            breaks_ciclos_S.Push("break");
                            string L_sentencias = recorrerArbol(hijos[2]);
                            return L_sentencias + continues_ciclos.Pop() + breaks_ciclos_S.Pop();
                        }
                        else if (raiz.ChildNodes.Count == 11)
                        {
                            //for
                            continues_ciclos.Push("for");
                            breaks_ciclos_S.Push("break");
                            string L_sentencias = recorrerArbol(hijos[9]);
                            return L_sentencias +continues_ciclos.Pop() + breaks_ciclos_S.Pop();
                        }
                        else if (raiz.ChildNodes.Count == 14)
                        {
                            //switch
                            breaks_ciclos_S.Push("break"); //recorrer el switch
                            string L_sentencias = recorrerArbol(hijos[8]);
                            string switch2 = recorrerArbol(hijos[9]);
                            string L_sentencias_D = recorrerArbol(hijos[12]);
                            return L_sentencias +switch2  + L_sentencias_D +breaks_ciclos_S.Pop();
                        }
                        else if (raiz.ChildNodes.Count == 3)
                        {
                            //return
                            if (hijos[0].ToString().Replace(" (Keyword)", "") == "return")
                            {
                                string condicion = recorrerArbol(hijos[1]);
                                linea = hijos[0].Token.Location.Line.ToString();
                                columna = hijos[0].Token.Location.Column.ToString();
                                return condicion;

                            }

                            }
                        else if (raiz.ChildNodes.Count == 2)
                        {
                            //para los braks
                            if (hijos[0].ToString().Replace(" (Keyword)", "") == "break") {

                                if (breaks_ciclos_S.Count == 0)
                                {
                                    MessageBox.Show("Error de ambito en el break");
                                    Errores nuevo = new Errores();
                                    nuevo.mensaje = "Error de ambito en el break";
                                    nuevo.linea = hijos[0].Token.Location.Line.ToString();
                                    nuevo.columna = hijos[0].Token.Location.Column.ToString();
                                    nuevo.tipo_error = "Error Sintactico";
                                    err.Add(nuevo);
                                }
                                /*else {
                                    MessageBox.Show("exiiiito");
                                }*/
                            }
                            if (hijos[0].ToString().Replace(" (Keyword)", "") == "continue")
                            {
                                if (continues_ciclos.Count == 0)
                                {
                                    MessageBox.Show("Error de ambito en el continue");
                                    Errores nuevo = new Errores();
                                    nuevo.mensaje = "Error de ambito en continue";
                                    nuevo.linea = hijos[0].Token.Location.Line.ToString();
                                    nuevo.columna = hijos[0].Token.Location.Column.ToString();
                                    nuevo.tipo_error = "Error Sintactico";
                                    err.Add(nuevo);
                                }
                                /*else
                                {
                                    MessageBox.Show("exiiiito");
                                }*/
                            }




                            }
                        return "";
                    }

                case "Switch2":
                    {
                        if (raiz.ChildNodes.Count == 2)
                        {
                            //L_Sentencias + Sentencias

                            string operador = recorrerArbol(hijos[0]);
                            string operador2 = recorrerArbol(hijos[1]);
                            string recursivo = operador + operador2;
                            return recursivo;




                        }
                        if (raiz.ChildNodes.Count == 1)
                        {
                            //Sentencias
                            string operador = recorrerArbol(hijos[0]);
                            return operador;



                        }
                        return "";

                    }
                case "Switch3":
                    {
                        if (raiz.ChildNodes.Count == 4)
                        {
                            string operador = recorrerArbol(hijos[3]);
                            return operador;
                        }
                        return "";
                    }

                case "Elses":
                    {
                        if (raiz.ChildNodes.Count == 2)
                        {
                            string operador = recorrerArbol(hijos[1]);
                            
                            return operador;
                            
                        }
                        return "";
                    }
                case "Elses2":
                    {
                        if (raiz.ChildNodes.Count == 3)
                        {
                            string operador = recorrerArbol(hijos[1]);
                            return operador;
                        }
                        else if (raiz.ChildNodes.Count == 8)
                        {
                            string operador = recorrerArbol(hijos[5]);
                            string operador2 = recorrerArbol(hijos[7]);
                            return operador + operador2;
                        }
                        return "";
                    }
                case "Expresion_Return":
                    {
                        if (raiz.ChildNodes.Count == 1 && returns.Count == 0)
                        {
                            MessageBox.Show("Error en el return");
                            Errores nuevo = new Errores();
                            nuevo.mensaje = "El return no puede ir acompañado de una expresion";
                            nuevo.linea = linea;
                            nuevo.columna = columna;
                            nuevo.tipo_error = "Error Sintactico";
                            err.Add(nuevo);
                        }else if (raiz.ChildNodes.Count == 0 && returns.Count > 0)
                        {
                            MessageBox.Show("Error en el return");
                            Errores nuevo = new Errores();
                            nuevo.mensaje = "El return esperaba una expresion";
                            nuevo.linea = linea;
                            nuevo.columna = columna;
                            nuevo.tipo_error = "Error Sintactico";
                            err.Add(nuevo);
                        }
                        return "";

                    }

                    }
           

                return "Ultimo Return";
        }


        public string regresar_Acceso(int i)
        {
            try { 
            string salida = "";
            Diagrama_de_Clases nodo = Clases[i];

            salida = nodo.Acceso;
            return salida;
            }
            catch (Exception e)
            {
                return "";
            }
        }

        public string regresar_Nombre_Clase(int i)
        {
            try { 
            string salida = "";
            Diagrama_de_Clases nodo = Clases[i];

            salida = nodo.Nombre_Clase;
            return salida;
            }
            catch (Exception e)
            {
                return "";
            }

        }

        public string regresar_Acceso_Atrbuto(int i)
        {
            try {
                string salida = "";
                Diagrama_de_Clases nodo = Atributos[i];

                salida = nodo.Acceso;
                return salida;
            } catch (Exception e) {
                return "";
            }
          

        }


        public string regresar_Tipo_Atributo(int i)
        {
            try { 
            string salida = "";
            Diagrama_de_Clases nodo = Atributos[i];

            salida = nodo.Tipo;
            return salida;
            }
            catch (Exception e)
            {
                return "";
            }

        }
        public string regresar_Nombre_Atributo(int i)
        {
            try { 
            string salida = "";
            Diagrama_de_Clases nodo = Atributos[i];

            salida = nodo.Nombre_Atributo;
            return salida;
            }
            catch (Exception e)
            {
                return "";
            }

        }

        public string regresar_Nombre_clases_A(int i)
        {
            try { 
            string salida = "";
            Diagrama_de_Clases nodo = Atributos[i];

            salida = nodo.Nombre_Clase;
            return salida;
            }
            catch (Exception e)
            {
                return "";
            }
        }


        public string regresar_Acceso_metodos(int i)
        {
            try { 
            string salida = "";
            Diagrama_de_Clases nodo = Metodos[i];

            salida = nodo.Acceso;
            return salida;
            }
            catch (Exception e)
            {
                return "";
            }
        }

        public string regresar_Nombre_metodos(int i)
        {
            try { 
            string salida = "";
            Diagrama_de_Clases nodo = Metodos[i];

            salida = nodo.Nombre_Metodos;
            return salida;
            }
            catch (Exception e)
            {
                return "";
            }
        }

        public string regresar_tipo_metodos(int i)
        {
            try { 
            string salida = "";
            Diagrama_de_Clases nodo = Metodos[i];

            salida = nodo.Tipo;
            return salida;
            }
            catch (Exception e)
            {
                return "";
            }
        }

        public string regresar_Nombre_clases_Metodos(int i)
        {
            try { 
            string salida = "";
            Diagrama_de_Clases nodo = Metodos[i];

            salida = nodo.Nombre_Clase;
            return salida;
            }
            catch (Exception e)
            {
                return "";
            }
        }


        public string regresar_Nombre_clasePrincipal_Herencia(int i)
        {
            try
            {
                string salida = "";
                Diagrama_de_Clases nodo = conexiones[i];

                salida = nodo.Nombre_Clase;
                return salida;
            }
            catch (Exception e)
            {
                return "";
            }
        }
        public string regresar_Nombre_claseConexion_Herencia(int i)
        {
            try
            {
                string salida = "";
                Diagrama_de_Clases nodo = conexiones[i];

                salida = nodo.Nombre_Clase_Conexion;
                return salida;
            }
            catch (Exception e)
            {
                return "";
            }
        }

        public string regresar_Nombre_clasePrincipal_Agregacion(int i)
        {
            try
            {
                string salida = "";
                Diagrama_de_Clases nodo = Agregacion[i];

                salida = nodo.Nombre_Clase;
                return salida;
            }
            catch (Exception e)
            {
                return "";
            }
        }
        public string regresar_Nombre_claseConexion_Agregacion(int i)
        {
            try
            {
                string salida = "";
                Diagrama_de_Clases nodo = Agregacion[i];

                salida = nodo.Nombre_Clase_Conexion;
                return salida;
            }
            catch (Exception e)
            {
                return "";
            }
        }

        public string Error_linea(int i) {

            try
            {
                string salida = "";
                Errores nodo = err[i];

                salida = nodo.linea;
                return salida;
            }
            catch (Exception e)
            {
                return "";
            }

        }


        public string Error_columna(int i)
        {

            try
            {
                string salida = "";
                Errores nodo = err[i];

                salida = nodo.columna;
                return salida;
            }
            catch (Exception e)
            {
                return "";
            }

        }

        public string Error_descripcion(int i)
        {

            try
            {
                string salida = "";
                Errores nodo = err[i];

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
                Errores nodo = err[i];

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
