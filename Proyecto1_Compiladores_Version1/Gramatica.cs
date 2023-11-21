using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using Irony.Ast;

namespace Proyecto1_Compiladores_Version1
{
    class Gramatica : Grammar
    {


        public Gramatica()
           : base(caseSensitive: true)
        {
            //Palabras clave
            KeyTerm mas = ToTerm("+");
            KeyTerm menos = ToTerm("-");
            KeyTerm por = ToTerm("*");
            KeyTerm division = ToTerm("/");
            KeyTerm llaveIzquierda = ToTerm("{");
            KeyTerm llaveDerecha = ToTerm("}");
            KeyTerm PuntoyComa = ToTerm(";");
            KeyTerm punto = ToTerm(".");
            KeyTerm coma = ToTerm(",");
            KeyTerm DosPuntos = ToTerm(":");
            KeyTerm parentesisIzquierdo = ToTerm("(");
            KeyTerm parentesisDerecho = ToTerm(")");
            KeyTerm And = ToTerm("&&");
            KeyTerm Or = ToTerm("||");
            KeyTerm MayorQue = ToTerm(">");
            KeyTerm MenosQue = ToTerm("<");
            KeyTerm MenorIgual = ToTerm("<=");
            KeyTerm MayorIgual = ToTerm(">=");
            KeyTerm Asig = ToTerm("=");
            KeyTerm Igual = ToTerm("==");
            KeyTerm Not = ToTerm("!");
            KeyTerm Diferente = ToTerm("!=");
            KeyTerm Public = ToTerm("public");
            KeyTerm Private = ToTerm("private");
            KeyTerm Protected = ToTerm("protected");
            KeyTerm Int = ToTerm("int");
            KeyTerm Double = ToTerm("double");
            KeyTerm String = ToTerm("String");
            KeyTerm Char = ToTerm("char");
            KeyTerm Bool = ToTerm("boolean");
            KeyTerm True = ToTerm("true");
            KeyTerm False = ToTerm("false");
            KeyTerm objeto = ToTerm("Object");
            KeyTerm Sobreescribir = ToTerm("@Override");
            KeyTerm Class = ToTerm("class");
            KeyTerm Extends = ToTerm("extends");
            KeyTerm This = ToTerm("this");
            KeyTerm Void = ToTerm("void");
            KeyTerm If = ToTerm("if");
            KeyTerm Else = ToTerm("else");
            KeyTerm While = ToTerm("while");
            KeyTerm For = ToTerm("for");
            KeyTerm Switch = ToTerm("switch");
            KeyTerm Case = ToTerm("case");
            KeyTerm Default = ToTerm("default");
            KeyTerm Do = ToTerm("do");
            KeyTerm Return = ToTerm("return");
            KeyTerm Continue = ToTerm("continue");
            KeyTerm Break = ToTerm("break");
            KeyTerm New = ToTerm("new");
            KeyTerm Nulo = ToTerm("null");
            KeyTerm Aumento = ToTerm("++");
            KeyTerm Disminuye = ToTerm("--");

            MarkReservedWords("this","true","false","null");




            //Terminales
            //DECLARACION DE TERMINALES POR MEDIO DE ER.
            RegexBasedTerminal num = new RegexBasedTerminal("num", "[0-9]+");
            RegexBasedTerminal dobles = new RegexBasedTerminal("numeros", "([+-]?\\d+(\\.\\d+)?([Ee]\\d+)? ?)+");
            IdentifierTerminal ID = new IdentifierTerminal("Identifier", "_-", "");
            StringLiteral CharLiteral = new StringLiteral("Char", "'", StringOptions.AllowsAllEscapes);
            StringLiteral StringLiteral = new StringLiteral("String", "\"", StringOptions.AllowsAllEscapes);
            //ME FALTA LOS DOUBLE

            CommentTerminal comment = new CommentTerminal("comment", "//", "\n", "\r");
            //comment must be added to NonGrammarTerminals list; it is not used directly in grammar rules,
            // so we add it to this list to let Scanner know that it is also a valid terminal. 
            NonGrammarTerminals.Add(comment);
            comment = new CommentTerminal("multilineComment", "/*", "*/");
            NonGrammarTerminals.Add(comment);

            this.RegisterOperators(1, Associativity.Left, mas, menos);

            this.RegisterOperators(2, Associativity.Left, por, division);

            NonTerminal
           S = new NonTerminal("S"),
           clase = new NonTerminal("clase"),
           Acceso = new NonTerminal("Acceso"),
           Herencia = new NonTerminal("Herencia"),
           Tipo = new NonTerminal("Tipo"),
           Tipo2 = new NonTerminal("Tipo2"),
           Instrucciones_Clase = new NonTerminal("Instrucciones_Clase"),
           Instrucciones_Clase2 = new NonTerminal("Instrucciones_Clase2"),
           Instrucciones_Clase3 = new NonTerminal("Instrucciones_Clase3"),
           Instrucciones_Clase4 = new NonTerminal("Instrucciones_Clase4"),
           Instrucciones_Clase5 = new NonTerminal("Instrucciones_Clase5"),
           constructor_Atrib = new NonTerminal("constructor_Atrib"),
           constructor_Atrib2 = new NonTerminal("constructor_Atrib2"),
           constructor_Atrib3 = new NonTerminal("constructor_Atrib3"),
           Parametros = new NonTerminal("Parametos"),
           Parametros2 = new NonTerminal("Parametos2"),
           Parametros3 = new NonTerminal("Parametos3"),
            Extension = new NonTerminal("Extension"),
             Exte2 = new NonTerminal("Exte2"),
             Extension2 = new NonTerminal("Extension"),
             Exte2_2 = new NonTerminal("Exte2"),
             Asigna = new NonTerminal("Asigna"),
              Atributos_Objetos = new NonTerminal("Atributos_Objetos"),
               Atributos_Objetos2 = new NonTerminal("Atributos_Objetos2"),
               Enviar_Parametros = new NonTerminal("Enviar_Parametros"),
               Enviar_Parametros2 = new NonTerminal("Enviar_Parametros2"),
               Enviar_Parametros3 = new NonTerminal("Enviar_Parametros3"),
               Enviar_Parametros4 = new NonTerminal("Enviar_Parametros4"),
               L_Sentencias = new NonTerminal("L_Sentencias"),
               Sentencias = new NonTerminal("Sentecias"),
               Variables = new NonTerminal("Variables"),
               Ver = new NonTerminal("Ver"),
               Ver2 = new NonTerminal("Ver2"),
               Op_Log = new NonTerminal("Op_Log"),
               Condicion = new NonTerminal("Condicion"),
               L_Not = new NonTerminal("L_Not"),
               L_Cond1 = new NonTerminal("L_Cond1"),
               L_Cond2 = new NonTerminal("L_Cond2"),
               L_Cond3 = new NonTerminal("L_Cond3"),
               L_Cond4 = new NonTerminal("L_Cond4"),
                L_Cond5 = new NonTerminal("L_Cond5"),
                Expresion_Return = new NonTerminal("Expresion_Return"),
                 Elses = new NonTerminal("Elses"),
                  Elses2 = new NonTerminal("Elses2"),
                   Elses3 = new NonTerminal("Elses3"),
                    Elses4 = new NonTerminal("Elses4"),
                   Declaracion_O_Asignacion = new NonTerminal("Declaracion_O_Asignacion"),
                   Asignacion_For = new NonTerminal("Asignacion_For"),
                   Asignacion_For2 = new NonTerminal("Asignacion_For2"),
                    Expresion_menos = new NonTerminal("Expresion_menos"),
               E = new NonTerminal("E"),
                T = new NonTerminal("T"),
                 R = new NonTerminal("R"),
                  W = new NonTerminal("W"),
                   F = new NonTerminal("F"),
                    Sobre_Metodo_Funcion = new NonTerminal("Sobre_Metodo_Funcion"),
              Switch2 = new NonTerminal("Switch2"),
              Switch3 = new NonTerminal("Switch3"),
           This2 = new NonTerminal("This2"),
            This3 = new NonTerminal("This3");


            Acceso.Rule = Public
                         | Private
                         | Protected
                         | Empty;

            Herencia.Rule = Extends + ID
                        | Empty;

            This2.Rule = This + punto;

            This3.Rule = This + punto
                        | Empty;
                        

            Tipo.Rule = Int
                       | Double
                       | String
                       | Char
                       | objeto
                       | Bool;

            Tipo2.Rule = Int
                      | Double
                      | String
                      | Char
                      | objeto
                      | Bool
                      | ID;

            Parametros.Rule = Tipo2 + ID + Parametros2
                             | Empty;

            Parametros2.Rule = Parametros2 + Parametros3
                                | Parametros3;

            Parametros3.Rule = coma + Tipo2 + ID
                               | Empty;

            E.Rule = E + mas + T
                    | T;

            T.Rule = T + menos + R
                    | R;

            R.Rule = R + por + W
                    | W;

            W.Rule = W + division + F
                    | F;

            F.Rule = num
                    | ID
                    | CharLiteral
                    | StringLiteral
                    | True
                    | False
                    | menos + Expresion_menos                  
                    | parentesisIzquierdo + E + parentesisDerecho // me falta el double y el new                    
                    | Nulo
                    | dobles
                    | ID + Atributos_Objetos + Enviar_Parametros  
                    |This2+ ID + Atributos_Objetos + Enviar_Parametros
                    | New + ID + Enviar_Parametros;

            Expresion_menos.Rule = num
                                  | dobles
                                  | parentesisIzquierdo + E + parentesisDerecho
                                  | ID;

            
            /////////////////////////////////////CONDICION//////////////////////////////////////////////////////////////////////////
            Condicion.Rule = Condicion + Op_Log + L_Cond1
                             |L_Cond1;

            L_Not.Rule = Not
                    | Empty;

            // L_Cond1.Rule = L_Not + L_Cond2;


            L_Cond1.Rule = Not + parentesisIzquierdo + Condicion + parentesisDerecho
                        | Not + E // + L_Cond5;
                        | E
                        | parentesisIzquierdo + Condicion + parentesisDerecho;


            


            //////////////////////////////////////////////////////////////////////////////////////////////////////////
            S.Rule = clase;
                
            clase.Rule = Acceso + Class + ID + Herencia + llaveIzquierda + Instrucciones_Clase + llaveDerecha;

            clase.ErrorRule = SyntaxError + llaveDerecha;//

            Instrucciones_Clase.Rule = Instrucciones_Clase + Instrucciones_Clase2
                                | Instrucciones_Clase2;


            Instrucciones_Clase2.Rule = Acceso + Instrucciones_Clase3 
                                       // | This + punto + ID + Atributos_Objetos + Enviar_Parametros + Asigna + PuntoyComa//Este es cuando venga el this.
                                       | Sobreescribir + Acceso +Sobre_Metodo_Funcion
                                        | Empty; // tengo que poner sobreescribir

            Sobre_Metodo_Funcion.Rule = Tipo + ID + parentesisIzquierdo + Parametros + parentesisDerecho + llaveIzquierda + L_Sentencias + llaveDerecha
                                       | Void + ID + parentesisIzquierdo + Parametros + parentesisDerecho + llaveIzquierda + L_Sentencias + llaveDerecha
                                       | ID + ID + parentesisIzquierdo + Parametros + parentesisDerecho + llaveIzquierda + L_Sentencias + llaveDerecha;

            Sobre_Metodo_Funcion.ErrorRule = SyntaxError + llaveDerecha;//
            Sobre_Metodo_Funcion.ErrorRule = SyntaxError + Acceso;//

            Instrucciones_Clase3.Rule =   Tipo + Instrucciones_Clase4 
                                        | Void + ID +parentesisIzquierdo +Parametros+ parentesisDerecho + llaveIzquierda  +L_Sentencias+ llaveDerecha //si sirve
                                        | constructor_Atrib;

            Instrucciones_Clase3.ErrorRule = SyntaxError + llaveDerecha;//
           // Instrucciones_Clase3.ErrorRule = SyntaxError + PuntoyComa;//

            Instrucciones_Clase4.Rule = ID + Instrucciones_Clase5; 

            Instrucciones_Clase5.Rule = Extension  + Asigna + PuntoyComa 
                                        |parentesisIzquierdo + Parametros + parentesisDerecho + llaveIzquierda + L_Sentencias + llaveDerecha;

            Instrucciones_Clase5.ErrorRule = SyntaxError + PuntoyComa;//
            Instrucciones_Clase5.ErrorRule = SyntaxError + llaveDerecha;//
            Instrucciones_Clase5.ErrorRule = SyntaxError + Acceso;//

            Extension.Rule = Extension + Exte2// esta es para las variables globales
                        | Exte2;

            Exte2.Rule = coma + ID
                         |Empty;

            Extension2.Rule = Extension2 + Exte2_2// estas es para las variables locales
                      | Exte2_2;

            Exte2_2.Rule = coma + ID
                         | Empty;


            Asigna.Rule = Asig  + E //Asig = Terminal , Asigna es el noterminal
                            | Empty;

            

            /////////////////////////////////////////////////////////////////////
            Atributos_Objetos.Rule = Atributos_Objetos + Atributos_Objetos2
                                    | Atributos_Objetos2;

            Atributos_Objetos2.Rule = punto + ID
                                    | Empty;

            /////////////////////////////////////////////////////////////////////////
            constructor_Atrib.Rule = ID + constructor_Atrib2;



            constructor_Atrib2.Rule = ID + constructor_Atrib3
                                    | parentesisIzquierdo + Parametros + parentesisDerecho + llaveIzquierda + L_Sentencias + llaveDerecha;
            // | Atributos_Objetos + Enviar_Parametros + Asigna + PuntoyComa;

            constructor_Atrib2.ErrorRule = SyntaxError + llaveDerecha;//

            constructor_Atrib3.Rule = Extension + Asigna + PuntoyComa
                                | parentesisIzquierdo + Parametros + parentesisDerecho + llaveIzquierda+ L_Sentencias + llaveDerecha;

            constructor_Atrib3.ErrorRule = SyntaxError + PuntoyComa;//
            constructor_Atrib3.ErrorRule = SyntaxError + llaveDerecha;//
            constructor_Atrib3.ErrorRule = SyntaxError + Acceso;//

            Enviar_Parametros.Rule = parentesisIzquierdo + Enviar_Parametros2 
                                    | Empty;

            Enviar_Parametros2.Rule =parentesisDerecho
                                     |E + Enviar_Parametros3 + parentesisDerecho; 

            Enviar_Parametros3.Rule = Enviar_Parametros3 + Enviar_Parametros4
                                        | Enviar_Parametros4;

            Enviar_Parametros4.Rule = coma + E
                                      | Empty;                     


            L_Sentencias.Rule = L_Sentencias + Sentencias
                                | Sentencias;


            Sentencias.Rule = If + parentesisIzquierdo+   Condicion  + parentesisDerecho + llaveIzquierda + L_Sentencias + llaveDerecha +Elses //8
                            | While + parentesisIzquierdo +    Condicion   + parentesisDerecho + llaveIzquierda + L_Sentencias + llaveDerecha //7
                            | For + parentesisIzquierdo +Declaracion_O_Asignacion + PuntoyComa +Condicion +PuntoyComa +Asignacion_For+ parentesisDerecho + llaveIzquierda + L_Sentencias + llaveDerecha //11
                            | Switch + parentesisIzquierdo + F + parentesisDerecho + llaveIzquierda + Case + F +DosPuntos  + L_Sentencias  +Switch2 + Default +DosPuntos +L_Sentencias+ llaveDerecha // 14 probar bien todas las sentencias
                            | Do + llaveIzquierda + L_Sentencias + llaveDerecha + While + parentesisIzquierdo+ Condicion + parentesisDerecho + PuntoyComa //9
                            | Break + PuntoyComa//2 ------------
                            | Continue + PuntoyComa//2 -------------------
                            | Return + Expresion_Return +PuntoyComa //3 ----------------------
                            | This2 + ID  + Ver + PuntoyComa //3 *con return
                            |ID + Ver2+PuntoyComa // 3 con return
                            | Variables + PuntoyComa //2 con break y continue
                            | Aumento + ID + PuntoyComa //3  con return
                            | Disminuye + ID + PuntoyComa //3 con return 
                            | Empty;

            Sentencias.ErrorRule = SyntaxError + llaveDerecha;//
            Sentencias.ErrorRule = SyntaxError + PuntoyComa;//

            Switch2.Rule = Switch2 + Switch3
                | Switch3;

            Switch3.Rule = Case + F +DosPuntos  + L_Sentencias
                | Empty;

            Variables.Rule = Tipo + ID + Extension2 + Asigna;

          //  Variables.ErrorRule = SyntaxError + PuntoyComa;//
           // Variables.ErrorRule = SyntaxError + llaveDerecha;//

           Ver.Rule = Aumento
                        | Disminuye
                        | Atributos_Objetos + Enviar_Parametros + Asigna ;

        //    Ver.ErrorRule = SyntaxError + PuntoyComa;//
          //  Ver.ErrorRule = SyntaxError + llaveDerecha;//

            Ver2.Rule = Aumento
                        | Disminuye
                        | ID + Extension2 + Asigna
                        | Atributos_Objetos + Enviar_Parametros + Asigna;

        //    Ver2.ErrorRule = SyntaxError + PuntoyComa;//
          //  Ver2.ErrorRule = SyntaxError + llaveDerecha;//

            Op_Log.Rule = And
                          | Or
                          | MayorQue
                          | MenosQue
                          | MenorIgual
                          | MayorIgual
                          | Igual
                          | Diferente;

            Expresion_Return.Rule = Condicion
                                | Empty;

           
            Elses.Rule = Else + Elses2
                        |Empty;

            Elses2.Rule = llaveIzquierda + L_Sentencias + llaveDerecha
                         | If + parentesisIzquierdo + Condicion + parentesisDerecho + llaveIzquierda + L_Sentencias + llaveDerecha + Elses;

            Elses2.ErrorRule = SyntaxError + llaveDerecha;
              
            Declaracion_O_Asignacion.Rule = Int + ID + Asigna
                                           | Double + ID + Asigna
                                           | ID + Asigna;

            Asignacion_For.Rule = Disminuye + ID
                                | Aumento + ID
                                | ID + Asignacion_For2;

            Asignacion_For2.Rule = Asigna
                                    | Aumento
                                    | Disminuye;

            this.Root = S;


        }


        }
    }
