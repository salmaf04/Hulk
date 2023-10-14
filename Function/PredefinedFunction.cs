class PredefinedFunction : Expression{
    public override string value {get;}

    public override SyntaxKind Kind {get;}
    List <Expression> parametros;

    public override string Evaluate()
    {
      switch(value){
      /*Si la funcion es "print", llamar a la funcion Print, 
      al tener un solo parametro, se le pasa parametros en la primera posicion*/
      case "print":
      if (parametros.Count > 1){
        throw new SemanticException("La funcion print espera un solo parametro");
      }
      return Print(parametros [0]);
      
      /*Si la funcion es "sin", llamar a la funcion Sin, 
      al tener un solo parametro, se le pasa parametros en la primera posicion*/
      case "sin":
      if (parametros.Count > 1){
        throw new SemanticException("La funcion sin espera un solo parametro");
      }
      return Sin(parametros[0]);
      /*Si la funcion es "cos", llamar a la funcion Cos, 
      al tener un solo parametro, se le pasa parametros en la primera posicion*/
      case "cos" :
      if (parametros.Count > 1){
        throw new SemanticException("La funcion cos espera un solo parametro");
      }
      return Cos(parametros[0]);
    /*Si la funcion es "tan", llamar a la funcion Tan, 
      al tener un solo parametro, se le pasa parametros en la primera posicion*/
      case "tan" :
      if (parametros.Count > 1){
        throw new SemanticException("La funcion tan espera un solo parametro");
      }
      return Tan(parametros[0]);
      /*Si la funcion es "log", llamar a la funcion Log, 
      al tener dos parametro, se le pasa parametros en la primera y segunda posicion*/
      case "log":
      if (parametros.Count < 2 || parametros.Count > 2){
        throw new SemanticException("La funcion log espera dos parametro");
      }
      return Log(parametros [1], parametros[0]);
      /*Si la funcion es "sqrt", llamar a la funcion Sqrt, 
      al tener un solo parametro, se le pasa parametros en la primera posicion*/
      case "sqrt":
      if (parametros.Count > 1){
        throw new SemanticException("La funcion sqrt espera un solo parametro");
      }
      return Sqrt(parametros[0]);
           }
           //En otro caso, lanzar una excepcion por error de sintaxis
           throw new SyntaxException ("This function doesn't exist");
    }
    //Lista de funciones predefinidas
   public static string[] PredefinedFunctions = {"print", "sin", "cos", "log", "sqrt", "tan"};

   public PredefinedFunction(string function, List <Expression> parametros){
    value = function;
    this.parametros = parametros;

    }

   
  //Funcion para imprimir
   public static string Print(Expression value){
      return value.Evaluate();
   }
  //Funcion para hallar raiz de un numero
   public static string Sqrt(Expression x){
    return Math.Sqrt(double.Parse(x.Evaluate())).ToString();
   }
  //Funcion para hallar logaritmo
   public static string Log(Expression basevalue, Expression x){
     return Math.Log(double.Parse(basevalue.Evaluate()), double.Parse(x.Evaluate())).ToString();
   }
  //Funcion para hallar seno de un numero
   public static string Sin(Expression x){
    return Math.Sin(double.Parse(x.Evaluate())).ToString();
   }
  //Funcion para hallar coseno de un numero
   public static string Cos (Expression x){
    return Math.Cos(double.Parse(x.Evaluate())).ToString();
   }
  //Funcion para hallar tangente de un numero
  public static string Tan (Expression x){
    return Math.Tan(double.Parse(x.Evaluate())).ToString();
  }
}